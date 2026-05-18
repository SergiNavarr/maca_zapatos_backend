using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadsController : ControllerBase
    {
        private readonly IImagenService _imagenService;

        public UploadsController(IImagenService imagenService)
        {
            _imagenService = imagenService;
        }

        [HttpPost]
        public async Task<IActionResult> SubirImagen(IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return BadRequest(new { Mensaje = "No se ha enviado ningún archivo o el archivo está vacío." });

                // Validamos la extensión del archivo del lado de la API web
                var extension = Path.GetExtension(archivo.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".webp")
                    return BadRequest(new { Mensaje = "Formato de imagen no permitido. Usa JPG, PNG o WEBP." });

                // Abrimos el Stream dentro de un bloque 'using' para asegurar que se libere la memoria al terminar
                using var stream = archivo.OpenReadStream();

                // Enviamos el Stream puro y limpio a la capa de negocio
                var urlImagen = await _imagenService.SubirImagenAsync(stream, archivo.FileName);

                return Ok(new { Url = urlImagen });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = $"Error al procesar la imagen: {ex.Message}" });
            }
        }
    }
}