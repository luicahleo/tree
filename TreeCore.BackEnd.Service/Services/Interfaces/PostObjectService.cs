using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services
{
    public abstract class PostObjectService<DTO, Entity, Mapper> : ModObjectService<DTO, Entity, Mapper>
        where DTO : BaseDTO
        where Entity : BaseEntity
        where Mapper : BaseMapper<DTO, Entity>
    {

        protected readonly PostDependencies<Entity> _postDependencies;  

        protected PostObjectService(IHttpContextAccessor httpcontextAccessor, PostDependencies<Entity> postDependencies, BasicValidation<DTO, Entity> validation) : base(httpcontextAccessor, postDependencies, validation)
        {
            _postDependencies = postDependencies;
        }

        public override async Task<Result<Entity>> SaveItem(Entity oEntidad)
        {
            await _postDependencies.Create(oEntidad);
            return oEntidad;
        }

        public virtual async Task<Result<DTO>> Create(DTO oEntidad, int clientID, string email, string code = "")
        {
            codesDict["LEVEL1"] = code;
            return await ValidateEntity(oEntidad, clientID, email, code)
                 .Bind(ValidateText)
                 .Bind(SaveItem)
                 .Bind(CommitTransaction)
                 .MapAsync(_ => oEntidad);
        }

    }
}
