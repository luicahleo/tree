using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class BankEntity : BaseEntity
    {
        public readonly int? BancoID;
        public readonly int? ClienteID;
        public readonly string CodigoBanco;
        public readonly string Banco;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public BankEntity(int? bancoID, int? clienteID, string CodigoBanco, string Banco, string Descripcion, bool Activo, bool Defecto)
        {
            this.BancoID = bancoID;
            this.ClienteID = clienteID;
            this.CodigoBanco = CodigoBanco ?? throw new ArgumentNullException(nameof(CodigoBanco));
            this.Banco = Banco ?? throw new ArgumentNullException(nameof(Banco));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        protected BankEntity()
        {
            
        }

        public static BankEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new BankEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);
        public static BankEntity UpdateId(BankEntity banco, int id) =>
            new BankEntity(id, banco.ClienteID, banco.CodigoBanco, banco.Banco, banco.Descripcion, banco.Activo, banco.Defecto);
    }
}
