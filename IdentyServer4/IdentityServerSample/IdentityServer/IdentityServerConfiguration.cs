using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{

    //https://nahidfa.com/posts/migrating-identityserver4-to-v4/
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("ApiOne")
            };
        }
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("ApiOne")
                {
                   Scopes = new []{ "ApiOne" }
                }
            };

        public static IEnumerable<Client> GetClient() =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client_id",
                    ClientSecrets={new Secret("clinet_secret_123".ToSha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"ApiOne"},

                }
            };
    }
}
