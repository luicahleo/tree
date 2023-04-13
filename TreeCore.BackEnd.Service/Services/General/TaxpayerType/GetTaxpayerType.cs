using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class GetTaxpayerType : GetObjectService<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper>
    {
        public GetTaxpayerType(GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor):base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
