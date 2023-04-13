using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using CapaNegocio;
using log4net;
using Ext.Net;
using Newtonsoft.Json.Linq;
using TreeCore.Data;
using TreeCore.Clases;
using System.Globalization;
using System.Transactions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TreeCore.PaginasComunes
{
    public class SeguimientosDocumentosFileUpload : BaseIHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected class DocumentosJson
        {
            public string Ruta { get; set; }
            public string Nombre { get; set; }
            public string Tamano { get; set; }
        }

        /*[DirectMethod]
        public DirectResponse GenerarSeguimientoDocumento(string jsonDocumentos, long SeguimientoPadreID)
        {
            DirectResponse direct = new DirectResponse
            {
                Success = true,
                Result = ""
            };
            return direct;
        }*/

        public override void ProcessRequest(HttpContext context)
        {
            resultMessage resultMessage = new resultMessage();

            try
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(context.Request.Params["CodeLanguage"].Split(',').First());
                string sSegSelect = context.Request.Params["SegSelect"];
                string sComentario = context.Request.Params["Comentario"];
                string sSeguimientoEstadoID = context.Request.Params["SeguimientoEstadoID"];
                string sTipoDocumentoID = context.Request.Params["TipoDocumentoID"];
                string sSegPadreID = context.Request.Params["SegPadreID"];
                string sUsuarioID = context.Request.Params["UsuarioID"];

                CoreWorkOrderSeguimientosController cSeguimiento = new CoreWorkOrderSeguimientosController();
                DocumentosController cDoc = new DocumentosController();
                cDoc.SetDataContext(cSeguimiento.Context);
                DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();
                cDocumentosEstados.SetDataContext(cSeguimiento.Context);
                UsuariosController cUsuarios = new UsuariosController();
                cUsuarios.SetDataContext(cSeguimiento.Context);
                var oUsuario = cUsuarios.GetItem(long.Parse(sUsuarioID));
                Data.DocumentosEstados DocEstado = cDocumentosEstados.GetDefecto();
                Data.CoreWorkOrderSeguimientos oSeguimiento;

                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                {
                    try
                    {
                        if (sSegSelect != "0")
                        {
                            oSeguimiento = cSeguimiento.GetItem(long.Parse(sSegSelect));
                            oSeguimiento.UsuarioID = oUsuario.UsuarioID;
                            oSeguimiento.Fecha = DateTime.Now;
                            oSeguimiento.Nota = sComentario;
                            oSeguimiento.Editado = false;
                            if (!cSeguimiento.UpdateItem(oSeguimiento))
                            {
                                trans.Dispose();
                                context.Response.Write("");
                                return;
                            }
                        }
                        else
                        {
                            oSeguimiento = new Data.CoreWorkOrderSeguimientos
                            {
                                UsuarioID = oUsuario.UsuarioID,
                                Fecha = DateTime.Now,
                                Nota = sComentario,
                                Activo = true,
                                Editado = false,
                                EsCambio = false,
                                CoreWorkOrderSeguimientoEstadoID = long.Parse(sSeguimientoEstadoID)
                            };
                            if (sSegPadreID != "0")
                            {
                                oSeguimiento.CoreWorkOrderSeguimientoPadreID = long.Parse(sSegPadreID);
                            }
                            if ((oSeguimiento = cSeguimiento.AddItem(oSeguimiento)) == null)
                            {
                                trans.Dispose();
                                context.Response.Write("");
                                return;
                            }
                        }                        
                        trans.Complete();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        trans.Dispose();
                        context.Response.Write("");
                        return;
                    }
                }
                foreach (string s in context.Request.Files)
                {
                    HttpPostedFile file = context.Request.Files[s];
                    string fileName = file.FileName;

                    if (string.IsNullOrEmpty(fileName))
                    {
                        context.Response.Write("");
                        return;
                    }
                    if (fileName.Count() > 200)
                    {
                        context.Response.Write("");
                        return;
                    }
                    if (DocEstado == null)
                    {
                        context.Response.Write("");
                        return;
                    }
                    else
                    {
                        string errorMessage = "";

                        if (!cDoc.GuardarDocumento(null, file, oUsuario.UsuarioID, 0, oSeguimiento.CoreWorkOrderSeguimientoID, Comun.ObjetosTipos.Seguimiento, oUsuario.UsuarioID, fileName, long.Parse(sTipoDocumentoID), null, DocEstado, sComentario, out errorMessage))
                        {
                            context.Response.Write("");
                            return;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            context.Response.Write("");
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}