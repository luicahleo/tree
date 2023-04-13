using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.Shared.ROP;
using TreeCore.Shared.DTO;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Model.Entity;
using TreeCore.BackEnd.Service.Mappers;

namespace TreeCore.BackEnd.Service.Services
{
    public abstract class DeleteObjectService<DTO, Entity, Mapper> : BaseObjectService<DTO, Entity, Mapper>
        where DTO: BaseDTO
        where Entity: BaseEntity
        where Mapper : BaseMapper<DTO, Entity>
    {

        private readonly DeleteDependencies<Entity> _deleteDependencies;

        protected DeleteObjectService(IHttpContextAccessor httpcontextAccessor, DeleteDependencies<Entity> deleteDependencies) : base(httpcontextAccessor, deleteDependencies)
        {
            _deleteDependencies = deleteDependencies;
        }
        public abstract Task<Result<DTO>> Delete(string sCode, int Client);
        public async Task<Result<int>> DeleteItem(Entity oEntity) {
            return await _deleteDependencies.Delete(oEntity);
        }
    }
}
