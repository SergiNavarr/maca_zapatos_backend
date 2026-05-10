using Application.DTOs;
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
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<int> CrearUsuarioAsync(CrearUsuarioDto dto)
        {
            // 1. Verificamos que el nombre de usuario no esté tomado
            var usuarioExistente = await _usuarioRepository.ObtenerPorNombreUsuarioAsync(dto.NombreUsuario);
            if (usuarioExistente != null)
            {
                throw new Exception("El nombre de usuario ya está en uso.");
            }

            // 2. Creamos la entidad y aplicamos BCrypt a la contraseña
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = dto.RolId,
                FechaCreacion = DateTime.UtcNow
            };

            // 3. Guardamos en la base de datos
            await _usuarioRepository.AgregarAsync(nuevoUsuario);

            return nuevoUsuario.Id;
        }
    }
}
