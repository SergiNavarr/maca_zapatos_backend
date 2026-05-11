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
    public class MarcaRepository : IMarcaRepository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marca>> ObtenerTodasAsync()
        {
            return await _context.Marcas
                .Where(m => m.FechaEliminacion == null)
                .ToListAsync();
        }

        public async Task<Marca?> ObtenerPorIdAsync(int id)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id && m.FechaEliminacion == null);
        }

        public async Task<Marca?> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(m => m.Nombre.ToLower() == nombre.ToLower() && m.FechaEliminacion == null);
        }

        public async Task AgregarAsync(Marca marca)
        {
            await _context.Marcas.AddAsync(marca);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Marca marca)
        {
            _context.Marcas.Update(marca);
            await _context.SaveChangesAsync();
        }
    }
}
