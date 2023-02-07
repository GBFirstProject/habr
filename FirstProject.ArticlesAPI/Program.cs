using AutoMapper;
using FirstProject.ArticlesAPI;
using FirstProject.ArticlesAPI.Data;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Services;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("config.json");
        builder.Services.AddDbContext<ArticlesDBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MappingProfile()));
        var mapper = mapperConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.AddTransient<IRepository<Article>, Repository<Article>>();
        builder.Services.AddTransient<IRepository<Author>, Repository<Author>>();
        builder.Services.AddTransient<IRepository<Hub>, Repository<Hub>>();
        builder.Services.AddTransient<IArticleService, ArticleService>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments($"{AppContext.BaseDirectory}\\FirstProject.ArticlesAPI.xml");
        });        

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}