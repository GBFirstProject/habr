using FirstProject.ArticlesAPI.Models.Habr.Original;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticlesAPI.Data
{
    public class ArticlesDBContext : DbContext
    {
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Flow> Flows { get; set; } = null!;
        public DbSet<Hub> Hubs { get; set; } = null!;
        public DbSet<LeadData> Leads { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;        
        public DbSet<User> Users { get; set; } = null!;
        public ArticlesDBContext(DbContextOptions options) : base(options)
        {            
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
            //optionsBuilder.UseSqlServer(ConnectionString);
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
