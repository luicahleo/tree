using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class RolValidation : BasicValidation<RolDTO, RolEntity>
    {
        public override Result<RolEntity> ValidateEntity(RolEntity rol, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (rol.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (rol.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (rol.Descripcion.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<RolEntity>(listaErrores.ToImmutableArray())
                : rol;
        }

        public override Result<RolDTO> ValidateDTO(RolDTO rol, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (rol.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (rol.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (rol.Description.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<RolDTO>(listaErrores.ToImmutableArray())
                : rol;
        }
    }
}
