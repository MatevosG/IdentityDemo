using AspNetIdentity.Demo.Shared;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo_Api.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManger;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManger = userManager;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            
            if(model==null)
                throw new ArgumentNullException("model");
            // passe erku angam havaqela erkusne chishta
            if (model.ConfirmPassword != model.Password)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };
            var result = await _userManger.CreateAsync(identityUser, model.Password);



            throw new NotImplementedException();
        }
    }
}
