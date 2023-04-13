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

    public class PutPaymentMethods : PutObjectService<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper>
    {
        private readonly GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> _getDependency;

        public PutPaymentMethods(PutDependencies<PaymentMethodsEntity> putDependency, GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> getDependency, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, putDependency, new PaymentMethodsValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<PaymentMethodsEntity>> ValidateEntity(PaymentMethodsDTO paymentTerm, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<PaymentMethodsEntity>> listPaymentMethods;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            PaymentMethodsEntity? paymentMethods = await _getDependency.GetItemByCode(code, client);
            PaymentMethodsEntity paymentTermFinal = null;

            if (paymentMethods == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodePaymentMethods + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                paymentTermFinal = new PaymentMethodsEntity(paymentMethods.MetodoPagoID, client, paymentTerm.Code, paymentTerm.Name, paymentTerm.Description, paymentTerm.Active, paymentTerm.Default, paymentTerm.RequiresBankAccount);

                filter = new Filter(nameof(PaymentMethodsDTO.Code), Operators.eq, paymentTerm.Code);
                listFilters.Add(filter);

                listPaymentMethods = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listPaymentMethods.Result != null && listPaymentMethods.Result.ListOrEmpty().Count > 0 &&
                    listPaymentMethods.Result.ListOrEmpty()[0].MetodoPagoID != paymentTermFinal.MetodoPagoID)
                {
                    lErrors.Add(Error.Create(_traduccion.CodePaymentMethods + " " + $"{paymentTerm.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (paymentTerm.Default && !paymentTerm.Active)
                {
                    lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
                }

                if (paymentTerm.Default)
                {
                    filter = new Filter(nameof(PaymentMethodsDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);

                    listPaymentMethods = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                    if (listPaymentMethods.Result != null && listPaymentMethods.Result.ListOrEmpty().Count > 0)
                    {
                        PaymentMethodsEntity pType = new PaymentMethodsEntity(listPaymentMethods.Result.ListOrEmpty()[0].MetodoPagoID.Value, client, listPaymentMethods.Result.ListOrEmpty()[0].CodigoMetodoPago,
                            listPaymentMethods.Result.ListOrEmpty()[0].MetodoPago, listPaymentMethods.Result.ListOrEmpty()[0].Descripcion, listPaymentMethods.Result.ListOrEmpty()[0].Activo, false, listPaymentMethods.Result.ListOrEmpty()[0].RequiereBanco);
                        await _putDependencies.Update(pType);
                    }
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<PaymentMethodsEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return paymentTermFinal;
            }
        }
    }
}
