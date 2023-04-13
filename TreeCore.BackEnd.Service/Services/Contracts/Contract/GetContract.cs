using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.ROP;


namespace TreeCore.BackEnd.Service.Services.Contracts
{
    public class GetContract : GetObjectService<ContractDTO, ContractEntity, ContractDTOMapper>
    {
      
        private readonly GetDependencies<SiteDTO, ContractEntity> _getDependencySites;
      
        public GetContract(GetDependencies<ContractDTO, ContractEntity> getDependencies,
             GetDependencies<SiteDTO, ContractEntity> getDependencySites,
              IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            
            
            _getDependencySites = getDependencySites;
            
        }
        public GetContract(GetDependencies<ContractDTO, ContractEntity> getDependencies,IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {


            

        }

        

        public async Task<Result<SiteDTO>> GetItemDetailsByCode(string code, int Client)
        {

            var ContractEntity = await _getDependencySites.GetItemByCode(code, Client);
            Result<ContractEntity> contract = (ContractEntity == null || ContractEntity.AlquilerID == null ?
                Result.Failure<ContractEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractEntity);

            return await contract.Async()
                .MapAsync(x => MapDetails(x));
        }


        private Task<SiteDTO> MapDetails(ContractEntity contract)
        {
            SiteDTO dto = new SiteDTO()
            {
                
                Code = contract.oSite.Codigo
               
            };
            return Task.FromResult(dto);
        }
    }
}
