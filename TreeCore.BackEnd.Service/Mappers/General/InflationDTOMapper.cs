using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class InflationDTOMapper : BaseMapper<InflationDTO, InflationEntity>
    {
        public override Task<InflationDTO> Map(InflationEntity inflation)
        {
            InflationDTO dto = new InflationDTO()
            {
                Code = inflation.Codigo,
                Name = inflation.Inflacion,
                Description = inflation.Descripcion,
                Estandar = inflation.Estandar,
                Active = inflation.Activo,
                CountryName = (inflation.Paises != null) ? inflation.Paises.Pais : ""
            };
            return Task.FromResult(dto);
        }
    }
}