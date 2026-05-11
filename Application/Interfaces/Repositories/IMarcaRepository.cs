using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IMarcaRepository
    {
        Task<IEnumerable<Marca>> ObtenerTodasAsync();
        Task<Marca?> ObtenerPorIdAsync(int id);
        Task<Marca?> ObtenerPorNombreAsync(string nombre);
        Task AgregarAsync(Marca marca);
        Task ActualizarAsync(Marca marca);
    }
}
