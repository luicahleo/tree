#region USING
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TreeCore.BackEnd.API.Settings;
using TreeCore.BackEnd.Data.Repository;
using TreeCore.BackEnd.Data.Repository.BusinessProcess;
using TreeCore.BackEnd.Data.Repository.Companies;
using TreeCore.BackEnd.Data.Repository.Config;
using TreeCore.BackEnd.Data.Repository.Contracts;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Data.Repository.ImportExport;
using TreeCore.BackEnd.Data.Repository.ProductCatalog;
using TreeCore.BackEnd.Data.Repository.Sites;
using TreeCore.BackEnd.Data.Repository.WorkFlows;
using TreeCore.BackEnd.Data.Repository.WorkOrders;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.Service.Mappers.Config;
using TreeCore.BackEnd.Service.Mappers.Contracts;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Auth;
using TreeCore.BackEnd.Service.Services.BusinessProcess;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.BackEnd.Service.Services.Config;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.BackEnd.Service.Services.Contracts.ContractHistory;
using TreeCore.BackEnd.Service.Services.Contracts.ContractLineEntidad;
using TreeCore.BackEnd.Service.Services.Contracts.ContractLineTaxes;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.BackEnd.Service.Services.General.Inflation;
using TreeCore.BackEnd.Service.Services.ImportExport;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.Service.Services.Sites;
using TreeCore.BackEnd.Service.Services.WorkFlows;
using TreeCore.BackEnd.Service.Services.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.ServiceDependencies.Services.Auth;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.DTO.Project;
using TreeCore.BackEnd.Service.Mappers.Project;
using TreeCore.BackEnd.Service.Services.Project.ProjectLifeCycleStatus;
using TreeCore.BackEnd.Data.Repository.Project;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.Shared.Utilities.Auth;

using TreeCore.BackEnd.Service.Services.Project;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.Shared.DTO.Inventory;
using TreeCore.BackEnd.Model.Entity.Inventory;
using TreeCore.BackEnd.Data.Repository.Inventory;
using TreeCore.BackEnd.Service.Services.Inventory;
using TreeCore.BackEnd.Service.Services.Program;
using TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus;
using TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder;
#endregion

namespace TreeCore.BackEnd.API.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            #region AUTH
            services.
                AddScoped<UserRepository>().
                AddScoped<LoginService>().
                AddScoped<LoginDependence>();

            //Load info of Authentication
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);


            //config authentication
            // Configure Pasword reset Token lifespan
            services.Configure<PasswordResetTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(configuration.GetValue<int>("PasswordResetToken:LifespanInMinutes"));
            });

            // Configure JWT Authentication and Authorization
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidateAudience = jwtSettings.ValidateAudience,
                RequireExpirationTime = jwtSettings.RequireExpirationTime,
                ValidateLifetime = jwtSettings.ValidateLifetime,
                ClockSkew = jwtSettings.Expiration
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });
            #endregion

            #region BUSINESS PROCESS MANAGEMENT

            #region BUSINESS PROCESS
            services
                .AddScoped<GetObjectService<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper>, GetBusinessProcess>()
                .AddScoped<BaseRepository<BusinessProcessEntity>, BusinessProcessRepository>()
                .AddScoped<BusinessProcessDTO>()
                .AddScoped<GetDependencies<BusinessProcessDTO, BusinessProcessEntity>>()
                .AddScoped<GetBusinessProcess>()
                .AddScoped<PutDependencies<BusinessProcessEntity>>()
                .AddScoped<PutBusinessProcess>()
                .AddScoped<PostDependencies<BusinessProcessEntity>>()
                .AddScoped<PostBusinessProcess>()
                .AddScoped<DeleteDependencies<BusinessProcessEntity>>()
                .AddScoped<DeleteBusinessProcess>();
            #endregion

            #region BUSINESS PROCESS TYPE
            services
                .AddScoped<GetObjectService<BusinessProcessTypeDTO, BusinessProcessTypeEntity, BusinessProcessTypeDTOMapper>, GetBusinessProcessType>()
                .AddScoped<BaseRepository<BusinessProcessTypeEntity>, BusinessProcessTypeRepository>()
                .AddScoped<BusinessProcessTypeDTO>()
                .AddScoped<GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity>>()
                .AddScoped<GetBusinessProcessType>()
                .AddScoped<PutDependencies<BusinessProcessTypeEntity>>()
                .AddScoped<PutBusinessProcessType>()
                .AddScoped<PostDependencies<BusinessProcessTypeEntity>>()
                .AddScoped<PostBusinessProcessType>()
                .AddScoped<DeleteDependencies<BusinessProcessTypeEntity>>()
                .AddScoped<DeleteBusinessProcessType>();
            #endregion

            #endregion

            #region COMPANIES MANAGEMENT

            #region COMPANY 
            services
                .AddScoped<GetObjectService<CompanyDTO, CompanyEntity, CompanyDTOMapper>, GetCompany>()
                .AddScoped<BaseRepository<CompanyEntity>, CompanyRepository>()
                .AddScoped<CompanyDTO>()
                .AddScoped<CompanyDetailsDTO>()
                .AddScoped<GetDependencies<CompanyDTO, CompanyEntity>>()
                .AddScoped<GetDependencies<CompanyDetailsDTO, CompanyEntity>>()
                .AddScoped<GetCompany>()
                .AddScoped<PutDependencies<CompanyEntity>>()
                .AddScoped<PutCompany>()
                .AddScoped<PostDependencies<CompanyEntity>>()
                .AddScoped<PostCompany>()
                .AddScoped<DeleteDependencies<CompanyEntity>>()
                .AddScoped<DeleteCompany>();
            #endregion

            #region COMPANY TYPE
            services
                .AddScoped<GetObjectService<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper>, GetCompanyType>()
                .AddScoped<BaseRepository<CompanyTypeEntity>, CompanyTypeRepository>()
                .AddScoped<CompanyTypeDTO>()
                .AddScoped<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>>()
                .AddScoped<GetCompanyType>()
                .AddScoped<PutDependencies<CompanyTypeEntity>>()
                .AddScoped<PutCompanyType>()
                .AddScoped<PostDependencies<CompanyTypeEntity>>()
                .AddScoped<PostCompanyType>()
                .AddScoped<DeleteDependencies<CompanyTypeEntity>>()
                .AddScoped<DeleteCompanyType>();
            #endregion

            #region COMPANY ADDRESS
            services
                .AddScoped<GetObjectService<CompanyAddressDTO, CompanyAddressEntity, CompanyAddressDTOMapper>, GetCompanyAddress>()
                .AddScoped<BaseRepository<CompanyAddressEntity>, CompanyAddressRepository>()
                .AddScoped<CompanyAddressDTO>()
                .AddScoped<GetDependencies<CompanyAddressDTO, CompanyAddressEntity>>()
                .AddScoped<GetCompanyAddress>()
                .AddScoped<PutDependencies<CompanyAddressEntity>>()
                .AddScoped<PutCompanyAddress>()
                .AddScoped<PostDependencies<CompanyAddressEntity>>()
                .AddScoped<PostCompanyAddress>();
            #endregion

            #region COMPANY ASSIGNED PAYMENT METHODS
            services
                .AddScoped<GetObjectService<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity, CompanyAssignedPaymentMethodsDTOMapper>, GetCompanyAssignedPaymentMethods>()
                .AddScoped<BaseRepository<CompanyAssignedPaymentMethodsEntity>, CompanyAssignedPaymentMethodsRepository>()
                .AddScoped<CompanyAssignedPaymentMethodsDTO>()
                .AddScoped<GetDependencies<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity>>()
                .AddScoped<GetCompanyAssignedPaymentMethods>()
                .AddScoped<PutDependencies<CompanyAssignedPaymentMethodsEntity>>()
                .AddScoped<PutCompanyAssignedPaymentMethods>()
                .AddScoped<PostDependencies<CompanyAssignedPaymentMethodsEntity>>()
                .AddScoped<PostCompanyAssignedPaymentMethods>();
            #endregion


            #endregion

            #region CONFIG

            #region VIEWS


            services
                .AddScoped<GetObjectService<ViewDTO, ViewEntity, ViewDTOMapper>, GetView>()
                .AddScoped<BaseRepository<ViewEntity>, ViewRepository>()
                .AddScoped<ViewDTO>()
                .AddScoped<GetDependencies<ViewDTO, ViewEntity>>()
                .AddScoped<GetView>()
                .AddScoped<PutDependencies<ViewEntity>>()
                .AddScoped<PutView>()
                .AddScoped<PostDependencies<ViewEntity>>()
                .AddScoped<PostView>()
                .AddScoped<DeleteDependencies<ViewEntity>>()
                .AddScoped<DeleteView>()
                ;

            #endregion

            #endregion

            #region CONTRACT

            #region CONTRACT 
            services
                .AddScoped<GetObjectService<ContractDTO, ContractEntity, ContractDTOMapper>, GetContract>()
                .AddScoped<BaseRepository<ContractEntity>, ContractRepository>()
                .AddScoped<ContractDTO>()
                .AddScoped<GetDependencies<ContractDTO, ContractEntity>>()
                .AddScoped<GetContract>()
                .AddScoped<PutDependencies<ContractEntity>>()
                .AddScoped<PutContract>()
                .AddScoped<PostDependencies<ContractEntity>>()
                .AddScoped<PostContract>()
                .AddScoped<DeleteDependencies<ContractEntity>>()
                .AddScoped<DeleteContract>();
            #endregion

            #region CONTRACT GROUP
            services
                .AddScoped<GetObjectService<ContractGroupDTO, ContractGroupEntity, ContractGroupDTOMapper>, GetContractGroup>()
                .AddScoped<BaseRepository<ContractGroupEntity>, ContractGroupRepository>()
                .AddScoped<ContractGroupDTO>()
                .AddScoped<GetDependencies<ContractGroupDTO, ContractGroupEntity>>()
                .AddScoped<GetContractGroup>()
                .AddScoped<PutDependencies<ContractGroupEntity>>()
                .AddScoped<PutContractGroup>()
                .AddScoped<PostDependencies<ContractGroupEntity>>()
                .AddScoped<PostContractGroup>()
                .AddScoped<DeleteDependencies<ContractGroupEntity>>()
                .AddScoped<DeleteContractGroup>();
            #endregion

            #region CONTRACT HISTORY
            services
                .AddScoped<GetObjectService<ContractHistoryDTO, ContractHistoryEntity, ContractHistoryDTOMapper>, GetContractHistory>()
                .AddScoped<BaseRepository<ContractHistoryEntity>, ContractHistoryRepository>()
                .AddScoped<ContractHistoryDTO>()
                .AddScoped<GetDependencies<ContractHistoryDTO, ContractHistoryEntity>>()
                .AddScoped<GetContractHistory>();

            #endregion

            #region CONTRACT LINE

            services
               .AddScoped<GetObjectService<ContractLineDTO, ContractLineEntity, ContractLineDTOMapper>, GetContractLine>()
               .AddScoped<BaseRepository<ContractLineEntity>, ContractLineRepository>()
               .AddScoped<ContractLineDTO>()
               .AddScoped<GetDependencies<ContractLineDTO, ContractLineEntity>>()
               .AddScoped<GetContractLine>()
               .AddScoped<PutDependencies<ContractLineEntity>>()
               .AddScoped<PutContractLine>()
               .AddScoped<PostDependencies<ContractLineEntity>>()
               .AddScoped<PostContractLine>()
               .AddScoped<DeleteDependencies<ContractLineEntity>>();

            #endregion

            #region CONTRACT LINE TAXES

            services
              .AddScoped<GetObjectService<ContractLineTaxesDTO, ContractLineTaxesEntity, ContractLineTaxesDTOMapper>, GetContractLineTaxes>()
              .AddScoped<BaseRepository<ContractLineTaxesEntity>, ContractLineTaxesRepository>()
              .AddScoped<ContractLineTaxesDTO>()
              .AddScoped<GetDependencies<ContractLineTaxesDTO, ContractLineTaxesEntity>>()
              .AddScoped<GetContractLineTaxes>()
              .AddScoped<PutDependencies<ContractLineTaxesEntity>>()
              .AddScoped<PutContractLineTaxes>()
              .AddScoped<PostDependencies<ContractLineTaxesEntity>>()
              .AddScoped<PostContractLineTaxes>()
              .AddScoped<DeleteDependencies<ContractLineTaxesEntity>>();
            #endregion

            #region CONTRACT LINE COMPANIES

            services
              .AddScoped<GetObjectService<ContractLineEntidadDTO, ContractLineEntidadEntity, ContractLineEntidadDTOMapper>, GetContractLineEntidad>()
              .AddScoped<BaseRepository<ContractLineEntidadEntity>, ContractLineEntidadRepository>()
              .AddScoped<ContractLineEntidadDTO>()
              .AddScoped<GetDependencies<ContractLineEntidadDTO, ContractLineEntidadEntity>>()
              .AddScoped<GetContractLineEntidad>()
              .AddScoped<PutDependencies<ContractLineEntidadEntity>>()
              .AddScoped<PutContractLineEntidad>()
              .AddScoped<PostDependencies<ContractLineEntidadEntity>>()
              .AddScoped<PostContractLineEntidad>()
              .AddScoped<DeleteDependencies<ContractLineEntidadEntity>>();
            #endregion

            #region CONTRACT LINE TYPE
            services
                .AddScoped<GetObjectService<ContractLineTypeDTO, ContractLineTypeEntity, ContractLineTypeDTOMapper>, GetContractLineType>()
                .AddScoped<BaseRepository<ContractLineTypeEntity>, ContractLineTypeRepository>()
                .AddScoped<ContractLineTypeDTO>()
                .AddScoped<GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity>>()
                .AddScoped<GetContractLineType>()
                .AddScoped<PutDependencies<ContractLineTypeEntity>>()
                .AddScoped<PutContractLineType>()
                .AddScoped<PostDependencies<ContractLineTypeEntity>>()
                .AddScoped<PostContractLineType>()
                .AddScoped<DeleteDependencies<ContractLineTypeEntity>>()
                .AddScoped<DeleteContractLineType>();
            #endregion

            #region CONTRACT TYPE
            services
                .AddScoped<GetObjectService<ContractTypeDTO, ContractTypeEntity, ContractTypeDTOMapper>, GetContractType>()
                .AddScoped<BaseRepository<ContractTypeEntity>, ContractTypeRepository>()
                .AddScoped<ContractTypeDTO>()
                .AddScoped<GetDependencies<ContractTypeDTO, ContractTypeEntity>>()
                .AddScoped<GetContractType>()
                .AddScoped<PutDependencies<ContractTypeEntity>>()
                .AddScoped<PutContractType>()
                .AddScoped<PostDependencies<ContractTypeEntity>>()
                .AddScoped<PostContractType>()
                .AddScoped<DeleteDependencies<ContractTypeEntity>>()
                .AddScoped<DeleteContractType>();
            #endregion

            #region CONTRACT STATE
            services
                .AddScoped<GetObjectService<ContractStatusDTO, ContractStatusEntity, ContractStatusDTOMapper>, GetContractStatus>()
                .AddScoped<BaseRepository<ContractStatusEntity>, ContractStateRepository>()
                .AddScoped<ContractStatusDTO>()
                .AddScoped<GetDependencies<ContractStatusDTO, ContractStatusEntity>>()
                .AddScoped<GetContractStatus>()
                .AddScoped<PutDependencies<ContractStatusEntity>>()
                .AddScoped<PutContractStatus>()
                .AddScoped<PostDependencies<ContractStatusEntity>>()
                .AddScoped<PostContractStatus>()
                .AddScoped<DeleteDependencies<ContractStatusEntity>>()
                .AddScoped<DeleteContractStatus>();
            #endregion

            #region FUNCTIONAL AREA
            services
                .AddScoped<GetObjectService<FunctionalAreaDTO, FunctionalAreaEntity, FuncionalAreaDTOMapper>, GetFunctionalArea>()
                .AddScoped<BaseRepository<FunctionalAreaEntity>, FunctionalAreaRepository>()
                .AddScoped<FunctionalAreaDTO>()
                .AddScoped<GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity>>()
                .AddScoped<GetFunctionalArea>()
                .AddScoped<PutDependencies<FunctionalAreaEntity>>()
                .AddScoped<PutFunctionalArea>()
                .AddScoped<PostDependencies<FunctionalAreaEntity>>()
                .AddScoped<PostFunctionalArea>()
                .AddScoped<DeleteDependencies<FunctionalAreaEntity>>()
                .AddScoped<DeleteFunctionalArea>();
            #endregion          

            #endregion

            #region GENERAL

            #region BANK
            services
                .AddScoped<GetObjectService<BankDTO, BankEntity, BankDTOMapper>, GetBank>()
                .AddScoped<BaseRepository<BankEntity>, BankRepository>()
                .AddScoped<BankDTO>()
                .AddScoped<GetDependencies<BankDTO, BankEntity>>()
                .AddScoped<GetBank>()
                .AddScoped<PutDependencies<BankEntity>>()
                .AddScoped<PutBank>()
                .AddScoped<PostDependencies<BankEntity>>()
                .AddScoped<PostBank>()
                .AddScoped<DeleteDependencies<BankEntity>>()
                .AddScoped<DeleteBank>();
            #endregion

            #region PROFILE
            services
                .AddScoped<GetObjectService<ProfileDTO, ProfileEntity, ProfileDTOMapper>, GetProfile>()
                .AddScoped<BaseRepository<ProfileEntity>, ProfileRepository>()
                .AddScoped<ProfileDTO>()
                .AddScoped<GetDependencies<ProfileDTO, ProfileEntity>>()
                .AddScoped<GetProfile>()
                .AddScoped<PutDependencies<ProfileEntity>>()
                .AddScoped<PutProfile>()
                .AddScoped<PostDependencies<ProfileEntity>>()
                .AddScoped<PostProfile>()
                .AddScoped<DeleteDependencies<ProfileEntity>>()
                .AddScoped<DeleteProfile>();
            #endregion

            #region ROL
            services
                .AddScoped<GetObjectService<RolDTO, RolEntity, RolDTOMapper>, GetRol>()
                .AddScoped<BaseRepository<RolEntity>, RolRepository>()
                .AddScoped<RolDTO>()
                .AddScoped<GetDependencies<RolDTO, RolEntity>>()
                .AddScoped<GetRol>()
                .AddScoped<PutDependencies<RolEntity>>()
                .AddScoped<PutRol>()
                .AddScoped<PostDependencies<RolEntity>>()
                .AddScoped<PostRol>()
                .AddScoped<DeleteDependencies<RolEntity>>()
                .AddScoped<DeleteRol>();
            #endregion

            #region BANK ACCOUNT
            services
                .AddScoped<GetObjectService<BankAccountDTO, BankAccountEntity, BankAccountDTOMapper>, GetBankAccount>()
                .AddScoped<BaseRepository<BankAccountEntity>, BankAccountRepository>()
                .AddScoped<BankAccountDTO>()
                .AddScoped<GetDependencies<BankAccountDTO, BankAccountEntity>>()
                .AddScoped<GetBankAccount>()
                .AddScoped<PutDependencies<BankAccountEntity>>()
                .AddScoped<PutBankAccount>()
                .AddScoped<PostDependencies<BankAccountEntity>>()
                .AddScoped<PostBankAccount>()
                .AddScoped<DeleteDependencies<BankAccountEntity>>();
            #endregion

            #region COUNTRY
            services
                .AddScoped<GetObjectService<CountryDTO, CountryEntity, CountryDTOMapper>, GetCountry>()
                .AddScoped<BaseRepository<CountryEntity>, CountryRepository>()
                .AddScoped<CountryDTO>()
                .AddScoped<GetDependencies<CountryDTO, CountryEntity>>()
                .AddScoped<GetCountry>();
            #endregion

            #region CURRENCY
            services
                .AddScoped<GetObjectService<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper>, GetCurrency>()
                .AddScoped<BaseRepository<CurrencyEntity>, CurrencyRepository>()
                .AddScoped<CurrencyDTO>()
                .AddScoped<GetDependencies<CurrencyDTO, CurrencyEntity>>()
                .AddScoped<GetCurrency>()
                .AddScoped<PutDependencies<CurrencyEntity>>()
                .AddScoped<PutCurrency>()
                .AddScoped<PostDependencies<CurrencyEntity>>()
                .AddScoped<PostCurrency>()
                .AddScoped<DeleteDependencies<CurrencyEntity>>()
                .AddScoped<DeleteCurrency>();
            #endregion

            #region CRON
            services
                .AddScoped<BaseRepository<CronEntity>, CronRepository>()
                .AddScoped<CronDTO>()
                .AddScoped<GetDependencies<CronDTO, CronEntity>>()
                .AddScoped<PutDependencies<CronEntity>>()
                .AddScoped<PostDependencies<CronEntity>>()
                .AddScoped<DeleteDependencies<CronEntity>>()
                ;
            #endregion

            #region INFLATION
            services
                .AddScoped<GetObjectService<InflationDTO, InflationEntity, InflationDTOMapper>, GetInflation>()
                .AddScoped<BaseRepository<InflationEntity>, InflationRepository>()
                .AddScoped<InflationDTO>()
                .AddScoped<GetDependencies<InflationDTO, InflationEntity>>()
                .AddScoped<GetInflation>()
                .AddScoped<PutDependencies<InflationEntity>>()
                .AddScoped<PutInflation>()
                .AddScoped<PostDependencies<InflationEntity>>()
                .AddScoped<PostInflation>()
                .AddScoped<DeleteDependencies<InflationEntity>>()
                .AddScoped<DeleteInflation>();
            #endregion

            #region PAYMENT METHODS 
            services
                .AddScoped<GetObjectService<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper>, GetPaymentMethods>()
                .AddScoped<BaseRepository<PaymentMethodsEntity>, PaymentMethodsRepository>()
                .AddScoped<PaymentMethodsDTO>()
                .AddScoped<GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity>>()
                .AddScoped<GetPaymentMethods>()
                .AddScoped<PutDependencies<PaymentMethodsEntity>>()
                .AddScoped<PutPaymentMethods>()
                .AddScoped<PostDependencies<PaymentMethodsEntity>>()
                .AddScoped<PostPaymentMethods>()
                .AddScoped<DeleteDependencies<PaymentMethodsEntity>>()
                .AddScoped<DeletePaymentMethods>();
            #endregion

            #region PAYMENT TERM 
            services
                .AddScoped<GetObjectService<PaymentTermDTO, PaymentTermEntity, PaymentTermDTOMapper>, GetPaymentTerm>()
                .AddScoped<BaseRepository<PaymentTermEntity>, PaymentTermRepository>()
                .AddScoped<PaymentTermDTO>()
                .AddScoped<GetDependencies<PaymentTermDTO, PaymentTermEntity>>()
                .AddScoped<GetPaymentTerm>()
                .AddScoped<PutDependencies<PaymentTermEntity>>()
                .AddScoped<PutPaymentTerm>()
                .AddScoped<PostDependencies<PaymentTermEntity>>()
                .AddScoped<PostPaymentTerm>()
                .AddScoped<DeleteDependencies<PaymentTermEntity>>()
                .AddScoped<DeletePaymentTerm>();
            #endregion           

            #region TAX IDENTIFICATION NUMBERCATEGORY 
            services
                .AddScoped<GetObjectService<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity, TaxIdentificationNumberCategoryDTOMapper>, GetTaxIdentificationNumberCategory>()
                .AddScoped<BaseRepository<TaxIdentificationNumberCategoryEntity>, TaxIdentificationNumberCategoryRepository>()
                .AddScoped<TaxIdentificationNumberCategoryDTO>()
                .AddScoped<GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity>>()
                .AddScoped<GetTaxIdentificationNumberCategory>()
                .AddScoped<PutDependencies<TaxIdentificationNumberCategoryEntity>>()
                .AddScoped<PutTaxIdentificationNumberCategory>()
                .AddScoped<PostDependencies<TaxIdentificationNumberCategoryEntity>>()
                .AddScoped<PostTaxIdentificationNumberCategory>()
                .AddScoped<DeleteDependencies<TaxIdentificationNumberCategoryEntity>>()
                .AddScoped<DeleteTaxIdentificationNumberCategory>();
            #endregion

            #region TAXES
            services
                .AddScoped<GetObjectService<TaxesDTO, TaxesEntity, TaxesDTOMapper>, GetTaxes>()
                .AddScoped<BaseRepository<TaxesEntity>, TaxesRepository>()
                .AddScoped<TaxesDTO>()
                .AddScoped<GetDependencies<TaxesDTO, TaxesEntity>>()
                .AddScoped<GetTaxes>()
                .AddScoped<PutDependencies<TaxesEntity>>()
                .AddScoped<PutTaxes>()
                .AddScoped<PostDependencies<TaxesEntity>>()
                .AddScoped<PostTaxes>()
                .AddScoped<DeleteDependencies<TaxesEntity>>()
                .AddScoped<DeleteTaxes>();
            #endregion

            #region TAXPAYER TYPE
            services
                .AddScoped<GetObjectService<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper>, GetTaxpayerType>()
                .AddScoped<BaseRepository<TaxpayerTypeEntity>, TaxpayerTypeRepository>()
                .AddScoped<TaxpayerTypeDTO>()
                .AddScoped<GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity>>()
                .AddScoped<GetTaxpayerType>()
                .AddScoped<PutDependencies<TaxpayerTypeEntity>>()
                .AddScoped<PutTaxpayerType>()
                .AddScoped<PostDependencies<TaxpayerTypeEntity>>()
                .AddScoped<PostTaxpayerType>()
                .AddScoped<DeleteDependencies<TaxpayerTypeEntity>>()
                .AddScoped<DeleteTaxpayerType>();
            #endregion

            #region USER
            services
                .AddScoped<GetObjectService<UserDTO, UserEntity, UserDTOMapper>, GetUser>()
                .AddScoped<BaseRepository<UserEntity>, UserRepository>()
                .AddScoped<UserDTO>()
                .AddScoped<GetDependencies<UserDTO, UserEntity>>()
                .AddScoped<GetUser>()
                .AddScoped<PutDependencies<UserEntity>>()
                //.AddScoped<PutUser>()
                .AddScoped<PostDependencies<UserEntity>>()
                //.AddScoped<PostUser>()
                .AddScoped<DeleteDependencies<UserEntity>>()
                //.AddScoped<DeleteUser>()
                ;
            #endregion

            #endregion

            #region IMPORT EXPORT

            #region IMPORT TASK
            services
                .AddScoped<GetObjectService<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper>, GetImportTask>()
                .AddScoped<BaseRepository<ImportTaskEntity>, ImportTaskRepository>()
                .AddScoped<ImportTaskDTO>()
                .AddScoped<GetDependencies<ImportTaskDTO, ImportTaskEntity>>()
                .AddScoped<GetImportTask>()
                .AddScoped<PutDependencies<ImportTaskEntity>>()
                .AddScoped<PutImportTask>()
                .AddScoped<PostDependencies<ImportTaskEntity>>()
                .AddScoped<PostImportTask>()
                .AddScoped<DeleteDependencies<ImportTaskEntity>>()
                .AddScoped<DeleteImportTask>();
            #endregion

            #region IMPORT TYPE
            services
                .AddScoped<GetObjectService<ImportTypeDTO, ImportTypeEntity, ImportTypeDTOMapper>, GetImportType>()
                .AddScoped<BaseRepository<ImportTypeEntity>, ImportTypeRepository>()
                .AddScoped<ImportTypeDTO>()
                .AddScoped<GetDependencies<ImportTypeDTO, ImportTypeEntity>>()
                .AddScoped<GetImportType>();
            //.AddScoped<PutDependencies<ImportTypeEntity>>()
            //.AddScoped<PutImportType>()
            //.AddScoped<PostDependencies<ImportTypeEntity>>()
            //.AddScoped<PostImportType>()
            //.AddScoped<DeleteDependencies<ImportTypeEntity>>()
            //.AddScoped<DeleteImportType>();
            #endregion

            #endregion

            #region INVENTORY
            services
                .AddScoped<GetObjectService<InventoryDTO, InventoryEntity, InventoryDTOMapper>, GetInventory>()
                .AddScoped<BaseRepository<InventoryEntity>, InventoryRepository>()
                .AddScoped<InventoryDTO>()
                .AddScoped<GetDependencies<InventoryDTO, InventoryEntity>>();
            #endregion

            #region PRODUCT CATALOG

            #region CATALOG
            services
                .AddScoped<GetObjectService<CatalogDTO, CatalogEntity, CatalogDTOMapper>, GetCatalog>()
                .AddScoped<BaseRepository<CatalogEntity>, CatalogRepository>()
                .AddScoped<CatalogDTO>()
                .AddScoped<GetDependencies<CatalogDTO, CatalogEntity>>()
                .AddScoped<GetCatalog>()
                .AddScoped<PutDependencies<CatalogEntity>>()
                .AddScoped<PutCatalog>()
                .AddScoped<PostDependencies<CatalogEntity>>()
                .AddScoped<PostCatalog>()
                .AddScoped<DeleteDependencies<CatalogEntity>>()
                .AddScoped<DeleteCatalog>()
                ;
            #endregion

            #region CATALOG ASSIGNED PRODUCTS
            services
                .AddScoped<GetObjectService<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity, CatalogAssignedProductsDTOMapper>, GetCatalogAssignedProducts>()
                .AddScoped<BaseRepository<CatalogAssignedProductsEntity>, CatalogAssignedProductsRepository>()
                .AddScoped<CatalogAssignedProductsDTO>()
                .AddScoped<GetDependencies<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity>>()
                .AddScoped<GetCatalogAssignedProducts>();

            #endregion

            #region CATALOG LIFECYCLE STATUS
            services
                .AddScoped<GetObjectService<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity, CatalogLifecycleStatusDTOMapper>, GetCatalogLifecycleStatus>()
                .AddScoped<BaseRepository<CatalogLifecycleStatusEntity>, CatalogLifecycleStatusRepository>()
                .AddScoped<CatalogLifecycleStatusDTO>()
                .AddScoped<GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity>>()
                .AddScoped<GetCatalogLifecycleStatus>()
                .AddScoped<PutDependencies<CatalogLifecycleStatusEntity>>()
                .AddScoped<PutCatalogLifecycleStatus>()
                .AddScoped<PostDependencies<CatalogLifecycleStatusEntity>>()
                .AddScoped<PostCatalogLifecycleStatus>()
                .AddScoped<DeleteDependencies<CatalogLifecycleStatusEntity>>()
                .AddScoped<DeleteCatalogLifecycleStatus>();
            #endregion

            #region CATALOG TYPE
            services
                .AddScoped<GetObjectService<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper>, GetCatalogType>()
                .AddScoped<BaseRepository<CatalogTypeEntity>, CatalogTypeRepository>()
                .AddScoped<CatalogTypeDTO>()
                .AddScoped<GetDependencies<CatalogTypeDTO, CatalogTypeEntity>>()
                .AddScoped<GetCatalogType>()
                .AddScoped<PutDependencies<CatalogTypeEntity>>()
                .AddScoped<PutCatalogType>()
                .AddScoped<PostDependencies<CatalogTypeEntity>>()
                .AddScoped<PostCatalogType>()
                .AddScoped<DeleteDependencies<CatalogTypeEntity>>()
                .AddScoped<DeleteCatalogType>()
                ;
            #endregion

            #region PRODUCT
            services
                .AddScoped<GetObjectService<ProductDTO, ProductEntity, ProductDTOMapper>, GetProduct>()
                .AddScoped<BaseRepository<ProductEntity>, ProductRepository>()
                .AddScoped<ProductDTO>()
                .AddScoped<ProductDetailsDTO>()
                .AddScoped<GetDependencies<ProductDTO, ProductEntity>>()
                .AddScoped<GetDependencies<ProductDetailsDTO, ProductEntity>>()
                .AddScoped<GetProduct>()
                .AddScoped<PutDependencies<ProductEntity>>()
                .AddScoped<PutProduct>()
                .AddScoped<PostDependencies<ProductEntity>>()
                .AddScoped<PostProduct>()
                .AddScoped<DeleteDependencies<ProductEntity>>()
                .AddScoped<DeleteProduct>();
            #endregion

            #region PRODUCT TYPE
            services
                .AddScoped<GetObjectService<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper>, GetProductType>()
                .AddScoped<BaseRepository<ProductTypeEntity>, ProductTypeRepository>()
                .AddScoped<ProductTypeDTO>()
                .AddScoped<GetDependencies<ProductTypeDTO, ProductTypeEntity>>()
                .AddScoped<GetProductType>()
                .AddScoped<PutDependencies<ProductTypeEntity>>()
                .AddScoped<PutProductType>()
                .AddScoped<PostDependencies<ProductTypeEntity>>()
                .AddScoped<PostProductType>()
                .AddScoped<DeleteDependencies<ProductTypeEntity>>()
                .AddScoped<DeleteProductType>();
            #endregion

            #endregion

            #region PROJECT

            #region PROGRAM
            services
                .AddScoped<GetObjectService<ProgramDTO, ProgramEntity, ProgramDTOMapper>, GetProgram>()
                .AddScoped<BaseRepository<ProgramEntity>, ProgramRepository>()
                .AddScoped<ProgramDTO>()
                .AddScoped<GetDependencies<ProgramDTO, ProgramEntity>>()
                .AddScoped<GetProgram>()
                .AddScoped<PutDependencies<ProgramEntity>>()
                .AddScoped<PutProgram>()
                .AddScoped<PostDependencies<ProgramEntity>>()
                .AddScoped<PostProgram>()
                .AddScoped<DeleteDependencies<ProgramEntity>>()
                .AddScoped<DeleteProgram>();
            #endregion

            #region PROJECT
            services
                .AddScoped<GetObjectService<ProjectDTO, ProjectEntity, ProjectDTOMapper>, GetProject>()
                .AddScoped<BaseRepository<ProjectEntity>, ProjectRepository>()
                .AddScoped<ProjectDTO>()
                .AddScoped<GetDependencies<ProjectDTO, ProjectEntity>>()
                .AddScoped<GetProject>()
                .AddScoped<PutDependencies<ProjectEntity>>()
                .AddScoped<PutProject>()
                .AddScoped<PostDependencies<ProjectEntity>>()
                .AddScoped<PostProject>()
                .AddScoped<DeleteDependencies<ProjectEntity>>()
                .AddScoped<DeleteProject>();
            #endregion

            #region PROJECT LIFE CYCLE STATUS
            services
                .AddScoped<GetObjectService<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper>, GetProjectLifeCycleStatus>()
                .AddScoped<BaseRepository<ProjectLifeCycleStatusEntity>, ProjectLifeCycleStatusRepository>()
                .AddScoped<ProjectLifeCycleStatusDTO>()
                .AddScoped<GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity>>()
                .AddScoped<GetProjectLifeCycleStatus>()
                .AddScoped<PutDependencies<ProjectLifeCycleStatusEntity>>()
                .AddScoped<PutProjectLifeCycleStatus>()
                .AddScoped<PostDependencies<ProjectLifeCycleStatusEntity>>()
                .AddScoped<PostProjectLifeCycleStatus>()
                .AddScoped<DeleteDependencies<ProjectLifeCycleStatusEntity>>()
                .AddScoped<DeleteProjectLifeCycleStatus>();
            #endregion

            #endregion

            #region SITES
            services
                .AddScoped<GetObjectService<SiteDTO, SiteEntity, SiteDTOMapper>, GetSite>()
                .AddScoped<BaseRepository<SiteEntity>, SiteRepository>()
                .AddScoped<SiteDTO>()
                .AddScoped<GetDependencies<SiteDTO, SiteEntity>>();
            #endregion

            #region WORKFLOWS MANAGEMENT

            #region WORKFLOWS
            services
                .AddScoped<GetObjectService<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper>, GetWorkflow>()
                .AddScoped<BaseRepository<WorkflowEntity>, WorkflowRepository>()
                .AddScoped<WorkflowDTO>()
                .AddScoped<GetDependencies<WorkflowDTO, WorkflowEntity>>()
                .AddScoped<GetWorkflow>()
                .AddScoped<PutDependencies<WorkflowEntity>>()
                .AddScoped<PutWorkflow>()
                .AddScoped<PostDependencies<WorkflowEntity>>()
                .AddScoped<PostWorkflow>()
                .AddScoped<DeleteDependencies<WorkflowEntity>>()
                .AddScoped<DeleteWorkflow>();
            #endregion

            #region WORKFLOWS NEXT  STATUS
            services
                .AddScoped<GetObjectService<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity, WorkFlowNextStatusDTOMapper>, GetWorkFlowNextStatus>()
                .AddScoped<BaseRepository<WorkFlowNextStatusEntity>, WorkFlowNextStatusRepository>()
                .AddScoped<WorkFlowNextStatusDTO>()
                .AddScoped<GetDependencies<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity>>()
                .AddScoped<GetWorkFlowNextStatus>()
                .AddScoped<PutDependencies<WorkFlowNextStatusEntity>>()
                .AddScoped<PutWorkFlowNextStatus>()
                .AddScoped<PostDependencies<WorkFlowNextStatusEntity>>()
                .AddScoped<PostWorkFlowNextStatus>()
                .AddScoped<DeleteDependencies<WorkFlowNextStatusEntity>>();
            #endregion

            #region WORKFLOWS STATUS
            services
                .AddScoped<GetObjectService<WorkFlowStatusDTO, WorkFlowStatusEntity, WorkFlowStatusDTOMapper>, GetWorkFlowStatus>()
                .AddScoped<BaseRepository<WorkFlowStatusEntity>, WorkFlowStatusRepository>()
                .AddScoped<WorkFlowStatusDTO>()
                .AddScoped<GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity>>()
                .AddScoped<GetWorkFlowStatus>()
                .AddScoped<PutDependencies<WorkFlowStatusEntity>>()
                .AddScoped<PutWorkFlowStatus>()
                .AddScoped<PostDependencies<WorkFlowStatusEntity>>()
                .AddScoped<PostWorkFlowStatus>()
                .AddScoped<DeleteDependencies<WorkFlowStatusEntity>>()
                .AddScoped<DeleteWorkFlowStatus>();
            #endregion

            #region WORKFLOW STATUS GROUP
            services
                .AddScoped<GetObjectService<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper>, GetWorkFlowStatusGroup>()
                .AddScoped<BaseRepository<WorkFlowStatusGroupEntity>, WorkFlowStatusGroupRepository>()
                .AddScoped<WorkFlowStatusGroupDTO>()
                .AddScoped<GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity>>()
                .AddScoped<GetWorkFlowStatusGroup>()
                .AddScoped<PutDependencies<WorkFlowStatusGroupEntity>>()
                .AddScoped<PutWorkFlowStatusGroup>()
                .AddScoped<PostDependencies<WorkFlowStatusGroupEntity>>()
                .AddScoped<PostWorkFlowStatusGroup>()
                .AddScoped<DeleteDependencies<WorkFlowStatusGroupEntity>>()
                .AddScoped<DeleteWorkFlowStatusGroup>();
            #endregion

            #endregion

            #region WORK ORDERS MANAGEMENT

            #region WORK ORDERS
            services
                .AddScoped<GetObjectService<WorkOrderDTO, WorkOrderEntity, WorkOrderDTOMapper>, GetWorkOrder>()
                .AddScoped<BaseRepository<WorkOrderEntity>, WorkOrderRepository>()
                .AddScoped<WorkOrderDTO>()
                .AddScoped<GetDependencies<WorkOrderDTO, WorkOrderEntity>>()
                .AddScoped<GetWorkOrder>()
                .AddScoped<PutDependencies<WorkOrderEntity>>()
                .AddScoped<PutWorkOrder>()
                .AddScoped<PostDependencies<WorkOrderEntity>>()
                .AddScoped<PostWorkOrder>();
            #endregion

            #region WORK ORDERS LIFE CYCLE STATUS
            services
                .AddScoped<GetObjectService<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper>, GetWorkOrderLifecycleStatus>()
                .AddScoped<BaseRepository<WorkOrderLifecycleStatusEntity>, WorkOrderLifecycleStatusRepository>()
                .AddScoped<WorkOrderLifecycleStatusDTO>()
                .AddScoped<GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity>>()
                .AddScoped<GetWorkOrderLifecycleStatus>()
                .AddScoped<PutDependencies<WorkOrderLifecycleStatusEntity>>()
                .AddScoped<PutWorkOrderLifecycleStatus>()
                .AddScoped<PostDependencies<WorkOrderLifecycleStatusEntity>>()
                .AddScoped<PostWorkOrderLifecycleStatus>()
                .AddScoped<DeleteDependencies<WorkOrderLifecycleStatusEntity>>()
                .AddScoped<DeleteWorkOrderLifecycleStatus>();
            #endregion

            #region WORK ORDERS TRACKING STATUS
            services
                .AddScoped<GetObjectService<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity, WorkOrderTrackingStatusDTOMapper>, GetWorkOrderTrackingStatus>()
                .AddScoped<BaseRepository<WorkOrderTrackingStatusEntity>, WorkOrderTrackingStatusRepository>()
                .AddScoped<WorkOrderTrackingStatusDTO>()
                .AddScoped<GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity>>()
                .AddScoped<GetWorkOrderTrackingStatus>()
                .AddScoped<PutDependencies<WorkOrderTrackingStatusEntity>>()
                .AddScoped<PutWorkOrderTrackingStatus>()
                .AddScoped<PostDependencies<WorkOrderTrackingStatusEntity>>()
                .AddScoped<PostWorkOrderTrackingStatus>()
                .AddScoped<DeleteDependencies<WorkOrderTrackingStatusEntity>>()
                .AddScoped<DeleteWorkOrderTrackingStatus>();
            #endregion

            #endregion           

            return services;
        }
    }
}
