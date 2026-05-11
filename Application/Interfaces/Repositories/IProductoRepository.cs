using Domain.Entities;
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
    }
}
