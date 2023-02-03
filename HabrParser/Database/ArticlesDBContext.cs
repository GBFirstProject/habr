using AutoMapper;
using HabrParser.Models.ArticleOnly;
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
        public DbSet<ParsedArticle> Articles { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Flow> Flows { get; set; } = null!;
        public DbSet<Hub> Hubs { get; set; } = null!;
        public DbSet<LeadData> Leads { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;        
        public DbSet<ParserResult> ParserResult { get; set; } = null!;
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
            modelBuilder.Entity<LeadData>()
                .HasOne(a => a.ParsedArticle)
                .WithOne(ld => ld.LeadData);
            modelBuilder.Entity<ParsedArticle>()
                .HasMany(a => a.Tags);
            modelBuilder.Entity<ParsedArticle>()
                .HasMany(a => a.Hubs);


            var dbContext = this;
            /*var tagKeyConverter = new ValueConverter<Tag, int>(
                v => v.TagId,
                v => new Tag(v, this));*/
            var allTagsConverter = new ValueConverter<List<Tag>, List<int>>(
                v => v.ConvertAll<int>(x => x.TagId),
                v => v.ConvertAll<Tag>(x => new Tag(x, dbContext)));

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
