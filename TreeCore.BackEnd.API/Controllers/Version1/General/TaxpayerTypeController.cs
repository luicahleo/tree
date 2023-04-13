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
    /// TaxpayerTypeController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class TaxpayerTypeController : ApiControllerBase<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper>, IDeleteController<TaxpayerTypeDTO>, IModController<TaxpayerTypeDTO>
    {

        private readonly PutTaxpayerType _putTaxpayerType;
        private readonly PostTaxpayerType _postTaxpayerType;
        private readonly DeleteTaxpayerType _deleteTaxpayerType;

        public TaxpayerTypeController(GetObjectService<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper> getObjectService, PutTaxpayerType putTaxpayerType, PostTaxpayerType postTaxpayerType, DeleteTaxpayerType deleteTaxpayerType)
            : base(getObjectService)
        {
            _putTaxpayerType = putTaxpayerType;
            _postTaxpayerType = postTaxpayerType;
            _deleteTaxpayerType = deleteTaxpayerType;
        }

        /// <summary>
        /// Post TaxpayerType
        /// </summary>
        /// <returns>List of TaxpayerType</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<TaxpayerTypeDTO>> Post(TaxpayerTypeDTO taxpayerType)
        {
            return (await _postTaxpayerType.Create(taxpayerType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of TaxpayerType
        /// </summary>
        /// <param name="code">Code of TaxpayerType</param>
        /// <returns>List of TaxpayerType</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<TaxpayerTypeDTO>> Put(TaxpayerTypeDTO taxpayerTypeDTO, string code)
        {
            return (await _putTaxpayerType.Update(taxpayerTypeDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of TaxpayerType
        /// </summary>
        /// <param name="code">Code of TaxpayerType</param>
        /// <returns>TaxpayerType</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<TaxpayerTypeDTO>> Delete(string code)
        {
            return (await _deleteTaxpayerType.Delete(code, Client)).MapDto(x => x);
        }

    }
}
