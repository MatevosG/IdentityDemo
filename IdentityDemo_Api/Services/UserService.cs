using AspNetIdentity.Demo.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityDemo_Api.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManger;
        private IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManger = userManager;
            _configuration = configuration;
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

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "user success created"
                };
            }

            return new UserManagerResponse
            {
                Errors = result.Errors.Select(x => x.Description),
                IsSuccess = false,
                Message = "user did not create"
            };
        }
        public async Task<UserManagerResponse> LuginUserAsync(LoginViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "There is no user with that email addres"
                };
            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if(!result)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Invalid password"
                };
            var claims = new[]
            {
              new Claim("Email",model.Email),
              new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings : Key"]));

            var token = new JwtSecurityToken(
                issuer : _configuration["AuthSettings : Issuer"],
                audience : _configuration["AuthSettings : Audience"],
                claims : claims,
                expires : DateTime.UtcNow.AddDays(10),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = tokenAsString,
                ExpireData = token.ValidTo
            };
        }
    }
}
