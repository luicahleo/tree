using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services
{
    public abstract class PutObjectService<DTO, Entity, Mapper> : ModObjectService<DTO, Entity, Mapper>
        where DTO : BaseDTO
        where Entity : BaseEntity
        where Mapper : BaseMapper<DTO, Entity>
    {

        protected readonly PutDependencies<Entity> _putDependencies;

        protected PutObjectService(IHttpContextAccessor httpcontextAccessor, PutDependencies<Entity> putDependencies, BasicValidation<DTO, Entity> validation) : base(httpcontextAccessor, putDependencies, validation)
        {
            _putDependencies = putDependencies;
        }

        public override async Task<Result<Entity>> SaveItem(Entity oEntidad)
        {
            await _putDependencies.Update(oEntidad);
            return oEntidad;
        }

        public virtual async Task<Result<DTO>> Update(DTO oEntidad, string sCode, int clientID, string email, string parentCode = "")
        {
            codesDict["LEVEL1"] = parentCode;
            codesDict["LEVEL2"] = sCode;

            return await ValidateEntity(oEntidad, clientID, sCode, email)
                 .Bind(ValidateText)
                 .Bind(SaveItem)
                 .Bind(CommitTransaction)
                 .MapAsync(_ => oEntidad);
        }
    }
}
