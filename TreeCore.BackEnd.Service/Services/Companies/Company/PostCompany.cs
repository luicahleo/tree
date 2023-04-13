using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{

    public class PostCompany : PostObjectService<CompanyDTO, CompanyEntity, CompanyDTOMapper>
    {
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getDependency;
        private readonly GetDependencies<CompanyTypeDTO, CompanyTypeEntity> _getCompanyTypeDependency;
        private readonly GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> _getTaxpayerTypeDependency;
        private readonly GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> _getTaxIdentificationNumberCategoryDependency;
        private readonly GetDependencies<PaymentTermDTO, PaymentTermEntity> _getPaymentTermDependency;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getCurrencyDependency;
        private readonly GetDependencies<BankDTO, BankEntity> _getBankDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly PostBankAccount _postBankAccountDependency;
        private readonly PostCompanyAssignedPaymentMethods _postCompanyAssignedPaymentMethods;
        private readonly PostCompanyAddress _postCompanyAddress;

        public PostCompany(PostDependencies<CompanyEntity> postDependency, GetDependencies<CompanyDTO, CompanyEntity> getDependency,
                GetDependencies<CompanyTypeDTO, CompanyTypeEntity> getCompanyTypeDependency,
                GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> getTaxpayerTypeDependency,
                GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> getTaxIdentificationNumberCategoryDependency,
                GetDependencies<PaymentTermDTO, PaymentTermEntity> getPaymentTermDependency,
                GetDependencies<CurrencyDTO, CurrencyEntity> getCurrencyDependency,
                GetDependencies<BankDTO, BankEntity> getBankDependency,
                GetDependencies<UserDTO, UserEntity> getUserDependency,
                PostCompanyAssignedPaymentMethods postCompanyAssignedPaymentMethodsDependency,
                PostBankAccount postBankAccountDependency,
                PostCompanyAddress postCompanyAddress,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new CompanyValidation())
        {
            _getDependency = getDependency;
            _getCompanyTypeDependency = getCompanyTypeDependency;
            _getTaxpayerTypeDependency = getTaxpayerTypeDependency;
            _getTaxIdentificationNumberCategoryDependency = getTaxIdentificationNumberCategoryDependency;
            _getPaymentTermDependency = getPaymentTermDependency;
            _getCurrencyDependency = getCurrencyDependency;
            _getBankDependency = getBankDependency;
            _postBankAccountDependency = postBankAccountDependency;
            _getUserDependency = getUserDependency;
            _postCompanyAssignedPaymentMethods = postCompanyAssignedPaymentMethodsDependency;
            _postCompanyAddress = postCompanyAddress;
        }

        public override async Task<Result<CompanyEntity>> ValidateEntity(CompanyDTO company, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;
            CompanyTypeEntity companyType = new CompanyTypeEntity();
            TaxpayerTypeEntity taxpayerType = new TaxpayerTypeEntity();
            TaxIdentificationNumberCategoryEntity TaxIdentificationNumberCategory = new TaxIdentificationNumberCategoryEntity();
            PaymentTermEntity PaymentTermEntity = new PaymentTermEntity();
            CurrencyEntity CurrencyEntity = new CurrencyEntity();
            UserEntity user = await _getUserDependency.GetItemByCode(email, Client);


            if (company.CompanyTypeCode != "" && company.CompanyTypeCode != null)
            {
                companyType = await _getCompanyTypeDependency.GetItemByCode(company.CompanyTypeCode, Client);
            }
            else
            {
                company.CompanyTypeCode = "";
            }
            if (company.TaxpayerTypeCode != "" && company.TaxpayerTypeCode != null)
            {
                taxpayerType = await _getTaxpayerTypeDependency.GetItemByCode(company.TaxpayerTypeCode, Client);
            }
            else
            {
                company.TaxpayerTypeCode = "";
            }

            if (company.TaxIdentificationNumberCategoryCode != "" && company.TaxIdentificationNumberCategoryCode != null)
            {
                TaxIdentificationNumberCategory = await _getTaxIdentificationNumberCategoryDependency.GetItemByCode(company.TaxIdentificationNumberCategoryCode, Client);
            }
            else
            {
                company.TaxIdentificationNumberCategoryCode = "";
            }
            if (company.PaymentTermCode != "" && company.PaymentTermCode != null)
            {
                PaymentTermEntity = await _getPaymentTermDependency.GetItemByCode(company.PaymentTermCode, Client);
            }
            else
            {
                company.PaymentTermCode = "";
            }
            if (company.CurrencyCode != "" && company.CurrencyCode != null)
            {
                CurrencyEntity = await _getCurrencyDependency.GetItemByCode(company.CurrencyCode, Client);
            }
            else
            {
                company.CurrencyCode = "";
            }

            company.CreationUserEmail = user.EMail;
            company.LastModificationUserEmail = user.EMail;
            company.CreationDate = DateTime.Now;
            company.LastModificationDate = DateTime.Now;

            if (companyType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCompanyType + " " + $"{company.CompanyTypeCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            if (company.TaxpayerTypeCode != "" && taxpayerType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeTaxpayerType + " " + $"{company.TaxpayerTypeCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            if (company.TaxIdentificationNumberCategoryCode != "" && TaxIdentificationNumberCategory == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeSAPTypeNIF + " " + $"{company.TaxIdentificationNumberCategoryCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            if (company.PaymentTermCode != "" && PaymentTermEntity == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodePaymentTerm + " " + $"{company.PaymentTermCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            if (CurrencyEntity == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCurrency + " " + $"{company.CurrencyCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            if (user == null)
            {
                lErrors.Add(Error.Create(_traduccion.Email + " " + $"{email}" + " " + _errorTraduccion.NotFound + "."));
            }

            List<BankAccountEntity> listBankAccount = new List<BankAccountEntity>();
            if (company.LinkedBankAccount != null && company.LinkedBankAccount.Count > 0)
            {
                Result<List<BankAccountEntity>> listBankAccountValidity = await _postBankAccountDependency.ValidateEntity(company.LinkedBankAccount, Client);
                if (listBankAccountValidity.Success)
                {
                    listBankAccount = listBankAccountValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listBankAccountValidity.Errors);
                }

            }
            IEnumerable<CompanyAssignedPaymentMethodsEntity> listPaymentMethods = new List<CompanyAssignedPaymentMethodsEntity>();
            if (company.LinkedPaymentMethodCode != null && company.LinkedPaymentMethodCode.Count > 0)
            {
                Result<List<CompanyAssignedPaymentMethodsEntity>> listCompanyAssignedPaymentMethodsValidity = await _postCompanyAssignedPaymentMethods.ValidateEntity(company.LinkedPaymentMethodCode, Client);
                if (listCompanyAssignedPaymentMethodsValidity.Success)
                {
                    listPaymentMethods = listCompanyAssignedPaymentMethodsValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listCompanyAssignedPaymentMethodsValidity.Errors);
                }

            }
            IEnumerable<CompanyAddressEntity> listAddresses = new List<CompanyAddressEntity>();
            if (company.LinkedAddresses != null && company.LinkedAddresses.Count > 0)
            {
                Result<List<CompanyAddressEntity>> listCompanyAddressValidity = await _postCompanyAddress.ValidateEntity(company.LinkedAddresses, Client);
                if (listCompanyAddressValidity.Success)
                {
                    listAddresses = listCompanyAddressValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listCompanyAddressValidity.Errors);
                }

            }
            CompanyEntity companyEntity = new CompanyEntity(null, Client, company.Code, company.Name, company.Alias, company.Email, company.Phone, company.Active, company.Owner, company.Payee, company.Supplier, company.Customer, company.TaxIdentificationNumber, companyType, taxpayerType, TaxIdentificationNumberCategory, PaymentTermEntity, CurrencyEntity, listBankAccount, listPaymentMethods, company.CreationDate.Value, company.LastModificationDate.Value, user, user, listAddresses);
            filter = new Filter(nameof(CompanyDTO.Code), Operators.eq, company.Code);
            listFilters.Add(filter);

            Task<IEnumerable<CompanyEntity>> listCompanys = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listCompanys.Result != null && listCompanys.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Company + " " + $"{company.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<CompanyEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return companyEntity;
            }
        }
    }
}

