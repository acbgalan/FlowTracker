using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.User;
using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.User
{
    public interface IUserService
    {
        Task<AppUser?> GetUserAsync();
        bool IsAdministrator();
        Task<bool> SetAdministrator(string email);
        Task<bool> RemoveAdministrator(string email);

        Task<ServiceResult<UserAuthenticationResponse>> LoginAsync(UserLoginRequest userLoginRequest);
        Task<UserAuthenticationResponse> BuildToken(string email);
    }
}