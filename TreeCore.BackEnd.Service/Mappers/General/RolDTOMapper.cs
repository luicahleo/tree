using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class RolDTOMapper : BaseMapper<RolDTO, RolEntity>
    {
        public override Task<RolDTO> Map(RolEntity oEntity)
        {
            RolDTO dto = new RolDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Name = oEntity.Nombre,
                Description = oEntity.Descripcion,
                Profiles = new List<string>()
            };
            foreach (var profile in oEntity.Profiles)
            {
                dto.Profiles.Add(profile.Perfil_esES);
            }
            return Task.FromResult(dto);
        }
    }
    
}
