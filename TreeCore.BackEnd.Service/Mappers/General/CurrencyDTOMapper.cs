using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class CurrencyDTOMapper : BaseMapper<CurrencyDTO, CurrencyEntity>
    {
        public override Task<CurrencyDTO> Map(CurrencyEntity currency)
        {
            CurrencyDTO dto = new CurrencyDTO()
            {
                Code = currency.Moneda,
                Symbol = currency.Simbolo,
                DollarChange = currency.CambioDollarUS,
                EuroChange = currency.CambioEuro,
                Active = currency.Activo,
                Default = currency.Defecto
            };
            return Task.FromResult(dto);
        }
    }
}
