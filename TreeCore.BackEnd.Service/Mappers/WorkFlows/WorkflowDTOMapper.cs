using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Mappers.WorkFlows
{
    public class WorkflowDTOMapper : BaseMapper<WorkflowDTO, WorkflowEntity>
    {
        public override Task<WorkflowDTO> Map(WorkflowEntity oEntity)
        {
            WorkflowDTO dto = new WorkflowDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Public = oEntity.Publico,
                Description = oEntity.Descripcion,
                Name = oEntity.Nombre,

            };

            if (oEntity.WorkflowsEstados != null && oEntity.WorkflowsEstados.ToList().Count > 0)
            {
                dto.LinkedStatus = new List<WorkFlowStatusDTO>();
                foreach (WorkFlowStatusEntity linkWorkFlowStatus in oEntity.WorkflowsEstados.ToList())
                {
                    WorkFlowStatusDTO status = new WorkFlowStatusDTO();
                    status.Code = linkWorkFlowStatus.Codigo;
                    status.Name = linkWorkFlowStatus.Nombre;
                    status.Description = linkWorkFlowStatus.Descripcion;
                    status.TimeFrame = linkWorkFlowStatus.Tiempo;
                    status.Complete = linkWorkFlowStatus.Completado;
                    status.PublicReading = linkWorkFlowStatus.PublicoLectura;
                    status.PublicWriting = linkWorkFlowStatus.PublicoEscritura;
                    status.StatusGroupCode = linkWorkFlowStatus.EstadosAgrupaciones.Codigo;
                    status.Active = linkWorkFlowStatus.Activo;
                    status.Default = linkWorkFlowStatus.Defecto;

                    status.LinkedWorkFlowNextStatus = new List<WorkFlowNextStatusDTO>();
                    if(linkWorkFlowStatus.EstadosSiguientes != null)
                    {
                        foreach (WorkFlowNextStatusEntity linkNextStatus in linkWorkFlowStatus.EstadosSiguientes)
                        {
                            WorkFlowNextStatusDTO nextstatus = new WorkFlowNextStatusDTO();
                            nextstatus.Default = linkNextStatus.Defecto;
                            nextstatus.WorkFlowStatusCode = (linkNextStatus.WorkFlowNextStatus != null) ? linkNextStatus.WorkFlowNextStatus.Codigo : null;
                            status.LinkedWorkFlowNextStatus.Add(nextstatus);
                        }
                    }

                    dto.LinkedStatus.Add(status);

                    if (linkWorkFlowStatus.EstadosRolesEscritura != null && linkWorkFlowStatus.EstadosRolesEscritura.ToList().Count > 0)
                    {
                        List<string> listRoles = new List<string>();

                        foreach (RolEntity linkRol in linkWorkFlowStatus.EstadosRolesEscritura.ToList())
                        {
                            listRoles.Add(linkRol.Codigo);
                        }

                        status.LinkedRolesWriting = listRoles;
                    }

                    if (linkWorkFlowStatus.EstadosRolesLectura != null && linkWorkFlowStatus.EstadosRolesLectura.ToList().Count > 0)
                    {
                        List<string> listRoles = new List<string>();

                        foreach (RolEntity linkRol in linkWorkFlowStatus.EstadosRolesLectura.ToList())
                        {
                            listRoles.Add(linkRol.Codigo);
                        }

                        status.LinkedRolesReading = listRoles;
                    }
                }
            }

            if (oEntity.WorkflowsRoles != null && oEntity.WorkflowsRoles.ToList().Count > 0)
            {
                List<string> listRoles = new List<string>();

                foreach (RolEntity linkRol in oEntity.WorkflowsRoles.ToList())
                {
                    listRoles.Add(linkRol.Codigo);
                }

                dto.LinkedRoles = listRoles;
            }

            return Task.FromResult(dto);
        }
    }
}
