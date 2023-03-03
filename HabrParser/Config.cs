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
            return new List<Client>
            {
                new Client
                {
                    ClientId="clientApp",
                    ClientSecrets= { new Secret("VdhjadcjihB5d4xfssjcugugVddn$xsvBxuy7xh".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={ "firstProject"}
                }
            };
        }

    }
}


