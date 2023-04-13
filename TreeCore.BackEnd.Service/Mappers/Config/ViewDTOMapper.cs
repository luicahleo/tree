using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.General;
using Newtonsoft.Json;
using System.Collections.Generic;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.BackEnd.Service.Mappers.Config
{
    public class ViewDTOMapper : BaseMapper<ViewDTO, ViewEntity>
    {
        public override Task<ViewDTO> Map(ViewEntity oEntity)
        {
            ViewDTO dto = new ViewDTO()
            {
                Code = oEntity.Nombre,
                Default = oEntity.Defecto,
                Page = oEntity.Pagina,
                Icon = oEntity.Icono,
                Columns = JsonConvert.DeserializeObject<List<ColumnDTO>>(oEntity.JsonColumnas),
                Filters = JsonConvert.DeserializeObject<List<FilterDTO>>(oEntity.JsonColumnas)
            };
            return Task.FromResult(dto);
        }
    }
    
}
