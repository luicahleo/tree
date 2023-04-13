using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractLineEntidadDTO : BaseDTO
    {


        [Required] public string CompanyCode { get; set; }
        [Required] public string PaymentMethodCode { get; set; }
        public string BankAcountCode { get; set; }
        public string currencyCode { get; set; }
       
        [Required] public double  Percent {get;set;}









        public ContractLineEntidadDTO()
        {
            map = new Dictionary<string, string>();

            map.Add(nameof(CompanyCode).ToLower(), "codigo");
            map.Add(nameof(BankAcountCode).ToLower(), "codigo");
            map.Add(nameof(currencyCode).ToLower(), "codigo");
            map.Add(nameof(PaymentMethodCode).ToLower(), "metodopago");
            map.Add(nameof(Percent).ToLower(), "cantidadporcentaje");


        }
    }
}