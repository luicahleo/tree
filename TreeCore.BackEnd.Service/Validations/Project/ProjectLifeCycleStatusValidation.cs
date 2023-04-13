using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.ROP;

using TreeCore.Shared.DTO.Project;
using TreeCore.BackEnd.Model.Entity.Project;

namespace TreeCore.BackEnd.Service.Validations.Project
{
    class ProjectLifeCycleStatusValidation : BasicValidation<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity>
    {
        public override Result<ProjectLifeCycleStatusEntity> ValidateEntity(ProjectLifeCycleStatusEntity projectLifeCycleStatus, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (projectLifeCycleStatus.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (projectLifeCycleStatus.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (projectLifeCycleStatus.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (projectLifeCycleStatus.Color.Length > 10)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProjectLifeCycleStatusEntity>(listaErrores.ToImmutableArray())
                : projectLifeCycleStatus;
        }

        public override Result<ProjectLifeCycleStatusDTO> ValidateDTO(ProjectLifeCycleStatusDTO projectLifeCycleStatus, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (projectLifeCycleStatus.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (projectLifeCycleStatus.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (projectLifeCycleStatus.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (projectLifeCycleStatus.Colour.Length > 10)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProjectLifeCycleStatusDTO>(listaErrores.ToImmutableArray())
                : projectLifeCycleStatus;
        }
    }
}
