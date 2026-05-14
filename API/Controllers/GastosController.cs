using Application.DTOs.Gastos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IGastoService _gastoService;

        public GastosController(IGastoService gastoService)
        {
            _gastoService = gastoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGastos([FromQuery] int mes, [FromQuery] int anio)
        {
            // Si no mandan fecha, asumimos el mes actual
            if (mes == 0) mes = DateTime.UtcNow.Month;
            if (anio == 0) anio = DateTime.UtcNow.Year;

            var gastos = await _gastoService.ObtenerGastosDelMesAsync(anio, mes);
            return Ok(gastos);
        }

        [HttpPost]
        public async Task<IActionResult> CrearGasto([FromBody] CrearGastoDto dto)
        {
            try
            {
                var id = await _gastoService.CrearGastoAsync(dto);
                return Ok(new { Mensaje = "Gasto registrado exitosamente", GastoId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
