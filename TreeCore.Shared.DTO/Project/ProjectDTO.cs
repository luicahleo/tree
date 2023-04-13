using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ValueObject;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.Shared.DTO.Project
{
    public class ProjectDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public string BusinessProcessCode { get; set; }
        [JsonIgnore]
        public BusinessProcessDTO BusinessProcessDTO { get; set; }
        [Required]
        public string ProgramCode { get; set; }
        [JsonIgnore]
        public ProgramDTO ProgramDTO { get; set; }
        [Required]
        public string ProjectLifeCycleStatusCode { get; set; }
        [JsonIgnore]
        public ProjectLifeCycleStatusDTO ProjectLifeCycleStatusDTO { get; set; }

        [Required]
        public BudgetDTO Budget { get; set; }
        
        public List<WorkOrderDTO> LinkedWorkOrders { get; set; }

        [JsonIgnore]
        public WorkOrderDTO WorkOrderDTO { get; set; }
        //[Required]
        //public List<AssetDTO> LinkedAssets { get; set; }

        [JsonIgnore]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; }

        [JsonIgnore]
        [DataType(DataType.Date)]
        public DateTime? LastModificationDate { get; set; }

        [JsonIgnore]
        public string CreationUserEmail { get; set; }

        [JsonIgnore]
        public string LastModificationUserEmail { get; set; }

        public ProjectDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
        }
    }
}
