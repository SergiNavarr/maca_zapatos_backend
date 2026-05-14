using Application.DTOs;
using Application.DTOs.Productos;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ProductoService(IProductoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
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

    public async Task<int> CrearProductoCompletoAsync(CrearProductoCompletoDto dto)
    {
        if (!dto.Variantes.Any())
        {
            throw new Exception("El producto debe tener al menos una variante.");
        }

        foreach (var variante in dto.Variantes)
        {
            if (await _repository.ExisteSKUAsync(variante.SKU))
            {
                throw new Exception($"El código SKU '{variante.SKU}' ya se encuentra registrado.");
            }
        }

        // 2. Obtenemos el ID del empleado que está haciendo la carga
        var usuarioActualId = _currentUserService.ObtenerUsuarioIdActual();

        var nuevoProducto = new Producto
        {
            CategoriaId = dto.CategoriaId,
            MarcaId = dto.MarcaId,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            ImagenUrl = dto.ImagenUrl,
            PrecioBase = dto.PrecioBase,
            FechaActualizacion = DateTime.UtcNow,
            FechaCreacion = DateTime.UtcNow,
            Variantes = new List<VarianteProducto>()
        };

        foreach (var vDto in dto.Variantes)
        {
            var nuevaVariante = new VarianteProducto
            {
                TalleId = vDto.TalleId,
                ColorId = vDto.ColorId,
                SKU = vDto.SKU,
                Stock = vDto.StockInicial,
                FechaActualizacion = DateTime.UtcNow,
                FechaCreacion = DateTime.UtcNow,
                Movimientos = new List<MovimientoStock>()
            };

            if (vDto.StockInicial > 0)
            {
                nuevaVariante.Movimientos.Add(new MovimientoStock
                {
                    UsuarioId = usuarioActualId,
                    TipoMovimiento = TipoMovimientoStock.Entrada,
                    Cantidad = vDto.StockInicial,
                    Motivo = "Ingreso de stock inicial por alta de producto",
                    FechaHora = DateTime.UtcNow
                });
            }

            nuevoProducto.Variantes.Add(nuevaVariante);
        }

        await _repository.AgregarProductoConVariantesAsync(nuevoProducto);

        return nuevoProducto.Id;
    }

    public async Task<IEnumerable<ProductoMaestroDto>> ObtenerProductosMaestrosAsync()
    {
        var productos = await _repository.ObtenerProductosMaestrosAsync();

        return productos.Select(p => new ProductoMaestroDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Marca = p.Marca.Nombre,
            Categoria = p.Categoria.Nombre,
            PrecioBase = p.PrecioBase,
            ImagenUrl = p.ImagenUrl
        }).OrderByDescending(p => p.Id).ToList();
    }

    public async Task<ProductoDetalleDto?> ObtenerProductoDetalleAsync(int id)
    {
        var p = await _repository.ObtenerProductoDetalleAsync(id);

        if (p == null) return null;

        return new ProductoDetalleDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion ?? "",
            Marca = p.Marca.Nombre,
            Categoria = p.Categoria.Nombre,
            PrecioBase = p.PrecioBase,
            ImagenUrl = p.ImagenUrl,
            Variantes = p.Variantes
                .Where(v => v.FechaEliminacion == null)
                .Select(v => new VarianteDetalleDto
                {
                    Id = v.Id,
                    Talle = v.Talle.Valor,
                    Color = v.Color.Nombre,
                    ColorHex = v.Color.CodigoHex,
                    SKU = v.SKU,
                    Stock = v.Stock
                }).ToList()
        };
    }
}