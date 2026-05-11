using Application.DTOs.Categorias;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync()
        {
            var categorias = await _categoriaRepository.ObtenerTodasAsync();
            return categorias.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            });
        }

        public async Task<int> CrearCategoriaAsync(CrearCategoriaDto dto)
        {
            // Regla de negocio: No repetir nombres
            var existe = await _categoriaRepository.ObtenerPorNombreAsync(dto.Nombre);
            if (existe != null)
            {
                throw new Exception($"La categoría '{dto.Nombre}' ya existe.");
            }

            var nuevaCategoria = new Categoria
            {
                Nombre = dto.Nombre,
                FechaCreacion = DateTime.UtcNow
            };

            await _categoriaRepository.AgregarAsync(nuevaCategoria);
            return nuevaCategoria.Id;
        }
    }
}
