using Application.Interfaces.Repositories;
using Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly AppDbContext _context;

        public ColorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> ObtenerTodosAsync()
        {
            return await _context.Colores
                .Where(c => c.FechaEliminacion == null)
                .ToListAsync();
        }

        public async Task<Color?> ObtenerPorIdAsync(int id)
        {
            return await _context.Colores
                .FirstOrDefaultAsync(c => c.Id == id && c.FechaEliminacion == null);
        }

        public async Task<Color?> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.Colores
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower() && c.FechaEliminacion == null);
        }

        public async Task AgregarAsync(Color color)
        {
            await _context.Colores.AddAsync(color);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Color color)
        {
            _context.Colores.Update(color);
            await _context.SaveChangesAsync();
        }
    }
}
