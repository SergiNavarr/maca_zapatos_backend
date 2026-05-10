using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet("pos")]
        public async Task<IActionResult> GetProductosParaVenta()
        {
            var productos = await _productoService.ObtenerProductosParaPOSAsync();
            return Ok(productos);
        }
    }
}
