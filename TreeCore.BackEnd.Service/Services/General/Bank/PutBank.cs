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

    public class PutBank : PutObjectService<BankDTO, BankEntity, BankDTOMapper>
    {
        private readonly GetDependencies<BankDTO, BankEntity> _getDependency;

        public PutBank(PutDependencies<BankEntity> putDependency,
            GetDependencies<BankDTO, BankEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new BankValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<BankEntity>> ValidateEntity(BankDTO banco, int Client,string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<BankEntity>> listBank;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            BankEntity bancoFinal = null;

            BankEntity? bank = await _getDependency.GetItemByCode(code, Client);
            if (bank == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeBank + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (banco.Default && !banco.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                bancoFinal = new BankEntity(bank.BancoID, Client, banco.Code, banco.Name, banco.Description, banco.Active, banco.Default);

                filter = new Filter(nameof(BankDTO.Code), Operators.eq, banco.Code);
                listFilters.Add(filter);

                listBank = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listBank.Result != null && listBank.Result.ListOrEmpty().Count > 0 &&
                    listBank.Result.ListOrEmpty()[0].BancoID != bancoFinal.BancoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeBank + " " + $"{banco.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (banco.Default)
                {
                    filter = new Filter(nameof(BankDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listBank = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listBank.Result != null && listBank.Result.ListOrEmpty().Count > 0)
                    {
                        BankEntity pType = new BankEntity(listBank.Result.ListOrEmpty()[0].BancoID.Value, Client, listBank.Result.ListOrEmpty()[0].CodigoBanco,
                            listBank.Result.ListOrEmpty()[0].Banco, listBank.Result.ListOrEmpty()[0].Descripcion, listBank.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<BankEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return bancoFinal;
            }
        }
    }
}
