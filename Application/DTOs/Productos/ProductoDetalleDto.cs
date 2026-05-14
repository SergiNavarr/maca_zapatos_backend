using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class ProductoDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public string? ImagenUrl { get; set; }
        public List<VarianteDetalleDto> Variantes { get; set; } = new();
    }
}
