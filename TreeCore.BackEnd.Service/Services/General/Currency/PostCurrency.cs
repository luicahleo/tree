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
    public class PostCurrency : PostObjectService<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper>
    {

        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getDependency;
        private readonly PutDependencies<CurrencyEntity> _putDependency;

        public PostCurrency(PostDependencies<CurrencyEntity> postDependency, GetDependencies<CurrencyDTO, CurrencyEntity> getDependency, PutDependencies<CurrencyEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new CurrencyValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<CurrencyEntity>> ValidateEntity(CurrencyDTO currency, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            CurrencyEntity currencyEntity = new CurrencyEntity(null, Client, currency.Code, currency.Symbol, currency.DollarChange, currency.EuroChange, DateTime.Now, currency.Active, currency.Default);

            filter = new Filter(nameof(CurrencyDTO.Code), Operators.eq, currency.Code);
            listFilters.Add(filter);

            Task<IEnumerable<CurrencyEntity>> listCurrency = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listCurrency.Result != null && listCurrency.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{currency.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (currency.Default && !currency.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }

            if (currency.Default)
            {
                filter = new Filter(nameof(CurrencyDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listCurrency = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listCurrency.Result != null && listCurrency.Result.ListOrEmpty().Count > 0)
                {
                    CurrencyEntity pType = new CurrencyEntity(listCurrency.Result.ListOrEmpty()[0].MonedaID, Client, listCurrency.Result.ListOrEmpty()[0].Moneda,
                        listCurrency.Result.ListOrEmpty()[0].Simbolo, listCurrency.Result.ListOrEmpty()[0].CambioDollarUS, listCurrency.Result.ListOrEmpty()[0].CambioEuro,
                        DateTime.Now, listCurrency.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<CurrencyEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return currencyEntity;
            }
        }
    }
}
