using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace CustomCommandBot.Server
{
    public static class IdentityConfig
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<IdentityServer4.Models.Client> Clients =>
            new List<IdentityServer4.Models.Client>
            {
                new IdentityServer4.Models.Client()
                {
                    ClientId = "client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = { "api1" }
                }
            };
    }
}
