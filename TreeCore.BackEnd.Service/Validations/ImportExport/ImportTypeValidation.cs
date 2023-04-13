using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ImportTypeValidation : BasicValidation<ImportTypeDTO, ImportTypeEntity>
    {
        public override Result<ImportTypeEntity> ValidateEntity(ImportTypeEntity task, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (task.DocumentoCargaPlantilla.Length > 250)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (task.RutaDocumento.Length > 500)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ImportTypeEntity>(listaErrores.ToImmutableArray())
                : task;
        }

        public override Result<ImportTypeDTO> ValidateDTO(ImportTypeDTO task, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (task.Code.Length > 250)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (task.Document.Length > 500)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ImportTypeDTO>(listaErrores.ToImmutableArray())
                : task;
        }
    }
}

