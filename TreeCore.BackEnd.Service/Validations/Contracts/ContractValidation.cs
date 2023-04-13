using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;
using System;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{

    public class ContractValidation : BasicValidation<ContractDTO, ContractEntity>
    {
        public override Result<ContractEntity> ValidateEntity(ContractEntity contract, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contract.NumContrato.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.NombreContrato.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.FechaFirmaContrato > contract.FechaInicioContrato)
            {
                listaErrores.Add(Error.Create(_traduccion.ExecutionDateError));
            }
            if (contract.FechaInicioContrato > contract.FechaFinContrato)
            {
                listaErrores.Add(Error.Create(_traduccion.StartDate));
            }
            //Meses cMeses = new Meses();
            //DateTime datefincontrato = cMeses.sumarMesesAFecha(contract.FechaInicioContrato, contract.Cadencias);
            //if (datefincontrato != contract.FechaFinContrato)
            //{
            //    listaErrores.Add(Error.Create(_traduccion.StartDate));
            //}
            foreach (ContractLineEntity contractline in contract.oAlquileresDetalles)
            {

                if (contractline.CodigoDetalle.Length > 150)
                {
                    listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
                }
                if (contractline.oAlquilerConcepto.Codigo.Length > 150)
                {
                    listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
                }

                if (contractline.Descripcion.Length > 500)
                {
                    listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
                }

                if (contractline.oAlquilerConcepto.EsPagoUnico || contractline.oAlquilerConcepto.EsFianza)
                {
                    if (contractline.FechaPrimerPago != contractline.FechaProximoPago || contractline.FechaPrimerPago != contractline.FechaUltimoPago)
                    {
                        listaErrores.Add(Error.Create(_traduccion.CodeContractLine +": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateSinglePayment));
                    }
                }
                if (contractline.oAlquilerConcepto.EsAlquilerBase || contractline.oAlquilerConcepto.EsPagoAdicional)
                {
                    if (contractline.Periodicidad <= 0)
                    {
                        listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFrecuency+" "+ contractline.Periodicidad + " :" + " " + _traduccion.FormatIncorrect));

                    }
                    if (contractline.FechaPrimerPago == System.DateTime.MinValue)
                    {
                        listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + contractline.FechaPrimerPago + " :" + " " + _traduccion.FormatIncorrect));
                    }
                    if (contractline.FechaProximoPago == System.DateTime.MinValue)
                    {
                        listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + contractline.FechaProximoPago + " :" + " " + _traduccion.FormatIncorrect));
                    }
                    if (contractline.FechaUltimoPago == System.DateTime.MinValue)
                    {
                        listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + contractline.FechaUltimoPago + " :" + " " + _traduccion.FormatIncorrect));
                    }




                    if (contractline.oReajuste.FechaInicioReajuste != null && contractline.oReajuste.FechaInicioReajuste != System.DateTime.MinValue)
                    {


                        if (contractline.FechaPrimerPago > contractline.oReajuste.FechaInicioReajuste)
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsStartDateReadjustment));
                        }
                        if (contract.FechaInicioContrato > contractline.FechaPrimerPago || (contract.oAjusteRenovaciones.ProrrogaAutomatica == false && contract.oAjusteRenovaciones.CadenciaProrrogaAutomatica == null && contract.FechaFinContrato < contractline.FechaPrimerPago))
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsFirstEndDate));
                        }
                        if (contract.FechaInicioContrato >  contractline.FechaPrimerPago || (contract.oAjusteRenovaciones.ProrrogaAutomatica == true && contract.oAjusteRenovaciones.FechaFinContratoAuxiliar < contractline.FechaPrimerPago))
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsRenewDate));
                        }
                        if (contract.FechaInicioContrato > contractline.FechaPrimerPago || (contract.oAjusteRenovaciones.ProrrogaAutomatica == false && contract.oAjusteRenovaciones.CadenciaProrrogaAutomatica!=null && contract.oAjusteRenovaciones.FechaFinContratoAuxiliar < contractline.FechaPrimerPago))
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsRenewDate));
                        }
                        if (contractline.FechaPrimerPago > contractline.FechaProximoPago || contractline.FechaPrimerPago > contractline.FechaUltimoPago || contractline.FechaProximoPago > contractline.FechaUltimoPago)
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDate));
                        }


                    }
                    else
                    {
                        if (contractline.FechaPrimerPago > contractline.FechaProximoPago || contractline.FechaPrimerPago > contractline.FechaUltimoPago || contractline.FechaProximoPago > contractline.FechaUltimoPago)
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDate));
                        }
                        if (contract.FechaInicioContrato > contractline.FechaPrimerPago || (contract.oAjusteRenovaciones.ProrrogaAutomatica == true && contract.oAjusteRenovaciones.FechaFinContratoAuxiliar < contractline.FechaPrimerPago))
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsRenewDate));
                        }
                        if (contract.FechaInicioContrato > contractline.FechaPrimerPago || (contract.oAjusteRenovaciones.ProrrogaAutomatica == false && contract.oAjusteRenovaciones.CadenciaProrrogaAutomatica != null && contract.oAjusteRenovaciones.FechaFinContratoAuxiliar < contractline.FechaPrimerPago))
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsRenewDate));
                        }
                        if (contract.FechaInicioContrato > contractline.FechaPrimerPago || (contract.oAjusteRenovaciones.ProrrogaAutomatica == false && contract.oAjusteRenovaciones.CadenciaProrrogaAutomatica == null && contract.FechaFinContrato < contractline.FechaPrimerPago))
                        {
                            listaErrores.Add(Error.Create(_traduccion.CodeContractLine + ": " + contractline.CodigoDetalle + " :" + " " + _traduccion.ValidFromDateVsFirstEndDate));
                        }
                    }
                }
            }







            return listaErrores.Any() ?
                Result.Failure<ContractEntity>(listaErrores.ToImmutableArray())
                : contract;
        }

        public override Result<ContractDTO> ValidateDTO(ContractDTO contract, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contract.Code.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.Name.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.SiteCode.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.ContractGroupCode.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.ContractTypeCode.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.ContractStatusCode.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.ExecutionDate > contract.StartDate)
            {
                listaErrores.Add(Error.Create(_traduccion.ExecutionDateError));
            }
            if (contract.StartDate > contract.FirstEndDate)
            {
                listaErrores.Add(Error.Create(_traduccion.StartDate));
            }
            //Meses cMeses = new Meses();
            //DateTime datefincontrato = cMeses.sumarMesesAFecha(contract.StartDate, ((contract.FirstEndDate.Year - contract.StartDate.Year) * 12) + contract.FirstEndDate.Month - contract.StartDate.Month);
            //if (datefincontrato != contract.FirstEndDate)
            //{
            //    listaErrores.Add(Error.Create(_traduccion.StartDate));
            //}
            foreach (ContractLineDTO contractlineDTO in contract.contractline)
            {
                if (contractlineDTO.Code.Length > 150)
                {
                    listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
                }
                if (contractlineDTO.lineTypeCode.Length > 150)
                {
                    listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
                }

                if (contractlineDTO.Description.Length > 500)
                {
                    listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
                }

                if (contractlineDTO.PricesReadjustment.StartDate != null && contractlineDTO.PricesReadjustment.StartDate != System.DateTime.MinValue)
                {
                    if (contractlineDTO.ValidFrom > contractlineDTO.PricesReadjustment.StartDate)
                    {
                        listaErrores.Add(Error.Create("contract line code: " + contractlineDTO.Code + " :" + " " + _traduccion.ValidFromDateVsStartDateReadjustment));
                    }
                    if (contract.StartDate > contractlineDTO.ValidFrom || contract.RenewalClause.RenewalExpirationDate < contractlineDTO.ValidFrom)
                    {
                        listaErrores.Add(Error.Create("contract line code: " + contractlineDTO.Code + " :" + " " + _traduccion.ValidFromDateVsRenewDate));
                    }
                }
                else
                {
                    if (contract.StartDate > contractlineDTO.ValidFrom || contract.RenewalClause.RenewalExpirationDate < contractlineDTO.ValidFrom)
                    {
                        listaErrores.Add(Error.Create("contract line code: " + contractlineDTO.Code + " :" + " " + _traduccion.ValidFromDateVsRenewDate));
                    }
                }
            }

            return listaErrores.Any() ?
                Result.Failure<ContractDTO>(listaErrores.ToImmutableArray())
                : contract;
        }
    }
}
