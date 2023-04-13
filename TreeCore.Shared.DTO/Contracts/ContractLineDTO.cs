using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ValueObject;


namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractLineDTO : BaseDTO
    {

        [StringLength(50)] public string Code { get; set; }
        [StringLength(500)] public string Description { get; set; }
        [StringLength(50)] public string lineTypeCode { get; set; }
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")] public int Frequency { get; set; }
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")] public float Value { get; set; }
        [StringLength(10)] public string CurrencyCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public bool ApplyRenewals { get; set; }
        public bool Apportionment { get; set; }
        public bool Prepaid { get; set; }
        public PriceReadjustmentDTO PricesReadjustment { get; set; }

        public List<ContractLineTaxesDTO> ContractLineTaxes { get; set; }
        public List<ContractLineEntidadDTO> Payees { get; set; }

        public ContractLineDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigodetalle");
            // map.Add(nameof(Name).ToLower(), "nombredetalle");
            map.Add(nameof(lineTypeCode).ToLower(), "Codigo");
            map.Add(nameof(Frequency).ToLower(), "periodicidad");
            map.Add(nameof(Value).ToLower(), "importe");
            map.Add(nameof(CurrencyCode).ToLower(), "moneda");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(ValidFrom).ToLower(), "fechaprimerpago");
            map.Add(nameof(ValidTo).ToLower(), "fechaultimopago");
            map.Add(nameof(NextPaymentDate).ToLower(), "fechaproximopago");
            map.Add(nameof(ApplyRenewals).ToLower(), "aplicaprorrogaautomatica");
            map.Add(nameof(Apportionment).ToLower(), "prorrateo");
            map.Add(nameof(Prepaid).ToLower(), "pagoanticipado");
            map.Add(nameof(PricesReadjustment).ToLower(), "reajustePrecios");
            map.Add(nameof(Payees).ToLower(), "alquileresdetallesentidades");

        }
    }
}
