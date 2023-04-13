using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// PaymentMethodsController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class PaymentMethodsController : ApiControllerBase<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper>, IDeleteController<PaymentMethodsDTO>, IModController<PaymentMethodsDTO>
    {

        private readonly PutPaymentMethods _putPaymentMethods;
        private readonly PostPaymentMethods _postPaymentMethods;
        private readonly DeletePaymentMethods _deletePaymentMethods;

        public PaymentMethodsController(GetObjectService<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper> getObjectService, PutPaymentMethods putPaymentMethods, PostPaymentMethods postPaymentMethods, DeletePaymentMethods deletePaymentMethods)
            : base(getObjectService)
        {
            _putPaymentMethods = putPaymentMethods;
            _postPaymentMethods = postPaymentMethods;
            _deletePaymentMethods = deletePaymentMethods;
        }

        /// <summary>
        /// Post PaymentMethods
        /// </summary>
        /// <returns>List of PaymentMethods</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<PaymentMethodsDTO>> Post(PaymentMethodsDTO paymentmethods)
        {
            return (await _postPaymentMethods.Create(paymentmethods, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of PaymentMethods
        /// </summary>
        /// <param name="code">Code of PaymentMethods</param>
        /// <returns>List of PaymentMethods</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<PaymentMethodsDTO>> Put(PaymentMethodsDTO paymentmethodsDTO, string code)
        {
            return (await _putPaymentMethods.Update(paymentmethodsDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of PaymentMethods
        /// </summary>
        /// <param name="code">Code of PaymentMethods</param>
        /// <returns>PaymentMethods</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<PaymentMethodsDTO>> Delete(string code)
        {
            return (await _deletePaymentMethods.Delete(code, Client)).MapDto(x => x);
        }

    }
}
