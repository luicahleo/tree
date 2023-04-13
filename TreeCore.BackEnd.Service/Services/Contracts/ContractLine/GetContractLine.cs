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
    public class GetContractLine : GetObjectService<ContractLineDTO, ContractLineEntity, ContractLineDTOMapper>
    {
      
        
      
        public GetContractLine(GetDependencies<ContractLineDTO, ContractLineEntity> getDependencies,
            
              IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            
            
           
            
        }
       

        

      
    }
}
