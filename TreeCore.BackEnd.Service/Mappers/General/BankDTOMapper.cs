using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class BankDTOMapper : BaseMapper<BankDTO, BankEntity>
    {
        public override Task<BankDTO> Map(BankEntity oEntity)
        {
            BankDTO dto = new BankDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.CodigoBanco,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Name = oEntity.Banco
            };
            return Task.FromResult(dto);
        }
    }
    
}
