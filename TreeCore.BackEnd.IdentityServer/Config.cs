﻿using IdentityServer4.Models;
using System.Collections.Generic;

namespace TreeCore.BackEnd.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            BuildClients.GetClients().Result;


            /*new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowAccessTokensViaBrowser = true,
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { 
                        "api1"
                    }
                }
            };*/
    }
}
