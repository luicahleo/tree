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
    /// PaymentTermController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class PaymentTermController : ApiControllerBase<PaymentTermDTO, PaymentTermEntity, PaymentTermDTOMapper>, IDeleteController<PaymentTermDTO>, IModController<PaymentTermDTO>
    {

        private readonly PutPaymentTerm _putPaymentTerm;
        private readonly PostPaymentTerm _postPaymentTerm;
        private readonly DeletePaymentTerm _deletePaymentTerm;

        public PaymentTermController(GetObjectService<PaymentTermDTO, PaymentTermEntity, PaymentTermDTOMapper> getObjectService, PutPaymentTerm putPaymentTerm, PostPaymentTerm postPaymentTerm, DeletePaymentTerm deletePaymentTerm) 
            : base(getObjectService)
        {
            _putPaymentTerm = putPaymentTerm;
            _postPaymentTerm = postPaymentTerm;
            _deletePaymentTerm = deletePaymentTerm;
        }

        /// <summary>
        /// Post PaymentTerm
        /// </summary>
        /// <returns>List of PaymentTerm</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<PaymentTermDTO>> Post(PaymentTermDTO paymentTerm)
        {
            return (await _postPaymentTerm.Create(paymentTerm, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of PaymentTerm
        /// </summary>
        /// <param name="code">Code of PaymentTerm</param>
        /// <returns>List of PaymentTerm</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<PaymentTermDTO>> Put(PaymentTermDTO paymentTermDTO, string code)
        {
            return (await _putPaymentTerm.Update(paymentTermDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of PaymentTerm
        /// </summary>
        /// <param name="code">Code of PaymentTerm</param>
        /// <returns>PaymentTerm</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<PaymentTermDTO>> Delete(string code)
        {
            return (await _deletePaymentTerm.Delete(code, Client)).MapDto(x => x);
        }

    }
}
