using System;
namespace TreeCore.BackEnd.Model.Entity.General
{
    public class TaxesEntity : BaseEntity
    {
        public readonly int? ImpuestoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Impuesto;
        public readonly string Descripcion;
        public DateTime FechaActualizacion;
        public readonly int? Valor;
        public CountryEntity Paises;
        public readonly bool Activo;
        public readonly bool Defecto;



        public TaxesEntity(int? impuestoID, int? clienteID, string codigo, string impuesto, string descripcion, DateTime fechaActualizacion, int? valor, CountryEntity paises, bool activo, bool defecto)
        {
            ImpuestoID = impuestoID;
            ClienteID = clienteID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Impuesto = impuesto ?? throw new ArgumentNullException(nameof(impuesto));
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            FechaActualizacion = fechaActualizacion;
            Valor = valor;
            Paises = paises;
            Activo = activo;
            Defecto = defecto;

        }

        protected TaxesEntity()
        {
        }

        public static TaxesEntity Create(int id, int clienteID, string codigo, string tipoContrato, string descripcion, DateTime FechaActualizacion, int valor, CountryEntity paises,
            bool activo, bool defecto)
            => new TaxesEntity(id, clienteID, codigo, tipoContrato, descripcion, FechaActualizacion, valor, paises, activo, defecto);

        public static TaxesEntity UpdateId(TaxesEntity taxes, int id) =>
            new TaxesEntity(id, taxes.ClienteID, taxes.Codigo, taxes.Impuesto, taxes.Descripcion, taxes.FechaActualizacion, taxes.Valor, taxes.Paises, taxes.Activo, taxes.Defecto);
    }
}
