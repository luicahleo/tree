using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CompanyAssignedPaymentMethodsValidation : BasicValidation<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity>
    {
        public override Result<CompanyAssignedPaymentMethodsEntity> ValidateEntity(CompanyAssignedPaymentMethodsEntity companyType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();


            return listaErrores.Any() ?
                Result.Failure<CompanyAssignedPaymentMethodsEntity>(listaErrores.ToImmutableArray())
                : companyType;
        }

        public override Result<CompanyAssignedPaymentMethodsDTO> ValidateDTO(CompanyAssignedPaymentMethodsDTO companyType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            

            return listaErrores.Any() ?
                Result.Failure<CompanyAssignedPaymentMethodsDTO>(listaErrores.ToImmutableArray())
                : companyType;
        }
    }
}
