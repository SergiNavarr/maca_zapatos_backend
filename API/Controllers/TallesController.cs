using Application.DTOs.Talles;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TallesController : ControllerBase
    {
        private readonly ITalleService _talleService;

        public TallesController(ITalleService talleService)
        {
            _talleService = talleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTalles()
        {
            var talles = await _talleService.ObtenerTodosAsync();
            return Ok(talles);
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> GetTallesPorCategoria(int categoriaId)
        {
            var talles = await _talleService.ObtenerPorCategoriaAsync(categoriaId);
            return Ok(talles);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTalle([FromBody] CrearTalleDto dto)
        {
            try
            {
                var id = await _talleService.CrearTalleAsync(dto);
                return Ok(new { Mensaje = "Talle creado", TalleId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
