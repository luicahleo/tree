using System.Collections.Generic;

namespace TreeCore.Shared.DTO.Companies
{
    public class CompanyAssignedPaymentMethodsDTO : BaseDTO
    {
        public string PaymentMethodCode { get; set; }
        public bool Default { get; set; }

        public CompanyAssignedPaymentMethodsDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(PaymentMethodCode).ToLower(), "metodopagoid");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}
