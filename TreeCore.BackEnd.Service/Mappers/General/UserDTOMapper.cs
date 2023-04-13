using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class UserDTOMapper : BaseMapper<UserDTO, UserEntity>
    {
        public override Task<UserDTO> Map(UserEntity user)
        {
            UserDTO dto = new UserDTO()
            {
                Name = user.Nombre,
                Surnames = user.Apellidos,
                Email = user.EMail,
                Password = user.Clave,
                Active = user.Activo,
                DateLastChange = user.FechaUltimoCambio,
                Phone = user.Telefono,
                ExpiryUserDate = user.FechaCaducidadUsuario,
                ExpiryPasswordDate = user.FechaCaducidadClave,
                LastAccessDate = user.FechaUltimoAcceso,
                FullName = user.NombreCompleto,
                CreationDate = user.FechaCreacion,
                DeactivationDate = user.FechaDesactivacion
            };
            return Task.FromResult(dto);
        }
    }
}
