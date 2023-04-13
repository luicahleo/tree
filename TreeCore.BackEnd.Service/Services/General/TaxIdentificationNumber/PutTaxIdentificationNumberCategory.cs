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

    public class PutTaxIdentificationNumberCategory : PutObjectService<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity, TaxIdentificationNumberCategoryDTOMapper>
    {
        private readonly GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> _getDependency;

        public PutTaxIdentificationNumberCategory(PutDependencies<TaxIdentificationNumberCategoryEntity> putDependency, GetDependencies<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity> getDependency, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, putDependency, new TaxIdentificationNumberCategoryValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<TaxIdentificationNumberCategoryEntity>> ValidateEntity(TaxIdentificationNumberCategoryDTO sapTypeNIF, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<TaxIdentificationNumberCategoryEntity>> listSAPTypeNIFs;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            TaxIdentificationNumberCategoryEntity? prodType = await _getDependency.GetItemByCode(code, client);
            TaxIdentificationNumberCategoryEntity prodTypeFinal = null;

            if (prodType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeSAPTypeNIF + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                prodTypeFinal = new TaxIdentificationNumberCategoryEntity(prodType.SAPTipoNIFID, client, sapTypeNIF.Code, sapTypeNIF.Name, sapTypeNIF.Description, sapTypeNIF.Active, sapTypeNIF.Default);

                filter = new Filter(nameof(TaxIdentificationNumberCategoryDTO.Code), Operators.eq, sapTypeNIF.Code);
                listFilters.Add(filter);

                listSAPTypeNIFs = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listSAPTypeNIFs.Result != null && listSAPTypeNIFs.Result.ListOrEmpty().Count > 0 &&
                    listSAPTypeNIFs.Result.ListOrEmpty()[0].SAPTipoNIFID != prodTypeFinal.SAPTipoNIFID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeSAPTypeNIF + " " + $"{sapTypeNIF.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (sapTypeNIF.Default && !sapTypeNIF.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }

                if (sapTypeNIF.Default)
                {
                    filter = new Filter(nameof(TaxIdentificationNumberCategoryDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listSAPTypeNIFs = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listSAPTypeNIFs.Result != null && listSAPTypeNIFs.Result.ListOrEmpty().Count > 0)
                    {
                        TaxIdentificationNumberCategoryEntity pType = new TaxIdentificationNumberCategoryEntity(listSAPTypeNIFs.Result.ListOrEmpty()[0].SAPTipoNIFID.Value, client, listSAPTypeNIFs.Result.ListOrEmpty()[0].Codigo,
                            listSAPTypeNIFs.Result.ListOrEmpty()[0].Nombre, listSAPTypeNIFs.Result.ListOrEmpty()[0].Descripcion, listSAPTypeNIFs.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<TaxIdentificationNumberCategoryEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return prodTypeFinal;
            }
        }
    }
}
