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
    public class TalleRepository : ITalleRepository
    {
        private readonly AppDbContext _context;

        public TalleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Talle>> ObtenerTodosAsync()
        {
            return await _context.Talles
                .Include(t => t.Categoria)
                .Where(t => t.FechaEliminacion == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Talle>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            return await _context.Talles
                .Where(t => t.CategoriaId == categoriaId && t.FechaEliminacion == null)
                .ToListAsync();
        }

        public async Task<Talle?> ObtenerPorIdAsync(int id)
        {
            return await _context.Talles
                .FirstOrDefaultAsync(t => t.Id == id && t.FechaEliminacion == null);
        }

        public async Task<Talle?> ObtenerPorValorYCategoriaAsync(string valor, int categoriaId)
        {
            return await _context.Talles
                .FirstOrDefaultAsync(t =>
                    t.Valor.ToLower() == valor.ToLower() &&
                    t.CategoriaId == categoriaId &&
                    t.FechaEliminacion == null);
        }

        public async Task AgregarAsync(Talle talle)
        {
            await _context.Talles.AddAsync(talle);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Talle talle)
        {
            _context.Talles.Update(talle);
            await _context.SaveChangesAsync();
        }
    }
}
