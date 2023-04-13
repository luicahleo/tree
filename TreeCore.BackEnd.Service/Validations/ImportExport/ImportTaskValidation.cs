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
    public class ImportTaskValidation : BasicValidation<ImportTaskDTO, ImportTaskEntity>
    {
        public override Result<ImportTaskEntity> ValidateEntity(ImportTaskEntity task, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (task.DocumentoCarga.Length > 250)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (task.RutaDocumento.Length > 500)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ImportTaskEntity>(listaErrores.ToImmutableArray())
                : task;
        }

        public override Result<ImportTaskDTO> ValidateDTO(ImportTaskDTO task, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (task.Code.Length > 250)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (task.Document.Document.Length > 500)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ImportTaskDTO>(listaErrores.ToImmutableArray())
                : task;
        }
    }
}

