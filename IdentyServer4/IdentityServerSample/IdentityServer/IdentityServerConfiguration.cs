using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{

    //https://nahidfa.com/posts/migrating-identityserver4-to-v4/
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources () =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
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
                },
                new ApiResource("ApiTwo")
                {
                   Scopes = new []{ "ApiTwo" }
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

                },
                new Client
                {
                    ClientId="client_id_mvc",
                    ClientSecrets={new Secret("clinet_secret_mvc_123".ToSha256())},
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris={"https://localhost:44352/signin-oidc" },
                    AllowedScopes={"ApiOne",
                                   "ApiTwo",
                                   IdentityServerConstants.StandardScopes.OpenId,
                                   IdentityServerConstants.StandardScopes.Profile
                                  },
                    RequireConsent=false
                }
            };
    }
}
