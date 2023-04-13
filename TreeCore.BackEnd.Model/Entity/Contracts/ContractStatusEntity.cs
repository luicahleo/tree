using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractStatusEntity : BaseEntity
    {
        public readonly int? AlquilerEstadoID;
        public readonly int? ClienteID;
        public readonly string codigo;
        public readonly string Estado;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;
        public readonly bool UsoPagar;



        public ContractStatusEntity(int? AlquilerEstadoID, int? clienteID, string Codigo, string Estado, string Descripcion, bool Activo, bool Defecto
                                  ,bool  UsoPagar)
        {
            this.AlquilerEstadoID = AlquilerEstadoID;
            this.Estado = Estado;
            this.UsoPagar = UsoPagar;
            this.Activo = Activo;
            this.Defecto = Defecto;
            this.ClienteID = clienteID ?? throw new ArgumentNullException(nameof(clienteID));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
          
        }
        protected ContractStatusEntity()
        {
           
        }


        public static ContractStatusEntity Create(int id, int clienteID, string Codigo, string Estado, string Descripcion,
            bool Activo, bool Defecto, bool UsoPagar)
            => new ContractStatusEntity(id, clienteID, Codigo, Estado, Descripcion, Activo, Defecto, UsoPagar);
        public static ContractStatusEntity UpdateId(ContractStatusEntity ContractState, int id) =>
            new ContractStatusEntity(id, ContractState.ClienteID, ContractState.codigo, ContractState.Estado, ContractState.Descripcion, ContractState.Activo, ContractState.Defecto, ContractState.UsoPagar
                                  );

    }
}
