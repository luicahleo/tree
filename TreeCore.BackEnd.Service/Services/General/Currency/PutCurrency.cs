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

    public class PutCurrency : PutObjectService<CurrencyDTO, CurrencyEntity, CurrencyDTOMapper>
    {
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getDependency;


        public PutCurrency(PutDependencies<CurrencyEntity> putDependency, GetDependencies<CurrencyDTO, CurrencyEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new CurrencyValidation())
        {
            _getDependency = getDependency;
        }


        public override async Task<Result<CurrencyEntity>> ValidateEntity(CurrencyDTO currency, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<CurrencyEntity>> listCurrency;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            CurrencyEntity currencyFinal = null;

            CurrencyEntity? cur = await _getDependency.GetItemByCode(code, Client);
            if (cur == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCurrency + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (currency.Default && !currency.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                currencyFinal = new CurrencyEntity(cur.MonedaID, Client, currency.Code, currency.Symbol, currency.DollarChange, currency.EuroChange, DateTime.Now, currency.Active, currency.Default);

                filter = new Filter(nameof(CurrencyDTO.Code), Operators.eq, currency.Code);
                listFilters.Add(filter);

                listCurrency = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listCurrency.Result != null && listCurrency.Result.ListOrEmpty().Count > 0 &&
                    listCurrency.Result.ListOrEmpty()[0].MonedaID != currencyFinal.MonedaID)
                {
                    lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{currency.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
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
                        await _putDependencies.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<CurrencyEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return currencyFinal;
            }
        }
    }
}

