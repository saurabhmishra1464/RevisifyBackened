using revisifyBackened.Models;
using revisifyBackened.Models.Dto;

namespace revisifyBackened.Interface
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Register(RegistrationRequestDto model);
        Task<ApiResponse<UserProfile>> Login(LoginRequestDto model);
        Task<ApiResponse<object>> SendConfirmationEmail(string email);
        Task<ApiResponse<object>> ConfirmEmail(string token, string email);
        Task<string> SaveQuestionsAsync(IFormFile file, int SubjectId);
    }
}
