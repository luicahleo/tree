using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeletePaymentMethods : DeleteObjectService<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper>
    {
        GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> _getDependencies;
        public DeletePaymentMethods(DeleteDependencies<PaymentMethodsEntity> dependencies, GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<PaymentMethodsDTO>> Delete(string sCode, int client)
        {
            var PaymentMethodsIdenty = await _getDependencies.GetItemByCode(sCode, client);
            Result<PaymentMethodsEntity> paymentMethods = (PaymentMethodsIdenty == null || PaymentMethodsIdenty.MetodoPagoID == null ?
                Result.Failure<PaymentMethodsEntity>(Error.Create(_errorTraduccion.NotFound))
                : PaymentMethodsIdenty);
            if (paymentMethods.Success)
            {
                if (paymentMethods.Valor.Defecto)
                {
                    return Result.Failure<PaymentMethodsDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(paymentMethods.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<PaymentMethodsDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await paymentMethods.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
