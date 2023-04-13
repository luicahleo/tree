using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Reflection;
using CapaNegocio;
using System.IO;
using System.Transactions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TreeCore.ModWorkOrders
{
    public partial class WorkOrdersSeguimientos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (this.Session["USUARIO"] != null)
                {
                    #region REGISTRO DE ESTADISTICAS

                    Data.Usuarios usuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                    hdUsuarioID.Value = usuario.UsuarioID;
                    if (usuario.ClienteID.HasValue)
                    {
                        hdCliID.Value = usuario.ClienteID;
                    }
                    else
                    {
                        hdCliID.Value = 0;
                    }
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, usuario.ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                    log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                    #endregion
                }
                hdTipoVista.Value = "Todo";
                hdSegSeleccionadoID.Value = 0;
                hdSegPadreID.Value = 0;
                hdCulture.Value = this._Locale;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                if (Request.QueryString["WorkOrderID"] != null && Request.QueryString["WorkOrderID"] != "")
                {
                    long WorkOrderID = long.Parse(Request.QueryString["WorkOrderID"]);

                    hdWorkOrderID.Value = WorkOrderID;
                    CargarEstadoActual(WorkOrderID);
                    GenerarSiguientesEstados(WorkOrderID);
                }
            }
        }

        #endregion

        #region STORES

        #region SEGUIMIENTOS

        protected void storeSeguimientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    CoreWorkOrdersController cCoreWorkOrders = new CoreWorkOrdersController();
                    CoreWorkOrderSeguimientosController cCoreWorkOrdersSeguimientos = new CoreWorkOrderSeguimientosController();
                    CoreWorkOrdersSeguimientosEstadosController cEstados = new CoreWorkOrdersSeguimientosEstadosController();
                    CoreWorkOrderSeguimientosDocumentosController cCoreWorkOrdersSeguimientosDocumentos = new CoreWorkOrderSeguimientosDocumentosController();

                    JsonObject oDato;
                    List<object> listaSeguimientos, listaSubseguimiento, listaDocumentos;
                    object oSeguimiento, oSubSeguimiento, oDocumento;
                    List<JsonObject> lista = new List<JsonObject>();
                    List<Data.Documentos> listaObjDocumentos;
                    string sTipo;

                    listaSeguimientos = new List<object>();

                    Data.CoreWorkOrdersSeguimientosEstados ObjEstado = cEstados.GetItem(long.Parse(hdEstadoActID.Value.ToString()));
                    oDato = new JsonObject();

                    oDato.Add("EstadoID", ObjEstado.CoreEstadoID);
                    oDato.Add("Nombre", ObjEstado.CoreEstados.Nombre);

                    List<Data.CoreWorkOrderSeguimientos> listaObjSeguimientos = cCoreWorkOrdersSeguimientos.GetSeguimientosFromEstadoFiltrado(ObjEstado.CoreWorkOrderSeguimientoEstadoID, hdTipoVista.Value.ToString());

                    listaSubseguimiento = new List<object>();

                    foreach (var oSeg in listaObjSeguimientos)
                    {

                        List<Data.CoreWorkOrderSeguimientos> listaObjSubSeguimientos = cCoreWorkOrdersSeguimientos.GetSubSeguimientos(oSeg.CoreWorkOrderSeguimientoID);

                        listaSubseguimiento = new List<object>();

                        foreach (var oSub in listaObjSubSeguimientos)
                        {
                            listaObjDocumentos = cCoreWorkOrdersSeguimientosDocumentos.GetDocumentosFromSeguimiento(oSub.CoreWorkOrderSeguimientoID);

                            listaDocumentos = new List<object>();

                            foreach (var oDoc in listaObjDocumentos)
                            {
                                oDocumento = new
                                {
                                    DocumentoID = oDoc.DocumentoID,
                                    Nombre = oDoc.Documento,
                                    Ruta = Path.Combine(DirectoryMapping.GetTempDirectory(oDoc.DocumentTipos.DocumentTipo) + oDoc.Archivo)
                                };
                                listaDocumentos.Add(oDocumento);
                            }

                            if (oSub.EsCambio)
                            {
                                sTipo = "Estado";
                            }
                            else if (listaDocumentos.Count > 0)
                            {
                                sTipo = "Documento";
                            }
                            else
                            {
                                sTipo = "Comentario";
                            }

                            oSubSeguimiento = new
                            {
                                SeguimientoID = oSub.CoreWorkOrderSeguimientoID,
                                Nombre = oSub.Usuarios.Nombre,
                                Imagen = "/_temp/imgUsuarios/" + oSub.UsuarioID + ".jpg",
                                Proyecto = oSub.Usuarios.NombreCompleto,
                                Fecha = oSub.Fecha.ToString(Comun.FORMATO_FECHA),
                                Comentarios = oSub.Nota,
                                Editado = oSub.Editado,
                                Documentos = listaDocumentos,
                                Tipo = sTipo
                            };

                            listaSubseguimiento.Add(oSubSeguimiento);
                        }

                        listaObjDocumentos = cCoreWorkOrdersSeguimientosDocumentos.GetDocumentosFromSeguimiento(oSeg.CoreWorkOrderSeguimientoID);

                        listaDocumentos = new List<object>();

                        foreach (var oDoc in listaObjDocumentos)
                        {
                            oDocumento = new
                            {
                                DocumentoID = oDoc.DocumentoID,
                                Nombre = oDoc.Documento,
                                Ruta = Path.Combine(DirectoryMapping.GetTempDirectory(oDoc.DocumentTipos.DocumentTipo) + oDoc.Archivo)
                            };
                            listaDocumentos.Add(oDocumento);
                        }

                        if (oSeg.EsCambio)
                        {
                            sTipo = "Estado";
                        }
                        else if (listaDocumentos.Count > 0)
                        {
                            sTipo = "Documento";
                        }
                        else
                        {
                            sTipo = "Comentario";
                        }

                        oSeguimiento = new
                        {
                            SeguimientoID = oSeg.CoreWorkOrderSeguimientoID,
                            Nombre = oSeg.Usuarios.Nombre,
                            Imagen = "/_temp/imgUsuarios/" + oSeg.UsuarioID + ".jpg",
                            Proyecto = oSeg.Usuarios.NombreCompleto,
                            Fecha = oSeg.Fecha.ToString(Comun.FORMATO_FECHA),
                            Comentarios = oSeg.Nota,
                            Editado = oSeg.Editado,
                            Documentos = listaDocumentos,
                            SubSeguimientos = listaSubseguimiento,
                            TieneComentarios = (listaSubseguimiento.Count > 0),
                            Tipo = sTipo
                        };

                        listaSeguimientos.Add(oSeguimiento);
                    }

                    oDato.Add("Seguimientos", listaSeguimientos);
                    lista.Add(oDato);

                    if (lista != null)
                    {
                        storeSeguimientos.DataSource = lista;
                        storeSeguimientos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region SEGUIMIENTOS ANTERIORES

        protected void storeSeguimientosAnteriores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    CoreWorkOrdersController cCoreWorkOrders = new CoreWorkOrdersController();
                    CoreWorkOrderSeguimientosController cCoreWorkOrdersSeguimientos = new CoreWorkOrderSeguimientosController();
                    CoreWorkOrderSeguimientosDocumentosController cCoreWorkOrdersSeguimientosDocumentos = new CoreWorkOrderSeguimientosDocumentosController();
                    CoreWorkOrdersSeguimientosEstadosController cEstados = new CoreWorkOrdersSeguimientosEstadosController();

                    JsonObject oDato;
                    List<object> listaSeguimientos, listaSubseguimiento, listaDocumentos;
                    object oSeguimiento, oSubSeguimiento, oDocumento;
                    List<JsonObject> lista = new List<JsonObject>();
                    List<Data.Documentos> listaObjDocumentos;
                    string sTipo;

                    listaSeguimientos = new List<object>();

                    List<Data.CoreWorkOrdersSeguimientosEstados> listaObjSeguimientosPadres = cEstados.GetEstadosAnteriores(long.Parse(hdWorkOrderID.Value.ToString()));


                    foreach (Data.CoreWorkOrdersSeguimientosEstados oSegPra in listaObjSeguimientosPadres)
                    {
                        oDato = new JsonObject();
                        oDato.Add("EstadoID", oSegPra.CoreEstadoID);
                        oDato.Add("Nombre", oSegPra.CoreEstados.Nombre);

                        List<Data.CoreWorkOrderSeguimientos> listaObjSeguimientos = cCoreWorkOrdersSeguimientos.GetSeguimientosFromEstadoFiltrado(oSegPra.CoreWorkOrderSeguimientoEstadoID, hdTipoVista.Value.ToString());

                        listaSeguimientos = new List<object>();

                        foreach (var oSeg in listaObjSeguimientos)
                        {

                            List<Data.CoreWorkOrderSeguimientos> listaObjSubSeguimientos = cCoreWorkOrdersSeguimientos.GetSubSeguimientos(oSeg.CoreWorkOrderSeguimientoID);

                            listaSubseguimiento = new List<object>();

                            foreach (var oSub in listaObjSubSeguimientos)
                            {
                                listaObjDocumentos = cCoreWorkOrdersSeguimientosDocumentos.GetDocumentosFromSeguimiento(oSub.CoreWorkOrderSeguimientoID);

                                listaDocumentos = new List<object>();

                                foreach (var oDoc in listaObjDocumentos)
                                {
                                    oDocumento = new
                                    {
                                        DocumentoID = oDoc.DocumentoID,
                                        Nombre = oDoc.Documento,
                                        Ruta = Path.Combine(DirectoryMapping.GetTempDirectory(oDoc.DocumentTipos.DocumentTipo) + oDoc.Archivo)
                                    };
                                    listaDocumentos.Add(oDocumento);
                                }

                                if (oSub.EsCambio)
                                {
                                    sTipo = "Estado";
                                }
                                else if (listaDocumentos.Count > 0)
                                {
                                    sTipo = "Documento";
                                }
                                else
                                {
                                    sTipo = "Comentario";
                                }

                                oSubSeguimiento = new
                                {
                                    SeguimientoID = oSub.CoreWorkOrderSeguimientoID,
                                    Nombre = oSub.Usuarios.Nombre,
                                    Imagen = "/_temp/imgUsuarios/" + oSub.UsuarioID + ".jpg",
                                    Proyecto = oSub.Usuarios.NombreCompleto,
                                    Fecha = oSub.Fecha.ToString(Comun.FORMATO_FECHA),
                                    Comentarios = oSub.Nota,
                                    Editado = oSub.Editado,
                                    Documentos = listaDocumentos,
                                    Tipo = sTipo
                                };

                                listaSubseguimiento.Add(oSubSeguimiento);
                            }

                            listaObjDocumentos = cCoreWorkOrdersSeguimientosDocumentos.GetDocumentosFromSeguimiento(oSeg.CoreWorkOrderSeguimientoID);

                            listaDocumentos = new List<object>();

                            foreach (var oDoc in listaObjDocumentos)
                            {
                                oDocumento = new
                                {
                                    DocumentoID = oDoc.DocumentoID,
                                    Nombre = oDoc.Documento,
                                    Ruta = Path.Combine(DirectoryMapping.GetTempDirectory(oDoc.DocumentTipos.DocumentTipo) + oDoc.Archivo)
                                };
                                listaDocumentos.Add(oDocumento);
                            }

                            if (oSeg.EsCambio)
                            {
                                sTipo = "Estado";
                            }
                            else if (listaDocumentos.Count > 0)
                            {
                                sTipo = "Documento";
                            }
                            else
                            {
                                sTipo = "Comentario";
                            }

                            oSeguimiento = new
                            {
                                SeguimientoID = oSeg.CoreWorkOrderSeguimientoID,
                                Nombre = oSeg.Usuarios.Nombre,
                                Imagen = "/_temp/imgUsuarios/" + oSeg.UsuarioID + ".jpg",
                                Proyecto = oSeg.Usuarios.NombreCompleto,
                                Fecha = oSeg.Fecha.ToString(Comun.FORMATO_FECHA),
                                Comentarios = oSeg.Nota,
                                Editado = oSeg.Editado,
                                Documentos = listaDocumentos,
                                SubSeguimientos = listaSubseguimiento,
                                TieneComentarios = (listaSubseguimiento.Count > 0),
                                Tipo = sTipo
                            };

                            listaSeguimientos.Add(oSeguimiento);
                        }

                        oDato.Add("Seguimientos", listaSeguimientos);
                        lista.Add(oDato);
                    }

                    if (lista != null)
                    {
                        storeSeguimientosAnteriores.DataSource = lista;
                        storeSeguimientosAnteriores.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TAREAS

        protected void storeTareas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject oDato;
                    List<JsonObject> lista = new List<JsonObject>();

                    oDato = new JsonObject();
                    oDato.Add("TareasID", 1);
                    oDato.Add("Nombre", "Tarea 1");
                    oDato.Add("Pagina", "Test1.aspx");
                    oDato.Add("Hecho", "checked");
                    lista.Add(oDato);

                    oDato = new JsonObject();
                    oDato.Add("TareasID", 2);
                    oDato.Add("Nombre", "Tarea 2");
                    oDato.Add("Pagina", "Test2.aspx");
                    oDato.Add("Hecho", "checked");
                    lista.Add(oDato);

                    oDato = new JsonObject();
                    oDato.Add("TareasID", 3);
                    oDato.Add("Nombre", "Tarea 3");
                    oDato.Add("Pagina", "Test3.aspx");
                    oDato.Add("Hecho", "");
                    lista.Add(oDato);


                    if (lista != null)
                    {
                        storeTareas.DataSource = lista;
                        storeTareas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region WORKORDESESTADOS

        protected void storeWorkOrderEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    CoreWorkOrdersController cCoreWorkOrders = new CoreWorkOrdersController();
                    Data.CoreWorkOrders oDato = cCoreWorkOrders.GetItem(long.Parse(hdWorkOrderID.Value.ToString()));
                    CoreWorkOrdersEstadosController cEstados = new CoreWorkOrdersEstadosController();
                    List<Data.CoreWorkOrdersEstados> listaDatos = cEstados.GetActivos(oDato.Proyectos.ClienteID);

                    if (listaDatos != null)
                    {
                        storeWorkOrderEstados.DataSource = listaDatos;
                        storeWorkOrderEstados.DataBind();
                        cmbEstadosWorkOrder.SetValue(oDato.CoreWorkOrderEstadoID);
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS DOCUMENTOS

        protected class DocTipo
        {
            public long DocumentTipoID { get; set; }
            public string DocumentTipo { get; set; }
            public string Extensiones { get; set; }
        }

        protected void storeTiposDocumentos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<DocTipo> docTipos = new List<DocTipo>();
                    CoreWorkOrdersController cCoreWorkOrders = new CoreWorkOrdersController();
                    Data.CoreWorkOrders oDato = cCoreWorkOrders.GetItem(long.Parse(hdWorkOrderID.Value.ToString()));
                    DocumentTiposController cTipos = new DocumentTiposController();
                    var listaDatos = cTipos.GetActivos(oDato.Proyectos.ClienteID);
                    if (listaDatos != null)
                    {
                        foreach (var item in listaDatos)
                        {
                            docTipos.Add(new DocTipo
                            {
                                DocumentTipo = item.DocumentTipo,
                                DocumentTipoID = item.DocumentTipoID,
                                Extensiones = String.Join(",", cTipos.GetExtByDocTipoID(item.DocumentTipoID).Select(c => "." + c.Extension).ToArray())
                            });
                        }
                        storeTiposDocumentos.DataSource = docTipos;
                        storeTiposDocumentos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region USUARIOS

        protected void storeUsuariosReasignar_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    var lista = cUsuarios.UsuariosValidosEstado(long.Parse(hdEstadoActID.Value.ToString()), long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeUsuariosReasignar.DataSource = lista;
                        storeUsuariosReasignar.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHODS

        #region ESTADOS

        [DirectMethod]
        public DirectResponse CambiarEstadoWorkOrder()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";

                CoreWorkOrdersController cCoreWorkOrders = new CoreWorkOrdersController();
                Data.CoreWorkOrders oDato = cCoreWorkOrders.GetItem(long.Parse(hdWorkOrderID.Value.ToString()));

                oDato.CoreWorkOrderEstadoID = long.Parse(cmbEstadosWorkOrder.Value.ToString());

                if (cCoreWorkOrders.UpdateItem(oDato))
                {
                    log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse PasarEstadoWorkOrder()
        {
            CoreWorkOrdersSeguimientosEstadosController cEstadosSeg = new CoreWorkOrdersSeguimientosEstadosController();
            CoreWorkOrdersController cWorkOrders = new CoreWorkOrdersController();
            cWorkOrders.SetDataContext(cEstadosSeg.Context);
            CoreWorkOrderSeguimientosController cSeguimientos = new CoreWorkOrderSeguimientosController();
            cSeguimientos.SetDataContext(cEstadosSeg.Context);
            DirectResponse direct = new DirectResponse();
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    direct.Success = true;
                    direct.Result = "";
                    Data.CoreWorkOrdersSeguimientosEstados oDato = new Data.CoreWorkOrdersSeguimientosEstados
                    {
                        CoreWorkOrderID = long.Parse(hdWorkOrderID.Value.ToString()),
                        CoreEstadoID = long.Parse(hdEstadoSiguienteID.Value.ToString()),
                        CoreEstadoAnteriorID = long.Parse(hdEstadoActID.Value.ToString()),
                        UsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                        Fecha = DateTime.Now
                    };
                    if ((oDato = cEstadosSeg.AddItem(oDato)) != null)
                    {
                        Data.CoreWorkOrders oWorkOrder = cWorkOrders.GetItem(long.Parse(hdWorkOrderID.Value.ToString()));
                        oWorkOrder.CoreWorkOrderSeguimientoEstadoID = oDato.CoreWorkOrderSeguimientoEstadoID;
                        if (!cWorkOrders.UpdateItem(oWorkOrder))
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                        Data.CoreWorkOrderSeguimientos oSeguimientos = new Data.CoreWorkOrderSeguimientos
                        {
                            UsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                            Fecha = DateTime.Now,
                            Nota = txtComentarioSiguienteEstado.Text,
                            Activo = true,
                            Editado = false,
                            EsCambio = true,
                            CoreWorkOrderSeguimientoEstadoID = oDato.CoreWorkOrderSeguimientoEstadoID
                        };
                        if (cSeguimientos.AddItem(oSeguimientos) == null)
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                        trans.Complete();
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    else
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                }
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse CambiarUsuarioEstado()
        {
            CoreWorkOrdersSeguimientosEstadosController cEstadosSeg = new CoreWorkOrdersSeguimientosEstadosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                Data.CoreWorkOrdersSeguimientosEstados oDato = cEstadosSeg.GetItem(long.Parse(hdEstadoActID.Value.ToString()));
                oDato.UsuarioID = long.Parse(cmbUsuariosReasignar.Value.ToString());
                if (!cEstadosSeg.UpdateItem(oDato))
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    return direct;
                }
                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        #region SEGUIMIENTOS

        [DirectMethod]
        public DirectResponse AñadirNuevoComentario()
        {
            CoreWorkOrderSeguimientosController cSeguimientos = new CoreWorkOrderSeguimientosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                Data.CoreWorkOrderSeguimientos oDato = new Data.CoreWorkOrderSeguimientos();
                oDato.CoreWorkOrderSeguimientoEstadoID = long.Parse(hdEstadoActID.Value.ToString());
                oDato.UsuarioID = ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID;
                oDato.Fecha = DateTime.Now;
                oDato.Nota = txtaComentario.Text;
                oDato.Activo = true;
                oDato.Editado = false;
                oDato.EsCambio = false;

                if (cSeguimientos.AddItem(oDato) != null)
                {
                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse AñadirNuevoSubComentario(string sSeguimientoPadre, string sNota)
        {
            CoreWorkOrderSeguimientosController cSeguimientos = new CoreWorkOrderSeguimientosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                Data.CoreWorkOrderSeguimientos oDato = new Data.CoreWorkOrderSeguimientos();
                oDato.CoreWorkOrderSeguimientoEstadoID = long.Parse(hdEstadoActID.Value.ToString());
                oDato.UsuarioID = ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID;
                oDato.Fecha = DateTime.Now;
                oDato.Nota = sNota;
                oDato.Activo = true;
                oDato.Editado = false;
                oDato.EsCambio = false;
                oDato.CoreWorkOrderSeguimientoPadreID = long.Parse(sSeguimientoPadre);

                if (cSeguimientos.AddItem(oDato) != null)
                {
                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse EliminarComentario(string lSeguimientoID)
        {
            CoreWorkOrderSeguimientosController cSeguimientos = new CoreWorkOrderSeguimientosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                Data.CoreWorkOrderSeguimientos oDato = cSeguimientos.GetItem(long.Parse(lSeguimientoID));
                oDato.Activo = false;
                if (cSeguimientos.UpdateItem(oDato))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse EditarComentario(string lSeguimientoID, string sNota)
        {
            CoreWorkOrderSeguimientosController cSeguimientos = new CoreWorkOrderSeguimientosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                Data.CoreWorkOrderSeguimientos oDato = cSeguimientos.GetItem(long.Parse(lSeguimientoID));
                oDato.Editado = true;
                oDato.Nota = sNota;
                if (cSeguimientos.UpdateItem(oDato))
                {
                    log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse DesactivarDocumento(long DocumentoID)
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            Data.Documentos doc;

            DocumentosController cDoc = new DocumentosController();
            HistoricoCoreDocumentosController cHistoricoCoreDocumentos = new HistoricoCoreDocumentosController();
            cHistoricoCoreDocumentos.SetDataContext(cDoc.Context);


            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    doc = cDoc.GetItem(DocumentoID);
                    doc.Activo = !doc.Activo;
                    doc.UltimaVersion = false;
                    if (!cDoc.UpdateItem(doc))
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                    if (!cHistoricoCoreDocumentos.addHistorico(doc, Usuario.UsuarioID))
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                }
            }
            return direct;
        }

        #endregion

        #endregion

        #region FUNCTIONS

        [DirectMethod]
        public DirectResponse CargarEstadoActual(long WorkOrderID)
        {
            CoreWorkOrdersSeguimientosEstadosController cEstados = new CoreWorkOrdersSeguimientosEstadosController();
            DirectResponse direct = new DirectResponse
            {
                Success = true,
                Result = ""
            };
            try
            {
                Data.CoreWorkOrdersSeguimientosEstados oEstado = cEstados.GetEstadoActual(WorkOrderID);
               // lblNombreTipologia.Text = oEstado.CoreEstados.CoreTipologias.Nombre;
                imgUsuarioActual.Src = "/_temp/imgUsuarios/" + oEstado.UsuarioID + ".jpg";
                lblNombreUsuarioActual.Text = oEstado.Usuarios.Nombre;
                lblEquipoUsuarioActual.Text = oEstado.Usuarios.NombreCompleto;
                lblFechaSeguimiento.Text = oEstado.Fecha.ToString(Comun.FORMATO_FECHA);
                lblEstadoActual.Text = oEstado.CoreEstados.Nombre;
                btnEstadoActual.Text = oEstado.CoreEstados.Nombre;
                lblDescripcionEstadoActual.Text = oEstado.CoreEstados.Descripcion;
                hdEstadoActID.Value = oEstado.CoreWorkOrderSeguimientoEstadoID;
              //  hdCliID.Value = oEstado.CoreEstados.CoreTipologias.ClienteID;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        protected void GenerarSiguientesEstados(long lWorkOrderID)
        {
            CoreWorkOrdersSeguimientosEstadosController cEstadosSeg = new CoreWorkOrdersSeguimientosEstadosController();
            EstadosController cEstados = new EstadosController();
            try
            {
                Data.CoreWorkOrdersSeguimientosEstados oEstado = cEstadosSeg.GetEstadoActual(lWorkOrderID);
                List<Data.CoreEstados> listaEstados = cEstados.getSiguientesEstadosByUsuario(oEstado.CoreEstadoID, ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                if (listaEstados != null && listaEstados.Count > 0)
                {
                    Ext.Net.MenuItem button;
                    foreach (var oEstadoSig in listaEstados)
                    {
                        button = new Ext.Net.MenuItem
                        {
                            Text = oEstadoSig.Nombre,
                            Handler = "SiguienteEstado(this)"
                        };
                        button.CustomConfig.Add(new ConfigItem("EstadoID", oEstadoSig.CoreEstadoID));
                        mnuSiguienteEstado.Items.Add(button);
                    }
                }
                else
                {
                    btnSiguienteEstado.Hidden = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion

    }
}