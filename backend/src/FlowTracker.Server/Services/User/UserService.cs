using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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

        public bool IsAdministrator()
        {
            var administratorClaim = _httpContext.HttpContext!.User.Claims.Where(x => x.Type == "Administrator").FirstOrDefault();
            return administratorClaim != null ? true : false;
        }

        public async Task<bool> SetAdministrator(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            await _userManager.AddClaimAsync(user, new Claim("Administrator", "true"));
            return true;
        }

        public async Task<bool> RemoveAdministrator(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            await _userManager.RemoveClaimAsync(user, new Claim("Administrator", "true"));
            return true;
        }
    }
}