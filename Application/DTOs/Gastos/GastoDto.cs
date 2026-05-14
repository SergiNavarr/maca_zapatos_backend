using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Gastos
{
    public class GastoDto
    {
        public int Id { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
