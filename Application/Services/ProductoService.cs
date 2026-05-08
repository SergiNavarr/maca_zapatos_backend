using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;

        // Inyectamos la interfaz, NO el DbContext
        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoParaVentaDto>> ObtenerProductosParaPOSAsync()
        {
            var variantes = await _repository.ObtenerVariantesConStockAsync();

            return variantes.Select(v => new ProductoParaVentaDto
            {
                VarianteId = v.Id,
                Nombre = v.Producto.Nombre,
                Marca = v.Producto.Marca.Nombre,
                Categoria = v.Producto.Categoria.Nombre,
                Talle = v.Talle.Valor,
                Color = v.Color.Nombre,
                Precio = v.Producto.PrecioBase,
                Stock = v.Stock,
                Imagen = v.Producto.ImagenUrl
            }).ToList();
        }
    }
}
