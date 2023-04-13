using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Web.UI;
using log4net;
using System.Reflection;
using System.Web;
using TreeCore.Data;
using System.Linq;
using Ext.Net.Utilities;

namespace TreeCore.Page
{
    /// <summary>
    /// Clase Base de Control de Acceso y Permisos
    /// </summary>
    /// 

    public class BasePageExtNet : System.Web.UI.Page
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string SesionGUID;
        public UsuariosSesiones UsuariosSesiones;
        public String paginaJS = "";
        public String directorioJs = "";
        public String NombrePagina = "";
        public TreeCore.Data.Usuarios Usuario;
        public TreeCore.Data.Accesos Accesos;
        public List<long> perfiles = new List<long>();
        public bool Autentificado = false;
        private bool _SesionDuplicada = false;
        private bool interno = false;
        private bool NoPrincipal = false;
        private bool _DesdeIpInterna = false;
        public long? ClienteID = null;
        public long? OperadorID = null;
        private Ext.Net.Theme _Tema = Comun.Tema("10");
        public static string sResultadoKPIid = ""; 
        public string IdsResultadosKPI;
        public List<long> listIdsResultadosKPI;
        public string nameIndiceID;

        public BasePageExtNet()
        {
            this.Init += new System.EventHandler(this.Page_Init);
            Ip = Context.Request.Params.Get("REMOTE_ADDR");

            HttpCookie SesionGUIDCookie = Cookies.GetCookie("SesionGUID");
            if ((SesionGUIDCookie != null) && !string.IsNullOrEmpty(SesionGUIDCookie.Value))
            {
                SesionGUID = SesionGUIDCookie.Value;
                UsuariosSesionesController SesionController = new UsuariosSesionesController();
                UsuariosSesiones = SesionController.GetSesion(SesionGUID);
                if (UsuariosSesiones != null)
                {
                    _Locale = UsuariosSesiones.Locale;
                }
            }
        }

        public string Ip = "";

        public string _Locale = "es-ES";
        protected override void InitializeCulture()
        {
            try
            {
                if (_Locale == null)
                {
                    _Locale = Comun.DefaultLocale;

                }
                if (this.Session["Locale"] != null)
                {
                    //_Locale = this.Session["Locale"].ToString();
                }
                Comun.SetCulture(_Locale);
                base.InitializeCulture();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public bool SesionDuplicada
        {
            get { return _SesionDuplicada; }
            set { _SesionDuplicada = value; }
        }

        public bool DesdeIpInterna
        {
            get { return _DesdeIpInterna; }
            set { _DesdeIpInterna = value; }
        }

        public Ext.Net.Theme Tema
        {
            get
            {
                return _Tema;
            }
            set { _Tema = value; }
        }

        private void Page_Init(object sender, System.EventArgs e)
        {
            Ip = Context.Request.Params.Get("REMOTE_ADDR");

            
            

            string paginaOrigen = AppRelativeVirtualPath;
            string pagina = null;
            string rutaJs = "";
            string[] aux = null;
            string[] aux2 = null;
            string[] separator = { ".aspx" };


            string virtualpath = AppRelativeVirtualPath;
            aux = virtualpath.Split("/".ToCharArray());
            aux2 = virtualpath.Split("~".ToCharArray());
            aux2 = aux2[aux2.Length - 1].Split("/".ToCharArray());
            rutaJs = aux2.Take(aux2.Length - 1).Join("/");
            virtualpath = aux[aux.Length - 1];
            

            aux = paginaOrigen.Split(Convert.ToChar("/"));
            paginaJS = aux[aux.Length - 1].Split(Convert.ToChar("?"))[0];
            pagina = virtualpath.ToLower();
            paginaJS = paginaJS.Split(separator, StringSplitOptions.None)[0];
            directorioJs = rutaJs;

            if ((aux.Length > 2) && (aux[2] == "pages"))
                interno = true;
            if ((aux.Length > 1) && (!paginaOrigen.Contains("pages")))
                NoPrincipal = true;

            bool _sesionValida = true;

            Usuario = null;
            if (UsuariosSesiones == null)
            {
                _sesionValida = false;
            }
            else
            {
                try
                {

                    if (this.Session.IsNewSession == true)
                    {
                        UsuariosController cUsuarios = new UsuariosController();
                        Usuarios us = new Usuarios();
                        us = cUsuarios.GetItem(UsuariosSesiones.UsuarioID);

                        if (us != null)
                        {
                            this.Session["LOGIN"] = true;
                            this.Session["USUARIO"] = us;
                            Accesos acceso = new Accesos(us);
                            this.Session["ACCESOS"] = acceso;
                            UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();
                            this.Session["PERFILES"] = cUsuariosPerfiles.perfilesAsignadosIDs(us.UsuarioID);
                            FuncionalidadesController fConntrol = new FuncionalidadesController();
                            ModulosController cModulos = new ModulosController();

                            if(us.ClienteID != null)
                            {
                                this.Session["FUNCIONALIDADES"] = fConntrol.getFuncionalidades(us.UsuarioID);
                                this.Session["LISTAFUNCIONALIDADES"] = fConntrol.getListFuncionalidades(us.UsuarioID);
                            }
                            else
                            {
                                this.Session["FUNCIONALIDADES"] = fConntrol.getFuncionalidadesSuperUsuario(us.UsuarioID);
                                this.Session["LISTAFUNCIONALIDADES"] = fConntrol.getListFuncionalidadesSuperUsuario(us.UsuarioID);
                            }
                            this.Session["LISTAMODULOS"] = cModulos.getAllModulos(us.UsuarioID);

                            this.Session["Locale"] = _Locale;
                        }
                    }

                    Usuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                    perfiles = (List<long>)this.Session["PERFILES"];
                    Accesos = (TreeCore.Data.Accesos)this.Session["ACCESOS"];

                    if (Usuario != null && Usuario.ClienteID.HasValue)
                    {
                        if (Usuario.ClienteID.HasValue)
                        {
                            ClienteID = Usuario.ClienteID;
                        }
                        else
                        {
                            ClienteID = null;
                        }
                    }

                    if (Session["OPERADOR"] != null)
                    {
                        OperadorID = (long)Session["OPERADOR"];
                    }
                    else
                    {
                        OperadorID = null;
                    }

                    if (!Accesos.PaginasPermitidas.Contains(pagina))
                    {
                        log.Info(string.Format(Comun.MsgPaginaNoPermitida, pagina));

                        String url = Page.ResolveUrl("~/AccesoNoPermitido.aspx");

                        if (Ext.Net.RequestManager.IsAjaxRequest)
                        {
                            Response.Redirect("{eventValidation:\"\",viewState:\"\",script:\"top.location.href='" + url + "';\"}");
                        }
                        else
                        {
                            Response.Redirect(url);
                        }

                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    _sesionValida = false;
                }
            }

            if (!_sesionValida)
            {
                String url;
                url = Page.ResolveUrl("~/Login.aspx");

                if (Ext.Net.RequestManager.IsAjaxRequest)
                {
                    Response.Write("{eventValidation:\"\",viewState:\"\",script:\"top.location.href='" + url + (SesionDuplicada ? "?ssDuplicada=1" : "") + (DesdeIpInterna ? "&EsInterna" : "") + "';\"}");
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">top.location.href='" + url + (SesionDuplicada ? "?ssDuplicada=1" : "") + (DesdeIpInterna ? "&EsInterna" : "") + "';</script>");
                }

                Response.End();

            }

            if (Page.Header != null)
            {
                Page.Header.Controls.Add(
                        new LiteralControl(
                            "<link runat=\"server\" type=\"image/x-icon\" rel=\"shortcut icon\" href=\"/ima/favicon.ico\"/>")
                    );
            }

            RegisterJavascriptStartupVariables(this);

            if (Request.QueryString[Comun.PARAM_IDS_RESULTADOS] != null && Request.QueryString[Comun.PARAM_NAME_INDICE_ID] != null)
            {
                listIdsResultadosKPI = new List<long>();
                IdsResultadosKPI = Request.QueryString.GetValues(Comun.PARAM_IDS_RESULTADOS)[0];
                nameIndiceID = Request.QueryString.GetValues(Comun.PARAM_NAME_INDICE_ID)[0];

                
                sResultadoKPIid = IdsResultadosKPI;


                DQKpisMonitoringController cDQKpisMonitoring = new DQKpisMonitoringController();
                long idDQKpiMonitoring = long.Parse(sResultadoKPIid);
                DQKpisMonitoring DQKpiMonitoring = cDQKpisMonitoring.GetItem(idDQKpiMonitoring);

                if (DQKpiMonitoring != null)
                {
                    List<long> listaIDs;
                    DQKpisController cDQKpis = new DQKpisController();
                    cDQKpis.ejecutarConsulta(DQKpiMonitoring.Filtro, out listaIDs);

                    listIdsResultadosKPI = listaIDs;
                }

            }
            else
            {
                sResultadoKPIid = "";
            }
        }

        public static void RegisterJavascriptStartupVariables(System.Web.UI.Page page)
        {
            // Define the name and type of the client scripts on the page.
            String csname = "ATREBO-JS-STARTUP";
            //String csname2 = "ButtonClickScript";
            Type cstype = page.GetType();

            // Get a ClientScriptManager reference from the Page class.
            System.Web.UI.ClientScriptManager cs = page.ClientScript;

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(cstype, csname))
            {
                String cstext = "";

                // produccion flags
#if TREE_PREPRODUCCION
                cstext += "var TREE_PREPRODUCCION = 1" + "\n";
#endif
#if TREE_PRODUCCION
                cstext += "var TREE_PRODUCCION = 1" + "\n";
#endif

                // per country flags
#if TREE_ALEMANIA
                cstext += "var TREE_ALEMANIA = 1" + "\n";
#endif
#if TREE_ARGENTINA
                cstext += "var TREE_ARGENTINA = 1" + "\n";
#endif
#if TREE_BRASIL
                cstext += "var TREE_BRASIL = 1" + "\n";
#endif
#if TREE_CAM
                cstext += "var TREE_CAM = 1" + "\n";
#endif
#if TREE_CHILE
                cstext += "var TREE_CHILE = 1" + "\n";
#endif
#if TREE_COLOMBIA
                cstext += "var TREE_COLOMBIA = 1" + "\n";
#endif
#if TREE_ECUADOR
                cstext += "var TREE_ECUADOR = 1" + "\n";
#endif
#if TREE_ESPAÑA
                cstext += "var TREE_ESPANA = 1" + "\n";
#endif
#if TREE_PERU
                cstext += "var TREE_PERU = 1" + "\n";
#endif
#if TREE_URUGUAY
                cstext += "var TREE_URUGUAY = 1" + "\n";
#endif
#if TREE_VENEZUELA
                cstext += "var TREE_VENEZUELA = 1" + "\n";
#endif
                // local
#if TREE_LOCAL
                cstext += "var TREE_LOCAL = 1" + "\n";
#endif

#if TREE_PUBLISH
                cstext += "var TREE_PUBLISH = 1" + "\n";
#endif
                // enables new IFRS seguimiento 
#if FORMULARIO_IFRS
                cstext += "var FORMULARIO_IFRS = 1" + "\n";
#if FORMULARIO_IFRS_GLOBAL
                cstext += "var FORMULARIO_IFRS_GLOBAL = 1" + "\n";
#endif
#if FORMULARIO_IFRS_ADQUISICIONES
                cstext += "var FORMULARIO_IFRS_ADQUISICIONES = 1" + "\n";
#endif
#if FORMULARIO_IFRS_BILLING
                cstext += "var FORMULARIO_IFRS_BILLING = 1" + "\n";
#endif
#if FORMULARIO_IFRS_TORREROS
                cstext += "var FORMULARIO_IFRS_TORREROS = 1" + "\n";
#endif
//#if FORMULARIO_IFRS_TORRERO_COLOCALIZACIONES
//                cstext += "var FORMULARIO_IFRS_TORRERO_COLOCALIZACIONES = 1" + "\n";
//#endif
#if FORMULARIO_IFRS_SHARING
                cstext += "var FORMULARIO_IFRS_SHARING = 1" + "\n";
#endif
#if FORMULARIO_IFRS_SAVING
                cstext += "var FORMULARIO_IFRS_SAVING = 1" + "\n";
#endif
#endif

                // push script into the page
                cs.RegisterStartupScript(cstype, csname, cstext, true);
            }

        }

        public string SafeResourceLookup(string key)
        {
            if (key == null ||
                key.Length == 0)
                return "BasePageExtNet:SafeResourceLookup - Recurso no encontrado";

            try
            {
                return this.GetLocalResourceObject(key).ToString();
            }
            catch (Exception)
            {

            }

            return key + " no existe";
        }

        public void ResourceManagerOperaciones(ResourceManager res)
        {
            string tema;
            if (Usuario == null || Usuario.Tema == "0" || Usuario.Tema == null)
            {
                tema = "10";
            }
            else
            {
                tema = Usuario.Tema;
            }
            Tema = Comun.Tema(tema);

            // agregamos el javascript concreto del locale
            Comun.AddCurrentCultureJavascript(res, this.Culture, paginaJS);

            if (Header != null)
            {
                directorioJs = directorioJs + "/" + (paginaJS.ToUpper().StartsWith("DEFAULT") ? "pages/js" : "js");
                Header.InnerHtml = Comun.CabeceraDeJavascriptYCSSVersionOrig(paginaJS, directorioJs, interno, NoPrincipal);
            }
        }

        public void MensajeBox(string Titulo, string mensaje, Ext.Net.MessageBox.Icon Icono, Exception ex)
        {
            Tree.Web.MiniExt.MensajeBox(Titulo, mensaje, Icono, ex);
        }

        public void MensajeErrorGenerico(Exception ex)
        {
#if SERVICESETTINGS
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Produccion"]))
#elif TREEAPI
            if (TreeAPI.Properties.Settings.Default.Produccion)
#else
            if (Properties.Settings.Default.Produccion)
#endif
            {
                MensajeBox(Resources.Comun.strAtencion, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }
            else
            {
                MensajeBox("Atención", Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, ex);
            }
        }

        public string GetGlobalResource(string key)
        {
            object valor;
            try
            {
                valor = this.GetGlobalResourceObject(Comun.NOMBRE_FICHERO_RECURSOS, key);
                if (valor != null)
                {
                    return valor.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
        [DirectMethod]
        public DirectResponse LogoutInicio()
        {
            DirectResponse ajax = new DirectResponse();

            #region ESTADISTICAS

            ProyectosTiposController cProyTip = new ProyectosTiposController();
            Data.ProyectosTipos oTipos = new Data.ProyectosTipos();
            oTipos = cProyTip.GetProyectosTiposByNombre("GLOBAL");
            if (Usuario != null)
            {
#if SERVICESETTINGS
                bool EscribeEstadistica = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["EscribeEstadistica"]);
#elif TREEAPI
            bool EscribeEstadistica = TreeAPI.Properties.Settings.Default.EscribeEstadistica;
#else
            bool EscribeEstadistica = TreeCore.Properties.Settings.Default.EscribeEstadistica;
#endif

                Util.EscribeEstadistica(Usuario.UsuarioID, oTipos.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], EscribeEstadistica, "LOGOUT");
            }

            #endregion

            Data.Usuarios oUser = ((Data.Usuarios)Session["USUARIO"]);
            UsuariosAccesosController cControlAcceso = new UsuariosAccesosController();
            cControlAcceso.limpiarSesion(Usuario.UsuarioID);

            Session["LOGIN"] = false;
            Session["USUARIO"] = null;
            Session["TOKEN"] = null;

            Cookies.Clear("SesionGUID");
            ajax.Success = true;
            ajax.Result = "";

            return ajax;
        }

        public string getThisPage()
        {
            string[] aux = AppRelativeVirtualPath.Split("/".ToCharArray());
            string pagina = aux[aux.Length - 1];
            pagina = pagina.ToLower();

            return pagina;
        }

    }
}

