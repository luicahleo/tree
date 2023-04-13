using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ImportExport;

namespace TreeCore.BackEnd.Service.Services.ImportExport
{
    public class GetImportTask : GetObjectService<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper>
    {

        public GetImportTask(GetDependencies<ImportTaskDTO, ImportTaskEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}

