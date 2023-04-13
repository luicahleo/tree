using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.Config;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Config
{
    public class GetView : GetObjectService<ViewDTO, ViewEntity, ViewDTOMapper>
    {
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        public GetView(GetDependencies<ViewDTO, ViewEntity> getDependencies,
            GetDependencies<UserDTO, UserEntity> getUserDependency, IHttpContextAccessor httpcontextAccessor):base(httpcontextAccessor, getDependencies)
        {
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<IEnumerable<ViewDTO>>> GetList(int Client, string Email, List<Filter> filters = null, List<string> orders = null, string direction = null, int pageSize = -1, int pageIndex = -1)
        {
            var user = await _getUserDependency.GetItemByCode(Email, Client);
            filters.Add(new Filter(nameof(ViewEntity.UsuarioID), Operators.eq, user.UsuarioID));
            var oEntityList = await _getDependencies.GetList(Client, filters, orders, direction, pageSize, pageIndex);
            Result<IEnumerable<ViewEntity>> entityList = (oEntityList == null ?
                Result.Failure<IEnumerable<ViewEntity>>(Error.Create(_errorTraduccion.NotFound)) :
                new Result<IEnumerable<ViewEntity>>(oEntityList));
            List<ViewDTO> lista = new List<ViewDTO>();
            foreach (var item in entityList.Valor)
            {
                lista.Add(await _mapper.Map(item));
            }
            int totalItems = (entityList.Valor.FirstOrDefault() != null) ? entityList.Valor.FirstOrDefault().TotalItems : 0;
            return new Result<IEnumerable<ViewDTO>>(lista, totalItems);
        }

    }
}
