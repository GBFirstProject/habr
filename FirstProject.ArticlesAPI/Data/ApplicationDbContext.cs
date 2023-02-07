using FirstProject.ArticlesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.ArticlesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Article>().HasData(new Article
            {
                ArticleId = 1,
                Author = "Vasya",
                Title = "Samosa",
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://dotnetmastery.blob.core.windows.net/mango/14.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Article>().HasData(new Article
            {
                ArticleId = 2,
                Author = "Vasya",
                Title = "Paneer Tikka",
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://dotnetmastery.blob.core.windows.net/mango/12.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Article>().HasData(new Article
            {
                ArticleId = 3,
                Title = "Sweet Pie",
                Author = "Petya",
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://dotnetmastery.blob.core.windows.net/mango/11.jpg",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Article>().HasData(new Article
            {
                ArticleId = 4,
                Title = "Pav Bhaji",
                Author = "Petya",
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://dotnetmastery.blob.core.windows.net/mango/13.jpg",
                CategoryName = "Entree"
            });
        }
    }
}
