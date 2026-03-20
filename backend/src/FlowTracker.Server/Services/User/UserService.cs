using Microsoft.AspNetCore.Identity;
using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task<AppUser?> GetUserAsync()
        {
            var emailClaim = _httpContext.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            if (emailClaim == null)
            {
                return null;
            }

            var email = emailClaim.Value;
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
