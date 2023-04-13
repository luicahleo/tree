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
    public class PostPaymentTerm : PostObjectService<PaymentTermDTO, PaymentTermEntity, PaymentTermDTOMapper>
    {

        private readonly GetDependencies<PaymentTermDTO, PaymentTermEntity> _getDependency;
        private readonly PutDependencies<PaymentTermEntity> _putDependency;


        public PostPaymentTerm(PostDependencies<PaymentTermEntity> postDependency,
            GetDependencies<PaymentTermDTO, PaymentTermEntity> getDependency,
            PutDependencies<PaymentTermEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new PaymentTermValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<PaymentTermEntity>> ValidateEntity(PaymentTermDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            PaymentTermEntity sapTypeNIFEntity = new PaymentTermEntity(null, client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Default);

            filter = new Filter(nameof(PaymentTermDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<PaymentTermEntity>> listPaymentTerm = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listPaymentTerm.Result != null && listPaymentTerm.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodePaymentTerm + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (oEntidad.Default && !oEntidad.Active)
            {
                lErrors.Add(Error.Create(_errorTraduccion.DefaultInactive));
            }
            if (oEntidad.Default)
            {
                filter = new Filter(nameof(PaymentTermDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);

                listPaymentTerm = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listPaymentTerm.Result != null && listPaymentTerm.Result.ListOrEmpty().Count > 0)
                {
                    PaymentTermEntity pType = PaymentTermEntity.Create(listPaymentTerm.Result.ListOrEmpty()[0].CondicionPagoID.Value, client, listPaymentTerm.Result.ListOrEmpty()[0].Codigo,
                        listPaymentTerm.Result.ListOrEmpty()[0].CondicionPago, listPaymentTerm.Result.ListOrEmpty()[0].Descripcion, listPaymentTerm.Result.ListOrEmpty()[0].Activo, false);
                    await _putDependency.Update(pType);
                }
            }


            if (lErrors.Count > 0)
            {
                return Result.Failure<PaymentTermEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return sapTypeNIFEntity;
            }
        }
    }
}