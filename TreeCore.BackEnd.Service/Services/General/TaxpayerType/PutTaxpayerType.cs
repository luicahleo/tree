using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class PutTaxpayerType : PutObjectService<TaxpayerTypeDTO, TaxpayerTypeEntity, TaxpayerTypeDTOMapper>
    {
        private readonly GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> _getDependency;

        public PutTaxpayerType(PutDependencies<TaxpayerTypeEntity> putDependency, GetDependencies<TaxpayerTypeDTO, TaxpayerTypeEntity> getDependency, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, putDependency, new TaxpayerTypeValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<TaxpayerTypeEntity>> ValidateEntity(TaxpayerTypeDTO paymentTerm, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<TaxpayerTypeEntity>> listTaxpayerType;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            TaxpayerTypeEntity? taxpayerType = await _getDependency.GetItemByCode(code, client);
            TaxpayerTypeEntity paymentTermFinal = null;

            if (taxpayerType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeTaxpayerType + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                paymentTermFinal = new TaxpayerTypeEntity(taxpayerType.TipoContribuyenteID, client, paymentTerm.Code, paymentTerm.Name, paymentTerm.Description, paymentTerm.Active, paymentTerm.Default);

                filter = new Filter(nameof(TaxpayerTypeDTO.Code), Operators.eq, paymentTerm.Code);
                listFilters.Add(filter);

                listTaxpayerType = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listTaxpayerType.Result != null && listTaxpayerType.Result.ListOrEmpty().Count > 0 &&
                    listTaxpayerType.Result.ListOrEmpty()[0].TipoContribuyenteID != paymentTermFinal.TipoContribuyenteID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeTaxpayerType + " " + $"{paymentTerm.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (paymentTerm.Default && !paymentTerm.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }

                if (paymentTerm.Default)
                {
                    filter = new Filter(nameof(TaxpayerTypeDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listTaxpayerType = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listTaxpayerType.Result != null && listTaxpayerType.Result.ListOrEmpty().Count > 0)
                    {
                        TaxpayerTypeEntity pType = new TaxpayerTypeEntity(listTaxpayerType.Result.ListOrEmpty()[0].TipoContribuyenteID.Value, client, listTaxpayerType.Result.ListOrEmpty()[0].Codigo,
                            listTaxpayerType.Result.ListOrEmpty()[0].TipoContribuyente, listTaxpayerType.Result.ListOrEmpty()[0].Descripcion, listTaxpayerType.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<TaxpayerTypeEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return paymentTermFinal;
            }
        }
    }
}
