using System.Security.Claims;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.API.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(claim, out var userId) ? userId : Guid.Empty;
            }

        }
        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        public bool IsInRole(string role)
            => _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }
}