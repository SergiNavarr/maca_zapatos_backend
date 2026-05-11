using Application.Interfaces.Repositories;
using Domain.Entities;
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
    }
}
