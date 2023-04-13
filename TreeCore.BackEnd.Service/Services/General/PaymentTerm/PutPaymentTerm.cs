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

    public class PutPaymentTerm : PutObjectService<PaymentTermDTO, PaymentTermEntity, PaymentTermDTOMapper>
    {
        private readonly GetDependencies<PaymentTermDTO, PaymentTermEntity> _getDependency;


        public PutPaymentTerm(PutDependencies<PaymentTermEntity> putDependency,
            GetDependencies<PaymentTermDTO, PaymentTermEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new PaymentTermValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<PaymentTermEntity>> ValidateEntity(PaymentTermDTO paymentTerm, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<PaymentTermEntity>> listpaymentTerms;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            PaymentTermEntity paymentTermFinal = null;

            PaymentTermEntity? prodType = await _getDependency.GetItemByCode(code, client);
            if (prodType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodePaymentTerm + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                if (paymentTerm.Default && !paymentTerm.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }
                paymentTermFinal = new PaymentTermEntity(prodType.CondicionPagoID, client, paymentTerm.Code, paymentTerm.Name, paymentTerm.Description, paymentTerm.Active, paymentTerm.Default);

                filter = new Filter(nameof(PaymentTermDTO.Code), Operators.eq, paymentTerm.Code);
                listFilters.Add(filter);

                listpaymentTerms = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listpaymentTerms.Result != null && listpaymentTerms.Result.ListOrEmpty().Count > 0 &&
                    listpaymentTerms.Result.ListOrEmpty()[0].CondicionPagoID != paymentTermFinal.CondicionPagoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodePaymentTerm + " " + $"{paymentTerm.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (paymentTerm.Default)
                {
                    filter = new Filter(nameof(PaymentTermDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listpaymentTerms = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listpaymentTerms.Result != null && listpaymentTerms.Result.ListOrEmpty().Count > 0)
                    {
                        PaymentTermEntity pType = new PaymentTermEntity(listpaymentTerms.Result.ListOrEmpty()[0].CondicionPagoID.Value, client, listpaymentTerms.Result.ListOrEmpty()[0].Codigo,
                            listpaymentTerms.Result.ListOrEmpty()[0].CondicionPago, listpaymentTerms.Result.ListOrEmpty()[0].Descripcion, listpaymentTerms.Result.ListOrEmpty()[0].Activo, false);
                        await _putDependencies.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<PaymentTermEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return paymentTermFinal;
            }
        }
    }
}
