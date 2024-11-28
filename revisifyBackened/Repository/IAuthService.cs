using revisifyBackened.Models.Dto;

namespace revisifyBackened.Repository
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Register(RegistrationRequestDto model);
    }
}
