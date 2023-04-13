using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    public class PostRol : PostObjectService<RolDTO, RolEntity, RolDTOMapper>
    {

        private readonly GetDependencies<RolDTO, RolEntity> _getDependency;
        private readonly PutDependencies<RolEntity> _putDependency;
        private readonly GetDependencies<ProfileDTO, ProfileEntity> _getProfileDependency;

        public PostRol(PostDependencies<RolEntity> postDependency,
            GetDependencies<RolDTO, RolEntity> getDependency,
            GetDependencies<ProfileDTO, ProfileEntity> getProfileDependency,
            PutDependencies<RolEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new RolValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getProfileDependency = getProfileDependency;
        }

        public override async Task<Result<RolEntity>> ValidateEntity(RolDTO oEntidad, int clientID, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            #region Profiles

            foreach (var grouping in oEntidad.Profiles.GroupBy(t => t).Where(t => t.Count() != 1))
            {
                lErrors.Add(Error.Create($"{nameof(ProfileDTO.Code)} '" + grouping.Key + $"' {_errorTraduccion.AlreadyExist}.", null));
            }

            IEnumerable<ProfileEntity> linkedProfiles = null;

            if (oEntidad.Profiles.Count > 0)
            {
                List<Filter> filters = new List<Filter>();
                List<Filter> filtersCodes = new List<Filter>();
                foreach (string profile in oEntidad.Profiles)
                {
                    filtersCodes.Add(new Filter(nameof(ProfileDTO.Code).ToLower(), Operators.eq, profile));
                }
                filters.Add(new Filter(nameof(ProfileDTO.Active), Operators.eq, "true"));
                filters.Add(new Filter(Filter.Types.OR, filtersCodes));

                linkedProfiles = await _getProfileDependency.GetList(clientID, filters, null, null, -1, -1);
                IEnumerable<string> iEcodes = linkedProfiles.Select(lp => lp.Perfil_esES);
                IEnumerable<string> lp = oEntidad.Profiles;

                IEnumerable<string> intersect = iEcodes.Union(lp).Except(iEcodes.Intersect(lp));

                if (intersect.Count() > 0)
                {
                    foreach (string scode in intersect.ToList())
                    {
                        lErrors.Add(Error.Create($"{nameof(ProfileDTO.Code)} '" + scode + $"' {_errorTraduccion.NotFound}.", null));
                    };
                }
            }

            #endregion

            RolEntity banco = RolEntity.Create(clientID, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, linkedProfiles);

            filter = new Filter(nameof(RolDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            Task<IEnumerable<RolEntity>> listRol = _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listRol.Result != null && listRol.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Rol + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<RolEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            return banco;
        }
    }
}