using Microsoft.AspNetCore.Mvc;
using revisifyBackened.Interface;
using revisifyBackened.Models.Dto;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var result = await _authService.Login(model);
            return Ok(result);
        }

        [HttpPost("SendConfirmationEmail")]
        public async Task<IActionResult> SendConfirmationEmail(ResendEmailDto resendEmailDto)
        {
            var result = await _authService.SendConfirmationEmail(resendEmailDto.Email);
            return Ok(result);
        }

    }
}
