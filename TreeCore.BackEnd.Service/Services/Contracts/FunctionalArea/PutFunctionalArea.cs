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

    public class PutFunctionalArea : PutObjectService<FunctionalAreaDTO, FunctionalAreaEntity,FuncionalAreaDTOMapper>
    {
        private readonly GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity> _getDependency;


        public PutFunctionalArea(PutDependencies<FunctionalAreaEntity> putDependency, GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new FunctionalAreaValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<FunctionalAreaEntity>> ValidateEntity(FunctionalAreaDTO functionalArea, int Client, string code, string email)
        {
            Task<IEnumerable<FunctionalAreaEntity>> listFunctionalAreas;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            FunctionalAreaEntity? prodType = await _getDependency.GetItemByCode(code, Client);
            FunctionalAreaEntity prodTypeFinal = new FunctionalAreaEntity(prodType.AreaFuncionalID, Client, functionalArea.Code, functionalArea.Name, functionalArea.Description, functionalArea.Active, functionalArea.Default);

            filter = new Filter(nameof(FunctionalAreaDTO.Code), Operators.eq, functionalArea.Code);
            listFilters.Add(filter);

            listFunctionalAreas = _getDependency.GetList(Client, listFilters, null, null, - 1, -1);
            if (listFunctionalAreas.Result != null && listFunctionalAreas.Result.ListOrEmpty().Count > 0 &&
                listFunctionalAreas.Result.ListOrEmpty()[0].AreaFuncionalID != prodTypeFinal.AreaFuncionalID)
            {
                return Result.Failure<FunctionalAreaEntity>(_traduccion.CodeFunctionalArea + " " + $"{functionalArea.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            if (functionalArea.Default)
            {
                filter = new Filter(nameof(FunctionalAreaDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listFunctionalAreas = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listFunctionalAreas.Result != null && listFunctionalAreas.Result.ListOrEmpty().Count > 0)
                {
                    FunctionalAreaEntity pType = new FunctionalAreaEntity(listFunctionalAreas.Result.ListOrEmpty()[0].AreaFuncionalID.Value, Client, listFunctionalAreas.Result.ListOrEmpty()[0].Codigo,
                        listFunctionalAreas.Result.ListOrEmpty()[0].AreaFuncional, listFunctionalAreas.Result.ListOrEmpty()[0].Descripcion, listFunctionalAreas.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependencies.Update(pType);
                }
            }

            return prodTypeFinal;
        }
      
    }
}
