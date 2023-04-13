using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;

namespace TreeCore.BackEnd.Service.Mappers.Contracts
{

    public class ContractLineEntidadDTOMapper : BaseMapper<ContractLineEntidadDTO, ContractLineEntidadEntity>
    {
        public override Task<ContractLineEntidadDTO> Map(ContractLineEntidadEntity contractlineEntidad)
        {
            ContractLineEntidadDTO dto = new ContractLineEntidadDTO()
            {

                CompanyCode = contractlineEntidad.oMetodoPagoEntidad.CoreCompany.Codigo,
                BankAcountCode=contractlineEntidad.oMoneda.Simbolo,
                currencyCode=contractlineEntidad.oMoneda.Simbolo,
                PaymentMethodCode=contractlineEntidad.oMetodoPagoEntidad.CoreMetodosPagos.CodigoMetodoPago,
                Percent=contractlineEntidad.CantidadPorcentaje

            };
            return Task.FromResult(dto);

        }
    }
}