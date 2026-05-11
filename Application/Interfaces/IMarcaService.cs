using Application.DTOs.Marcas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMarcaService
    {
        Task<IEnumerable<MarcaDto>> ObtenerTodasAsync();
        Task<int> CrearMarcaAsync(CrearMarcaDto dto);
    }
}
