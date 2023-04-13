using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class PutProfile : PutObjectService<ProfileDTO, ProfileEntity, ProfileDTOMapper>
    {
        private readonly GetDependencies<ProfileDTO, ProfileEntity> _getDependency;

        public PutProfile(PutDependencies<ProfileEntity> putDependency,
            GetDependencies<ProfileDTO, ProfileEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ProfileValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<ProfileEntity>> ValidateEntity(ProfileDTO oEntidad, int clientID, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<ProfileEntity>> listProfile;
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            ProfileEntity? taxpayerType = await _getDependency.GetItemByCode(oEntidad.Code, clientID);
            ProfileEntity banco = ProfileEntity.Create(clientID, oEntidad.Code, oEntidad.Description, oEntidad.Active, oEntidad.ModuleCode, JsonConvert.SerializeObject(oEntidad.UserFuntionalities));
            ProfileEntity bancoFinal = ProfileEntity.UpdateId(banco, (int)taxpayerType.PerfilID);

            filter = new Filter(nameof(ProfileDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            listProfile = _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listProfile.Result != null && listProfile.Result.ListOrEmpty().Count > 0 &&
                listProfile.Result.ListOrEmpty()[0].PerfilID != bancoFinal.PerfilID)
            {
                lErrors.Add(Error.Create(_traduccion.Profile + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<ProfileEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            return bancoFinal;
        }
    }
}
