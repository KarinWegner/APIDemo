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
        

        [HttpPost("refresh")]
        public async Task<ActionResult> Authenticate(TokenDto token)
        {
            TokenDto tokenDto = await authService.RefreshTokenAsync(token);
            return Ok(tokenDto);
        }
    }
}
