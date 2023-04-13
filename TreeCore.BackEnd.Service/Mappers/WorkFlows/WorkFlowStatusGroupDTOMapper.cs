using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Mappers.WorkFlows
{
    public class WorkFlowStatusGroupDTOMapper : BaseMapper<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity>
    {
        public override Task<WorkFlowStatusGroupDTO> Map(WorkFlowStatusGroupEntity oEntity)
        {
            WorkFlowStatusGroupDTO dto = new WorkFlowStatusGroupDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Name = oEntity.Nombre
            };
            return Task.FromResult(dto);
        }
    }
}

