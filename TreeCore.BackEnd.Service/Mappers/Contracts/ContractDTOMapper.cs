using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.ValueObject;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractDTOMapper : BaseMapper<ContractDTO, ContractEntity>
    {
        public override Task<ContractDTO> Map(ContractEntity oEntity)
        {

            RenewalClauseDTO renewalReadjustment = new RenewalClauseDTO();
            if (oEntity.oAjusteRenovaciones != null)
            {
                if (oEntity.oAjusteRenovaciones.ProrrogaAutomatica == true)
                {
                    renewalReadjustment.Type = RenewalReadjustment.sAuto;
                }
                else if (oEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica != 0 && oEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica != null)
                {
                    renewalReadjustment.Type = RenewalReadjustment.sOptional;
                }
                else if (oEntity.oAjusteRenovaciones.ProrrogaAutomatica == false && oEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica == null && oEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica == null)
                {
                    renewalReadjustment.Type = RenewalReadjustment.sPreviousNegotiation;
                }
                else
                {
                    renewalReadjustment.Type = "";
                }


                renewalReadjustment.Frequencymonths = oEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica;
                renewalReadjustment.TotalRenewalNumber = oEntity.oAjusteRenovaciones.NumProrrogasMaximas;
                renewalReadjustment.CurrentRenewalNumber = oEntity.oAjusteRenovaciones.ProrrogasConsumidas;
                renewalReadjustment.RenewalExpirationDate = oEntity.oAjusteRenovaciones.FechaFinContratoAuxiliar;
                renewalReadjustment.NotificationNumberDays = oEntity.oAjusteRenovaciones.NumdiasNotificacion;
                renewalReadjustment.Renewalnotificationdate = oEntity.oAjusteRenovaciones.FechaNotificacionRenovacion;
                renewalReadjustment.ContractExpirationDate = oEntity.oAjusteRenovaciones.FechaEfectivaFinContrato;
            }


            ContractDTO dto = new ContractDTO()
            {
                Code = oEntity.NumContrato,
                Name = oEntity.NombreContrato,
                ContractStatusCode = oEntity.oAlquilerEstado != null ? oEntity.oAlquilerEstado.codigo : "",
                SiteCode = oEntity.oSite != null ? oEntity.oSite.Codigo : "",
                CurrencyCode = oEntity.oMoneda != null ? oEntity.oMoneda.Moneda : "",
                ContractGroupCode = oEntity.oAlquilerTipoContratacion != null ? oEntity.oAlquilerTipoContratacion.codigo : "",
                ContractTypeCode = oEntity.oAlquilerTipoContrato != null ? oEntity.oAlquilerTipoContrato.Codigo : "",
                Description = oEntity.ComentariosGenerales,
                MasterContractNumber = "",
                ExecutionDate = oEntity.FechaFirmaContrato,
                StartDate = oEntity.FechaInicioContrato,
                FirstEndDate = oEntity.FechaFinContrato,
                ClosedAtExpiration = oEntity.ExpirarAVencimiento,
                RenewalClause = renewalReadjustment,
                CreationUserEmail = (oEntity.CreadoPor != null) ? oEntity.CreadoPor.EMail : "",
                LastModificationUserEmail = (oEntity.ModificadoPor != null) ? oEntity.ModificadoPor.EMail : "",
                CreationDate = oEntity.FechaCreacion,
                LastModificationDate = oEntity.FechaModificacion,


            };
            if (oEntity.oAlquileresDetalles != null && oEntity.oAlquileresDetalles.ToList().Count > 0)
            {
                dto.contractline = new List<ContractLineDTO>();
                foreach (ContractLineEntity linkContractLine in oEntity.oAlquileresDetalles.ToList())
                {
                    PriceReadjustmentDTO Readjustment = new PriceReadjustmentDTO();
                    PriceReadjustmentDTO pricesDTO = new PriceReadjustmentDTO();
                    pricesDTO.Type = (linkContractLine.oReajuste != null && linkContractLine.oReajuste.Tipo!=null) ? linkContractLine.oReajuste.Tipo : PReadjustment.sWithoutIncrements;
                    pricesDTO.CodeInflation = ((linkContractLine.oReajuste != null) && (linkContractLine.oReajuste.Inflacion != null)) ? linkContractLine.oReajuste.Inflacion.Codigo : "";
                    pricesDTO.FixedAmount = (linkContractLine.oReajuste != null) ? linkContractLine.oReajuste.CantidadFija : null;
                    pricesDTO.FixedPercentage = (linkContractLine.oReajuste != null) ? linkContractLine.oReajuste.PorcentajeFijo : null;
                    pricesDTO.Frequency = (linkContractLine.oReajuste != null) ? linkContractLine.oReajuste.Periodicidad : null;
                    pricesDTO.StartDate = (linkContractLine.oReajuste != null) ? linkContractLine.oReajuste.FechaInicioReajuste : null;
                    pricesDTO.NextDate = (linkContractLine.oReajuste != null) ? linkContractLine.oReajuste.FechaProximaReajuste : null;
                    pricesDTO.EndDate = (linkContractLine.oReajuste != null) ? linkContractLine.oReajuste.FechaFinReajuste : null;

                    ContractLineDTO contractline = new ContractLineDTO();
                    contractline.Code = linkContractLine.CodigoDetalle;

                    contractline.lineTypeCode = linkContractLine.oAlquilerConcepto != null ? linkContractLine.oAlquilerConcepto.Codigo : "";
                    contractline.Frequency = linkContractLine.Periodicidad;
                    contractline.CurrencyCode = linkContractLine.oMoneda != null ? linkContractLine.oMoneda.Moneda : "";
                    contractline.Value = linkContractLine.Importe;
                    contractline.Description = linkContractLine.Descripcion;
                    contractline.ValidFrom = linkContractLine.FechaPrimerPago;
                    contractline.ValidTo = linkContractLine.FechaUltimoPago;
                    contractline.NextPaymentDate = linkContractLine.FechaProximoPago;

                    contractline.ApplyRenewals = linkContractLine.AplicaProrrogaAutomatica;
                    contractline.Apportionment = linkContractLine.Prorrateo;
                    contractline.Prepaid = linkContractLine.PagoAnticipado;
                    contractline.PricesReadjustment = pricesDTO;
                    if (linkContractLine.oAlquileresDetallesImpuestos != null && linkContractLine.oAlquileresDetallesImpuestos.ToList().Count > 0)
                    {
                        contractline.ContractLineTaxes = new List<ContractLineTaxesDTO>();
                        foreach (ContractLineTaxesEntity linkContractLineTaxes in linkContractLine.oAlquileresDetallesImpuestos.ToList())
                        {
                            ContractLineTaxesDTO contractlinetaxe = new ContractLineTaxesDTO();
                            contractlinetaxe.TaxCode = linkContractLineTaxes.oImpuesto.Codigo;

                            contractline.ContractLineTaxes.Add(contractlinetaxe);
                        }
                    }
                    if (linkContractLine.oAlquileresDetallesEntidades != null && linkContractLine.oAlquileresDetallesEntidades.ToList().Count > 0)
                    {
                        contractline.Payees = new List<ContractLineEntidadDTO>();
                        foreach (ContractLineEntidadEntity linkContractLineEntidades in linkContractLine.oAlquileresDetallesEntidades.ToList())
                        {
                            ContractLineEntidadDTO contractlineentidad = new ContractLineEntidadDTO();
                            contractlineentidad.CompanyCode = linkContractLineEntidades.oMetodoPagoEntidad.CoreCompany.Codigo; 
                            contractlineentidad.BankAcountCode = (linkContractLineEntidades.oEntidadCuentaBankaria != null) ? linkContractLineEntidades.oEntidadCuentaBankaria.Codigo : null;
                            contractlineentidad.currencyCode = linkContractLineEntidades.oMoneda.Moneda;
                            contractlineentidad.PaymentMethodCode = linkContractLineEntidades.oMetodoPagoEntidad.CoreMetodosPagos.CodigoMetodoPago;
                            contractlineentidad.Percent = linkContractLineEntidades.CantidadPorcentaje;

                            contractline.Payees.Add(contractlineentidad);
                        }
                    }

                    dto.contractline.Add(contractline);
                }
            }


            return Task.FromResult(dto);
        }
    }
}
