using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly ICurrentUserService _currentUserService;

        public VentaService(IVentaRepository ventaRepository, ICurrentUserService currentUserService)
        {
            _ventaRepository = ventaRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> ProcesarVentaAsync(CrearVentaDto dto)
        {
            // 1. Obtenemos quién es el vendedor (Por ahora siempre es el 1, luego será con JWT)
            var usuarioId = _currentUserService.GetUserId()
                ?? throw new UnauthorizedAccessException("Usuario no autenticado.");

            // 2. Buscamos en la BD los productos que nos mandó el carrito
            var varianteIds = dto.Detalles.Select(d => d.VarianteId).ToList();
            var variantesEnDb = await _ventaRepository.ObtenerVariantesParaActualizarAsync(varianteIds);

            // 3. Preparamos las listas para la transacción
            var detallesVenta = new List<DetalleVenta>();
            var movimientosStock = new List<MovimientoStock>();

            // 4. Validamos stock y armamos los detalles
            foreach (var itemCarrito in dto.Detalles)
            {
                var variante = variantesEnDb.FirstOrDefault(v => v.Id == itemCarrito.VarianteId);

                if (variante == null)
                    throw new Exception($"El producto con ID {itemCarrito.VarianteId} no existe.");

                if (variante.Stock < itemCarrito.Cantidad)
                    throw new Exception($"Stock insuficiente para {variante.SKU}. Solicitado: {itemCarrito.Cantidad}, Disponible: {variante.Stock}");

                // Descontamos el stock en memoria
                variante.Stock -= itemCarrito.Cantidad;

                // Creamos el detalle del ticket
                detallesVenta.Add(new DetalleVenta
                {
                    VarianteProductoId = variante.Id,
                    Cantidad = itemCarrito.Cantidad,
                    PrecioUnitario = itemCarrito.PrecioUnitario
                });

                // Creamos el registro histórico de salida de mercadería
                movimientosStock.Add(new MovimientoStock
                {
                    VarianteProductoId = variante.Id,
                    UsuarioId = usuarioId,
                    TipoMovimiento = TipoMovimientoStock.Salida,
                    Cantidad = itemCarrito.Cantidad,
                    Motivo = "Venta en POS",
                    FechaHora = DateTime.UtcNow
                });
            }

            // 5. Armamos la Venta principal
            var nuevaVenta = new Venta
            {
                UsuarioId = usuarioId,
                MontoTotal = dto.MontoTotal,
                MetodoPago = (MetodoPago)dto.MetodoPago,
                Detalles = detallesVenta
            };

            var ventaGuardada = await _ventaRepository.RegistrarVentaAsync(nuevaVenta, movimientosStock);

            return ventaGuardada.Id;
        }
    }
}
