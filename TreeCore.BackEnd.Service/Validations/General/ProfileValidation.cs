using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ProfileValidation : BasicValidation<ProfileDTO, ProfileEntity>
    {
        public override Result<ProfileEntity> ValidateEntity(ProfileEntity taxpayerType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            return listaErrores.Any() ?
                Result.Failure<ProfileEntity>(listaErrores.ToImmutableArray())
                : taxpayerType;
        }

        public override Result<ProfileDTO> ValidateDTO(ProfileDTO taxpayerType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxpayerType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxpayerType.Description.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxpayerType.ModuleCode.Length > 10)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProfileDTO>(listaErrores.ToImmutableArray())
                : taxpayerType;
        }
    }
}
