using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{

    public class DeleteCompanyType: DeleteObjectService<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper>
    {
        GetDependencies<CompanyTypeDTO, CompanyTypeEntity> _getDependencies;
        public DeleteCompanyType(DeleteDependencies<CompanyTypeEntity> dependencies, GetDependencies<CompanyTypeDTO, CompanyTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<CompanyTypeDTO>> Delete(string sCode, int Client)
        {
            var CompanyTypeIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<CompanyTypeEntity> companyType = (CompanyTypeIdentty == null || CompanyTypeIdentty.EntidadTipo == null ?
                Result.Failure<CompanyTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : CompanyTypeIdentty);
            if (companyType.Success)
            {
                if (companyType.Valor.Defecto)
                {
                    return Result.Failure<CompanyTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(companyType.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CompanyTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await companyType.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
