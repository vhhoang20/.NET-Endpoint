using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Security.Claims;

namespace WebApplication1.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("myApi.read"),
            new ApiScope("myApi.write"),
        };

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            //new IdentityResource("sub", new List<string> { JwtClaimTypes.Subject })
        };
        }

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes =  GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("supersecret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OfflineAccess,
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    "myApi.read","myApi.write"}
                }
            };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("myApi")
            {
                Scopes = new List<string>{ "myApi.read","myApi.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
            }
        };
    }

}
