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
        private readonly IMailServise _mailServise;

        public AuthController(IUserService userService, IMailServise mailServise)
        {
            _userService = userService;
            _mailServise = mailServise;
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
    }
}
