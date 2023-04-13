using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Inventory;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.Shared.DTO.Inventory;
using TreeCore.Shared.DTO.Sites;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class InventoryDTOMapper : BaseMapper<InventoryDTO, InventoryEntity>
    {
        public override Task<InventoryDTO> Map(InventoryEntity Site)
        {
            InventoryDTO dto = new InventoryDTO()
            {

                Code = Site.Codigo,

            };
            return Task.FromResult(dto);
        }
    }
}
