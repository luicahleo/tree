using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.Shared.DTO.Project;

namespace TreeCore.BackEnd.Service.Mappers.Project
{
    public class ProjectLifeCycleStatusDTOMapper : BaseMapper<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity>
    {
        public override Task<ProjectLifeCycleStatusDTO> Map(ProjectLifeCycleStatusEntity projectLifeCycleStatusEntity)
        {
            ProjectLifeCycleStatusDTO dto = new ProjectLifeCycleStatusDTO()
            {
                Code = projectLifeCycleStatusEntity.Codigo,
                Name = projectLifeCycleStatusEntity.Nombre,
                Description = projectLifeCycleStatusEntity.Descripcion,
                Active = projectLifeCycleStatusEntity.Activo,
                Colour = projectLifeCycleStatusEntity.Color
            };
            return Task.FromResult(dto);
        }
    }
}
