using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class ActualizarVarianteDto
    {
        public int? Id { get; set; }
        public int TalleId { get; set; }
        public int ColorId { get; set; }
        public string SKU { get; set; } = string.Empty;
    }
}
