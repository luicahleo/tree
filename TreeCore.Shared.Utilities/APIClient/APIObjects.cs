using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using TreeCore.Shared.DTO;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.APIClient
{
    public static class APIObjects
    {
        public static string rutaAPI;
        public static Hashtable ObjectsRoutes = new Hashtable()
        {
            { typeof(CatalogTypeDTO), "v1/CatalogTypeManagement/CatalogType"},
            { typeof(ProductTypeDTO), "v1/ProductTypeManagement/ProductType"},
            { typeof(CompanyTypeDTO), "v1/CompanyTypeManagement/CompanyType"},
            { typeof(CompanyDTO), "v1/CompanyManagement/Company"},
            { typeof(ProductDTO), "v1/ProductManagement/Product"},
            { typeof(ImportTaskDTO), "v1/ImportTaskManagement/ImportTask"},
            { typeof(PaymentTermDTO), "v1/PaymentTermManagement/PaymentTerm"},
            { typeof(TaxpayerTypeDTO), "v1/TaxpayerTypeManagement/TaxpayerType"},
            { typeof(InflationDTO), "v1/InflationManagement/Inflation"},
            { typeof(CurrencyDTO), "v1/CurrencyManagement/Currency"},
            { typeof(TaxesDTO), "v1/TaxesManagement/Taxes"},
            { typeof(PaymentMethodsDTO), "v1/PaymentMethodsManagement/PaymentMethods"},
            { typeof(TaxIdentificationNumberCategoryDTO), "v1/TaxIdentificationNumberCategoryManagement/TaxIdentificationNumberCategory"},
            { typeof(ContractLineTypeDTO), "v1/ContractLineTypeManagement/ContractLineType"},
            { typeof(BankDTO), "v1/BankManagement/Bank"},
            { typeof(WorkflowDTO), "v1/WorkflowManagement/Workflow"},
            { typeof(ContractStatusDTO), "v1/ContractStateManagement/ContractState"}
        };



        public static string GetRuta(Type c){
            return $"v1/{c.Name.Replace("DTO", "")}Management/{c.Name.Replace("DTO", "")}";
        }

        public const string LOGIN = "v1/Auth/login";
    }
}