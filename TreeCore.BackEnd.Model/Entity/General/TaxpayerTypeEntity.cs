using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class TaxpayerTypeEntity : BaseEntity
    {
        public readonly int? TipoContribuyenteID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string TipoContribuyente;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public TaxpayerTypeEntity(int? tipoContribuyenteID, int? clienteID, string Codigo, string TipoContribuyente, string Descripcion, bool Activo, bool Defecto)
        {
            this.TipoContribuyenteID = tipoContribuyenteID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.TipoContribuyente = TipoContribuyente ?? throw new ArgumentNullException(nameof(TipoContribuyente));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        public TaxpayerTypeEntity()
        {
            
        }

        public static TaxpayerTypeEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new TaxpayerTypeEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);
        public static TaxpayerTypeEntity UpdateId(TaxpayerTypeEntity TipoContribuyenteID, int id) =>
            new TaxpayerTypeEntity(id, TipoContribuyenteID.ClienteID, TipoContribuyenteID.Codigo, TipoContribuyenteID.TipoContribuyente, TipoContribuyenteID.Descripcion, TipoContribuyenteID.Activo, TipoContribuyenteID.Defecto);
    }
}
