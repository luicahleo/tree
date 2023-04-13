using System;

namespace TreeCore.BackEnd.Model.Entity.ImportExport
{
    public class ImportTaskEntity : BaseEntity
    {
        public readonly int? DocumentoCargaID;
        public readonly int? ClienteID;
        public readonly int? UsuarioID;
        public readonly string DocumentoCarga;
        public readonly string RutaDocumento;
        public readonly string RutaLog;
        public readonly DateTime FechaSubida;
        public readonly DateTime FechaEstimadaSubida;
        public readonly bool Procesado;
        public readonly bool Activo;
        public readonly bool Exito;
        public ImportTypeEntity DocumentosCargasPlantillas;

        public ImportTaskEntity(int? documentoCargaID, int? clienteID, string documentoCarga, string rutaDocumento, 
            DateTime fechaSubida, DateTime fechaestimadaCarga, bool procesado, bool activo, bool exito, 
            ImportTypeEntity documentosCargasPlantillas, string rutaLog, int? usuarioID)
        {
            DocumentoCargaID = documentoCargaID;
            ClienteID = clienteID;
            UsuarioID = usuarioID;
            DocumentoCarga = documentoCarga;
            RutaDocumento = rutaDocumento;
            FechaSubida = fechaSubida;
            FechaEstimadaSubida = fechaestimadaCarga;
            Procesado = procesado;
            Activo = true;
            Exito = exito;
            DocumentosCargasPlantillas = documentosCargasPlantillas;
            RutaLog = rutaLog;
        }

        protected ImportTaskEntity() { }

        public static ImportTaskEntity UpdateId(ImportTaskEntity task, int id) =>
            new ImportTaskEntity(id, task.ClienteID, task.DocumentoCarga, task.RutaDocumento, task.FechaSubida, task.FechaEstimadaSubida, 
                task.Procesado, task.Activo, task.Exito, task.DocumentosCargasPlantillas, task.RutaLog, task.UsuarioID);
        public static ImportTaskEntity Create(int client, string documentoCarga, string rutaDocumento, DateTime fechaSubida, DateTime fechaestimadaCarga, 
            ImportTypeEntity documentosCargasPlantillas, int? usuarioID) =>
            new ImportTaskEntity(null, client, documentoCarga, rutaDocumento, fechaSubida, fechaestimadaCarga, false, true, false, documentosCargasPlantillas, "", usuarioID);
    }
}