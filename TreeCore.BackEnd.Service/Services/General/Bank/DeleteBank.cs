using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeleteBank : DeleteObjectService<BankDTO, BankEntity, BankDTOMapper>
    {
        GetDependencies<BankDTO, BankEntity> _getDependencies;
        public DeleteBank(DeleteDependencies<BankEntity> dependencies, GetDependencies<BankDTO, BankEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<BankDTO>> Delete(string sCode, int Client)
        {
            var BankIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<BankEntity> bank = (BankIdenty == null || BankIdenty.BancoID == null ?
                Result.Failure<BankEntity>(Error.Create(_errorTraduccion.NotFound))
                : BankIdenty);
            if (bank.Success)
            {
                if (bank.Valor.Defecto)
                {
                    return Result.Failure<BankDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(bank.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<BankDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await bank.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
