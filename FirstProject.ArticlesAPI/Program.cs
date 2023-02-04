using AutoMapper;
using FirstProject.ArticlesAPI;
using FirstProject.ArticlesAPI.Data;
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

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();        

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