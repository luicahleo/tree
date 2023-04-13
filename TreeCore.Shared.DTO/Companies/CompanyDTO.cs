using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.General;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeCore.Shared.DTO.Companies
{
    public class CompanyDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string CompanyTypeCode { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public bool Active { get; set; }
        
        public bool Owner { get; set; }
        public bool Supplier { get; set; }
        public bool Customer { get; set; }
        public bool Payee { get; set; }

        public string TaxIdentificationNumber { get; set; }        
        public string TaxpayerTypeCode { get; set; }
        public string TaxIdentificationNumberCategoryCode { get; set; }
        public string PaymentTermCode { get; set; }
        

        [Editable(false)]
        [DataType(DataType.Date)]
        //[SwaggerSchema(ReadOnly = true)]
        public DateTime? CreationDate { get; set; }
        [Editable(false)]
        [DataType(DataType.Date)]
        //[SwaggerSchema(ReadOnly = true)]
        public DateTime? LastModificationDate { get; set; }
        [Editable(false)]
        //[SwaggerSchema(ReadOnly = true)]
        public string CreationUserEmail { get; set; }
        [Editable(false)]
        //[SwaggerSchema(ReadOnly = true)]
        public string LastModificationUserEmail { get; set; }

        
        public List<BankAccountDTO> LinkedBankAccount { get; set; }
        public List<CompanyAssignedPaymentMethodsDTO> LinkedPaymentMethodCode { get; set; }
        public List<CompanyAddressDTO> LinkedAddresses { get; set; }

        public CompanyDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Alias).ToLower(), "alias");
            map.Add(nameof(Phone).ToLower(), "telefono");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Owner).ToLower(), "espropietario");
            map.Add(nameof(Supplier).ToLower(), "esproveedor");
            map.Add(nameof(Customer).ToLower(), "esoperador");
            map.Add(nameof(Payee).ToLower(), "esbeneficiario");
            map.Add(nameof(CompanyTypeCode).ToLower(), "entidadtipocodigo");
            map.Add(nameof(TaxpayerTypeCode).ToLower(), "contribuyentetipocodigo");
            map.Add(nameof(TaxIdentificationNumberCategoryCode).ToLower(), "SAPTipoNIFcodigo");
            map.Add(nameof(PaymentTermCode).ToLower(), "condicionPagocodigo");
            map.Add(nameof(CurrencyCode).ToLower(), "monedacodigo");
            map.Add(nameof(TaxIdentificationNumber).ToLower(), "numIdentContribuyente");
            map.Add(nameof(LinkedBankAccount).ToLower(), "cuentasBancarias");
            map.Add(nameof(LinkedPaymentMethodCode).ToLower(), "metodosPagos");
            map.Add(nameof(CreationDate).ToLower(), "fechacreaccion");
            map.Add(nameof(LastModificationDate).ToLower(), "fechaultimamodificacion");
            map.Add(nameof(CreationUserEmail).ToLower(), "usuariocreadorid");
            map.Add(nameof(LastModificationUserEmail).ToLower(), "usuariomodificadorid");
            map.Add(nameof(LinkedAddresses).ToLower(), "direcciones");
        }
    }
}
