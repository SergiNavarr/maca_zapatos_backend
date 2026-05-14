using Application.DTOs.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGastoService
    {
        Task<IEnumerable<GastoDto>> ObtenerGastosDelMesAsync(int anio, int mes);
        Task<int> CrearGastoAsync(CrearGastoDto dto);
    }
}
