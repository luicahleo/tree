using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General.Inflation
{
    public class DeleteInflation : DeleteObjectService<InflationDTO, InflationEntity, InflationDTOMapper>
    {
        GetDependencies<InflationDTO, InflationEntity> _getDependencies;
        public DeleteInflation(DeleteDependencies<InflationEntity> dependencies, GetDependencies<InflationDTO, InflationEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<InflationDTO>> Delete(string code, int client)
        {
            var InflationIdentty = await _getDependencies.GetItemByCode(code, client);
            Result<InflationEntity> Inflation = (InflationIdentty == null || InflationIdentty.InflacionID == null ?
                Result.Failure<InflationEntity>(Error.Create(_errorTraduccion.NotFound))
                : InflationIdentty);
            if (Inflation.Success)
            {
                var iResult = await DeleteItem(Inflation.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<InflationDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await Inflation.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
