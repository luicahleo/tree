using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.BusinessProcess
{
    public class DeleteBusinessProcessType : DeleteObjectService<BusinessProcessTypeDTO, BusinessProcessTypeEntity, BusinessProcessTypeDTOMapper>
    {

        private readonly GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> _getDependencies;
        public DeleteBusinessProcessType(DeleteDependencies<BusinessProcessTypeEntity> dependencies, GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<BusinessProcessTypeDTO>> Delete(string sCode, int client)
        {
            var BusinessProcessTypeIdentty = await _getDependencies.GetItemByCode(sCode, client);
            Result<BusinessProcessTypeEntity> businessProcessType = (BusinessProcessTypeIdentty == null || BusinessProcessTypeIdentty.CoreBusinessProcessTipoID == null ?
                Result.Failure<BusinessProcessTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : BusinessProcessTypeIdentty);
            if (businessProcessType.Success)
            {
                if (businessProcessType.Value.Defecto)
                {
                    return Result.Failure<BusinessProcessTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(businessProcessType.Value);
                if (iResult.Value == 0)
                {
                    return Result.Failure<BusinessProcessTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await businessProcessType.Async()
                .MapAsync(x => _mapper.Map(x));
        }


    }
}
