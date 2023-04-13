using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.Shared.DTO.Sites;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class SiteDTOMapper : BaseMapper<SiteDTO, SiteEntity>
    {
        public override Task<SiteDTO> Map(SiteEntity Site)
        {
            SiteDTO dto = new SiteDTO()
            {

                Code = Site.Codigo,

            };
            return Task.FromResult(dto);
        }
    }
}
