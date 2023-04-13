using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
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

    public class PutCatalogType : PutObjectService<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper>
    {
        private readonly GetDependencies<CatalogTypeDTO, CatalogTypeEntity> _getDependency;

        public PutCatalogType(PutDependencies<CatalogTypeEntity> putDependency, GetDependencies<CatalogTypeDTO, CatalogTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, putDependency, new CatalogTypeValidation())
        {
            _getDependency = getDependency;
        }

        public override async Task<Result<CatalogTypeEntity>> ValidateEntity(CatalogTypeDTO catalogType, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<CatalogTypeEntity>> listCatalogTypes;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            if (catalogType.IsPurchasing == false && catalogType.IsOffering == false)
            {
                lErrors.Add(Error.Create(_errorTraduccion.IsRequiredBool));
            }

            if (catalogType.IsPurchasing == true && catalogType.IsOffering == true)
            {
                lErrors.Add(Error.Create(_errorTraduccion.NotPossibleBool));
            }

            CatalogTypeEntity? prodType = await _getDependency.GetItemByCode(code, client);
            CatalogTypeEntity prodTypeFinal = new CatalogTypeEntity(prodType.CoreProductCatalogTipoID, client, catalogType.Code, catalogType.Name, catalogType.Description,
                catalogType.Active, catalogType.Default, catalogType.IsPurchasing, catalogType.IsOffering);

            filter = new Filter(nameof(CatalogTypeDTO.Code), Operators.eq, catalogType.Code);
            listFilters.Add(filter);

            listCatalogTypes = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listCatalogTypes.Result != null && listCatalogTypes.Result.ListOrEmpty().Count > 0 &&
                listCatalogTypes.Result.ListOrEmpty()[0].CoreProductCatalogTipoID != prodTypeFinal.CoreProductCatalogTipoID)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCatalogType + " " + $"{catalogType.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (catalogType.Default)
            {
                filter = new Filter(nameof(CatalogTypeDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listCatalogTypes = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listCatalogTypes.Result != null && listCatalogTypes.Result.ListOrEmpty().Count > 0)
                {
                    CatalogTypeEntity pType = new CatalogTypeEntity(listCatalogTypes.Result.ListOrEmpty()[0].CoreProductCatalogTipoID.Value, client, listCatalogTypes.Result.ListOrEmpty()[0].Codigo,
                        listCatalogTypes.Result.ListOrEmpty()[0].Nombre, listCatalogTypes.Result.ListOrEmpty()[0].Descripcion, listCatalogTypes.Result.ListOrEmpty()[0].Activo, listCatalogTypes.Result.ListOrEmpty()[0].Defecto,
                        listCatalogTypes.Result.ListOrEmpty()[0].EsCompra, listCatalogTypes.Result.ListOrEmpty()[0].EsVenta);
                    await _putDependencies.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<CatalogTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return prodTypeFinal;
            }
        }
    }
}
