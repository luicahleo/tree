using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class DeleteTaxes : DeleteObjectService<TaxesDTO, TaxesEntity, TaxesDTOMapper>
    {
        GetDependencies<TaxesDTO, TaxesEntity> _getDependencies;
        public DeleteTaxes(DeleteDependencies<TaxesEntity> dependencies, GetDependencies<TaxesDTO, TaxesEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<TaxesDTO>> Delete(string sCode, int client)
        {
            var TaxesIdentty = await _getDependencies.GetItemByCode(sCode, client);
            Result<TaxesEntity> taxes = (TaxesIdentty == null || TaxesIdentty.ImpuestoID == null ?
                Result.Failure<TaxesEntity>(Error.Create(_errorTraduccion.NotFound))
                : TaxesIdentty);
            if (taxes.Success)
            {
                if (taxes.Valor.Defecto)
                {
                    return Result.Failure<TaxesDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(taxes.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<TaxesDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await taxes.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
