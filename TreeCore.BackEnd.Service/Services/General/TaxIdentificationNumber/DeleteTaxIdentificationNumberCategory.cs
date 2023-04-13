using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeleteTaxIdentificationNumberCategory : DeleteObjectService<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity, TaxIdentificationNumberCategoryDTOMapper>
    {
        GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> _getDependencies;
        public DeleteTaxIdentificationNumberCategory(DeleteDependencies<TaxIdentificationNumberCategoryEntity> dependencies, GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<TaxIdentificationNumberCategoryDTO>> Delete(string sCode, int client)
        {
            var SAPTypeNIDIdddenty = await _getDependencies.GetItemByCode(sCode, client);
            Result<TaxIdentificationNumberCategoryEntity> sapTypeNIF = (SAPTypeNIDIdddenty == null || SAPTypeNIDIdddenty.SAPTipoNIFID == null ?
                Result.Failure<TaxIdentificationNumberCategoryEntity>(Error.Create(_errorTraduccion.NotFound))
                : SAPTypeNIDIdddenty);
            if (sapTypeNIF.Success)
            {
                if (sapTypeNIF.Valor.Defecto)
                {
                    return Result.Failure<TaxIdentificationNumberCategoryDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(sapTypeNIF.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<TaxIdentificationNumberCategoryDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await sapTypeNIF.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
