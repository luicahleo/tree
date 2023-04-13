using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class PutContractLineType : PutObjectService<ContractLineTypeDTO, ContractLineTypeEntity, ContractLineTypeDTOMapper>
    {
        private readonly GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> _getDependency;


        public PutContractLineType(PutDependencies<ContractLineTypeEntity> putDependency, GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ContractLineTypeValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<ContractLineTypeEntity>> ValidateEntity(ContractLineTypeDTO contractLineType, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<ContractLineTypeEntity>> listContractLineTypes;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            ContractLineTypeEntity? prodType = await _getDependency.GetItemByCode(code, Client);
            if (prodType == null)
            { return Result.Failure<ContractLineTypeEntity>(_traduccion.CodeContractLineType + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."); }
            ContractLineTypeEntity prodTypeFinal = new ContractLineTypeEntity(prodType.AlquilerConceptoID, Client, contractLineType.Code, contractLineType.Name, contractLineType.Description, contractLineType.Active, contractLineType.Default, contractLineType.Single, contractLineType.Recurrent, contractLineType.Income);

            filter = new Filter(nameof(ContractLineTypeDTO.Code), Operators.eq, contractLineType.Code);
            listFilters.Add(filter);

            listContractLineTypes = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContractLineTypes.Result != null && listContractLineTypes.Result.ListOrEmpty().Count > 0 &&
                listContractLineTypes.Result.ListOrEmpty()[0].AlquilerConceptoID != prodTypeFinal.AlquilerConceptoID)
            {
                return Result.Failure<ContractLineTypeEntity>(_traduccion.CodeContractLineType + " " + $"{contractLineType.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            if (contractLineType.Default && !contractLineType.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (contractLineType.Single && contractLineType.Recurrent)
            {
                lErrors.Add(Error.Create(nameof(ContractLineTypeDTO.Single) + ", " + nameof(ContractLineTypeDTO.Recurrent) + ". " + _errorTraduccion.InconsistentValues));
            }

            if (contractLineType.Default)
            {
                filter = new Filter(nameof(ContractLineTypeDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listContractLineTypes = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listContractLineTypes.Result != null && listContractLineTypes.Result.ListOrEmpty().Count > 0)
                {
                    ContractLineTypeEntity pType = new ContractLineTypeEntity(listContractLineTypes.Result.ListOrEmpty()[0].AlquilerConceptoID.Value, Client, listContractLineTypes.Result.ListOrEmpty()[0].Codigo,
                        listContractLineTypes.Result.ListOrEmpty()[0].AlquilerConcepto, listContractLineTypes.Result.ListOrEmpty()[0].Descripcion, listContractLineTypes.Result.ListOrEmpty()[0].Activo, false, false, false, false);
                    await _putDependencies.Update(pType);
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<ContractLineTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return prodTypeFinal;
            }
        }

    }
}
