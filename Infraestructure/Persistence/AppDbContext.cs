using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<Color> Colores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<VarianteProducto> VariantesProducto { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Gasto> Gastos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configurar Postgres para que use JSONB en los logs de auditoría
            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.ValoresAnteriores)
                .HasColumnType("jsonb");

            modelBuilder.Entity<AuditoriaLog>()
                .Property(a => a.ValoresNuevos)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Gasto>()
                .Property(g => g.Monto)
                .HasPrecision(18, 2);

            // 2. Filtro Global para Soft Deletes
            // Usamos reflection y Expression Trees para aplicar el filtro a todas las clases que hereden de BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Creamos el parámetro dinámico: "e" (del tipo de la entidad actual)
                    var parameter = Expression.Parameter(entityType.ClrType, "e");

                    // Accedemos a la propiedad: e.FechaEliminacion
                    var property = Expression.Property(parameter, nameof(BaseEntity.FechaEliminacion));

                    // Creamos la comparación: e.FechaEliminacion == null
                    var condition = Expression.Equal(property, Expression.Constant(null, typeof(DateTime?)));

                    // Armamos la expresión lambda final: e => e.FechaEliminacion == null
                    var lambda = Expression.Lambda(condition, parameter);

                    // Aplicamos el filtro al modelo
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}