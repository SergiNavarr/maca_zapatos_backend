using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MovimientoStock
    {
        public int Id { get; set; }

        public int VarianteProductoId { get; set; }
        public VarianteProducto VarianteProducto { get; set; } = null!;

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } = null!;

        public TipoMovimientoStock TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    }
}
