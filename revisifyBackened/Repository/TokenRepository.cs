using Microsoft.EntityFrameworkCore;
using revisifyBackened.Data;
using revisifyBackened.Interface.IRepository;
using revisifyBackened.Models;

namespace revisifyBackened.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly RevisifyContext _dbContext;

        public TokenRepository(RevisifyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.UserRefreshTokens
                .FirstOrDefaultAsync(token => token.RefreshToken == refreshToken);
        }

        public async Task<List<UserRefreshToken>> GetListOfRefreshTokensByUserIdAsync(string userId)
        {
            return await _dbContext.UserRefreshTokens
                .Where(token => token.UserId == userId)
                .ToListAsync();
        }

        public async Task AddRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            await _dbContext.UserRefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(IEnumerable<UserRefreshToken> refreshTokens)
        {
            _dbContext.UserRefreshTokens.UpdateRange(refreshTokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            _dbContext.UserRefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
    }

}
