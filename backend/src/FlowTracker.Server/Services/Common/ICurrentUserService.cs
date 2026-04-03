using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.Common
{
    public interface ICurrentUserService
    {
        Task<AppUser?> GetUserAsync();
        Task<string?> GetUserId();
        string? GetEmail();
    }
}
