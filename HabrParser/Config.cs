using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace HabrParser
{
    public static class Config
    {
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string User = "User";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> {
                new ApiScope("firstProject", "All API application")
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            var bffClientUrl = configuration["javascriptbff-client"]!.ToString();

            return new List<Client>
            {
                new Client
                {
                    ClientId="clientApp",
                    ClientSecrets= { new Secret("VdhjadcjihB5d4xfssjcugugVddn$xsvBxuy7xh".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={ "firstProject"}
                },
                new Client
                {
                    ClientId="clientUser",
                    ClientSecrets= {
                        new Secret("Acbudhbfsigfdgd773bcibkaf23bcgisid7gYgd".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { $"{bffClientUrl}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{bffClientUrl}/signout-callback-oidc" },
                    FrontChannelLogoutUri = $"{bffClientUrl}/signout-oidc",
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "firstProject"
                    },
                    AllowOfflineAccess=true
                }
            };
        }

    }
}


