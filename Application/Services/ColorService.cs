using Application.DTOs.Colores;
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
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public async Task<IEnumerable<ColorDto>> ObtenerTodosAsync()
        {
            var colores = await _colorRepository.ObtenerTodosAsync();
            return colores.Select(c => new ColorDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                CodigoHex = c.CodigoHex
            });
        }

        public async Task<int> CrearColorAsync(CrearColorDto dto)
        {
            var existe = await _colorRepository.ObtenerPorNombreAsync(dto.Nombre);
            if (existe != null)
            {
                throw new Exception($"El color '{dto.Nombre}' ya existe.");
            }

            var nuevoColor = new Color
            {
                Nombre = dto.Nombre,
                CodigoHex = dto.CodigoHex,
                FechaCreacion = DateTime.UtcNow
            };

            await _colorRepository.AgregarAsync(nuevoColor);
            return nuevoColor.Id;
        }
    }
}
