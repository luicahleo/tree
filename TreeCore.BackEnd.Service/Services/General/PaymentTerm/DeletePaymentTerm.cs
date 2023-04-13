using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeletePaymentTerm : DeleteObjectService<PaymentTermDTO, PaymentTermEntity, PaymentTermDTOMapper>
    {
        GetDependencies<PaymentTermDTO, PaymentTermEntity> _getDependencies;
        public DeletePaymentTerm(DeleteDependencies<PaymentTermEntity> dependencies, GetDependencies<PaymentTermDTO, PaymentTermEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<PaymentTermDTO>> Delete(string sCode, int client)
        {
            var PaymentTermIdenty = await _getDependencies.GetItemByCode(sCode, client);
            Result<PaymentTermEntity> paymentTerm = (PaymentTermIdenty == null || PaymentTermIdenty.CondicionPagoID == null ?
                Result.Failure<PaymentTermEntity>(Error.Create(_errorTraduccion.NotFound))
                : PaymentTermIdenty);
            if (paymentTerm.Success)
            {
                if (paymentTerm.Valor.Defecto)
                {
                    return Result.Failure<PaymentTermDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(paymentTerm.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<PaymentTermDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await paymentTerm.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
