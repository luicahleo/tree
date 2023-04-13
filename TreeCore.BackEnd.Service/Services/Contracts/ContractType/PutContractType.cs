using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class PutContractType : PutObjectService<ContractTypeDTO, ContractTypeEntity, ContractTypeDTOMapper>
    {
        private readonly GetDependencies<ContractTypeDTO, ContractTypeEntity> _getDependency;

        public PutContractType(PutDependencies<ContractTypeEntity> putDependency, GetDependencies<ContractTypeDTO, ContractTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ContractTypeValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<ContractTypeEntity>> ValidateEntity(ContractTypeDTO contractType, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<ContractTypeEntity>> listContractTypes;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            ContractTypeEntity? prodType = await _getDependency.GetItemByCode(code, Client);
            if (prodType == null)
            { return Result.Failure<ContractTypeEntity>(_traduccion.CodeContractType + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."); }
            ContractTypeEntity prodTypeFinal = new ContractTypeEntity(prodType.AlquilerTipoContratoID, Client, contractType.Code, contractType.Name, contractType.Description, contractType.Active, contractType.Default);

            filter = new Filter(nameof(ContractTypeDTO.Code), Operators.eq, contractType.Code);
            listFilters.Add(filter);

            

            listContractTypes = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContractTypes.Result != null && listContractTypes.Result.ListOrEmpty().Count > 0 &&
                listContractTypes.Result.ListOrEmpty()[0].AlquilerTipoContratoID != prodTypeFinal.AlquilerTipoContratoID)
            {
                return Result.Failure<ContractTypeEntity>(_traduccion.CodeContractType + " " + $"{contractType.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            if (contractType.Default && !contractType.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }

            if (contractType.Default)
            {
                filter = new Filter(nameof(ContractTypeDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listContractTypes = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listContractTypes.Result != null && listContractTypes.Result.ListOrEmpty().Count > 0)
                {
                    ContractTypeEntity pType = new ContractTypeEntity(listContractTypes.Result.ListOrEmpty()[0].AlquilerTipoContratoID.Value, Client, listContractTypes.Result.ListOrEmpty()[0].Codigo,
                        listContractTypes.Result.ListOrEmpty()[0].TipoContrato, listContractTypes.Result.ListOrEmpty()[0].Descripcion, listContractTypes.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependencies.Update(pType);
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<ContractTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return prodTypeFinal;
            }
        }

    }
}
