using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class PostTaxes : PostObjectService<TaxesDTO, TaxesEntity, TaxesDTOMapper>
    {

        private readonly GetDependencies<TaxesDTO, TaxesEntity> _getDependency;
        private readonly PutDependencies<TaxesEntity> _putDependency;
        private readonly GetDependencies<CountryDTO, CountryEntity> _getCountryDependency;

        public PostTaxes(PostDependencies<TaxesEntity> postDependency, GetDependencies<TaxesDTO, TaxesEntity> getDependency, PutDependencies<TaxesEntity> putDependency,

            IHttpContextAccessor httpcontextAccessor, GetDependencies<CountryDTO, CountryEntity> getCountryDependency) :
            base(httpcontextAccessor, postDependency, new TaxesValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getCountryDependency = getCountryDependency;
        }

        public override async Task<Result<TaxesEntity>> ValidateEntity(TaxesDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            CountryEntity country = await _getCountryDependency.GetItemByCode(oEntidad.CountryCode, client);
            if (country == null)
            {
                lErrors.Add(Error.Create(_traduccion.NameCountry + " " + $"{oEntidad.CountryCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            TaxesEntity taxesEntity = new TaxesEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description,DateTime.Now, oEntidad.Valor, country, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(TaxesDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<TaxesEntity>> listTaxess = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listTaxess.Result != null && listTaxess.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeTaxes + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }

            if (oEntidad.Default)
            {
                filter = new Filter(nameof(TaxesDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listTaxess = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listTaxess.Result != null && listTaxess.Result.ListOrEmpty().Count > 0)
                {
                    TaxesEntity pType = TaxesEntity.Create(listTaxess.Result.ListOrEmpty()[0].ImpuestoID.Value, client, listTaxess.Result.ListOrEmpty()[0].Codigo,
                        listTaxess.Result.ListOrEmpty()[0].Impuesto, listTaxess.Result.ListOrEmpty()[0].Descripcion, DateTime.Now, (int)listTaxess.Result.ListOrEmpty()[0].Valor, listTaxess.Result.ListOrEmpty()[0].Paises, listTaxess.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

                
            if (lErrors.Count > 0)
            {
                return Result.Failure<TaxesEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return taxesEntity;
            }
        }
    }
}
