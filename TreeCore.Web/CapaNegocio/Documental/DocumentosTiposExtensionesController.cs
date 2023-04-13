using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using Ext.Net;
using System.Web;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class DocumentosTiposExtensionesController : GeneralBaseController<DocumentosTiposExtensiones, TreeCoreContext>, IGestionBasica<DocumentosTiposExtensiones>
    {
        public DocumentosTiposExtensionesController()
            : base()
        { }

        public InfoResponse Add(DocumentosTiposExtensiones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }

            return oResponse;
        }

        public InfoResponse Update(DocumentosTiposExtensiones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = null
                };
            }

            return oResponse;
        }

        public InfoResponse Delete(DocumentosTiposExtensiones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }

            return oResponse;
        }

        public List<Vw_DocumentosTiposExtensiones> GetExtensionByDocumentTipo(long lTipoDocumentoID)
        {

            List<Vw_DocumentosTiposExtensiones> documentos;
            List<long> datos;

            datos = (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == lTipoDocumentoID select c.DocumentoExtensionID).ToList<long>();
            documentos = (from c in Context.Vw_DocumentosTiposExtensiones where (datos.Contains((long)c.DocumentoExtensionID) && lTipoDocumentoID == c.DocumentTipoID) select c).OrderBy("Extension").ToList<Vw_DocumentosTiposExtensiones>();

            return documentos;

        }


        public List<DocumentosTiposExtensiones> GetExtensionesByDocumentTipo(long? lTipoDocumentoID)
        {

            List<DocumentosTiposExtensiones> documentos;
            documentos = (from c in Context.DocumentosTiposExtensiones where (c.DocumentTipoID == lTipoDocumentoID) select c).ToList<DocumentosTiposExtensiones>();

            return documentos;

        }

        public List<Vw_DocumentosTiposExtensiones> GetExtensionesNoAsignadosByDocumentTipo(long lTipoDocumentoID)
        {

            List<Vw_DocumentosTiposExtensiones> documentos;
            List<long> datos;

            datos = (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == lTipoDocumentoID select c.DocumentoExtensionID).ToList<long>();
            documentos = (from c in Context.Vw_DocumentosTiposExtensiones where !(datos.Contains((long)c.DocumentoExtensionID)) select c).OrderBy("Extension").ToList<Vw_DocumentosTiposExtensiones>();

            return documentos;

        }

        public List<Vw_DocumentosTiposExtensiones> GetExtensionesActivasNoAsignadasByDocumentTipo(long lTipoDocumentoID)
        {

            List<Vw_DocumentosTiposExtensiones> documentos = new List<Vw_DocumentosTiposExtensiones>();
            List<long> extensionesActivas;
            List<long> datos;
            try
            {
                extensionesActivas = (from c in Context.DocumentosExtensiones where c.Activo select c.DocumentoExtensionID).ToList();
                datos = (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == lTipoDocumentoID select c.DocumentoExtensionID).ToList<long>();
                documentos = (from c in Context.Vw_DocumentosTiposExtensiones where !(datos.Contains((long)c.DocumentoExtensionID)) && (extensionesActivas.Contains(c.DocumentoExtensionID)) select c).OrderBy("Extension").ToList<Vw_DocumentosTiposExtensiones>();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return documentos;

        }


        public List<Vw_DocumentosTiposExtensiones> GetTiposDocumentosByExtension(long lDocumentoExtensionID)
        {

            List<Vw_DocumentosTiposExtensiones> documentos = null;

            documentos = (from c in Context.Vw_DocumentosTiposExtensiones where c.DocumentoExtensionID == lDocumentoExtensionID select c).ToList();

            return documentos;

        }

        public List<Vw_DocumentosTiposExtensiones> ExtensionesPermitidasByTipoDocumento(long TipoDocumentoID)
        {
            //bool valida = false;

            List<Vw_DocumentosTiposExtensiones> documentos = null;

            documentos = (from c in Context.Vw_DocumentosTiposExtensiones where (c.DocumentTipoID == TipoDocumentoID) select c).ToList();

            return documentos;
        }
    }
}