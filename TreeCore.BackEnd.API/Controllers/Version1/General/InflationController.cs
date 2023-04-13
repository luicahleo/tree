using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services.General.Inflation;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// InflationController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class InflationController : ApiControllerBase<InflationDTO, InflationEntity, InflationDTOMapper>, IDeleteController<InflationDTO>, IModController<InflationDTO>
    {
        private readonly PostInflation _postInflation;
        private readonly PutInflation _putInflation;
        private readonly GetInflation _getInflation;
        private readonly DeleteInflation _deleteInflation;

        public InflationController (GetInflation getInflation, PutInflation putInflation,
            PostInflation postInflation, DeleteInflation deleteInflation) : base(getInflation)
        {
            _postInflation = postInflation;
            _putInflation = putInflation;
            _getInflation = getInflation;
            _deleteInflation = deleteInflation;
        }

        /// <summary>
        /// Post Inflation
        /// </summary>
        /// <returns>List of Inflations</returns>
        [HttpPost("")]
        public async Task<ResultDto<InflationDTO>> Post(InflationDTO inflation)
        {
            return (await _postInflation.Create(inflation, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put Inflation
        /// </summary>
        /// <param name="code">Code of Inflation</param>
        /// <returns>List of Inflations</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<InflationDTO>> Put(InflationDTO inflation, string code)
        {
            return (await _putInflation.Update(inflation, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Inflation
        /// </summary>
        /// <param name="code">Code of Inflation</param>
        /// <returns>Inflation</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<InflationDTO>> Delete(string code)
        {
            return (await _deleteInflation.Delete(code, Client)).MapDto(x => x);
        }
    }
}
