using FirstProject.CommentsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FirstProject.CommentsAPI
{
    public class CommentsDbContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; } = null!;

        public CommentsDbContext(DbContextOptions<CommentsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(x =>
            {
                x.Property(y => y.Likes)
                    .HasConversion(
                        guid => string.Join(';', guid),
                        str => str.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => Guid.Parse(s)).ToList());
                x.Property(y => y.Dislikes)
                    .HasConversion(
                        guid => string.Join(';', guid),
                        str => str.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => Guid.Parse(s)).ToList());
            });
        }
    }
}
