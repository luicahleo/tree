using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers.General;

namespace TreeCore.BackEnd.IdentityServer
{
    public class BuildClients
    {
        protected readonly GetObjectService<UserDTO,UserEntity,UserDTOMapper> _getObjectService;
        public BuildClients(GetObjectService<UserDTO,UserEntity,UserDTOMapper> getObjectService)
        {
            _getObjectService = getObjectService;
        }
        public static async Task<List<Client>> GetClients()
        {
            List<Client> clients = new List<Client>();

            List<Filter> filters = new List<Filter>() {
                new Filter(nameof(UserDTO.Active), nameof(Operators.eq).ToString(), true)
            };
            List<string> orders = new List<string>();



            //ResultDto<IEnumerable<UserEntity>> a = (await _getObjectService.GetList(filters, orders, -1, -1)).MapDto(x => x);




            clients.Add(new Client
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
            });
            return clients;
        }

    }
}
