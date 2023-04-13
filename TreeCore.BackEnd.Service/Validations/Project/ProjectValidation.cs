using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ProjectValidation : BasicValidation<ProjectDTO, ProjectEntity>
    {
        public override Result<ProjectEntity> ValidateEntity(ProjectEntity Project, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (Project.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Project.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Project.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProjectEntity>(listaErrores.ToImmutableArray())
                : Project;
        }

        public override Result<ProjectDTO> ValidateDTO(ProjectDTO Project, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (Project.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Project.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Project.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProjectDTO>(listaErrores.ToImmutableArray())
                : Project;
        }
    }
}
