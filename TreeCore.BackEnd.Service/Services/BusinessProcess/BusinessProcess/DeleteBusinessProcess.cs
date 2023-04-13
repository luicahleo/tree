using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers.BusinessProcess;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.BusinessProcess
{
    public class DeleteBusinessProcess : DeleteObjectService<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper>
    {
        GetDependencies<BusinessProcessDTO, BusinessProcessEntity> _getDependencies;
        public DeleteBusinessProcess(DeleteDependencies<BusinessProcessEntity> dependencies, GetDependencies<BusinessProcessDTO, BusinessProcessEntity> getDepencies, 
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<BusinessProcessDTO>> Delete(string sCode, int Client)
        {
            var BusinessProcessIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<BusinessProcessEntity> businessProcess = (BusinessProcessIdenty == null || BusinessProcessIdenty.CoreBusinessProcessID == null ?
                Result.Failure<BusinessProcessEntity>(Error.Create(_errorTraduccion.NotFound))
                : BusinessProcessIdenty);

            if (businessProcess.Success)
            {
                var iResult = await DeleteItem(businessProcess.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<BusinessProcessDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }

            return await businessProcess.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
