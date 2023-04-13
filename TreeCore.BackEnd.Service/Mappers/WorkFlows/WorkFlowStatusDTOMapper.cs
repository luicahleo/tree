using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Mappers.WorkFlows
{
    public class WorkFlowStatusDTOMapper : BaseMapper<WorkFlowStatusDTO, WorkFlowStatusEntity>
    {
        public override Task<WorkFlowStatusDTO> Map(WorkFlowStatusEntity oEntity)
        {
            List<WorkFlowNextStatusDTO> listNextSatus = new List<WorkFlowNextStatusDTO>();
            if (oEntity.EstadosSiguientes != null)
            {
                foreach (WorkFlowNextStatusEntity linkNextStatus in oEntity.EstadosSiguientes)
                {
                    WorkFlowNextStatusDTO nextstatus = new WorkFlowNextStatusDTO();
                    nextstatus.Default = linkNextStatus.Defecto;
                    nextstatus.WorkFlowStatusCode = (linkNextStatus.WorkFlowNextStatus != null) ? linkNextStatus.WorkFlowNextStatus.Codigo : null;
                    listNextSatus.Add(nextstatus);
                }
            }

            WorkFlowStatusDTO dto = new WorkFlowStatusDTO()
            {
                Code = oEntity.Codigo,
                Name = oEntity.Nombre,
                Description = oEntity.Descripcion,
                TimeFrame = oEntity.Tiempo,
                Complete = oEntity.Completado,
                PublicReading = oEntity.PublicoLectura,
                PublicWriting = oEntity.PublicoEscritura,
                StatusGroupCode = (oEntity.EstadosAgrupaciones != null) ? oEntity.EstadosAgrupaciones.Codigo : null,
                Active = oEntity.Activo,
                Default = oEntity.Defecto,
                LinkedWorkFlowNextStatus = listNextSatus
            };

            if (oEntity.EstadosRolesEscritura != null && oEntity.EstadosRolesEscritura.ToList().Count > 0)
            {
                List<string> listRoles = new List<string>();

                foreach (RolEntity linkRol in oEntity.EstadosRolesEscritura.ToList())
                {
                    listRoles.Add(linkRol.Codigo);
                }

                dto.LinkedRolesWriting = listRoles;
            }

            if (oEntity.EstadosRolesLectura != null && oEntity.EstadosRolesLectura.ToList().Count > 0)
            {
                List<string> listRoles = new List<string>();

                foreach (RolEntity linkRol in oEntity.EstadosRolesLectura.ToList())
                {
                    listRoles.Add(linkRol.Codigo);
                }

                dto.LinkedRolesReading = listRoles;
            }

            return Task.FromResult(dto);
        }
    }
}

