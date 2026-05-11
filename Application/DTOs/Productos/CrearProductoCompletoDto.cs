using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class CrearProductoCompletoDto
    {
        public int CategoriaId { get; set; }
        public int MarcaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal PrecioBase { get; set; }

        // La lista de variantes que se crearán junto con este producto maestro
        public List<CrearVarianteDto> Variantes { get; set; } = new();
    }
}
