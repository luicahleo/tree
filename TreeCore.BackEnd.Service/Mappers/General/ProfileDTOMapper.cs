using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class ProfileDTOMapper : BaseMapper<ProfileDTO, ProfileEntity>
    {
        public override Task<ProfileDTO> Map(ProfileEntity oEntity)
        {
            ProfileDTO dto = new ProfileDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Perfil_esES,
                Description = oEntity.Descripcion,
                ModuleCode = oEntity.CodigoModulo,
                UserFuntionalities = JsonConvert.DeserializeObject<List<string>>(oEntity.JsonUserFunctionalities)
            };
            return Task.FromResult(dto);
        }
    }
    
}
