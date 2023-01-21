using FirstProject.MessageTest;

using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




builder.Host.UseMassTransit();


string RabbitMqHost = builder.Configuration.GetConnectionString("RabbitMqHost");
string RabbitMqVHost = builder.Configuration.GetConnectionString("RabbitMqVHost");
string RabbitMqUser = builder.Configuration.GetConnectionString("RabbitMqUser");
string RabbitMqPassword = builder.Configuration.GetConnectionString("RabbitMqPassword");


builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, cfgM) =>
    {
        cfgM.Host(RabbitMqHost, RabbitMqVHost, h =>
        {
            h.Username(RabbitMqUser);
            h.Password(RabbitMqPassword);
        });
        cfgM.Durable = true;
        cfgM.AutoDelete = false;
        cfgM.ConfigureEndpoints(context);
    });
    cfg.AddConsumer<TestConsumer>();
    ;

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}


app.UseAuthorization();

app.MapControllers();

app.Run();
