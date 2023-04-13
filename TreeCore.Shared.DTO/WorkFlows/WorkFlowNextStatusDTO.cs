using System.Collections.Generic;

namespace TreeCore.Shared.DTO.WorkFlows
{
    public class WorkFlowNextStatusDTO : BaseDTO
    {
        public string WorkFlowStatusCode { get; set; }
        public bool Default { get; set; }

        public WorkFlowNextStatusDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(WorkFlowStatusCode).ToLower(), "coreestadoposibleID");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}
