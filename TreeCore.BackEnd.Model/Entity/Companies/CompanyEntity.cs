using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.ValueObject;

namespace TreeCore.BackEnd.Model.Entity.Companies
{
    public class CompanyEntity : BaseEntity
    {
        public readonly int? EntidadID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Alias;
        public readonly string Email;
        public readonly string Telefono;
        public readonly bool Activo;
        public readonly bool EsPropietario;
        public readonly bool EsBeneficiario;
        public readonly bool EsProveedor;
        public readonly bool EsOperador;
        public readonly string NumIdentContribuyente;
        public readonly DateTime FechaCreaccion;
        public readonly DateTime FechaUltimaModificacion;
        public UserEntity UsuariosCreadores;
        public UserEntity UsuariosModificadores;
        public CompanyTypeEntity EntidadesTipos;
        public TaxpayerTypeEntity TiposContribuyentes;
        public TaxIdentificationNumberCategoryEntity SAPTipoNIF;
        public PaymentTermEntity CondicionPago;
        public CurrencyEntity Moneda;
        public readonly bool EntidadCliente;
        public IEnumerable<BankAccountEntity> CuentasBancarias;
        public IEnumerable<CompanyAssignedPaymentMethodsEntity> MetodosPagos;
        public IEnumerable<CompanyAddressEntity> Direcciones;




        public CompanyEntity
            (int? companyID, int? clienteID, string Codigo, string Nombre, 
            string Alias, string Email, string Telefono, bool Activo, 
            bool EsPropietario, bool EsBeneficiario, bool EsProveedor, bool EsOperador, string NumIdentContribuyente,
            CompanyTypeEntity CompanyType, TaxpayerTypeEntity? taxpayerType, TaxIdentificationNumberCategoryEntity? SAPTipoNIF, 
            PaymentTermEntity? CondicionPago, CurrencyEntity Moneda, IEnumerable<BankAccountEntity> cuentasBancarias,
            IEnumerable<CompanyAssignedPaymentMethodsEntity> metodosPagos, 
            DateTime FechaCreaccion, DateTime FechaUltimaModificacion, UserEntity UsuarioCreador, UserEntity UsuarioModificador, IEnumerable<CompanyAddressEntity> listAddresses)
        {
            this.EntidadID = companyID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.Nombre = Nombre ?? throw new ArgumentNullException(nameof(Nombre));
            this.Alias = Alias ?? throw new ArgumentNullException(nameof(Alias));
            this.Email = Email ?? throw new ArgumentNullException(nameof(Email));
            this.Telefono = Telefono;
            this.Activo = Activo;
            this.EsPropietario = EsPropietario;
            this.EsBeneficiario = EsBeneficiario;
            this.EsProveedor = EsProveedor;
            this.EsOperador = EsOperador;
            this.NumIdentContribuyente = NumIdentContribuyente;
            this.EntidadesTipos = CompanyType;
            this.TiposContribuyentes = taxpayerType;
            this.SAPTipoNIF = SAPTipoNIF;
            this.CondicionPago = CondicionPago;
            this.Moneda = Moneda;
            this.CuentasBancarias = cuentasBancarias;
            this.MetodosPagos = metodosPagos;
            this.UsuariosCreadores = UsuarioCreador;
            this.UsuariosModificadores = UsuarioModificador;
            this.FechaCreaccion = FechaCreaccion;
            this.FechaUltimaModificacion = FechaUltimaModificacion;
            this.Direcciones = listAddresses;
        }
        protected CompanyEntity()
        {

        }

        public static CompanyEntity Create(int id, int clienteID, string codigo, string nombre, string alias, string email, string telefono,
            bool activo,bool EsPropietario, bool EsBeneficiario, bool EsProveedor, bool EsCliente, string NumIdentContribuyente, CompanyTypeEntity CompanyType, TaxpayerTypeEntity? taxpayerType, TaxIdentificationNumberCategoryEntity? SAPTipoNIF, PaymentTermEntity? CondicionPago, CurrencyEntity Moneda, IEnumerable<BankAccountEntity> cuentaBancarias, IEnumerable<CompanyAssignedPaymentMethodsEntity> metodosPagos, DateTime FechaCreaccion, DateTime FechaUltimaModificacion, UserEntity UsuarioCreador, UserEntity UsuarioModificador, IEnumerable<CompanyAddressEntity> listAddresses)
            => new CompanyEntity(id, clienteID, codigo, nombre, alias, email, telefono, activo, EsPropietario, EsBeneficiario, EsProveedor, EsCliente, NumIdentContribuyente, CompanyType, taxpayerType, SAPTipoNIF, CondicionPago, Moneda, cuentaBancarias, metodosPagos, FechaCreaccion, FechaUltimaModificacion,UsuarioCreador, UsuarioModificador, listAddresses);
        public static CompanyEntity UpdateId(CompanyEntity company, int id) =>
            new CompanyEntity(id, company.ClienteID, company.Codigo, company.Nombre, company.Alias, company.Email, company.Telefono,company.Activo,company.EsPropietario, company.EsBeneficiario, company.EsProveedor, company.EsOperador, company.NumIdentContribuyente, company.EntidadesTipos, company.TiposContribuyentes, company.SAPTipoNIF, company.CondicionPago, company.Moneda, company.CuentasBancarias, company.MetodosPagos, company.FechaCreaccion, company.FechaUltimaModificacion, company.UsuariosCreadores, company.UsuariosModificadores, company.Direcciones);
    }
}
