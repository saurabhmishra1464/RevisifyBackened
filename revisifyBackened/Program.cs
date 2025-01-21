using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using revisifyBackened;
using revisifyBackened.Data;
using revisifyBackened.ExceptionHandelling;
using revisifyBackened.Interface;
using revisifyBackened.Interface.IRepository;
using revisifyBackened.Repository;
using revisifyBackened.Services;
using revisifyBackened.Utilities;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var jwtSettings = new JwtSettings();
configuration.Bind("JwtSettings", jwtSettings);
builder.Services.AddSingleton(jwtSettings);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
options.SwaggerDoc("v1", new OpenApiInfo { Title = "Nanhi Duniya User Management API", Version = "v1" });
options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
{
Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
Name = "Authorization",
In = ParameterLocation.Header,
Type = SecuritySchemeType.ApiKey,
Scheme = JwtBearerDefaults.AuthenticationScheme
});

options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                Scheme = "0auth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ClockSkew = jwtSettings.Expire
    };
    options.SaveToken = true;
    options.Events = new JwtBearerEvents();
    options.Events.OnMessageReceived = context =>
    {

        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
        }

        return Task.CompletedTask;
    };
})
     .AddCookie(options =>
     {
         options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         options.Cookie.IsEssential = true;
     });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:3000")  // Allow requests from frontend
            .AllowAnyHeader()                     // Allow headers like Authorization
            .AllowAnyMethod()                     // Allow GET, POST, PUT, etc.
            .AllowCredentials());                 // Allow cookies (if needed)
});

builder.Services.Configure<JWTService>(configuration.GetSection("JwtSettings"));

//Sql Server Setup
if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using AWS RDS SqlServer Db");
    builder.Services.AddDbContext<RevisifyContext>(options => options.UseSqlServer(configuration.GetConnectionString("RevisifyConnection")));
}
else
{
    Console.WriteLine("--> local Db");
    builder.Services.AddDbContext<RevisifyContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<GenerateLink>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<RevisifyContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
