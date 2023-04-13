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
    /// BankController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class BankController : ApiControllerBase<BankDTO, BankEntity, BankDTOMapper>, IDeleteController<BankDTO>, IModController<BankDTO>
    {

        private readonly PutBank _putBank;
        private readonly PostBank _postBank;
        private readonly DeleteBank _deleteBank;

        public BankController(GetObjectService<BankDTO, BankEntity, BankDTOMapper> getObjectService, PutBank putBank, PostBank postBank, DeleteBank deleteBank) 
            : base(getObjectService)
        {
            _putBank = putBank;
            _postBank = postBank;
            _deleteBank = deleteBank;
        }

        /// <summary>
        /// Post Bank
        /// </summary>
        /// <returns>List of Bank</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<BankDTO>> Post(BankDTO bank)
        {
            return (await _postBank.Create(bank, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Bank
        /// </summary>
        /// <param name="code">Code of Bank</param>
        /// <returns>List of Bank</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<BankDTO>> Put(BankDTO bankDTO, string code)
        {
            return (await _putBank.Update(bankDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Bank
        /// </summary>
        /// <param name="code">Code of Bank</param>
        /// <returns>Bank</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<BankDTO>> Delete(string code)
        {
            return (await _deleteBank.Delete(code, Client)).MapDto(x => x);
        }

    }
}
