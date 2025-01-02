using Microsoft.AspNetCore.Mvc;
using revisifyBackened.Interface;
using revisifyBackened.Models;
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
        public async Task<IActionResult> SaveQuestions([FromForm] IFormFile file, [FromForm] int subjectId)
        {
            var outputFilePath = await _authService.SaveQuestionsAsync(file, subjectId);

            return Ok(outputFilePath);
        }

        [HttpPost("UploadQuestionImage")]
        public async Task<IActionResult> UploadQuestionImage(IFormFile imageFile, int questionId)
        {
            var outputFilePath = await _authService.UploadQuestionImage(imageFile, questionId);
            return Ok(outputFilePath);
        }

        [HttpGet("GetAllSubjects")]
        public async Task<ActionResult<ApiResponse<object>>> GetAllSubjects()
        {
            var response = await _authService.GetAllSubjectsAsync();

            // Return appropriate HTTP status code based on ApiResponse
            return Ok(response);
        }
    }
}
