using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeleteTaxpayerType : DeleteObjectService<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper>
    {
        GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> _getDependencies;
        public DeleteTaxpayerType(DeleteDependencies<TaxpayerTypeEntity> dependencies, GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<TaxpayerTypeDTO>> Delete(string sCode, int client)
        {
            var TaxpayerTypeIdenty = await _getDependencies.GetItemByCode(sCode, client);
            Result<TaxpayerTypeEntity> taxpayerType = (TaxpayerTypeIdenty == null || TaxpayerTypeIdenty.TipoContribuyenteID == null ?
                Result.Failure<TaxpayerTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : TaxpayerTypeIdenty);
            if (taxpayerType.Success)
            {
                if (taxpayerType.Valor.Defecto)
                {
                    return Result.Failure<TaxpayerTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(taxpayerType.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<TaxpayerTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await taxpayerType.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
