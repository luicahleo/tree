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

    public class PutContractGroup : PutObjectService<ContractGroupDTO, ContractGroupEntity,ContractGroupDTOMapper>
    {
        private readonly GetDependencies<ContractGroupDTO, ContractGroupEntity> _getDependency;

        public PutContractGroup(PutDependencies<ContractGroupEntity> putDependency, GetDependencies<ContractGroupDTO, ContractGroupEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ContractGroupValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<ContractGroupEntity>> ValidateEntity(ContractGroupDTO contractGroup, int Client,  string code, string email)
        {
            Task<IEnumerable<ContractGroupEntity>> listContractGroups;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            

            ContractGroupEntity? prodType = await _getDependency.GetItemByCode(code, Client);
            if(prodType==null)
                { return Result.Failure<ContractGroupEntity>(_traduccion.CodeContractGroup + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."); }
            ContractGroupEntity prodTypeFinal = new ContractGroupEntity(prodType.AlquilerTipoContratacionID, Client, contractGroup.Code, contractGroup.Name, contractGroup.Description, contractGroup.Active, contractGroup.Default);

            filter = new Filter(nameof(ContractGroupDTO.Code), Operators.eq, contractGroup.Code);
            listFilters.Add(filter);

            listContractGroups = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContractGroups.Result != null && listContractGroups.Result.ListOrEmpty().Count > 0 &&
                listContractGroups.Result.ListOrEmpty()[0].AlquilerTipoContratacionID != prodTypeFinal.AlquilerTipoContratacionID)
            {
                return Result.Failure<ContractGroupEntity>(_traduccion.CodeContractGroup + " " + $"{contractGroup.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            if (contractGroup.Default && !contractGroup.Active)
            {
                return Result.Failure<ContractGroupEntity>(_traduccion.CodeContractGroup + " " + $"{contractGroup.Code}" + " " + _errorTraduccion.DefaultInactive + ".");
            }
            if (contractGroup.Default)
            {
                filter = new Filter(nameof(ContractGroupDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listContractGroups = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listContractGroups.Result != null && listContractGroups.Result.ListOrEmpty().Count > 0)
                {
                    ContractGroupEntity pType = new ContractGroupEntity(listContractGroups.Result.ListOrEmpty()[0].AlquilerTipoContratacionID.Value, Client, listContractGroups.Result.ListOrEmpty()[0].codigo,
                        listContractGroups.Result.ListOrEmpty()[0].TipoContratacion, listContractGroups.Result.ListOrEmpty()[0].Descripcion, listContractGroups.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependencies.Update(pType);
                }
            }

            return prodTypeFinal;
        }
        
    }
}
