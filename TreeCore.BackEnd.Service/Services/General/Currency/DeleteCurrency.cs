using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class DeleteCurrency : DeleteObjectService<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper>
    {
        GetDependencies<CurrencyDTO, CurrencyEntity> _getDependencies;
        public DeleteCurrency(DeleteDependencies<CurrencyEntity> dependencies, GetDependencies<CurrencyDTO, CurrencyEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<CurrencyDTO>> Delete(string code, int Client)
        {
            var CurrencyIdentty = await _getDependencies.GetItemByCode(code, Client);
            Result<CurrencyEntity> Currency = (CurrencyIdentty == null || CurrencyIdentty.MonedaID == null ?
                Result.Failure<CurrencyEntity>(Error.Create(_errorTraduccion.NotFound))
                : CurrencyIdentty);
            if (Currency.Success)
            {
                if (Currency.Valor.Defecto)
                {
                    return Result.Failure<CurrencyDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(Currency.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CurrencyDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await Currency.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}