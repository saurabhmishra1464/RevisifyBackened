using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using revisifyBackened.Data;
using revisifyBackened.Interface;
using revisifyBackened.Interface.IRepository;
using revisifyBackened.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace revisifyBackened.Services
{
    public class TokenService: ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _settings;
        private readonly ITokenRepository _tokenRepository;
        public TokenService(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository, JwtSettings settings)
        {
            _userManager = userManager;
            _settings = settings;
            _tokenRepository = tokenRepository;
        }

        public async Task<string> GenerateConfirmEmailToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GenerateAccessToken(string email, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId)
            }.Union(userClaims).Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            _settings.Issuer,
                _settings.Audience,
            claims,
                expires: DateTime.UtcNow + _settings.Expire,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task AddRefreshTokenAsync(UserRefreshToken refreshToken)
        {

            await _tokenRepository.AddRefreshTokenAsync(refreshToken);
        }

        public async Task<string> GenerateRefreshToken()
        {
            var refreshToken = GenerateRandomString();

            return refreshToken;
        }

        private string GenerateRandomString()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
