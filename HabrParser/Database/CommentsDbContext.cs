using HabrParser.Models.APIComments;
using Microsoft.EntityFrameworkCore;

namespace HabrParser.Database
{
    public class CommentsDbContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CommentsCount> CommentsCount { get; set; } = null!;

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
