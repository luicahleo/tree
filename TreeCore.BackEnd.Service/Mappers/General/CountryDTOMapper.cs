using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class CountryDTOMapper : BaseMapper<CountryDTO, CountryEntity>
    {
        public override Task<CountryDTO> Map(CountryEntity Country)
        {
            CountryDTO dto = new CountryDTO()
            {
                Code = Country.PaisCod,
                Name = Country.Pais,
                Default= Country.Defecto
            };
            return Task.FromResult(dto);
        }
    }
}
