using Application.DTOs.Talles;
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
    public class TalleService : ITalleService
    {
        private readonly ITalleRepository _talleRepository;

        public TalleService(ITalleRepository talleRepository)
        {
            _talleRepository = talleRepository;
        }

        public async Task<IEnumerable<TalleDto>> ObtenerTodosAsync()
        {
            var talles = await _talleRepository.ObtenerTodosAsync();
            return talles.Select(t => new TalleDto
            {
                Id = t.Id,
                Valor = t.Valor,
                CategoriaId = t.CategoriaId,
                CategoriaNombre = t.Categoria?.Nombre ?? "Sin Categoría"
            });
        }

        public async Task<IEnumerable<TalleDto>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            var talles = await _talleRepository.ObtenerPorCategoriaAsync(categoriaId);
            return talles.Select(t => new TalleDto
            {
                Id = t.Id,
                Valor = t.Valor,
                CategoriaId = t.CategoriaId
            });
        }

        public async Task<int> CrearTalleAsync(CrearTalleDto dto)
        {
            var existe = await _talleRepository.ObtenerPorValorYCategoriaAsync(dto.Valor, dto.CategoriaId);
            if (existe != null)
            {
                throw new Exception($"El talle '{dto.Valor}' ya existe para esta categoría.");
            }

            var nuevoTalle = new Talle
            {
                Valor = dto.Valor,
                CategoriaId = dto.CategoriaId,
                FechaCreacion = DateTime.UtcNow
            };

            await _talleRepository.AgregarAsync(nuevoTalle);
            return nuevoTalle.Id;
        }
    }
}
