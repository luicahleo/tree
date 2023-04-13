using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CompanyAddressValidation : BasicValidation<CompanyAddressDTO, CompanyAddressEntity>
    {
        public override Result<CompanyAddressEntity> ValidateEntity(CompanyAddressEntity companyAddress, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (companyAddress.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }


            if (companyAddress.EntidadDireccion.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CompanyAddressEntity>(listaErrores.ToImmutableArray())
                : companyAddress;
        }

        public override Result<CompanyAddressDTO> ValidateDTO(CompanyAddressDTO companyAddress, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (companyAddress.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (companyAddress.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CompanyAddressDTO>(listaErrores.ToImmutableArray())
                : companyAddress;
        }
    }
}
