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

    public class PostContractLineType : PostObjectService<ContractLineTypeDTO, ContractLineTypeEntity,ContractLineTypeDTOMapper>
    {

        private readonly GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> _getDependency;
        private readonly PutDependencies<ContractLineTypeEntity> _putDependency;

        public PostContractLineType(PostDependencies<ContractLineTypeEntity> postDependency, GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> getDependency, PutDependencies<ContractLineTypeEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ContractLineTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ContractLineTypeEntity>> ValidateEntity(ContractLineTypeDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            ContractLineTypeEntity contractLineTypeEntity = new ContractLineTypeEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default, oEntidad.Single, oEntidad.Recurrent, oEntidad.Income);

            filter = new Filter(nameof(ContractLineTypeDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ContractLineTypeEntity>> listContractLineTypes = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContractLineTypes.Result != null && listContractLineTypes.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<ContractLineTypeEntity>( "Contract Condition " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }
            else
            {
                if (oEntidad.Default && !oEntidad.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                if(oEntidad.Single && oEntidad.Recurrent)
                {
                    lErrors.Add(Error.Create(nameof(ContractLineTypeDTO.Single) + ", " + nameof(ContractLineTypeDTO.Recurrent) + ". " + _errorTraduccion.InconsistentValues));
                }
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(ContractLineTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listContractLineTypes = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listContractLineTypes.Result != null && listContractLineTypes.Result.ListOrEmpty().Count > 0)
                    {
                        ContractLineTypeEntity pType = ContractLineTypeEntity.Create(listContractLineTypes.Result.ListOrEmpty()[0].AlquilerConceptoID.Value, Client, listContractLineTypes.Result.ListOrEmpty()[0].Codigo,
                            listContractLineTypes.Result.ListOrEmpty()[0].AlquilerConcepto, listContractLineTypes.Result.ListOrEmpty()[0].Descripcion, listContractLineTypes.Result.ListOrEmpty()[0].Activo, false, false, false, false);
                        await _putDependency.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<ContractLineTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return contractLineTypeEntity;
            }
        }

        
    }
}
