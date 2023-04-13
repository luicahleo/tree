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

    public class PutWorkFlowStatusGroup : PutObjectService<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper>
    {
        private readonly GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> _getDependency;

        public PutWorkFlowStatusGroup(PutDependencies<WorkFlowStatusGroupEntity> putDependency,
            GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new WorkFlowStatusGroupValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<WorkFlowStatusGroupEntity>> ValidateEntity(WorkFlowStatusGroupDTO statusGroup, int Client,string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<WorkFlowStatusGroupEntity>> listWorkFlowStatusGroup;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkFlowStatusGroupEntity statusGroupFinal = null;

            WorkFlowStatusGroupEntity? workFlowStatusGroup = await _getDependency.GetItemByCode(code, Client);
            if (workFlowStatusGroup == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkFlowStatusGroup + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (statusGroup.Default && !statusGroup.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                statusGroupFinal = new WorkFlowStatusGroupEntity(workFlowStatusGroup.EstadoAgrupacionID, Client, statusGroup.Code, statusGroup.Name, statusGroup.Description, statusGroup.Active, statusGroup.Default);

                filter = new Filter(nameof(WorkFlowStatusGroupDTO.Code), Operators.eq, statusGroup.Code);
                listFilters.Add(filter);

                listWorkFlowStatusGroup = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listWorkFlowStatusGroup.Result != null && listWorkFlowStatusGroup.Result.ListOrEmpty().Count > 0 &&
                    listWorkFlowStatusGroup.Result.ListOrEmpty()[0].EstadoAgrupacionID != statusGroupFinal.EstadoAgrupacionID)
                {
                    lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkFlowStatusGroup + " " + $"{code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (statusGroup.Default)
                {
                    filter = new Filter(nameof(WorkFlowStatusGroupDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listWorkFlowStatusGroup = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listWorkFlowStatusGroup.Result != null && listWorkFlowStatusGroup.Result.ListOrEmpty().Count > 0)
                    {
                        WorkFlowStatusGroupEntity pType = new WorkFlowStatusGroupEntity(listWorkFlowStatusGroup.Result.ListOrEmpty()[0].EstadoAgrupacionID.Value, Client, listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Codigo,
                            listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Nombre, listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Descripcion, listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkFlowStatusGroupEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return statusGroupFinal;
            }
        }
    }
}
