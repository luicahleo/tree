using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// TaxIdentificationNumberCategoryController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class TaxIdentificationNumberCategoryController : ApiControllerBase<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity, TaxIdentificationNumberCategoryDTOMapper>, IDeleteController<TaxIdentificationNumberCategoryDTO>, IModController<TaxIdentificationNumberCategoryDTO>
    {

        private readonly PutTaxIdentificationNumberCategory _putTaxIdentificationNumberCategory;
        private readonly PostTaxIdentificationNumberCategory _postTaxIdentificationNumberCategory;
        private readonly DeleteTaxIdentificationNumberCategory _deleteTaxIdentificationNumberCategory;

        public TaxIdentificationNumberCategoryController(GetObjectService<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity, TaxIdentificationNumberCategoryDTOMapper> getObjectService, PutTaxIdentificationNumberCategory putTaxIdentificationNumberCategory, PostTaxIdentificationNumberCategory postTaxIdentificationNumberCategory, DeleteTaxIdentificationNumberCategory deleteTaxIdentificationNumberCategory) 
            : base(getObjectService)
        {
            _putTaxIdentificationNumberCategory = putTaxIdentificationNumberCategory;
            _postTaxIdentificationNumberCategory = postTaxIdentificationNumberCategory;
            _deleteTaxIdentificationNumberCategory = deleteTaxIdentificationNumberCategory;
        }

        /// <summary>
        /// Post TaxIdentificationNumberCategory
        /// </summary>
        /// <returns>List of TaxIdentificationNumberCategory</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<TaxIdentificationNumberCategoryDTO>> Post(TaxIdentificationNumberCategoryDTO sapTypeNIF)
        {
            return (await _postTaxIdentificationNumberCategory.Create(sapTypeNIF, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of TaxIdentificationNumberCategory
        /// </summary>
        /// <param name="code">Code of TaxIdentificationNumberCategory</param>
        /// <returns>List of TaxIdentificationNumberCategory</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<TaxIdentificationNumberCategoryDTO>> Put(TaxIdentificationNumberCategoryDTO sapTypeNIFDTO, string code)
        {
            return (await _putTaxIdentificationNumberCategory.Update(sapTypeNIFDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of TaxIdentificationNumberCategory
        /// </summary>
        /// <param name="code">Code of TaxIdentificationNumberCategory</param>
        /// <returns>TaxIdentificationNumberCategory</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<TaxIdentificationNumberCategoryDTO>> Delete(string code)
        {
            return (await _deleteTaxIdentificationNumberCategory.Delete(code, Client)).MapDto(x => x);
        }

    }
}
