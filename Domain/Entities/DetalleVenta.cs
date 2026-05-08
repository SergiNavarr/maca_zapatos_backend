using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DetalleVenta : BaseEntity
    {
        public int VentaId { get; set; }
        public Venta Venta { get; set; } = null!;

        public int VarianteProductoId { get; set; }
        public VarianteProducto VarianteProducto { get; set; } = null!;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
