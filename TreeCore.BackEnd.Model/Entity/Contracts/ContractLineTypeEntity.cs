using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractLineTypeEntity : BaseEntity
    {
        public readonly int? AlquilerConceptoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string AlquilerConcepto;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;
        public readonly bool EsPagoUnico;
        public readonly bool EsAlquilerBase;
        public readonly bool EsCobro;        

        public readonly bool ImplicaDeuda;
        public readonly bool EsFianza;
        public readonly bool EsPagoAdicional;
        public readonly bool EsInfraestructura;
        public readonly bool OpcionCompra;
        public readonly bool OpcionAmortizacion;
        public readonly bool EsEstadistica;
        public readonly bool OpcionValorResidual;
        public readonly bool OpcionSubarrendamiento;
        public readonly bool ObjetivoUnico;
        public readonly bool ContarAlquiler;
        public readonly bool ContarSuministro;
        public readonly bool Torrero;
        public readonly bool Sharing;
        public readonly bool Variable;
        public readonly bool PagoAnticipadoAuto;



        public ContractLineTypeEntity(int? AlquilerConceptoID, int? clienteID, string Codigo, string AlquilerConcepto, string Descripcion, bool Activo, bool Defecto, bool EsPagoUnico, bool EsAlquilerBase, bool EsCobro)
        {
            this.AlquilerConceptoID = AlquilerConceptoID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.AlquilerConcepto = AlquilerConcepto ?? throw new ArgumentNullException(nameof(AlquilerConcepto));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
            this.EsPagoUnico = EsPagoUnico;
            this.EsAlquilerBase = EsAlquilerBase;
            this.EsCobro = EsCobro;
        }

        protected ContractLineTypeEntity()
        {
            
        }

        public static ContractLineTypeEntity Create(int id, int clienteID, string codigo, string tipoContrato, string descripcion,
            bool activo, bool defecto, bool esPagoUnico, bool esAlquilerBase, bool esCobro)
            => new ContractLineTypeEntity(id, clienteID, codigo, tipoContrato, descripcion, activo, defecto, esPagoUnico, esAlquilerBase, esCobro);
        public static ContractLineTypeEntity UpdateId(ContractLineTypeEntity ContractLineType, int id) =>
            new ContractLineTypeEntity(id, ContractLineType.ClienteID, ContractLineType.Codigo, ContractLineType.AlquilerConcepto, ContractLineType.Descripcion, ContractLineType.Activo, ContractLineType.Defecto, ContractLineType.EsPagoUnico, ContractLineType.EsAlquilerBase, ContractLineType.EsCobro);
    }
}
