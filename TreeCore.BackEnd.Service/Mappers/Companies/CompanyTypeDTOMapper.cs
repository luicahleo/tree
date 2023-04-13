using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.Shared.DTO.Companies;

namespace TreeCore.BackEnd.Service.Mappers.Companies
{
    public class CompanyTypeDTOMapper : BaseMapper<CompanyTypeDTO, CompanyTypeEntity>
    {
        public override Task<CompanyTypeDTO> Map(CompanyTypeEntity companyType)
        {
            CompanyTypeDTO dto = new CompanyTypeDTO()
            {
                Active = companyType.Activo,
                Code = companyType.Codigo,
                Default = companyType.Defecto,
                Description = companyType.Descripcion,
                Name = companyType.EntidadTipo
            };
            return Task.FromResult(dto);
        }
    }
}
