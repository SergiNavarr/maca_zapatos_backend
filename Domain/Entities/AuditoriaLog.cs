using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuditoriaLog
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } = null!;

        public string EntidadAfectada { get; set; } = string.Empty;
        public int EntidadId { get; set; }
        public AccionAuditoria Accion { get; set; }

        public string? ValoresAnteriores { get; set; }
        public string? ValoresNuevos { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    }
}
