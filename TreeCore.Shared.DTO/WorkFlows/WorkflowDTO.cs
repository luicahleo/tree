using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TreeCore.Shared.DTO.General;

namespace TreeCore.Shared.DTO.WorkFlows
{
    public class WorkflowDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public bool Public { get; set; }
        [Required] public List<WorkFlowStatusDTO> LinkedStatus { get; set; }
        [JsonIgnore] public WorkFlowStatusDTO WorkFlowStatusDTO { get; }
        [Required] public List<string> LinkedRoles { get; set; }
        [JsonIgnore] public RolDTO RolDTO { get; }

        public WorkflowDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Public).ToLower(), "publico");
            map.Add(nameof(LinkedStatus).ToLower(), "estadosVinculados");
            map.Add(nameof(LinkedRoles).ToLower(), "rolesVinculados");
        }
    }
}
