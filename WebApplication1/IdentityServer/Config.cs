using IdentityServer4;
using IdentityServer4.Models;

namespace WebApplication1.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
        {
            new ApiScope("api1", "My API")
        };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
            new Client
            {
                ClientId = "client1",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1" }
            },
            // Other clients...
        };
        }
    }

}
