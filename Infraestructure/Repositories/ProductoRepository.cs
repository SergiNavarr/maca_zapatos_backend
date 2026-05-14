using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VarianteProducto>> ObtenerVariantesConStockAsync()
        {
            return await _context.VariantesProducto
                .Include(v => v.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(v => v.Producto)
                    .ThenInclude(p => p.Marca)
                .Include(v => v.Talle)
                .Include(v => v.Color)
                .AsNoTracking()
                .Where(v => v.Stock > 0 && v.FechaEliminacion == null)
                .ToListAsync();
        }

        public async Task AgregarProductoConVariantesAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteSKUAsync(string sku)
        {
            return await _context.VariantesProducto
                .AnyAsync(v => v.SKU.ToLower() == sku.ToLower() && v.FechaEliminacion == null);
        }
        public async Task<IEnumerable<Producto>> ObtenerProductosMaestrosAsync()
        {
            return await _context.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .Where(p => p.FechaEliminacion == null)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Producto?> ObtenerProductoDetalleAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .Include(p => p.Variantes)
                    .ThenInclude(v => v.Talle)
                .Include(p => p.Variantes)
                    .ThenInclude(v => v.Color)
                .FirstOrDefaultAsync(p => p.Id == id && p.FechaEliminacion == null);
        }

        public async Task<IEnumerable<VarianteProducto>> ObtenerInventarioFisicoAsync()
        {
            return await _context.VariantesProducto
                .Include(v => v.Producto)
                    .ThenInclude(p => p.Marca)
                .Include(v => v.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(v => v.Talle)
                .Include(v => v.Color)
                .Where(v => v.FechaEliminacion == null && v.Producto.FechaEliminacion == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AjustarStockAsync(int varianteId, int cantidad, TipoMovimientoStock tipo, string motivo, int? usuarioId)
        {
            var variante = await _context.VariantesProducto.FindAsync(varianteId);

            if (variante == null)
                throw new Exception("La variante no existe.");

            if (cantidad <= 0)
                throw new Exception("La cantidad a ajustar debe ser mayor a cero.");

            if (tipo == TipoMovimientoStock.Entrada)
            {
                variante.Stock += cantidad;
            }
            else if (tipo == TipoMovimientoStock.Salida)
            {
                if (variante.Stock < cantidad)
                    throw new Exception($"Stock insuficiente. Hay {variante.Stock} disponibles y se intentan sacar {cantidad}.");

                variante.Stock -= cantidad;
            }

            var movimiento = new MovimientoStock
            {
                VarianteProductoId = varianteId,
                UsuarioId = usuarioId,
                TipoMovimiento = tipo,
                Cantidad = cantidad,
                Motivo = motivo,
                FechaHora = DateTime.UtcNow
            };

            _context.Set<MovimientoStock>().Add(movimiento);

            await _context.SaveChangesAsync();
        }
    }
}
