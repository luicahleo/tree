using System;

namespace TreeCore.BackEnd.Model.Entity.ImportExport
{
    public class ImportTypeEntity : BaseEntity
    {
        public readonly int? DocumentoCargaPlantillaID;
        public readonly string DocumentoCargaPlantilla;
        public readonly int ProyectoTipoID;
        public readonly DateTime FechaSubida;
        public readonly string RutaDocumento;
        public readonly bool Activo;

        public ImportTypeEntity(int? documentoCargaPlantillaID, string documentoCargaPlantilla, int proyectoTipoID, DateTime fechaSubida, string rutaDocumento, bool activo)
        {
            DocumentoCargaPlantillaID = documentoCargaPlantillaID;
            DocumentoCargaPlantilla = documentoCargaPlantilla;
            ProyectoTipoID = proyectoTipoID;
            FechaSubida = fechaSubida;
            RutaDocumento = rutaDocumento;
            Activo = activo;
        }

        protected ImportTypeEntity() { }

        public static ImportTypeEntity UpdateId(ImportTypeEntity type, int id) =>
            new ImportTypeEntity(id, type.DocumentoCargaPlantilla, type.ProyectoTipoID, type.FechaSubida, type.RutaDocumento, type.Activo);
    }
}