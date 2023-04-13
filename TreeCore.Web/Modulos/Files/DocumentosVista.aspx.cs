using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.PaginasComunes.pages
{
    public partial class DocumentosVista : TreeCore.Page.BasePageExtNet
    {
        public long? lObjetoID = null;
        public string sObjetoTipo = null;
        public long? lClienteID = null;
        public static long DocumentoIDPanelLateral = 0;
        public static long DocumentoIDVersion = 0;
        public long lModuloID = 0;
        public long UsuarioID = 0;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        private static bool visor = false;
        private static string imageUnknownFile = "../ima/ico-UnknownFile-195x350.svg#toolbar=0";
        private long tamanoMaximoDescarga = 0;
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private String identificadorDoc = "documento-";

        //Shared
        private string sSlug;

        #region const
        private string classIconTipoDoucment = "btnTiposDocs";
        private string ES_DOCUMENTO = "EsDocumento";
        #endregion

        #region Gestión Página (Init/Load)

        private void Page_Init(object sender, EventArgs e)
        {
            ParametrosController cParametros = new ParametrosController();
            DocumentosController cDocumentos = new DocumentosController();
            DocumentTiposController cDocumentTipos = new DocumentTiposController();
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
            EstadisticasController cEstadisticas = new EstadisticasController();
            cDocumentTipos.SetDataContext(cDocumentos.Context);
            cParametros.SetDataContext(cDocumentos.Context);
            cProyectosTipos.SetDataContext(cDocumentos.Context);
            cEstadisticas.SetDataContext(cDocumentos.Context);

            if (Request.Params["ObjetoID"] != null)
            {
                lObjetoID = long.Parse(Request.Params["ObjetoID"]);
                hdObjetoID.Value = lObjetoID.ToString();
            }
            if (Request.Params["ObjetoTipo"] != null)
            {
                sObjetoTipo = Request.Params["ObjetoTipo"].ToString();
                hdObjetoTipo.Value = sObjetoTipo;


                string sTitle = GetGlobalResource("str" + sObjetoTipo) + " " + GetGlobalResource("strDocumentos");
                grid.Title = sTitle;
            }
            else
            {
                grid.Title = GetGlobalResource("strDocumentos");
            }
            if (Request.Params["ProyectoTipo"] != null)
            {
                string ProyectoTipo = Request.Params["ProyectoTipo"];
                lModuloID = cProyectosTipos.getidProyectoTipo(ProyectoTipo);
                hdModuloID.Value = lModuloID.ToString();
            }
            if (Request.Params[Comun.VirtualPath.Shared.slug] != null)
            {
                sSlug = Request.Params[Comun.VirtualPath.Shared.slug];
            }

            if (this.Session["USUARIO"] != null)
            {
                Usuarios usuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                UsuarioID = usuario.UsuarioID;
                hdUsuarioID.Value = UsuarioID.ToString();

                #region REGISTRO DE ESTADISTICAS

                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, usuario.ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion
            }
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { "Tamano" };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                hdVisor.SetValue(0); // Modo Grid
                storeDocumentoLateral.RemoveAll();
                lblAsideNameR.Text = GetGlobalResource("strInfoDoc");
                //ArbolJerarquico();
                btnInfoVisor.Disabled = true;
                btnHistoricoVisor.Disabled = true;
                btnDescargarVisor.Disabled = true;
                btnDesactivarVisor.Disabled = true;
                btnTbHeaderDescargar.Disabled = true;
                btnDescargar.Disabled = true;

                if (!lObjetoID.HasValue)
                {
                    btnAgregarDocumento.SetHidden(true);
                }
            }

            #region Tamano maximo de carga
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            if (section != null)
            {
                hdMaxRequestLength.Value = section.MaxRequestLength;
            }
            #endregion

            //Asignación de cultura para devolver errores al subir un fichero
            hdCulture.Value = this._Locale;


            //TAMANO_MAXIMO_DESCARGA_DOCUMENTOS
            Parametros param = cParametros.GetItemByName(Comun.Parametros.TAMANO_MAXIMO_DESCARGA_DOCUMENTOS);

            tamanoMaximoDescarga = 1073741824;
            if (param != null)
            {
                tamanoMaximoDescarga = long.Parse(param.Valor);
            }

            hdTamanoMaximoDescarga.SetValue(tamanoMaximoDescarga);

            #region Navegación a documento desde URL
            if (RouteData != null && RouteData.Values.Count > 0 && RouteData.Values.ContainsKey(Comun.VirtualPath.Shared.action) && RouteData.Values.ContainsKey(Comun.VirtualPath.Shared.slug))
            {
                string sAction = "";
                if (RouteData.Values.ContainsKey(Comun.VirtualPath.Shared.action))
                {
                    sAction = RouteData.Values[Comun.VirtualPath.Shared.action].ToString();
                }
                if (RouteData.Values.ContainsKey(Comun.VirtualPath.Shared.slug))
                {
                    sSlug = RouteData.Values[Comun.VirtualPath.Shared.slug].ToString();
                }

                //Ocultar botones para limitar la pagina a la visualización del documento
                btnCloseShowVisorTreeP.Hidden = true;
                GridbtnvistaGrid.Hidden = true;
                btnQuickFilters.Hidden = true;

                if (string.IsNullOrEmpty(sAction) || sAction.Equals(Comun.VirtualPath.Shared.Action.show))
                {
                    //Visualización del documento

                }
                else if (!IsPostBack && !RequestManager.IsAjaxRequest)
                {
                    #region Descarga documento
                    Vw_CoreDocumentos vwCoreDoc = cDocumentos.getDocumentoBySlug(sSlug, Usuario.UsuarioID);

                    Vw_Documentos vwDoc;
                    Documentos doc;

                    if (vwCoreDoc != null)
                    {
                        DocumentosTiposRoles permisoTipDoc = cDocumentTipos.CompruebaPermisoDocumentoTipo(vwCoreDoc.DocumentTipoID.Value, Usuario.UsuarioID);


                        if (permisoTipDoc != null && permisoTipDoc.PermisoDescarga)
                        {
                            vwDoc = cDocumentos.GetItem<Vw_Documentos>(vwCoreDoc.DocumentoID);
                            doc = cDocumentos.GetItem<Documentos>(vwCoreDoc.DocumentoID);

                            if (vwDoc != null && doc != null)
                            {
                                string ruta = DirectoryMapping.GetTempDirectory(vwDoc.DocumentTipo);
                                string documento;
                                string nombreArchivo;

                                DocumentosVista.CopiarDocumentoDirectorioVisible(ruta, doc, vwDoc.DocumentTipo);

                                documento = Path.Combine(DirectoryMapping.GetTempDirectory(vwDoc.DocumentTipo), DocumentosVista.DocumentosNombreExtension(doc));


                                FileInfo file = new FileInfo(documento);
                                nombreArchivo = vwDoc.Documento;
                                HttpContext.Current.Response.Clear();
                                HttpContext.Current.Response.ContentType = Comun.GetMimeType(vwDoc.Extension);
                                nombreArchivo = nombreArchivo.Replace(" ", "_");

                                if (Path.GetExtension(nombreArchivo) != String.Empty)
                                {
                                    Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);
                                }
                                else
                                {
                                    Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + "." + vwDoc.Extension);
                                }

                                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                                HttpContext.Current.Response.TransmitFile(documento);
                                HttpContext.Current.Response.Flush();

                            }
                        }
                        else
                        {
                            X.Js.Call("docNotPermitDownload");
                        }

                    }
                    else
                    {
                        X.Js.Call("docNotFound");
                    }
                    #endregion
                }
            }
            #endregion

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_DocumentosVista_Total))
            //{
            //    btnDescargarVisor.Hidden = false;
            //    btnTbHeaderDescargar.Hidden = false;
            //    btnDescargar.Hidden = false;
            //    lbTgDocumentsActivos.Hidden = false;
            //    btnTgDocumentsActivos.Hidden = false;
            //}
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { lbTgDocumentsActivos, btnTgDocumentsActivos } },
                { "Download", new List<ComponentBase> { btnDescargar, btnDescargarVisor, btnTbHeaderDescargar }},
                { "Post", new List<ComponentBase> { }},
                { "Put", new List<ComponentBase> { }},
                { "Delete", new List<ComponentBase> { }}
            };
        }

        #endregion

        #region DIRECT METHODS

        #region CargarDocumentoVisor
        [DirectMethod]
        public DirectResponse CargarDocumentoVisor(long docID, bool permisoLectura)
        {
            DirectResponse direct = new DirectResponse();
            string ProyectoTipo = "";

            try
            {

                Vw_Documentos doc = null;
                DocumentosController cDocs = new DocumentosController();
                DocumentTiposController cDocumentosTipos = new DocumentTiposController();
                ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
                ParametrosController cParametros = new ParametrosController();
                cDocumentosTipos.SetDataContext(cDocs.Context);
                cProyectosTipos.SetDataContext(cDocs.Context);
                cParametros.SetDataContext(cDocs.Context);

                doc = cDocs.GetItem<Vw_Documentos>(docID);

                if (doc != null)
                {
                    DocumentosTiposRoles permiso = cDocumentosTipos.CompruebaPermisoDocumentoTipo(doc.DocumentTipoID.Value, Usuario.UsuarioID);

                    if (permiso != null)
                    {
                        permisoLectura = permiso.PermisoLectura;

                        if (permiso.PermisoDescarga)
                        {
                            btnDescargarVisor.Enable();
                        }
                        else
                        {
                            btnDescargarVisor.Disable();
                        }
                    }

                    btnAgregarDocumento.Disabled = true;
                    if (permisoLectura)
                    {
                        if (lModuloID != 0)
                        {
                            ProyectoTipo = cProyectosTipos.GetItem(lModuloID).ProyectoTipo;
                        }


                        if (doc != null)
                        {
                            btnEditarDocumento.Disabled = false;
                            btnDesactivar.Disabled = false;
                        }
                        else
                        {
                            btnEditarDocumento.Disabled = true;
                            btnDesactivar.Disabled = true;
                        }
                        Documentos docExtension = cDocs.GetItem(docID);
                        string ruta = DirectoryMapping.GetTempDirectory(doc.DocumentTipo);

                        string EXTENSIONES_VISOR = "";
                        EXTENSIONES_VISOR = cParametros.GetItemValor(Comun.Parametros.EXTENSIONES_VISOR);
                        string[] extensiones_visor = EXTENSIONES_VISOR.Split(',');
                        bool extensionVisualizable = false;

                        if (docExtension != null)
                        {
                            foreach (string ext in extensiones_visor)
                            {
                                if (String.Compare(docExtension.Extension, ext, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    extensionVisualizable = true;
                                }
                            }

                            if (extensionVisualizable)
                            {
                                CopiarDocumentoDirectorioVisible(ruta, docExtension, doc.DocumentTipo);

                                if (doc != null)
                                {
                                    string pre = (string.IsNullOrEmpty(sSlug)) ? "" : "../";

                                    PanelVisorMain.Loader.SuspendScripting();
                                    PanelVisorMain.Loader.Url = pre + "../_temp/" + doc.DocumentTipo + "/" + DocumentosNombreExtension(docExtension) + "#toolbar=0";
                                    PanelVisorMain.Loader.DisableCaching = true;
                                    PanelVisorMain.LoadContent();
                                    labelVisor.Text = doc.Documento;
                                    lblAsideNameR.Text = doc.Documento;
                                    lblAsideNameR.IconCls = getIconoByExtension(doc.Extension);
                                }
                            }
                            else
                            {
                                // Extensión no visualizable. Cargar imagen que lo indique.
                                PanelVisorMain.Loader.SuspendScripting();
                                PanelVisorMain.Loader.Url = imageUnknownFile;
                                PanelVisorMain.Loader.DisableCaching = true;
                                PanelVisorMain.LoadContent();
                                labelVisor.Text = doc.Documento;
                                lblAsideNameR.Text = doc.Documento;
                                lblAsideNameR.IconCls = getIconoByExtension(doc.Extension);
                            }
                        }
                    }
                    else
                    {
                        // No tiene permiso de lectura
                        PanelVisorMain.Loader.SuspendScripting();
                        PanelVisorMain.Loader.Url = imageUnknownFile;
                        PanelVisorMain.Loader.DisableCaching = true;
                        PanelVisorMain.LoadContent();
                        labelVisor.Text = doc.Documento;
                        lblAsideNameR.Text = doc.Documento;
                        lblAsideNameR.IconCls = getIconoByExtension(doc.Extension);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                PanelVisorMain.Loader.SuspendScripting();
                PanelVisorMain.Loader.Url = imageUnknownFile;
                PanelVisorMain.Loader.DisableCaching = true;
                PanelVisorMain.LoadContent();
                labelVisor.Text = "";
                return direct;
            }
            direct.Success = true;
            direct.Result = "";

            return direct;
        }
        #endregion

        #region QuitarDocumentoVisorMultiSelect
        [DirectMethod]
        public DirectResponse QuitarDocumentoVisorMultiSelect()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                // No tiene permiso de lectura
                PanelVisorMain.Loader.SuspendScripting();
                PanelVisorMain.Loader.Url = imageUnknownFile;
                PanelVisorMain.Loader.DisableCaching = true;
                PanelVisorMain.LoadContent();
                labelVisor.Text = "";
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);

                PanelVisorMain.Loader.SuspendScripting();
                PanelVisorMain.Loader.Url = imageUnknownFile;
                PanelVisorMain.Loader.DisableCaching = true;
                PanelVisorMain.LoadContent();
                labelVisor.Text = "";

                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }
        #endregion

        #region CopiarDocumentoDirectorioVisible
        public static void CopiarDocumentoDirectorioVisible(string ruta, Documentos documento, string documentTipo)
        {
            string rutaDestinoExtension = Path.Combine(ruta, DocumentosNombreExtension(documento));
            if (File.Exists(rutaDestinoExtension))
            {
                File.Delete(rutaDestinoExtension);
            }

            string res = DirectoryMapping.GetDocumentDirectory();
            res = Tree.Utiles.Utils.BarraPath(res) + documentTipo;
            res = Path.Combine(res, documento.Archivo);

            if (File.Exists(res))
            {
                File.Copy(res, rutaDestinoExtension);
            }
        }
        #endregion

        #region MoverDocumentoADocumentoTipo
        private bool MoverDocumentoADocumentoTipo(Documentos doc, DocumentTipos documentTipoOrigen, DocumentTipos documentTipoDestino)
        {
            bool movido = false;
            string nombreDoc = doc.Archivo;
            string res = DirectoryMapping.GetDocumentDirectory();

            string directorioDestino = Tree.Utiles.Utils.BarraPath(Tree.Utiles.Utils.BarraPath(res) + documentTipoDestino.DocumentTipo);
            string rutaOrigen = Tree.Utiles.Utils.BarraPath(Tree.Utiles.Utils.BarraPath(res) + documentTipoOrigen.DocumentTipo) + nombreDoc;
            string rutaDestino = directorioDestino + nombreDoc;



            try
            {
                if (File.Exists(rutaOrigen))
                {

                    if (!Directory.Exists(directorioDestino))
                    {
                        Directory.CreateDirectory(directorioDestino);
                    }

                    File.Move(rutaOrigen, rutaDestino);
                    movido = true;
                }
            }
            catch (Exception ex)
            {
                movido = false;
            }

            return movido;
        }
        #endregion

        #region RecargaPanelLateral
        [DirectMethod]
        public DirectResponse RecargaPanelLateral(long docID)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDoc = new DocumentosController();
            Vw_CoreDocumentos doc;
            JsonObject oDato = new JsonObject();
            string sMinuto = "";
            string sHora = "";

            try
            {
                DocumentoIDPanelLateral = docID;
                doc = cDoc.GetItem<Vw_CoreDocumentos>(docID);
                if (doc != null)
                {
                    lblAsideNameR.Text = doc.Documento;
                    lblAsideNameR.IconCls = getIconoByExtension(doc.Extension);
                    storeVersiones.Reload();

                    cDoc.GetObjectDataByDocumentID(doc.DocumentoID, out long? id, out string tipo);
                    string sObjectCode = cDoc.GetObjectCodeByObjectID(id, tipo);
                    string sObjectName = cDoc.GetObjectNameByObjectID(id, tipo);

                    Documentos docum = cDoc.GetItem(doc.DocumentoID);
                    string tipoObj = cDoc.getObjetoTipo(docum);

                    oDato.Add(GetGlobalResource("strNombre"), doc.Documento);
                    oDato.Add(GetGlobalResource("strExtension"), doc.Extension);
                    oDato.Add(GetGlobalResource("strEstado"), doc.NombreEstado);

                    if (tipoObj == Comun.ObjetosTipos.Emplazamiento)
                    {
                        oDato.Add(GetGlobalResource("strObjetoTipo"), GetGlobalResource("strEmplazamiento"));
                    }
                    else if (tipoObj == Comun.ObjetosTipos.InventarioElemento)
                    {
                        oDato.Add(GetGlobalResource("strObjetoTipo"), GetGlobalResource("strInventario"));
                    }

                    oDato.Add(GetGlobalResource("strCodigoObjeto"), sObjectCode);
                    oDato.Add(GetGlobalResource("strNombreObjeto"), sObjectName);
                    oDato.Add(GetGlobalResource("strCreador"), doc.Creador);
                    oDato.Add(GetGlobalResource("strVersion_"), doc.Version);
                    //oDato.Add(GetGlobalResource("strProyectoTipo"), doc.Alias);
                    //oDato.Add(GetGlobalResource("strProyecto"), doc.Proyecto);
                    if (doc.FechaDocumento.HasValue)
                    {
                        oDato.Add(GetGlobalResource("strFecha"), Comun.DateTimeToString(doc.FechaDocumento.Value));

                        if (doc.FechaDocumento.Value.Hour < 10)
                        {
                            sHora = "0" + doc.FechaDocumento.Value.Hour.ToString();
                        }
                        else
                        {
                            sHora = doc.FechaDocumento.Value.Hour.ToString();
                        }

                        if (doc.FechaDocumento.Value.Minute < 10)
                        {
                            sMinuto = "0" + doc.FechaDocumento.Value.Minute.ToString();
                        }
                        else
                        {
                            sMinuto = doc.FechaDocumento.Value.Minute.ToString();
                        }

                        oDato.Add(GetGlobalResource("strHora"), sHora + ":" + sMinuto);
                    }
                    if (doc.Tamano.HasValue)
                    {
                        oDato.Add(GetGlobalResource("strTamano"), SizeSuffix(doc.Tamano.Value));
                    }

                    Documentos docTem = cDoc.GetItem(doc.DocumentoID);
                    if (docTem != null && !string.IsNullOrEmpty(docTem.Observaciones))
                    {
                        oDato.Add(GetGlobalResource("strDescripcion"), docTem.Observaciones);
                    }


                    direct.Success = true;
                    direct.Result = oDato;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region DesactivarDocumento
        [DirectMethod]
        public DirectResponse DesactivarDocumento(long DocumentoVersionID, bool esVersion)
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;

            DocumentosController cDoc = new DocumentosController();

            try
            {
                RowSelectionModel sm = grid.GetSelectionModel() as RowSelectionModel;

                List<long> listDocumentoID = new List<long>();
                if (DocumentoVersionID != 0)
                {
                    listDocumentoID.Add(DocumentoVersionID);
                }
                else
                {
                    if (!string.IsNullOrEmpty(sm.SelectedRecordID))
                    {
                        listDocumentoID.Add(long.Parse(sm.SelectedRecordID));
                    }
                    else
                    {

                        for (int i = 0; i < sm.SelectedRows.Count; i++)
                        {
                            listDocumentoID.Add(long.Parse(sm.SelectedRows[i].RecordID));
                        }
                    }
                }

                InfoResponse infoResponse = cDoc.DesactivarDocumento(listDocumentoID, esVersion, Usuario);
                if (infoResponse.Result)
                {
                    direct.Result = "";
                    direct.Success = true;
                }
                else
                {
                    direct.Result = infoResponse.Description;
                    direct.Success = false;
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            storeVersiones.Reload();
            return direct;
        }
        #endregion

        #region RestaurarDocumento
        [DirectMethod]
        public DirectResponse RestaurarDocumento(long docID)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDoc = new DocumentosController();

            try
            {
                InfoResponse infoResponse = cDoc.RestaurarDocumento(docID, lModuloID, Usuario);
                if (infoResponse.Result)
                {
                    direct.Success = infoResponse.Result;
                }
                else
                {
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region DescargarDocumentoSeleccionado
        [DirectMethod]
        public void DescargarDocumentoSeleccionado(long docID)
        {

            DocumentosController cDoc = new DocumentosController();
            Vw_CoreDocumentos vwDoc;
            Documentos doc;

            try
            {
                vwDoc = cDoc.GetItem<Vw_CoreDocumentos>(docID);
                doc = cDoc.GetItem<Documentos>(docID);

                if (vwDoc != null && doc != null)
                {
                    string ruta = DirectoryMapping.GetTempDirectory(vwDoc.DocumentTipo);
                    string documento;
                    string nombreArchivo;

                    CopiarDocumentoDirectorioVisible(ruta, doc, vwDoc.DocumentTipo);

                    documento = Path.Combine(DirectoryMapping.GetTempDirectory(vwDoc.DocumentTipo), DocumentosNombreExtension(doc));


                    FileInfo file = new FileInfo(documento);
                    nombreArchivo = vwDoc.Documento;
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = Comun.GetMimeType(vwDoc.Extension);
                    nombreArchivo = nombreArchivo.Replace(" ", "_");

                    if (Path.GetExtension(nombreArchivo) != String.Empty)
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + ConcatenarIdetificadorMasNombre(vwDoc.Identificador, nombreArchivo));
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + ConcatenarIdetificadorMasNombre(vwDoc.Identificador, nombreArchivo + "." + vwDoc.Extension));
                    }

                    HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                    HttpContext.Current.Response.TransmitFile(documento);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region DescargarDocumentosSeleccionados
        [DirectMethod]
        public DirectResponse DescargarDocumentosSeleccionados(string sJson)
        {
            DirectResponse direct = new DirectResponse();

            DocumentosController cDocumentos = new DocumentosController();
            DocumentTiposController cDocumentTipos = new DocumentTiposController();

            try
            {
                List<long> idsTiposDoc = new List<long>();
                List<long> idsDoc = new List<long>();
                List<long> idsOtros = new List<long>();
                long tamanoTemp = 0;


                Dictionary<string, List<MyFileInfo>> FilesInfo = new Dictionary<string, List<MyFileInfo>>();

                #region Lectura JSON
                JObject json = JObject.Parse(sJson);
                JArray array = (JArray)json["elementos"];

                List<JObject> listItemsJson = array.Select(obj => (JObject)obj).ToList();
                listItemsJson.ForEach(itemJson =>
                {
                    long id = 0;
                    long? DocumentoTipoID = null;
                    bool EsCarpeta = false;
                    bool EsDocumento = false;

                    if (itemJson.TryGetValue("id", out JToken iDToken) && iDToken != null)
                    {
                        id = long.Parse(iDToken.ToString());
                    }
                    if (itemJson.TryGetValue("DocumentoTipoID", out JToken docTipoIDToken) && docTipoIDToken != null)
                    {
                        DocumentoTipoID = long.Parse(docTipoIDToken.ToString());
                    }
                    if (itemJson.TryGetValue("EsCarpeta", out JToken esCarpetaToken) && esCarpetaToken != null)
                    {
                        EsCarpeta = bool.Parse(esCarpetaToken.ToString());
                    }
                    if (itemJson.TryGetValue("EsDocumento", out JToken esDocumentoToken) && esDocumentoToken != null)
                    {
                        EsDocumento = bool.Parse(esDocumentoToken.ToString());
                    }

                    if (EsCarpeta)
                    {
                        idsTiposDoc.Add(id);
                    }
                    else if (EsDocumento)
                    {
                        idsDoc.Add(id);
                    }
                    else
                    {
                        idsOtros.Add(id);
                    }
                });
                #endregion

                if (idsTiposDoc != null && idsTiposDoc.Count == 0 && idsDoc != null && idsDoc.Count == 1)
                {
                    #region Descargar solo un documento
                    DescargarDocumentoSeleccionado(idsDoc[0]);
                    #endregion
                }
                else
                {
                    #region Comprobar permisos sobre TipoDocumento
                    List<DocumentTipos> tiposDocConPermiso = cDocumentTipos.GetTiposPermisoDescarga(idsTiposDoc, UsuarioID);
                    List<Vw_Documentos> documentosConPermiso = cDocumentos.GetDocumentosConPermisoDescarga(idsDoc, UsuarioID);

                    if (documentosConPermiso != null)
                    {
                        documentosConPermiso.ForEach(doc =>
                        {
                            doc.Observaciones = doc.DocumentTipo + "/";
                        });
                    }
                    else
                    {
                        documentosConPermiso = new List<Vw_Documentos>();
                    }

                    string tipoObj = null;
                    long? objID = null;
                    if (lObjetoID.HasValue)
                    {
                        objID = lObjetoID;
                        tipoObj = sObjetoTipo;
                    }

                    //Tipos de documentos
                    tiposDocConPermiso.ForEach(typeDocument =>
                    {
                        List<Vw_Documentos> documentosConPermisoByTipo = cDocumentos.GetDocumentosConPermisoDescargaByTipo(UsuarioID, Usuario.ClienteID.Value, typeDocument, "", objID, tipoObj);

                        if (documentosConPermisoByTipo != null && documentosConPermisoByTipo.Count > 0)
                        {
                            documentosConPermiso.AddRange(documentosConPermisoByTipo);
                        }
                    });

                    //Documentos Sueltos
                    documentosConPermiso.ForEach(vwDoc =>
                    {
                        Documentos doc = cDocumentos.GetItem(vwDoc.DocumentoID);
                        Vw_CoreDocumentos coreDoc = cDocumentos.GetItem<Vw_CoreDocumentos>(vwDoc.DocumentoID);

                        string ruta = DirectoryMapping.GetTempDirectory(vwDoc.DocumentTipo);
                        CopiarDocumentoDirectorioVisible(ruta, doc, vwDoc.DocumentTipo);
                        string documento = Path.Combine(DirectoryMapping.GetTempDirectory(vwDoc.DocumentTipo), DocumentosNombreExtension(doc));
                        if (File.Exists(documento))
                        {
                            if (!FilesInfo.ContainsKey(vwDoc.Observaciones))
                            {
                                FilesInfo.Add(vwDoc.Observaciones, new List<MyFileInfo>());
                            }

                            FilesInfo[vwDoc.Observaciones].Add(new MyFileInfo()
                            {
                                FileInfo = new FileInfo(documento),
                                nameToDownload = ConcatenarIdetificadorMasNombre(coreDoc.Identificador, coreDoc.Documento)
                            });
                            if (vwDoc.Tamano.HasValue)
                            {
                                tamanoTemp += vwDoc.Tamano.Value;
                            }
                        }
                    });
                    #endregion

                    if (tamanoMaximoDescarga >= tamanoTemp)
                    {

                        #region Crear Zip
                        string pathDirZIP = DirectoryMapping.GetTempDirectory("ZIP");
                        long nameFile = DateTime.Now.Ticks;
                        string nameFileZIP = nameFile + ".zip";
                        string pathZIP = pathDirZIP + "/" + nameFileZIP;
                        string pathCarpeta = pathDirZIP + "/" + nameFile;


                        if (Directory.Exists(pathCarpeta))
                        {
                            Directory.Delete(pathCarpeta, true);
                        }
                        Directory.CreateDirectory(pathCarpeta);


                        FilesInfo.Keys.ToList().ForEach(docType =>
                        {
                            string carpetaTemp = pathCarpeta + "/" + docType;
                            Directory.CreateDirectory(carpetaTemp);

                            FilesInfo[docType].ForEach(myFileInfo =>
                            {
                                FileInfo fileInfo = myFileInfo.FileInfo;
                                string nombreArchivo = carpetaTemp + "/" + myFileInfo.nameToDownload + fileInfo.Extension;

                                fileInfo.CopyTo(nombreArchivo, true);
                            });
                        });

                        ZipFile.CreateFromDirectory(pathCarpeta, pathZIP);

                        #endregion

                        #region Descargar ZIP
                        Response.AddHeader("content-disposition", "attachment; filename=" + nameFileZIP);

                        FileInfo file = new FileInfo(pathZIP);

                        HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                        HttpContext.Current.Response.TransmitFile(pathZIP);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                        #endregion

                        direct.Success = true;
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsTamanoDocumentoExcedido);
                    }
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
        #endregion

        #region WinChangeNameFileBotonGuardar
        [DirectMethod]
        public DirectResponse WinChangeNameFileBotonGuardar(long docID)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            HistoricoCoreDocumentosController cHistoricoCoreDocumentos = new HistoricoCoreDocumentosController();

            try
            {
                Documentos documento = cDocumentos.GetItem(docID);

                if (documento != null && !string.IsNullOrEmpty(txtNameFile.Value.ToString()))
                {
                    bool nombrePermitido = true;
                    string sObjetoTipo = cDocumentos.getObjetoTipo(documento);
                    long ObjetoID = cDocumentos.getObjetoTipoID(documento, sObjetoTipo);

                    List<Vw_CoreDocumentos> coincidencias = cDocumentos.HasDocumentWhitNameAndDocumentTypeAndObjectType(txtNameFile.Value.ToString(), documento.DocumentTipoID.Value, ObjetoID, sObjetoTipo, null);
                    
                    coincidencias.ForEach(coincidencia => {
                        if (coincidencia.DocumentoID != docID)
                        {
                            nombrePermitido = false;
                        }
                    });

                    if (nombrePermitido)
                    {
                        documento.Documento = txtNameFile.Value.ToString();

                        if (txtDescripcion.Value != null)
                        {
                            documento.Observaciones = txtDescripcion.Value.ToString();
                        }
                        else
                        {
                            documento.Observaciones = "";
                        }

                        documento.DocumentoEstadoID = Convert.ToInt64(cmbDocumentosEstados.Value);

                        InfoResponse inforesponse = cDocumentos.Update(documento);
                        if (inforesponse != null && inforesponse.Result)
                        {
                            direct.Success = true;

                            cHistoricoCoreDocumentos.addHistorico(documento, UsuarioID);

                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strNombreYaExisteParaElObjeto);
                    }
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
                return direct;
            }

            return direct;
        }
        #endregion

        #region winChangeStautsMultipleBotonGuardar
        [DirectMethod]
        public DirectResponse winChangeStautsMultipleBotonGuardar()
        {
            DirectResponse direct = new DirectResponse();

            try
            {




            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region MoverDocumentos
        [DirectMethod]
        public DirectResponse MoverDocumentos(string[] idDocumentosMover, string sDocumentTipoID)
        {
            DirectResponse direct = new DirectResponse();

            DocumentosController cDocumentos = new DocumentosController();
            DocumentTiposController cDocumentTipos = new DocumentTiposController();
            DocumentosTiposExtensionesController cDocumentosTiposExtensiones = new DocumentosTiposExtensionesController();
            HistoricoCoreDocumentosController cHistoricoCoreDocumentos = new HistoricoCoreDocumentosController();
            cDocumentTipos.SetDataContext(cDocumentos.Context);
            cDocumentosTiposExtensiones.SetDataContext(cDocumentos.Context);
            cHistoricoCoreDocumentos.SetDataContext(cDocumentos.Context);

            bool documentosNoMovidos = false;

            try
            {

                if (idDocumentosMover != null && idDocumentosMover.Length > 0 && !string.IsNullOrEmpty(sDocumentTipoID))
                {
                    long documentTipoIDDestino = long.Parse(sDocumentTipoID);
                    List<long> ids = new List<long>();

                    foreach (string sID in idDocumentosMover)
                    {
                        ids.Add(long.Parse(sID));
                    }

                    try
                    {
                        List<Documentos> documentos = cDocumentos.getDocumentos(ids);
                        DocumentTipos tipoDestino = cDocumentTipos.GetItem(documentTipoIDDestino);
                        DocumentosTiposRoles permisoDestino = cDocumentTipos.CompruebaPermisoDocumentoTipo(tipoDestino.DocumentTipoID, UsuarioID);

                        if (tipoDestino != null && permisoDestino != null && permisoDestino.PermisoEscritura)
                        {
                            documentos.ForEach(doc =>
                            {

                                DocumentTipos tipoOrigen = cDocumentTipos.GetItem(doc.DocumentTipoID.Value);
                                DocumentosTiposRoles permisoTipDoc = cDocumentTipos.CompruebaPermisoDocumentoTipo(tipoOrigen.DocumentTipoID, UsuarioID);

                                if (permisoTipDoc != null && permisoTipDoc.PermisoEscritura &&
                                    permisoDestino.PermisoLectura == permisoTipDoc.PermisoLectura &&
                                    permisoDestino.PermisoDescarga == permisoTipDoc.PermisoDescarga)
                                {
                                    bool extensionPermitida = false;
                                    List<Vw_DocumentosTiposExtensiones> extensionesPermitidas = cDocumentosTiposExtensiones.ExtensionesPermitidasByTipoDocumento(tipoDestino.DocumentTipoID);
                                    foreach (Vw_DocumentosTiposExtensiones ext in extensionesPermitidas)
                                    {
                                        if (String.Compare(doc.Extension, ext.Extension, StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            extensionPermitida = true;
                                        }
                                    }

                                    if (extensionPermitida)
                                    {
                                        if (MoverDocumentoADocumentoTipo(doc, tipoOrigen, tipoDestino))
                                        {
                                            doc.DocumentTipoID = documentTipoIDDestino;
                                            cDocumentos.Update(doc);

                                            cHistoricoCoreDocumentos.addHistorico(doc, UsuarioID);
                                            cHistoricoCoreDocumentos.SubmitChanges();
                                        }
                                    }
                                    else
                                    {
                                        documentosNoMovidos = true;
                                    }
                                }
                                else
                                {
                                    documentosNoMovidos = true;
                                }
                            });
                        }
                        else
                        {
                            documentosNoMovidos = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                }

                direct.Success = true;
                direct.Result = (documentosNoMovidos) ? GetGlobalResource(Comun.strDocumentosNoMovidosSatisfactorio) : ""/*GetGlobalResource(Comun.strDocumentosMovidosSatisfactorio)*/;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region LoaderMenuPegarComo
        [DirectMethod]
        public string LoaderMenuPegarComo(Dictionary<string, string> parameters)
        {

            List<Ext.Net.MenuItem> items = new List<MenuItem>();

            try
            {
                long idCarpeta = long.Parse(hdCarpetaPadreID.Value.ToString());
                string[] docIDs = hdDocumentosIDsCopiados.ToString().Split(',');

                List<DocumentTipos> lista = getListaDocumentosTiposPorCarpeta(idCarpeta, UsuarioID);

                if (lista != null && lista.Count > 0)
                {
                    lista.ForEach(item =>
                    {
                        items.Add(new Ext.Net.MenuItem(item.DocumentTipo)
                        {
                            ID = item.DocumentTipoID.ToString(),
                            Text = item.DocumentTipo
                        });
                    });
                    PasteFile.SetHidden(false);
                }
                else
                {
                    PasteFile.SetHidden(true);
                }
            }
            catch (Exception ex)
            {
                items = new List<MenuItem>();
                log.Error(ex);
            }

            return ComponentLoader.ToConfig(items);
        }
        #endregion

        #region LoaderMenuCambiarEstado
        [DirectMethod]
        public string LoaderMenuCambiarEstado(Dictionary<string, string> parameters)
        {
            List<Ext.Net.MenuItem> items = new List<MenuItem>();

            try
            {
                DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();
                List<DocumentosEstados> tipos = cDocumentosEstados.GetActivos(ClienteID.Value);

                if (tipos != null)
                {
                    tipos.ForEach(tipo =>
                    {
                        items.Add(new Ext.Net.MenuItem(tipo.Nombre)
                        {
                            ID = tipo.DocumentoEstadoID.ToString(),
                            Text = tipo.Nombre
                        });
                    });
                    mnCambiarEstadoMulti.SetHidden(false);
                }
                else
                {
                    mnCambiarEstadoMulti.SetHidden(true);
                }
            }
            catch (Exception ex)
            {
                items = new List<MenuItem>();
                log.Error(ex);
            }

            return ComponentLoader.ToConfig(items);
        }
        #endregion

        #region CambiarEstadoMulti
        [DirectMethod]
        public DirectResponse CambiarEstadoMulti(string[] sIDDocumentos, string sDocumentEstadoID)
        {
            DirectResponse direct = new DirectResponse();
            bool todosCambiados = true;

            if (sIDDocumentos != null && sIDDocumentos.Length > 0 && !string.IsNullOrEmpty(sDocumentEstadoID))
            {
                DocumentosController cDocumentos = new DocumentosController();
                DocumentTiposController cDocumentTipos = new DocumentTiposController();
                HistoricoCoreDocumentosController cHistoricoCoreDocumentos = new HistoricoCoreDocumentosController();
                cDocumentTipos.SetDataContext(cDocumentos.Context);
                cHistoricoCoreDocumentos.SetDataContext(cDocumentos.Context);

                long documentEstadoID = long.Parse(sDocumentEstadoID);
                List<long> idDocumentos = new List<long>();

                foreach (string sID in sIDDocumentos)
                {
                    idDocumentos.Add(long.Parse(sID));
                }

                try
                {
                    List<Documentos> documentos = cDocumentos.getDocumentos(idDocumentos);
                    documentos.ForEach(doc =>
                    {

                        DocumentosTiposRoles permisoDestino = cDocumentTipos.CompruebaPermisoDocumentoTipo(doc.DocumentTipoID.Value, UsuarioID);

                        if (permisoDestino != null && permisoDestino.PermisoEscritura)
                        {
                            doc.DocumentoEstadoID = documentEstadoID;
                            cDocumentos.Update(doc);

                            cHistoricoCoreDocumentos.addHistorico(doc, UsuarioID);

                        }
                        else
                        {
                            todosCambiados = false;
                        }
                    });

                    direct.Success = true;
                    direct.Result = (todosCambiados) ? "" : GetGlobalResource("strAlgunosDocumentosNoModificados");
                }
                catch (Exception ex)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }
            else
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            return direct;
        }
        #endregion

        #region CargarEditarMetadatos
        [DirectMethod]
        public DirectResponse CargarEditarMetadatos(long docID)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();

            try
            {
                Documentos doc = cDocumentos.GetItem(docID);

                if (doc != null)
                {
                    txtNameFile.SetValue(doc.Documento);
                    txtDescripcion.SetValue(doc.Observaciones);
                    if (doc.DocumentoEstadoID.HasValue)
                    {
                        cmbDocumentosEstados.SetValue(doc.DocumentoEstadoID);
                    }
                }

                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region cargarPredictivoBuscador
        [DirectMethod]
        public DirectResponse cargarPredictivoBuscador()
        {

            DirectResponse direct = new DirectResponse();
            DocumentTiposController cDocumentTipos = new DocumentTiposController();

            try
            {
                bool showDocsInactivos = btnTgDocumentsActivos.Pressed;
                Dictionary<long, List<Vw_CoreDocumentos>> documentos;

                documentos = GetAllDocumentForSearchBox(showDocsInactivos);

                List<JsonObject> listaJson = new List<JsonObject>();


                if (documentos != null)
                {
                    documentos.Keys.ToList().ForEach(key =>
                    {
                        List<Vw_CoreDocumentos> documentosTemp = documentos[key];

                        if (documentosTemp != null)
                        {
                            //DocumentosTiposRoles permiso = cDocumentTipos.CompruebaPermisoDocumentoTipo(key, UsuarioID);

                            documentosTemp.ForEach(documento =>
                            {
                                JsonObject oJson = new JsonObject();

                                #region Asignación de campos
                                oJson.Add("id", documento.DocumentoID);
                                //oJson.Add("idElemento", identificadorDoc + documento.DocumentoID);
                                oJson.Add("Icono", getIconoByExtension(documento.Extension));
                                oJson.Add("Nombre", documento.Documento);
                                oJson.Add("Extension", documento.Extension);
                                //oJson.Add("Alias", documento.Alias);
                                //oJson.Add("Proyecto", documento.ProyectoID);
                                //oJson.Add("Creador", documento.Creador);
                                //oJson.Add("CreadorID", documento.CreadorID);
                                //oJson.Add("Fecha", documento.FechaDocumento);
                                //oJson.Add("Estado", documento.NombreEstado);
                                //oJson.Add("EstadoID", documento.DocumentoEstadoID);
                                //oJson.Add("Version", documento.Version);
                                //oJson.Add("DocumentoPadreID", documento.DocumentoPadreID);
                                //oJson.Add("EmplazamientoID", documento.EmplazamientoID);
                                //oJson.Add("Activo", documento.Activo);
                                //oJson.Add("Tamano", documento.Tamano);
                                //oJson.Add("EsDocumento", true);
                                //oJson.Add("tipoDocumento", false);
                                //oJson.Add("slug", documento.Slug);
                                //oJson.Add("DocumentoTipoID", documento.DocumentTipoID);
                                //oJson.Add("DocumentoTipo", documento.DocumentTipo);
                                //oJson.Add("EsCarpeta", false);
                                //oJson.Add("Codigo", documento.Identificador);
                                oJson.Add("Descripcion", documento.Observaciones);

                                #region Tipo objeto
                                //string objetoTipo = "";
                                //if (documento.UsuarioID.HasValue)
                                //{
                                //    objetoTipo = "strUsuario";
                                //}
                                //else if (documento.EmplazamientoID.HasValue)
                                //{
                                //    objetoTipo = "strEmplazamiento";
                                //}
                                //else if (documento.InventarioElementoID.HasValue)
                                //{
                                //    objetoTipo = "strInventarioElemento";
                                //}

                                //if (!string.IsNullOrEmpty(objetoTipo))
                                //{
                                //    oJson.Add("ObjetoTipo", GetGlobalResource(objetoTipo));
                                //}
                                #endregion

                                #region Permisos
                                //if (permiso != null)
                                //{
                                //    oJson.Add("Lectura", permiso.PermisoLectura);
                                //    oJson.Add("Escritura", permiso.PermisoEscritura);
                                //    oJson.Add("Descarga", permiso.PermisoDescarga);
                                //}
                                #endregion 

                                #endregion

                                listaJson.Add(oJson);
                            });
                        }

                    });
                }

                direct.Result = JSON.Serialize(listaJson).ToString();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        public Dictionary<long, List<Vw_CoreDocumentos>> GetAllDocumentForSearchBox(bool showDocsInactivos)
        {
            DocumentosController cDocumentos = new DocumentosController();

            Dictionary<long, List<Vw_CoreDocumentos>> documentos;

            if (lObjetoID.HasValue)
            {
                documentos = cDocumentos.GetVwCoreByObjetoID(lObjetoID.Value, sObjetoTipo, showDocsInactivos, UsuarioID);
            }
            else if (sSlug != null)
            {
                documentos = cDocumentos.toDictionary(cDocumentos.GetDocumentosBySlug(sSlug), UsuarioID);
            }
            else if (ClienteID.HasValue)
            {
                documentos = cDocumentos.GetAllByUsuario(showDocsInactivos, UsuarioID);
            }
            else
            {
                documentos = new Dictionary<long, List<Vw_CoreDocumentos>>();
            }

            return documentos;
        }
        #endregion

        #endregion

        #region STORES
        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int curPage = e.Page - 1;
                    int pageSize = Convert.ToInt32(cmbPagination.Value);
                    int total = 0;

                    string filter = e.Parameters["filter"];


                    List<JsonObject> listaJson;
                    listaJson = listaPrincipal(curPage, pageSize, out total, filter, e.Sort);
                    e.Total = total;

                    //filtrarStoresFiltros(listaJson);

                    storePrincipal.DataSource = listaJson;
                    storePrincipal.DataBind();

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<JsonObject> listaPrincipal(int curPage, int pageSize, out int total, string fc, DataSorter[] sorters)
        {
            bool showDocsInactivos = btnTgDocumentsActivos.Pressed;
            bool desdeSlug = false;
            List<DocumentTipos> carpetas = new List<DocumentTipos>();
            List<Vw_CoreDocumentos> listDocumentos = new List<Vw_CoreDocumentos>();
            Dictionary<long, List<Vw_CoreDocumentos>> documentos = new Dictionary<long, List<Vw_CoreDocumentos>>();

            hdCarpetaActual.SetValue(hdCarpetaPadreID.Value.ToString());

            #region GetDocumentos
            GetAllDocuments(out listDocumentos, listDocumentos, out carpetas, carpetas, out documentos, documentos, out desdeSlug, desdeSlug, showDocsInactivos);
            #endregion

            #region Filtro lateral
            if (documentos != null && documentos.Count > 0)
            {
                documentos = filtroRapidoDocumentosDictionary(documentos);
            }
            else if (listDocumentos != null && listDocumentos.Count > 0)
            {
                listDocumentos = filtroRapidoDocumentosList(listDocumentos);
            }
            #endregion

            List<JsonObject> listaJson = createListaJson(carpetas, documentos, listDocumentos);

            #region Filtro de columna
            listaJson = LinqEngine.filtroJson(listaJson, fc);

            listaJson = LinqEngine.SortJson(listaJson, sorters);
            #endregion

            #region Paginación
            total = listaJson.Count;
            listaJson = listaJson.Skip(curPage * pageSize).Take(pageSize).ToList();
            #endregion

            return listaJson;
        }

        private void GetAllDocuments(out List<Vw_CoreDocumentos> outListDocumentos, List<Vw_CoreDocumentos> listDocumentos,
            out List<DocumentTipos> outCarpetas, List<DocumentTipos> carpetas,
            out Dictionary<long, List<Vw_CoreDocumentos>> outDocumentos, Dictionary<long, List<Vw_CoreDocumentos>> documentos,
            out bool outDesdeSlug, bool desdeSlug, bool showDocsInactivos)
        {
            DocumentTiposController cDocumentTipos = new DocumentTiposController();
            DocumentosController cDocumentos = new DocumentosController();
            cDocumentos.SetDataContext(cDocumentTipos.Context);

            long? padre = null;
            if (long.Parse(hdCarpetaPadreID.Value.ToString()) != 0)
            {
                long.Parse(hdCarpetaPadreID.Value.ToString());
            }

            outListDocumentos = listDocumentos;
            outCarpetas = carpetas;
            outDocumentos = documentos;
            outDesdeSlug = desdeSlug;

            outDesdeSlug = false;

            string sIDDocumentoBuscado = hdIdDocumentBuscador.Value.ToString();
            string sBuscador = hdStringBuscador.Value.ToString();

            if (!string.IsNullOrEmpty(sIDDocumentoBuscado) || !string.IsNullOrEmpty(sBuscador))
            {
                if (!string.IsNullOrEmpty(sIDDocumentoBuscado))
                {
                    Vw_CoreDocumentos docTemp = cDocumentos.GetItem<Vw_CoreDocumentos>(long.Parse(sIDDocumentoBuscado));
                    if (docTemp != null)
                    {
                        outListDocumentos.Add(docTemp);
                    }
                }
                else
                {
                    List<Vw_CoreDocumentos> docsTemp = cDocumentos.GetDocumentosByNombreAndDescripcion(sBuscador, showDocsInactivos, sSlug, lObjetoID, sObjetoTipo, ClienteID);
                    if (docsTemp != null)
                    {
                        outListDocumentos.AddRange(docsTemp);
                    }
                }
                outCarpetas = null;
            }
            else if (sSlug != null)
            {
                outListDocumentos = cDocumentos.GetDocumentosBySlug(sSlug);
                outCarpetas = cDocumentTipos.GetCarpetasByPadre(ClienteID.Value, padre, showDocsInactivos);
                long idDoc = (outListDocumentos != null && outListDocumentos.Count > 0) ? outListDocumentos[0].DocumentoID : -1;
                
                X.Js.Call("abrirVisorSlug", new object[] { idDoc });

                outDesdeSlug = true;
            }
            else if (lObjetoID.HasValue)
            {
                if (long.Parse(hdCarpetaPadreID.Value.ToString()) != 0)
                {

                    outCarpetas = cDocumentTipos.GetCarpetasByPadre(ClienteID.Value, long.Parse(hdCarpetaPadreID.Value.ToString()), showDocsInactivos);
                    outDocumentos = cDocumentos.GetVwCoreByObjetoID(lObjetoID.Value, sObjetoTipo, showDocsInactivos, UsuarioID, long.Parse(hdCarpetaPadreID.Value.ToString()));
                }
                else
                {

                    outCarpetas = cDocumentTipos.GetCarpetasByPadre(ClienteID.Value, padre, showDocsInactivos);
                    outDocumentos = cDocumentos.GetVwCoreByObjetoID(lObjetoID.Value, sObjetoTipo, showDocsInactivos, UsuarioID, padre);
                }
            }
            else if (ClienteID.HasValue)
            {
                if (long.Parse(hdCarpetaPadreID.Value.ToString()) != 0)
                {
                    outCarpetas = cDocumentTipos.GetCarpetasByPadre(ClienteID.Value, long.Parse(hdCarpetaPadreID.Value.ToString()), showDocsInactivos);
                    outDocumentos = cDocumentos.GetByCarpeta(long.Parse(hdCarpetaPadreID.Value.ToString()), showDocsInactivos, UsuarioID);
                }
                else
                {
                    outCarpetas = cDocumentTipos.GetCarpetasByPadre(ClienteID.Value, padre, showDocsInactivos);
                    outDocumentos = cDocumentos.GetByCarpeta(padre, showDocsInactivos, UsuarioID);
                }
            }
            else
            {
                outDocumentos = new Dictionary<long, List<Vw_CoreDocumentos>>();
                outCarpetas = new List<DocumentTipos>();
            }
        }

        private List<JsonObject> createListaJson(List<DocumentTipos> carpetas, Dictionary<long, List<Vw_CoreDocumentos>> DictionarioDocumentos, List<Vw_CoreDocumentos> documentos)
        {
            DocumentTiposController cDocumentTipos = new DocumentTiposController();

            List<JsonObject> listaJson = new List<JsonObject>();
            JsonObject oJson;

            try
            {

                if (documentos != null)
                {
                    //DictionarioDocumentos
                    documentos.ForEach(doc =>
                    {
                        if (!DictionarioDocumentos.ContainsKey(doc.DocumentTipoID.Value))
                        {
                            DictionarioDocumentos.Add(doc.DocumentTipoID.Value, new List<Vw_CoreDocumentos>());
                        }

                        DictionarioDocumentos[doc.DocumentTipoID.Value].Add(doc);
                    });
                }

                #region CARPETAS
                if (carpetas != null)
                {
                    carpetas.ForEach(carpeta =>
                    {
                        oJson = new JsonObject();

                        oJson.Add("id", carpeta.DocumentTipoID);
                        oJson.Add("idElemento", "carpeta-" + carpeta.DocumentTipoID);
                        oJson.Add("Icono", "/ima/ico-folder.svg");
                        oJson.Add("Nombre", carpeta.DocumentTipo);
                        oJson.Add("Codigo", carpeta.Codigo);
                        //oJson.Add("ObjetoTipo", carpeta.ObjetoTipo);
                        //oJson.Add("Extension", carpeta.Extension);
                        //oJson.Add("Alias", carpeta.Alias);
                        //oJson.Add("Proyecto", carpeta.Proyecto);
                        //oJson.Add("Creador", carpeta.Creador);
                        //oJson.Add("Fecha", carpeta.Fecha);
                        //oJson.Add("Estado", carpeta.Estado);
                        //oJson.Add("EstadoID", carpeta.EstadoID);
                        //oJson.Add("Version", carpeta.Version);
                        oJson.Add("DocumentoPadreID", carpeta.SuperDocumentTipoID);
                        //oJson.Add("EmplazamientoID", carpeta.EmplazamientoID);
                        oJson.Add("Activo", carpeta.Activo);
                        //oJson.Add("Tamano", carpeta.Tamano);
                        oJson.Add("EsDocumento", false);
                        oJson.Add("tipoDocumento", carpeta.DocumentTipo);
                        //oJson.Add("slug", carpeta.slug);
                        oJson.Add("DocumentoTipoID", carpeta.DocumentTipoID);
                        oJson.Add("EsCarpeta", carpeta.EsCarpeta);
                        //oJson.Add("Descripcion", carpeta.Descripcion);

                        listaJson.Add(oJson);
                    });
                }
                #endregion

                #region DOCUMENTOS
                if (DictionarioDocumentos != null)
                {
                    DictionarioDocumentos.Keys.ToList().ForEach(key =>
                    {
                        List<Vw_CoreDocumentos> documentosTemp = DictionarioDocumentos[key];

                        if (documentosTemp != null)
                        {
                            DocumentosTiposRoles permiso = cDocumentTipos.CompruebaPermisoDocumentoTipo(key, UsuarioID);

                            documentosTemp.ForEach(documento =>
                            {
                                oJson = new JsonObject();

                                #region Asignación de campos
                                oJson.Add("id", documento.DocumentoID);
                                oJson.Add("idElemento", identificadorDoc + documento.DocumentoID);
                                oJson.Add("Icono", getIconoByExtension(documento.Extension));
                                oJson.Add("Nombre", documento.Documento);
                                oJson.Add("Extension", documento.Extension);
                                oJson.Add("Alias", documento.Alias);
                                oJson.Add("Proyecto", documento.ProyectoID);
                                oJson.Add("Creador", documento.Creador);
                                oJson.Add("CreadorID", documento.CreadorID);
                                oJson.Add("Fecha", documento.FechaDocumento);
                                oJson.Add("Estado", documento.NombreEstado);
                                oJson.Add("EstadoID", documento.DocumentoEstadoID);
                                oJson.Add("Version", documento.Version);
                                oJson.Add("DocumentoPadreID", documento.DocumentoPadreID);
                                oJson.Add("EmplazamientoID", documento.EmplazamientoID);
                                oJson.Add("Activo", documento.Activo);
                                oJson.Add("Tamano", documento.Tamano);
                                oJson.Add("EsDocumento", true);
                                oJson.Add("tipoDocumento", false);
                                oJson.Add("slug", documento.Slug);
                                oJson.Add("DocumentoTipoID", documento.DocumentTipoID);
                                oJson.Add("DocumentoTipo", documento.DocumentTipo);
                                oJson.Add("EsCarpeta", false);
                                oJson.Add("Codigo", documento.Identificador);
                                oJson.Add("Descripcion", documento.Observaciones);
                                oJson.Add("NombreObjeto", documento.NombreObjeto);

                                #region Tipo objeto
                                string objetoTipo = "";
                                if (documento.UsuarioID.HasValue)
                                {
                                    objetoTipo = GetGlobalResource("strUsuario");
                                }
                                else if (documento.EmplazamientoID.HasValue)
                                {
                                    objetoTipo = GetGlobalResource("strEmplazamiento");
                                }
                                else if (documento.InventarioElementoID.HasValue)
                                {
                                    if (!string.IsNullOrEmpty(documento.InventarioCategoria))
                                    {
                                        objetoTipo = documento.InventarioCategoria;
                                    }
                                    else
                                    {
                                        objetoTipo = GetGlobalResource("strInventarioElemento");
                                    }
                                }

                                if (!string.IsNullOrEmpty(objetoTipo))
                                {
                                    oJson.Add("ObjetoTipo", objetoTipo);
                                }
                                #endregion

                                #region Permisos
                                if (permiso != null)
                                {
                                    oJson.Add("Lectura", permiso.PermisoLectura);
                                    oJson.Add("Escritura", permiso.PermisoEscritura);
                                    oJson.Add("Descarga", permiso.PermisoDescarga);
                                }
                                #endregion 

                                #endregion

                                listaJson.Add(oJson);
                            });
                        }

                    });
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaJson = new List<JsonObject>();
            }

            return listaJson;
        }


        #region Clientes
        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes;

                    listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
        #endregion

        protected void storeDocumentoLateral_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentosController cDocumentos = new DocumentosController();

                try
                {
                    long DocumentoID = 0;
                    Vw_Documentos doc = null;
                    List<Vw_Documentos> lDoc = new List<Vw_Documentos>();

                    DocumentoID = DocumentoIDPanelLateral;

                    if (DocumentoID != 0)
                    {
                        doc = cDocumentos.GetItem<Vw_Documentos>(DocumentoID);
                    }

                    if (doc != null)
                    {
                        lDoc.Add(doc);
                        storeDocumentoLateral.DataSource = lDoc;
                        storeDocumentoLateral.DataBind();
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        protected void storeVersiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentosController cDocumentos = new DocumentosController();

                try
                {
                    long DocumentoID = 0;
                    Vw_Documentos doc = null;

                    DocumentoID = DocumentoIDPanelLateral;

                    bool inactivos = btnTgDocumentsActivos.Pressed;

                    if (DocumentoID != 0)
                    {
                        doc = cDocumentos.GetItem<Vw_Documentos>(DocumentoID);

                        List<Vw_Documentos> versiones = new List<Vw_Documentos>();
                        List<object> datos = new List<object>();
                        if (doc != null)
                        {
                            versiones = cDocumentos.GetVersiones(doc, inactivos);
                        }
                        if (versiones != null)
                        {
                            versiones.ForEach(ver =>
                            {
                                datos.Add(new
                                {
                                    DocumentoID = ver.DocumentoID,
                                    Documento = ver.Documento,
                                    Fecha = (ver.FechaDocumento.HasValue) ? ((Comun.DateTimeToString(ver.FechaDocumento.Value) + " | " + ver.FechaDocumento.Value.ToString("HH:mm"))) : "",
                                    Version = ver.Version,
                                    Creador = ver.Creador,
                                    UltimaVersion = ver.UltimaVersion,
                                    Activo = ver.Activo,
                                    Tamano = ver.Tamano
                                });
                            });
                            storeVersiones.DataSource = datos;
                            storeVersiones.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #region FILTROS RAPIDOS
        protected void storeFiltCreador_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    List<Usuarios> lista = cUsuarios.getUsuariosCreadorDocumentos(ClienteID.Value);


                    if (lista != null)
                    {
                        storeFiltCreador.DataSource = lista;
                        storeFiltCreador.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void storeFiltExtension_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    DocumentosExtensionesController cDocumentosExtensiones = new DocumentosExtensionesController();
                    List<DocumentosExtensiones> lista = cDocumentosExtensiones.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        storeFiltExtension.DataSource = lista;
                        storeFiltExtension.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void storeFiltEstado_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();
                    List<DocumentosEstados> lista = cDocumentosEstados.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        storeFiltEstado.DataSource = lista;
                        storeFiltEstado.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void storeFiltDocumentoTipo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    DocumentTiposController cDocumentTipos = new DocumentTiposController();
                    List<DocumentTipos> lista = cDocumentTipos.GetDocumentosTiposUsuario(UsuarioID);


                    if (lista != null)
                    {
                        lista = lista.FindAll(i => !i.EsCarpeta);

                        storeFiltDocumentoTipo.DataSource = lista;
                        storeFiltDocumentoTipo.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TiposDocumentos
        protected void storeTiposDocumentos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentTiposController cDocumentTipos = new DocumentTiposController();

                try
                {
                    
                    long? idCarpeta = long.Parse(hdCarpetaPadreID.Value.ToString());
                    idCarpeta = (idCarpeta == 0) ? null : idCarpeta;
                    List<DocumentTipos> lista = getListaDocumentosTiposPorCarpeta(idCarpeta, UsuarioID);

                    if (lista != null)
                    {
                        storeTiposDocumentos.DataSource = lista;
                        storeTiposDocumentos.DataBind();
                    }
                    
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<DocumentTipos> getListaDocumentosTiposPorCarpeta(long? idCarpeta, long usuarioID)
        {
            DocumentTiposController cDocumentTipos = new DocumentTiposController();

            List<DocumentTipos> lista = cDocumentTipos.GetDocumentosTiposBySuperDocumentTipoPermisoEscritura(idCarpeta, usuarioID);

            return lista;
        }
        #endregion

        #region DocumentosEstados
        protected void storeDocumentosEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();

                try
                {
                    List<DocumentosEstados> lista = cDocumentosEstados.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        storeDocumentosEstados.DataSource = lista;
                        storeDocumentosEstados.DataBind();
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

        #region FUNCTIONS


        private Dictionary<long, List<Vw_CoreDocumentos>> filtroRapidoDocumentosDictionary(Dictionary<long, List<Vw_CoreDocumentos>> documentos)
        {
            Dictionary<long, List<Vw_CoreDocumentos>> result = new Dictionary<long, List<Vw_CoreDocumentos>>();

            List<string> filtDocumentTipoIDs = new List<string>();
            List<string> filtExtensiones = new List<string>();
            List<string> filtEstadosIDs = new List<string>();
            List<string> filtCreadoresIDs = new List<string>();

            try
            {
                if (documentos != null)
                {
                    documentos.Keys.ToList().ForEach(key =>
                    {
                        documentos[key].ForEach(documento =>
                        {
                            bool cumpleFiltro = true;

                            #region DocumentTipo
                            if (!filtDocumentTipoIDs.Contains(documento.DocumentTipoID.ToString()))
                            {
                                filtDocumentTipoIDs.Add(documento.DocumentTipoID.ToString());
                            }
                            if (cmbfiltDocumentoTipo.SelectedItems.Count > 0)
                            {
                                bool cumpleFiltroSingular = false;
                                for (int i = 0; i < cmbfiltDocumentoTipo.SelectedItems.Count; i++)
                                {
                                    long documentTipoID = long.Parse(cmbfiltDocumentoTipo.SelectedItems[i].Value);
                                    if (documento.DocumentTipoID == documentTipoID)
                                    {
                                        cumpleFiltroSingular = true;
                                    }
                                }
                                if (!cumpleFiltroSingular)
                                {
                                    cumpleFiltro = cumpleFiltroSingular;
                                }
                            }
                            #endregion

                            #region Extensiones
                            if (!filtExtensiones.Contains(documento.Extension))
                            {
                                filtExtensiones.Add(documento.Extension);
                            }
                            if (cmbfiltExtension.SelectedItems.Count > 0)
                            {
                                bool cumpleFiltroSingular = false;
                                for (int i = 0; i < cmbfiltExtension.SelectedItems.Count; i++)
                                {
                                    string extension = cmbfiltExtension.SelectedItems[i].Value;
                                    if (documento.Extension == extension)
                                    {
                                        cumpleFiltroSingular = true;
                                    }
                                }
                                if (!cumpleFiltroSingular)
                                {
                                    cumpleFiltro = cumpleFiltroSingular;
                                }
                            }
                            #endregion

                            #region Estados
                            if (!filtEstadosIDs.Contains(documento.DocumentoEstadoID.ToString()))
                            {
                                filtEstadosIDs.Add(documento.DocumentoEstadoID.ToString());
                            }
                            if (cmbfiltEstado.SelectedItems.Count > 0)
                            {
                                bool cumpleFiltroSingular = false;
                                for (int i = 0; i < cmbfiltEstado.SelectedItems.Count; i++)
                                {
                                    long estado = long.Parse(cmbfiltEstado.SelectedItems[i].Value);
                                    if (documento.DocumentoEstadoID == estado)
                                    {
                                        cumpleFiltroSingular = true;
                                    }
                                }
                                if (!cumpleFiltroSingular)
                                {
                                    cumpleFiltro = cumpleFiltroSingular;
                                }
                            }
                            #endregion

                            #region Creador
                            if (!filtCreadoresIDs.Contains(documento.CreadorID.ToString()))
                            {
                                filtCreadoresIDs.Add(documento.CreadorID.ToString());
                            }
                            if (cmbfiltCreador.SelectedItems.Count > 0)
                            {
                                bool cumpleFiltroSingular = false;
                                for (int i = 0; i < cmbfiltCreador.SelectedItems.Count; i++)
                                {
                                    long idCreador = long.Parse(cmbfiltCreador.SelectedItems[i].Value);
                                    if (documento.CreadorID == idCreador)
                                    {
                                        cumpleFiltroSingular = true;
                                    }
                                }
                                if (!cumpleFiltroSingular)
                                {
                                    cumpleFiltro = cumpleFiltroSingular;
                                }
                            }
                            #endregion

                            #region DateStart
                            if (datfiltDateStart.Value != null && !string.IsNullOrEmpty(datfiltDateStart.Value.ToString()) && Convert.ToDateTime(datfiltDateStart.Value) != DateTime.MinValue)
                            {
                                DateTime dateStart = Convert.ToDateTime(datfiltDateStart.Value);
                                if (documento.FechaDocumento < dateStart)
                                {
                                    cumpleFiltro = false;
                                }
                            }
                            #endregion

                            #region DateEnd
                            if (datfiltDateEnd.Value != null && !string.IsNullOrEmpty(datfiltDateEnd.Value.ToString()) && Convert.ToDateTime(datfiltDateEnd.Value) != DateTime.MinValue)
                            {
                                DateTime dateEnd = Convert.ToDateTime(datfiltDateEnd.Value);
                                if (documento.FechaDocumento > dateEnd)
                                {
                                    cumpleFiltro = false;
                                }
                            }
                            #endregion

                            if (cumpleFiltro)
                            {
                                if (!result.ContainsKey(key))
                                {
                                    result.Add(key, new List<Vw_CoreDocumentos>());
                                }
                                result[key].Add(documento);
                            }
                        });
                    });
                }

                #region Guardar IDs para filtro lateral
                string sFiltDocumentTipoIDs = "";
                string sFiltExtensiones = "";
                string sFiltEstadosIDs = "";
                string sFiltCreadoresIDs = "";

                filtDocumentTipoIDs.ForEach(s =>
                {
                    sFiltDocumentTipoIDs += ((sFiltDocumentTipoIDs != "") ? "," : "") + s;
                });
                filtExtensiones.ForEach(s =>
                {
                    sFiltExtensiones += ((sFiltExtensiones != "") ? "," : "") + s;
                });
                filtEstadosIDs.ForEach(s =>
                {
                    sFiltEstadosIDs += ((sFiltEstadosIDs != "") ? "," : "") + s;
                });
                filtCreadoresIDs.ForEach(s =>
                {
                    sFiltCreadoresIDs += ((sFiltCreadoresIDs != "") ? "," : "") + s;
                });

                hdFiltDocumentTipoIDs.SetValue(sFiltDocumentTipoIDs);
                hdFiltExtensiones.SetValue(sFiltExtensiones);
                hdFiltEstadosIDs.SetValue(sFiltEstadosIDs);
                hdFiltCreadoresIDs.SetValue(sFiltCreadoresIDs);
                #endregion
            }
            catch (Exception ex)
            {
                result = new Dictionary<long, List<Vw_CoreDocumentos>>();
            }

            return result;
        }

        private List<Vw_CoreDocumentos> filtroRapidoDocumentosList(List<Vw_CoreDocumentos> documentos)
        {
            List<Vw_CoreDocumentos> result = new List<Vw_CoreDocumentos>();

            List<string> filtDocumentTipoIDs = new List<string>();
            List<string> filtExtensiones = new List<string>();
            List<string> filtEstadosIDs = new List<string>();
            List<string> filtCreadoresIDs = new List<string>();

            try
            {
                if (documentos != null)
                {

                    documentos.ForEach(documento =>
                    {
                        bool cumpleFiltro = true;

                        #region DocumentTipo
                        if (!filtDocumentTipoIDs.Contains(documento.DocumentTipoID.ToString()))
                        {
                            filtDocumentTipoIDs.Add(documento.DocumentTipoID.ToString());
                        }
                        if (cmbfiltDocumentoTipo.SelectedItems.Count > 0)
                        {
                            bool cumpleFiltroSingular = false;
                            for (int i = 0; i < cmbfiltDocumentoTipo.SelectedItems.Count; i++)
                            {
                                long documentTipoID = long.Parse(cmbfiltDocumentoTipo.SelectedItems[i].Value);
                                if (documento.DocumentTipoID == documentTipoID)
                                {
                                    cumpleFiltroSingular = true;
                                }
                            }
                            if (!cumpleFiltroSingular)
                            {
                                cumpleFiltro = cumpleFiltroSingular;
                            }
                        }
                        #endregion

                        #region Extensiones
                        if (!filtExtensiones.Contains(documento.Extension))
                        {
                            filtExtensiones.Add(documento.Extension);
                        }
                        if (cmbfiltExtension.SelectedItems.Count > 0)
                        {
                            bool cumpleFiltroSingular = false;
                            for (int i = 0; i < cmbfiltExtension.SelectedItems.Count; i++)
                            {
                                string extension = cmbfiltExtension.SelectedItems[i].Value;
                                if (documento.Extension == extension)
                                {
                                    cumpleFiltroSingular = true;
                                }
                            }
                            if (!cumpleFiltroSingular)
                            {
                                cumpleFiltro = cumpleFiltroSingular;
                            }
                        }
                        #endregion

                        #region Estados
                        if (!filtEstadosIDs.Contains(documento.DocumentoEstadoID.ToString()))
                        {
                            filtEstadosIDs.Add(documento.DocumentoEstadoID.ToString());
                        }
                        if (cmbfiltEstado.SelectedItems.Count > 0)
                        {
                            bool cumpleFiltroSingular = false;
                            for (int i = 0; i < cmbfiltEstado.SelectedItems.Count; i++)
                            {
                                long estado = long.Parse(cmbfiltEstado.SelectedItems[i].Value);
                                if (documento.DocumentoEstadoID == estado)
                                {
                                    cumpleFiltroSingular = true;
                                }
                            }
                            if (!cumpleFiltroSingular)
                            {
                                cumpleFiltro = cumpleFiltroSingular;
                            }
                        }
                        #endregion

                        #region Creador
                        if (!filtCreadoresIDs.Contains(documento.CreadorID.ToString()))
                        {
                            filtCreadoresIDs.Add(documento.CreadorID.ToString());
                        }
                        if (cmbfiltCreador.SelectedItems.Count > 0)
                        {
                            bool cumpleFiltroSingular = false;
                            for (int i = 0; i < cmbfiltCreador.SelectedItems.Count; i++)
                            {
                                long idCreador = long.Parse(cmbfiltCreador.SelectedItems[i].Value);
                                if (documento.CreadorID == idCreador)
                                {
                                    cumpleFiltroSingular = true;
                                }
                            }
                            if (!cumpleFiltroSingular)
                            {
                                cumpleFiltro = cumpleFiltroSingular;
                            }
                        }
                        #endregion

                        #region DateStart
                        if (datfiltDateStart.Value != null && !string.IsNullOrEmpty(datfiltDateStart.Value.ToString()) && Convert.ToDateTime(datfiltDateStart.Value) != DateTime.MinValue)
                        {
                            DateTime dateStart = Convert.ToDateTime(datfiltDateStart.Value);
                            if (documento.FechaDocumento < dateStart)
                            {
                                cumpleFiltro = false;
                            }
                        }
                        #endregion

                        #region DateEnd
                        if (datfiltDateEnd.Value != null && !string.IsNullOrEmpty(datfiltDateEnd.Value.ToString()) && Convert.ToDateTime(datfiltDateEnd.Value) != DateTime.MinValue)
                        {
                            DateTime dateEnd = Convert.ToDateTime(datfiltDateEnd.Value);
                            if (documento.FechaDocumento > dateEnd)
                            {
                                cumpleFiltro = false;
                            }
                        }
                        #endregion

                        if (cumpleFiltro)
                        {
                            result.Add(documento);
                        }
                    });

                }

                #region Guardar IDs para filtro lateral
                string sFiltDocumentTipoIDs = "";
                string sFiltExtensiones = "";
                string sFiltEstadosIDs = "";
                string sFiltCreadoresIDs = "";

                filtDocumentTipoIDs.ForEach(s =>
                {
                    sFiltDocumentTipoIDs += ((sFiltDocumentTipoIDs != "") ? "," : "") + s;
                });
                filtExtensiones.ForEach(s =>
                {
                    sFiltExtensiones += ((sFiltExtensiones != "") ? "," : "") + s;
                });
                filtEstadosIDs.ForEach(s =>
                {
                    sFiltEstadosIDs += ((sFiltEstadosIDs != "") ? "," : "") + s;
                });
                filtCreadoresIDs.ForEach(s =>
                {
                    sFiltCreadoresIDs += ((sFiltCreadoresIDs != "") ? "," : "") + s;
                });

                hdFiltDocumentTipoIDs.SetValue(sFiltDocumentTipoIDs);
                hdFiltExtensiones.SetValue(sFiltExtensiones);
                hdFiltEstadosIDs.SetValue(sFiltEstadosIDs);
                hdFiltCreadoresIDs.SetValue(sFiltCreadoresIDs);
                #endregion
            }
            catch (Exception ex)
            {
                result = new List<Vw_CoreDocumentos>();
            }

            return result;
        }

        public static string DocumentosNombreExtension(Data.Documentos documento)
        {
            return documento.DocumentoID.ToString().PadLeft(5, '0') + "_" + documento.Documento + "." + documento.Extension;
        }

        public string getIconoByExtension(string Extension)
        {
            string cssClass = "/ima/";



            switch (Extension.ToLower())
            {
                case Comun.Documentos.Extensiones.PDF:
                    cssClass += Comun.Documentos.IconFile.PDF;
                    break;
                case Comun.Documentos.Extensiones.DOC:
                case Comun.Documentos.Extensiones.DOCX:
                    cssClass += Comun.Documentos.IconFile.WORD;
                    break;
                case Comun.Documentos.Extensiones.PPT:
                    cssClass += Comun.Documentos.IconFile.POWERPOINT;
                    break;
                case Comun.Documentos.Extensiones.XLS:
                case Comun.Documentos.Extensiones.XSLX:
                    cssClass += Comun.Documentos.IconFile.EXCEL;
                    break;
                default:
                    cssClass += Comun.Documentos.IconFile.OTHER;
                    break;
            }


            return cssClass;
        }

        private static string SizeSuffix(Int64 value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        public string ConcatenarIdetificadorMasNombre(string Identificador, string nombreDoc)
        {
            return Identificador + " - " + nombreDoc;
        }

        #endregion

    }

    public class ElementoGridDocumento
    {
        public string tipo { get; set; }
        public object obj { get; set; }
    }
    public class MyFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public string nameToDownload { get; set; }
    }
}