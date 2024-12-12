using revisifyBackened.Models;

namespace revisifyBackened.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateConfirmEmailToken(string email);
        Task<string> GenerateAccessToken(string email, string userId);
        Task<String> GenerateRefreshToken();
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
    }
}
