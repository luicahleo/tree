using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ImportExport
{
    public class DeleteImportTask : DeleteObjectService<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper>
    {
        GetDependencies<ImportTaskDTO, ImportTaskEntity> _getDependencies;
        public DeleteImportTask(DeleteDependencies<ImportTaskEntity> dependencies, GetDependencies<ImportTaskDTO, ImportTaskEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ImportTaskDTO>> Delete(string code, int client)
        {
            var CurrencyIdentty = await _getDependencies.GetItemByCode(code, client);
            Result<ImportTaskEntity> Currency = (CurrencyIdentty == null || CurrencyIdentty.DocumentoCargaID == null ?
                Result.Failure<ImportTaskEntity>(Error.Create(_errorTraduccion.NotFound))
                : CurrencyIdentty);
            if (Currency.Success)
            {
                var iResult = await DeleteItem(Currency.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ImportTaskDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await Currency.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}