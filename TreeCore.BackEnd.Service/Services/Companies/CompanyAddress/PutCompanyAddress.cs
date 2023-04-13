using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
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

    public class PutCompanyAddress : PutObjectService<CompanyAddressDTO, CompanyAddressEntity, CompanyAddressDTOMapper>
    {
        private readonly GetDependencies<CompanyAddressDTO, CompanyAddressEntity> _getDependency;


        public PutCompanyAddress(PutDependencies<CompanyAddressEntity> putDependency, GetDependencies<CompanyAddressDTO, CompanyAddressEntity> getDependency, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new CompanyAddressValidation())
        {
            _getDependency = getDependency;
        }


        public override async Task<Result<CompanyAddressEntity>> ValidateEntity(CompanyAddressDTO companyAddress, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<CompanyAddressEntity>> listCompanyAddresss;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            CompanyAddressEntity? prodAddress = await _getDependency.GetItemByCode(code, Client);
            CompanyAddressEntity companyAddressEntity = new CompanyAddressEntity(prodAddress.EntidadDireccionID, null, companyAddress.Code, companyAddress.Name, companyAddress.Default, JsonSerializer.Serialize(companyAddress.Address));

            filter = new Filter(nameof(CompanyAddressDTO.Code), Operators.eq, companyAddress.Code);
            listFilters.Add(filter);

            listCompanyAddresss = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listCompanyAddresss.Result != null && listCompanyAddresss.Result.ListOrEmpty().Count > 0 &&
                listCompanyAddresss.Result.ListOrEmpty()[0].EntidadDireccionID != companyAddressEntity.EntidadDireccionID)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCompanyAddress + " " + $"{companyAddress.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (companyAddress.Default)
            {
                filter = new Filter(nameof(CompanyAddressDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listCompanyAddresss = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listCompanyAddresss.Result != null && listCompanyAddresss.Result.ListOrEmpty().Count > 0)
                {
                    CompanyAddressEntity pAddress = new CompanyAddressEntity(listCompanyAddresss.Result.ListOrEmpty()[0].EntidadDireccionID, null, listCompanyAddresss.Result.ListOrEmpty()[0].Codigo,
                            listCompanyAddresss.Result.ListOrEmpty()[0].EntidadDireccion, false, listCompanyAddresss.Result.ListOrEmpty()[0].DireccionJSON);
                    await _putDependencies.Update(pAddress);
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<CompanyAddressEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return companyAddressEntity;
            }
        }

        public async Task<Result<List<CompanyAddressEntity>>> ValidateEntity(List<CompanyAddressDTO> linkedcompanyAddress, int clientID, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<CompanyAddressEntity> linkedCompanyAddressEntity = new List<CompanyAddressEntity>();

            foreach (CompanyAddressDTO companyAddress in linkedcompanyAddress)
            {
                if (controlRepetido(linkedcompanyAddress, companyAddress))
                {
                    lErrors.Add(Error.Create(_traduccion.CodeCompanyAddress + " " + $"{companyAddress.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }
                

                linkedCompanyAddressEntity.Add(new CompanyAddressEntity(null, null, companyAddress.Code, companyAddress.Name, companyAddress.Default, JsonSerializer.Serialize(companyAddress.Address)));

            };

            if (controlDefecto(linkedcompanyAddress))
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultAlreadyExist));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<List<CompanyAddressEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return linkedCompanyAddressEntity;
            }
        }

        private bool controlRepetido(List<CompanyAddressDTO> lista, CompanyAddressDTO elemento)
        {
            int cont = 0;
            foreach (CompanyAddressDTO companyAddress in lista)
            {
                if (elemento.Code == companyAddress.Code)
                {
                    cont++;
                }
            }
            if (cont > 1)
            {
                return true;
            }
            return false;
        }
        private bool controlDefecto(List<CompanyAddressDTO> lista)
        {
            int cont = 0;
            foreach (CompanyAddressDTO companyAddress in lista)
            {
                if (companyAddress.Default == true)
                {
                    cont++;
                }
            }
            if (cont > 1)
            {
                return true;
            }
            return false;
        }

    }
}
