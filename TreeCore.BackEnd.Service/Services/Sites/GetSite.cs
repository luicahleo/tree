using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;
namespace TreeCore.BackEnd.Service.Services.Sites
{
    public class GetSite : GetObjectService<SiteDTO, SiteEntity,SiteDTOMapper>
    {

        public GetSite(GetDependencies<SiteDTO, SiteEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
        }
      


    }
}
