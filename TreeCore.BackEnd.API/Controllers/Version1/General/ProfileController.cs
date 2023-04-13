using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// ProfileController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ProfileController : ApiControllerBase<ProfileDTO, ProfileEntity, ProfileDTOMapper>, IDeleteController<ProfileDTO>, IModController<ProfileDTO>
    {

        private readonly PutProfile _putProfile;
        private readonly PostProfile _postProfile;
        private readonly DeleteProfile _deleteProfile;

        public ProfileController(GetObjectService<ProfileDTO, ProfileEntity, ProfileDTOMapper> getObjectService, PutProfile putProfile, PostProfile postProfile, DeleteProfile deleteProfile) 
            : base(getObjectService)
        {
            _putProfile = putProfile;
            _postProfile = postProfile;
            _deleteProfile = deleteProfile;
        }

        /// <summary>
        /// Post Profile
        /// </summary>
        /// <returns>List of Profile</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<ProfileDTO>> Post(ProfileDTO Profile)
        {
            return (await _postProfile.Create(Profile, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Profile
        /// </summary>
        /// <param name="code">Code of Profile</param>
        /// <returns>List of Profile</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ProfileDTO>> Put(ProfileDTO ProfileDTO, string code)
        {
            return (await _putProfile.Update(ProfileDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Profile
        /// </summary>
        /// <param name="code">Code of Profile</param>
        /// <returns>Profile</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ProfileDTO>> Delete(string code)
        {
            return (await _deleteProfile.Delete(code, Client)).MapDto(x => x);
        }

    }
}
