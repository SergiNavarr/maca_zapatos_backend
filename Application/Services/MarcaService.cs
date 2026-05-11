using Application.DTOs.Marcas;
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
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaRepository _marcaRepository;

        public MarcaService(IMarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<IEnumerable<MarcaDto>> ObtenerTodasAsync()
        {
            var marcas = await _marcaRepository.ObtenerTodasAsync();
            return marcas.Select(m => new MarcaDto
            {
                Id = m.Id,
                Nombre = m.Nombre
            });
        }

        public async Task<int> CrearMarcaAsync(CrearMarcaDto dto)
        {
            var existe = await _marcaRepository.ObtenerPorNombreAsync(dto.Nombre);
            if (existe != null)
            {
                throw new Exception($"La marca '{dto.Nombre}' ya existe.");
            }

            var nuevaMarca = new Marca
            {
                Nombre = dto.Nombre,
                FechaCreacion = DateTime.UtcNow
            };

            await _marcaRepository.AgregarAsync(nuevaMarca);
            return nuevaMarca.Id;
        }
    }
}
