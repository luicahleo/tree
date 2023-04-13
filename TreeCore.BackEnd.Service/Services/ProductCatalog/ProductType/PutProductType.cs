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
    public class PutProductType : PutObjectService<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper>
    {
        private readonly GetDependencies<ProductTypeDTO, ProductTypeEntity> _getDependency;

        public PutProductType(PutDependencies<ProductTypeEntity> putDependency, GetDependencies<ProductTypeDTO, ProductTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ProductTypeValidation())
        {
            _getDependency = getDependency;
        }

        public override async Task<Result<ProductTypeEntity>> ValidateEntity(ProductTypeDTO oEntidad, int client, string email, string code = "")
        {
            Task<IEnumerable<ProductTypeEntity>> listProductTypes;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            ProductTypeEntity? prodType = await _getDependency.GetItemByCode(code, client);
            ProductTypeEntity prodTypeFinal = new ProductTypeEntity(prodType.CoreProductCatalogServicioTipoID, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(ProductTypeDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            listProductTypes = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listProductTypes.Result != null && listProductTypes.Result.ListOrEmpty().Count > 0 &&
                listProductTypes.Result.ListOrEmpty()[0].CoreProductCatalogServicioTipoID != prodTypeFinal.CoreProductCatalogServicioTipoID)
            {
                return Result.Failure<ProductTypeEntity>(_traduccion.CodeProductType + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            if (oEntidad.Default)
            {
                filter = new Filter(nameof(ProductTypeDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listProductTypes = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listProductTypes.Result != null && listProductTypes.Result.ListOrEmpty().Count > 0)
                {
                    ProductTypeEntity pType = new ProductTypeEntity(listProductTypes.Result.ListOrEmpty()[0].CoreProductCatalogServicioTipoID, listProductTypes.Result.ListOrEmpty()[0].Codigo,
                        listProductTypes.Result.ListOrEmpty()[0].Nombre, listProductTypes.Result.ListOrEmpty()[0].Descripcion, listProductTypes.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependencies.Update(pType);
                }
            }

            return prodTypeFinal;
        }
    }
}
