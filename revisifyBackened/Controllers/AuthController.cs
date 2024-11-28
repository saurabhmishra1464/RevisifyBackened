using Microsoft.AspNetCore.Mvc;
using revisifyBackened.Models.Dto;
using revisifyBackened.Repository;

namespace revisifyBackened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService accountService)
        {
            _authService = accountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.Register(model);
            return Ok(result);

        }

    }
}
