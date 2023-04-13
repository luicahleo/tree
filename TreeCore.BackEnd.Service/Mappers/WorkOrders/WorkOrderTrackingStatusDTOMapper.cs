using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.BackEnd.Service.Mappers.WorkOrders
{
    public class WorkOrderTrackingStatusDTOMapper : BaseMapper<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity>
    {
        public override Task<WorkOrderTrackingStatusDTO> Map(WorkOrderTrackingStatusEntity oEntity)
        {
            WorkOrderTrackingStatusDTO dto = new WorkOrderTrackingStatusDTO()
            {
                Code = oEntity.Codigo,
                WorkFlowStatusCode = oEntity.WorkOrders.Codigo,
                PreviusWorkOrderTrackingStatusCode = oEntity.PreviusCoreWorkOrderTrackingStatus.Codigo,
                AssignedUserEmail = oEntity.AssignedUsuario.EMail,
                StatusCode = oEntity.Estado.Codigo,
                CreationDate = oEntity.FechaCreaccion,
                CreationUserEmail = oEntity.UsuarioCreador.EMail
            };

            return Task.FromResult(dto);
        }
    }
}