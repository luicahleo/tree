using CapaNegocio;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.PaginasComunes
{
    /// <summary>
    /// Descripción breve de DocumentosFileUpload
    /// </summary>
    public class DocumentosFileUpload : BaseIHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void ProcessRequest(HttpContext context)
        {
            resultMessage resultMessage = new resultMessage();

            DocumentosController cDoc = new DocumentosController();
            DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();
            cDocumentosEstados.SetDataContext(cDoc.Context);

            try
            {
                string documentoID = null;
                long DocumentoPadreID = 0;
                long versionDocAnterior = 0;
                long documentoEstado = 0;


                string lObjetoID = context.Request.Params["ObjetoID"].Split(',').First();
                string sObjetoTipo = context.Request.Params["ObjetoTipo"].Split(',').First();
                string lUsuarioID = context.Request.Params["UsuarioID"].Split(',').First();
                string lModuloID = context.Request.Params["ModuloID"].Split(',').First();
                string lDocumentoTipoID = context.Request.Params["DocumentoTipoID"].Split(',').First();
                string ExtensionNoPermitida = context.Request.Params["ExtensionNoPermitida"].Split(',').First();
                string NumeroMaximoCaracteres = context.Request.Params["NumeroMaximoCaracteres"].Split(',').First();
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(context.Request.Params["CodeLanguage"].Split(',').First());
                bool EsAgregar = bool.Parse(context.Request.Params["Agregar"].Split(',').First());
                string descripcionDoc = context.Request.Params["descripcionDoc"].Split(',').First();

                if (!EsAgregar)
                {
                    DocumentoPadreID = long.Parse(context.Request.Params["DocumentoPadreID"].Split(',').First());
                    versionDocAnterior = long.Parse(context.Request.Params["versionDocAnterior"].Split(',').First());
                    documentoEstado = long.Parse(context.Request.Params["documentoEstado"].Split(',').First());
                }


                //DESCARGAR ARCHIVO
                if (documentoID != null)
                {

                }
                else //SUBIR ARCHIVO
                {
                    long UsuarioID = 0;
                    long DocumentoTipoID = 0;
                    long? ModuloID = null;
                    long ObjetoID = 0;


                    if (!string.IsNullOrEmpty(lUsuarioID)) {
                        if (!string.IsNullOrEmpty(lDocumentoTipoID))
                        {
                            UsuarioID = long.Parse(lUsuarioID);
                            DocumentoTipoID = long.Parse(lDocumentoTipoID);
                            ObjetoID = long.Parse(lObjetoID);

                            if (!string.IsNullOrEmpty(lModuloID))
                            {
                                ModuloID = long.Parse(lModuloID);
                            }
                            
                            foreach (string s in context.Request.Files)
                            {
                                HttpPostedFile file = context.Request.Files[s];
                                string fileName = file.FileName;

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    if (fileName.Count() <= 200)
                                    {
                                        DocumentosEstados DocEstado = cDocumentosEstados.GetDefecto();

                                        if (DocEstado != null)
                                        {
                                            if (EsAgregar)
                                            {
                                                string errorMessage;
                                                if (cDoc.GuardarDocumento(null, file, UsuarioID, ModuloID, ObjetoID, sObjetoTipo, UsuarioID, fileName, DocumentoTipoID, null, DocEstado, descripcionDoc, out errorMessage))
                                                {
                                                    // Documento subido correctamente
                                                    resultMessage.addResultMessage(fileName, "");
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(errorMessage))
                                                    {
                                                        resultMessage.addResultMessage(fileName, GetGlobalResource(Comun.jsMensajeGenerico, cultureInfo));
                                                    }
                                                    else
                                                    {
                                                        resultMessage.addResultMessage(fileName, errorMessage);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Documentos documentoPadre = cDoc.GetItem(DocumentoPadreID);
                                                if (documentoPadre != null)
                                                {
                                                    sObjetoTipo = cDoc.getObjetoTipo(documentoPadre);
                                                    ObjetoID = cDoc.getObjetoTipoID(documentoPadre, sObjetoTipo);
                                                    Documentos docNewVersion;
                                                    string errorMessage;
                                                    if (cDoc.AddNuevaVersionDocumento(null, file, "", UsuarioID, ModuloID, ObjetoID, sObjetoTipo, UsuarioID, fileName,
                                                                                            DocumentoTipoID, null, DocumentoPadreID, versionDocAnterior,
                                                                                            documentoEstado, null, null, descripcionDoc, out docNewVersion, out errorMessage))
                                                    {
                                                        //Documento editado correctamente

                                                        resultMessage.addResultMessage(fileName, "");
                                                    }
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(errorMessage))
                                                        {
                                                            resultMessage.addResultMessage(fileName, GetGlobalResource(Comun.jsMensajeGenerico, cultureInfo));
                                                        }
                                                        else
                                                        {
                                                            resultMessage.addResultMessage(fileName, errorMessage);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    resultMessage.addResultMessage(fileName, GetGlobalResource(Comun.jsMensajeGenerico, cultureInfo));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            resultMessage.addResultMessage(fileName, GetGlobalResource(Comun.strDocumentoEstadoPorDefectoNoEncontrado, cultureInfo));
                                        }
                                    }
                                    else
                                    {
                                        // El nombre supera el límite de caracteres
                                        resultMessage.addResultMessage(fileName, NumeroMaximoCaracteres);
                                    }
                                }
                                else
                                {
                                    //nombre Vacio
                                    resultMessage.addResultMessage(fileName, "file name not valid");
                                }
                            }
                        }
                        else
                        {
                            //Tipo de documento no válido
                            resultMessage.addResultMessage(GetGlobalResource("strDocumento", cultureInfo), GetGlobalResource(Comun.ERRORAJAXGENERICO, cultureInfo));
                        }
                    }
                    else
                    {
                        //Usuario no valido
                        resultMessage.addResultMessage(GetGlobalResource("strDocumento", cultureInfo), GetGlobalResource(Comun.ERRORAJAXGENERICO, cultureInfo));
                    }
                }

            }
            catch(Exception ex)
            {
                log.Error(ex);
                resultMessage.AllFilesError();
            }

            context.Response.Write(resultMessage.ToJson());
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class resultMessage
    {
        List<object> messages = new List<object>();
        bool AllError = false;

        public void AllFilesError()
        {
            this.AllError = true;
        }


        public void addResultMessage(string fileName, string message)
        {
            messages.Add(new { fileName= fileName, message= message});
        }

        public string ToJson()
        {
            JObject json = new JObject();
            json["files"] = JToken.FromObject(messages);
            json["allError"] = this.AllError;

            return json.ToString();
        }
    }
}