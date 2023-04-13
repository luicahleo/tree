using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DocumentosController : GeneralBaseController<Documentos, TreeCoreContext>
    {
        public DocumentosController()
            : base()
        { }

        public ILog log = LogManager.GetLogger("");

        public List<String> ValoresDocumentosTipos()
        {
            List<string> datos = new List<string>();
            datos = (from c in Context.DocumentTipos where c.Activo == true && c.EsCarpeta == false select c.DocumentTipo).ToList();
            datos.Sort();
            return datos;
        }

        public string ObtenerRutaDocumento(long DocumentoTipoID)
        {
            DocumentTiposController cDocumentTipos = new DocumentTiposController();
            string path = "";

            try
            {
                DocumentTipos docTipo = new DocumentTipos();
                docTipo = cDocumentTipos.GetItem(DocumentoTipoID);

                if (docTipo != null)
                {
                    path = TreeCore.DirectoryMapping.GetDocumentDirectory();
                    path = Path.Combine(path, docTipo.DocumentTipo);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                return path;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.Message);
                return null;
            }
        }
        public string ObtenerRutaDocumentoServicio(long DocumentoTipoID, string ruta)
        {
            DocumentTiposController cDocumentTipos = new DocumentTiposController();
            string path = "";

            try
            {
                DocumentTipos docTipo = new DocumentTipos();
                docTipo = cDocumentTipos.GetItem(DocumentoTipoID);

                if (docTipo != null)
                {
                    path = ruta;
                    path = Path.Combine(path, docTipo.DocumentTipo);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                return path;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.Message);
                return null;
            }
        }

        #region DOC PROVEEDORES

        public List<Documentos> GetallItemsProveedores(Int64 proyectoid)
        {
            // Local variable
            List<Documentos> lista = null;

            try
            {
                lista = (from c in Context.Documentos
                         where c.ProyectoID == proyectoid && c.FechaCaducidad < DateTime.Today
                         select c
                         ).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return lista;
        }

        #endregion

        #region DOCUMENTOS USUARIO

        public List<Vw_Documentos> GetDocumentosByUsuarioID(long UsuarioID)
        {
            return (from c in Context.Vw_Documentos where c.Activo == true && c.UsuarioIDDocumento == UsuarioID select c).ToList();
        }


        #endregion

        #region GUARGAR ICONO OPERADOR/ENTIDAD

        public bool GuardarIconoOperadorEntidad(Ext.Net.FileUploadField file, long OperadorID)
        {
            bool guardado = false;

            string ruta = TreeCore.DirectoryMapping.GetIconoOperadorDirectory();
            ruta = Path.Combine(ruta, getFileNameIconoOperador(OperadorID));

            if (!File.Exists(ruta) && file != null)
            {
                file.PostedFile.SaveAs(ruta);
                guardado = true;
            }

            return guardado;
        }

        public string getFileNameIconoOperador(long id)
        {
            return "ico-" + id + "-map.svg";
        }

        #endregion

        #region GUARGAR IMAGEN USUARIO

        public bool GuardarImagenUsuario(System.Drawing.Image file, long UsuarioID, string Extension)
        {
            bool guardado = false;

            string ruta = TreeCore.DirectoryMapping.GetImagenUsuarioDirectory();

            string[] files = Directory.GetFiles(ruta, UsuarioID.ToString() + ".*");
            foreach (string rutaImagen in files)
            {
                if (rutaImagen.Contains(UsuarioID.ToString()))
                {
                    File.Delete(rutaImagen);
                    break;
                }
            }

            string fileName = UsuarioID.ToString() + Extension;
            ruta = Path.Combine(ruta, fileName);
            file.Save(ruta);

            //Copiamos la imagen al temporal
            string rutaTemp = TreeCore.DirectoryMapping.GetImagenUsuarioTempDirectory();
            string[] tempfiles = Directory.GetFiles(rutaTemp, UsuarioID.ToString() + ".*");
            foreach (string rutaImagen in tempfiles)
            {
                if (rutaImagen.Contains(UsuarioID.ToString()))
                {
                    File.Delete(rutaImagen);
                    break;
                }
            }

            File.Copy(ruta, Path.Combine(TreeCore.DirectoryMapping.GetImagenUsuarioTempDirectory(), fileName));
            guardado = true;

            return guardado;
        }
        #endregion

        #region GUARDAR DOCUMENTO
        public bool GuardarDocumento(Ext.Net.FileUploadField file, HttpPostedFile dropzoneFile, long CreadorID, long? ProyectoTipoID,
                                    long? EmplazamientoID, long? UsuarioID, string NombreDocumento, long DocumentoTipoID, string NombreArchivo,
                                    long? ProyectoID, DocumentosEstados documentoEstado, string descripcionDoc)
        {
            bool guardado = false;

            string extension = NombreDocumento.Split('.').ToList<string>().Last();

            Documentos doc = new Documentos()
            {
                FechaDocumento = DateTime.Now,
                Firmado = false,
                Observaciones = descripcionDoc,
                Activo = true,
                Archivo = NombreArchivo,
                Extension = extension,
                CreadorID = CreadorID,
                EmplazamientoID = EmplazamientoID,
                DocumentTipoID = DocumentoTipoID,
                Version = 1,
                Subversion = 0,
                UltimaVersion = true,
                ProyectoID = ProyectoID,
                ComentariosVersion = "",
                DocumentoEstadoID = documentoEstado.DocumentoEstadoID
            };

            if (NombreDocumento == "" || NombreDocumento.Equals(NombreArchivo))
            {
                doc.Documento = getNameDocument(NombreArchivo);
            }
            else
            {
                doc.Documento = NombreDocumento;
            }

            doc = AddItem(doc);

            if (doc != null)
            {
                doc.Archivo = doc.DocumentoID + doc.Archivo;
                UpdateItem(doc);

                #region Guardar archivo en file system

                string ruta = ObtenerRutaDocumento(doc.DocumentTipoID.Value);
                ruta = Path.Combine(ruta, doc.Archivo);

                if (!File.Exists(ruta) && file != null)
                {
                    file.PostedFile.SaveAs(ruta);
                    guardado = true;
                }
                else if (!File.Exists(ruta) && dropzoneFile != null)
                {
                    dropzoneFile.SaveAs(ruta);
                    guardado = true;
                }

                #endregion
            }

            return guardado;
        }

        public bool GuardarDocumento(Ext.Net.FileUploadField file, HttpPostedFile dropzoneFile, long CreadorID, long? ProyectoTipoID,
                                    long? ObjetoID, string ObjetoTipo, long? UsuarioID, string NombreDocumento, long DocumentoTipoID, string NombreArchivo,
                                    long? ProyectoID, DocumentosEstados documentoEstado, string descripcionDoc)
        {
            HistoricoCoreDocumentosController cHistoricoCoreDocumentos = new HistoricoCoreDocumentosController();

            bool guardado = false;

            string extension = NombreDocumento.Split('.').ToList<string>().Last();

            Documentos doc = new Documentos()
            {
                FechaDocumento = DateTime.Now,
                Firmado = false,
                Observaciones = descripcionDoc,
                Activo = true,
                Archivo = NombreArchivo,
                Extension = extension,
                CreadorID = CreadorID,
                DocumentTipoID = DocumentoTipoID,
                Version = 1,
                Subversion = 0,
                UltimaVersion = true,
                ProyectoID = ProyectoID,
                ComentariosVersion = "",
                DocumentoEstadoID = documentoEstado.DocumentoEstadoID,
                Tamano = dropzoneFile.ContentLength
            };

            switch (ObjetoTipo)
            {
                case Comun.ObjetosTipos.Emplazamiento:
                    doc.EmplazamientoID = ObjetoID;
                    break;
                case Comun.ObjetosTipos.InventarioElemento:
                    doc.InventarioElementoID = ObjetoID;
                    break;
                case Comun.ObjetosTipos.Usuario:
                    doc.UsuarioID = ObjetoID;
                    break;
                default:
                    break;
            }

            if (NombreDocumento == "" || NombreDocumento.Equals(NombreArchivo))
            {
                doc.Documento = getNameDocument(NombreArchivo);
            }
            else
            {
                doc.Documento = NombreDocumento;
            }

            doc.Slug = getNewDocumentSlug(doc.Documento);

            doc = AddItem(doc);

            if (doc != null)
            {

                doc.Archivo = doc.DocumentoID + doc.Archivo;
                UpdateItem(doc);

                #region Guardar archivo en file system

                string ruta = ObtenerRutaDocumento(doc.DocumentTipoID.Value);
                ruta = Path.Combine(ruta, doc.Archivo);

                if (!File.Exists(ruta) && file != null)
                {
                    file.PostedFile.SaveAs(ruta);
                    guardado = true;
                }
                else if (!File.Exists(ruta) && dropzoneFile != null)
                {
                    dropzoneFile.SaveAs(ruta);
                    guardado = true;
                }

                cHistoricoCoreDocumentos.addHistorico(doc, CreadorID);
                #endregion
            }

            return guardado;
        }

        #endregion

        #region AGREGAR NUEVA VERSIÓN DOCUMENTO

        public bool AddNuevaVersionDocumento(Ext.Net.FileUploadField file, HttpPostedFile dropzoneFile, string rutaDocumentoCopiaVersion, long CreadorID,
                                            long? ProyectoTipoID, long? EmplazamientoID, long? UsuarioID, string NombreDocumento, long DocumentoTipoID, string NombreArchivo,
                                            long? ProyectoID, long DocumentoPadreID, long versionDocAnterior, long DocumentoEstadoID)
        {
            bool actualizado = false;
            string ruta = ObtenerRutaDocumento(DocumentoTipoID);

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    string extension = NombreDocumento.Split('.').ToList<string>().Last();
                    Documentos ultimaVersion = getUltimaVersion(DocumentoPadreID);
                    long? version = ultimaVersion.Version;

                    Documentos doc = new Documentos()
                    {
                        FechaDocumento = DateTime.Now,
                        Firmado = false,
                        Observaciones = "",
                        Activo = true,
                        Archivo = NombreArchivo,
                        Extension = extension,
                        CreadorID = CreadorID,
                        EmplazamientoID = EmplazamientoID,
                        DocumentTipoID = DocumentoTipoID,
                        Version = ++version,
                        Subversion = 0,
                        UltimaVersion = true,
                        ComentariosVersion = "",
                        ProyectoID = ProyectoID,
                        DocumentoPadreID = DocumentoPadreID,
                        DocumentoEstadoID = DocumentoEstadoID
                    };

                    if (file != null && dropzoneFile != null)
                    {
                        doc.Documento = NombreDocumento;
                    }
                    else if (NombreDocumento == "" || NombreDocumento.Equals(NombreArchivo))
                    {
                        doc.Documento = getNameDocument(NombreArchivo);
                    }
                    else
                    {
                        doc.Documento = NombreDocumento;
                    }

                    doc = AddItem(doc);

                    if (doc != null)
                    {
                        string archivoOld = doc.Archivo;
                        doc.Archivo = doc.DocumentoID + doc.Archivo;
                        UpdateItem(doc);

                        if (setUltimaVersion(DocumentoPadreID, doc.DocumentoID))
                        {

                            #region Guardar Documento en file system


                            string rutaSave = Path.Combine(ruta, doc.Archivo);

                            if (file != null && !File.Exists(rutaSave))
                            {
                                file.PostedFile.SaveAs(rutaSave);
                                actualizado = true;
                            }
                            else if (dropzoneFile != null && !File.Exists(rutaSave))
                            {
                                dropzoneFile.SaveAs(rutaSave);
                                actualizado = true;
                            }
                            else
                            {
                                //Restauración de versión
                                string rutaOld = Path.Combine(ruta, archivoOld);

                                File.Copy(rutaOld, rutaSave);
                                actualizado = true;
                            }

                            trans.Complete();
                            #endregion
                        }
                        else
                        {
                            trans.Dispose();
                        }
                    }
                    else
                    {
                        trans.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    log.Error(ex.Message);
                    actualizado = false;
                }
            }
            return actualizado;
        }

        #endregion

        public string getNameDocument(string NombreDocumento)
        {
            string result = "";

            List<string> split = NombreDocumento.Split('.').ToList<string>();
            if (split.Count > 1)
            {
                split.RemoveAt(split.Count - 1);
                result = String.Join(".", split.ToArray());
            }
            else
            {
                result = split.First();
            }

            return result;
        }

        #region Export Plantilla Carga Documentos

        public void ExportarModeloDatos(string archivo, string tab, List<string> filaTipoDato, List<string> filaValoresPosibles, List<string> filaCabecera)
        {
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                //Sheets
                var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                    SheetId = 1,
                    Name = tab
                };

                sheets.Append(sheet);

                //Heading
                DocumentFormat.OpenXml.Spreadsheet.Row FilaTipoDato = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaValoresPosibles = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaCabecera = new DocumentFormat.OpenXml.Spreadsheet.Row();

                //Primera fila
                foreach (String tipoDato in filaTipoDato)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(tipoDato);
                    FilaTipoDato.AppendChild(cell);
                }

                //Segunda fila
                foreach (String valorPosible in filaValoresPosibles)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(valorPosible);
                    FilaValoresPosibles.AppendChild(cell);
                }

                //Segunda fila
                foreach (String cabecera in filaCabecera)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cabecera);
                    FilaCabecera.AppendChild(cell);
                }

                sheetData.AppendChild(FilaTipoDato);
                sheetData.AppendChild(FilaValoresPosibles);
                sheetData.AppendChild(FilaCabecera);

            }
        }

        #endregion

        public bool setUltimaVersion(long padreID, long nuevoDocumentoID)
        {
            bool updateSucefull = true;
            List<long> lista;
            try
            {
                lista = (from c in Context.Documentos
                         where c.DocumentoPadreID == padreID
                         select c.DocumentoID).ToList();

                lista.Add(padreID);

                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i] != nuevoDocumentoID)
                    {
                        Documentos doc = GetItem(lista[i]);

                        doc.UltimaVersion = false;
                        if (!UpdateItem(doc))
                        {
                            updateSucefull = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                updateSucefull = false;
            }

            return updateSucefull;
        }

        public Documentos getUltimaVersion(long docPadreID)
        {
            Documentos doc;
            try
            {
                doc = (from c in Context.Documentos
                       where
                            c.DocumentoPadreID == docPadreID ||
                            c.DocumentoID == docPadreID
                       orderby c.Version descending
                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                doc = null;
            }
            return doc;
        }

        public Documentos getDocIsUltimaVersion(long docPadreID)
        {
            Documentos doc;
            try
            {
                doc = (from c in Context.Documentos
                       where
                            (c.DocumentoPadreID == docPadreID ||
                            c.DocumentoID == docPadreID) &&
                            c.UltimaVersion == true
                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                doc = null;
            }
            return doc;
        }

        //METODO UTILIZADO EN EL SERVICIO IMPORT EXPORT
        public Documentos getDocumentoByDocumentoEmplazamientoCodigo(string documento, long emplazamientoID)
        {
            Documentos datos = null;

            try
            {
                datos = (from c in Context.Documentos where c.Documento == documento && c.EmplazamientoID == emplazamientoID select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //Comun.MensajeLog("DocumentosController", "getDocumentoByDocumentoEmplazamientoCodigo", ex.Message);
                datos = null;
            }

            return datos;
        }

        public Documentos getDocumentoByFile(string sFile)
        {
            Documentos datos = null;

            try
            {
                datos = (from c in Context.Documentos where c.Archivo == sFile select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //Comun.MensajeLog("DocumentosController", "getDocumentoByDocumentoEmplazamientoCodigo", ex.Message);
                datos = null;
            }

            return datos;
        }

        public List<Vw_CoreDocumentos> GetDocumentosUltimaVersionActivos(long EmplazamientoID, long DocumentoTipoID)
        {
            List<Vw_CoreDocumentos> lista;

            try
            {
                lista = (from c in Context.Vw_CoreDocumentos
                         where
                             c.EmplazamientoID == EmplazamientoID &&
                             c.DocumentTipoID == DocumentoTipoID &&
                             c.UltimaVersion == true &&
                             c.Activo
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Vw_CoreDocumentos>();
            }
            return lista;
        }

        public List<Vw_Documentos> GetVersiones(Vw_Documentos Documento, bool inactivos)
        {
            List<Vw_Documentos> lista;
            try
            {
                IQueryable<Vw_Documentos> query = (from c in Context.Vw_Documentos
                                                   where
                                                       (c.DocumentoPadreID == Documento.DocumentoPadreID || c.DocumentoID == Documento.DocumentoPadreID || c.DocumentoID == Documento.DocumentoID)
                                                   orderby c.Version descending
                                                   select c);

                if (!inactivos)
                {
                    query = query.Where(x => x.Activo);
                }

                lista = query.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Vw_Documentos>();
            }
            return lista;
        }

        public List<Documentos> GetVersiones(long DocumentoID, bool ultimaVersion)
        {
            List<Documentos> lista;
            try
            {
                lista = (from c in Context.Documentos
                         where
                             c.Activo &&
                             (c.DocumentoPadreID == DocumentoID || c.DocumentoID == DocumentoID || c.DocumentoID == DocumentoID) &&
                             c.UltimaVersion == ultimaVersion
                         orderby c.Version descending
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Documentos>();
            }
            return lista;
        }

        public List<Vw_Documentos> GetDocumentosHijos(long documentoID)
        {
            List<Vw_Documentos> lista;

            try
            {
                lista = (from c in Context.Vw_Documentos
                         where c.DocumentoPadreID == documentoID
                         orderby c.Version ascending
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Vw_Documentos>();
            }
            return lista;
        }

        public List<Documentos> GetObjectDocumentosHijos(long documentoID)
        {
            List<Documentos> lista;

            try
            {
                lista = (from c in Context.Documentos
                         where c.DocumentoPadreID == documentoID
                         orderby c.Version ascending
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Documentos>();
            }
            return lista;
        }

        public List<Vw_CoreDocumentos> GetVwCoreByEmplazamientoID(long emplazamientoID, bool showDocsInactivos)
        {
            List<Vw_CoreDocumentos> docs;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (
                        from doc in Context.Vw_CoreDocumentos
                        where
                            doc.EmplazamientoID == emplazamientoID &&
                            doc.UltimaVersion == true
                        orderby doc.Documento
                        select doc);

                if (!showDocsInactivos)
                {
                    query = query.Where(x => x.Activo);
                }

                docs = query.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                docs = new List<Vw_CoreDocumentos>();
            }

            return docs;
        }

        public Documentos GetDocumentoByObjetoNegocio(string sNombre, string sObjeto, string sIdentificador)
        {
            Documentos oDato = null;
            try
            {
                switch (sObjeto)
                {
                    case "SITES":
                        oDato = (from c in Context.Documentos join Empl in Context.Emplazamientos on c.EmplazamientoID equals Empl.EmplazamientoID where c.Documento == sNombre && Empl.Codigo == sIdentificador select c).First();
                        break;
                    default:
                        oDato = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        #region METODOS GENERALIZADOS

        public List<Vw_CoreDocumentos> GetVwCoreByClienteID(long clienteID, bool showDocsInactivos)
        {
            List<Vw_CoreDocumentos> docs;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (from doc in Context.Vw_CoreDocumentos
                                                       join user in Context.Usuarios on doc.CreadorID equals user.UsuarioID
                                                       where
                                                           user.ClienteID == clienteID &&
                                                           doc.UltimaVersion == true
                                                       orderby doc.Documento
                                                       select doc);

                if (!showDocsInactivos)
                {
                    query = query.Where(x => x.Activo);
                }

                docs = query.ToList();
            }
            catch (Exception)
            {
                docs = new List<Vw_CoreDocumentos>();
            }

            return docs;
        }

        public Dictionary<long, List<Vw_CoreDocumentos>> GetVwCoreByObjetoID(long objetoID, string objetoTipo, bool showDocsInactivos, long usuarioID, long? documentTipoID)
        {
            List<Vw_CoreDocumentos> docs = null;
            Dictionary<long, List<Vw_CoreDocumentos>> diccionario = null;

            try
            {
                if (documentTipoID.HasValue)
                {
                    IQueryable<Vw_CoreDocumentos> query = (
                            from doc in Context.Vw_CoreDocumentos
                            join docTyp in Context.DocumentTipos on doc.DocumentTipoID equals docTyp.DocumentTipoID
                            where
                                docTyp.SuperDocumentTipoID == documentTipoID &&
                                doc.UltimaVersion == true
                            orderby doc.Documento
                            select doc);

                    switch (objetoTipo)
                    {
                        case Comun.ObjetosTipos.Emplazamiento:
                            query = query.Where(o => o.EmplazamientoID == objetoID);
                            break;
                        case Comun.ObjetosTipos.InventarioElemento:
                            query = query.Where(o => o.InventarioElementoID == objetoID);
                            break;
                        case Comun.ObjetosTipos.Usuario:
                            query = query.Join(Context.Documentos,
                                                doc => doc.DocumentoID,
                                                vDoc => vDoc.DocumentoID,
                                                (doc, vDoc) => new { doc, vDoc })
                                            .Where(x => x.vDoc.UsuarioID == objetoID)
                                            .Select(x => x.doc);
                            break;
                        default:
                            throw new Exception("GetVwCoreByObjetoID - Object not implemented yet");
                    }

                    if (!showDocsInactivos)
                    {
                        query = query.Where(x => x.Activo);
                    }

                    docs = query.ToList();

                    diccionario = toDictionary(docs, usuarioID);
                }
                else
                {
                    List<long> listaDocumentosTiposIDs = (from c in Context.DocumentTipos
                          where c.SuperDocumentTipoID == null && !c.EsCarpeta
                          select c.DocumentTipoID).ToList();

                    IQueryable<Vw_CoreDocumentos> query = (
                            from doc in Context.Vw_CoreDocumentos
                            where 
                                doc.UltimaVersion == true && 
                                listaDocumentosTiposIDs.Contains((long)doc.DocumentTipoID)
                            orderby doc.Documento
                            select doc);

                    switch (objetoTipo)
                    {
                        case Comun.ObjetosTipos.Emplazamiento:
                            query = query.Where(o => o.EmplazamientoID == objetoID);
                            break;
                        case Comun.ObjetosTipos.InventarioElemento:
                            query = query.Where(o => o.InventarioElementoID == objetoID);
                            break;
                        case Comun.ObjetosTipos.Usuario:
                            query = query.Join(Context.Documentos,
                                                doc => doc.DocumentoID,
                                                vDoc => vDoc.DocumentoID,
                                                (doc, vDoc) => new { doc, vDoc })
                                            .Where(x => x.vDoc.UsuarioID == objetoID)
                                            .Select(x => x.doc);
                            break;
                        default:
                            throw new Exception("GetVwCoreByObjetoID - Object not implemented yet");
                    }

                    if (!showDocsInactivos)
                    {
                        query = query.Where(x => x.Activo);
                    }

                    docs = query.ToList();
                    diccionario = toDictionary(docs, usuarioID);
                }
            }
            catch (Exception)
            {
                diccionario = new Dictionary<long, List<Vw_CoreDocumentos>>();
            }

            return diccionario;
        }

        public Dictionary<long, List<Vw_CoreDocumentos>> GetVwCoreByObjetoID(long objetoID, string objetoTipo, bool showDocsInactivos, long usuarioID)
        {
            List<Vw_CoreDocumentos> docs;
            Dictionary<long, List<Vw_CoreDocumentos>> diccionario;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (
                        from doc in Context.Vw_CoreDocumentos
                        join docTyp in Context.DocumentTipos on doc.DocumentTipoID equals docTyp.DocumentTipoID
                        where
                            docTyp.Activo &&
                            doc.UltimaVersion == true
                        orderby doc.Documento
                        select doc);

                switch (objetoTipo)
                {
                    case Comun.ObjetosTipos.Emplazamiento:
                        query = query.Where(o => o.EmplazamientoID == objetoID);
                        break;
                    case Comun.ObjetosTipos.InventarioElemento:
                        query = query.Where(o => o.InventarioElementoID == objetoID);
                        break;
                    case Comun.ObjetosTipos.Usuario:
                        query = query.Join(Context.Documentos,
                                            doc => doc.DocumentoID,
                                            vDoc => vDoc.DocumentoID,
                                            (doc, vDoc) => new { doc, vDoc })
                                        .Where(x => x.vDoc.UsuarioID == objetoID)
                                        .Select(x => x.doc);
                        break;
                    default:
                        throw new Exception("GetVwCoreByObjetoID - Object not implemented yet");
                }

                if (!showDocsInactivos)
                {
                    query = query.Where(x => x.Activo);
                }

                docs = query.ToList();

                diccionario = toDictionary(docs, usuarioID);
            }
            catch (Exception)
            {
                diccionario = new Dictionary<long, List<Vw_CoreDocumentos>>();
            }

            return diccionario;
        }

        public bool GetObjectDataByDocumentID(long docID, out long? objetoID, out string objetoTipo)
        {
            bool res = true;

            objetoID = null;
            objetoTipo = null;

            Documentos doc = GetItem(docID);
            foreach (PropertyInfo prop in doc.GetType().GetProperties())
            {

                bool nameCheck = prop.Name != "DocumentoID" && prop.Name != "CreadorID" && prop.Name != "DocumentTipoID";
                nameCheck = nameCheck && prop.Name != "DocumentoPadreID" && prop.Name != "DocumentoEstadoID";

                dynamic value = prop.GetValue(doc, null);
                if (nameCheck && value != null && prop.Name.EndsWith("ID"))
                {
                    objetoID = value;
                    objetoTipo = prop.Name.Substring(0, prop.Name.Length - 2);
                    res = true;
                    break;
                }
                else
                {
                    res = false;
                }
            }

            return res;
        }

        public string getCodigoByObjectID(long docID, string objetoTipo)
        {

            string codigoRes;
            try
            {
                IQueryable<Documentos> query = (from c in Context.Documentos where c.DocumentoID == docID select c);

                switch (objetoTipo)
                {
                    case Comun.ObjetosTipos.Emplazamiento:
                        codigoRes = query.Join(Context.Emplazamientos,
                                                doc => doc.EmplazamientoID,
                                                emp => emp.EmplazamientoID,
                                                (doc, emp) => emp.Codigo).FirstOrDefault();
                        break;
                    case Comun.ObjetosTipos.InventarioElemento:
                        codigoRes = query.Join(Context.InventarioElementos,
                                                doc => doc.InventarioElementoID,
                                                invEle => invEle.InventarioElementoID,
                                                (doc, invEle) => invEle.NumeroInventario).FirstOrDefault();
                        break;
                    case Comun.ObjetosTipos.Usuario:
                        codigoRes = query.Join(Context.Usuarios,
                                                doc => doc.UsuarioID,
                                                user => user.UsuarioID,
                                                (doc, user) => user.EMail).FirstOrDefault();
                        break;
                    default:
                        throw new Exception("AddNuevaVersionDocumento - Object type not implemented yet");
                }
            }
            catch (Exception ex)
            {
                codigoRes = "";
            }

            return codigoRes;
        }

        public string getNombreByObjectID(long docID, string objetoTipo)
        {

            string codigoRes;
            try
            {
                IQueryable<Documentos> query = (from c in Context.Documentos where c.DocumentoID == docID select c);

                switch (objetoTipo)
                {
                    case Comun.ObjetosTipos.Emplazamiento:
                        codigoRes = query.Join(Context.Emplazamientos,
                                                doc => doc.EmplazamientoID,
                                                emp => emp.EmplazamientoID,
                                                (doc, emp) => emp.NombreSitio).FirstOrDefault();
                        break;
                    case Comun.ObjetosTipos.InventarioElemento:
                        codigoRes = query.Join(Context.InventarioElementos,
                                                doc => doc.InventarioElementoID,
                                                invEle => invEle.InventarioElementoID,
                                                (doc, invEle) => invEle.Nombre).FirstOrDefault();
                        break;
                    case Comun.ObjetosTipos.Usuario:
                        codigoRes = query.Join(Context.Usuarios,
                                                doc => doc.UsuarioID,
                                                user => user.UsuarioID,
                                                (doc, user) => user.NombreCompleto).FirstOrDefault();
                        break;
                    default:
                        throw new Exception("AddNuevaVersionDocumento - Object type not implemented yet");
                }
            }
            catch (Exception ex)
            {
                codigoRes = "";
            }

            return codigoRes;
        }

        public bool AddNuevaVersionDocumento(Ext.Net.FileUploadField file, HttpPostedFile dropzoneFile, string rutaDocumentoCopiaVersion, long CreadorID,
                                            long? ProyectoTipoID, long? ObjetoID, string ObjetoTipo, long? UsuarioID, string NombreDocumento, long DocumentoTipoID,
                                            string NombreArchivo, long? ProyectoID, long DocumentoPadreID, long versionDocAnterior, long DocumentoEstadoID,
                                            long? TamanoVersionRestaurada, Vw_Documentos docAnterior, string descripcionDoc, out Documentos doc)
        {
            bool actualizado = false;
            string ruta = ObtenerRutaDocumento(DocumentoTipoID);
            Documentos docHist = null;

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    string extension = NombreArchivo.Split('.').ToList<string>().Last();
                    Documentos ultimaVersion = getUltimaVersion(DocumentoPadreID);
                    long? version = ultimaVersion.Version;

                    doc = new Documentos()
                    {
                        FechaDocumento = DateTime.Now,
                        Firmado = false,
                        Observaciones = descripcionDoc,
                        Activo = true,
                        Archivo = NombreArchivo,
                        Extension = extension,
                        CreadorID = CreadorID,
                        DocumentTipoID = DocumentoTipoID,
                        Version = ++version,
                        Subversion = 0,
                        UltimaVersion = true,
                        ComentariosVersion = "",
                        ProyectoID = ProyectoID,
                        DocumentoPadreID = DocumentoPadreID,
                        DocumentoEstadoID = DocumentoEstadoID
                    };

                    if (dropzoneFile == null && !string.IsNullOrEmpty(rutaDocumentoCopiaVersion))
                    {
                        doc.Tamano = TamanoVersionRestaurada;
                    }
                    else if (dropzoneFile != null)
                    {
                        doc.Tamano = dropzoneFile.ContentLength;
                    }

                    doc = setObjetoTipo(ObjetoTipo, doc, ObjetoID.Value);

                    if (file != null && dropzoneFile != null)
                    {
                        doc.Documento = NombreDocumento;
                    }
                    else if (NombreDocumento == "" || NombreDocumento.Equals(NombreArchivo))
                    {
                        doc.Documento = getNameDocument(NombreArchivo);
                    }
                    else
                    {
                        doc.Documento = NombreDocumento;
                    }

                    //doc.Slug = getNewDocumentSlug(doc.Documento);
                    doc.Slug = ultimaVersion.Slug;

                    doc = AddItem(doc);

                    if (doc != null)
                    {

                        string archivoOld = "";
                        if (docAnterior != null)
                        {
                            archivoOld = docAnterior.Archivo;
                        }

                        doc.Archivo = doc.DocumentoID + doc.Archivo;
                        UpdateItem(doc);


                        if (setUltimaVersion(DocumentoPadreID, doc.DocumentoID))
                        {

                            #region Guardar Documento en file system


                            string rutaSave = Path.Combine(ruta, doc.Archivo);

                            if (file != null && !File.Exists(rutaSave))
                            {
                                file.PostedFile.SaveAs(rutaSave);
                                actualizado = true;
                            }
                            else if (dropzoneFile != null && !File.Exists(rutaSave))
                            {
                                dropzoneFile.SaveAs(rutaSave);
                                actualizado = true;
                            }
                            else
                            {
                                //Restauración de versión
                                string rutaOld = Path.Combine(ruta, archivoOld);

                                File.Copy(rutaOld, rutaSave);
                                actualizado = true;
                            }


                            trans.Complete();

                            #endregion
                        }
                        else
                        {
                            trans.Dispose();
                        }
                    }
                    else
                    {
                        trans.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    log.Error(ex.Message);
                    actualizado = false;
                    doc = null;
                }
            }
            return actualizado;
        }

        public Documentos setObjetoTipo(string ObjetoTipo, Documentos doc, long ObjetoID)
        {
            switch (ObjetoTipo)
            {
                case Comun.ObjetosTipos.Emplazamiento:
                    doc.EmplazamientoID = ObjetoID;
                    break;
                case Comun.ObjetosTipos.InventarioElemento:
                    doc.InventarioElementoID = ObjetoID;
                    break;
                case Comun.ObjetosTipos.Usuario:
                    doc.UsuarioID = ObjetoID;
                    break;
                default:
                    throw new Exception("AddNuevaVersionDocumento - Object type not implemented yet");

            }

            return doc;
        }

        public long getObjetoTipoID(Documentos doc, string ObjetoTipo)
        {
            long tipoID = -1;

            switch (ObjetoTipo)
            {
                case Comun.ObjetosTipos.Emplazamiento:
                    tipoID = doc.EmplazamientoID.Value;
                    break;
                case Comun.ObjetosTipos.InventarioElemento:
                    tipoID = doc.InventarioElementoID.Value;
                    break;
                case Comun.ObjetosTipos.Usuario:
                    tipoID = doc.UsuarioID.Value;
                    break;
                default:
                    throw new Exception("AddNuevaVersionDocumento - Object type not implemented yet");

            }
            return tipoID;
        }

        public long getObjetoTipoID(Vw_Documentos doc, string ObjetoTipo)
        {
            long tipoID = -1;

            switch (ObjetoTipo)
            {
                case Comun.ObjetosTipos.Emplazamiento:
                    tipoID = doc.EmplazamientoID.Value;
                    break;
                case Comun.ObjetosTipos.InventarioElemento:
                    tipoID = doc.InventarioElementoID.Value;
                    break;
                case Comun.ObjetosTipos.Usuario:
                    tipoID = doc.UsuarioID.Value;
                    break;
                default:
                    throw new Exception("AddNuevaVersionDocumento - Object type not implemented yet");

            }
            return tipoID;
        }

        public string getObjetoTipo(Documentos doc)
        {
            string tipo;

            if (doc.EmplazamientoID.HasValue)
            {
                tipo = Comun.ObjetosTipos.Emplazamiento;
            }
            else if (doc.InventarioElementoID.HasValue)
            {
                tipo = Comun.ObjetosTipos.InventarioElemento;
            }
            else if (doc.UsuarioID.HasValue)
            {
                tipo = Comun.ObjetosTipos.Usuario;
            }
            else
            {
                tipo = "";
            }

            return tipo;
        }

        public string getObjetoTipo(Vw_CoreDocumentos doc)
        {
            string tipo;

            if (doc.EmplazamientoID.HasValue)
            {
                tipo = Comun.ObjetosTipos.Emplazamiento;
            }
            else if (doc.InventarioElementoID.HasValue)
            {
                tipo = Comun.ObjetosTipos.InventarioElemento;
            }
            else if (doc.UsuarioID.HasValue)
            {
                tipo = Comun.ObjetosTipos.Usuario;
            }
            else
            {
                tipo = "";
            }

            return tipo;
        }

        public string GetObjectCodeByObjectID(long? objetoID, string objetoTipo)
        {
            string res;

            try
            {
                switch (objetoTipo)
                {
                    case Comun.ObjetosTipos.Emplazamiento:
                        EmplazamientosController cEmplazamiento = new EmplazamientosController();
                        res = cEmplazamiento.GetItem(Convert.ToInt64(objetoID)).Codigo;
                        break;
                    case Comun.ObjetosTipos.InventarioElemento:
                        InventarioElementosController cInvElemento = new InventarioElementosController();
                        res = cInvElemento.GetItem(Convert.ToInt64(objetoID)).NumeroInventario;
                        break;
                    case Comun.ObjetosTipos.Usuario:
                        UsuariosController cUsuarios = new UsuariosController();
                        res = cUsuarios.GetItem(Convert.ToInt64(objetoID)).EMail;
                        break;
                    default:
                        throw new Exception("GetObjectCodeByObjectID - Object type not implemented yet");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res = "";
            }

            return res;
        }

        public string GetObjectNameByObjectID(long? objetoID, string objetoTipo)
        {
            string res;

            try
            {
                switch (objetoTipo)
                {
                    case Comun.ObjetosTipos.Emplazamiento:
                        EmplazamientosController cEmplazamiento = new EmplazamientosController();
                        res = cEmplazamiento.GetItem(Convert.ToInt64(objetoID)).NombreSitio;
                        break;
                    case Comun.ObjetosTipos.InventarioElemento:
                        InventarioElementosController cInvElemento = new InventarioElementosController();
                        res = cInvElemento.GetItem(Convert.ToInt64(objetoID)).Nombre;
                        break;
                    case Comun.ObjetosTipos.Usuario:
                        UsuariosController cUsuarios = new UsuariosController();
                        res = cUsuarios.GetItem(Convert.ToInt64(objetoID)).NombreCompleto;
                        break;
                    default:
                        throw new Exception("GetObjectCodeByObjectID - Object type not implemented yet");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res = "";
            }

            return res;
        }


        #endregion

        public List<Vw_Documentos> GetDocumentosConPermisoDescarga(List<long> idsDoc, long UsuarioID)
        {
            List<Vw_Documentos> lista;
            List<long> documentosTipos = new List<long>();

            try
            {
                List<long> lDocumentosTipossAux;

                Usuarios usuario = (from user in Context.Usuarios where user.UsuarioID == UsuarioID select user).First();

                #region Tipos por Rol
                lDocumentosTipossAux = (
                    from uRol in Context.UsuariosRoles
                    join docRol in Context.DocumentosTiposRoles on uRol.RolID equals docRol.RolID
                    join docTipo in Context.DocumentTipos on docRol.TipoDocumentoID equals docTipo.DocumentTipoID
                    join user in Context.Usuarios on uRol.UsuarioID equals user.UsuarioID
                    where
                        uRol.UsuarioID == UsuarioID &&
                        docRol.Activo &&
                        docTipo.Activo &&
                        docTipo.ClienteID == user.ClienteID &&
                        docRol.PermisoDescarga

                    orderby docTipo.DocumentTipo
                    select docTipo.DocumentTipoID).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region RolDefecto
                lDocumentosTipossAux = (
                    from docRol in Context.DocumentosTiposRoles
                    join docTipo in Context.DocumentTipos on docRol.TipoDocumentoID equals docTipo.DocumentTipoID
                    where
                        docRol.RolID == null &&
                        docRol.Activo &&
                        docTipo.Activo &&
                        docTipo.ClienteID == usuario.ClienteID &&
                        docRol.PermisoDescarga

                    orderby docTipo.DocumentTipo
                    select docTipo.DocumentTipoID).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                List<long> docTiposDef = new List<long>();
                DocumentTiposController cDocumentTipos = new DocumentTiposController();
                documentosTipos.ForEach(dt =>
                {
                    DocumentosTiposRoles permiso = cDocumentTipos.CompruebaPermisoDocumentoTipo(dt, UsuarioID);
                    if (permiso != null && permiso.PermisoDescarga)
                    {
                        docTiposDef.Add(dt);
                    }
                });


                lista = (from c in Context.Vw_Documentos
                         where
                            idsDoc.Contains(c.DocumentoID) &&
                            c.DocumentTipoID != null &&
                            docTiposDef.Contains(c.DocumentTipoID.Value) &&
                            c.UltimaVersion == true

                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<Vw_Documentos> GetDocumentosConPermisoDescargaByTipo(long UsuarioID, long clienteID, DocumentTipos documentTipo, string path, long? objID, string tipoObj)
        {
            List<Vw_Documentos> lista = new List<Vw_Documentos>();

            try
            {
                DocumentTiposController cDocumentTipos = new DocumentTiposController();
                DocumentosTiposRoles permisos = cDocumentTipos.CompruebaPermisoDocumentoTipo(documentTipo.DocumentTipoID, UsuarioID);

                if (permisos != null && permisos.PermisoDescarga)
                {
                    List<DocumentTipos> listDocTip = cDocumentTipos.GetBySuperDocumentTipo(documentTipo.DocumentTipoID);

                    path += documentTipo.DocumentTipo + "/";

                    lista.AddRange((from doc in Context.Vw_Documentos
                                    join doctyp in Context.DocumentTipos on doc.DocumentTipoID equals doctyp.DocumentTipoID
                                    join docRoles in Context.DocumentosTiposRoles on doctyp.DocumentTipoID equals docRoles.TipoDocumentoID into dp
                                    from d in dp.DefaultIfEmpty()
                                    join usPer in Context.UsuariosRoles on d.RolID equals usPer.RolID into up
                                    from p in up.DefaultIfEmpty()
                                    where
                                       (p.UsuarioID == UsuarioID || (d.RolID == default && doctyp.ClienteID == clienteID)) &&
                                       (d.Activo || d == default) &&
                                       doctyp.Activo &&
                                       doc.Activo &&
                                       documentTipo.DocumentTipoID == doctyp.DocumentTipoID &&
                                       d.PermisoDescarga &&
                                       doc.UltimaVersion == true
                                    select doc).ToList());

                    lista.ForEach(doc =>
                    {
                        doc.Observaciones = path;
                    });

                    lista.RemoveAll(doc => (objID == getObjetoTipoID(doc, tipoObj)));

                    listDocTip.ForEach(docTip =>
                    {
                        List<Vw_Documentos> docTemp = GetDocumentosConPermisoDescargaByTipo(UsuarioID, clienteID, docTip, path, objID, tipoObj);
                        lista.AddRange(docTemp);
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<Vw_CoreDocumentos> GetDocumentosBySlug(string sSlug)
        {
            List<Vw_CoreDocumentos> docs = new List<Vw_CoreDocumentos>();
            try
            {
                docs = (from doc in Context.Vw_CoreDocumentos
                        where
                            doc.Slug == sSlug &&
                            doc.UltimaVersion == true &&
                            doc.Activo
                        select doc).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return docs;
        }

        public Vw_CoreDocumentos getDocumentoBySlug(string slug, long UsuarioID)
        {
            Vw_CoreDocumentos doc;
            List<long> documentosTipos = new List<long>();

            try
            {
                List<long> lDocumentosTipossAux;

                Usuarios usuario = (from user in Context.Usuarios where user.UsuarioID == UsuarioID select user).First();

                #region Tipos por Roles
                lDocumentosTipossAux = (
                    from uRoles in Context.UsuariosRoles
                    join docRol in Context.DocumentosTiposRoles on uRoles.RolID equals docRol.RolID
                    join docTipo in Context.DocumentTipos on docRol.TipoDocumentoID equals docTipo.DocumentTipoID
                    join user in Context.Usuarios on uRoles.UsuarioID equals user.UsuarioID
                    where
                        uRoles.UsuarioID == UsuarioID &&
                        docRol.Activo &&
                        docTipo.ClienteID == user.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo.DocumentTipoID).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region RolDefecto
                lDocumentosTipossAux = (
                    from docRol in Context.DocumentosTiposRoles
                    join docTipo in Context.DocumentTipos on docRol.TipoDocumentoID equals docTipo.DocumentTipoID
                    where
                        docRol.RolID == null &&
                        docRol.Activo &&
                        docTipo.ClienteID == usuario.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo.DocumentTipoID).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                doc = (from c in Context.Vw_CoreDocumentos
                       where c.Slug == slug && documentosTipos.Contains(c.DocumentTipoID.Value)
                       select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                doc = null;
            }
            return doc;
        }
        public bool existsSlug(string newSlug)
        {
            bool existe;
            try
            {
                existe = (from c in Context.Documentos
                          where c.Slug == newSlug
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = true;
            }
            return existe;
        }

        public string getNewDocumentSlug(string nombreDocumento)
        {
            string slug = "";
            long countSlug = 1;

            try
            {
                string preSlug = System.Net.WebUtility.UrlEncode(nombreDocumento);
                preSlug = preSlug.Replace("+", "_");
                string slugTemp = preSlug;
                bool existe = true;
                while (slug == "" && existe)
                {
                    existe = existsSlug(slugTemp);
                    if (existe)
                    {
                        countSlug++;
                        slugTemp = preSlug + "_" + countSlug;
                    }
                    else
                    {
                        slug = slugTemp;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                slug = null;
            }
            return slug;
        }

        public Dictionary<long, List<Vw_CoreDocumentos>> GetByCarpeta(long? tipo, bool inactivos, long usuarioID)
        {
            List<Vw_CoreDocumentos> documentos;
            Dictionary<long, List<Vw_CoreDocumentos>> diccionarioResult;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (from doc in Context.Vw_CoreDocumentos
                                                       where
                                                           doc.UltimaVersion == true
                                                       select doc);
                if (tipo.HasValue)
                {
                    query = query.Join(Context.DocumentTipos,
                            vDoc => vDoc.DocumentTipoID,
                            tipos => tipos.DocumentTipoID,
                            (vDoc, tipos) => new { tipos, vDoc })
                        .Where(x=> x.tipos.SuperDocumentTipoID == tipo && !x.tipos.EsCarpeta).
                        Select(x => x.vDoc);
                }
                else
                {
                    query = query.Join(Context.DocumentTipos,
                            vDoc => vDoc.DocumentTipoID,
                            tipos => tipos.DocumentTipoID,
                            (vDoc, tipos) => new { tipos, vDoc })
                        .Where(x => x.tipos.SuperDocumentTipoID == null && !x.tipos.EsCarpeta).
                        Select(x => x.vDoc);
                }

                if (!inactivos)
                {
                    query = query.Where(x => x.Activo);
                }

                documentos = query.ToList();


                diccionarioResult = toDictionary(documentos, usuarioID);


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                diccionarioResult = new Dictionary<long, List<Vw_CoreDocumentos>>();
            }
            return diccionarioResult;
        }

        public Dictionary<long, List<Vw_CoreDocumentos>> GetAllByUsuario(bool inactivos, long usuarioID)
        {
            List<Vw_CoreDocumentos> documentos;
            Dictionary<long, List<Vw_CoreDocumentos>> diccionarioResult;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (from doc in Context.Vw_CoreDocumentos
                                                       join docTyp in Context.DocumentTipos on doc.DocumentTipoID equals docTyp.DocumentTipoID
                                                       where
                                                           docTyp.Activo &&
                                                           doc.UltimaVersion == true
                                                       select doc);
                if (!inactivos)
                {
                    query = query.Where(x => x.Activo);
                }

                documentos = query.ToList();
                diccionarioResult = toDictionary(documentos, usuarioID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                diccionarioResult = new Dictionary<long, List<Vw_CoreDocumentos>>();
            }
            return diccionarioResult;
        }
        public Dictionary<long, List<Vw_CoreDocumentos>> toDictionary(List<Vw_CoreDocumentos> documentos, long usuarioID)
        {
            DocumentTiposController cDocumentTipos = new DocumentTiposController();
            Dictionary<long, List<Vw_CoreDocumentos>> diccionarioResult = new Dictionary<long, List<Vw_CoreDocumentos>>();

            documentos.ForEach(doc =>
            {

                if (!diccionarioResult.ContainsKey(doc.DocumentTipoID.Value))
                {
                    DocumentosTiposRoles permiso = cDocumentTipos.CompruebaPermisoDocumentoTipo(doc.DocumentTipoID.Value, usuarioID);

                    if (permiso != null && (permiso.PermisoLectura || permiso.PermisoEscritura || permiso.PermisoDescarga))
                    {
                        diccionarioResult.Add(doc.DocumentTipoID.Value, new List<Vw_CoreDocumentos>());
                        diccionarioResult[doc.DocumentTipoID.Value].Add(doc);
                    }
                }
                else
                {
                    diccionarioResult[doc.DocumentTipoID.Value].Add(doc);
                }

            });

            return diccionarioResult;
        }

        public List<Documentos> getDocumentos(List<long> ids)
        {
            List<Documentos> documentos;

            try
            {
                documentos = (from c in Context.Documentos
                              where
                                ids.Contains(c.DocumentoID) &&
                                c.UltimaVersion == true
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                documentos = new List<Documentos>();
            }

            return documentos;
        }

        public List<Vw_CoreDocumentos> GetDocumentosByNombreAndDescripcion(string textoBuscado, bool verInactivos, string sSlug,
                                                                            long? lObjetoID, string sObjetoTipo, long? ClienteID)
        {
            List<Vw_CoreDocumentos> documentos;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (from c in Context.Vw_CoreDocumentos
                                                       join type in Context.DocumentTipos on c.DocumentTipoID equals type.DocumentTipoID into documents
                                                       from doct in documents.DefaultIfEmpty()
                                                       where
                                                           c.UltimaVersion == true &&
                                                           (c.Documento.Contains(textoBuscado) ||
                                                           c.Observaciones.Contains(textoBuscado))
                                                       select c);

                if (!verInactivos)
                {
                    query = query.Where(c => c.Activo);
                }

                if (lObjetoID != null && !string.IsNullOrEmpty(sObjetoTipo))
                {
                    switch (sObjetoTipo)
                    {
                        case Comun.ObjetosTipos.Emplazamiento:
                            query = query.Where(o => o.EmplazamientoID == lObjetoID);
                            break;
                        case Comun.ObjetosTipos.InventarioElemento:
                            query = query.Where(o => o.InventarioElementoID == lObjetoID);
                            break;
                        case Comun.ObjetosTipos.Usuario:
                            query = query.Join(Context.Documentos,
                                                doc => doc.DocumentoID,
                                                vDoc => vDoc.DocumentoID,
                                                (doc, vDoc) => new { doc, vDoc })
                                            .Where(x => x.vDoc.UsuarioID == lObjetoID)
                                            .Select(x => x.doc);
                            break;
                        default:
                            throw new Exception("GetDocumentosByNombreAndDescripcion - Object not implemented yet");
                    }
                }

                documentos = query.ToList();





            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                documentos = new List<Vw_CoreDocumentos>();
            }

            return documentos;
        }

        public List<Vw_CoreDocumentos> HasDocumentWhitNameAndDocumentTypeAndObjectType(string fileName, long DocumentoTipoID, long ObjetoID, string sObjetoTipo)
        {
            List<Vw_CoreDocumentos> coincidencias;

            try
            {
                IQueryable<Vw_CoreDocumentos> query = (from c in Context.Vw_CoreDocumentos
                 where 
                    c.Documento == fileName && 
                    c.DocumentTipoID == DocumentoTipoID &&
                    c.UltimaVersion == true
                 select c);

                switch (sObjetoTipo)
                {
                    case Comun.ObjetosTipos.Emplazamiento:
                        query = query.Where(o => o.EmplazamientoID == ObjetoID);
                        break;
                    case Comun.ObjetosTipos.InventarioElemento:
                        query = query.Where(o => o.InventarioElementoID == ObjetoID);
                        break;
                    case Comun.ObjetosTipos.Usuario:
                        query = query.Join(Context.Documentos,
                                            doc => doc.DocumentoID,
                                            vDoc => vDoc.DocumentoID,
                                            (doc, vDoc) => new { doc, vDoc })
                                        .Where(x => x.vDoc.UsuarioID == ObjetoID)
                                        .Select(x => x.doc);
                        break;
                    default:
                        throw new Exception("HasDocumentWhitNameAndDocumentTypeAndObjectType - Object not implemented yet");
                }

                coincidencias = query.ToList();

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                coincidencias = null;
            }

            return coincidencias;
        }


    }
}