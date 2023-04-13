using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;

namespace TreeCore.Shared.DTO.WorkOrders
{
    public class WorkOrderDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string WorkOrderLifecycleStatusCode { get; set; }
        [JsonIgnore] public WorkOrderLifecycleStatusDTO WorkOrderLifecycleStatusDTO { get; }
        [Required] public string CustomerCompanyCode { get; set; }
        [JsonIgnore] public CompanyDTO CompanyDTO { get; }
        [Required] public string CustomerLeaderUserEmail { get; set; }
        [JsonIgnore] public UserDTO UserDTO { get; }
        [Required] public string SupplierCompanyCode { get; set; }
        [Required] public string SupplierLeaderUserEmail { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public List<WorkOrderTrackingStatusDTO> LinkedWorkOrderTrakingStatus { get; set; }
        [JsonIgnore] public WorkOrderTrackingStatusDTO WorkOrderTrakingStatusDTO { get; }
        [Required] public List<AssetDTO> LinkedAssets { get; set; }
        [JsonIgnore] public AssetDTO AssetDTO { get; }
        [JsonIgnore] public float Percentage { get; set; }
        [JsonIgnore] [DataType(DataType.Date)] public DateTime? EndDate { get; set; }
        [JsonIgnore] [DataType(DataType.Date)] public DateTime? CreationDate { get; set; }
        [JsonIgnore] [DataType(DataType.Date)] public DateTime? LastModificationDate { get; set; }
        [JsonIgnore] public string CreationUserEmail { get; set; }
        [JsonIgnore] public string LastModificationUserEmail { get; set; }

        public WorkOrderDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(WorkOrderLifecycleStatusCode).ToLower(), "coreworkorderlifecyclestatusid");
            map.Add(nameof(CustomerCompanyCode).ToLower(), "customerentidadid");
            map.Add(nameof(CustomerLeaderUserEmail).ToLower(), "customerusuarioid");
            map.Add(nameof(SupplierCompanyCode).ToLower(), "supplierentidadid");
            map.Add(nameof(SupplierLeaderUserEmail).ToLower(), "supplierusuarioid");
            map.Add(nameof(StartDate).ToLower(), "fechainicio");
            map.Add(nameof(LinkedAssets).ToLower(), "activos");
            map.Add(nameof(Percentage).ToLower(), "porcentaje");
            map.Add(nameof(EndDate).ToLower(), "fechafin");
            map.Add(nameof(CreationDate).ToLower(), "fechacreacion");
            map.Add(nameof(LastModificationDate).ToLower(), "fechamodificacion");
            map.Add(nameof(CreationUserEmail).ToLower(), "usuariocreadorid");
            map.Add(nameof(LastModificationUserEmail).ToLower(), "usuariomodificadorid");
        }
    }
}
