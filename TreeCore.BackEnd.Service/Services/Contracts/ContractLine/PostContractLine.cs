using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;
using System.Collections;
using System.Collections.Generic;
using TreeCore.Shared.Data.Query;
using System.Collections.Immutable;
using TreeCore.Shared.DTO.ValueObject;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.BackEnd.Service.Services.Contracts.ContractLineTaxes;
using TreeCore.BackEnd.Service.Services.Contracts.ContractLineEntidad;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class PostContractLine : PostObjectService<ContractLineDTO, ContractLineEntity, ContractLineDTOMapper>
    {

        private readonly GetDependencies<ContractLineDTO, ContractLineEntity> _getDependency;

        private readonly GetDependencies<ContractDTO, ContractEntity> _getDependencyContract;
        private readonly GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> _getDependencyContractLineType;

        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getDependencyCurrency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly GetDependencies<InflationDTO, InflationEntity> _getInflationDependency;
        private readonly PostContractLineTaxes _getPostDependencyContractLineTaxes;
        private readonly PostContractLineEntidad _getPostDependencyContractLineEntidades;




        public PostContractLine(PostDependencies<ContractLineEntity> postDependency, GetDependencies<ContractLineDTO, ContractLineEntity> getDependency, 
            GetDependencies<ContractDTO, ContractEntity> getDependencyContract,
            GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> getDependencyContractLineType,
           GetDependencies<CurrencyDTO, CurrencyEntity> getDependencyCurrency,
           GetDependencies<InflationDTO, InflationEntity> getInflationDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            PostContractLineTaxes getPostDependencyContractLineTaxes,
            PostContractLineEntidad getPostDependencyContractLineEntidades,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ContractLineValidation())
        {
            _getDependency = getDependency;
            _getDependencyContract = getDependencyContract;
            _getDependencyContractLineType = getDependencyContractLineType;
            _getDependencyCurrency = getDependencyCurrency;
            _getInflationDependency = getInflationDependency;
            _getUserDependency = getUserDependency;
            _getPostDependencyContractLineTaxes = getPostDependencyContractLineTaxes;
            _getPostDependencyContractLineEntidades = getPostDependencyContractLineEntidades;




        }

      


      
        public async Task<Result<List<ContractLineEntity>>> ValidateEntity(List<ContractLineDTO> linkedContractLine, int clientID, string code )
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<ContractLineEntity> linkedContractLineEntity = new List<ContractLineEntity>();

            foreach (ContractLineDTO contractline in linkedContractLine)
            {
                if (controlRepetido(linkedContractLine, contractline))
                {
                    lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + $"{contractline.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                #region OBJETCT VALUE

               
                List<Error> lErrorsReadjustment = new List<Error>();
                InflationEntity inflation = null;
                string tipo;
                float? fixedAmount;
                float? fixedPercentage;
                float? frequency;
                DateTime? startDate;
                DateTime? nextDate;
                DateTime? endDate;
                if (contractline.PricesReadjustment != null)
                {
                   
                    
                    inflation = await _getInflationDependency.GetItemByCode(contractline.PricesReadjustment.CodeInflation, clientID);
                    tipo = contractline.PricesReadjustment.Type;
                    if(tipo!= PReadjustment.sPCI && tipo != PReadjustment.sFixedAmount && tipo != PReadjustment.sFixedPercentege && tipo != PReadjustment.sWithoutIncrements)
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + $"{tipo}" + " " + _errorTraduccion.FormatError + "."));
                    }
                        
                     fixedAmount = contractline.PricesReadjustment.FixedAmount;
                    fixedPercentage = contractline.PricesReadjustment.FixedPercentage;
                    frequency = contractline.PricesReadjustment.Frequency;
                    startDate = contractline.PricesReadjustment.StartDate;
                    nextDate = contractline.PricesReadjustment.NextDate;
                    endDate = contractline.PricesReadjustment.EndDate;

                    PriceReadjustmentDTO.Readjustment pType = PriceReadjustmentDTO.Readjustment.GetByCode<PriceReadjustmentDTO.Readjustment>(tipo);
                    PriceReadjustmentDTO pRead = new PriceReadjustmentDTO();

                    if (pType != null)
                    {
                        pRead.Type = (pType != null) ? pType.Name : "null";
                        pRead.CodeInflation = (inflation != null) ? inflation.Codigo : null;
                        pRead.FixedAmount = fixedAmount;
                        pRead.FixedPercentage = fixedPercentage;
                        pRead.Frequency = frequency;
                        pRead.StartDate = startDate;
                        pRead.NextDate = nextDate;
                        pRead.EndDate = endDate;
                        lErrorsReadjustment = pType.ValidateObject(pRead);

                        if (lErrorsReadjustment.Count > 0)
                        {
                            foreach (Error err in lErrorsReadjustment)
                            {
                                lErrors.Add(err);
                            }
                        }
                    }
                }
                else
                {
                    inflation = null;
                    tipo = null;
                    fixedAmount = null;
                    fixedPercentage = null;
                    frequency = null;
                    startDate = null;
                    nextDate = null;
                    endDate = null;
                }

                PriceReadjustment prices = new PriceReadjustment(tipo, inflation, fixedAmount, fixedPercentage, frequency, startDate, nextDate, endDate);
                #endregion
              
                ContractLineTypeEntity oContractlineType = await _getDependencyContractLineType.GetItemByCode(contractline.lineTypeCode, clientID);
                CurrencyEntity oCurrency = await _getDependencyCurrency.GetItemByCode(contractline.CurrencyCode, clientID);

               
                if(contractline.Code != null && contractline.Code != "" && oContractlineType != null && contractline.lineTypeCode != "" && oCurrency != null && contractline.CurrencyCode != "")
                {

                    
                    List<ContractLineTaxesEntity> listContractLineTaxes = new List<ContractLineTaxesEntity>();
                    if (contractline.ContractLineTaxes != null && contractline.ContractLineTaxes.Count > 0)
                    {
                        Result<List<ContractLineTaxesEntity>> listContractLineTaxesValidity = await _getPostDependencyContractLineTaxes.ValidateEntity(contractline.ContractLineTaxes, clientID, contractline.Code);
                        if (listContractLineTaxesValidity.Success)
                        {
                            float TotalImpuestos = 0;
                         
                            listContractLineTaxes = listContractLineTaxesValidity.Value;
                        }
                        else
                        {
                            lErrors.AddRange(listContractLineTaxesValidity.Errors);
                        }

                    }
                    List<ContractLineEntidadEntity> listContractLineEntidades = new List<ContractLineEntidadEntity>();
                    if (contractline.Payees != null && contractline.Payees.Count > 0)
                    {
                        Result<List<ContractLineEntidadEntity>> listContractLineEntidadesValidity = await _getPostDependencyContractLineEntidades.ValidateEntity(contractline.Payees, clientID);
                        if (listContractLineEntidadesValidity.Success)
                        {
                            listContractLineEntidades = listContractLineEntidadesValidity.Value;
                        }
                        else
                        {
                            lErrors.AddRange(listContractLineEntidadesValidity.Errors);
                        }

                    }
                    int NumberOfPayment = 0;
                        float Total = 0;
                    float TotalWithRenewals = 0;
                    float ImporteMensual = 0;
                    float ImporteAnual = 0;
                    if (contractline.Frequency > 0)
                    {
                        ImporteMensual = contractline.Value / contractline.Frequency;
                        ImporteAnual = (contractline.Value / contractline.Frequency) * 12;
                        NumberOfPayment = CalcularNumCuotasByAlquilerDetalle(contractline);
                        Total = (float)CalcularTotalConceptoByAlquilerDetalle(contractline);
                    }
                    linkedContractLineEntity.Add(new ContractLineEntity(null, contractline.Code,
                              null,
                              oContractlineType,
                              contractline.Frequency,
                              contractline.Value,
                              oCurrency,
                              contractline.Description,
                              contractline.ValidFrom,
                              contractline.ValidTo,
                              contractline.NextPaymentDate,
                              NumberOfPayment,
                              Total,
                              TotalWithRenewals,
                              contractline.ApplyRenewals,
                              contractline.Apportionment,
                              contractline.Prepaid,
                              prices, listContractLineTaxes, listContractLineEntidades));
                }
                else
                {
                    if (contractline.Code == null && contractline.Code != "")
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + $"{contractline.Code}" + " " + _errorTraduccion.FormatError + "."));
                    }
                    if(oContractlineType == null && contractline.lineTypeCode != "") 
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + $"{contractline.Code}" + _traduccion.CodeContractLineType + " " + $"{contractline.Code}" + " " + _errorTraduccion.NotFound + "."));
                    }
                    if (oCurrency == null && contractline.CurrencyCode != "")
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractLine + " " + $"{contractline.Code}" + _traduccion.CodeCurrency + " " + $"{contractline.CurrencyCode}" + " " + _errorTraduccion.NotFound + "."));
                    }
                }
                
            };


            if (lErrors.Count > 0)
            {
                return Result.Failure<List<ContractLineEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return linkedContractLineEntity;
            }
        }

        public override Task<Result<ContractLineEntity>> ValidateEntity(ContractLineDTO oEntidad, int clientID, string email, string code = "")
        {
            throw new NotImplementedException();
        }

        private bool controlRepetido(List<ContractLineDTO> lista, ContractLineDTO elemento)
        {
            int cont = 0;
            foreach (ContractLineDTO contractline in lista)
            {
                if (elemento.Code == contractline.Code)
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
        #region GET NUMERO CUOTAS CONCEPTO
        public int CalcularNumCuotasByAlquilerDetalle(ContractLineDTO alqDetalle)
        {
            //Calcular el numero de cuotas del concepto de pago
            int numCuotas = 0;

            if (alqDetalle.ValidFrom != DateTime.MinValue &&
               alqDetalle.ValidTo != DateTime.MinValue)
            {
                DateTime fechaInicioPago = alqDetalle.ValidFrom;
                int periodicidad = 0;
                DateTime fechaFinPago = alqDetalle.ValidTo;
                Meses cMeses = new Meses();
                int mesesPagos = 0;


                periodicidad = alqDetalle.Frequency;

                if (fechaFinPago.Year > 1 && periodicidad > 0 && fechaInicioPago.Year > 0)
                {
                    if (fechaFinPago.Year == fechaInicioPago.Year && fechaFinPago.Month == fechaInicioPago.Month)
                    {
                        mesesPagos = 0;
                        numCuotas = (mesesPagos / periodicidad) + 1;
                    }
                    else
                    {
                        mesesPagos = cMeses.GetMonthDiff2(fechaInicioPago, fechaFinPago);

                        if (mesesPagos % periodicidad > 0)
                        {
                            numCuotas = (mesesPagos / periodicidad) + 1;
                        }
                        else
                        {
                            numCuotas = mesesPagos / periodicidad;
                        }

                    }

                }
                else
                {
                    numCuotas = 1;
                }
            }




            return numCuotas;
        }
        #endregion

        #region GET TOTAL CONCEPTO ALQUILER DETALLE
        public double CalcularTotalConceptoByAlquilerDetalle(ContractLineDTO alqDetalle)
        {
            //Calcular el total del concepto
            double totalConcepto = 0;
            if (alqDetalle.ValidFrom != DateTime.MinValue &&
                 alqDetalle.ValidTo != DateTime.MinValue)
            {
                DateTime fechaInicioPago = alqDetalle.ValidFrom;
                int periodicidad = (int)alqDetalle.Frequency;
                DateTime fechaFinPago = alqDetalle.ValidTo;
                Meses cMeses = new Meses();
                int mesesPagos = 0;
                int numCuotas = 0;
                double importe = (double)alqDetalle.Value;


                if (fechaFinPago.Year > 1 && periodicidad > 0 && fechaInicioPago.Year > 0)
                {
                    if (fechaFinPago.Year == fechaInicioPago.Year && fechaFinPago.Month == fechaInicioPago.Month)
                    {
                        mesesPagos = 0;
                        numCuotas = (mesesPagos / periodicidad) + 1;
                        totalConcepto = numCuotas * importe;
                    }
                    else
                    {
                        mesesPagos = cMeses.GetMonthDiff2(fechaInicioPago, fechaFinPago);
                        if (mesesPagos % periodicidad > 0)
                        {
                            numCuotas = (mesesPagos / periodicidad) + 1;
                            totalConcepto = numCuotas * importe;
                        }
                        else
                        {
                            numCuotas = (mesesPagos / periodicidad);
                            totalConcepto = numCuotas * importe;
                        }


                    }

                }
                else
                {
                    totalConcepto = importe;
                }
            }



            return totalConcepto;
        }
        #endregion
    }
}
