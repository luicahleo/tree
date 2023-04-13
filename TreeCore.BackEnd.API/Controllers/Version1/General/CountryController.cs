using Microsoft.AspNetCore.Mvc;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// CountryController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class CountryController : ApiControllerBase<CountryDTO, CountryEntity, CountryDTOMapper>
    {

        public CountryController(GetObjectService<CountryDTO, CountryEntity, CountryDTOMapper> getObjectService) 
            : base(getObjectService)
        {}
    }
}
