using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class VarianteDetalleDto
    {
        public int Id { get; set; }
        public string Talle { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
