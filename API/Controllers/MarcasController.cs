using Application.DTOs.Marcas;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly IMarcaService _marcaService;

        public MarcasController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMarcas()
        {
            var marcas = await _marcaService.ObtenerTodasAsync();
            return Ok(marcas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearMarca([FromBody] CrearMarcaDto dto)
        {
            try
            {
                var id = await _marcaService.CrearMarcaAsync(dto);
                return Ok(new { Mensaje = "Marca creada", MarcaId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
