using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class TaxIdentificationNumberCategoryEntity : BaseEntity
    {
        public readonly int? SAPTipoNIFID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public TaxIdentificationNumberCategoryEntity(int? SAPTipoNIFID, int? clienteID, string Codigo, string Nombre, string Descripcion, bool Activo, bool Defecto)
        {
            this.SAPTipoNIFID = SAPTipoNIFID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.Nombre = Nombre ?? throw new ArgumentNullException(nameof(Nombre));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        public TaxIdentificationNumberCategoryEntity()
        {
            
        }

        public static TaxIdentificationNumberCategoryEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new TaxIdentificationNumberCategoryEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);
        public static TaxIdentificationNumberCategoryEntity UpdateId(TaxIdentificationNumberCategoryEntity SAPTipoNIF, int id) =>
            new TaxIdentificationNumberCategoryEntity(id, SAPTipoNIF.ClienteID, SAPTipoNIF.Codigo, SAPTipoNIF.Nombre, SAPTipoNIF.Descripcion, SAPTipoNIF.Activo, SAPTipoNIF.Defecto);
    }
}
