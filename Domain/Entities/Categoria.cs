using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Talle> Talles { get; set; } = new List<Talle>();
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
