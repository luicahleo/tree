using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.Contracts;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts.ContractLineTaxes
{
    public class PostContractLineTaxes : PostObjectService<ContractLineTaxesDTO, ContractLineTaxesEntity, ContractLineTaxesDTOMapper>
    {

        private readonly GetDependencies<ContractLineTaxesDTO, ContractLineTaxesEntity> _getDependency;
        private readonly GetDependencies<ContractLineDTO, ContractLineEntity> _getDependencyContractLine;
     
        private readonly GetDependencies<TaxesDTO, TaxesEntity> _getDependencyTaxes;
        public PostContractLineTaxes(PostDependencies<ContractLineTaxesEntity> postDependency, GetDependencies<ContractLineTaxesDTO, ContractLineTaxesEntity> getDependency,
            GetDependencies<TaxesDTO, TaxesEntity> getDependencyTaxes, GetDependencies<ContractLineDTO, ContractLineEntity> getDependencyContractLine,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ContractLineTaxesValidation())
        {
            _getDependency = getDependency;
           
            _getDependencyTaxes = getDependencyTaxes;
            _getDependencyContractLine = getDependencyContractLine;
        }

        public override async Task<Result<ContractLineTaxesEntity>> ValidateEntity(ContractLineTaxesDTO ocontractlinetaxesDTO, int client, string email, string code = "")
        {
        
            TaxesEntity tax = await _getDependencyTaxes.GetItemByCode(ocontractlinetaxesDTO.TaxCode, client);
            ContractLineEntity  contractline = await _getDependencyContractLine.GetItemByCode(code, client);
            ContractLineTaxesEntity contractLineTaxesEntity = new ContractLineTaxesEntity(null, contractline, tax,tax.Valor);
            
            return contractLineTaxesEntity;
        }

        public  async Task<Result<List<ContractLineTaxesEntity>>> ValidateEntity(List<ContractLineTaxesDTO> listcontractlinetaxesDTO, int client, string code )
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<ContractLineTaxesEntity> listcontractLineTaxesEntity = new List<ContractLineTaxesEntity>();

            foreach (ContractLineTaxesDTO elemento in listcontractlinetaxesDTO)
            {
                if (controlRepetido(listcontractlinetaxesDTO, elemento))
                {

                    lErrors.Add(Error.Create(_traduccion.CodeContractLineTaxes + " " + $"{elemento.TaxCode}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                TaxesEntity tax = await _getDependencyTaxes.GetItemByCode(elemento.TaxCode, client);
                
                if (tax == null) { lErrors.Add(Error.Create(_traduccion.CodeContractLineTaxes + " " + $"{elemento.TaxCode}" + " " + _errorTraduccion.NotFound + ".")); }
               
                
                if(tax!=null )
                {
                    double? Cantidad = tax.Valor;
                    listcontractLineTaxesEntity.Add(new ContractLineTaxesEntity(null, null, tax, Cantidad));
                }
               
                

            }
            if(lErrors.Count>0)
                {
                return Result.Failure<List<ContractLineTaxesEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return listcontractLineTaxesEntity;
            }

        }

        private bool controlRepetido(List<ContractLineTaxesDTO> lista, ContractLineTaxesDTO elemento)
        {
            int cont = 0;
            foreach (ContractLineTaxesDTO item in lista)
            {
                if ( elemento.TaxCode==item.TaxCode)
                {
                    cont++;
                }
            }
            if (cont > 1)
            {
                return true;
            }
            return false;
        }


    }
}
