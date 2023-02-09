using System.Security.Claims;

namespace iHome.Logic
{
    public class HttpContextUserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        public string Name => _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;

        public string Email => _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    }
}
