using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Program
{

    public class DeleteProgram : DeleteObjectService<ProgramDTO, ProgramEntity, ProgramDTOMapper>
    {
        private readonly GetDependencies<ProgramDTO, ProgramEntity> _getDependencies;
        public DeleteProgram(DeleteDependencies<ProgramEntity> dependencies, GetDependencies<ProgramDTO, ProgramEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ProgramDTO>> Delete(string sCode, int Client)
        {
            var programIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ProgramEntity> trackingStatus = (programIdenty == null || programIdenty.CoreProgramID == null ?
                Result.Failure<ProgramEntity>(Error.Create(_errorTraduccion.NotFound))
                : programIdenty);
            if (trackingStatus.Success)
            {
                var iResult = await DeleteItem(trackingStatus.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ProgramDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await trackingStatus.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
