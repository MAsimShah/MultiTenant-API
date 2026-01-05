using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace DotNetCoreWithIdentityServer.Models
{
    public static class IdentityConfig
    {
        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope("myapi", "My Protected API")
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "web-client",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // allows username+password login
                    AccessTokenLifetime = 60, // one minute
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 180, // 3 minute
                    AllowOfflineAccess = true,

                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    AllowedScopes = { "openid", "profile", "myapi", "offline_access" }
                }
            };
    }
}