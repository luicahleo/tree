using System.Globalization;
using TreeCore.Shared.Language;
using TreeCore.BackEnd.Translations;

namespace TreeCore.BackEnd.Translations
{
    public class ErrorTranslations
    {
        private readonly CultureInfo _culture;
        
        public ErrorTranslations(CultureInfo culture)
        {
            _culture = culture;

        }

        public string AlreadyExist => LocalizationUtils<ErrorTranslations>.GetValue("strAlreadyExist", _culture);    
        public string DateError => LocalizationUtils<ErrorTranslations>.GetValue("strDateError", _culture);
        public string DateIncorrect => LocalizationUtils<ErrorTranslations>.GetValue("strDateIncorrect", _culture);
        public string DateVsDuracion => LocalizationUtils<ErrorTranslations>.GetValue("strDateVsDuracion", _culture);/////The start date cannot be higher than the firstend date
        public string DefaultAlreadyExist => LocalizationUtils<ErrorTranslations>.GetValue("strDefaultAlreadyExist", _culture);
        public string DefaultInactive => LocalizationUtils<ErrorTranslations>.GetValue("strDefaultInactive", _culture);
        public string DeleteDefault => LocalizationUtils<ErrorTranslations>.GetValue("strDeleteDefault", _culture);
        public string EmailFormatError => LocalizationUtils<ErrorTranslations>.GetValue("strEmailFormatError", _culture);
        public string EmptyField => LocalizationUtils<ErrorTranslations>.GetValue("strEmptyField", _culture);
        public string ErrorFK => LocalizationUtils<ErrorTranslations>.GetValue("strErrorFK", _culture);
        public string ErrorPack => LocalizationUtils<ErrorTranslations>.GetValue("strErrorPack", _culture);
        public string ExecutionDateError => LocalizationUtils<ErrorTranslations>.GetValue("strExecutionDate", _culture);//The Execution date cannot be higher than the start date
        public string FormatError => LocalizationUtils<ErrorTranslations>.GetValue("strFormatError", _culture);
        public string FormatIncorrect => LocalizationUtils<ErrorTranslations>.GetValue("strFormatIncorrect", _culture);
        public string IsnotSupplier => LocalizationUtils<ErrorTranslations>.GetValue("strIsnotSupplier", _culture);
        public string IsRequiredBool => LocalizationUtils<ErrorTranslations>.GetValue("strIsRequiredBool", _culture);
        public string InconsistentValues => LocalizationUtils<ErrorTranslations>.GetValue("strInconsistentValues", _culture);
        public string MaxLengthText => LocalizationUtils<ErrorTranslations>.GetValue("strMaxLengthText", _culture);
        public string NotFound => LocalizationUtils<ErrorTranslations>.GetValue("strNotFound", _culture);
        public string NotPossibleBool => LocalizationUtils<ErrorTranslations>.GetValue("strNotPossibleBool", _culture);
        public string Percent => LocalizationUtils<ErrorTranslations>.GetValue("strPercent", _culture); //the percentage cannot exceed 100
        public string RangeInvalid => LocalizationUtils<ErrorTranslations>.GetValue("strRangeInvalid", _culture); //the percentage cannot exceed 100
        public string StartDate => LocalizationUtils<ErrorTranslations>.GetValue("strStartDate", _culture);/////The start date cannot be higher than the firstend date
        public string ValidFromDate => LocalizationUtils<ErrorTranslations>.GetValue("strValidFromDate", _culture);//the valid from date,valid to date and next payment date are inconsistents
        public string ValidFromDateSinglePayment => LocalizationUtils<ErrorTranslations>.GetValue("strValidFromDateSinglePayment", _culture);//As the payment is unique,the payment date must be the same
        public string ValidFromDateVsRenewDate => LocalizationUtils<ErrorTranslations>.GetValue("strValidFromDateVsRenewDate", _culture);//The valid from  date cannot be higher than the Renew date or tan start date contract
        public string ValidFromDateVsFirstEndDate => LocalizationUtils<ErrorTranslations>.GetValue("strValidFromDateVsFirstEndDate", _culture);//The valid from  date cannot be higher than the Renew date or tan start date contract
        public string ValidFromDateVsStartDateReadjustment => LocalizationUtils<ErrorTranslations>.GetValue("strValidFromDateVsStartDateReadjustment", _culture);//The valid from  date cannot be higher than the readjustment start date
        public string CodeContractLine => LocalizationUtils<ErrorTranslations>.GetValue("strCodeContractLine", _culture);
        public string ValidFrecuency => LocalizationUtils<ErrorTranslations>.GetValue("strValidFrecuency", _culture);
    }
}
