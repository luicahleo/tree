using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{

    public class PutWorkFlowStatus : PutObjectService<WorkFlowStatusDTO, WorkFlowStatusEntity, WorkFlowStatusDTOMapper>
    {
        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getDependency;
        private readonly GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> _getStatusGroupDependency;
        private readonly GetDependencies<WorkflowDTO, WorkflowEntity> _getWorkflowDependency;
        private readonly GetDependencies<RolDTO, RolEntity> _getRolesDependency;
        private readonly PutWorkFlowNextStatus _putNextStatus;
        private readonly GetWorkFlowStatus _getStatus;

        public PutWorkFlowStatus(PutDependencies<WorkFlowStatusEntity> putDependency,
            GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getDependency,
            GetDependencies<WorkflowDTO, WorkflowEntity> getWorkflowDependency,
            GetDependencies<RolDTO, RolEntity> getRolesDependency,
            GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> getStatusGroupDependency,
            PutWorkFlowNextStatus putNextStatus,
            GetWorkFlowStatus getStatus,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new WorkFlowStatusValidation())
        {
            _getDependency = getDependency;
            _getRolesDependency = getRolesDependency;
            _getWorkflowDependency = getWorkflowDependency;
            _getStatusGroupDependency = getStatusGroupDependency;
            _putNextStatus = putNextStatus;
            _getStatus = getStatus;
        }
        public override async Task<Result<WorkFlowStatusEntity>> ValidateEntity(WorkFlowStatusDTO status, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            IEnumerable<WorkFlowStatusEntity> listWorkFlowStatus;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkflowEntity workflow = null;
            int? workflowID = null;
            WorkFlowStatusEntity statusFinal = null;
            IEnumerable<RolEntity> linkedRolesWriting = null;
            IEnumerable<RolEntity> linkedRolesReading = null;
            IEnumerable<RolEntity> linkedRolesWorkflow = null;

            WorkFlowStatusEntity? workFlowStatus = await _getDependency.GetItemByCode(code, Client);
            if (workFlowStatus == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (status.Default && !status.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                WorkFlowStatusGroupEntity statusGroup = await _getStatusGroupDependency.GetItemByCode(status.StatusGroupCode, Client);
                if (statusGroup == null)
                {
                    lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkFlowStatusGroup + " " + $"{status.Code}" + " " + _errorTraduccion.NotFound + "."));
                }

                if (codesDict != null && codesDict.Count > 0 && codesDict["LEVEL1"] != "" && codesDict["LEVEL2"] != "")
                {
                    workflow = await _getWorkflowDependency.GetItemByCode(codesDict["LEVEL1"], Client);
                    if (workflow == null)
                    {
                        lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.Workflow + " " + $"{codesDict["LEVEL1"]}" + " " + _errorTraduccion.NotFound + "."));
                    }
                    else
                    {
                        workflowID = workflow.CoreWorkFlowID;
                        linkedRolesWorkflow = workflow.WorkflowsRoles;

                        if (status != null && status.LinkedRolesWriting != null && status.LinkedRolesWriting.Count > 0 && status.LinkedRolesWriting[0] != null)
                        {
                            List<Filter> filtersRoles = new List<Filter>();

                            foreach (string codeLinkedRolesWriting in status.LinkedRolesWriting)
                            {
                                filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesWriting, Filter.Types.OR, null));
                            }

                            linkedRolesWriting = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                            if (!workflow.Publico)
                            {
                                IEnumerable<string> iEcodesRolesWriting = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                    iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                if (workflow.WorkflowsRoles != null && workflow.WorkflowsRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        foreach (RolEntity rolesWriting in workflow.WorkflowsRoles)
                                        {
                                            if (rolesWriting.Codigo != scodeRolesWriting)
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        }
                                    };
                                }

                                else if (iEcodesRolesWriting == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                }

                                else if (iEcodesRolesWriting.Count() > 0 && lpRolesWriting.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        if (!iEcodesRolesWriting.Contains(scodeRolesWriting))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + workflow.Codigo + "'.", null));
                                        }
                                    };
                                }
                            }
                            else
                            {
                                IEnumerable<string> iEcodesRolesWriting = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                    iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                if (workflow.WorkflowsRoles != null && workflow.WorkflowsRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        foreach (RolEntity rolesWriting in workflow.WorkflowsRoles)
                                        {
                                            if (rolesWriting.Codigo == scodeRolesWriting)
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        }
                                    };
                                }

                                else if (iEcodesRolesWriting == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                }
                            }
                        }

                        if (status != null && status.LinkedRolesReading != null && status.LinkedRolesReading.Count > 0 && status.LinkedRolesReading[0] != null)
                        {
                            List<Filter> filtersRoles = new List<Filter>();

                            foreach (string codeLinkedRolesReading in status.LinkedRolesReading)
                            {
                                filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesReading, Filter.Types.OR, null));
                            }

                            linkedRolesReading = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                            if (!workflow.Publico)
                            {
                                IEnumerable<string> iEcodesRolesReading = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                    iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                if (workflow.WorkflowsRoles != null && workflow.WorkflowsRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        foreach (RolEntity rolesReading in workflow.WorkflowsRoles)
                                        {
                                            if (rolesReading.Codigo != scodeRolesReading)
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        }
                                    };
                                }

                                else if (iEcodesRolesReading == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                }

                                else if (iEcodesRolesReading.Count() > 0 && lpRolesReading.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (!iEcodesRolesReading.Contains(scodeRolesReading))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                            + workflow.Codigo + "'.", null));
                                        }
                                    };
                                }
                            }
                            else
                            {
                                IEnumerable<string> iEcodesRolesReading = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                    iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                if (workflow.WorkflowsRoles != null && workflow.WorkflowsRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        foreach (RolEntity rolesReading in workflow.WorkflowsRoles)
                                        {
                                            if (rolesReading.Codigo != scodeRolesReading)
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        }
                                    };
                                }

                                else if (iEcodesRolesReading == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                }

                                else if (iEcodesRolesReading.Count() > 0 && lpRolesReading.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (iEcodesRolesReading.Contains(scodeRolesReading))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is in the list of possible roles in the workflow '"
                                            + workflow.Codigo + "'.", null));
                                        }
                                    };
                                }
                            }
                        }
                    }
                }
                string WorkFlowCode = codesDict["LEVEL1"];
                List<WorkFlowStatusDTO> listStatus = await _getStatus.GetListByWorkFlowV2(WorkFlowCode, Client);
                IEnumerable<WorkFlowNextStatusEntity> listNextStatus = new List<WorkFlowNextStatusEntity>();

                if (status.LinkedWorkFlowNextStatus != null && status.LinkedWorkFlowNextStatus.Count > 0)
                {
                    Result<List<WorkFlowNextStatusEntity>> listNextStatusValidity = await _putNextStatus.ValidateEntity(status.LinkedWorkFlowNextStatus, status, listStatus, Client);


                    if (listNextStatusValidity.Success)
                    {
                        listNextStatus = listNextStatusValidity.Value;
                    }
                    else
                    {
                        lErrors.AddRange(listNextStatusValidity.Errors);
                    }
                }
                statusFinal = new WorkFlowStatusEntity(workFlowStatus.CoreEstadoID, status.Code, status.Name, status.Description, status.TimeFrame,
                    status.Complete, status.PublicReading, status.PublicWriting, statusGroup, status.Active, status.Default, workflow, listNextStatus,
                    linkedRolesWriting, linkedRolesReading);

                filter = new Filter(nameof(WorkFlowStatusDTO.Code), Operators.eq, status.Code);
                listFilters.Add(filter);

                listWorkFlowStatus = await _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listWorkFlowStatus != null && listWorkFlowStatus.ListOrEmpty().Count > 0 &&
                    listWorkFlowStatus.ListOrEmpty()[0].CoreEstadoID != statusFinal.CoreEstadoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{status.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (status.Default)
                {
                    filter = new Filter(nameof(WorkFlowStatusDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listWorkFlowStatus = await _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listWorkFlowStatus != null && listWorkFlowStatus.ListOrEmpty().Count > 0)
                    {
                        WorkFlowStatusEntity pType = new WorkFlowStatusEntity(listWorkFlowStatus.ListOrEmpty()[0].CoreEstadoID.Value, listWorkFlowStatus.ListOrEmpty()[0].Codigo,
                            listWorkFlowStatus.ListOrEmpty()[0].Nombre, listWorkFlowStatus.ListOrEmpty()[0].Descripcion, listWorkFlowStatus.ListOrEmpty()[0].Tiempo,
                            listWorkFlowStatus.ListOrEmpty()[0].Completado, listWorkFlowStatus.ListOrEmpty()[0].PublicoLectura, listWorkFlowStatus.ListOrEmpty()[0].PublicoEscritura,
                            listWorkFlowStatus.ListOrEmpty()[0].EstadosAgrupaciones, listWorkFlowStatus.ListOrEmpty()[0].Activo, false, listWorkFlowStatus.ListOrEmpty()[0].WorkFlow,
                            listWorkFlowStatus.ListOrEmpty()[0].EstadosSiguientes, listWorkFlowStatus.ListOrEmpty()[0].EstadosRolesEscritura, listWorkFlowStatus.ListOrEmpty()[0].EstadosRolesLectura);
                        await _putDependencies.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkFlowStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return statusFinal;
            }
        }

        public async Task<Result<List<WorkFlowStatusEntity>>> ValidateEntity(List<WorkFlowStatusDTO> oEntidad, int Client, WorkflowDTO workF, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            Filter filter2;
            WorkflowEntity workflow = null;
            int? workflowID = null;
            IEnumerable<RolEntity> linkedRolesWriting = null;
            IEnumerable<RolEntity> linkedRolesReading = null;
            IEnumerable<RolEntity> linkedRolesWorkflow = null;
            List<string> linkedRoleswFlow = null;

            List<WorkFlowStatusEntity> linkedStatusEntity = new List<WorkFlowStatusEntity>();
            if (numDefectos(oEntidad) > 1)
            {
                lErrors.Add(Error.Create(_traduccion.DefectoYaExiste + "."));
            }
            foreach (WorkFlowStatusDTO status in oEntidad)
            {
                if (status != null)
                {

                    if (status.Default && !status.Active)
                    {
                        lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                    }
                    if (controlRepetido(oEntidad, status))
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{status.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                    }

                    WorkFlowStatusGroupEntity statusGroup = await _getStatusGroupDependency.GetItemByCode(status.StatusGroupCode, Client);
                    if (statusGroup == null)
                    {
                        lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkFlowStatusGroup + " " + $"{status.Code}" + " " + _errorTraduccion.NotFound + "."));
                    }

                    if (codesDict != null && codesDict.Count > 0 && codesDict["LEVEL1"] != "" && codesDict["LEVEL2"] != "")
                    {
                        workflow = await _getWorkflowDependency.GetItemByCode(codesDict["LEVEL1"], Client);
                        if (workflow == null)
                        {
                            lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.Workflow + " " + $"{codesDict["LEVEL1"]}" + " " + _errorTraduccion.NotFound + "."));
                        }
                        else
                        {
                            workflowID = workflow.CoreWorkFlowID;
                            linkedRolesWorkflow = workflow.WorkflowsRoles;

                            if (status != null && status.LinkedRolesWriting != null && status.LinkedRolesWriting.Count > 0 && status.LinkedRolesWriting[0] != null)
                            {
                                List<Filter> filtersRoles = new List<Filter>();

                                foreach (string codeLinkedRolesWriting in status.LinkedRolesWriting)
                                {
                                    filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesWriting, Filter.Types.OR, null));
                                }

                                linkedRolesWriting = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                                if (!workflow.Publico)
                                {
                                    IEnumerable<string> iEcodesRolesWriting = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                    IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                    IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                        iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                    if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                    {
                                        foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                        {
                                            if (!workF.LinkedRoles.Contains(scodeRolesWriting))
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        };
                                    }

                                    else if (iEcodesRolesWriting == null)
                                    {
                                        lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                    }

                                    else if (iEcodesRolesWriting.Count() > 0 && lpRolesWriting.Count() > 0)
                                    {
                                        foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                        {
                                            if (!iEcodesRolesWriting.Contains(scodeRolesWriting))
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        };
                                    }
                                }
                                else
                                {
                                    IEnumerable<string> iEcodesRolesWriting = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                    IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                    IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                        iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                    if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                    {
                                        foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                        {
                                            if (workF.LinkedRoles.Contains(scodeRolesWriting))
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        };
                                    }

                                    else if (iEcodesRolesWriting == null)
                                    {
                                        lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                    }
                                }
                            }

                            if (status != null && status.LinkedRolesReading != null && status.LinkedRolesReading.Count > 0 && status.LinkedRolesReading[0] != null)
                            {
                                List<Filter> filtersRoles = new List<Filter>();

                                foreach (string codeLinkedRolesReading in status.LinkedRolesReading)
                                {
                                    filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesReading, Filter.Types.OR, null));
                                }

                                linkedRolesReading = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                                if (!workflow.Publico)
                                {
                                    IEnumerable<string> iEcodesRolesReading = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                    IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                    IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                        iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                    if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                    {
                                        foreach (string scodeRolesReading in lpRolesReading.ToList())
                                        {
                                            if (!workF.LinkedRoles.Contains(scodeRolesReading))
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        };
                                    }

                                    else if (iEcodesRolesReading == null)
                                    {
                                        lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                    }

                                    else if (iEcodesRolesReading.Count() > 0 && lpRolesReading.Count() > 0)
                                    {
                                        foreach (string scodeRolesReading in lpRolesReading.ToList())
                                        {
                                            if (!iEcodesRolesReading.Contains(scodeRolesReading))
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        };
                                    }
                                }
                                else
                                {
                                    IEnumerable<string> iEcodesRolesReading = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                                    IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                    IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                        iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                    if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                    {
                                        foreach (string scodeRolesReading in lpRolesReading.ToList())
                                        {
                                            if (workF.LinkedRoles.Contains(scodeRolesReading))
                                            {
                                                lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
                                            }
                                        };
                                    }

                                    else if (iEcodesRolesReading == null)
                                    {
                                        lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workflow.Codigo + "' has no assigned role.", null));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        linkedRoleswFlow = workF.LinkedRoles;
                        workflow = await _getWorkflowDependency.GetItemByCode(workF.Code, Client);

                        if (workflow != null)
                        {
                            linkedRolesWorkflow = workflow.WorkflowsRoles;
                        }

                        if (status != null && status.LinkedRolesWriting != null && status.LinkedRolesWriting.Count > 0 && status.LinkedRolesWriting[0] != null)
                        {
                            List<Filter> filtersRoles = new List<Filter>();

                            foreach (string codeLinkedRolesWriting in status.LinkedRolesWriting)
                            {
                                filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesWriting, Filter.Types.OR, null));
                            }

                            linkedRolesWriting = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                            if (!workF.Public)
                            {
                                IEnumerable<string> iEcodesRolesWriting = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                    iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        if (!workF.LinkedRoles.Contains(scodeRolesWriting) 
                                            && workF.LinkedRoles != null && workF.LinkedRoles[0] != null)
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + workF.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesWriting == null && linkedRolesWorkflow == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workF.Code + "' has no assigned role.", null));
                                }

                                else if (iEcodesRolesWriting.Count() > 0 && lpRolesWriting.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        if (!iEcodesRolesWriting.Contains(scodeRolesWriting))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + workF.Code + "'.", null));
                                        }
                                    };
                                }
                            }
                            else
                            {
                                IEnumerable<string> iEcodesRolesWriting = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                    iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        if (workF.LinkedRoles.Contains(scodeRolesWriting)
                                            && workF.LinkedRoles != null && workF.LinkedRoles[0] != null)
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is in the list of possible roles in the workflow '"
                                            + workF.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesWriting == null && linkedRolesWorkflow == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workF.Code + "' has no assigned role.", null));
                                }
                            }
                        }

                        if (status != null && status.LinkedRolesReading != null && status.LinkedRolesReading.Count > 0 && status.LinkedRolesReading[0] != null)
                        {
                            List<Filter> filtersRoles = new List<Filter>();

                            foreach (string codeLinkedRolesReading in status.LinkedRolesReading)
                            {
                                filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesReading, Filter.Types.OR, null));
                            }

                            linkedRolesReading = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                            if (!workF.Public)
                            {
                                IEnumerable<string> iEcodesRolesReading = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                    iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (!workF.LinkedRoles.Contains(scodeRolesReading)
                                            && workF.LinkedRoles != null && workF.LinkedRoles[0] != null)
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                            + workF.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesReading == null && linkedRolesWorkflow == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workF.Code + "' has no assigned role.", null));
                                }

                                else if (iEcodesRolesReading.Count() > 0 && lpRolesReading.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (!iEcodesRolesReading.Contains(scodeRolesReading))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                            + workF.Code + "'.", null));
                                        }
                                    };
                                }
                            }
                            else
                            {
                                IEnumerable<string> iEcodesRolesReading = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                    iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                if (workF.LinkedRoles != null && workF.LinkedRoles.Count > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (workF.LinkedRoles.Contains(scodeRolesReading)
                                            && workF.LinkedRoles != null && workF.LinkedRoles[0] != null)
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is in the list of possible roles in the workflow '"
                                            + workF.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesReading == null && linkedRolesWorkflow == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + workF.Code + "' has no assigned role.", null));
                                }
                            }
                        }
                    }

                    IEnumerable<WorkFlowNextStatusEntity> listNextStatus = new List<WorkFlowNextStatusEntity>();

                    if (status != null && status.LinkedWorkFlowNextStatus != null && status.LinkedWorkFlowNextStatus.Count > 0 && status.LinkedWorkFlowNextStatus[0] != null)
                    {
                        Result<List<WorkFlowNextStatusEntity>> listNextStatusValidity = await _putNextStatus.ValidateEntity(status.LinkedWorkFlowNextStatus, status, oEntidad, Client);
                        if (listNextStatusValidity.Success)
                        {
                            listNextStatus = listNextStatusValidity.Value;
                        }
                        else
                        {
                            lErrors.AddRange(listNextStatusValidity.Errors);
                        }
                    }

                    WorkFlowStatusEntity statusEntity = new WorkFlowStatusEntity(null, status.Code, status.Name, status.Description, status.TimeFrame, status.Complete, status.PublicReading,
                        status.PublicWriting, statusGroup, status.Active, status.Default, workflow, listNextStatus, linkedRolesWriting, linkedRolesReading);

                    foreach (WorkFlowNextStatusEntity item in statusEntity.EstadosSiguientes)
                    {
                        item.WorkFlowStatus = statusEntity;
                    }

                    //AÑADIR CONTROL POR DEFECTO
                    //if (statusEntity.Defecto)
                    //{


                    //    filter = new Filter(nameof(WorkFlowStatusDTO.Default), Operators.eq, true, Filter.Types.AND, null);
                    //    filter2 = new Filter(nameof(WorkflowEntity.CoreWorkFlowID), Operators.eq, workflow.CoreWorkFlowID, Filter.Types.AND, null);
                    //    listFiltersDefault.Add(filter);
                    //    listFiltersDefault.Add(filter2);
                    //    Task<IEnumerable<WorkFlowStatusEntity>> listWorkFlowStatus = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    //    if (listWorkFlowStatus.Result != null && listWorkFlowStatus.Result.ListOrEmpty().Count > 0)
                    //    {
                    //        WorkFlowStatusEntity pType = new WorkFlowStatusEntity(listWorkFlowStatus.Result.ListOrEmpty()[0].CoreEstadoID.Value, listWorkFlowStatus.Result.ListOrEmpty()[0].Codigo,
                    //                listWorkFlowStatus.Result.ListOrEmpty()[0].Nombre, listWorkFlowStatus.Result.ListOrEmpty()[0].Descripcion, listWorkFlowStatus.Result.ListOrEmpty()[0].Tiempo,
                    //                listWorkFlowStatus.Result.ListOrEmpty()[0].Completado, listWorkFlowStatus.Result.ListOrEmpty()[0].PublicoLectura,
                    //                listWorkFlowStatus.Result.ListOrEmpty()[0].PublicoEscritura, listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosAgrupaciones,
                    //                listWorkFlowStatus.Result.ListOrEmpty()[0].Activo, false, listWorkFlowStatus.Result.ListOrEmpty()[0].WorkFlow, listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosSiguientes,
                    //                listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosRolesEscritura, listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosRolesLectura);
                    //        await _putDependencies.Update(pType);
                    //    }
                    //}

                    linkedStatusEntity.Add(statusEntity);

                }
                else
                {
                    lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<List<WorkFlowStatusEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return linkedStatusEntity;
            }

        }
        private int numDefectos(List<WorkFlowStatusDTO> lista)
        {
            int cont = 0;
            foreach (WorkFlowStatusDTO nextStatus in lista)
            {
                if (nextStatus.Default)
                {
                    cont++;
                }
            }
            return cont;
        }

        private bool controlRepetido(List<WorkFlowStatusDTO> lista, WorkFlowStatusDTO elemento)
        {
            int cont = 0;

            foreach (WorkFlowStatusDTO status in lista)
            {
                if (status != null && elemento.Code == status.Code)
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
    }
}
