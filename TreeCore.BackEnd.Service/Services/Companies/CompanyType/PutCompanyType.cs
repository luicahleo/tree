using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{

    public class PutCompanyType : PutObjectService<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper>
    {
        private readonly GetDependencies<CompanyTypeDTO, CompanyTypeEntity> _getDependency;


        public PutCompanyType(PutDependencies<CompanyTypeEntity> putDependency, GetDependencies<CompanyTypeDTO, CompanyTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new CompanyTypeValidation())
        {
            _getDependency = getDependency;
        }


        public override async Task<Result<CompanyTypeEntity>> ValidateEntity(CompanyTypeDTO oEntidad, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<CompanyTypeEntity>> listCompanyTypes;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            CompanyTypeEntity? prodType = await _getDependency.GetItemByCode(code, Client);
            CompanyTypeEntity prodTypeFinal = null;
            if (prodType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCompanyType + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                prodTypeFinal = new CompanyTypeEntity(prodType.EntidadTipoID, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);
                filter = new Filter(nameof(CompanyTypeDTO.Code), Operators.eq, oEntidad.Code);
                listFilters.Add(filter);

                listCompanyTypes = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listCompanyTypes.Result != null && listCompanyTypes.Result.ListOrEmpty().Count > 0 &&
                    listCompanyTypes.Result.ListOrEmpty()[0].EntidadTipoID != prodTypeFinal.EntidadTipoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeCompanyType + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }
                if (oEntidad.Default && !oEntidad.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }

                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(CompanyTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listCompanyTypes = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listCompanyTypes.Result != null && listCompanyTypes.Result.ListOrEmpty().Count > 0)
                    {
                        CompanyTypeEntity pType = new CompanyTypeEntity(listCompanyTypes.Result.ListOrEmpty()[0].EntidadTipoID.Value, Client, listCompanyTypes.Result.ListOrEmpty()[0].Codigo,
                            listCompanyTypes.Result.ListOrEmpty()[0].EntidadTipo, listCompanyTypes.Result.ListOrEmpty()[0].Descripcion, listCompanyTypes.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }
           
            if (lErrors.Count > 0)
            {
                return Result.Failure<CompanyTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return prodTypeFinal;
            }
        }

    }
}
