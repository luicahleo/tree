using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.BackEnd.Service.Mappers.Companies
{
    public class CompanyDTOMapper : BaseMapper<CompanyDTO, CompanyEntity>
    {
        public override Task<CompanyDTO> Map(CompanyEntity company)
        {
            CompanyDTO dto = new CompanyDTO()
            {
                Active = company.Activo,
                Code = company.Codigo,
                Name = company.Nombre,
                Alias = company.Alias,
                Email = company.Email,
                Phone = company.Telefono,
                Owner = company.EsPropietario,
                Supplier = company.EsProveedor,
                Customer = company.EsOperador,
                Payee = company.EsBeneficiario,
                CreationUserEmail = (company.UsuariosCreadores != null) ? company.UsuariosCreadores.EMail : "",
                LastModificationUserEmail = (company.UsuariosModificadores != null) ? company.UsuariosModificadores.EMail : "",
                CreationDate = company.FechaCreaccion,
                LastModificationDate =  company.FechaUltimaModificacion,
                TaxIdentificationNumber = company.NumIdentContribuyente,
                CompanyTypeCode = (company.EntidadesTipos != null) ? company.EntidadesTipos.Codigo : "",
                TaxpayerTypeCode = (company.TiposContribuyentes != null) ? company.TiposContribuyentes.Codigo : "",
                TaxIdentificationNumberCategoryCode = (company.SAPTipoNIF != null) ? company.SAPTipoNIF.Codigo : "",
                PaymentTermCode = (company.CondicionPago != null) ? company.CondicionPago.Codigo : "",
                CurrencyCode = (company.Moneda != null) ? company.Moneda.Moneda : ""
            };

            if (company.CuentasBancarias != null && company.CuentasBancarias.ToList().Count > 0)
            {
                dto.LinkedBankAccount = new List<BankAccountDTO>();
                foreach (BankAccountEntity linkBankAccount in company.CuentasBancarias.ToList())
                {
                    BankAccountDTO bankAccount = new BankAccountDTO();
                    bankAccount.BankCode = linkBankAccount.Bank.CodigoBanco;
                    bankAccount.Code = linkBankAccount.Codigo;
                    bankAccount.IBAN = linkBankAccount.IBAN;
                    bankAccount.Description = linkBankAccount.Descripcion;
                    bankAccount.SWIFT = linkBankAccount.SWIFT;
                    dto.LinkedBankAccount.Add(bankAccount);
                }
            }

            if (company.MetodosPagos != null && company.MetodosPagos.ToList().Count > 0)
            {
                dto.LinkedPaymentMethodCode = new List<CompanyAssignedPaymentMethodsDTO>();
                foreach (CompanyAssignedPaymentMethodsEntity linkPaymentMethod in company.MetodosPagos.ToList())
                {
                    CompanyAssignedPaymentMethodsDTO paymentMethod = new CompanyAssignedPaymentMethodsDTO();
                    paymentMethod.PaymentMethodCode = linkPaymentMethod.CoreMetodosPagos.CodigoMetodoPago;
                    paymentMethod.Default = linkPaymentMethod.Defecto;
                    dto.LinkedPaymentMethodCode.Add(paymentMethod);
                }
            }
            
            if (company.Direcciones != null && company.Direcciones.ToList().Count > 0)
            {
                dto.LinkedAddresses = new List<CompanyAddressDTO>();
                foreach (CompanyAddressEntity linkcompanyAddress in company.Direcciones.ToList())
                {
                    CompanyAddressDTO companyAddress = new CompanyAddressDTO();
                    AddressDTO direccion = JsonSerializer.Deserialize<AddressDTO>(linkcompanyAddress.DireccionJSON);
                    companyAddress.Code = linkcompanyAddress.Codigo;
                    companyAddress.Name = linkcompanyAddress.EntidadDireccion;
                    companyAddress.Default = linkcompanyAddress.Defecto;
                    companyAddress.Address = direccion;
                    dto.LinkedAddresses.Add(companyAddress);
                }
            }
            return Task.FromResult(dto);
        }
    }
}
