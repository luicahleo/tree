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

    public class PutRol : PutObjectService<RolDTO, RolEntity, RolDTOMapper>
    {
        private readonly GetDependencies<RolDTO, RolEntity> _getDependency;
        private readonly GetDependencies<ProfileDTO, ProfileEntity> _getProfileDependency;

        public PutRol(PutDependencies<RolEntity> putDependency,
            GetDependencies<RolDTO, RolEntity> getDependency,
            GetDependencies<ProfileDTO, ProfileEntity> getProfileDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new RolValidation())
        {
            _getDependency = getDependency;
            _getProfileDependency = getProfileDependency;
        }
        public override async Task<Result<RolEntity>> ValidateEntity(RolDTO oEntidad, int clientID, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<RolEntity>> listRol;
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

            RolEntity? taxpayerType = await _getDependency.GetItemByCode(code, clientID);
            if (taxpayerType == null)
            {
                lErrors.Add(Error.Create(_traduccion.Rol + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
                return Result.Failure<RolEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            RolEntity banco = RolEntity.Create(clientID, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, linkedProfiles);
            RolEntity bancoFinal = RolEntity.UpdateId(banco, (int)taxpayerType.RolID);

            filter = new Filter(nameof(RolDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            listRol = _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listRol.Result != null && listRol.Result.ListOrEmpty().Count > 0 &&
                listRol.Result.ListOrEmpty()[0].RolID != bancoFinal.RolID)
            {
                lErrors.Add(Error.Create(_traduccion.Rol + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<RolEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            return bancoFinal;
        }
    }
}
