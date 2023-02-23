using HabrParser;
using HabrParser.Database;
using HabrParser.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(builder => builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", optional: false))
        .ConfigureServices((context, services) =>
        {
            services.AddAutoMapper(config => config.RegisterMaps());

            services.AddDbContext<ArticlesDBContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("Articles")));
            services.AddDbContext<CommentsDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("Comments")));

            services.AddTransient<IArticlesRepository, ArticlesRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();

            services.AddHostedService<Worker>();
        }).Build();

    host.Start();
    host.WaitForShutdown();
}
catch (Exception e)
{
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(e.Message.PadRight(60));
    Console.ResetColor();
}