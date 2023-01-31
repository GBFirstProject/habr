using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FirstProject.AuthAPI.Areas.Identity.IdentityHostingStartup))]
namespace FirstProject.AuthAPI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}