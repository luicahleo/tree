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

    public class PutTaxes : PutObjectService<TaxesDTO, TaxesEntity, TaxesDTOMapper>
    {
        private readonly GetDependencies<TaxesDTO, TaxesEntity> _getDependency;
        private readonly GetDependencies<CountryDTO, CountryEntity> _getCountryDependency;


        public PutTaxes(PutDependencies<TaxesEntity> putDependency, GetDependencies<TaxesDTO, TaxesEntity> getDependency,

            IHttpContextAccessor httpcontextAccessor, GetDependencies<CountryDTO, CountryEntity> getCountryDependency) :

            base(httpcontextAccessor, putDependency, new TaxesValidation())
        {
            _getDependency = getDependency;
            _getCountryDependency = getCountryDependency;
        }
        public override async Task<Result<TaxesEntity>> ValidateEntity(TaxesDTO taxes, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<TaxesEntity>> listTaxess;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            TaxesEntity prodTypeFinal = null;

            TaxesEntity ? prodType = await _getDependency.GetItemByCode(code, client);
            if (prodType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeTaxes + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (taxes.Default && !taxes.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                CountryEntity country = await _getCountryDependency.GetItemByCode(taxes.CountryCode, client);
                if (country == null)
                {
                    lErrors.Add(Error.Create(_traduccion.NameCountry + " " + $"{taxes.CountryCode}" + " " + _errorTraduccion.NotFound + "."));
                }
                prodTypeFinal = new TaxesEntity(prodType.ImpuestoID, client, taxes.Code, taxes.Name, taxes.Description,DateTime.Now, taxes.Valor, country, taxes.Active, taxes.Default);

                filter = new Filter(nameof(TaxesDTO.Code), Operators.eq, taxes.Code);
                listFilters.Add(filter);

                listTaxess = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listTaxess.Result != null && listTaxess.Result.ListOrEmpty().Count > 0 &&
                    listTaxess.Result.ListOrEmpty()[0].ImpuestoID != prodTypeFinal.ImpuestoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeTaxes + " " + $"{taxes.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (taxes.Default)
                {
                    filter = new Filter(nameof(TaxesDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listTaxess = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listTaxess.Result != null && listTaxess.Result.ListOrEmpty().Count > 0)
                    {
                        TaxesEntity pType = new TaxesEntity(listTaxess.Result.ListOrEmpty()[0].ImpuestoID.Value, client, listTaxess.Result.ListOrEmpty()[0].Codigo,
                            listTaxess.Result.ListOrEmpty()[0].Impuesto, listTaxess.Result.ListOrEmpty()[0].Descripcion, DateTime.Now, listTaxess.Result.ListOrEmpty()[0].Valor, listTaxess.Result.ListOrEmpty()[0].Paises, listTaxess.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<TaxesEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return prodTypeFinal;
            }
        }
    }
}
