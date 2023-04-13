using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General.Inflation
{
    public class PutInflation : PutObjectService<InflationDTO, InflationEntity, InflationDTOMapper>
    {
        private readonly GetDependencies<InflationDTO, InflationEntity> _getDependency;
        private readonly GetDependencies<CountryDTO, CountryEntity> _getCountryDependency;

        public PutInflation(PutDependencies<InflationEntity> putDependency, GetDependencies<InflationDTO, InflationEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor, GetDependencies<CountryDTO, CountryEntity> getCountryDependency) :
            base(httpcontextAccessor, putDependency, new InflationValidation())
        {
            _getDependency = getDependency;
            _getCountryDependency = getCountryDependency;
        }

        public override async Task<Result<InflationEntity>> ValidateEntity(InflationDTO Inflation, int client, string code, string email)
        {
            Task<IEnumerable<InflationEntity>> listInflation;
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            CountryEntity country = await _getCountryDependency.GetItemByCode(Inflation.CountryName, client);
            if (country == null)
            {
                return Result.Failure<InflationEntity>(_traduccion.NameCountry + " " + $"{Inflation.CountryName}" + " " + _errorTraduccion.NotFound + ".");
            }

            InflationEntity? cur = await _getDependency.GetItemByCode(code, client);
            InflationEntity inflationFinal = new InflationEntity(cur.InflacionID, Inflation.Name, Inflation.Code, Inflation.Description, Inflation.Estandar, Inflation.Active, country);

            filter = new Filter(nameof(InflationDTO.Code), Operators.eq, Inflation.Code);
            listFilters.Add(filter);

            listInflation = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listInflation.Result != null && listInflation.Result.ListOrEmpty().Count > 0 &&
                listInflation.Result.ListOrEmpty()[0].InflacionID != inflationFinal.InflacionID)
            {
                return Result.Failure<InflationEntity>(_traduccion.Inflation + " " + $"{Inflation.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            return inflationFinal;
        }
    }
}
