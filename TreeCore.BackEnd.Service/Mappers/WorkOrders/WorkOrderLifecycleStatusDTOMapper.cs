using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.BackEnd.Service.Mappers.WorkOrders
{
    public class WorkOrderLifecycleStatusDTOMapper : BaseMapper<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity>
    {
        public override Task<WorkOrderLifecycleStatusDTO> Map(WorkOrderLifecycleStatusEntity oEntity)
        {
            WorkOrderLifecycleStatusDTO dto = new WorkOrderLifecycleStatusDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Colour = oEntity.Color,
                Name = oEntity.Nombre
            };
            return Task.FromResult(dto);
        }
    }
}

