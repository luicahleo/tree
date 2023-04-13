using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.WorkOrders
{
    public class WorkOrderTrackingDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Comment { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        public DateTime? LastModificationDate { get; set; }

        [Editable(false)]
        public string CreationUserEmail { get; set; }

        [Editable(false)]
        public string LastModificationUserEmail { get; set; }

        public WorkOrderTrackingDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
        }
    }
}
