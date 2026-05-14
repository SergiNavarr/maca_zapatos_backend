using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<VarianteProducto>> ObtenerVariantesConStockAsync();
        Task AgregarProductoConVariantesAsync(Producto producto);
        Task<bool> ExisteSKUAsync(string sku);
        Task<IEnumerable<Producto>> ObtenerProductosMaestrosAsync();
        Task<Producto?> ObtenerProductoDetalleAsync(int id);
        Task<IEnumerable<VarianteProducto>> ObtenerInventarioFisicoAsync();
        Task AjustarStockAsync(int varianteId, int cantidad, TipoMovimientoStock tipo, string motivo, int? usuarioId);
    }
}
