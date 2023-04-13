using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.Shared.DTO.Project
{
    public class PostProjectDTO : BaseDTO
    {
        [Required] public ProjectDTO ProjectDTO { get; set; }
        [Required] public WorkOrderDTO WorkOrderDTO { get; set; }
    }
}
