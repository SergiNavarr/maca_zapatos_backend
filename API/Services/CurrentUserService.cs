using Application.Interfaces;
using System.Security.Claims;

namespace API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int ObtenerUsuarioIdActual()
        {
            // Buscamos el "NameIdentifier" dentro del Token JWT que envió React
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId; 
            }

            // Si por alguna razón no hay token, devolvemos 0 o tiramos error
            return 0;
        }
    }
}
