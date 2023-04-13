using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders
{

    public class PutWorkOrderLifecycleStatus : PutObjectService<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper>
    {
        private readonly GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> _getDependency;

        public PutWorkOrderLifecycleStatus(PutDependencies<WorkOrderLifecycleStatusEntity> putDependency,
            GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new WorkOrderLifecycleStatusValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<WorkOrderLifecycleStatusEntity>> ValidateEntity(WorkOrderLifecycleStatusDTO trackingStatus, int Client,string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<WorkOrderLifecycleStatusEntity>> listWorkOrderTrackingStatus;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkOrderLifecycleStatusEntity trackingStatusFinal = null;

            WorkOrderLifecycleStatusEntity? workFlowStatusGroup = await _getDependency.GetItemByCode(code, Client);
            if (workFlowStatusGroup == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeWorkOrderTrackingStatus + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (trackingStatus.Default && !trackingStatus.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                trackingStatusFinal = new WorkOrderLifecycleStatusEntity(workFlowStatusGroup.CoreWorkOrderEstadoID, Client, trackingStatus.Code, trackingStatus.Name, trackingStatus.Description,trackingStatus.Colour, trackingStatus.Active, trackingStatus.Default);

                filter = new Filter(nameof(WorkOrderLifecycleStatusDTO.Code), Operators.eq, trackingStatus.Code);
                listFilters.Add(filter);

                listWorkOrderTrackingStatus = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listWorkOrderTrackingStatus.Result != null && listWorkOrderTrackingStatus.Result.ListOrEmpty().Count > 0 &&
                    listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].CoreWorkOrderEstadoID != trackingStatusFinal.CoreWorkOrderEstadoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeWorkOrderTrackingStatus + " " + $"{trackingStatus.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (trackingStatus.Default)
                {
                    filter = new Filter(nameof(WorkOrderLifecycleStatusDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listWorkOrderTrackingStatus = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listWorkOrderTrackingStatus.Result != null && listWorkOrderTrackingStatus.Result.ListOrEmpty().Count > 0)
                    {
                        WorkOrderLifecycleStatusEntity pType = new WorkOrderLifecycleStatusEntity(listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].CoreWorkOrderEstadoID.Value, Client, listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Codigo,
                            listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Nombre, listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Descripcion, listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Color, listWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkOrderLifecycleStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return trackingStatusFinal;
            }
        }
    }
}
