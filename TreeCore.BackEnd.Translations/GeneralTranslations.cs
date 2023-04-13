using System.Globalization;
using TreeCore.Shared.Language;

namespace TreeCore.BackEnd.Translations
{
    public class GeneralTranslations
    {
        private readonly CultureInfo _culture;
        public GeneralTranslations(CultureInfo culture)
        {
            _culture = culture;
        }

        public string BusinessProcess => LocalizationUtils<GeneralTranslations>.GetValue("strBusinessProcess", _culture);
        public string BusinessProcessType => LocalizationUtils<GeneralTranslations>.GetValue("strBusinessProcessType", _culture);
        public string CatalogType => LocalizationUtils<GeneralTranslations>.GetValue("strCatalogType", _culture);
        public string Code => LocalizationUtils<GeneralTranslations>.GetValue("strCode", _culture);
        public string CodeBank => LocalizationUtils<GeneralTranslations>.GetValue("strCodeBank", _culture);
        public string CodeBankAccount => LocalizationUtils<GeneralTranslations>.GetValue("strCodeBankAccount", _culture);
        public string CodeCatalog => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCatalog", _culture);
        public string CodeCatalogLifecycleStatus => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCatalogLifecycleStatus", _culture);       
        public string CodeCatalogType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCatalogType", _culture);
        public string CodeContract => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContract", _culture);
        public string CodeContractDate => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractDate", _culture);
        public string CodeContractFrequencyMonth => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractFrequencyMonth", _culture);
        public string CodeContractRenewalNumber => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractRenewalNumber", _culture);
        public string CodeContractCurrentRenewalNumber => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCodeContractCurrentRenewalNumber", _culture);
        public string CodeContractRenewalDate => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractRenewalDate", _culture);
        public string CodeContractGroup => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractGroup", _culture);
        public string CodeContractLine => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractLine", _culture);
        public string CodeContractLineEntidad => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractLineEntidad", _culture);
        public string CodeContractLineTaxes => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractLineTaxes", _culture);
        public string CodeContractLineType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractLineType", _culture);
        public string CodeView => LocalizationUtils<GeneralTranslations>.GetValue("strCodeView", _culture);       
        public string CodeContractState => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractState", _culture);
        public string CodeContractType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeContractType", _culture);        
        public string CodeCompany => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCompany", _culture);
        public string CodeCompanyAddress => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCompanyAddress", _culture);
        public string CodeCompanyType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCompanyType", _culture);      
        public string CodeCurrency => LocalizationUtils<GeneralTranslations>.GetValue("strCodeCurrency", _culture);
        public string CodeFunctionalArea => LocalizationUtils<GeneralTranslations>.GetValue("strCodeFunctionalArea", _culture);
        public string CodeInflation => LocalizationUtils<GeneralTranslations>.GetValue("strCodeInflation", _culture);
        public string CodeInflationType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeInflationTipo", _culture);
        public string CodePaymentMethods => LocalizationUtils<GeneralTranslations>.GetValue("strCodePaymentMethods", _culture);
        public string CodePaymentTerm => LocalizationUtils<GeneralTranslations>.GetValue("strCodePaymentTerm", _culture);
        public string CodeProductType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeProductType", _culture);
        public string CodeSite => LocalizationUtils<GeneralTranslations>.GetValue("strCodeSite", _culture);
        public string CodeSAPTypeNIF => LocalizationUtils<GeneralTranslations>.GetValue("strCodeSAPTypeNIF", _culture);
        public string CodeTaxes => LocalizationUtils<GeneralTranslations>.GetValue("strCodeTaxes", _culture);
        public string CodeTaxpayerType => LocalizationUtils<GeneralTranslations>.GetValue("strCodeTaxpayerType", _culture);
        public string CodeWorkFlowStatus => LocalizationUtils<GeneralTranslations>.GetValue("strCodeWorkFlowStatus", _culture);
        public string CodeWorkflow => LocalizationUtils<GeneralTranslations>.GetValue("strCodeWorkflow", _culture);
        public string WorkFlowStatusGroup => LocalizationUtils<GeneralTranslations>.GetValue("strWorkFlowStatusGroup", _culture);
        public string CodeWorkOrderTrackingStatus => LocalizationUtils<GeneralTranslations>.GetValue("strCodeWorkOrderTrakingStatus", _culture);
        public string Company => LocalizationUtils<GeneralTranslations>.GetValue("strCompany", _culture);        
        public string Currency => LocalizationUtils<GeneralTranslations>.GetValue("strCurrency", _culture);
        public string DefectoYaExiste => LocalizationUtils<GeneralTranslations>.GetValue("strDefecto", _culture);
        public string Email => LocalizationUtils<GeneralTranslations>.GetValue("strEmail", _culture);       
        public string Inflation => LocalizationUtils<GeneralTranslations>.GetValue("strInflation", _culture);
        public string ImportTask => LocalizationUtils<GeneralTranslations>.GetValue("strImportTask", _culture);
        public string ImportType => LocalizationUtils<GeneralTranslations>.GetValue("strImportType", _culture);
        public string IsPack => LocalizationUtils<GeneralTranslations>.GetValue("strIsPack", _culture);
        public string NameCountry => LocalizationUtils<GeneralTranslations>.GetValue("strNameCountry", _culture);
        public string Product => LocalizationUtils<GeneralTranslations>.GetValue("strProduct", _culture);
        public string Profile => LocalizationUtils<GeneralTranslations>.GetValue("strProfile", _culture);
        public string Program => LocalizationUtils<GeneralTranslations>.GetValue("strProgram", _culture);
        public string ProductWithCode => LocalizationUtils<GeneralTranslations>.GetValue("strProductWithCode", _culture);
        public string ProjectLifeCycleStatus => LocalizationUtils<GeneralTranslations>.GetValue("strProjectLifeCycleStatus", _culture);
        public string Project => LocalizationUtils<GeneralTranslations>.GetValue("strProject", _culture);
        public string Prorrogas => LocalizationUtils<GeneralTranslations>.GetValue("strProrrogas", _culture);
        public string Rol => LocalizationUtils<GeneralTranslations>.GetValue("strRol", _culture);
        public string NotFound { get; set; }
        public string User => LocalizationUtils<GeneralTranslations>.GetValue("strUser", _culture);
        public string Workflow => LocalizationUtils<GeneralTranslations>.GetValue("strWorkflow", _culture);
        public string WorkOrder => LocalizationUtils<GeneralTranslations>.GetValue("strWorkOrder", _culture);
        public string WorkOrderTrackingStatus => LocalizationUtils<GeneralTranslations>.GetValue("strWorkOrderTrackingStatus", _culture);

    }
}
