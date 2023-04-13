using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class GetCurrency : GetObjectService<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper>
    {

        public GetCurrency(GetDependencies<CurrencyDTO, CurrencyEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}

