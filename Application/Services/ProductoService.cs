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
    private readonly ITalleRepository _talleRepository;
    private readonly IColorRepository _colorRepository;
    private readonly ICurrentUserService _currentUserService;

    public ProductoService(
        IProductoRepository repository,
        ITalleRepository talleRepository,
        IColorRepository colorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _talleRepository = talleRepository;
        _colorRepository = colorRepository;
        _currentUserService = currentUserService;
    }

    // Normalización compartida: misma regla para el SkuBase persistido y para los SKU
    // de variante, de modo que las comparaciones de unicidad sean consistentes.
    private static string NormalizarSku(string valor) =>
        (valor ?? string.Empty).ToUpperInvariant().Replace(" ", "");

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
            throw new Exception("El producto debe tener al menos una variante.");

        // 1. Validar y normalizar el código base del producto.
        if (string.IsNullOrWhiteSpace(dto.SkuBase))
            throw new Exception("El código base (SkuBase) es obligatorio.");

        var skuBaseNormalizado = NormalizarSku(dto.SkuBase.Trim());
        if (skuBaseNormalizado.Length == 0)
            throw new Exception("El código base (SkuBase) es obligatorio.");

        // 2. Unicidad del código base: comparación exacta contra otros productos.
        if (await _repository.ExisteSkuBaseAsync(skuBaseNormalizado))
            throw new Exception($"El código base '{dto.SkuBase}' ya está en uso en otro producto.");

        // 3. Resolver talles y colores por ID (sin N+1: una sola búsqueda por ID distinto).
        var talles = new Dictionary<int, string>();
        foreach (var talleId in dto.Variantes.Select(v => v.TalleId).Distinct())
        {
            var talle = await _talleRepository.ObtenerPorIdAsync(talleId);
            if (talle == null)
                throw new Exception($"El talle con ID {talleId} no existe.");
            talles[talleId] = talle.Valor;
        }

        var colores = new Dictionary<int, string>();
        foreach (var colorId in dto.Variantes.Select(v => v.ColorId).Distinct())
        {
            var color = await _colorRepository.ObtenerPorIdAsync(colorId);
            if (color == null)
                throw new Exception($"El color con ID {colorId} no existe.");
            colores[colorId] = color.Nombre;
        }

        var usuarioActualId = _currentUserService.ObtenerUsuarioIdActual();

        var nuevoProducto = new Producto
        {
            CategoriaId = dto.CategoriaId,
            MarcaId = dto.MarcaId,
            Nombre = dto.Nombre,
            SkuBase = skuBaseNormalizado,
            Descripcion = dto.Descripcion,
            ImagenUrl = dto.ImagenUrl,
            PrecioBase = dto.PrecioBase,
            FechaActualizacion = DateTime.UtcNow,
            FechaCreacion = DateTime.UtcNow,
            Variantes = new List<VarianteProducto>()
        };

        // Detecta colisiones entre las propias variantes del payload (mismo talle+color).
        var skusGenerados = new HashSet<string>();

        foreach (var vDto in dto.Variantes)
        {
            // 4. Generar el SKU de la variante: {SkuBase}-{talle}-{color} en MAYÚSCULAS y sin espacios.
            var sku = NormalizarSku($"{skuBaseNormalizado}-{talles[vDto.TalleId]}-{colores[vDto.ColorId]}");

            if (!skusGenerados.Add(sku))
                throw new Exception($"Hay variantes repetidas que generan el mismo SKU '{sku}'. Revisá talle y color duplicados.");

            // 5. Salvaguarda: que el SKU generado no choque con uno ya existente en la base.
            if (await _repository.ExisteSKUAsync(sku))
                throw new Exception($"El código SKU '{sku}' ya se encuentra registrado.");

            var nuevaVariante = new VarianteProducto
            {
                TalleId = vDto.TalleId,
                ColorId = vDto.ColorId,
                SKU = sku,
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

    public async Task ActualizarProductoCompletoAsync(int id, ActualizarProductoDto dto)
    {
        // 1. Obtenemos el producto de la BD 
        var productoExistente = await _repository.ObtenerProductoDetalleAsync(id);
        if (productoExistente == null)
            throw new Exception($"No se encontró el producto con ID {id}.");

        // 2. Actualizamos los datos maestros
        productoExistente.CategoriaId = dto.CategoriaId;
        productoExistente.MarcaId = dto.MarcaId;
        productoExistente.Nombre = dto.Nombre;
        productoExistente.Descripcion = dto.Descripcion;
        productoExistente.ImagenUrl = dto.ImagenUrl;
        productoExistente.PrecioBase = dto.PrecioBase;
        productoExistente.FechaActualizacion = DateTime.UtcNow;

        // 3. Procesamos las Variantes
        var variantesDtoIds = dto.Variantes.Where(v => v.Id.HasValue && v.Id > 0).Select(v => v.Id.Value).ToList();

        // 3a. Eliminación lógica de variantes que el usuario sacó del frontend
        foreach (var varExistente in productoExistente.Variantes.Where(v => v.FechaEliminacion == null))
        {
            if (!variantesDtoIds.Contains(varExistente.Id))
            {
                varExistente.FechaEliminacion = DateTime.UtcNow;
            }
        }

        // 3b. Actualizar existentes y agregar nuevas
        foreach (var vDto in dto.Variantes)
        {
            if (vDto.Id.HasValue && vDto.Id.Value > 0)
            {
                // La variante ya existía, la buscamos y actualizamos
                var varExistente = productoExistente.Variantes.FirstOrDefault(v => v.Id == vDto.Id.Value);
                if (varExistente != null)
                {
                    // Validar si le cambiaron el SKU por uno que ya pertenece a otro zapato
                    if (varExistente.SKU != vDto.SKU && await _repository.ExisteSKUAsync(vDto.SKU))
                        throw new Exception($"El código SKU '{vDto.SKU}' ya está en uso en otro producto.");

                    varExistente.TalleId = vDto.TalleId;
                    varExistente.ColorId = vDto.ColorId;
                    varExistente.SKU = vDto.SKU;
                    varExistente.FechaActualizacion = DateTime.UtcNow;
                }
            }
            else
            {
                // Es una variante completamente nueva agregada en la edición
                if (await _repository.ExisteSKUAsync(vDto.SKU))
                    throw new Exception($"El código SKU '{vDto.SKU}' ya está en uso.");

                var nuevaVariante = new VarianteProducto
                {
                    TalleId = vDto.TalleId,
                    ColorId = vDto.ColorId,
                    SKU = vDto.SKU,
                    Stock = 0, // Las variantes nuevas nacen en 0. Para sumarle stock se usa "AjustarStock".
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                };
                productoExistente.Variantes.Add(nuevaVariante);
            }
        }

        // 4. Guardamos los cambios
        await _repository.ActualizarProductoAsync(productoExistente);
    }
    // -------------------------------------

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

    public async Task<IEnumerable<InventarioFisicoDto>> ObtenerInventarioFisicoAsync()
    {
        var variantes = await _repository.ObtenerInventarioFisicoAsync();

        return variantes.Select(v => new InventarioFisicoDto
        {
            VarianteId = v.Id,
            ProductoNombre = v.Producto.Nombre,
            Marca = v.Producto.Marca.Nombre,
            Categoria = v.Producto.Categoria.Nombre,
            SKU = v.SKU,
            Talle = v.Talle.Valor,
            Color = v.Color.Nombre,
            ColorHex = v.Color.CodigoHex,
            Stock = v.Stock
        }).OrderBy(v => v.ProductoNombre).ThenBy(v => v.Talle).ToList();
    }

    public async Task AjustarStockAsync(AjustarStockDto dto)
    {
        var usuarioId = _currentUserService.ObtenerUsuarioIdActual();
        await _repository.AjustarStockAsync(dto.VarianteId, dto.Cantidad, dto.TipoMovimiento, dto.Motivo, usuarioId > 0 ? usuarioId : null);
    }
}