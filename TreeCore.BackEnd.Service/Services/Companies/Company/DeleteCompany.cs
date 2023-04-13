using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{
    public class DeleteCompany : DeleteObjectService<CompanyDTO, CompanyEntity, CompanyDTOMapper>
    {
        GetDependencies<CompanyDTO, CompanyEntity> _getDependencies;
        public DeleteCompany(DeleteDependencies<CompanyEntity> dependencies, GetDependencies<CompanyDTO, CompanyEntity> getDependencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDependencies;
        }


        public override async Task<Result<CompanyDTO>> Delete(string code, int client)
        {
            var CompanyIdentty = await _getDependencies.GetItemByCode(code, client);
            Result<CompanyEntity> Company = (CompanyIdentty == null || CompanyIdentty.EntidadID == null ?
                Result.Failure<CompanyEntity>(Error.Create(_errorTraduccion.NotFound))
                : CompanyIdentty);
            if (Company.Success)
            {
                var iResult = await DeleteItem(Company.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CompanyDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await Company.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}

