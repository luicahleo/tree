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
    /// ContractGroupController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ContractGroupController : ApiControllerBase<ContractGroupDTO, ContractGroupEntity, ContractGroupDTOMapper>, IDeleteController<ContractGroupDTO>, IModController<ContractGroupDTO>
    {

        private readonly PutContractGroup _putContractGroup;
        private readonly PostContractGroup _postContractGroup;
        private readonly DeleteContractGroup _deleteContractGroup;

        public ContractGroupController(GetObjectService<ContractGroupDTO, ContractGroupEntity, ContractGroupDTOMapper> getObjectService, PutContractGroup putContractGroup, PostContractGroup postContractGroup, DeleteContractGroup deleteContractGroup) 
            : base(getObjectService)
        {
            _putContractGroup = putContractGroup;
            _postContractGroup = postContractGroup;
            _deleteContractGroup = deleteContractGroup;
        }

        /// <summary>
        /// Post Contract Group
        /// </summary>
        /// <returns>List of Contract Group</returns>
        [HttpPost("")]
        public async Task<ResultDto<ContractGroupDTO>> Post(ContractGroupDTO contractGroup)
        {
            return (await _postContractGroup.Create(contractGroup, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Contract Group
        /// </summary>
        /// <param name="code">Code of Contract Group</param>
        /// <returns>List of Contract Group</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ContractGroupDTO>> Put(ContractGroupDTO contractGroupDTO, string code)
        {
            return (await _putContractGroup.Update(contractGroupDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Contract Group
        /// </summary>
        /// <param name="code">Code of Contract Group</param>
        /// <returns>Contract Group</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ContractGroupDTO>> Delete(string code)
        {
            return (await _deleteContractGroup.Delete(code, Client)).MapDto(x => x);
        }

    }
}
