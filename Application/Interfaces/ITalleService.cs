using Application.DTOs.Talles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITalleService
    {
        Task<IEnumerable<TalleDto>> ObtenerTodosAsync();
        Task<IEnumerable<TalleDto>> ObtenerPorCategoriaAsync(int categoriaId);
        Task<int> CrearTalleAsync(CrearTalleDto dto);
    }
}
