using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Companies
{
    /// <summary>
    /// CompanyTypeController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class CompanyTypeController : ApiControllerBase<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper>, IDeleteController<CompanyTypeDTO>, IModController<CompanyTypeDTO>
    {

        private readonly PutCompanyType _putCompanyType;
        private readonly PostCompanyType _postCompanyType;
        private readonly DeleteCompanyType _deleteCompanyType;

        public CompanyTypeController(GetObjectService<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper> getObjectService, PutCompanyType putCompanyType, PostCompanyType postCatalogType, DeleteCompanyType deleteCompanyType) 
            : base(getObjectService)
        {
            _putCompanyType = putCompanyType;
            _postCompanyType = postCatalogType;
            _deleteCompanyType = deleteCompanyType;
        }

        /// <summary>
        /// Delete object of Company Types
        /// </summary>
        /// <param name="code">Code of Company Type</param>
        /// <returns>Company type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<CompanyTypeDTO>> Delete(string code)
        {
            return (await _deleteCompanyType.Delete(code, Client)).MapDto(x => x);
        }


        /// <summary>
        /// Post Company Type
        /// </summary>
        /// <returns>Company Types</returns>
        [HttpPost("")]
        public async Task<ResultDto<CompanyTypeDTO>> Post(CompanyTypeDTO CompanyType)
        {
            return (await _postCompanyType.Create(CompanyType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Company Types
        /// </summary>
        /// <param name="code">Code of Company Type</param>
        /// <returns>List of Company Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<CompanyTypeDTO>> Put(CompanyTypeDTO CompanyTypeDTO, string code)
        {
            return (await _putCompanyType.Update(CompanyTypeDTO, code, Client, EmailUser)).MapDto(x => x);
        }

    }
}
