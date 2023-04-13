using System;
using System.IO;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using System.Globalization;


namespace TreeCore.PaginasComunes
{
    public partial class DocumentosDescarga : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private void Page_Init(object sender, EventArgs e)
        {
            try
            {
                

                if ((Request.QueryString["DocumentoID"] != null))
                {
                    string documento = "";
                    string nombreArchivo = null;
                    long documentoID = 0;
                    Data.Vw_Documentos doc = new Data.Vw_Documentos();
                    
                    try
                    {
                        documentoID = long.Parse(Request.Params["DocumentoID"]);
                        DocumentosController cDocumentos = new DocumentosController();
                        doc = cDocumentos.GetItem<Data.Vw_Documentos>("DocumentoID == " + documentoID.ToString());
                        string ruta = cDocumentos.ObtenerRutaDocumento((long) doc.DocumentTipoID);
                        documento = ruta = Path.Combine(ruta, doc.Archivo);

                        if (File.Exists(documento))
                        {
                            System.IO.FileInfo file = new System.IO.FileInfo(documento);
                            nombreArchivo = doc.Documento;
                            Response.Clear();
                            Response.ContentType = Comun.GetMimeType(doc.Extension);
                            nombreArchivo = nombreArchivo.Replace(" ", "_");

                            if (Path.GetExtension(nombreArchivo) != String.Empty)
                            {
                                Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);
                            }
                            else
                            {
                                Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + "." + doc.Extension);
                            }

                            Response.AddHeader("Content-Length", file.Length.ToString());
                            Response.TransmitFile(documento);
                            Response.Flush();

                        }
                        else
                        {
                            Response.Write(GetGlobalResource("strMsgDocumentoNoEncontrado"));
                            Response.End();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                    Response.SuppressContent = true;
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}