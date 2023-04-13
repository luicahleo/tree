using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.API.Settings
{
    public class BuildAuthentication
    {
        public static string GetAuthorityServer(IConfiguration config)
        {
            Authentication authSettings = new Authentication();
            config.Bind("Authentication", authSettings);

            return authSettings.AuthorityServer;
        }



        public class Authentication
        {
            public string AuthorityServer { get; set; }
        }
    }
}
