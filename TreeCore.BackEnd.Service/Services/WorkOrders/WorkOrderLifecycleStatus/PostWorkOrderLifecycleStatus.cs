using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders
{
    public class PostWorkOrderLifecycleStatus : PostObjectService<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper>
    {

        private readonly GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> _getDependency;
        private readonly PutDependencies<WorkOrderLifecycleStatusEntity> _putDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;

        public PostWorkOrderLifecycleStatus(PostDependencies<WorkOrderLifecycleStatusEntity> postDependency,
            GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> getDependency,
            PutDependencies<WorkOrderLifecycleStatusEntity> putDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkOrderLifecycleStatusValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<WorkOrderLifecycleStatusEntity>> ValidateEntity(WorkOrderLifecycleStatusDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkOrderLifecycleStatusEntity trackingStatus = new WorkOrderLifecycleStatusEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description,oEntidad.Colour, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(WorkOrderLifecycleStatusDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            Task<IEnumerable<WorkOrderLifecycleStatusEntity>> lisWorkOrderTrackingStatus = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (lisWorkOrderTrackingStatus.Result != null && lisWorkOrderTrackingStatus.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeWorkOrderTrackingStatus + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (oEntidad.Default)
            {
                filter = new Filter(nameof(WorkOrderLifecycleStatusDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                lisWorkOrderTrackingStatus = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (lisWorkOrderTrackingStatus.Result != null && lisWorkOrderTrackingStatus.Result.ListOrEmpty().Count > 0)
                {
                    WorkOrderLifecycleStatusEntity pType = WorkOrderLifecycleStatusEntity.Create(lisWorkOrderTrackingStatus.Result.ListOrEmpty()[0].CoreWorkOrderEstadoID.Value, Client, lisWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Codigo,
                        lisWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Nombre, lisWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Descripcion,lisWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Color, lisWorkOrderTrackingStatus.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkOrderLifecycleStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return trackingStatus;
            }
        }
    }
}