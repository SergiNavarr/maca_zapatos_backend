using Application.DTOs.Productos;
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

        [HttpGet]
        public async Task<IActionResult> GetProductosMaestros()
        {
            try
            {
                var productos = await _productoService.ObtenerProductosMaestrosAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpGet("pos")]
        public async Task<IActionResult> GetProductosParaVenta()
        {
            var productos = await _productoService.ObtenerProductosParaPOSAsync();
            return Ok(productos);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProductoCompleto([FromBody] CrearProductoCompletoDto dto)
        {
            try
            {
                var id = await _productoService.CrearProductoCompletoAsync(dto);
                return Ok(new { Mensaje = "Producto y stock inicial ingresados con éxito", ProductoId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoDetalle(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerProductoDetalleAsync(id);
                if (producto == null) return NotFound(new { Mensaje = "Producto no encontrado" });
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpGet("inventario")]
        public async Task<IActionResult> GetInventarioFisico()
        {
            try
            {
                var inventario = await _productoService.ObtenerInventarioFisicoAsync();
                return Ok(inventario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpPost("inventario/ajustar")]
        public async Task<IActionResult> AjustarStock([FromBody] AjustarStockDto dto)
        {
            try
            {
                await _productoService.AjustarStockAsync(dto);
                return Ok(new { Mensaje = "Stock ajustado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}