using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.User;
using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.User
{
    public interface IUserService
    {
        Task<ServiceResult<UserAuthenticationResponse>> RegisterAsync(UserRegisterRequest userRegisterRequest);
        Task<ServiceResult<UserAuthenticationResponse>> LoginAsync(UserLoginRequest userLoginRequest);
    }
}