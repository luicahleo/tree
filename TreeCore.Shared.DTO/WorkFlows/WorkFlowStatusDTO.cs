using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TreeCore.Shared.DTO.General;

namespace TreeCore.Shared.DTO.WorkFlows
{
    public class WorkFlowStatusDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        [Required] public int TimeFrame { get; set; }
        [Required] public bool Complete { get; set; }
        [Required] public bool Default { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public bool PublicWriting { get; set; }
        [Required] public bool PublicReading { get; set; }
        [Required] public string StatusGroupCode { get; set; }
        [JsonIgnore] public WorkFlowStatusGroupDTO WorkFlowStatusGroupDTO { get; }        
        public List<WorkFlowNextStatusDTO> LinkedWorkFlowNextStatus { get; set; }
        [JsonIgnore] public WorkFlowNextStatusDTO WorkFlowNextStatusDTO { get; }
        [Required] public List<string> LinkedRolesWriting { get; set; }
        [Required] public List<string> LinkedRolesReading { get; set; }
        [JsonIgnore] public RolDTO RolDTO { get; }

        public WorkFlowStatusDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(TimeFrame).ToLower(), "tiempo");
            map.Add(nameof(Complete).ToLower(), "completado");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(PublicWriting).ToLower(), "publicoescritura");
            map.Add(nameof(PublicReading).ToLower(), "publicolectura");
            map.Add(nameof(StatusGroupCode).ToLower(), "estadoagrupacionid");
            map.Add("CoreWorkFlowID".ToLower(), "coreWorkFlowID");
            map.Add(nameof(LinkedRolesWriting).ToLower(), "rolesEscrituraVinculados");
            map.Add(nameof(LinkedRolesReading).ToLower(), "rolesLecturaVinculados");
        }
    }
}
