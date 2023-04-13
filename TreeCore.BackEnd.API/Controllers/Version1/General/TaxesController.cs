using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// TaxesController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class TaxesController : ApiControllerBase<TaxesDTO, TaxesEntity, TaxesDTOMapper>, IDeleteController<TaxesDTO>, IModController<TaxesDTO>
    {

        private readonly PutTaxes _putTaxes;
        private readonly PostTaxes _postTaxes;
        private readonly GetTaxes _getTaxes;
        private readonly DeleteTaxes _deleteTaxes;

        public TaxesController(GetTaxes getTaxes, PutTaxes putTaxes,
            PostTaxes postTaxes, DeleteTaxes deleteTaxes) : base(getTaxes)
        {
            _putTaxes = putTaxes;
            _postTaxes = postTaxes;
            _getTaxes = getTaxes;
            _deleteTaxes = deleteTaxes;
        }

        /// <summary>
        /// Post Product Type
        /// </summary>
        /// <returns>List of Product Types</returns>
        [HttpPost("")]
        public async Task<ResultDto<TaxesDTO>> Post(TaxesDTO taxes)
        {
            return (await _postTaxes.Create(taxes, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>List of Catalog Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<TaxesDTO>> Put(TaxesDTO taxesDTO, string code)
        {
            return (await _putTaxes.Update(taxesDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>Catalog type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<TaxesDTO>> Delete(string code)
        {
            return (await _deleteTaxes.Delete(code, Client)).MapDto(x => x);
        }

    }
}
