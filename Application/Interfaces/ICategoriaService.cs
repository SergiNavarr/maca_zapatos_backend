using Application.DTOs.Categorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync();
        Task<int> CrearCategoriaAsync(CrearCategoriaDto dto);
    }
}
