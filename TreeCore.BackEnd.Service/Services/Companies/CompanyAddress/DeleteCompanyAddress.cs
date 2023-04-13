using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{

    public class DeleteCompanyAddress : DeleteObjectService<CompanyAddressDTO, CompanyAddressEntity, CompanyAddressDTOMapper>
    {
        GetDependencies<CompanyAddressDTO, CompanyAddressEntity> _getDependencies;
        public DeleteCompanyAddress(DeleteDependencies<CompanyAddressEntity> dependencies, GetDependencies<CompanyAddressDTO, CompanyAddressEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<CompanyAddressDTO>> Delete(string sCode, int Client)
        {
            var companyAddressIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<CompanyAddressEntity> companyAddress = (companyAddressIdentty == null || companyAddressIdentty.EntidadDireccion == null ?
                Result.Failure<CompanyAddressEntity>(Error.Create(_errorTraduccion.NotFound))
                : companyAddressIdentty);
            if (companyAddress.Success)
            {
                if (companyAddress.Valor.Defecto)
                {
                    return Result.Failure<CompanyAddressDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(companyAddress.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CompanyAddressDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await companyAddress.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
