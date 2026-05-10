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
    public class VentaRepository : IVentaRepository
    {
        private readonly AppDbContext _context;

        public VentaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Venta> RegistrarVentaAsync(Venta venta, List<MovimientoStock> movimientosStock)
        {
            // 1. Iniciamos una transacción explícita por seguridad extrema
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. Agregamos la Venta
                await _context.Ventas.AddAsync(venta);

                // 3. Agregamos el historial de movimientos
                await _context.MovimientosStock.AddRangeAsync(movimientosStock);

                // 4. Guardamos todos los cambios en la base de datos de un solo golpe
                await _context.SaveChangesAsync();

                // 5. Confirmamos la transacción (Si llegamos aquí, nada falló)
                await transaction.CommitAsync();

                return venta;
            }
            catch (Exception)
            {
                // Si algo falla deshacemos TODO. No se guarda ni la venta ni el descuento de stock.
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<VarianteProducto>> ObtenerVariantesParaActualizarAsync(IEnumerable<int> varianteIds)
        {
            return await _context.VariantesProducto
                .Where(v => varianteIds.Contains(v.Id))
                .ToListAsync();
        }
    }
}
