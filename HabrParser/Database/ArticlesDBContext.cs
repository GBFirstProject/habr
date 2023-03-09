using AutoMapper;
using HabrParser.Models.APIArticles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace HabrParser.Database
{
    public class ArticlesDBContext : DbContext
    {
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Hub> Hubs { get; set; } = null!;
        public DbSet<LeadData> Leads { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<ParserResult> ParserResult { get; set; } = null!;
        public DbSet<Statistics> Statistics { get; set; } = null!;
        public DbSet<Metadata> Metadata { get; set; } = null!;

        public ArticlesDBContext(DbContextOptions<ArticlesDBContext> options) : base(options)
        {            
            Database.EnsureCreated();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            TrackChangesAtRelatedToArticleTables();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            TrackChangesAtRelatedToArticleTables();
            return base.SaveChanges();
        }
        private void TrackChangesAtRelatedToArticleTables()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Article && (
                        e.State == EntityState.Added));

            foreach (var entry in entries)
            {
                var article = entry.Entity as Article;
                if (article != null && article.LeadData != null)
                {
                    article.LeadData.ArticleId = article.Id;
                }
            }
            foreach (var entry in entries)
            {
                var article = entry.Entity as Article;
                if (article != null && article.MetaData != null)
                {
                    article.MetaData.ArticleId = article.Id;
                }
            }
            foreach (var entry in entries)
            {
                var article = entry.Entity as Article;
                if (article != null && article.Statistics != null)
                {
                    article.Statistics.ArticleId = article.Id;
                }
            }
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
                    if (property.PropertyType == typeof(List<Guid>))
                    {
                        modelBuilder.Entity(entity.Name)
                            .Property(property.Name)
                            .HasConversion(new ValueConverter<List<Guid>, string>(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<List<Guid>>(v)));
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
            modelBuilder.Entity<Article>()
                .HasOne(a => a.LeadData)
                .WithOne(a => a.Article)
                .HasForeignKey<LeadData>(l => l.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Article>()
                .HasOne(a => a.MetaData)
                .WithOne(a => a.Article)
                .HasForeignKey<Metadata>(m => m.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Statistics)
                .WithOne(a => a.Article)
                .HasForeignKey<Statistics>(s => s.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Article>()
                .HasIndex(a => a.TimePublished);

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
