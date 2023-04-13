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
    /// ContractTypeController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ContractTypeController : ApiControllerBase<ContractTypeDTO, ContractTypeEntity, ContractTypeDTOMapper>, IDeleteController<ContractTypeDTO>, IModController<ContractTypeDTO>
    {

        private readonly PutContractType _putContractType;
        private readonly PostContractType _postContractType;
        private readonly DeleteContractType _deleteContractType;

        public ContractTypeController(GetObjectService<ContractTypeDTO, ContractTypeEntity, ContractTypeDTOMapper> getObjectService, PutContractType putContractType, PostContractType postContractType, DeleteContractType deleteContractType)
            : base(getObjectService)
        {
            _putContractType = putContractType;
            _postContractType = postContractType;
            _deleteContractType = deleteContractType;
        }

        /// <summary>
        /// Post Product Type
        /// </summary>
        /// <returns>List of Product Types</returns>
        [HttpPost("")]
        public async Task<ResultDto<ContractTypeDTO>> Post(ContractTypeDTO contractType)
        {
            return (await _postContractType.Create(contractType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>List of Catalog Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ContractTypeDTO>> Put(ContractTypeDTO contractTypeDTO, string code)
        {
            return (await _putContractType.Update(contractTypeDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>Catalog type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ContractTypeDTO>> Delete(string code)
        {
            return (await _deleteContractType.Delete(code, Client)).MapDto(x => x);
        }

    }
}
