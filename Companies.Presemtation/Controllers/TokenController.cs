using Companies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Companies.Presemtation.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService authService;

        public TokenController(IAuthService authservice)
        {
            authService = authservice;
        }
        

        [HttpPost("login")]
        public async Task<ActionResult> Authenticate(UserForAuthDto authDto)
        {
            if (!await authService.ValidateUserAsync(authDto)) return Unauthorized();

            return Ok(new { Token = await _serviceManager.AuthService.CreateTokenAsync() });
        }
    }
}
