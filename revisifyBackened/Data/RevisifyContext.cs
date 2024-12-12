using Microsoft.EntityFrameworkCore;
using revisifyBackened.Models;
using revisifyBackened.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace revisifyBackened.Data
{
    public class RevisifyContext : IdentityDbContext<ApplicationUser>
    {
        public RevisifyContext(DbContextOptions<RevisifyContext> options) : base(options) { }
    
        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }


    }
}
