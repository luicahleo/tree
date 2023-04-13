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

    public class PutWorkFlowNextStatus : PutObjectService<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity, WorkFlowNextStatusDTOMapper>
    {
        private readonly GetDependencies<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity> _getDependency;
        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getWorkFlowStatusDependency;

        public PutWorkFlowNextStatus(PutDependencies<WorkFlowNextStatusEntity> putDependency,
                GetDependencies<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity> getDependency,
                GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getWorkFlowStatusDependency,
        IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, putDependency, new WorkFlowNextStatusValidation())
        {
            _getDependency = getDependency;
            _getWorkFlowStatusDependency = getWorkFlowStatusDependency;
        }

        public override async Task<Result<WorkFlowNextStatusEntity>> ValidateEntity(WorkFlowNextStatusDTO nextStatus, int clientID, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            WorkFlowStatusEntity bank = await _getWorkFlowStatusDependency.GetItemByCode(nextStatus.WorkFlowStatusCode, clientID);
            if (bank == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{nextStatus.WorkFlowStatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }


            // WorkFlowNextStatusEntity companyEntity = new WorkFlowNextStatusEntity(null, nextStatus.Code, nextStatus.IBAN, nextStatus.Description, nextStatus.SWIFT, company, bank);
            filter = new Filter(nameof(WorkFlowNextStatusDTO.WorkFlowStatusCode), Operators.eq, nextStatus.WorkFlowStatusCode);
            listFilters.Add(filter);

            IEnumerable<WorkFlowNextStatusEntity> listBankAccounts = await _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listBankAccounts != null && listBankAccounts.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " - " + _traduccion.DefectoYaExiste + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkFlowNextStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return null;
                //return companyEntity;
            }
        }

        public async Task<Result<List<WorkFlowNextStatusEntity>>> ValidateEntity(List<WorkFlowNextStatusDTO> linkedNextStatus, WorkFlowStatusDTO status, List<WorkFlowStatusDTO> linkedStatus, int clientID, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<WorkFlowNextStatusEntity> linkedWorkFlowNextStatusEntity = new List<WorkFlowNextStatusEntity>();
            if (numDefectos(linkedNextStatus) == 0)
            {
                linkedNextStatus[0].Default = true;
            }
            foreach (WorkFlowNextStatusDTO nextStatusList in linkedNextStatus)
            {

                if (nextStatusList != null){
					if(nextStatusList.Default && controlRepetido(linkedNextStatus, nextStatusList))
                	{

                    	lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " - " + _traduccion.DefectoYaExiste + "."));
                	}

                	WorkFlowStatusDTO statusDTO = controlExiste(linkedStatus, nextStatusList, status.Code);
                	if(statusDTO == null)
                	{
                    	lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{nextStatusList.WorkFlowStatusCode}" + " " + _errorTraduccion.NotFound + "."));
                	}
                	else
                	{
                        WorkFlowStatusEntity nextStatus;
                        nextStatus = await _getWorkFlowStatusDependency.GetItemByCode(statusDTO.Code, clientID);
                        //WorkFlowStatusEntity statusAdd = new WorkFlowStatusEntity(null, statusDTO.Code, statusDTO.Name, statusDTO.Description, statusDTO.Percentage, statusDTO.Complete, statusDTO.PublicReading,
                        //	statusDTO.PublicWriting, null, statusDTO.Active, statusDTO.Default, null, null, null, null);
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

