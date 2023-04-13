using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class FuncionalAreaDTOMapper : BaseMapper<FunctionalAreaDTO, FunctionalAreaEntity>
    {
        public override Task<FunctionalAreaDTO> Map(FunctionalAreaEntity functionalArea)
        {
            FunctionalAreaDTO dto = new FunctionalAreaDTO()
            {
                Active = functionalArea.Activo,
                Code = functionalArea.Codigo,
                Default = functionalArea.Defecto,
                Description = functionalArea.Descripcion,
                Name = functionalArea.AreaFuncional
            };
            return Task.FromResult(dto);
        }
    }
}

