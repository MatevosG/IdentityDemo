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

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var register = await _userService.RegisterUserAsync(model);
                if(register.IsSuccess)
                    return Ok(register);
                return BadRequest(register);
            }
            return BadRequest();
        }

    }
}
