using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductoParaVentaDto
    {
        public int VarianteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Talle { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; } 
    }
}
