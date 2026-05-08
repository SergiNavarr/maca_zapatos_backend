using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        public string NombreUsuario { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<MovimientoStock> MovimientosStock { get; set; } = new List<MovimientoStock>();
        public ICollection<AuditoriaLog> AuditoriaLogs { get; set; } = new List<AuditoriaLog>();
    }
}
