using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity;
using TreeCore.Shared.DTO;
using TreeCore.BackEnd.Service.Validations;
using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;

namespace TreeCore.BackEnd.Service.Services
{
    public abstract class ModObjectService<DTO, Entity, Mapper> : BaseObjectService<DTO, Entity, Mapper>
        where DTO : BaseDTO
        where Entity : BaseEntity
        where Mapper : BaseMapper<DTO, Entity>
    {
        private readonly BasicValidation<DTO, Entity> _validation;
        protected readonly Dictionary<string, string> codesDict;

        protected ModObjectService(IHttpContextAccessor httpcontextAccessor, BasicDependence<Entity> dependencies, BasicValidation<DTO, Entity> validation):base(httpcontextAccessor, dependencies) {
            _validation = validation;
            codesDict = new Dictionary<string, string>();
        }

        public abstract Task<Result<Entity>> ValidateEntity(DTO oEntidad, int clientID, string email, string code = "");

        protected Task<Result<Entity>> ValidateText(Entity oEntidad)
            => _validation.ValidateEntity(oEntidad, _errorTraduccion).Async();

        public abstract Task<Result<Entity>> SaveItem(Entity oEntidad);

    }
}
