using HabrParser.Database;
using HabrParser.Database.Repositories;
using HabrParser.Interfaces;
using HabrParser.Models.APIAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HabrParser
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(config => config.RegisterMaps());

            services.AddDbContext<ArticlesDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Articles")));
            services.AddDbContext<CommentsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Comments")));
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Auth")));

            services.AddTransient<IArticlesRepository, ArticlesRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();
            services.AddTransient<ICommentsCountRepository, CommentsCountRepository>();

            services.AddDefaultIdentity<ApplicationUser>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>();

            services.AddHostedService<Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}