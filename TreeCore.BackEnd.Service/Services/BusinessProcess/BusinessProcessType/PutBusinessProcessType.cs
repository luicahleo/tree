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
    public class PutBusinessProcessType : PutObjectService<BusinessProcessTypeDTO, BusinessProcessTypeEntity, BusinessProcessTypeDTOMapper>
    {
        private readonly GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> _getDependency;

        public PutBusinessProcessType(PutDependencies<BusinessProcessTypeEntity> putDependency, GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new BusinessProcessTypeValidation())
        {
            _getDependency = getDependency;
        }

        public override async Task<Result<BusinessProcessTypeEntity>> ValidateEntity(BusinessProcessTypeDTO oEntidad, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<BusinessProcessTypeEntity>> listBusinessProcessTypes;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            BusinessProcessTypeEntity? bProcessType = await _getDependency.GetItemByCode(code, client);
            BusinessProcessTypeEntity bProcessTypeFinal = null;

            if (bProcessType == null)
            {
                lErrors.Add(Error.Create(_traduccion.BusinessProcessType + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                bProcessTypeFinal = new BusinessProcessTypeEntity(bProcessType.CoreBusinessProcessTipoID, oEntidad.Code, oEntidad.Name, oEntidad.Active, oEntidad.Description, oEntidad.Default, client);

                filter = new Filter(nameof(BusinessProcessTypeDTO.Code), Operators.eq, oEntidad.Code);
                listFilters.Add(filter);

                listBusinessProcessTypes = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listBusinessProcessTypes.Result != null && listBusinessProcessTypes.Result.ListOrEmpty().Count > 0 &&
                    listBusinessProcessTypes.Result.ListOrEmpty()[0].CoreBusinessProcessTipoID != bProcessTypeFinal.CoreBusinessProcessTipoID)
                {
                    lErrors.Add(Error.Create(_traduccion.BusinessProcessType + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }
                if (oEntidad.Default && !oEntidad.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }

                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(BusinessProcessTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listBusinessProcessTypes = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listBusinessProcessTypes.Result != null && listBusinessProcessTypes.Result.ListOrEmpty().Count > 0)
                    {
                        BusinessProcessTypeEntity pType = new BusinessProcessTypeEntity(listBusinessProcessTypes.Result.ListOrEmpty()[0].CoreBusinessProcessTipoID.Value, listBusinessProcessTypes.Result.ListOrEmpty()[0].Codigo,
                            listBusinessProcessTypes.Result.ListOrEmpty()[0].Nombre, listBusinessProcessTypes.Result.ListOrEmpty()[0].Activo, listBusinessProcessTypes.Result.ListOrEmpty()[0].Descripcion, false, client);
                        await _putDependencies.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<BusinessProcessTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return bProcessTypeFinal;
            }
        }


    }
}
