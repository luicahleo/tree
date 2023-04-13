using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{

    public class PostWorkFlowNextStatus : PostObjectService<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity, WorkFlowNextStatusDTOMapper>
    {
        private readonly GetDependencies<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity> _getDependency;
        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getWorkFlowStatusDependency;

        public PostWorkFlowNextStatus(PostDependencies<WorkFlowNextStatusEntity> postDependency,
                GetDependencies<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity> getDependency,
                GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getWorkFlowStatusDependency,
        IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkFlowNextStatusValidation())
        {
            _getDependency = getDependency;
            _getWorkFlowStatusDependency = getWorkFlowStatusDependency;
        }

        public override async Task<Result<WorkFlowNextStatusEntity>> ValidateEntity(WorkFlowNextStatusDTO nextStatus, int clientID, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            filter = new Filter(nameof(WorkFlowNextStatusDTO.WorkFlowStatusCode), Operators.eq, nextStatus.WorkFlowStatusCode);
            listFilters.Add(filter);

            Task<IEnumerable<WorkFlowNextStatusEntity>> listBankAccounts = _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listBankAccounts.Result != null && listBankAccounts.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeBankAccount + " " + $"{nextStatus.WorkFlowStatusCode}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkFlowNextStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return null;
            }
        }

        public async Task<Result<List<WorkFlowNextStatusEntity>>> ValidateEntity(List<WorkFlowNextStatusDTO> linkednextStatus, WorkFlowStatusDTO status, List<WorkFlowStatusDTO> linkedStatus, int clientID)
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<WorkFlowNextStatusEntity> linkedWorkFlowNextStatusEntity = new List<WorkFlowNextStatusEntity>();
            WorkFlowStatusDTO statusDTO = null;
            if (numDefectos(linkednextStatus) == 0)
            {
                linkednextStatus[0].Default = true;
            }
            foreach (WorkFlowNextStatusDTO nextStatusList in linkednextStatus)
            {
                if (nextStatusList != null)
                {
                    if (nextStatusList.Default && controlRepetido(linkednextStatus, nextStatusList))
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " - " + _traduccion.DefectoYaExiste + "."));

                    }


                    statusDTO = controlExiste(linkedStatus, nextStatusList, status.Code);
                    if (statusDTO == null)
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{nextStatusList.WorkFlowStatusCode}" + " " + _errorTraduccion.NotFound + "."));
                    }
                    else
                    {
                        WorkFlowStatusEntity nextStatus;
                        nextStatus = await _getWorkFlowStatusDependency.GetItemByCode(statusDTO.Code, clientID);
                        if(nextStatus == null)
                        {
                            nextStatus = new WorkFlowStatusEntity(null, statusDTO.Code, statusDTO.Name, statusDTO.Description, statusDTO.TimeFrame, statusDTO.Complete, statusDTO.PublicReading,
                            statusDTO.PublicWriting, null, statusDTO.Active, statusDTO.Default, null, null, null, null);
                        }
                        linkedWorkFlowNextStatusEntity.Add(new WorkFlowNextStatusEntity(null, null, nextStatus, nextStatusList.Default));
                    }
                }
            };


            if (lErrors.Count > 0)
            {
                return Result.Failure<List<WorkFlowNextStatusEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return linkedWorkFlowNextStatusEntity;
            }
        }

        private bool controlRepetido(List<WorkFlowNextStatusDTO> lista, WorkFlowNextStatusDTO elemento)
        {
            int cont = 0;
            foreach (WorkFlowNextStatusDTO nextStatus in lista)
            {
                if (elemento.Default == nextStatus.Default)
                {
                    cont++;
                }
            }
            if (cont > 1)
            {
                return true;
            }
            return false;
        }
        
        private int numDefectos(List<WorkFlowNextStatusDTO> lista)
        {
            int cont = 0;
            foreach (WorkFlowNextStatusDTO nextStatus in lista)
            {
                if (nextStatus.Default)
                {
                    cont++;
                }
            }
            return cont;
        }

        private WorkFlowStatusDTO controlExiste(List<WorkFlowStatusDTO> lista, WorkFlowNextStatusDTO elemento, string code)
        {
            foreach (WorkFlowStatusDTO nextStatus in lista)
            {
                if (elemento.WorkFlowStatusCode == nextStatus.Code && nextStatus.Code != code)
                {
                    return nextStatus;
                }
            }

            return null;
        }

    }
}

