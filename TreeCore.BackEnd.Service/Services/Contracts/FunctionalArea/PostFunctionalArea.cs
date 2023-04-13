using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{
    public class PostFunctionalArea : PostObjectService<FunctionalAreaDTO, FunctionalAreaEntity, FuncionalAreaDTOMapper>
    {

        private readonly GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity> _getDependency;
        private readonly PutDependencies<FunctionalAreaEntity> _putDependency;

        public PostFunctionalArea(PostDependencies<FunctionalAreaEntity> postDependency, GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity> getDependency, PutDependencies<FunctionalAreaEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new FunctionalAreaValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<FunctionalAreaEntity>> ValidateEntity(FunctionalAreaDTO oEntidad, int Client, string email, string code = "")
        {
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            FunctionalAreaEntity functionalAreaEntity = new FunctionalAreaEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(FunctionalAreaDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<FunctionalAreaEntity>> listFunctionalAreas = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listFunctionalAreas.Result != null && listFunctionalAreas.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<FunctionalAreaEntity>(_traduccion.CodeFunctionalArea + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            else
            {
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(FunctionalAreaDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listFunctionalAreas = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listFunctionalAreas.Result != null && listFunctionalAreas.Result.ListOrEmpty().Count > 0)
                    {
                        FunctionalAreaEntity pType = FunctionalAreaEntity.Create(listFunctionalAreas.Result.ListOrEmpty()[0].AreaFuncionalID.Value, Client, listFunctionalAreas.Result.ListOrEmpty()[0].Codigo,
                            listFunctionalAreas.Result.ListOrEmpty()[0].AreaFuncional, listFunctionalAreas.Result.ListOrEmpty()[0].Descripcion, listFunctionalAreas.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependency.Update(pType);
                    }
                }
            }

            return functionalAreaEntity;
        }

     }
}
