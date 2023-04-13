using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class GetCountry : GetObjectService<CountryDTO, CountryEntity, CountryDTOMapper>
    {

        public GetCountry(GetDependencies<CountryDTO, CountryEntity> getDependencies, IHttpContextAccessor httpcontextAccessor)
            : base(httpcontextAccessor, getDependencies) {}
    }
}


