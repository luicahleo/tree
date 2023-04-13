using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class PutCatalogLifecycleStatus : PutObjectService<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity, CatalogLifecycleStatusDTOMapper>
    {
        private readonly GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> _getDependency;

        public PutCatalogLifecycleStatus(PutDependencies<CatalogLifecycleStatusEntity> putDependency, GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, putDependency, new CatalogLifecycleStatusValidation())
        {
            _getDependency = getDependency;
        }

        public override async Task<Result<CatalogLifecycleStatusEntity>> ValidateEntity(CatalogLifecycleStatusDTO catalogLifecycleStatus, int client, string code, string email)
        {
            Task<IEnumerable<CatalogLifecycleStatusEntity>> listCatalogLifecycleStatuss;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            CatalogLifecycleStatusEntity? catLifecycleStatus = await _getDependency.GetItemByCode(code, client);
            CatalogLifecycleStatusEntity catalogLifecycleStatusFinal = new CatalogLifecycleStatusEntity(catLifecycleStatus.CoreProductCatalogEstadoGlobalID,
                catalogLifecycleStatus.Code, catalogLifecycleStatus.Name, catalogLifecycleStatus.Description, catalogLifecycleStatus.Active, catalogLifecycleStatus.Default, client);

            filter = new Filter(nameof(CatalogLifecycleStatusDTO.Code), Operators.eq, catalogLifecycleStatus.Code);
            listFilters.Add(filter);

            listCatalogLifecycleStatuss = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listCatalogLifecycleStatuss.Result != null && listCatalogLifecycleStatuss.Result.ListOrEmpty().Count > 0 &&
                listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].CoreProductCatalogEstadoGlobalID != catalogLifecycleStatusFinal.CoreProductCatalogEstadoGlobalID)
            {
                return Result.Failure<CatalogLifecycleStatusEntity>(_traduccion.CodeCatalogLifecycleStatus + " " + $"{catalogLifecycleStatus.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            if (catalogLifecycleStatus.Default)
            {
                filter = new Filter(nameof(CatalogLifecycleStatusDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listCatalogLifecycleStatuss = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listCatalogLifecycleStatuss.Result != null && listCatalogLifecycleStatuss.Result.ListOrEmpty().Count > 0)
                {
                    CatalogLifecycleStatusEntity pType = new CatalogLifecycleStatusEntity(listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].CoreProductCatalogEstadoGlobalID, listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].Codigo,
                        listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].Nombre, listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].Descripcion, listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].Activo, false,
                        listCatalogLifecycleStatuss.Result.ListOrEmpty()[0].ClienteID);
                    await _putDependencies.Update(pType);
                }
            }

            return catalogLifecycleStatusFinal;
        }
    }
}
