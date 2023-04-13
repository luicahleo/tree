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
    public class PostProductType : PostObjectService<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper>
    {
        private readonly GetDependencies<ProductTypeDTO, ProductTypeEntity> _getDependency;
        private readonly PutDependencies<ProductTypeEntity> _putDependency;

        public PostProductType(PostDependencies<ProductTypeEntity> postDependency, PutDependencies<ProductTypeEntity> putDependency, GetDependencies<ProductTypeDTO, ProductTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, postDependency, new ProductTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ProductTypeEntity>> ValidateEntity(ProductTypeDTO productType, int client, string email, string code = "")
        {
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            ProductTypeEntity productTypeEntity = new ProductTypeEntity(null, productType.Code, productType.Name, productType.Description, productType.Active, productType.Default);

            filter = new Filter(nameof(ProductTypeDTO.Code), Operators.eq, productType.Code);            
            listFilters.Add(filter);

            Task<IEnumerable<ProductTypeEntity>> listProductTypes = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listProductTypes.Result != null && listProductTypes.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<ProductTypeEntity>(_traduccion.CodeProductType + " " + $"{productType.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            else
            {
                if (productType.Default)
                {
                    filter = new Filter(nameof(ProductTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listProductTypes = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listProductTypes.Result != null && listProductTypes.Result.ListOrEmpty().Count > 0)
                    {
                        ProductTypeEntity pType = new ProductTypeEntity(listProductTypes.Result.ListOrEmpty()[0].CoreProductCatalogServicioTipoID, listProductTypes.Result.ListOrEmpty()[0].Codigo,
                            listProductTypes.Result.ListOrEmpty()[0].Nombre, listProductTypes.Result.ListOrEmpty()[0].Descripcion, listProductTypes.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependency.Update(pType);
                    }
                }
            }

            return productTypeEntity;
        }
    }
}
