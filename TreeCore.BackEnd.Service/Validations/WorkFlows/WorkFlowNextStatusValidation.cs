using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class WorkFlowNextStatusValidation : BasicValidation<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity>
    {
        public override Result<WorkFlowNextStatusEntity> ValidateEntity(WorkFlowNextStatusEntity companyType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();


            return listaErrores.Any() ?
                Result.Failure<WorkFlowNextStatusEntity>(listaErrores.ToImmutableArray())
                : companyType;
        }

        public override Result<WorkFlowNextStatusDTO> ValidateDTO(WorkFlowNextStatusDTO companyType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            

            return listaErrores.Any() ?
                Result.Failure<WorkFlowNextStatusDTO>(listaErrores.ToImmutableArray())
                : companyType;
        }
    }
}
