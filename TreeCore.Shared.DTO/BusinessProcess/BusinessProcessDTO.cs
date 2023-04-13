using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.Shared.DTO.BusinessProcess
{
    public class BusinessProcessDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public string BusinessProcessTypeCode { get; set; }
        [JsonIgnore] public BusinessProcessTypeDTO BusinessProcessTypeDTO { get; }       
        [JsonIgnore] public WorkflowDTO WorkflowDTO { get; }
        [Required] public string InitialWorkflowCode { get; set; }
        [Required] public string InitialWorkflowStatusCode { get; set; } 
        [JsonIgnore] public WorkFlowStatusDTO WorkFlowStatusDTO { get; }
        [Required] public List<string> LinkedWorkflows { get; set; }

        public BusinessProcessDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(BusinessProcessTypeCode).ToLower(), "corebusinessprocesstipoid");
        }
    }
}
