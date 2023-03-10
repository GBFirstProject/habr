using HabrParser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

try
{
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, builder) => builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", optional: false))
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .Build().Run();
}
catch (Exception e)
{
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(e.Message.PadRight(60));
    Console.ResetColor();
}