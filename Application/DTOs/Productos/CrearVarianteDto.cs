using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class CrearVarianteDto
    {
        // El SKU ya no se recibe del frontend: lo genera el backend a partir del SkuBase
        // del producto + el talle y el color de esta variante.
        public int TalleId { get; set; }
        public int ColorId { get; set; }
        public int StockInicial { get; set; }
    }
}
