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

    public class PostContractGroup : PostObjectService<ContractGroupDTO, ContractGroupEntity,ContractGroupDTOMapper>
    {

        private readonly GetDependencies<ContractGroupDTO, ContractGroupEntity> _getDependency;
        private readonly PutDependencies<ContractGroupEntity> _putDependency;

        public PostContractGroup(PostDependencies<ContractGroupEntity> postDependency, GetDependencies<ContractGroupDTO, ContractGroupEntity> getDependency, PutDependencies<ContractGroupEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ContractGroupValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ContractGroupEntity>> ValidateEntity(ContractGroupDTO oEntidad, int client, string email, string code = "")
        {
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            ContractGroupEntity contractGroupEntity = new ContractGroupEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(ContractGroupDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ContractGroupEntity>> listContractGroups = _getDependency.GetList(client, listFilters, null, null, -1, -1);

            if (oEntidad.Default && !oEntidad.Active)
            {
                return Result.Failure<ContractGroupEntity>(_traduccion.CodeContractGroup + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.DefaultInactive + ".");
            }
            if (listContractGroups.Result != null && listContractGroups.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<ContractGroupEntity>(_traduccion.CodeContractGroup + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            else
            {
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(ContractGroupDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listContractGroups = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listContractGroups.Result != null && listContractGroups.Result.ListOrEmpty().Count > 0)
                    {
                        ContractGroupEntity pType = ContractGroupEntity.Create(listContractGroups.Result.ListOrEmpty()[0].AlquilerTipoContratacionID.Value, client, listContractGroups.Result.ListOrEmpty()[0].codigo,
                            listContractGroups.Result.ListOrEmpty()[0].TipoContratacion, listContractGroups.Result.ListOrEmpty()[0].Descripcion, listContractGroups.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependency.Update(pType);
                    }
                }
            }

            return contractGroupEntity;
        }

        
    }
}
