using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Talle : BaseEntity
    {
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public string Valor { get; set; } = string.Empty;

        public ICollection<VarianteProducto> Variantes { get; set; } = new List<VarianteProducto>();
    }
}
