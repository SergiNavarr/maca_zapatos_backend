using Application.DTOs.Gastos;
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
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public GastoService(IGastoRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<GastoDto>> ObtenerGastosDelMesAsync(int anio, int mes)
        {
            var gastos = await _repository.ObtenerGastosDelMesAsync(anio, mes);

            return gastos.Select(g => new GastoDto
            {
                Id = g.Id,
                Concepto = g.Concepto,
                Monto = g.Monto,
                FechaCreacion = g.FechaCreacion,
                NombreUsuario = g.Usuario?.NombreUsuario ?? "Desconocido"
            }).ToList();
        }

        public async Task<int> CrearGastoAsync(CrearGastoDto dto)
        {
            // Extraemos el ID del usuario que está logueado haciendo la petición
            var usuarioId = _currentUserService.ObtenerUsuarioIdActual();

            var gasto = new Gasto
            {
                Concepto = dto.Concepto,
                Monto = dto.Monto,
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.UtcNow
            };

            var nuevoGasto = await _repository.CrearGastoAsync(gasto);

            return nuevoGasto.Id;
        }
    }
}
