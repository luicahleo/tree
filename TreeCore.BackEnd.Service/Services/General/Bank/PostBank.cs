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
    public class PostBank : PostObjectService<BankDTO, BankEntity, BankDTOMapper>
    {

        private readonly GetDependencies<BankDTO, BankEntity> _getDependency;
        private readonly PutDependencies<BankEntity> _putDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;

        public PostBank(PostDependencies<BankEntity> postDependency,
            GetDependencies<BankDTO, BankEntity> getDependency,
            PutDependencies<BankEntity> putDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new BankValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<BankEntity>> ValidateEntity(BankDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            BankEntity banco = new BankEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(BankDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            Task<IEnumerable<BankEntity>> listBank = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listBank.Result != null && listBank.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeBank + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (oEntidad.Default)
            {
                filter = new Filter(nameof(BankDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listBank = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listBank.Result != null && listBank.Result.ListOrEmpty().Count > 0)
                {
                    BankEntity pType = BankEntity.Create(listBank.Result.ListOrEmpty()[0].BancoID.Value, Client, listBank.Result.ListOrEmpty()[0].CodigoBanco,
                        listBank.Result.ListOrEmpty()[0].Banco, listBank.Result.ListOrEmpty()[0].Descripcion, listBank.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<BankEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return banco;
            }
        }
    }
}