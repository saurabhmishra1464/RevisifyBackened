using revisifyBackened.Models;

namespace revisifyBackened.Interface.IRepository
{
    public interface ITokenRepository
    {
        Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task<List<UserRefreshToken>> GetListOfRefreshTokensByUserIdAsync(string userId);
        Task AddRefreshTokenAsync(UserRefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(IEnumerable<UserRefreshToken> refreshTokens);
        Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken);
    }

}
