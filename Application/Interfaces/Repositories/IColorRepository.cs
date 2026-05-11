using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IColorRepository
    {
        Task<IEnumerable<Color>> ObtenerTodosAsync();
        Task<Color?> ObtenerPorIdAsync(int id);
        Task<Color?> ObtenerPorNombreAsync(string nombre);
        Task AgregarAsync(Color color);
        Task ActualizarAsync(Color color);
    }
}
