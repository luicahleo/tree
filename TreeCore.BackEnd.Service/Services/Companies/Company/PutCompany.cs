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

    public class PutCompany : PutObjectService<CompanyDTO, CompanyEntity, CompanyDTOMapper>
    {
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getDependency;
        private readonly GetDependencies<CompanyTypeDTO, CompanyTypeEntity> _getCompanyTypeDependency;
        private readonly GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> _getTaxpayerTypeDependency;
        private readonly GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> _getTaxIdentificationNumberCategoryDependency;
        private readonly GetDependencies<PaymentTermDTO, PaymentTermEntity> _getPaymentTermDependency;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getCurrencyDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly PutCompanyAssignedPaymentMethods _putCompanyAssignedPaymentMethods;
        private readonly PutBankAccount _putBankAccountDependency;
        private readonly PutCompanyAddress _putCompanyAddress;

        public PutCompany(PutDependencies<CompanyEntity> putDependency, GetDependencies<CompanyDTO, CompanyEntity> getDependency,
            GetDependencies<CompanyTypeDTO, CompanyTypeEntity> getCompanyTypeDependency,
            GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> getTaxpayerTypeDependency,
            GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> getTaxIdentificationNumberCategoryDependency,
            GetDependencies<PaymentTermDTO, PaymentTermEntity> getPaymentTermDependency,
            GetDependencies<CurrencyDTO, CurrencyEntity> getCurrencyDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            PutCompanyAssignedPaymentMethods putCompanyAssignedPaymentMethods,
            PutBankAccount putBankAccountDependency,
            PutCompanyAddress putCompanyAddress,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new CompanyValidation())
        {
            _getDependency = getDependency;
            _getCompanyTypeDependency = getCompanyTypeDependency;
            _getTaxpayerTypeDependency = getTaxpayerTypeDependency;
            _getTaxIdentificationNumberCategoryDependency = getTaxIdentificationNumberCategoryDependency;
            _getPaymentTermDependency = getPaymentTermDependency;
            _getCurrencyDependency = getCurrencyDependency;
            _putBankAccountDependency = putBankAccountDependency;
            _putCompanyAssignedPaymentMethods = putCompanyAssignedPaymentMethods;
            _getUserDependency = getUserDependency;
            _putCompanyAddress = putCompanyAddress;
        }

        public override async Task<Result<CompanyEntity>> ValidateEntity(CompanyDTO company, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<CompanyEntity>> listCompanys;
            List<Filter> listFilters = new List<Filter>();
            Filter filter;
            CompanyEntity prodFinal = null;
            CompanyEntity companyEntity = await _getDependency.GetItemByCode(code, client);
            CompanyTypeEntity companyType = new CompanyTypeEntity();
            TaxpayerTypeEntity taxpayerType = new TaxpayerTypeEntity();
            TaxIdentificationNumberCategoryEntity TaxIdentificationNumberCategory = new TaxIdentificationNumberCategoryEntity();
            PaymentTermEntity PaymentTermEntity = new PaymentTermEntity();
            CurrencyEntity CurrencyEntity = new CurrencyEntity();
            UserEntity user = await _getUserDependency.GetItemByCode(email, client);


            if (company.CompanyTypeCode != "" && company.CompanyTypeCode != null)
            {
                companyType = await _getCompanyTypeDependency.GetItemByCode(company.CompanyTypeCode, client);
            }
            else
            {
                company.CompanyTypeCode = "";
            }
            if (company.TaxpayerTypeCode != "" && company.TaxpayerTypeCode != null)
            {
                taxpayerType = await _getTaxpayerTypeDependency.GetItemByCode(company.TaxpayerTypeCode, client);
            }
            else
            {
                company.TaxpayerTypeCode = "";
            }

            if (company.TaxIdentificationNumberCategoryCode != "" && company.TaxIdentificationNumberCategoryCode != null)
            {
                TaxIdentificationNumberCategory = await _getTaxIdentificationNumberCategoryDependency.GetItemByCode(company.TaxIdentificationNumberCategoryCode, client);
            }
            else
            {
                company.TaxIdentificationNumberCategoryCode = "";
            }
            if (company.PaymentTermCode != "" && company.PaymentTermCode != null)
            {
                PaymentTermEntity = await _getPaymentTermDependency.GetItemByCode(company.PaymentTermCode, client);
            }
            else
            {
                company.PaymentTermCode = "";
            }
            if (company.CurrencyCode != "" && company.CurrencyCode != null)
            {
                CurrencyEntity = await _getCurrencyDependency.GetItemByCode(company.CurrencyCode, client);
            }
            else
            {
                company.CurrencyCode = "";
            }

            if (companyEntity == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCompanyType + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
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
                    Result<List<BankAccountEntity>> listBankAccountValidity = await _putBankAccountDependency.ValidateEntity(company.LinkedBankAccount, client);
                    if (listBankAccountValidity.Success)
                    {
                        listBankAccount = listBankAccountValidity.Value;
                    }
                    else
                    {
                        lErrors.AddRange(listBankAccountValidity.Errors);
                    }
                }

                List<CompanyAddressEntity> listCompanyAddress = new List<CompanyAddressEntity>();

                if (company.LinkedAddresses != null && company.LinkedAddresses.Count > 0)
                {
                    Result<List<CompanyAddressEntity>> listCompanyAddressValidity = await _putCompanyAddress.ValidateEntity(company.LinkedAddresses, client);
                    if (listCompanyAddressValidity.Success)
                    {
                        listCompanyAddress = listCompanyAddressValidity.Value;
                    }
                    else
                    {
                        lErrors.AddRange(listCompanyAddressValidity.Errors);
                    }
                }

                List<CompanyAssignedPaymentMethodsEntity> listPaymentMethods = new List<CompanyAssignedPaymentMethodsEntity>();
                if (company.LinkedPaymentMethodCode != null && company.LinkedPaymentMethodCode.Count > 0)
                {
                    Result<List<CompanyAssignedPaymentMethodsEntity>> listCompanyAssignedPaymentMethodsValidity = await _putCompanyAssignedPaymentMethods.ValidateEntity(company.LinkedPaymentMethodCode, client, companyEntity);
                    if (listCompanyAssignedPaymentMethodsValidity.Success)
                    {
                        listPaymentMethods = listCompanyAssignedPaymentMethodsValidity.Value;
                    }
                    else
                    {
                        lErrors.AddRange(listCompanyAssignedPaymentMethodsValidity.Errors);
                    }
                }
                if (companyType == null)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeCompanyType + " " + $"{company.CompanyTypeCode}" + " " + _errorTraduccion.NotFound + "."));
                }
                if (CurrencyEntity == null)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeCurrency + " " + $"{company.CurrencyCode}" + " " + _errorTraduccion.NotFound + "."));
                }

                CompanyEntity? prod = await _getDependency.GetItemByCode(code, client);

                //filter = new Filter(nameof(CompanyEntity.EntidadID), Operators.eq, prod.EntidadID);
                //listFilters.Add(filter);
                if (prod != null)
                {
                    prodFinal = new CompanyEntity(
                        prod.EntidadID, client, company.Code, company.Name,
                        company.Alias, company.Email, company.Phone, company.Active,
                        company.Owner, company.Payee, company.Supplier, company.Customer, company.TaxIdentificationNumber,
                        companyType, taxpayerType, TaxIdentificationNumberCategory, PaymentTermEntity,
                        CurrencyEntity, listBankAccount, listPaymentMethods, companyEntity.FechaCreaccion, DateTime.Now, companyEntity.UsuariosCreadores, user, listCompanyAddress);

                }

                filter = new Filter(nameof(CompanyDTO.Code), Operators.eq, company.Code);
                listFilters.Add(filter);

                listCompanys = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (prodFinal != null)
                {
                    if (listCompanys.Result != null && listCompanys.Result.ListOrEmpty().Count > 0 &&
                    listCompanys.Result.ListOrEmpty()[0].EntidadID != prodFinal.EntidadID)
                    {
                        lErrors.Add(Error.Create(_traduccion.Company + " " + $"{company.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                    }
                }
                company.LastModificationUserEmail = prodFinal.UsuariosModificadores.EMail;
                if (prodFinal.UsuariosCreadores != null)
                {
                    company.CreationUserEmail = prodFinal.UsuariosCreadores.EMail;
                }
                company.CreationDate = prodFinal.FechaCreaccion;
                company.LastModificationDate = DateTime.Now;
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<CompanyEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return prodFinal;
            }
        }

    }
}

