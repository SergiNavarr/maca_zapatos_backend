using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rol : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
