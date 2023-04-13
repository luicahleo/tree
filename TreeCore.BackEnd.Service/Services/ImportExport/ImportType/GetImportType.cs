using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;

namespace TreeCore.BackEnd.Service.Services.ImportExport
{
    public class GetImportType : GetObjectService<ImportTypeDTO, ImportTypeEntity, ImportTypeDTOMapper>
    {

        public GetImportType(GetDependencies<ImportTypeDTO, ImportTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}

