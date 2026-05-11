using Application.DTOs.Categorias;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();
            return Ok(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaDto dto)
        {
            try
            {
                var id = await _categoriaService.CrearCategoriaAsync(dto);
                return Ok(new { Mensaje = "Categoría creada", CategoriaId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
