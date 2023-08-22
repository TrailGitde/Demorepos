using IdentityServer4.Models;
using System.Collections.Generic;

namespace Framework.Authentication
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("myresourceapi","MyResource API")
                {
                    Scopes={new Scope("apiscope")}
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId="secret_client_id",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "apiscope"}
                }
            };
        }
    }
}
