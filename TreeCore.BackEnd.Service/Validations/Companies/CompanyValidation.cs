using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CompanyValidation : BasicValidation<CompanyDTO, CompanyEntity>
    {
        public override Result<CompanyEntity> ValidateEntity(CompanyEntity company, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (company.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.Email != "" && !IsValidEmail(company.Email))
            {
                listaErrores.Add(Error.Create(_traduccion.EmailFormatError));
            }



            return listaErrores.Any() ?
                Result.Failure<CompanyEntity>(listaErrores.ToImmutableArray())
                : company;
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override Result<CompanyDTO> ValidateDTO(CompanyDTO company, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (company.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (company.Alias.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.Phone.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.Email.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.CompanyTypeCode.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CompanyDTO>(listaErrores.ToImmutableArray())
                : company;
        }
    }
}
