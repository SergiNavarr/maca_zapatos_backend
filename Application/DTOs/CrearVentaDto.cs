using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CrearVentaDto
    {
        public int MetodoPago { get; set; }

        public decimal MontoTotal { get; set; }

        public List<CrearDetalleVentaDto> Detalles { get; set; } = new();
    }
}
