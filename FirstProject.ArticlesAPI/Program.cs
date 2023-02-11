using AutoMapper;
using FirstProject.ArticlesAPI;
using FirstProject.ArticlesAPI.Data;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Services;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(setup =>
        {
            setup.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(
                        builder.Configuration["ServiceUrls:AuthAPI"]!,
                        builder.Configuration["ServiceUrls:Web"]!
                        )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

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

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = builder.Configuration["ServiceUrls:AuthAPI"];

                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    ValidateLifetime = false,
                    RequireExpirationTime = false,
                    RequireSignedTokens = false
                };
            });

        var app = builder.Build();
        app.UseCors();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}