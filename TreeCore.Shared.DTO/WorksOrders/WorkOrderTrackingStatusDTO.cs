using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.WorkOrders
{
    public class WorkOrderTrackingStatusDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string WorkFlowStatusCode { get; set; }
        [Required]
        public string PreviusWorkOrderTrackingStatusCode { get; set; }
        [Required]
        public string AssignedUserEmail { get; set; }
        
        [Required]
        public string StatusCode { get; set; }

        [Required]
        public List<WorkOrderTrackingDTO> LinkedTrakings { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; }

        [Editable(false)]
        public string CreationUserEmail { get; set; }

        public WorkOrderTrackingStatusDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(WorkFlowStatusCode).ToLower(), "coreworkorderid");
            map.Add(nameof(PreviusWorkOrderTrackingStatusCode).ToLower(), "previuscoreworkordertrakingstatusid");
            map.Add(nameof(AssignedUserEmail).ToLower(), "assignedusuario");
            map.Add(nameof(StatusCode).ToLower(), "coreestadoid");
            map.Add(nameof(CreationDate).ToLower(), "fechacreacion");
            map.Add(nameof(CreationUserEmail).ToLower(), "usuariocreadorid");
        }
    }
}
