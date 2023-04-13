using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class PostTaxIdentificationNumberCategory : PostObjectService<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity, TaxIdentificationNumberCategoryDTOMapper>
    {

        private readonly GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> _getDependency;
        private readonly PutDependencies<TaxIdentificationNumberCategoryEntity> _putDependency;

        public PostTaxIdentificationNumberCategory(PostDependencies<TaxIdentificationNumberCategoryEntity> postDependency, GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> getDependency, PutDependencies<TaxIdentificationNumberCategoryEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new TaxIdentificationNumberCategoryValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<TaxIdentificationNumberCategoryEntity>> ValidateEntity(TaxIdentificationNumberCategoryDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            TaxIdentificationNumberCategoryEntity sapTypeNIFEntity = new TaxIdentificationNumberCategoryEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(TaxIdentificationNumberCategoryDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<TaxIdentificationNumberCategoryEntity>> listSAPTypeNIFs = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listSAPTypeNIFs.Result != null && listSAPTypeNIFs.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeSAPTypeNIF + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }

            if (oEntidad.Default)
            {
                filter = new Filter(nameof(TaxIdentificationNumberCategoryDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listSAPTypeNIFs = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listSAPTypeNIFs.Result != null && listSAPTypeNIFs.Result.ListOrEmpty().Count > 0)
                {
                    TaxIdentificationNumberCategoryEntity pType = TaxIdentificationNumberCategoryEntity.Create(listSAPTypeNIFs.Result.ListOrEmpty()[0].SAPTipoNIFID.Value, client, listSAPTypeNIFs.Result.ListOrEmpty()[0].Codigo,
                        listSAPTypeNIFs.Result.ListOrEmpty()[0].Nombre, listSAPTypeNIFs.Result.ListOrEmpty()[0].Descripcion, listSAPTypeNIFs.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<TaxIdentificationNumberCategoryEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return sapTypeNIFEntity;
            }
        }
    }
}