using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class PostWorkFlowStatusGroup : PostObjectService<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper>
    {

        private readonly GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> _getDependency;
        private readonly PutDependencies<WorkFlowStatusGroupEntity> _putDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;

        public PostWorkFlowStatusGroup(PostDependencies<WorkFlowStatusGroupEntity> postDependency,
            GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> getDependency,
            PutDependencies<WorkFlowStatusGroupEntity> putDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkFlowStatusGroupValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<WorkFlowStatusGroupEntity>> ValidateEntity(WorkFlowStatusGroupDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkFlowStatusGroupEntity statusGroup = new WorkFlowStatusGroupEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(WorkFlowStatusGroupDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            Task<IEnumerable<WorkFlowStatusGroupEntity>> listWorkFlowStatusGroup = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listWorkFlowStatusGroup.Result != null && listWorkFlowStatusGroup.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkFlowStatusGroup + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (oEntidad.Default)
            {
                filter = new Filter(nameof(WorkFlowStatusGroupDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listWorkFlowStatusGroup = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listWorkFlowStatusGroup.Result != null && listWorkFlowStatusGroup.Result.ListOrEmpty().Count > 0)
                {
                    WorkFlowStatusGroupEntity pType = WorkFlowStatusGroupEntity.Create(listWorkFlowStatusGroup.Result.ListOrEmpty()[0].EstadoAgrupacionID.Value, Client, listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Codigo,
                        listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Nombre, listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Descripcion, listWorkFlowStatusGroup.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkFlowStatusGroupEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return statusGroup;
            }
        }
    }
}