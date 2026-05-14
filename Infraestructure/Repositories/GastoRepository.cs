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
    public class GastoRepository : IGastoRepository
    {
        private readonly AppDbContext _context;

        public GastoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastosDelMesAsync(int anio, int mes)
        {
            return await _context.Gastos
                .Include(g => g.Usuario)
                .Where(g => g.FechaCreacion.Year == anio &&
                            g.FechaCreacion.Month == mes &&
                            g.FechaEliminacion == null)
                .OrderByDescending(g => g.FechaCreacion)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Gasto> CrearGastoAsync(Gasto gasto)
        {
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();
            return gasto;
        }
    }
}
