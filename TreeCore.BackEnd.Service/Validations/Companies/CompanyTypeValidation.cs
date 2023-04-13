using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CompanyTypeValidation : BasicValidation<CompanyTypeDTO, CompanyTypeEntity>
    {
        public override Result<CompanyTypeEntity> ValidateEntity(CompanyTypeEntity companyType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (companyType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (companyType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (companyType.EntidadTipo.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CompanyTypeEntity>(listaErrores.ToImmutableArray())
                : companyType;
        }

        public override Result<CompanyTypeDTO> ValidateDTO(CompanyTypeDTO companyType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (companyType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (companyType.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (companyType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CompanyTypeDTO>(listaErrores.ToImmutableArray())
                : companyType;
        }
    }
}
