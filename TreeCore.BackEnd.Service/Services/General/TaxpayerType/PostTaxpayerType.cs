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
    public class PostTaxpayerType : PostObjectService<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper>
    {

        private readonly GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> _getDependency;
        private readonly PutDependencies<TaxpayerTypeEntity> _putDependency;

        public PostTaxpayerType(PostDependencies<TaxpayerTypeEntity> postDependency, GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> getDependency, PutDependencies<TaxpayerTypeEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new TaxpayerTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<TaxpayerTypeEntity>> ValidateEntity(TaxpayerTypeDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            TaxpayerTypeEntity sapTypeNIFEntity = new TaxpayerTypeEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(TaxpayerTypeDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<TaxpayerTypeEntity>> listTaxpayerType = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listTaxpayerType.Result != null && listTaxpayerType.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeTaxpayerType + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }

            if (oEntidad.Default)
            {
                filter = new Filter(nameof(TaxpayerTypeDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listTaxpayerType = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listTaxpayerType.Result != null && listTaxpayerType.Result.ListOrEmpty().Count > 0)
                {
                    TaxpayerTypeEntity pType = TaxpayerTypeEntity.Create(listTaxpayerType.Result.ListOrEmpty()[0].TipoContribuyenteID.Value, client, listTaxpayerType.Result.ListOrEmpty()[0].Codigo,
                        listTaxpayerType.Result.ListOrEmpty()[0].TipoContribuyente, listTaxpayerType.Result.ListOrEmpty()[0].Descripcion, listTaxpayerType.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<TaxpayerTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return sapTypeNIFEntity;
            }
        }
    }
}