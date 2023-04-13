using System;
using TreeCore.BackEnd.Model.Entity.Companies;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class BankAccountEntity : BaseEntity
    {
        public readonly int? EntidadCuentaBancariaID;
        public CompanyEntity? Company;
        public BankEntity Bank;
        public readonly string Codigo;
        public readonly string IBAN;
        public readonly string Descripcion;
        public readonly string SWIFT;
        
        public BankAccountEntity(int? EntidadCuentaBancariaID, string Codigo, string IBAN, string Descripcion, string SWIFT, CompanyEntity? Company, BankEntity Bank)
        {
            this.EntidadCuentaBancariaID = EntidadCuentaBancariaID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.IBAN = IBAN ?? throw new ArgumentNullException(nameof(IBAN));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.SWIFT = SWIFT ?? throw new ArgumentNullException(nameof(SWIFT));
            this.Company = Company;
            this.Bank = Bank ?? throw new ArgumentNullException(nameof(Bank));
        }
        protected BankAccountEntity()
        {

        }

        public static BankAccountEntity Create(int EntidadCuentaBancariaID, string Codigo, string IBAN, string Descripcion, string SWIFT, CompanyEntity Company, BankEntity Bank)
            => new BankAccountEntity(EntidadCuentaBancariaID, Codigo, IBAN, Descripcion,  SWIFT, Company, Bank);
        public static BankAccountEntity UpdateId(BankAccountEntity company, int id) =>
            new BankAccountEntity(id, company.Codigo, company.IBAN, company.Descripcion, company.SWIFT, company.Company, company.Bank);
    }
}
