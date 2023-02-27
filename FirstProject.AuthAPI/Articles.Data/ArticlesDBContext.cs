using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;

namespace FirstProject.AuthAPI.Articles.Data
{
    public class ArticlesDBContext : DbContext
    {        
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Hub> Hubs { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public ArticlesDBContext(DbContextOptions options) : base(options)
        {           
            Database.EnsureCreated();            
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
                .HasMany(t => t.Tags)
                .WithMany(t => t.Articles);
            modelBuilder.Entity<Article>()
                .HasMany(h => h.Hubs)
                .WithMany(h => h.Articles);

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
                .HasIndex(a => a.NickName);
            modelBuilder.Entity<Author>()
                .Property(x => x.Rating).HasColumnType("decimal").HasPrecision(6, 2);         
        }
    }
}
