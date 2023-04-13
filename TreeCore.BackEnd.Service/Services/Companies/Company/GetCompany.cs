using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{
    public class GetCompany : GetObjectService<CompanyDTO, CompanyEntity, CompanyDTOMapper>
    {
        GetDependencies<CompanyDetailsDTO, CompanyEntity> _getDetailsDependencies;
        public GetCompany(GetDependencies<CompanyDTO, CompanyEntity> getDependencies, GetDependencies<CompanyDetailsDTO, CompanyEntity> getDetailsDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            _getDetailsDependencies = getDetailsDependencies;
        }
        public GetCompany(GetDependencies<CompanyDTO, CompanyEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            
        }

        private Task<CompanyDetailsDTO> MapDetails(CompanyEntity company)
        {
            CompanyDetailsDTO dto = new CompanyDetailsDTO()
            {
                Active = company.Activo,
                Code = company.Codigo,
                Name = company.Nombre,
                Alias = company.Alias,
                Email = company.Email,
                Phone = company.Telefono,
                CompanyTypeCode = company.EntidadesTipos.Codigo,
                CompanyTypeName = company.EntidadesTipos.EntidadTipo,
                CompanyTypeDescription = company.EntidadesTipos.Descripcion,
                TaxpayerTypeCode = (company.TiposContribuyentes != null) ? company.TiposContribuyentes.Codigo:"",
                TaxpayerTypeName = (company.TiposContribuyentes != null) ? company.TiposContribuyentes.TipoContribuyente:"",
                TaxpayerTypeDescription = (company.TiposContribuyentes != null) ? company.TiposContribuyentes.Descripcion:""
            };
            return Task.FromResult(dto);
        }

        public async Task<Result<CompanyDetailsDTO>> GetItemDetailsByCode(string code, int Client)
        {
            var CompanyIdentty = await _getDetailsDependencies.GetItemByCode(code, Client);
            Result<CompanyEntity> Company = (CompanyIdentty == null || CompanyIdentty.EntidadID == null ?
                Result.Failure<CompanyEntity>(Error.Create(_errorTraduccion.NotFound))
                : CompanyIdentty);

            return await Company.Async()
                .MapAsync(x => MapDetails(x));
        }

    }
}
