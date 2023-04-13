using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Contracts
{
    /// <summary>
    /// ContractStateController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ContractStatusController : ApiControllerBase<ContractStatusDTO, ContractStatusEntity, ContractStatusDTOMapper>, IDeleteController<ContractStatusDTO>, IModController<ContractStatusDTO>
    {

        private readonly PutContractStatus _putContractState;
        private readonly PostContractStatus _postContractState;
        private readonly DeleteContractStatus _deleteContractState;

        public ContractStatusController(GetObjectService<ContractStatusDTO, ContractStatusEntity, ContractStatusDTOMapper> getObjectService, PutContractStatus putContractState, PostContractStatus postContractState, DeleteContractStatus deleteContractState)
            : base(getObjectService)
        {
            _putContractState = putContractState;
            _postContractState = postContractState;
            _deleteContractState = deleteContractState;
        }

        /// <summary>
        /// Post Contract State
        /// </summary>
        /// <returns>List of Contract State</returns>
        [HttpPost("")]
        public async Task<ResultDto<ContractStatusDTO>> Post(ContractStatusDTO contractState)
        {
            return (await _postContractState.Create(contractState, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Contract State
        /// </summary>
        /// <param name="code">Code of Contract State</param>
        /// <returns>List of Contract State</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ContractStatusDTO>> Put(ContractStatusDTO contractStateDTO, string code)
        {
            return (await _putContractState.Update(contractStateDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Contract State
        /// </summary>
        /// <param name="code">Code of Contract State</param>
        /// <returns>Contract State</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ContractStatusDTO>> Delete(string code)
        {
            return (await _deleteContractState.Delete(code, Client)).MapDto(x => x);
        }

    }
}
