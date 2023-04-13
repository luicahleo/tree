using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.BackEnd.Service.Mappers.Companies
{
    public class CompanyAddressDTOMapper : BaseMapper<CompanyAddressDTO, CompanyAddressEntity>
    {
        public override Task<CompanyAddressDTO> Map(CompanyAddressEntity companyAddress)
        {
            AddressDTO direccion = JsonSerializer.Deserialize<AddressDTO>(companyAddress.DireccionJSON);
            CompanyAddressDTO dto = new CompanyAddressDTO()
            {
                Code = companyAddress.Codigo,
                Default = companyAddress.Defecto,
                Name = companyAddress.EntidadDireccion,
                Address = direccion
            };
            return Task.FromResult(dto);
        }
    }
}
