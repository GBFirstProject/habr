﻿using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

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
        public DbSet<Statistics> Statistics { get; set; } = null!;
        public DbSet<Metadata> Metadata { get; set; } = null!;
        public ArticlesDBContext(DbContextOptions options) : base(options)
        {            
            Database.EnsureCreated();
            /*if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.ClrType.GetProperties())
                {
                    if (property.PropertyType == typeof(List<string>))
                    {
                        modelBuilder.Entity(entity.Name)
                            .Property(property.Name)
                            .HasConversion(new ValueConverter<List<string>, string>(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<List<string>>(v)));
                    }
                }
            }

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
            modelBuilder.Entity<Article>()
                .Property(a => a.Language)
                .HasConversion(new EnumToStringConverter<ArticleLanguage>());
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Statistics)
                .WithOne(a => a.Article)
                .HasForeignKey<Statistics>(a => a.Id);
            modelBuilder.Entity<Article>()
                .HasOne(l => l.MetaData)
                .WithOne(l => l.Article)
                .HasForeignKey<Metadata>(l => l.Id);

            modelBuilder.Entity<Tag>()
                .HasMany(tag => tag.Articles)
                .WithMany(article => article.Tags);
            modelBuilder.Entity<Tag>().
                HasIndex(t => t.TagName);

            modelBuilder.Entity<Hub>()
                .HasMany(hub => hub.Articles)
                .WithMany(hub => hub.Hubs);
            modelBuilder.Entity<Hub>().
                HasIndex(h => h.Title);

            modelBuilder.Entity<Author>()
            .HasAlternateKey(a => a.NickName);
            modelBuilder.Entity<Author>()
               .HasMany(a => a.Articles)
               .WithOne(article => article.Author);
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Contacts)
                .WithOne(a => a.Author);
            modelBuilder.Entity<Author>()
                .HasIndex(a => a.NickName);
            modelBuilder.Entity<Author>()
                .Property(x => x.Rating).HasColumnType("decimal").HasPrecision(6, 2);

            modelBuilder.Entity<Contact>()
                .HasOne(a => a.Author)
                .WithMany(a => a.Contacts);

            modelBuilder.Entity<LeadData>()
                .HasOne(le => le.Article)
                .WithOne(le => le.LeadData)
                .HasForeignKey<Article>(le => le.LeadDataId);
                        
            modelBuilder.Entity<Metadata>()
                .HasOne(me => me.Article)
                .WithOne(me => me.MetaData)
                .HasForeignKey<Article>(le => le.MetaDataId);

            modelBuilder.Entity<Statistics>()
                .HasOne(le => le.Article)
                .WithOne(le => le.Statistics)
                .HasForeignKey<Article>(le => le.StatisticsId);


        }
    }
}
