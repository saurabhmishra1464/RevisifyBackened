using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using revisifyBackened.Data;
using revisifyBackened.Models.Dto;
using revisifyBackened.Utilities;

namespace revisifyBackened.Repository
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RevisifyContext _dbContext;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,RevisifyContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }
        public async Task<ApiResponse<object>> Register(RegistrationRequestDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return ApiResponseHelper.CreateErrorResponse<object>("User already exists!", StatusCodes.Status409Conflict);
            }
            var user = BuildUserFromRegistrationModel(model);
            var result = await CreateUserAsync(user, user.PasswordHash);

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


            //var token = await _tokenService.GenerateConfirmEmailToken(user.Email);

            // Customize the email template and send verify email link

            //var verifyEmailLink = _userService.GenerateVerifyEmailLink(user.Email, token);

            //List<string> toEmailLoist = null;
            //toEmailLoist ??= new List<string>();
            //toEmailLoist.Add(user.Email);
            //toEmailLoist.Add("saurabhmishra1464@gmail.com");
            //var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{"VerifyEmail"}.html");
            //var htmlBody = LoadHtmlTemplate(templatePath, verifyEmailLink, user.FirstName, user.Email);


            //_logger.LogInformation("User registered successfully: {Email}", model.Email);
            return ApiResponseHelper.CreateSuccessResponse<object>(null, "User Registered Succesfully");
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

        private async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string? password)
        {
            return await _userManager.CreateAsync(user, password!);
        }

        //public async Task InsertAsync(RegistrationRequestDto registration)
        //{
        //    await _dbContext.Admins.AddAsync(admin);
        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
