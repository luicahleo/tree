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
    /// ContractLineTypeController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ContractLineTypeController : ApiControllerBase<ContractLineTypeDTO, ContractLineTypeEntity, ContractLineTypeDTOMapper>, IDeleteController<ContractLineTypeDTO>, IModController<ContractLineTypeDTO>
    {

        private readonly PutContractLineType _putContractLineType;
        private readonly PostContractLineType _postContractLineType;
        private readonly DeleteContractLineType _deleteContractLineType;

        public ContractLineTypeController(GetObjectService<ContractLineTypeDTO, ContractLineTypeEntity, ContractLineTypeDTOMapper> getObjectService, PutContractLineType putContractLineType, PostContractLineType postContractLineType, DeleteContractLineType deleteContractLineType) 
            : base(getObjectService)
        {
            _putContractLineType = putContractLineType;
            _postContractLineType = postContractLineType;
            _deleteContractLineType = deleteContractLineType;
        }

        /// <summary>
        /// Post Product Type
        /// </summary>
        /// <returns>List of Product Types</returns>
        [HttpPost("")]
        public async Task<ResultDto<ContractLineTypeDTO>> Post(ContractLineTypeDTO contractLineType)
        {
            return (await _postContractLineType.Create(contractLineType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>List of Catalog Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ContractLineTypeDTO>> Put(ContractLineTypeDTO contractLineTypeDTO, string code)
        {
            return (await _putContractLineType.Update(contractLineTypeDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>Catalog type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ContractLineTypeDTO>> Delete(string code)
        {
            return (await _deleteContractLineType.Delete(code, Client)).MapDto(x => x);
        }

    }
}
