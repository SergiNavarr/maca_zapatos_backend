using Application.DTOs;
using Application.DTOs.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoParaVentaDto>> ObtenerProductosParaPOSAsync();
        Task<int> CrearProductoCompletoAsync(CrearProductoCompletoDto dto);
        Task<IEnumerable<ProductoMaestroDto>> ObtenerProductosMaestrosAsync();
        Task<ProductoDetalleDto?> ObtenerProductoDetalleAsync(int id);
    }
}
