using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// CurrencyController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route (Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class CurrencyController : ApiControllerBase<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper>, IDeleteController<CurrencyDTO>, IModController<CurrencyDTO>
    {
        private readonly PostCurrency _postCurrency;
        private readonly PutCurrency _putCurrency;
        private readonly DeleteCurrency _deleteCurrency;

        public CurrencyController(GetObjectService<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper> getObjectService, PostCurrency postCurrency, 
            PutCurrency putCurrency, DeleteCurrency deleteCurrency) 
            : base(getObjectService)
        {
            _postCurrency = postCurrency;
            _putCurrency = putCurrency;
            _deleteCurrency = deleteCurrency;
        }

        /// <summary>
        /// Post Currency
        /// </summary>
        /// <returns>List of Currencies</returns>
        [HttpPost("")]
        public async Task<ResultDto<CurrencyDTO>> Post(CurrencyDTO currency)
        {
            return (await _postCurrency.Create(currency, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put Currency 
        /// </summary>
        /// <param name="codeCurrency">Code of Currency</param>
        /// <returns>List of Currencies</returns>
        [HttpPut("{codeCurrency}")]
        public async Task<ResultDto<CurrencyDTO>> Put(CurrencyDTO currency, string codeCurrency)
        {
            return (await _putCurrency.Update(currency, codeCurrency, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Currency
        /// </summary>
        /// <param name="code">Code of Currency</param>
        /// <returns>Currency</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<CurrencyDTO>> Delete(string code)
        {
            return (await _deleteCurrency.Delete(code, Client)).MapDto(x => x);
        }
    }
}
