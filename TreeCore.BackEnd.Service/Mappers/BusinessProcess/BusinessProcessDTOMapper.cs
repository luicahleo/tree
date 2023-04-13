using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.DTO.BusinessProcess;

namespace TreeCore.BackEnd.Service.Mappers.BusinessProcess
{
    public class BusinessProcessDTOMapper : BaseMapper<BusinessProcessDTO, BusinessProcessEntity>
    {
        public override Task<BusinessProcessDTO> Map(BusinessProcessEntity oBusinessProcess)
        {
            List<string> listWorkflows = new List<string>();
            if (oBusinessProcess.WorkflowsVinculados != null)
            {
                foreach (WorkflowEntity oWorkflow in oBusinessProcess.WorkflowsVinculados)
                {
                    listWorkflows.Add(oWorkflow.Codigo);
                }
            }

            BusinessProcessDTO dto = new BusinessProcessDTO()
            {
                Active = oBusinessProcess.Activo,
                Code = oBusinessProcess.Codigo,
                Description = oBusinessProcess.Descripcion,
                Name = oBusinessProcess.Nombre,
                InitialWorkflowStatusCode = (oBusinessProcess.CoreEstados != null) ? oBusinessProcess.CoreEstados.Codigo : null,
                InitialWorkflowCode = (oBusinessProcess.CoreEstados != null && oBusinessProcess.CoreEstados.WorkFlow != null) ? oBusinessProcess.CoreEstados.WorkFlow.Codigo : null,
                LinkedWorkflows = listWorkflows,
                BusinessProcessTypeCode = (oBusinessProcess.CoreBusinessProcessTipos != null) ? oBusinessProcess.CoreBusinessProcessTipos.Codigo : null
            };

            return Task.FromResult(dto);
        }
    }
}
