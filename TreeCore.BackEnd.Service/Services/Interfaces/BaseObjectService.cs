using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Translations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;

namespace TreeCore.BackEnd.Service.Services
{
    public abstract class BaseObjectService<DTO, Entity, Mapper>
        where Entity: class
        where Mapper: BaseMapper<DTO, Entity>
    {
        protected readonly GeneralTranslations _traduccion;
        protected readonly ErrorTranslations _errorTraduccion;
        protected readonly BasicDependence<Entity> _basicDependence;
        protected readonly BaseMapper<DTO, Entity> _mapper;

        protected BaseObjectService(IHttpContextAccessor httpcontextAccessor, BasicDependence<Entity> basicDependence) {
            _traduccion = new GeneralTranslations(httpcontextAccessor.HttpContext.Request.Headers.GetCultureInfo());
            _errorTraduccion = new ErrorTranslations(httpcontextAccessor.HttpContext.Request.Headers.GetCultureInfo());
            _basicDependence = basicDependence;
            _mapper = (BaseMapper<DTO, Entity>)Activator.CreateInstance(typeof(Mapper));
        }
        protected async Task<Result<Entity>> CommitTransaction(Entity oEntity)
        {
            await _basicDependence.CommitTransaction();
            return oEntity;
        }
    }
}
