using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using revisifyBackened.Data;
using revisifyBackened.ExceptionHandelling.CustomException;
using revisifyBackened.Interface;
using revisifyBackened.Models;
using revisifyBackened.Models.Dto;
using revisifyBackened.Utilities;
using System.Web;

namespace revisifyBackened.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly GenerateLink _generateLink;
        private readonly ILogger<AuthService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTService _jwtService;

        public AuthService(IWebHostEnvironment env, IConfiguration configuration,
            ITokenService tokenService, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, RevisifyContext dbContext,
            IEmailService emailService,
            ILogger<AuthService> logger,
            GenerateLink generateLink,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JWTService> options

            )
        {
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
            _generateLink = generateLink;
            this._logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = options.Value;
        }
        public async Task<ApiResponse<object>> Register(RegistrationRequestDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                throw new InvalidOperationException();
            }
            var user = BuildUserFromRegistrationModel(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            //Add User Roles
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin) && model.Email == "saurabhmishra1464@gmail.com")
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User) && model.Email != "saurabhmishra1464@gmail.com")
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            var token = await _tokenService.GenerateConfirmEmailToken(user.Email);

            var verifyEmailLink = GenerateVerifyEmailLink(user.Email, token);

            List<string> toEmailLoist = null;
            toEmailLoist ??= new List<string>();
            toEmailLoist.Add(user.Email);
            toEmailLoist.Add("saurabhmishra1464@gmail.com");
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{"VerifyEmail"}.html");
            var htmlBody = LoadHtmlTemplate(templatePath, verifyEmailLink, user.FullName, user.Email);
            var emailRequest = new EmailRequest()
            {
                ToList = toEmailLoist,
                From = "saurabhmishra1464@gmail.com",
                Subject = "Confirm Email",
                HtmlBody = htmlBody
            };
            _emailService.SendEmailAsync(emailRequest);

            _logger.LogInformation("User registered successfully: {Email}", model.Email);
            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "User Registered Succesfully",
                Data = null
            };               
        }

        public async Task<ApiResponse<UserProfile>> Login(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null)
            {
                _logger.LogWarning($"User with email {loginDto.UserName} was not found");
                throw new NotFoundException($"User with email {loginDto.UserName} was not found");
            }

            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidUser)
            {
                throw new UnauthorizedAccessException();
            }
            if (!user.EmailConfirmed)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                var userProfile = MapToUserProfile(user, userRole);
                throw new EmailNotConfirmedException();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.GenerateAccessToken(user.Email, user.Id);
            var refreshToken = await _tokenService.GenerateRefreshToken();
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtService.AccessTokenExpiry)) });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddHours(Convert.ToInt32(_jwtService.RefreshTokenExpiry)) });
            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Refresh-Token", refreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddHours(Convert.ToInt32(_jwtService.RefreshTokenExpiry)) });

            var loggedInUser = MapToUserProfile(user, roles);

            var userRefreshToken = new UserRefreshToken
            {
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(_jwtService.RefreshTokenExpiry),
                Created = DateTime.UtcNow,
                UserId = user.Id,
                IsRevoked = false
            };
            await _tokenService.AddRefreshTokenAsync(userRefreshToken);

            return new ApiResponse<UserProfile>()
            {
                Data = loggedInUser,
                Message = "Logged In Succesfully"
            };
        }

        public async Task<ApiResponse<object>> SendConfirmationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            var token = await _tokenService.GenerateConfirmEmailToken(email);
            var confirmEmailLink = _generateLink.GenerateVerifyEmailLink(user.Email, token);

            List<string> toEmailLoist = null;
            toEmailLoist ??= new List<string>();
            toEmailLoist.Add(user.Email);
            toEmailLoist.Add("saurabhmishra1464@gmail.com");
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{"VerifyEmail"}.html");
            var htmlBody = LoadHtmlTemplate(templatePath, confirmEmailLink, user.FullName, user.Email);
            var emailRequest = new EmailRequest()
            {
                ToList = toEmailLoist,
                From = "saurabhmishra1464@gmail.com",
                Subject = "Confirm Email",
                HtmlBody = htmlBody
            };
            _emailService.SendEmailAsync(emailRequest);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Confirmation Email Sent Succesfully",
                Data = null
            };
        }

        public async Task<ApiResponse<object>> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException();

            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new EmailNotConfirmedException();
            }

            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Email Verification Completed Succesfully",
                Data = null
            };
        }
        private static ApplicationUser BuildUserFromRegistrationModel(RegistrationRequestDto model)
        {
            return new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FullName = model.FullName,
                PasswordHash = model.Password,
                UserName = model.Email
            };
        }

        public string GenerateVerifyEmailLink(string email, string token)
        {
            var frontendUrl = _configuration["BaseUrls:Frontend"];
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedEmail = HttpUtility.UrlEncode(email);

            return $"{frontendUrl}/auth/verifyEmail?token={encodedToken}&email={encodedEmail}";
        }

        public string LoadHtmlTemplate(string templatePath, string resetLink, string firstName, string to)
        {
            ValidateFileExists(templatePath);
            string htmlContent = File.ReadAllText(templatePath);
            htmlContent = htmlContent.Replace("{{SchoolName}}", "Revisify");
            htmlContent = htmlContent.Replace("{{FirstName}}", firstName);
            htmlContent = htmlContent.Replace("{{resetLink}}", resetLink);
            htmlContent = htmlContent.Replace("{{Email}}", to);
            htmlContent = htmlContent.Replace("{{SchoolDomain}}", to);
            return htmlContent;
        }

        public void ValidateFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}", filePath);
        }

        private UserProfile MapToUserProfile(ApplicationUser user, IList<string> roles)
        {
            return new UserProfile
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Roles = roles.ToList(),
                IEmailConfirmed = user.EmailConfirmed,
            };
        }
    }
}
