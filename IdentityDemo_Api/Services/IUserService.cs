using AspNetIdentity.Demo.Shared;

namespace IdentityDemo_Api.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }
}
