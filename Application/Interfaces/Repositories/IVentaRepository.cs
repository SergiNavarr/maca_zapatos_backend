using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IVentaRepository
    {
        Task<Venta> RegistrarVentaAsync(Venta venta, List<MovimientoStock> movimientosStock);
        Task<List<VarianteProducto>> ObtenerVariantesParaActualizarAsync(IEnumerable<int> varianteIds);
    }
}
