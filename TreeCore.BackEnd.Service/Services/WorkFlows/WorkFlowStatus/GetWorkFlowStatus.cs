using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{
    public class GetWorkFlowStatus : GetObjectService<WorkFlowStatusDTO, WorkFlowStatusEntity, WorkFlowStatusDTOMapper>
    {
        GetDependencies<WorkflowDTO, WorkflowEntity> _getWorkflowDependency;

        public GetWorkFlowStatus(GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getDependencies,
            GetDependencies<WorkflowDTO, WorkflowEntity> getWorkflowDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            _getWorkflowDependency = getWorkflowDependency;
        }

        public async Task<Result<IEnumerable<WorkFlowStatusDTO>>> GetListByWorkFlow(string code, int Client)
        {
            List<Filter> filters = new List<Filter>();
            List<Error> lErrors = new List<Error>();

            WorkflowEntity obj = await _getWorkflowDependency.GetItemByCode(code, Client);
            Result<IEnumerable<WorkFlowStatusEntity>> entityList;
            List<WorkFlowStatusDTO> lista = new List<WorkFlowStatusDTO>();
            int totalItems = 0;
            if (obj != null)
            {

                filters.Add(new Filter(nameof(WorkflowEntity.CoreWorkFlowID).ToLower(), Operators.eq, obj.CoreWorkFlowID.Value, Filter.Types.AND, null));

                IEnumerable<WorkFlowStatusEntity> StatusIdentty = await _getDependencies.GetList(Client, filters, null, null, -1, -1);

                entityList = (StatusIdentty == null ?
                    Result.Failure<IEnumerable<WorkFlowStatusEntity>>(Error.Create(_errorTraduccion.NotFound)) :
                    new Result<IEnumerable<WorkFlowStatusEntity>>(StatusIdentty));
                foreach (var item in entityList.Valor)
                {
                    lista.Add(await _mapper.Map(item));
                    totalItems = (entityList.Valor.FirstOrDefault() != null) ? entityList.Valor.FirstOrDefault().TotalItems : 0;
                }
            }
            else
            {
               return Result.Failure<IEnumerable<WorkFlowStatusDTO>>(Error.Create(_traduccion.Code + " " + _traduccion.Workflow + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            return new Result<IEnumerable<WorkFlowStatusDTO>>(lista, totalItems);

        }
        public async Task<List<WorkFlowStatusDTO>> GetListByWorkFlowV2(string code, int Client)
        {
            List<Filter> filters = new List<Filter>();
            List<Error> lErrors = new List<Error>();

            WorkflowEntity obj = await _getWorkflowDependency.GetItemByCode(code, Client);
            List<WorkFlowStatusDTO> lista = new List<WorkFlowStatusDTO>();
            if (obj != null)
            {

                foreach (var item in obj.WorkflowsEstados)
                {
                    lista.Add(await _mapper.Map(item));
                }

            }
            return lista;

        }
    }
}
