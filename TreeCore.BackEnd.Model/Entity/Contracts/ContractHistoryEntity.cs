using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractHistoryEntity : BaseEntity
    {
        public readonly int? AlquilerHistoricoCompletoID;
        public readonly int ? AlquilerID;
        public readonly bool Activo;
        public readonly DateTime FechaCreacion;
        public readonly string ViaModificacion;
        public readonly bool ObtenerDatosHistoricos;
        public readonly bool VersionXML;
        public readonly string Datos;
        public readonly int? CreadorID;




        public ContractHistoryEntity(int? AlquilerHistoricoCompletoID, int? AlquilerID, bool Activo, DateTime FechaCreacion, string ViaModificacion,
            bool ObtenerDatosHistoricos, bool VersionXml, string Datos, int? CreadorID)
                                  
        {
            this.AlquilerHistoricoCompletoID = AlquilerHistoricoCompletoID;
            this.AlquilerID = AlquilerID;
            this.Activo = Activo;
            this.FechaCreacion = FechaCreacion;
            this.ViaModificacion = ViaModificacion;
            this.ObtenerDatosHistoricos = ObtenerDatosHistoricos;
            this.ObtenerDatosHistoricos = ObtenerDatosHistoricos;
            this.VersionXML = VersionXml;
            this.Datos = Datos;
            this.CreadorID = CreadorID;

        }
        protected ContractHistoryEntity()
        {

        }


        public static ContractHistoryEntity Create(int? AlquilerHistoricoCompletoID, int? AlquilerID, bool Activo, DateTime FechaCreacion, string ViaModificacion,
            bool ObtenerDatosHistoricos, bool VersionXml, string Datos, int? CreadorID)
            => new ContractHistoryEntity(AlquilerHistoricoCompletoID, AlquilerID, Activo, FechaCreacion, ViaModificacion, ObtenerDatosHistoricos, VersionXml, Datos, CreadorID);
        public static ContractHistoryEntity UpdateId(ContractHistoryEntity ContractHistory, int id) =>
            new ContractHistoryEntity(id, ContractHistory.AlquilerID, ContractHistory.Activo,
                ContractHistory.FechaCreacion, ContractHistory.ViaModificacion,
                ContractHistory.ObtenerDatosHistoricos, ContractHistory.VersionXML, 
                ContractHistory.Datos,ContractHistory.CreadorID);

    }
}

