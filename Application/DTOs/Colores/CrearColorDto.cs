using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Colores
{
    public class CrearColorDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? CodigoHex { get; set; }
    }
}
