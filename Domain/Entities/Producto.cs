using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Producto : BaseEntity
    {
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public int MarcaId { get; set; }
        public Marca Marca { get; set; } = null!;

        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal PrecioBase { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<VarianteProducto> Variantes { get; set; } = new List<VarianteProducto>();
    }
}
