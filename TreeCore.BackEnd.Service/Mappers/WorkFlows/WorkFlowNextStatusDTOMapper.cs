using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Mappers.WorkFlows
{
    public class WorkFlowNextStatusDTOMapper : BaseMapper<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity>
    {
        public override Task<WorkFlowNextStatusDTO> Map(WorkFlowNextStatusEntity WorkFlowNextStatus)
        {
            WorkFlowNextStatusDTO dto = new WorkFlowNextStatusDTO()
            {
                Default = WorkFlowNextStatus.Defecto,
                WorkFlowStatusCode = WorkFlowNextStatus.WorkFlowStatus.Codigo
            };

            
            return Task.FromResult(dto);
        }
    }
}
