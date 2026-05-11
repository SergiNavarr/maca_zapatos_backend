using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Talles
{
    public class CrearTalleDto
    {
        public string Valor { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
    }
}
