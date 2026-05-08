using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Color : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string? CodigoHex { get; set; }

        public ICollection<VarianteProducto> Variantes { get; set; } = new List<VarianteProducto>();
    }
}
