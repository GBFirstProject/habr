using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services
    .AddBff()
    .AddRemoteApis();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";
        options.ClientId = "webClient";
        options.ClientSecret = "Bdg&6ed3hfh(jcB@3r5fDwJdyd";
        options.ResponseType = "code";
        options.Scope.Add("allAPI");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseBff();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBffManagementEndpoints();
});

app.Run();
