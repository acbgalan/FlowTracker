using Microsoft.AspNetCore.Identity;
using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContext, UserManager<AppUser> userManager)
        {
            _httpContext = httpContext;
            _userManager = userManager;
        }

        public async Task<AppUser?> GetUserAsync()
        {
            var claimEmail = _httpContext.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            if (claimEmail == null)
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(claimEmail.Value);
        }

        public async Task<string?> GetUserIdAsync()
        {
            var user = await this.GetUserAsync();

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public string? GetEmail()
        {
            var claimEmail = _httpContext.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            if (claimEmail == null)
            {
                return null;
            }

            return claimEmail!.Value;
        }


    }
}
