using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class TaxesDTOMapper : BaseMapper<TaxesDTO, TaxesEntity>
    {
        public override Task<TaxesDTO> Map(TaxesEntity oEntity)
        {
            TaxesDTO dto = new TaxesDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Valor = (int)oEntity.Valor,
                CountryCode = (oEntity.Paises != null) ? oEntity.Paises.Pais : "",
                Name = oEntity.Impuesto
            };
            return Task.FromResult(dto);
        }
    }
}

