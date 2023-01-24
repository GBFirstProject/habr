using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace FirstProject.AuthAPI;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string> 
                { 
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        { 
            new ApiScope("allAPI", "All API application") 
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        { 
        };

    public static IEnumerable<Client> Clients =>
        new List<Client> 
        {
            // machine-to-machine client
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("Ncydu%ggus&txgyYFTd$56h_dk".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "allAPI" }
            },
            // JavaScript BFF client
            new Client
            {
                ClientId = "webClient",
                ClientSecrets = { new Secret("Bdg&6ed3hfh(jcB@3r5fDwJdyd".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                
                // where to redirect to after login
                RedirectUris = { "https://localhost:5003/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "allAPI"
                }
            }
        };
}