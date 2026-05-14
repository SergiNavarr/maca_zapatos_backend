using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGastoRepository
    {
        Task<IEnumerable<Gasto>> ObtenerGastosDelMesAsync(int anio, int mes);
        Task<Gasto> CrearGastoAsync(Gasto gasto);
    }
}
