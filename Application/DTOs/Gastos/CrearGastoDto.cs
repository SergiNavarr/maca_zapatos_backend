using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Gastos
{
    public class CrearGastoDto
    {
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
