using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{

    public class PutCompanyAssignedPaymentMethods : PutObjectService<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity, CompanyAssignedPaymentMethodsDTOMapper>
    {
        private readonly GetDependencies<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity> _getDependency;
        private readonly GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> _getPaymentMethodsDependency;

        public PutCompanyAssignedPaymentMethods(PutDependencies<CompanyAssignedPaymentMethodsEntity> putDependency,
                GetDependencies<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity> getDependency,
                GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> getPaymentMethodsDependency,
        IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, putDependency, new CompanyAssignedPaymentMethodsValidation())
        {
            _getDependency = getDependency;
            _getPaymentMethodsDependency = getPaymentMethodsDependency;
        }

        public override async Task<Result<CompanyAssignedPaymentMethodsEntity>> ValidateEntity(CompanyAssignedPaymentMethodsDTO paymentMethod, int clientID, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            PaymentMethodsEntity bank = await _getPaymentMethodsDependency.GetItemByCode(paymentMethod.PaymentMethodCode, clientID);
            if (bank == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeBank + " " + $"{paymentMethod.PaymentMethodCode}" + " " + _errorTraduccion.NotFound + "."));
            }


            // CompanyAssignedPaymentMethodsEntity companyEntity = new CompanyAssignedPaymentMethodsEntity(null, paymentMethod.Code, paymentMethod.IBAN, paymentMethod.Description, paymentMethod.SWIFT, company, bank);
            filter = new Filter(nameof(CompanyAssignedPaymentMethodsDTO.PaymentMethodCode), Operators.eq, paymentMethod.PaymentMethodCode);
            listFilters.Add(filter);

            Task<IEnumerable<CompanyAssignedPaymentMethodsEntity>> listBankAccounts = _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listBankAccounts.Result != null && listBankAccounts.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodePaymentMethods + " - " + _traduccion.DefectoYaExiste + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<CompanyAssignedPaymentMethodsEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return null;
                //return companyEntity;
            }
        }

        public async Task<Result<List<CompanyAssignedPaymentMethodsEntity>>> ValidateEntity(List<CompanyAssignedPaymentMethodsDTO> linkedpaymentMethod, int clientID, CompanyEntity EntidadID)
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<CompanyAssignedPaymentMethodsEntity> linkedCompanyAssignedPaymentMethodsEntity = new List<CompanyAssignedPaymentMethodsEntity>();

            foreach (CompanyAssignedPaymentMethodsDTO paymentMethodList in linkedpaymentMethod)
            {
                if (paymentMethodList.Default && controlRepetido(linkedpaymentMethod, paymentMethodList))
                {
                    lErrors.Add(Error.Create(_traduccion.DefectoYaExiste+ "."));
                }


                PaymentMethodsEntity paymentMethod = await _getPaymentMethodsDependency.GetItemByCode(paymentMethodList.PaymentMethodCode, clientID);
                if (paymentMethod == null)
                {
                    lErrors.Add(Error.Create(_traduccion.CodePaymentMethods + " " + $"{paymentMethodList.PaymentMethodCode}" + " " + _errorTraduccion.NotFound + "."));
                }
                else
                {
                    linkedCompanyAssignedPaymentMethodsEntity.Add(new CompanyAssignedPaymentMethodsEntity(null, EntidadID, paymentMethod, paymentMethodList.Default));
                }

            };


            if (lErrors.Count > 0)
            {
                return Result.Failure<List<CompanyAssignedPaymentMethodsEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return linkedCompanyAssignedPaymentMethodsEntity;
            }
        }

        private bool controlRepetido(List<CompanyAssignedPaymentMethodsDTO> lista, CompanyAssignedPaymentMethodsDTO elemento)
        {
            int cont = 0;
            foreach (CompanyAssignedPaymentMethodsDTO paymentMethod in lista)
            {
                if (elemento.Default == paymentMethod.Default)
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

