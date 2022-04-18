using AspNetIdentity.Demo.Shared;
using IdentityDemo_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailServise;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IMailService mailServise, IConfiguration configuration)
        {
            _userService = userService;
            _mailServise = mailServise;
            _configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var register = await _userService.RegisterUserAsync(model);
                if(register.IsSuccess)
                    return Ok(register);
                return BadRequest(register);
            }
            return BadRequest("some properties are not valid");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LuginUserAsync(model);
                if (!result.IsSuccess)
                    return BadRequest(result);
                await _mailServise.SendEmailAsync(model.Email, "New login", "<h1> Hey!,new login to your account npticed</h1><p>New login your account at" + DateTime.Now + "</p>");
                return Ok(result);
            }
            return BadRequest("some properties are not valid");
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
            }

            return BadRequest(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.IsSuccess)
                return Ok(result); 

            return BadRequest(result); 
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }
    }
}
