using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VarianteProducto : BaseEntity
    {
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        public int TalleId { get; set; }
        public Talle Talle { get; set; } = null!;

        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;

        public string SKU { get; set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<MovimientoStock> Movimientos { get; set; } = new List<MovimientoStock>();
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}
