using Duende.Bff.Yarp;
using FirstProject.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.ConfigureAPIBase(builder.Configuration);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services
    .AddBff()
    .AddRemoteApis();

builder.Services
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["ServiceUrls:AuthAPI"];
        options.ClientId = "clientUser";
        options.ClientSecret = "Acbudhbfsigfdgd773bcibkaf23bcgisid7gYgd";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("firstProject");
        options.Scope.Add("offline_access");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
    });

builder.Services.AddControllers();

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot().AddDelegatingHandler<HttpDelegatingHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseBff();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBffManagementEndpoints();

    endpoints.MapControllers()
         .AsBffApiEndpoint();
});

app.UseOcelot().Wait();

app.Run();
