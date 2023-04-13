using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.Contracts;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;
using System.Linq;

namespace TreeCore.BackEnd.Service.Services.Contracts.ContractLineEntidad
{
    public class PutContractLineEntidad : PutObjectService<ContractLineEntidadDTO, ContractLineEntidadEntity, ContractLineEntidadDTOMapper>
    {
        private readonly GetDependencies<ContractLineEntidadDTO, ContractLineEntidadEntity> _getDependency;
        private readonly GetDependencies<ContractLineDTO, ContractLineEntity> _getDependencyContractLine;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getDependencyCurrency;
        private readonly GetDependencies<BankAccountDTO, BankAccountEntity> _getDependencyBankAcount;
        private readonly GetDependencies<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity> _getDependencyPaymentMethod;
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getDependencyCompany;
        public PutContractLineEntidad(PutDependencies<ContractLineEntidadEntity> putDependency, GetDependencies<ContractLineEntidadDTO, ContractLineEntidadEntity> getDependency,
             GetDependencies<ContractLineDTO, ContractLineEntity> getDependencyContractLine,
              GetDependencies<CurrencyDTO, CurrencyEntity> getDependencyCurrency,
              GetDependencies<BankAccountDTO, BankAccountEntity> getDependencyBankAcount,
              GetDependencies<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity> getDependencyPaymentMethod,
              GetDependencies<CompanyDTO, CompanyEntity> getDependencyCompany,
        IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, putDependency, new ContractLineEntidadValidation())
        {
            _getDependency = getDependency;
            _getDependencyContractLine = getDependencyContractLine;
            _getDependencyCurrency = getDependencyCurrency;
            _getDependencyBankAcount = getDependencyBankAcount;
            _getDependencyPaymentMethod = getDependencyPaymentMethod;
            _getDependencyCompany = getDependencyCompany;
        }


       

        public async Task<Result<List<ContractLineEntidadEntity>>> ValidateEntity(List<ContractLineEntidadDTO> listcontractlineEntidadDTO, int client, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<ContractLineEntidadEntity> listcontractLineEntidadEntity = new List<ContractLineEntidadEntity>();

           
            foreach (ContractLineEntidadDTO elemento in listcontractlineEntidadDTO)
            {
                if (controlRepetido(listcontractlineEntidadDTO, elemento))
                {

                    lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + $"{elemento.CompanyCode}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                CompanyEntity company = await _getDependencyCompany.GetItemByCode(elemento.CompanyCode, client);
                CurrencyEntity currency = await _getDependencyCurrency.GetItemByCode(elemento.currencyCode, client);
                

                if (company == null) { lErrors.Add(Error.Create(_traduccion.CodeContractLineEntidad + " " + $"{elemento.CompanyCode}" + " " + _errorTraduccion.NotFound + ".")); }
                if (currency == null) { lErrors.Add(Error.Create(_traduccion.CodeCurrency + " " + $"{elemento.currencyCode}" + " " + _errorTraduccion.NotFound + ".")); }

                if (company != null)
                {
                    CompanyAssignedPaymentMethodsEntity companyassignedpaymentMethods = await _getDependencyPaymentMethod.GetItemByCode(elemento.PaymentMethodCode, (int)company.EntidadID);
                    BankAccountEntity bankAcount = await _getDependencyBankAcount.GetItemByCode(elemento.BankAcountCode, (int)company.EntidadID);
                    if (companyassignedpaymentMethods == null){ lErrors.Add(Error.Create(_traduccion.CodePaymentMethods + " " + $"{elemento.PaymentMethodCode}" + " " + _errorTraduccion.NotFound + "."));}
                    if (lErrors.Count == 0)
                    {
                        listcontractLineEntidadEntity.Add(new ContractLineEntidadEntity(null, null, currency, bankAcount!=null? bankAcount:null, companyassignedpaymentMethods, elemento.Percent));
                    }
                    
                }



            }
            if (listcontractlineEntidadDTO.Count > 0)
            {
                int suma = (int)System.Math.Round(listcontractlineEntidadDTO.Sum(x => x.Percent), System.MidpointRounding.AwayFromZero);
                if(suma != 100)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + _errorTraduccion.Percent + "."));
                }
            }
            
            if (lErrors.Count > 0)
            {
                return Result.Failure<List<ContractLineEntidadEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return listcontractLineEntidadEntity;
            }

        }

        public override Task<Result<ContractLineEntidadEntity>> ValidateEntity(ContractLineEntidadDTO oEntidad, int clientID, string email, string code = "")
        {
            throw new System.NotImplementedException();
        }

        private bool controlRepetido(List<ContractLineEntidadDTO> lista, ContractLineEntidadDTO elemento)
        {
            int cont = 0;
            foreach (ContractLineEntidadDTO item in lista)
            {
                if (elemento.CompanyCode == item.CompanyCode)
                {
                    cont++;
                }
            }
            
            if (cont > 1)
            {
                return true;
            }
            return false;
        }
    }
}

