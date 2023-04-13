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
    public class PostWorkFlowStatus : PostObjectService<WorkFlowStatusDTO, WorkFlowStatusEntity, WorkFlowStatusDTOMapper>
    {

        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getDependency;
        private readonly PutDependencies<WorkFlowStatusEntity> _putDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> _getStatusGroupDependency;
        private readonly GetDependencies<WorkflowDTO, WorkflowEntity> _getWorkflowDependency;
        private readonly PostWorkFlowNextStatus _postNextStatus;
        private readonly GetDependencies<RolDTO, RolEntity> _getRolesDependency;
        private readonly GetWorkFlowStatus _getStatus;

        public PostWorkFlowStatus(PostDependencies<WorkFlowStatusEntity> postDependency,
            GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getDependency,
            PutDependencies<WorkFlowStatusEntity> putDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            GetDependencies<RolDTO, RolEntity> getRolesDependency,
            GetDependencies<WorkflowDTO, WorkflowEntity> getWorkflowDependency,
            GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> getStatusGroupDependency,
            PostWorkFlowNextStatus postNextStatus,
            GetWorkFlowStatus getStatus,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkFlowStatusValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getRolesDependency = getRolesDependency;
            _getUserDependency = getUserDependency;
            _getStatusGroupDependency = getStatusGroupDependency;
            _getWorkflowDependency = getWorkflowDependency;
            _postNextStatus = postNextStatus;
            _getStatus = getStatus;
        }

        public override async Task<Result<WorkFlowStatusEntity>> ValidateEntity(WorkFlowStatusDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            IEnumerable<RolEntity> linkedRolesWriting = null;
            IEnumerable<RolEntity> linkedRolesReading = null;
            IEnumerable<RolEntity> linkedRolesWorkflow = null;
            Filter filter;
            WorkflowEntity workflow = null;
            int? workflowID = null;

            WorkFlowStatusGroupEntity statusGroup = await _getStatusGroupDependency.GetItemByCode(oEntidad.StatusGroupCode, Client);
            if (statusGroup == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkFlowStatusGroup + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.NotFound + "."));
            }

            if (codesDict != null && codesDict["LEVEL1"] != "")
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

                    if (oEntidad != null && oEntidad.LinkedRolesWriting != null && oEntidad.LinkedRolesWriting.Count > 0 && oEntidad.LinkedRolesWriting[0] != null)
                    {
                        List<Filter> filtersRoles = new List<Filter>();

                        foreach (string codeLinkedRolesWriting in oEntidad.LinkedRolesWriting)
                        {
                            filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesWriting, Filter.Types.OR, null));
                        }

                        linkedRolesWriting = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                        if (!workflow.Publico)
                        {
                            IEnumerable<string> iEcodesRolesWriting = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                            IEnumerable<string> lpRolesWriting = oEntidad.LinkedRolesWriting;
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

                            else if (intersectRolesWriting.Count() > 0)
                            {
                                foreach (string scodeRolesWriting in intersectRolesWriting.ToList())
                                {
                                    if (lpRolesWriting.Contains(scodeRolesWriting))
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
                            IEnumerable<string> lpRolesWriting = oEntidad.LinkedRolesWriting;
                            IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                iEcodesRolesWriting.Intersect(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

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

                    if (oEntidad != null && oEntidad.LinkedRolesReading != null && oEntidad.LinkedRolesReading.Count > 0 && oEntidad.LinkedRolesReading[0] != null)
                    {
                        List<Filter> filtersRoles = new List<Filter>();

                        foreach (string codeLinkedRolesReading in oEntidad.LinkedRolesReading)
                        {
                            filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesReading, Filter.Types.OR, null));
                        }

                        linkedRolesReading = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                        if (!workflow.Publico)
                        {
                            IEnumerable<string> iEcodesRolesReading = (linkedRolesWorkflow != null) ? linkedRolesWorkflow.Select(lp => lp.Codigo) : null;
                            IEnumerable<string> lpRolesReading = oEntidad.LinkedRolesReading;
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

                            else if (intersectRolesReading.Count() > 0)
                            {
                                foreach (string scodeRolesReading in intersectRolesReading.ToList())
                                {
                                    if (lpRolesReading.Contains(scodeRolesReading))
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
                            IEnumerable<string> lpRolesReading = oEntidad.LinkedRolesReading;
                            IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                            if (workflow.WorkflowsRoles != null && workflow.WorkflowsRoles.Count() > 0)
                            {
                                foreach (string scodeRolesReading in lpRolesReading.ToList())
                                {
                                    foreach (RolEntity rolesReading in workflow.WorkflowsRoles)
                                    {
                                        if (rolesReading.Codigo == scodeRolesReading)
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
                        }
                    }
                }
            }

            List<WorkFlowStatusDTO> listStatus = await _getStatus.GetListByWorkFlowV2(code, Client);

            filter = new Filter(nameof(WorkflowDTO.Code), Operators.eq, code);
            listFilters.Add(filter);


            IEnumerable<WorkFlowNextStatusEntity> listNextStatus = new List<WorkFlowNextStatusEntity>();
            if (oEntidad.LinkedWorkFlowNextStatus != null && oEntidad.LinkedWorkFlowNextStatus.Count > 0)
            {
                Result<List<WorkFlowNextStatusEntity>> listNextStatusValidity = await _postNextStatus.ValidateEntity(oEntidad.LinkedWorkFlowNextStatus, oEntidad, listStatus, Client);
                
                
                if (listNextStatusValidity.Success)
                {
                    listNextStatus = listNextStatusValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listNextStatusValidity.Errors);
                }
            }

            WorkFlowStatusEntity status = new WorkFlowStatusEntity(null, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.TimeFrame,
            oEntidad.Complete, oEntidad.PublicReading, oEntidad.PublicWriting, statusGroup, oEntidad.Active, oEntidad.Default, workflow, listNextStatus,
            linkedRolesWriting, linkedRolesReading);

            filter = new Filter(nameof(WorkFlowStatusDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            Task<IEnumerable<WorkFlowStatusEntity>> listWorkFlowStatus = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listWorkFlowStatus.Result != null && listWorkFlowStatus.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeWorkFlowStatus + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (oEntidad.Default)
            {
                filter = new Filter(nameof(WorkFlowStatusDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listWorkFlowStatus = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listWorkFlowStatus.Result != null && listWorkFlowStatus.Result.ListOrEmpty().Count > 0)
                {
                    WorkFlowStatusEntity pType = new WorkFlowStatusEntity(listWorkFlowStatus.Result.ListOrEmpty()[0].CoreEstadoID.Value, listWorkFlowStatus.Result.ListOrEmpty()[0].Codigo,
                            listWorkFlowStatus.Result.ListOrEmpty()[0].Nombre, listWorkFlowStatus.Result.ListOrEmpty()[0].Descripcion, listWorkFlowStatus.Result.ListOrEmpty()[0].Tiempo,
                            listWorkFlowStatus.Result.ListOrEmpty()[0].Completado, listWorkFlowStatus.Result.ListOrEmpty()[0].PublicoLectura,
                            listWorkFlowStatus.Result.ListOrEmpty()[0].PublicoEscritura, listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosAgrupaciones,
                            listWorkFlowStatus.Result.ListOrEmpty()[0].Activo, false, listWorkFlowStatus.Result.ListOrEmpty()[0].WorkFlow, listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosSiguientes,
                            listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosRolesEscritura, listWorkFlowStatus.Result.ListOrEmpty()[0].EstadosRolesLectura);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkFlowStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return status;
            }
        }

        public async Task<Result<List<WorkFlowStatusEntity>>> ValidateEntity(List<WorkFlowStatusDTO> oEntidad, int Client, WorkflowDTO wFlow, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            Filter filter2;
            WorkflowEntity workflow = null;
            int? workflowID = null;
            IEnumerable<RolEntity> linkedRolesReading = null;
            IEnumerable<RolEntity> linkedRolesWriting = null;
            IEnumerable<RolEntity> linkedRolesWorkflow = null;
            List<string> linkedRoleswFlow = null;

            List<WorkFlowStatusEntity> linkedStatusEntity = new List<WorkFlowStatusEntity>();

            if (oEntidad != null && oEntidad.Count() > 0 && oEntidad[0] != null)
            {
                if (numDefectos(oEntidad) >1)
                {
                    lErrors.Add(Error.Create(_traduccion.DefectoYaExiste + "."));
                }
                foreach (WorkFlowStatusDTO status in oEntidad)
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

                    if (codesDict != null && codesDict.Count > 0 && codesDict["LEVEL1"] != "")
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

                            if (status != null && status.LinkedRolesWriting != null && status.LinkedRolesWriting.Count > 0)
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

                                    else if (intersectRolesWriting.Count() > 0)
                                    {
                                        foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
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

                            if (oEntidad != null && status.LinkedRolesReading != null && status.LinkedRolesReading.Count > 0)
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

                                    else if (intersectRolesReading.Count() > 0)
                                    {
                                        foreach (string scodeRolesReading in lpRolesReading.ToList())
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                                + workflow.Codigo + "'.", null));
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
                                                if (rolesReading.Codigo == scodeRolesReading)
                                                {
                                                    lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is  in the list of possible roles in the workflow '"
                                                    + workflow.Codigo + "'.", null));
                                                }
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
                        linkedRoleswFlow = wFlow.LinkedRoles;

                        if (status != null && status.LinkedRolesWriting != null && status.LinkedRolesWriting.Count > 0)
                        {
                            List<Filter> filtersRoles = new List<Filter>();

                            foreach (string codeLinkedRolesWriting in status.LinkedRolesWriting)
                            {
                                filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesWriting, Filter.Types.OR, null));
                            }

                            linkedRolesWriting = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                            if (!wFlow.Public)
                            {
                                IEnumerable<string> iEcodesRolesWriting = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                    iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                if (wFlow.LinkedRoles != null && wFlow.LinkedRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        if (!wFlow.LinkedRoles.Contains(scodeRolesWriting))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesWriting == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + wFlow.Code + "' has no assigned role.", null));
                                }

                                else if (intersectRolesWriting.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                    };
                                }
                            }
                            else
                            {
                                IEnumerable<string> iEcodesRolesWriting = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesWriting = status.LinkedRolesWriting;
                                IEnumerable<string> intersectRolesWriting = (iEcodesRolesWriting != null) ?
                                    iEcodesRolesWriting.Union(lpRolesWriting).Except(iEcodesRolesWriting.Intersect(lpRolesWriting)) : null;

                                if (wFlow.LinkedRoles != null && wFlow.LinkedRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesWriting.ToList())
                                    {
                                        if (wFlow.LinkedRoles.Contains(scodeRolesWriting))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesWriting == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + wFlow.Code + "' has no assigned role.", null));
                                }
                            }
                        }

                        if (oEntidad != null && status.LinkedRolesReading != null && status.LinkedRolesReading.Count > 0)
                        {
                            List<Filter> filtersRoles = new List<Filter>();

                            foreach (string codeLinkedRolesReading in status.LinkedRolesReading)
                            {
                                filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRolesReading, Filter.Types.OR, null));
                            }

                            linkedRolesReading = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);

                            if (!wFlow.Public)
                            {
                                IEnumerable<string> iEcodesRolesReading = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                    iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                if (wFlow.LinkedRoles != null && wFlow.LinkedRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (!wFlow.LinkedRoles.Contains(scodeRolesReading))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is not in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesReading == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + wFlow.Code + "' has no assigned role.", null));
                                }

                                else if (intersectRolesReading.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesReading.ToList())
                                    {
                                        lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                    };
                                }
                            }
                            else
                            {
                                IEnumerable<string> iEcodesRolesReading = (linkedRoleswFlow != null) ? linkedRoleswFlow : null;
                                IEnumerable<string> lpRolesReading = status.LinkedRolesReading;
                                IEnumerable<string> intersectRolesReading = (iEcodesRolesReading != null) ?
                                    iEcodesRolesReading.Union(lpRolesReading).Except(iEcodesRolesReading.Intersect(lpRolesReading)) : null;

                                if (wFlow.LinkedRoles != null && wFlow.LinkedRoles.Count() > 0)
                                {
                                    foreach (string scodeRolesReading in lpRolesReading.ToList())
                                    {
                                        if (wFlow.LinkedRoles.Contains(scodeRolesReading))
                                        {
                                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesReading + $"' is  in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                        }
                                    };
                                }

                                else if (iEcodesRolesReading == null)
                                {
                                    lErrors.Add(Error.Create($"{_traduccion.Workflow} {_traduccion.Code} '" + wFlow.Code + "' has no assigned role.", null));
                                }

                                else if (intersectRolesReading.Count() > 0)
                                {
                                    foreach (string scodeRolesWriting in lpRolesReading.ToList())
                                    {
                                        lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRolesWriting + $"' is not in the list of possible roles in the workflow '"
                                            + wFlow.Code + "'.", null));
                                    };
                                }
                            }
                        }
                    }

                    IEnumerable<WorkFlowNextStatusEntity> listNextStatus = new List<WorkFlowNextStatusEntity>();

                    if (status.LinkedWorkFlowNextStatus != null && status.LinkedWorkFlowNextStatus.Count > 0)
                    {
                        Result<List<WorkFlowNextStatusEntity>> listNextStatusValidity = await _postNextStatus.ValidateEntity(status.LinkedWorkFlowNextStatus, status, oEntidad, Client);
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

                    linkedStatusEntity.Add(statusEntity);
                };
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

            if (lista != null && lista.Count() > 0 && lista[0] != null)
            {
                foreach (WorkFlowStatusDTO status in lista)
                {
                    if (elemento.Code == status.Code)
                    {
                        cont++;
                    }
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