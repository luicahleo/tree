using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class PutContractStatus : PutObjectService<ContractStatusDTO, ContractStatusEntity, ContractStatusDTOMapper>
    {
        private readonly GetDependencies<ContractStatusDTO, ContractStatusEntity> _getDependency;

        public PutContractStatus(PutDependencies<ContractStatusEntity> putDependency, GetDependencies<ContractStatusDTO, ContractStatusEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ContractStatusValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<ContractStatusEntity>> ValidateEntity(ContractStatusDTO contractState, int Client, string code,string email)
        {
            Task<IEnumerable<ContractStatusEntity>> listContractStates;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            ContractStatusEntity? contractstate = await _getDependency.GetItemByCode(code, Client);
            if (contractstate == null)
            { return Result.Failure<ContractStatusEntity>(_traduccion.CodeContractState + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."); }
            ContractStatusEntity contractstateFinal = new ContractStatusEntity(contractstate.AlquilerEstadoID, Client, contractState.Code, contractState.Name, contractState.Description, contractState.Active, contractState.Default, false);

            filter = new Filter(nameof(ContractStatusDTO.Code), Operators.eq, contractState.Code);
            listFilters.Add(filter);

            listContractStates = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContractStates.Result != null && listContractStates.Result.ListOrEmpty().Count > 0 &&
                listContractStates.Result.ListOrEmpty()[0].AlquilerEstadoID != contractstateFinal.AlquilerEstadoID)
            {
                return Result.Failure<ContractStatusEntity>(_traduccion.CodeContractState + " " + $"{contractState.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            if (contractState.Default && !contractState.Active)
            {
                return Result.Failure<ContractStatusEntity>(_traduccion.CodeContractState + " " + $"{contractState.Code}" + " " + _errorTraduccion.DefaultInactive + ".");
            }
            if (contractState.Default)
            {
                filter = new Filter(nameof(ContractStatusDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listContractStates = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listContractStates.Result != null && listContractStates.Result.ListOrEmpty().Count > 0)
                {
                    ContractStatusEntity pType = new ContractStatusEntity(listContractStates.Result.ListOrEmpty()[0].AlquilerEstadoID.Value, Client, listContractStates.Result.ListOrEmpty()[0].codigo,
                        listContractStates.Result.ListOrEmpty()[0].Estado, listContractStates.Result.ListOrEmpty()[0].Descripcion, listContractStates.Result.ListOrEmpty()[0].Activo, false, false);
                    await _putDependencies.Update(pType);
                }
            }

            return contractstateFinal;
        }

    }
}
