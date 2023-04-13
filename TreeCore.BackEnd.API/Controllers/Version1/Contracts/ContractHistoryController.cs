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
    /// ContractHistoryController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ContractHistoryController : ApiControllerBase<ContractHistoryDTO, ContractHistoryEntity, ContractHistoryDTOMapper>, IDeleteController<ContractHistoryDTO>, IModController<ContractHistoryDTO>
    {

      

        public ContractHistoryController(GetObjectService<ContractHistoryDTO, ContractHistoryEntity, ContractHistoryDTOMapper> getObjectService)
            : base(getObjectService)
        {
           
        }
        /// <summary>
        /// Delete object of Contract History
        /// </summary>
        /// <param name="code">Code of Contract History</param>
        /// <returns>Contract History</returns>
        [HttpDelete("{code}")]
        public Task<ResultDto<ContractHistoryDTO>> Delete(string code)
        {
            throw new System.NotImplementedException();
        }
        // <summary>
        /// Contract History
        /// </summary>
        /// <returns>List of Contract History</returns>
        [HttpPost("")]
        public Task<ResultDto<ContractHistoryDTO>> Post(ContractHistoryDTO oDTO)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Put object of Contract History
        /// </summary>
        /// <param name="code">Code of Contract History</param>
        /// <returns>List of Contract History</returns>
        [HttpPut("{id}")]
        public Task<ResultDto<ContractHistoryDTO>> Put(ContractHistoryDTO oDTO, string sCode)
        {
            throw new System.NotImplementedException();
        }
    }
}
