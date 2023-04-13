using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class HistoricoCoreDocumentosController : GeneralBaseController<HistoricoCoreDocumentos, TreeCoreContext>
    {
        public HistoricoCoreDocumentosController()
            : base()
        { }

        public bool addHistorico(Documentos doc, long usuarioID)
        {
            bool hecho = false;

            try
            {
                DocumentosController cDocumentos = new DocumentosController();

                Vw_CoreDocumentos Vw_doc = cDocumentos.GetItem<Vw_CoreDocumentos>(doc.DocumentoID);

                if (Vw_doc == null)
                {
                    DocumentTipos tipo = null;
                    DocumentosEstados estado = null;
                    Usuarios userCreador = null;
                    if (doc.DocumentTipoID.HasValue)
                    {
                        DocumentTiposController cDocumentTipos = new DocumentTiposController();
                        tipo = cDocumentTipos.GetItem(doc.DocumentTipoID.Value);
                    }
                    if (doc.DocumentoEstadoID.HasValue)
                    {
                        DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();
                        estado = cDocumentosEstados.GetItem(doc.DocumentoEstadoID.Value);
                    }
                    UsuariosController cUsuarios = new UsuariosController();
                    userCreador = cUsuarios.GetItem(doc.CreadorID);

                    string objectType = cDocumentos.getObjetoTipo(doc);
                    string identificador = cDocumentos.getCodigoByObjectID(doc.DocumentoID, objectType);
                    string nombreobjeto = cDocumentos.getNombreByObjectID(doc.DocumentoID, objectType);

                    string docTipo = (tipo != null)? tipo.DocumentTipo : "";
                    string nombreEstado = (estado != null) ? estado.Nombre : "";
                    string creador = (userCreador != null) ? userCreador.Nombre + " " + userCreador.Apellidos : "";

                    Vw_doc = new Vw_CoreDocumentos() { 
                        Identificador = identificador,
                        Documento = doc.Documento,
                        Extension = doc.Extension,
                        Creador = creador,
                        FechaDocumento = doc.FechaDocumento,
                        NombreEstado = nombreEstado,
                        Version = doc.Version,
                        Tamano = doc.Tamano,
                        DocumentTipo = docTipo,
                        Observaciones = doc.Observaciones,
                        NombreObjeto = nombreobjeto
                    };
                }

                HistoricoCoreDocumentos histDoc = new HistoricoCoreDocumentos();

                histDoc.DocumentoID = (Vw_doc.DocumentoPadreID.HasValue) ? Vw_doc.DocumentoPadreID.Value : Vw_doc.DocumentoID;
                histDoc.FechaModificacion = DateTime.Now;
                histDoc.UsuarioID = usuarioID;
                histDoc.DatosJson = Comun.ObjectToJSON(Vw_doc, "Documentos").ToString();

                AddItem(histDoc);
                hecho = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return hecho;
        }

        public Vw_HistoricoCoreDocumentos getUltimoHistorico(long documentoID)
        {
            Vw_HistoricoCoreDocumentos hist;
            try
            {
                hist = (from doc in Context.Documentos
                            join docHist in Context.Vw_HistoricoCoreDocumentos on doc.DocumentoID equals docHist.DocumentoID
                        where (doc.DocumentoID == documentoID || doc.DocumentoPadreID.Value == documentoID)
                        select docHist).FirstOrDefault();
            }
            catch(Exception ex)
            {
                hist = null;
                log.Error(ex.Message);
            }
            return hist;
        }

        public List<Vw_HistoricoCoreDocumentos> GetHistoricoByDocumentoID(long documentoID)
        {
            List<Vw_HistoricoCoreDocumentos> lista;

            try
            {
                lista = (from c in Context.Vw_HistoricoCoreDocumentos 
                         where c.DocumentoID==documentoID
                         orderby c.FechaModificacion descending
                         select c).ToList();
                if (lista.Count > 0)
                {
                    lista.RemoveAt(0);
                }
            }
            catch(Exception ex)
            {
                lista = new List<Vw_HistoricoCoreDocumentos>();
                log.Error(ex.Message);
            }

            return lista;
        }
    }
}