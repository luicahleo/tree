using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Inventory;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Inventory;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;
namespace TreeCore.BackEnd.Service.Services.Inventory
{
    public class GetInventory : GetObjectService<InventoryDTO, InventoryEntity,InventoryDTOMapper>
    {

        public GetInventory(GetDependencies<InventoryDTO, InventoryEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
        }
      


    }
}
