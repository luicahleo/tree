using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.ROP;
using TreeCore.Shared.DTO;
using TreeCore.BackEnd.Model.Entity;
using System.Linq;
using TreeCore.BackEnd.Service.Mappers;

namespace TreeCore.BackEnd.Service.Services
{
    public abstract class GetObjectService<DTO, Entity, Mapper> : BaseObjectService<DTO, Entity, Mapper>
        where DTO : BaseDTO
        where Entity : BaseEntity
        where Mapper : BaseMapper<DTO, Entity>
    {
        protected readonly GetDependencies<DTO,Entity> _getDependencies;
        protected GetObjectService(IHttpContextAccessor httpcontextAccessor, GetDependencies<DTO, Entity> getDependencies) :base(httpcontextAccessor, getDependencies) {
            _getDependencies = getDependencies;
        }

        public async Task<Result<DTO>> GetItemByCode(string code, int Client) {
            var oEntity = await _getDependencies.GetItemByCode(code, Client);
            Result<Entity> oResult = oEntity ?? Result.Failure<Entity>(Error.Create(_errorTraduccion.NotFound));
            return await oResult.Async()
                .MapAsync(x => _mapper.Map(x));
        }
        public virtual async Task<Result<IEnumerable<DTO>>> GetList(int Client, string Email, List<Filter> filters = null, List<string> orders = null, string direction = null, int pageSize = -1, int pageIndex = -1) {
            var oEntityList = await _getDependencies.GetList(Client, filters, orders, direction, pageSize, pageIndex);
            Result<IEnumerable<Entity>> entityList = (oEntityList == null ?
                Result.Failure<IEnumerable<Entity>>(Error.Create(_errorTraduccion.NotFound)) :
                new Result<IEnumerable<Entity>>(oEntityList));
            List<DTO> lista = new List<DTO>();
            foreach (var item in entityList.Valor)
            {
                lista.Add(await _mapper.Map(item));
            }
            int totalItems = (entityList.Valor.FirstOrDefault() != null) ? entityList.Valor.FirstOrDefault().TotalItems : 0;
            return new Result<IEnumerable<DTO>>(lista, totalItems);
        }
    }
}
