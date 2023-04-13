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
    public class PostInflation : PostObjectService<InflationDTO, InflationEntity, InflationDTOMapper>
    {
        private readonly GetDependencies<InflationDTO, InflationEntity> _getDependency;
        private readonly PutDependencies<InflationEntity> _putDependency;

        private readonly GetDependencies<CountryDTO, CountryEntity> _getCountryDependency;

        public PostInflation(PostDependencies<InflationEntity> postDependency, GetDependencies<InflationDTO, InflationEntity> getDependency, PutDependencies<InflationEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor, GetDependencies<CountryDTO, CountryEntity> getCountryDependency) :
            base(httpcontextAccessor, postDependency, new InflationValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;

            _getCountryDependency = getCountryDependency;
        }

        public override async Task<Result<InflationEntity>> ValidateEntity(InflationDTO Inflation, int client, string email, string code = "")
        {
            List<Filter> listFilters = new List<Filter>();
            List<string> listOrders = new List<string>();
            Filter filter;

            CountryEntity country = await _getCountryDependency.GetItemByCode(Inflation.CountryName, client);
            if (country == null)
            {
                return Result.Failure<InflationEntity>(_traduccion.NameCountry + " " + $"{Inflation.CountryName}" + " " + _errorTraduccion.NotFound + ".");
            }

            InflationEntity inflationEntity = new InflationEntity(null, Inflation.Name, Inflation.Code, Inflation.Description, Inflation.Estandar, Inflation.Active, country);

            filter = new Filter(nameof(InflationDTO.Code).ToLower(), Operators.eq, Inflation.Code);
            listFilters.Add(filter);

            Task<IEnumerable<InflationEntity>> listInflation = _getDependency.GetList(client, listFilters, listOrders, null, -1, -1);
            if (listInflation.Result != null && listInflation.Result.ListOrEmpty().Count > 0)
            {
                return Result.Failure<InflationEntity>(_traduccion.Inflation + " " + $"{Inflation.Code}" + " " + _errorTraduccion.AlreadyExist + ".");
            }

            return inflationEntity;
        }
    }
}
