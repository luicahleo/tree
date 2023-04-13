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
    /// CompanyController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class CompanyController : ApiControllerBase<CompanyDTO, CompanyEntity, CompanyDTOMapper>, IDeleteController<CompanyDTO>, IModController<CompanyDTO>
    {

        private readonly PutCompany _putCompany;
        private readonly PostCompany _postCompany;
        private readonly GetCompany _getCompany;
        private readonly DeleteCompany _deleteCompany;

        public CompanyController(GetObjectService<CompanyDTO, CompanyEntity, CompanyDTOMapper> getObjectService, GetCompany getCompany, PostCompany postCompany, PutCompany putCompany, DeleteCompany deleteCompany) 
            : base(getObjectService)
        {
            _getCompany = getCompany;
            _postCompany = postCompany;
            _putCompany = putCompany;
            _deleteCompany = deleteCompany;
        }

        /// <summary>
        /// Post Company 
        /// </summary>
        /// <returns>Company</returns>
        [HttpPost("")]
        public async Task<ResultDto<CompanyDTO>> Post(CompanyDTO company)
        {
            return (await _postCompany.Create(company, Client,EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Get Details Company By Code
        /// </summary>
        /// <param name="code">Code of Company</param>
        /// <returns>Details Company</returns>
        [HttpGet("Details/{code}")]
        public async Task<ResultDto<CompanyDetailsDTO>> GetDetails(string code)
        {
            return (await _getCompany.GetItemDetailsByCode(code, Client)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Company s
        /// </summary>
        /// <param name="code">Code of Company </param>
        /// <returns>List of Company s</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<CompanyDTO>> Put(CompanyDTO CompanyDTO, string code)
        {
            return (await _putCompany.Update(CompanyDTO, code, Client,EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Company
        /// </summary>
        /// <param name="code">Code of Company</param>
        /// <returns>Company</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<CompanyDTO>> Delete(string code)
        {
            return (await _deleteCompany.Delete(code, Client)).MapDto(x => x);
        }

    }
}
