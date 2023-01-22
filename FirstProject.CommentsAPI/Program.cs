using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Repositories;
using FirstProject.CommentsAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CommentsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddAutoMapper(config =>
{
    config.RegisterMaps();
});

builder.Services.AddTransient<ICommentsRepository, CommentsRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var filepath = AppContext.BaseDirectory;
    var filename = "FirstProject.CommentsAPI.xml";
    options.IncludeXmlComments(Path.Combine(filepath, filename));
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
