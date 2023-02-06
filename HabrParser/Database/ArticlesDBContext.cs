using AutoMapper;
using HabrParser.Models.APIArticles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;

namespace HabrParser.Database
{
    public class ArticlesDBContext : DbContext
    {
        private readonly string _connectionString = "";
        public DbSet<HabrParser.Models.APIArticles.Article> Articles { get; set; } = null!;
        public DbSet<HabrParser.Models.APIArticles.Author> Authors { get; set; } = null!;
        public DbSet<HabrParser.Models.APIArticles.Contact> Contacts { get; set; } = null!;
        public DbSet<HabrParser.Models.APIArticles.Hub> Hubs { get; set; } = null!;
        public DbSet<HabrParser.Models.APIArticles.LeadData> Leads { get; set; } = null!;
        public DbSet<HabrParser.Models.APIArticles.Tag> Tags { get; set; } = null!;
        public DbSet<HabrParser.Models.APIArticles.ParserResult> ParserResult { get; set; } = null!;
        public ArticlesDBContext(DbContextOptions options) : base(options)
        {            
            Database.EnsureCreated();
        }
        public ArticlesDBContext(string connectionString)
        {
            _connectionString= connectionString;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(a => a.Articles)
                .HasForeignKey(a => a.AuthorId);
            modelBuilder.Entity<Article>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Articles);
            modelBuilder.Entity<Article>()
                .HasMany(h => h.Hubs)
                .WithMany(h => h.Articles);
            modelBuilder.Entity<Article>()
                .HasOne(l => l.LeadData)
                .WithOne(l => l.Article)
                .HasForeignKey<LeadData>(l => l.Id);

            modelBuilder.Entity<Tag>()
                .HasMany(tag => tag.Articles)
                .WithMany(article => article.Tags);

            modelBuilder.Entity<Hub>()
                .HasMany(hub => hub.Articles)
                .WithMany(hub => hub.Hubs);

            modelBuilder.Entity<Author>()
               .HasMany(a => a.Articles)
               .WithOne(article => article.Author);
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Contacts)
                .WithOne(a => a.Author);

            modelBuilder.Entity<Contact>()
                .HasOne(a => a.Author)
                .WithMany(a => a.Contacts);

            modelBuilder.Entity<LeadData>()
                .HasOne(le => le.Article)
                .WithOne(le => le.LeadData);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != "")
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            else
            {
                base.OnConfiguring(optionsBuilder);
            }            
        }
    }
}
