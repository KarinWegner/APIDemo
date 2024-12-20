using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Companies.Presemtation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost]
        public async Task<ActionResult> RegisterUser(UserForRegistrationDto registrationDto)
        {
            var result = await _serviceManager.AuthService.RegisterUserAsync(registrationDto);
            return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors); 
        }

        [HttpPost("login")]
        public async Task<ActionResult> Authenticate(UserForAuthDto authDto)
        {
            if (!await _serviceManager.AuthService.ValidateUserAsync(authDto)) return Unauthorized();

            return Ok(new {Token = await _serviceManager.AuthService.CreateTokenAsync()});
        }
    }
}
