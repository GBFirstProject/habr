﻿using FirstProject.ArticlesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticlesAPI.Data
{
    public class ArticlesDBContext : DbContext
    {
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Hub> Hubs { get; set; } = null!;
        public DbSet<LeadData> Leads { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;                
        public ArticlesDBContext(DbContextOptions options) : base(options)
        {            
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);            
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

        }
    }
}
