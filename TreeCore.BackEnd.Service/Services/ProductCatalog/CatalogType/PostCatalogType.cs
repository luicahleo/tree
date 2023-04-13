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
    public class PostCatalogType : PostObjectService<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper>
    {

        private readonly GetDependencies<CatalogTypeDTO, CatalogTypeEntity> _getDependency;
        private readonly PutDependencies<CatalogTypeEntity> _putDependency;

        public PostCatalogType(PostDependencies<CatalogTypeEntity> postDependency, GetDependencies<CatalogTypeDTO, CatalogTypeEntity> getDependency, PutDependencies<CatalogTypeEntity> putDependency, 
            IHttpContextAccessor httpcontextAccessor): base(httpcontextAccessor, postDependency, new CatalogTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<CatalogTypeEntity>> ValidateEntity(CatalogTypeDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            if (oEntidad.IsPurchasing == false && oEntidad.IsOffering == false)
            {
                lErrors.Add(Error.Create(_errorTraduccion.IsRequiredBool));
            }

            if (oEntidad.IsPurchasing == true && oEntidad.IsOffering == true)
            {
                lErrors.Add(Error.Create(_errorTraduccion.NotPossibleBool));
            }

            CatalogTypeEntity catalogTypeEntity = new CatalogTypeEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default,
                oEntidad.IsPurchasing, oEntidad.IsOffering);

            filter = new Filter(nameof(CatalogTypeDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<CatalogTypeEntity>> listCatalogTypes = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listCatalogTypes.Result != null && listCatalogTypes.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCatalogType + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            else
            {
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(CatalogTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listCatalogTypes = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listCatalogTypes.Result != null && listCatalogTypes.Result.ListOrEmpty().Count > 0)
                    {
                        CatalogTypeEntity pType = CatalogTypeEntity.Create(listCatalogTypes.Result.ListOrEmpty()[0].CoreProductCatalogTipoID.Value, client, listCatalogTypes.Result.ListOrEmpty()[0].Codigo,
                            listCatalogTypes.Result.ListOrEmpty()[0].Nombre, listCatalogTypes.Result.ListOrEmpty()[0].Descripcion, listCatalogTypes.Result.ListOrEmpty()[0].Activo,
                            listCatalogTypes.Result.ListOrEmpty()[0].Defecto, listCatalogTypes.Result.ListOrEmpty()[0].EsCompra, listCatalogTypes.Result.ListOrEmpty()[0].EsVenta);
                        await _putDependency.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<CatalogTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return catalogTypeEntity;
            }
        }
    }
}