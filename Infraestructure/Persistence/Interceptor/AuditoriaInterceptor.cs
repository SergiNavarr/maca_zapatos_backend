using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Persistence.Interceptor
{
    public class AuditoriaInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;

        public AuditoriaInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var userId = _currentUserService.ObtenerUsuarioIdActual();
            var auditLogs = new List<AuditoriaLog>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // 1. Lógica Automática de Soft Delete
                if (entry.Entity is BaseEntity baseEntity && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    baseEntity.FechaEliminacion = DateTime.UtcNow;
                }

                // Ignoramos los logs de auditoría y evitamos loguear entidades sin cambios
                if (entry.Entity is AuditoriaLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                // Si el usuario es 0 (no hay token válido), frenamos la transacción por seguridad.
                if (userId == 0)
                {
                    throw new UnauthorizedAccessException("Se detectó un intento de modificación en la base de datos sin un usuario autenticado válido.");
                }

                // 2. Creación del Log de Auditoría
                var auditLog = new AuditoriaLog
                {
                    UsuarioId = userId,
                    EntidadAfectada = entry.Entity.GetType().Name,
                    FechaHora = DateTime.UtcNow,
                    Accion = entry.State switch
                    {
                        EntityState.Added => AccionAuditoria.Create,
                        EntityState.Modified => ((BaseEntity)entry.Entity).FechaEliminacion != null ? AccionAuditoria.SoftDelete : AccionAuditoria.Update,
                        _ => AccionAuditoria.Update
                    }
                };

                // Intentamos capturar el Id de la entidad 
                if (entry.Properties.Any(p => p.Metadata.Name == "Id"))
                {
                    var idProp = entry.Property("Id");
                    if (idProp.CurrentValue != null) auditLog.EntidadId = (int)idProp.CurrentValue;
                }

                // Valores Anteriores
                if (entry.State == EntityState.Modified)
                {
                    var oldValues = entry.Properties.Where(p => p.IsModified).ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);
                    auditLog.ValoresAnteriores = JsonSerializer.Serialize(oldValues);
                }

                // Valores Nuevos
                var newValues = entry.Properties.Where(p => entry.State == EntityState.Added || p.IsModified).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
                auditLog.ValoresNuevos = JsonSerializer.Serialize(newValues);

                auditLogs.Add(auditLog);
            }

            if (auditLogs.Any())
            {
                context.Set<AuditoriaLog>().AddRange(auditLogs);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}