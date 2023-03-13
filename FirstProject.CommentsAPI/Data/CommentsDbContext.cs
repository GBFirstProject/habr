using FirstProject.CommentsAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.CommentsAPI.Data
{
    public class CommentsDbContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; } = null!;

        public CommentsDbContext(DbContextOptions<CommentsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(x =>
            {
                x.Property(y => y.Likes)
                    .HasConversion(
                        list => string.Join(';', list),
                        str => str.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => s).ToList());
                x.Property(y => y.Dislikes)
                    .HasConversion(
                        list => string.Join(';', list),
                        str => str.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => s).ToList());
            });
        }
    }
}
