using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class InventarioFisicoDto
    {
        public int VarianteId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Talle { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
