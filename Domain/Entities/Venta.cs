using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Venta : BaseEntity
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public decimal MontoTotal { get; set; }
        public MetodoPago MetodoPago { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
