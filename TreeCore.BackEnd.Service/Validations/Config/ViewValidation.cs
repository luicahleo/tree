using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ViewValidation : BasicValidation<ViewDTO, ViewEntity>
    {
        public override Result<ViewEntity> ValidateEntity(ViewEntity view, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (view.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (view.Pagina.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ViewEntity>(listaErrores.ToImmutableArray())
                : view;
        }

        public override Result<ViewDTO> ValidateDTO(ViewDTO view, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (view.Code.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (view.Page.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ViewDTO>(listaErrores.ToImmutableArray())
                : view;
        }
    }
}
