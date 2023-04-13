using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.Sites;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class ProgramDTOMapper : BaseMapper<ProgramDTO, ProgramEntity>
    {
        public override Task<ProgramDTO> Map(ProgramEntity oEntity)
        {
            ProgramDTO dto = new ProgramDTO()
            {
                Code = oEntity.Codigo,
                Name = oEntity.Nombre,
                Description = oEntity.Descripcion,
                Active = oEntity.Activo,

            };
            return Task.FromResult(dto);
        }
    }
}
