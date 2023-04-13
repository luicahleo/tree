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
    public class PostPaymentMethods : PostObjectService<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper>
    {

        private readonly GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> _getDependency;
        private readonly PutDependencies<PaymentMethodsEntity> _putDependency;

        public PostPaymentMethods(PostDependencies<PaymentMethodsEntity> postDependency, GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> getDependency, PutDependencies<PaymentMethodsEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new PaymentMethodsValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<PaymentMethodsEntity>> ValidateEntity(PaymentMethodsDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            PaymentMethodsEntity sapTypeNIFEntity = new PaymentMethodsEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default, oEntidad.RequiresBankAccount);

            filter = new Filter(nameof(PaymentMethodsDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<PaymentMethodsEntity>> listPaymentMethods = _getDependency.GetList(client, listFilters, null, null, -1, -1);

            if (listPaymentMethods.Result != null && listPaymentMethods.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodePaymentMethods + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }

            if (oEntidad.Default)
            {
                filter = new Filter(nameof(PaymentMethodsDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listPaymentMethods = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listPaymentMethods.Result != null && listPaymentMethods.Result.ListOrEmpty().Count > 0)
                {
                    PaymentMethodsEntity pType = PaymentMethodsEntity.Create(listPaymentMethods.Result.ListOrEmpty()[0].MetodoPagoID.Value, client, listPaymentMethods.Result.ListOrEmpty()[0].CodigoMetodoPago,
                        listPaymentMethods.Result.ListOrEmpty()[0].MetodoPago, listPaymentMethods.Result.ListOrEmpty()[0].Descripcion, listPaymentMethods.Result.ListOrEmpty()[0].Activo, false, listPaymentMethods.Result.ListOrEmpty()[0].RequiereBanco);
                    await _putDependency.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<PaymentMethodsEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return sapTypeNIFEntity;
            }
        }
    }
}