using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class PostBankAccount : PostObjectService<BankAccountDTO, BankAccountEntity, BankAccountDTOMapper>
    {
        private readonly GetDependencies<BankAccountDTO, BankAccountEntity> _getDependency;
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getCompanyDependency;
        private readonly GetDependencies<BankDTO, BankEntity> _getBankDependency;

        public PostBankAccount(PostDependencies<BankAccountEntity> postDependency,
                GetDependencies<BankAccountDTO, BankAccountEntity> getDependency,
                GetDependencies<CompanyDTO, CompanyEntity> getCompanyDependency,
                GetDependencies<BankDTO, BankEntity> getBankDependency,
        IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new BankAccountValidation())
        {
            _getDependency = getDependency;
            _getCompanyDependency = getCompanyDependency;
            _getBankDependency = getBankDependency;
        }

        public override async Task<Result<BankAccountEntity>> ValidateEntity(BankAccountDTO bankAccount, int clientID, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            BankEntity bank = await _getBankDependency.GetItemByCode(bankAccount.BankCode, clientID);
            if (bank == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeBank + " " + $"{bankAccount.BankCode}" + " " + _errorTraduccion.NotFound + "."));
            }


            // BankAccountEntity companyEntity = new BankAccountEntity(null, bankAccount.Code, bankAccount.IBAN, bankAccount.Description, bankAccount.SWIFT, company, bank);
            filter = new Filter(nameof(BankAccountDTO.Code), Operators.eq, bankAccount.Code);
            listFilters.Add(filter);

            Task<IEnumerable<BankAccountEntity>> listBankAccounts = _getDependency.GetList(clientID, listFilters, null, null, -1, -1);
            if (listBankAccounts.Result != null && listBankAccounts.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeBankAccount + " " + $"{bankAccount.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<BankAccountEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return null;
                //return companyEntity;
            }
        }

        public async Task<Result<List<BankAccountEntity>>> ValidateEntity(List<BankAccountDTO> linkedbankAccount, int clientID, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            List<BankAccountEntity> linkedBankAccountEntity = new List<BankAccountEntity>();

            foreach (BankAccountDTO bankAccount in linkedbankAccount)
            {
                if (controlRepetido(linkedbankAccount, bankAccount))
                {
                    lErrors.Add(Error.Create(_traduccion.CodeBankAccount + " " + $"{bankAccount.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }


                BankEntity bankEntity = await _getBankDependency.GetItemByCode(bankAccount.BankCode, clientID);
                if (bankEntity == null)
                {
                    lErrors.Add(Error.Create(_traduccion.CodeBank + " " + $"{bankAccount.BankCode}" + " " + _errorTraduccion.NotFound + "."));
                }
                else
                {
                    linkedBankAccountEntity.Add(new BankAccountEntity(null, bankAccount.Code, bankAccount.IBAN, bankAccount.Description, bankAccount.SWIFT, null, bankEntity));
                }

            };


            if (lErrors.Count > 0)
            {
                return Result.Failure<List<BankAccountEntity>>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return linkedBankAccountEntity;
            }
        }

        private bool controlRepetido(List<BankAccountDTO> lista, BankAccountDTO elemento)
        {
            int cont = 0;
            foreach (BankAccountDTO bankAccount in lista)
            {
                if (elemento.Code == bankAccount.Code)
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

