using FirstProject.CommentsAPI;
using FirstProject.CommentsAPI.Data;
using FirstProject.CommentsAPI.Data.Repositories;
using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(setup =>
{
    setup.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration["ServiceUrls:AuthAPI"],
                builder.Configuration["ServiceUrls:Web"]
                )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<CommentsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddAutoMapper(config =>
{
    config.RegisterMaps();
});

builder.Services.AddTransient<ICommentsRepository, CommentsRepository>();
builder.Services.AddTransient<ICommentsCountRepository, CommentsCountRepository>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "CommentsAPI", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

    var filepath = AppContext.BaseDirectory;
    var filename = "FirstProject.CommentsAPI.xml";
    options.IncludeXmlComments(Path.Combine(filepath, filename));
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
