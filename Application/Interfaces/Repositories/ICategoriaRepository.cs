using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ObtenerTodasAsync();
        Task<Categoria?> ObtenerPorIdAsync(int id);
        Task<Categoria?> ObtenerPorNombreAsync(string nombre);
        Task AgregarAsync(Categoria categoria);
        Task ActualizarAsync(Categoria categoria);
    }
}
