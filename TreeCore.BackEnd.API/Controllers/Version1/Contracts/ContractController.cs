using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Contracts
{
    /// <summary>
    /// ContractController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ContractController : ApiControllerBase<ContractDTO, ContractEntity, ContractDTOMapper>, IDeleteController<ContractDTO>, IModController<ContractDTO>
    {

        private readonly PutContract _putContract;
        private readonly PostContract _postContract;
        private readonly DeleteContract _deleteContract;
        private readonly GetContract _getContract;

        public ContractController(GetObjectService<ContractDTO, ContractEntity, ContractDTOMapper> getObjectService, PutContract putContract, PostContract postContract, DeleteContract deleteContract) 
            : base(getObjectService)
        {
            _putContract = putContract;
            _postContract = postContract;
            _deleteContract = deleteContract;
        }

        /// <summary>
        /// Post Contract 
        /// </summary>
        /// <returns>List of Contract </returns>
        [HttpPost("")]
        public async Task<ResultDto<ContractDTO>> Post(ContractDTO contract)
        {
            return (await _postContract.Create(contract, Client, EmailUser)).MapDto(x => x);
        }


        ///// <summary>
        ///// Get Site Contract By Code
        ///// </summary>
        ///// <param name="code">Code of Company</param>
        ///// <returns>Details Company</returns>
        //[HttpGet("Details/{code}")]
        //public async Task<ResultDto<SiteDTO>> GetDetails(string code)
        //{
        //    return (await _getContract.GetItemDetailsByCode(code, Client)).MapDto(x => x);
        //}

        /// <summary>
        /// Put object of Contract 
        /// </summary>
        /// <param name="code">Code of Contract </param>
        /// <returns>List of Contract </returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ContractDTO>> Put(ContractDTO contractDTO, string code)
        {
            return (await _putContract.Update(contractDTO, code, Client,EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Contract 
        /// </summary>
        /// <param name="code">Code of Contract </param>
        /// <returns>Contract </returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ContractDTO>> Delete(string code)
        {
            return (await _deleteContract.Delete(code, Client)).MapDto(x => x);
        }

    }
}
