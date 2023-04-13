using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractTypeEntity : BaseEntity
    {
        public readonly int? AlquilerTipoContratoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string TipoContrato;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public ContractTypeEntity(int? AlquilerTipoContratoID, int? clienteID, string Codigo, string TipoContrato, string Descripcion, bool Activo, bool Defecto)
        {
            this.AlquilerTipoContratoID = AlquilerTipoContratoID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.TipoContrato = TipoContrato ?? throw new ArgumentNullException(nameof(TipoContrato));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        protected ContractTypeEntity()
        {
            //AlquilerTipoContratoID = AlquilerTipoContratoID;
            //ClienteID = ClienteID;
            //Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            //TipoContrato = TipoContrato ?? throw new ArgumentNullException(nameof(TipoContrato));
            //Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            //Activo = Activo;
            //Defecto = Defecto;
        }

        public static ContractTypeEntity Create(int id, int clienteID, string codigo, string tipoContrato, string descripcion,
            bool activo, bool defecto)
            => new ContractTypeEntity(id, clienteID, codigo, tipoContrato, descripcion, activo, defecto);
        public static ContractTypeEntity UpdateId(ContractTypeEntity ContractType, int id) =>
            new ContractTypeEntity(id, ContractType.ClienteID, ContractType.Codigo, ContractType.TipoContrato, ContractType.Descripcion, ContractType.Activo, ContractType.Defecto);
    }
}
