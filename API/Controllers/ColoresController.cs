using Application.DTOs.Colores;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ColoresController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColoresController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetColores()
        {
            var colores = await _colorService.ObtenerTodosAsync();
            return Ok(colores);
        }

        [HttpPost]
        public async Task<IActionResult> CrearColor([FromBody] CrearColorDto dto)
        {
            try
            {
                var id = await _colorService.CrearColorAsync(dto);
                return Ok(new { Mensaje = "Color creado", ColorId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
