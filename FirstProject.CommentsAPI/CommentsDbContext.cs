using FirstProject.CommentsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.CommentsAPI
{
    public class CommentsDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; } = null!;

        public CommentsDbContext(DbContextOptions<CommentsDbContext> options) : base(options)
        {

        }
    }
}
