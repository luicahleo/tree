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
    public class PostCompanyType : PostObjectService<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper>
    {
        private readonly GetDependencies<CompanyTypeDTO, CompanyTypeEntity> _getDependency;
        private readonly PutDependencies<CompanyTypeEntity> _putDependency;


        public PostCompanyType(PostDependencies<CompanyTypeEntity> postDependency, GetDependencies<CompanyTypeDTO, CompanyTypeEntity> getDependency, PutDependencies<CompanyTypeEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new CompanyTypeValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }


        public override async Task<Result<CompanyTypeEntity>> ValidateEntity(CompanyTypeDTO companyType, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            CompanyTypeEntity companyTypeEntity = new CompanyTypeEntity(null, Client, companyType.Code, companyType.Name, companyType.Description, companyType.Active, companyType.Default);

            filter = new Filter(nameof(CompanyTypeDTO.Code), Operators.eq, companyType.Code);
            listFilters.Add(filter);

            Task<IEnumerable<CompanyTypeEntity>> listCompanyTypes = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listCompanyTypes.Result != null && listCompanyTypes.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCompanyType + " " + $"{companyType.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (companyType.Default && !companyType.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (companyType.Default)
            {
                filter = new Filter(nameof(CompanyTypeDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listCompanyTypes = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listCompanyTypes.Result != null && listCompanyTypes.Result.ListOrEmpty().Count > 0)
                {
                    CompanyTypeEntity pType = new CompanyTypeEntity(listCompanyTypes.Result.ListOrEmpty()[0].EntidadTipoID, Client, listCompanyTypes.Result.ListOrEmpty()[0].Codigo,
                        listCompanyTypes.Result.ListOrEmpty()[0].EntidadTipo, listCompanyTypes.Result.ListOrEmpty()[0].Descripcion, listCompanyTypes.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }


            if (lErrors.Count > 0)
            {
                return Result.Failure<CompanyTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return companyTypeEntity;
            }
        }
    }
}
