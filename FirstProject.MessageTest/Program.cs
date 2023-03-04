//Проект для тестирования уведомлений

using FirstProject.MessageTest.Hubs;

using MassTransit;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSignalR();

builder.Host.UseMassTransit();
string RabbitMqHost = builder.Configuration.GetConnectionString("RabbitMqHost");
string RabbitMqVHost = builder.Configuration.GetConnectionString("RabbitMqVHost");
string RabbitMqUser = builder.Configuration.GetConnectionString("RabbitMqUser");
string RabbitMqPassword = builder.Configuration.GetConnectionString("RabbitMqPassword");


builder.Services.AddMassTransit(cfg =>
{
    

    var entryAssembly = Assembly.GetEntryAssembly();
    cfg.AddConsumers(entryAssembly);
 
    cfg.UsingRabbitMq((context, cfgM) =>
    {
        cfgM.Host(RabbitMqHost, RabbitMqVHost, h =>
        {
            h.Username(RabbitMqUser);
            h.Password(RabbitMqPassword);
        });
        
        cfgM.ConfigureEndpoints(context);  
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseAuthorization();

app.MapHub<NotificationHub>("/Notification");

app.Run();
