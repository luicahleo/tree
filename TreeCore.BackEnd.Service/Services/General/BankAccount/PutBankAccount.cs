using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class PutBankAccount : PutObjectService<BankAccountDTO, BankAccountEntity, BankAccountDTOMapper>
    {
        private readonly GetDependencies<BankAccountDTO, BankAccountEntity> _getDependency;
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getCompanyDependency;
        private readonly GetDependencies<BankDTO, BankEntity> _getBankDependency;

        public PutBankAccount(
            PutDependencies<BankAccountEntity> putDependency,
            GetDependencies<BankAccountDTO, BankAccountEntity> getDependency,
            GetDependencies<CompanyDTO, CompanyEntity> getCompanyDependency,
            GetDependencies<BankDTO, BankEntity> getBankDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new BankAccountValidation())
        {
            _getDependency = getDependency;
            _getCompanyDependency = getCompanyDependency;
            _getBankDependency = getBankDependency;
        }

        public override async Task<Result<BankAccountEntity>> ValidateEntity(BankAccountDTO bankAccount, int clientID, string email, string code = "")
        {
            
            return null;
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

