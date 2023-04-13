using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.BackEnd.Service.Mappers.WorkOrders
{
    public class WorkOrderDTOMapper : BaseMapper<WorkOrderDTO, WorkOrderEntity>
    {
        public override Task<WorkOrderDTO> Map(WorkOrderEntity oEntity)
        {
            WorkOrderDTO dto = new WorkOrderDTO()
            {
                Code = oEntity.Codigo,
                WorkOrderLifecycleStatusCode = oEntity.CoreWorkOrderLifeCycleStatus.Codigo,
                CustomerCompanyCode = oEntity.EntidadesCustomer.Codigo,
                CustomerLeaderUserEmail = oEntity.UsuariosCustomer.EMail,
                SupplierCompanyCode = oEntity.EntidadesSupplier.Codigo,
                SupplierLeaderUserEmail = oEntity.UsuariosSupplier.EMail,
                StartDate = oEntity.FechaInicio,
                Percentage = oEntity.Porcentaje,
                EndDate = oEntity.FechaFin,
                CreationDate = oEntity.FechaCreacion,
                LastModificationDate = oEntity.FechaUltimaModificacion,
                CreationUserEmail = oEntity.UsuariosCreador.EMail,
                LastModificationUserEmail = oEntity.UsuariosModificador.EMail,
                LinkedWorkOrderTrakingStatus = new List<WorkOrderTrackingStatusDTO>(),
                LinkedAssets = new List<Shared.DTO.General.AssetDTO>()
            };

            if (oEntity.WorkOrderTrackingStatus != null && oEntity.WorkOrderTrackingStatus.ToList().Count > 0)
            {
                foreach (WorkOrderTrackingStatusEntity linkedWorkOrderTrackingStatus in oEntity.WorkOrderTrackingStatus.ToList())
                {
                    WorkOrderTrackingStatusDTO status = new WorkOrderTrackingStatusDTO();
                    status.Code = linkedWorkOrderTrackingStatus.Codigo;
                    status.WorkFlowStatusCode = linkedWorkOrderTrackingStatus.Estado.Codigo;
                    status.PreviusWorkOrderTrackingStatusCode = linkedWorkOrderTrackingStatus.PreviusCoreWorkOrderTrackingStatus.Codigo;
                    status.AssignedUserEmail = linkedWorkOrderTrackingStatus.AssignedUsuario.EMail;

                    dto.LinkedWorkOrderTrakingStatus.Add(status);
                }
            }

            return Task.FromResult(dto);
        }
    }
}
