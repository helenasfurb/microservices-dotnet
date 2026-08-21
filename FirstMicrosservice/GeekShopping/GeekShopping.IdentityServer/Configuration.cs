using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Drawing;

namespace GeekShopping.IdentityServer;

public static class Configuration
{

    public const string Admin = "Admin";
    public const string Customer = "Customer";
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("geek_shopping", "GeekShopping Full Access"),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "geek_shopping_web",
                ClientName = "Geek Shopping Web",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                RedirectUris = { "http://localhost:5180/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5180/signout-callback-oidc" },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "geek_shopping"
                }
            }
        };
}
