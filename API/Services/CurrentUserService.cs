using Application.Interfaces;

namespace API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int? GetUserId()
        {
            return 1;
        }
    }
}
