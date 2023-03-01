using FirstProject.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabrParser.Database
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApplicationUser> Users { get; set; } = null!;

    }
}
