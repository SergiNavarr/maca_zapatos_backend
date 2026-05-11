using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITalleRepository
    {
        Task<IEnumerable<Talle>> ObtenerTodosAsync();
        Task<IEnumerable<Talle>> ObtenerPorCategoriaAsync(int categoriaId);
        Task<Talle?> ObtenerPorIdAsync(int id);
        Task<Talle?> ObtenerPorValorYCategoriaAsync(string valor, int categoriaId);
        Task AgregarAsync(Talle talle);
        Task ActualizarAsync(Talle talle);
    }
}
