using Microsoft.AspNetCore.Mvc;
using revisifyBackened.Interface;
using revisifyBackened.Models.Dto;
using System.Xml;

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

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var verifyEmailResult = await _authService.ConfirmEmail(token, email);
            return Ok(verifyEmailResult);
        }

        [HttpPost("SaveQuestions")]
        public async Task<IActionResult> SaveQuestions(IFormFile file, int SubjectId )
        {
            var outputFilePath = await _authService.SaveQuestionsAsync(file, SubjectId);

            return Ok(new { Message = "File processed successfully.", OutputFile = outputFilePath });
        }

    }
}
