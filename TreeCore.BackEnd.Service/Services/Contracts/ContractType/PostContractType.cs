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

    public class PostContractType : PostObjectService<ContractTypeDTO, ContractTypeEntity, ContractTypeDTOMapper>
    {

        private readonly GetDependencies<ContractTypeDTO, ContractTypeEntity> _getDependency;
        private readonly PutDependencies<ContractTypeEntity> _putDependency;

        public PostContractType(PostDependencies<ContractTypeEntity> postDependency, GetDependencies<ContractTypeDTO, ContractTypeEntity> getDependency, PutDependencies<ContractTypeEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ContractTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ContractTypeEntity>> ValidateEntity(ContractTypeDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            ContractTypeEntity contractTypeEntity = new ContractTypeEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(ContractTypeDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ContractTypeEntity>> listContractTypes = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContractTypes.Result != null && listContractTypes.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<ContractTypeEntity>(_traduccion.CodeContractType + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            else
            {
                if (oEntidad.Default && !oEntidad.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(ContractTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listContractTypes = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listContractTypes.Result != null && listContractTypes.Result.ListOrEmpty().Count > 0)
                    {
                        ContractTypeEntity pType = ContractTypeEntity.Create(listContractTypes.Result.ListOrEmpty()[0].AlquilerTipoContratoID.Value, Client, listContractTypes.Result.ListOrEmpty()[0].Codigo,
                            listContractTypes.Result.ListOrEmpty()[0].TipoContrato, listContractTypes.Result.ListOrEmpty()[0].Descripcion, listContractTypes.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependency.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<ContractTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return contractTypeEntity;
            }
                
        }


    }
}
