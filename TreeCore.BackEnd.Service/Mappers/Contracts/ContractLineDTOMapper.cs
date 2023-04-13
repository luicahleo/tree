using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractLineDTOMapper : BaseMapper<ContractLineDTO, ContractLineEntity>
    {
        public override Task<ContractLineDTO> Map(ContractLineEntity oEntity)
        {

            PriceReadjustmentDTO renewalReadjustment = new PriceReadjustmentDTO();
            PriceReadjustmentDTO pricesDTO = new PriceReadjustmentDTO();
            pricesDTO.Type = (oEntity.oReajuste != null) ? oEntity.oReajuste.Tipo : "null";
            pricesDTO.CodeInflation = ((oEntity.oReajuste != null) && (oEntity.oReajuste.Inflacion != null)) ? oEntity.oReajuste.Inflacion.Codigo : "";
            pricesDTO.FixedAmount = (oEntity.oReajuste != null) ? oEntity.oReajuste.CantidadFija : null;
            pricesDTO.FixedPercentage = (oEntity.oReajuste != null) ? oEntity.oReajuste.PorcentajeFijo : null;
            pricesDTO.Frequency = (oEntity.oReajuste != null) ? oEntity.oReajuste.Periodicidad : null;
            pricesDTO.StartDate = (oEntity.oReajuste != null) ? oEntity.oReajuste.FechaInicioReajuste : null;
            pricesDTO.NextDate = (oEntity.oReajuste != null) ? oEntity.oReajuste.FechaProximaReajuste : null;
            pricesDTO.EndDate = (oEntity.oReajuste != null) ? oEntity.oReajuste.FechaFinReajuste : null;


            ContractLineDTO dto = new ContractLineDTO()
            {
                Code = oEntity.CodigoDetalle,

                lineTypeCode = oEntity.oAlquilerConcepto != null ? oEntity.oAlquilerConcepto.Codigo : "",
                Frequency = oEntity.Periodicidad ,
                CurrencyCode = oEntity.oMoneda != null ? oEntity.oMoneda.Simbolo : "",
                Value = oEntity.Importe ,
                Description = oEntity.Descripcion,
                ValidFrom = oEntity.FechaPrimerPago,
                ValidTo = oEntity.FechaUltimoPago,
                NextPaymentDate = oEntity.FechaProximoPago,
               
                ApplyRenewals = oEntity.AplicaProrrogaAutomatica,
                Apportionment = oEntity.Prorrateo,
                Prepaid = oEntity.PagoAnticipado,
                PricesReadjustment = pricesDTO,


            };
            if (oEntity.oAlquileresDetallesImpuestos != null && oEntity.oAlquileresDetallesImpuestos.ToList().Count > 0)
            {
                dto.ContractLineTaxes = new List<ContractLineTaxesDTO>();
                foreach (ContractLineTaxesEntity linkContractLineTaxes in oEntity.oAlquileresDetallesImpuestos.ToList())
                {
                    ContractLineTaxesDTO contractlinetaxe = new ContractLineTaxesDTO();
                    contractlinetaxe.TaxCode = linkContractLineTaxes.oImpuesto.Codigo;
                    
                    dto.ContractLineTaxes.Add(contractlinetaxe);
                }
            }
            return Task.FromResult(dto);
        }
    }
}
