using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.User
{
    public interface IUserService
    {
        Task<AppUser?> GetUserAsync();
    }
}
