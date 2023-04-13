using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class PostContractStatus : PostObjectService<ContractStatusDTO, ContractStatusEntity, ContractStatusDTOMapper>
    {

        private readonly GetDependencies<ContractStatusDTO, ContractStatusEntity> _getDependency;
        private readonly PutDependencies<ContractStatusEntity> _putDependency;

        public PostContractStatus(PostDependencies<ContractStatusEntity> postDependency, GetDependencies<ContractStatusDTO, ContractStatusEntity> getDependency, PutDependencies<ContractStatusEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ContractStatusValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ContractStatusEntity>> ValidateEntity(ContractStatusDTO oEntidad, int Client,string email, string code = "")
        {
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            ContractStatusEntity contractStateEntity = new ContractStatusEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default, false);

            filter = new Filter(nameof(ContractStatusDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ContractStatusEntity>> listContractStates = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (oEntidad.Default && !oEntidad.Active)
            {
                return Result.Failure<ContractStatusEntity>(_traduccion.CodeContractState + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.DefaultInactive + ".");
            }
            if (listContractStates.Result != null && listContractStates.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<ContractStatusEntity>(_traduccion.CodeContractState + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            else
            {
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(ContractStatusDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listContractStates = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listContractStates.Result != null && listContractStates.Result.ListOrEmpty().Count > 0)
                    {
                        ContractStatusEntity pType = ContractStatusEntity.Create(listContractStates.Result.ListOrEmpty()[0].AlquilerEstadoID.Value, Client, listContractStates.Result.ListOrEmpty()[0].codigo,
                            listContractStates.Result.ListOrEmpty()[0].Estado, listContractStates.Result.ListOrEmpty()[0].Descripcion, listContractStates.Result.ListOrEmpty()[0].Activo, false, false);
                        await _putDependency.Update(pType);
                    }
                }
            }

            return contractStateEntity;
        }


    }
}
