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
using Microsoft.Extensions.Hosting;

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Auth")));

            services.AddTransient<IArticlesRepository, ArticlesRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddDefaultIdentity<ApplicationUser>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients(Configuration))
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential();

            services.AddHostedService<Worker>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context,
            IDbInitializer dbInit)
        {
            if (env.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            dbInit.Initialize();
        }
    }
}