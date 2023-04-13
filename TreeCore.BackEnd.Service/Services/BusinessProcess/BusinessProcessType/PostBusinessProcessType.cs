using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.Service.Validations.BusinessProcess;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;


namespace TreeCore.BackEnd.Service.Services.BusinessProcess
{
    public class PostBusinessProcessType : PostObjectService<BusinessProcessTypeDTO, BusinessProcessTypeEntity, BusinessProcessTypeDTOMapper>
    {


        private readonly GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> _getDependency;
        private readonly PutDependencies<BusinessProcessTypeEntity> _putDependency;

        public PostBusinessProcessType(PostDependencies<BusinessProcessTypeEntity> postDependency, GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> getDependency, PutDependencies<BusinessProcessTypeEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new BusinessProcessTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<BusinessProcessTypeEntity>> ValidateEntity(BusinessProcessTypeDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            BusinessProcessTypeEntity businessProcessTypeEntity = new BusinessProcessTypeEntity(null, oEntidad.Code, oEntidad.Name, oEntidad.Active, oEntidad.Description, oEntidad.Default, client);

            if (!oEntidad.Active && oEntidad.Default)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
           
            filter = new Filter(nameof(BusinessProcessTypeDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<BusinessProcessTypeEntity>> listBusinessProcessTypes = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listBusinessProcessTypes.Result != null && listBusinessProcessTypes.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.BusinessProcessType + " " + _traduccion.Code + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            else
            {
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(BusinessProcessTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listBusinessProcessTypes = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listBusinessProcessTypes.Result != null && listBusinessProcessTypes.Result.ListOrEmpty().Count > 0)
                    {
                        BusinessProcessTypeEntity pType = BusinessProcessTypeEntity.Create(listBusinessProcessTypes.Result.ListOrEmpty()[0].CoreBusinessProcessTipoID.Value, listBusinessProcessTypes.Result.ListOrEmpty()[0].Codigo,
                            listBusinessProcessTypes.Result.ListOrEmpty()[0].Nombre, listBusinessProcessTypes.Result.ListOrEmpty()[0].Activo, listBusinessProcessTypes.Result.ListOrEmpty()[0].Descripcion,false, client);
                        await _putDependency.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<BusinessProcessTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return businessProcessTypeEntity;
            }
        }
    }
}
