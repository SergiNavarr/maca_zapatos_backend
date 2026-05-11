using Application.DTOs.Colores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<ColorDto>> ObtenerTodosAsync();
        Task<int> CrearColorAsync(CrearColorDto dto);
    }
}
