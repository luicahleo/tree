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

namespace TreeCore.BackEnd.Service.Services.Project
{

    public class DeleteProject : DeleteObjectService<ProjectDTO, ProjectEntity, ProjectDTOMapper>
    {
        GetDependencies<ProjectDTO, ProjectEntity> _getDependencies;
        public DeleteProject(DeleteDependencies<ProjectEntity> dependencies, GetDependencies<ProjectDTO, ProjectEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ProjectDTO>> Delete(string sCode, int Client)
        {
            var WorkOrderTrackingStatusIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ProjectEntity> trackingStatus = (WorkOrderTrackingStatusIdenty == null || WorkOrderTrackingStatusIdenty.CoreProjectID == null ?
                Result.Failure<ProjectEntity>(Error.Create(_errorTraduccion.NotFound))
                : WorkOrderTrackingStatusIdenty);
            if (trackingStatus.Success)
            {
                var iResult = await DeleteItem(trackingStatus.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ProjectDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await trackingStatus.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
