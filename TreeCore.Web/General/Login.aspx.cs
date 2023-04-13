using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using TreeCore.APIClient;
using TreeCore.Data;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.DTO.General;

namespace TreeCore
{
    public partial class Login : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string _Locale = Comun.DefaultLocale;
        public String paginaJS = "";

        #region FUNCIONES CARGA

        protected void Page_Load(object sender, EventArgs e)
        {

            string parametro = Request.QueryString["Cliente"];
            parametro = HttpUtility.UrlEncode(parametro);

            this.Title = GetLocalResourceObject("strPageTitle").ToString();
            if (!String.IsNullOrEmpty(Request.QueryString["ssDuplicada"]))
            {
                Ext.Net.MessageBox msg = new Ext.Net.MessageBox();
                msg.Alert(Comun.MsgTituloDublicado, Comun.MsgDublicado, new JFunction { Fn = "showResult" }).Show();

            }

            txtUserName.Focus(false, 100);

#if TREE_PUBLISH
            btnDesarrollador.Hidden=true;
#endif

            string SesionGUID = Request.QueryString["GUID"];
            SesionGUID = HttpUtility.UrlEncode(SesionGUID);
            if (parametro != null)
            {
                if (parametro.Equals("ok"))
                {
                    if (SesionGUID != null)
                    {
                        HttpCookie cookie = new HttpCookie("SesionGUID", SesionGUID);
                        cookie.Expires = DateTime.Now.AddHours(12);
                        Response.Cookies.Add(cookie);
                        Tree.Web.MiniExt.Location(ResourceManagerTreeCore, "Default.aspx", false);
                    }
                }
            }
            if (!String.IsNullOrEmpty(Request.QueryString["ssDuplicada"]))
            {
                Ext.Net.MessageBox msg = new Ext.Net.MessageBox();
                msg.Alert(Comun.MsgTituloDublicado, Comun.MsgDublicado, new JFunction { Fn = "showResult" }).Show();

            }


        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CargarLocale())
            {
                string version = "V " + TreeCore.Properties.Settings.Default.Version;
                lblVersion.Text = version;
                lblEntorno.Text = TreeCore.Properties.Settings.Default.Nombre_Entorno;

                this.Title = Comun.TituloPaginas;
                if (!IsPostBack && !RequestManager.IsAjaxRequest)
                {
                    ResourceManagerTreeCore.Theme = Comun.Tema("10");
                    

                    if (this.Form.Attributes != null)
                        this.Form.Attributes.Add("onsubmit", "return false;");

                    Comun.AddCurrentCultureJavascript(ResourceManagerTreeCore, this.Culture, "Login", null);

                    string connectionURI = HttpContext.Current.Request.Url.AbsoluteUri;

                    var uri = new Uri(connectionURI);
                    string Url = connectionURI = uri.GetLeftPart(UriPartial.Authority);

                    ParametrosController cParametros = new ParametrosController();

                    Parametros oCertificadoSSL = cParametros.GetItemByName("CERTIFICADO_SSL");
                    if (oCertificadoSSL != null && (oCertificadoSSL.Valor == "SI" || oCertificadoSSL.Valor == "YES"))
                    {
                        if (connectionURI.Contains("http:"))
                            Url = connectionURI.Replace("http:", "https:");
                    }
                    else
                    {
                        if (connectionURI.Contains("https:"))
                            Url = connectionURI.Replace("https:", "http:");
                    }

                    Parametros oDobleValidacion = cParametros.GetItemByName("DOBLE_VALIDACION");
                    if (oDobleValidacion != null)
                    {
                        if (oDobleValidacion.Valor.ToUpper().Equals("SI") || oDobleValidacion.Valor.ToUpper().Equals("YES"))
                        {
                            txtCode.Hidden = false;
                        }
                    }


                }
            }
        }

        protected override void InitializeCulture()
        {
            CargarLocale();
            Comun.SetCulture(_Locale);
            base.InitializeCulture();
        }

        #endregion

        #region STORE IDIOMAS
        protected void storeIdiomas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir = null;
                    sort = e.Sort[0].Property.ToString();
                    dir = e.Sort[0].Direction.ToString();
                    int count = 0;
                    string filtro = "";

                    var lista = ListaIdiomas(e.Start, e.Limit, sort, dir, ref count, filtro);

                    if (lista != null)
                    {
                        storeIdiomas.DataSource = lista;

                        PageProxy temp = (PageProxy)storeIdiomas.Proxy[0];
                        temp.Total = count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Idiomas> ListaIdiomas(int start, int limit, string sort, string dir, ref int count, string filtro)
        {
            List<Data.Idiomas> datos = new List<Idiomas>();
            IdiomasController cIdiomas = new IdiomasController();

            try
            {
                datos = cIdiomas.GetItemsWithExtNetFilterList(filtro, sort, dir, start, limit, ref count, "Activo == true");

                foreach (Idiomas item in datos)
                {

                    mnuIdiomas.Items.Add(new Ext.Net.CheckMenuItem()
                    {
                        ID = item.CodigoIdioma.ToString(),
                        Text = item.Idioma,
                        Group = "Idioma",
                        Checked = false,
                    });

                }

                mnuIdiomas.UpdateContent();

                CargarLocale();
                CheckIdioma();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }

            return datos;
        }

        private bool IsServerConnected(string connectionString)
        {
            connectionString += "Connection Timeout=5;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    log.Error(ex.Message);
                    return false;
                }
            }
        }

        static async System.Threading.Tasks.Task ExecuteSqlTransaction(string connectionString, Ext.Net.Label lbl)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string cmdCall = "SELECT * FROM Parametros WHERE Parametro = 'GOOGLE_CADENA_HTTP'";
                SqlCommand versionCmd = new SqlCommand(cmdCall, connection);

                using (SqlDataReader vendorReader = await versionCmd.ExecuteReaderAsync())
                {
                    while (await vendorReader.ReadAsync())
                    {
                        if (vendorReader.FieldCount == 3)
                        {
                            string paramID = vendorReader[0].ToString(); // PARAMETROID
                            string paramName = vendorReader[1].ToString(); // PARAMETRO
                            string paramValor = vendorReader[2].ToString(); // VALOR

                            System.Diagnostics.Trace.WriteLine(paramID);
                            System.Diagnostics.Trace.WriteLine(paramName);
                            System.Diagnostics.Trace.WriteLine(paramValor);

                            lbl.Text = paramValor;
                        }
                    }
                }
            }
        }

        #endregion

        #region GESTION

        [DirectMethod]
        public DirectResponse Login_Click()
        {
            DirectResponse ajax = new DirectResponse();
            UsuariosController cController = new UsuariosController();
            ToolIntegracionesServiciosMetodosController cIntegra = new ToolIntegracionesServiciosMetodosController();
            LicenciasController licController = new LicenciasController();

            this.Session["LOGIN"] = false;
            this.Session["USUARIO"] = null;
            this.Session["PERMISOSACCESOS"] = null;
            this.Session["PERFILES"] = null;
            this.Session["OPERADOR"] = null;
            this.Session["TOKEN_API"] = null;

            bool bIntegracion = false;

            Usuarios us = new Usuarios();
            try
            {
                if (txtUserName.Text.Equals(Comun.TREE_SUPER_USER))
                {
                    if (!txtPassword.Text.Equals(getCodigoCompletoAutenticacion(txtUserName.Text, DateTime.Now)))
                    {
                        log.Info(GetLocalResourceObject("strUsuarioNoExiste").ToString());
                        ajax.Result = GetLocalResourceObject("strUsuarioNoExiste").ToString();
                        ajax.Success = false;
                        return ajax;
                    }
                    else
                    {
                        us = cController.getUsuarioByEmail(txtUserName.Text);
                    }
                }
                else
                {
                    #region LOGIN REGULAR USER

                    Vw_ToolServicios dato = null;
                    List<Data.ToolConexiones> listaConexiones;
                    ToolConexionesController cConex = new ToolConexionesController();
                    string sDominioABuscar = "";

                    // Searches for an integration for the login service
                    dato = cIntegra.GetIntegracionActivaByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                    if (dato != null)
                    {
                        listaConexiones = cConex.getListaConexiones((long)dato.IntegracionID);
                        if (listaConexiones.Count > 0)
                        {
                            bIntegracion = true;
                        }
                    }

                    if (bIntegracion)
                    {
                        #region LOGIN LDAP

                        if (txtUserName.Text.Contains("\\"))
                        {
                            sDominioABuscar = txtUserName.Text.Substring(0, txtUserName.Text.LastIndexOf("\\"));
                        }

                        string sIntegracion = dato.Integracion;
                        string sNombreClase = dato.NombreClase;
                        Type type = Type.GetType(sNombreClase);

                        if (type != null)
                        {
                            MethodInfo methodInfo = type.GetMethod(dato.Metodo);

                            if (methodInfo != null)
                            {
                                object result = null;
                                ParameterInfo[] parameters = methodInfo.GetParameters();
                                object classInstance = Activator.CreateInstance(type, sDominioABuscar);

                                if (parameters.Length == 0)
                                {
                                    result = methodInfo.Invoke(classInstance, null);
                                }
                                else
                                {
                                    object[] parametersArray = new object[] { txtUserName.Text, txtPassword.Text };
                                    result = methodInfo.Invoke(classInstance, parametersArray);
                                    if (result != null)
                                    {
                                        us = (Usuarios)result;
                                    }
                                    else
                                    {
                                        us = null;
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        string user = txtUserName.Text;
                        string pass = txtPassword.Text;
                        us = cController.UsuariosLogin(user, pass);

                        #region Login token on API
                        var apiClient = new APIClient.BaseAPIClient<TokenDTO>();
                        var result = apiClient.Login(user, pass).Result;

                        if (result.Success && result.Value != null)
                        {
                            this.Session["TOKEN_API"] = result.Value.AccessToken;
                        }
                        else
                        {
                            us = null;
                        }
                        #endregion
                    }

                    if (us != null)
                    {
                        #region GESTIÓN DE LA FECHA DE CADUCIDAD USUARIO

                        if (!bIntegracion)
                        {
                            if (!us.Activo)
                            {
                                log.Info(GetLocalResourceObject("strUsuarioInactivo").ToString());
                                ajax.Result = GetLocalResourceObject("strUsuarioInactivo").ToString();
                                ajax.Success = false;
                                return ajax;
                            }

                            if (us.FechaCaducidadUsuario != null && us.FechaCaducidadUsuario <= DateTime.Now)
                            {
                                log.Info(GetLocalResourceObject("strUsuarioCaducado").ToString());
                                ajax.Result = GetLocalResourceObject("strUsuarioCaducado").ToString();
                                ajax.Success = false;
                                return ajax;
                            }
                        }
                        else
                        {
                            if (!us.Activo)
                            {
                                us.Activo = true;
                                cController.UpdateItem(us);
                            }
                        }

                        #endregion

                        #region MULTIHOMMING

                        ParametrosController cParametros = new ParametrosController();
                        string sMulti = cParametros.ObtenerParametro(Comun.ACCESO_MULTIHOMING);

                        if (sMulti != null && sMulti != "" && (sMulti == "SI" || sMulti == "YES") && us.ClienteID != null)
                        {

                            GruposAccesosWebUsuariosController cgruposAccesosWebUsuarios = new GruposAccesosWebUsuariosController();
                            GruposAccesosWebRolesController cRoles = new GruposAccesosWebRolesController();
                            GruposAccesosWebController cGruposAccesosWeb = new GruposAccesosWebController();
                            List<GruposAccesosWebRoles> lGruposAccesosWebRoles = new List<GruposAccesosWebRoles>();
                            List<GruposAccesosWebUsuarios> lGruposAcceosWebUsuarios = new List<GruposAccesosWebUsuarios>();
                            List<Vw_GruposAccesoWebUsuarios> lGruposAcceosWebUsuariosCompleto = new List<Vw_GruposAccesoWebUsuarios>();
                            List<GruposAccesosWeb> lGruposAccesosWeb = new List<GruposAccesosWeb>();
                            List<long> lIDs = new List<long>();

                            lGruposAcceosWebUsuariosCompleto = cgruposAccesosWebUsuarios.GetItemsList<Vw_GruposAccesoWebUsuarios>("ClienteID==" + us.ClienteID.Value.ToString());

                            //Si ya hay configurado algún usuario entonces se tiene en cuenta el multihoming
                            if (lGruposAcceosWebUsuariosCompleto != null && lGruposAcceosWebUsuariosCompleto.Count > 0)
                            {
                                //Se buscan los grupos de acceso a los que tiene permiso el usuario por el rol
                                lGruposAccesosWebRoles = cRoles.getGruposAccesoByUsuarioID(us.UsuarioID);

                                if (lGruposAccesosWebRoles != null && lGruposAccesosWebRoles.Count > 0)
                                {
                                    string connectionURI = HttpContext.Current.Request.Url.AbsoluteUri;

                                    var uri = new Uri(connectionURI);
                                    string Url = connectionURI = uri.GetLeftPart(UriPartial.Authority);

                                    lIDs = cRoles.getGrupoAccesoWebID(lGruposAccesosWebRoles);

                                    if (lIDs != null)
                                    {
                                        lGruposAccesosWeb = cGruposAccesosWeb.getListaByIDs(lIDs, us.ClienteID);

                                        if (lGruposAccesosWeb != null && lGruposAccesosWeb.Count > 0)
                                        {
                                            bool btieneAcceso = false;
                                            foreach (GruposAccesosWeb item in lGruposAccesosWeb)
                                            {
                                                if (item.URL == Url)
                                                {
                                                    btieneAcceso = true;
                                                    break;
                                                }
                                            }

                                            if (!btieneAcceso)
                                            {
                                                log.Warn(GetGlobalResourceObject("Comun", "strAccesoDenegadoMultihomming"));
                                                ajax.Result = GetGlobalResourceObject("Comun", "strAccesoDenegadoMultihomming");
                                                ajax.Success = false;

                                                return ajax;
                                            }
                                            else
                                            {
                                                log.Warn(GetGlobalResourceObject("Comun", "strMsgMultihomingActivoPermisoConcedido"));
                                            }

                                        }
                                        else
                                        {
                                            log.Warn(GetGlobalResourceObject("Comun", "strAccesoDenegadoMultihomming"));
                                            ajax.Result = GetGlobalResourceObject("Comun", "strAccesoDenegadoMultihomming");
                                            ajax.Success = false;

                                            return ajax;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Warn(GetGlobalResourceObject("Comun", "strAccesoDenegadoMultihomming"));
                                    ajax.Result = GetGlobalResourceObject("Comun", "strAccesoDenegadoMultihomming");
                                    ajax.Success = false;

                                    return ajax;
                                }
                            }
                            else
                            {
                                log.Warn(GetGlobalResourceObject("Comun", "strMsgMultihomingActivoNingunUsuarioConfigurado"));
                            }
                        }
                        #endregion

                        #region DOBLE_AUTENTICACION

                        ParametrosController cParametrosDoble = new ParametrosController();
                        string sDobleAutenticacion = cParametrosDoble.ObtenerParametro(Comun.DOBLE_VALIDACION);
                        bool bIsCodigoCorrecto = false;

                        if (sDobleAutenticacion != null && sDobleAutenticacion != "" && (sDobleAutenticacion == "SI" || sDobleAutenticacion == "YES"))
                        {
                            bIsCodigoCorrecto = txtCode.Text.Equals(getCodigoCompletoAutenticacion(us.EMail, DateTime.Now));

                            if (!bIsCodigoCorrecto)
                            {
                                log.Info(GetLocalResourceObject("strDobleFactorIncorrecto").ToString());
                                ajax.Result = GetLocalResourceObject("strDobleFactorIncorrecto").ToString();
                                ajax.Success = false;
                                return ajax;
                            }
                            else
                            {
                                log.Info(GetLocalResourceObject("strDobleFactorCorrecto").ToString());
                            }
                        }

                        #endregion                        
                    }
                    else
                    {
                        #region INTENTOS CLAVE (SOLO LOGIN INTERNO)

                        if (!bIntegracion)
                        {
                            if (cController.getUsuarioByEmail(txtUserName.Text) != null)
                            {
                                Comun.IntentosClave = Comun.IntentosClave + 1;

                                if (this.Session["INTENTOS"] == null)
                                {
                                    Session["INTENTOS"] = 1;
                                }
                                else
                                {
                                    Session["INTENTOS"] = Comun.IntentosClave;
                                }

                                if (Comun.IntentosClave == 2)
                                {
                                    log.Info(GetLocalResourceObject("strClaveIncorrecta").ToString() + ": " + GetLocalResourceObject("strSegundoIntento").ToString());
                                    ajax.Result = Resources.Comun.strMsgUsuarioClaveIncorrectos;
                                }
                                else if (Comun.IntentosClave == 3)
                                {
                                    if (cController.BloquearUsuario(txtUserName.Text))
                                    {
                                        log.Info(GetLocalResourceObject("strClaveIncorrecta").ToString() + ": " + GetLocalResourceObject("strTercerIntento").ToString() + ". " + Resources.Comun.strUsuarioBloqueado);
                                        Comun.IntentosClave = 0;
                                        this.Session["INTENTOS"] = null;
                                    }
                                    ajax.Result = string.Format(Resources.Comun.strUsuarioBloqueado, txtUserName.Text);
                                }
                                else
                                {
                                    log.Info(GetLocalResourceObject("strClaveIncorrecta").ToString() + ": " + GetLocalResourceObject("strPrimerIntento").ToString());
                                    ajax.Result = Resources.Comun.strMsgUsuarioClaveIncorrectos;
                                }

                                ajax.Success = false;
                                return ajax;
                            }
                            else
                            {
                                Comun.IntentosClave = 0;
                                Session["INTENTOS"] = null;
                            }
                        }

                        #endregion

                        log.Info(GetLocalResourceObject("strUsuarioNoExiste").ToString());
                        ajax.Result = GetLocalResourceObject("strUsuarioNoExiste").ToString();
                        ajax.Success = false;
                        return ajax;
                    }

                    #endregion
                }

                if (us != null)
                {
                    UsuariosSesionesController SesionController = new UsuariosSesionesController();
                    UsuariosSesiones usuarioSesion = new UsuariosSesiones();
                    usuarioSesion.SesionGUID = Guid.NewGuid().ToString();
                    usuarioSesion.UltimaSolicitud = DateTime.Now;
                    usuarioSesion.Locale = _Locale;
                    usuarioSesion.UsuarioID = us.UsuarioID;
                    usuarioSesion.DireccionIP = Context.Request.Params["REMOTE_ADDR"];
                    SesionController.AddItem(usuarioSesion);

                    HttpCookie cookie = new HttpCookie("SesionGUID", usuarioSesion.SesionGUID);
                    cookie.Expires = DateTime.Now.AddHours(12);
                    Response.Cookies.Add(cookie);

                    Comun.IntentosClave = 0;
                    this.Session["INTENTOS"] = null;
                    this.Session["LOGIN"] = true;
                    this.Session["USUARIO"] = us;
                    Accesos acceso = new Accesos(us);
                    this.Session["ACCESOS"] = acceso;
                    UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();
                    this.Session["PERFILES"] = cUsuariosPerfiles.perfilesAsignadosIDs(us.UsuarioID);
                    FuncionalidadesController fConntrol = new FuncionalidadesController();
                    ModulosController cModulos = new ModulosController();
                    PerfilesController perfiles = new PerfilesController();
                    var listaPerfiles = perfiles.GetPerfilesByUsuario(us.UsuarioID);
                    BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(this.Session["TOKEN_API"].ToString());
                    var listaPer = baseAPI.GetList().Result.Value;
                    listaPer = listaPer.Where(x=> listaPerfiles.Select(c => c.Perfil_esES).ToList().Contains(x.Code)).ToList();
                    this.Session["FUNTIONALITIES"] = listaPer.SelectMany(c => c.UserFuntionalities).ToList();

                    if (us.EMail.Equals(Comun.TREE_SUPER_USER))
                    {
                        this.Session["FUNCIONALIDADES"] = fConntrol.getFuncionalidadesSuperUsuario(us.UsuarioID);
                        this.Session["LISTAFUNCIONALIDADES"] = fConntrol.getListFuncionalidadesSuperUsuario(us.UsuarioID);
                    }
                    else
                    {
                        this.Session["FUNCIONALIDADES"] = fConntrol.getFuncionalidades(us.UsuarioID);
                        this.Session["LISTAFUNCIONALIDADES"] = fConntrol.getListFuncionalidades(us.UsuarioID);
                    }
                    this.Session["LISTAMODULOS"] = cModulos.getAllModulos(us.UsuarioID);

                    UsuariosRolesController cUsuariosRoles = new UsuariosRolesController();
                    List<long> listaRoles = cUsuariosRoles.getRolesAsignadosByUsuario(us.UsuarioID);
                    if (listaRoles.Count == 0 && us.EMail != Comun.TREE_SUPER_USER)
                    {
                        log.Info(GetLocalResourceObject("strUsuarioSinRoles").ToString());
                        ajax.Result = GetLocalResourceObject("strUsuarioSinRoles").ToString();
                        ajax.Success = false;
                        return ajax;
                    }
                    else
                    {
                        Tree.Web.MiniExt.Location(ResourceManagerTreeCore, "/", false);
                    }
                }
                else
                {
                    log.Info(GetLocalResourceObject("strImposibleAutentificar").ToString());
                    ajax.Result = GetLocalResourceObject("strImposibleAutentificar").ToString();
                    ajax.Success = false;
                    return ajax;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ajax.Result = GetLocalResourceObject("strConexionCerrada").ToString();
                ajax.Success = false;
                return ajax;
            }

            return ajax;
        }

        [DirectMethod]
        public DirectResponse pasaLogin(string user)
        {
            DirectResponse ajax = new DirectResponse();
            UsuariosController uControl = new UsuariosController();
            ModulosController cModulos = new ModulosController();
            ajax.Success = true;
            ajax.Result = "";
            string token = "";
            string remoteAddr = Context.Request.Params["REMOTE_ADDR"].ToString();

            // seguridad
            bool allowPasaLogin = false;

#if TREE_PUBLISH
            ParametrosController cParametros = new ParametrosController();
            Parametros param = new Parametros();
            param = cParametros.GetItemByName("PASA_LOGIN");

            if (param != null && 
                (param.Valor == "SI" || param.Valor == "YES")
            {
                allowPasaLogin = true;
            }
            else
            {
                // allow hasta esta probado
                allowPasaLogin = true;
            }
#endif
            // local development, always allow
            if (!String.IsNullOrEmpty(remoteAddr) &&
                (remoteAddr == "127.0.0.1" || // IPv4
                remoteAddr == "::1" || // IPv6
                remoteAddr.ToLower() == "localhost"))
                allowPasaLogin = true;

            if (allowPasaLogin)
            {

                try
                {

                    string realUserName = "";

                    switch (user)
                    {
                        case "ADMIN":
                            {
                                realUserName = "admin@tree.com";
                            }
                            break;
                        case "SUPER":
                            {

                                realUserName = "david.humanes@alteda.com";
#if TREE_ECUADOR
                                realUserName = "soporte@sites.com";
#elif TREE_CHILE
                                realUserName = "tree@atrebo.com";
#else
                                //realUserName = "soporte@atrebo.com";
#endif
                            }
                            break;
                        default:
                            break;
                    }

                    Usuarios usu = uControl.GetItem("Email == \"" + realUserName + "\"");

                    this.Session["LOGIN"] = true;
                    this.Session["USUARIO"] = usu;
                    Comun.IntentoSeguridad = 0;
                    Accesos acceso = new Accesos(usu);
                    this.Session["ACCESOS"] = acceso;

                    FuncionalidadesController fConntrol = new FuncionalidadesController();
                    this.Session["FUNCIONALIDADES"] = fConntrol.getFuncionalidades(usu.UsuarioID);
                    this.Session["LISTAFUNCIONALIDADES"] = fConntrol.getListFuncionalidades(usu.UsuarioID);
                    this.Session["LISTAMODULOS"] = cModulos.getAllModulos(usu.UsuarioID);

                    UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();
                    this.Session["PERFILES"] = cUsuariosPerfiles.perfilesAsignadosIDs(usu.UsuarioID);

                    if (usu.ClienteID.HasValue)
                    {
                        ClientesController cCliente = new ClientesController();
                        this.Session["OPERADOR"] = cCliente.GetItem(usu.ClienteID.Value).OperadorID;
                    }

                    //Registro de Estadistica
                    ProyectosTiposController cProyTip = new ProyectosTiposController();
                    Data.ProyectosTipos ptip = new Data.ProyectosTipos();
                    ptip = cProyTip.GetProyectosTiposByNombre("GLOBAL");
                    Util.EscribeEstadistica(usu.UsuarioID, ptip.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], TreeCore.Properties.Settings.Default.EscribeEstadistica, "LOGIN");


                    UsuariosSesiones usuarioSesion = new UsuariosSesiones();
                    usuarioSesion.SesionGUID = Guid.NewGuid().ToString();
                    usuarioSesion.UltimaSolicitud = DateTime.Now;
                    usuarioSesion.Locale = _Locale;
                    usuarioSesion.UsuarioID = usu.UsuarioID;


                    HttpCookie cookie = new HttpCookie("SesionGUID", usuarioSesion.SesionGUID);
                    cookie.Expires = DateTime.Now.AddHours(12);
                    Response.Cookies.Add(cookie);

                    if (!String.IsNullOrEmpty(Context.Request.Params["REMOTE_ADDR"]))
                    {
                        usuarioSesion.DireccionIP = Context.Request.Params["REMOTE_ADDR"];
                    }

                    UsuariosSesionesController SesionController = new UsuariosSesionesController();
                    SesionController.AddItem(usuarioSesion);

                    UsuariosRolesController cUsuariosRoles = new UsuariosRolesController();
                    List<long> listaRoles = cUsuariosRoles.getRolesAsignadosByUsuario(usu.UsuarioID);
                    if (listaRoles.Count == 0 && usu.EMail != Comun.TREE_SUPER_USER)
                    {
                        log.Info(GetLocalResourceObject("strUsuarioSinRoles").ToString());
                        ajax.Result = GetLocalResourceObject("strUsuarioSinRoles").ToString();
                        ajax.Success = false;
                        return ajax;
                    }
                    else
                    {
                        Tree.Web.MiniExt.Location(ResourceManagerTreeCore, "/", false);
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    ajax.Result = GetLocalResourceObject("strConexionCerrada").ToString();
                    ajax.Success = false;
                    return ajax;
                }

            }
            return ajax;
        }

        protected void Logout(object sender, DirectEventArgs e)
        {
            Session["LOGIN"] = false;
            Session["USUARIO"] = null;
        }

        #endregion

        #region MODO AUTENTICACION

        private bool IsAutentificacionIntegrada()
        {
            // Local variables
            bool bRes = false;
            ToolIntegracionesServiciosMetodosController cIntegra = new ToolIntegracionesServiciosMetodosController();
            Vw_ToolServicios dato = null;

            // Searches for an integration for the login service
            dato = cIntegra.GetIntegracionActivaByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);

            if (dato != null)
            {
                bRes = true;
            }

            // Returns the result
            return bRes;
        }

        #endregion

        #region RECUPERAR PASSWORD

        [DirectMethod]
        public DirectResponse RecuperarContra()
        {
            //Elemento de respuesta de la función
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            UsuariosController uController;

            try
            {
                uController = new UsuariosController();

                //Se recupera el usuario logueado en el sistema
                Data.Usuarios dato = new Data.Usuarios();
                ParametrosController cParametros = new ParametrosController();
                dato = uController.getUsuarioByEmail(txtEmail.Text);
                if (dato != null)
                {
                    string connectionURI = HttpContext.Current.Request.Url.AbsoluteUri;

                    var uri = new Uri(connectionURI);
                    string Url = connectionURI = uri.GetLeftPart(UriPartial.Authority);

                    string emailURL = Url + "/pages/RecuperarContra.aspx?emailuser=" + txtEmail.Text + "&locale=" + Session["LOCALE"];

                    String pUsuario = "SMTP_Mail";
                    String pPassword = "SMTP_Clave";
                    String SMTP = "SMTP_Servidor";
                    String Puerto = "SMTP_PUERTO";
                    int SMPTPuerto = Convert.ToInt32(cParametros.GetItemValor(Puerto));


                    System.Net.Mail.MailMessage mailmessage = new System.Net.Mail.MailMessage();
                    mailmessage.To.Add(txtEmail.Text);
                    mailmessage.From = new System.Net.Mail.MailAddress(cParametros.GetItemValor(pUsuario));
                    mailmessage.Subject = GetLocalResourceObject("strCambioContraseña").ToString();

                    mailmessage.Body = GetLocalResourceObject("srtIndicaciones").ToString() + emailURL;
                    mailmessage.Priority = System.Net.Mail.MailPriority.High;

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(cParametros.GetItemValor(SMTP), SMPTPuerto);
                    smtpclient.Credentials = new System.Net.NetworkCredential(cParametros.GetItemValor(pUsuario), cParametros.GetItemValor(pPassword));
                    smtpclient.EnableSsl = true;
                    smtpclient.Timeout = 60000;

                    try
                    {
                        smtpclient.Send(mailmessage);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                }
                else
                {
                    //MensajeBox(GetLocalResourceObject("jsAvisos").ToString(), GetLocalResourceObject("strNoEsUser").ToString(), MessageBox.Icon.INFO, null);
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ajax.Result = GetLocalResourceObject("strConexionCerrada").ToString();
                ajax.Success = false;
                return ajax;
            }

            uController = null;
            return ajax;
        }

        #endregion

        #region IDIOMAS

        private bool CargarLocale()
        {
            bool LocaleOK = true;

            try
            {
                string valor = Request.QueryString["locale"];

                if ((valor != null))
                {
                    _Locale = valor;
                    Session["LOCALE"] = valor;
                }
                else
                {
                    String idioma = "";

                    if (mnuIdiomas != null && mnuIdiomas.Items.Count != 0)
                    {
                        foreach (CheckMenuItem x in mnuIdiomas.Items)
                        {
                            if (x.Checked == true)
                            {
                                idioma = x.ID;
                            }
                        }
                    }

                    if (idioma == "")
                    {
                        IdiomasController cIdiomas = new IdiomasController();
                        Data.Idiomas idiomaDefecto = cIdiomas.GetItem("Defecto == true");

                        if (idiomaDefecto != null)
                        {
                            idioma = idiomaDefecto.CodigoIdioma;
                        }
                        else
                        {
                            // Se recupera el parámetro del idioma por defecto
                            ParametrosController cparametros = new ParametrosController();
                            Data.Parametros param = new Data.Parametros();
                            param = cparametros.GetItemByName("IDIOMA_LOGIN");

                            if (param != null)
                            {
                                idioma = param.Valor;
                            }
                            else
                            {
                                idioma = "es-ES";
                            }
                        }
                    }

                    _Locale = idioma;
                    Session["LOCALE"] = idioma;

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                LocaleOK = false;

                String url = Page.ResolveUrl("~/General/" + Comun.PaginaNoEncontrada);

                if (Ext.Net.RequestManager.IsAjaxRequest)
                {
                    Response.Write("{eventValidation:\"\",viewState:\"\",script:\"top.location.href='" + url + "';\"}");
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">top.location.href='" + url + "';</script>");
                }

                Response.End();
            }

            return LocaleOK;

        }

        public void CheckIdioma()
        {
            string sLocale = Request.QueryString["locale"];

            if (sLocale != null)
            {
                foreach (CheckMenuItem menu in mnuIdiomas.Items)
                {
                    if (menu.ID == sLocale)
                    {
                        menu.Checked = true;
                        break;
                    }
                }
            }
            else
            {
                IdiomasController cIdiomas = new IdiomasController();
                Data.Idiomas idiomaDefecto = cIdiomas.GetItem("Defecto == true");
                if (idiomaDefecto != null)
                {
                    foreach (CheckMenuItem menu in mnuIdiomas.Items)
                    {
                        if (menu.ID == idiomaDefecto.CodigoIdioma)
                        {
                            menu.Checked = true;
                            break;
                        }
                    }
                }
                else
                {
                    ParametrosController cparametros = new ParametrosController();
                    Data.Parametros param = new Data.Parametros();
                    param = cparametros.GetItemByName("IDIOMA_LOGIN");

                    foreach (CheckMenuItem menu in mnuIdiomas.Items)
                    {
                        if (menu.ID == param.Valor)
                        {
                            menu.Checked = true;
                            break;
                        }
                    }
                }
            }

            mnuIdiomas.UpdateContent();

        }

        [DirectMethod]
        public DirectResponse CambioIdioma(String ID)
        {
            DirectResponse direct = new DirectResponse();
            try
            {

                foreach (CheckMenuItem menuItem in mnuIdiomas.Items)
                {
                    if (menuItem.ID == ID)
                    {
                        cycleIdiomas.Text = menuItem.Text;
                        break;
                    }

                }

                Tree.Web.MiniExt.Location(ResourceManagerTreeCore, "login.aspx?locale=" + ID, false);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = true;
                return direct;
            }
            direct.Success = true;
            direct.Result = "";
            return direct;
        }


        #endregion

        #region CODIGO DOBLE VALIDACIÓN

        private static int getCodigoByLetras(string sEmail)
        {

            //1 - Crear el mapa 
            Dictionary<char, int> dLetras = new Dictionary<char, int>();

            dLetras.Add('a', 1001); dLetras.Add('b', 1002); dLetras.Add('c', 1003); dLetras.Add('d', 1004);
            dLetras.Add('e', 1005); dLetras.Add('f', 1006); dLetras.Add('g', 1007); dLetras.Add('h', 1008);
            dLetras.Add('i', 1009); dLetras.Add('j', 1010); dLetras.Add('k', 1011); dLetras.Add('l', 1012);
            dLetras.Add('m', 1013); dLetras.Add('n', 1014); dLetras.Add('o', 1015); dLetras.Add('p', 1016);
            dLetras.Add('q', 1017); dLetras.Add('r', 1018); dLetras.Add('s', 1019); dLetras.Add('t', 1020);
            dLetras.Add('u', 1021); dLetras.Add('v', 1022); dLetras.Add('w', 1023); dLetras.Add('x', 1024);
            dLetras.Add('y', 1025); dLetras.Add('z', 1026); dLetras.Add('@', 1027); dLetras.Add('_', 1028);
            dLetras.Add('-', 1029); dLetras.Add('.', 1030);

            String str = sEmail;

            //2 - Pasar de string a list character el email

            List<char> lLetrasEmail = convertStringToCharList(sEmail);
            List<int> lValores = new List<int>();
            List<int> lNumCaracter = new List<int>();
            List<int> lPosicion = new List<int>();

            int iLongitudUsuario = lLetrasEmail.Count;
            int iDistanciaCaracteres = iLongitudUsuario / 6;
            List<int> lValorCodigo = new List<int>();
            int iCodigoUsuario = 0;

            //3- Comprobar las letras del email y añadirle su valor

            for (int i = 1; i <= 6; i++)
            {
                Console.WriteLine(i);
                if (dLetras.ContainsKey(lLetrasEmail[i - 1]))
                {
                    Console.WriteLine(i);
                    lValores.Add(dLetras[lLetrasEmail[i - 1]]);
                }

                lNumCaracter.Add(i - 1);
                lPosicion.Add(i * iDistanciaCaracteres);

                lValorCodigo.Add(lPosicion[i - 1] * lValores[i - 1]);
                iCodigoUsuario += lValorCodigo[i - 1];
            }

            return iCodigoUsuario;
        }

        private static int getCodigoByFecha(DateTime dFecha)
        {
            int iYear = dFecha.Year;
            int iMonth = dFecha.Month;
            int iDay = dFecha.Day;
            int iHour = dFecha.Hour;
            int iMinute = dFecha.Minute;

            Console.WriteLine("fecha: " + dFecha);

            List<int> lElementosFecha = new List<int>();
            List<int> lCodigos = new List<int>();

            int iCodigoMultiplicado = 0;
            List<int> lCodigosMultiplicados = new List<int>();

            lElementosFecha.Add(iDay);
            lElementosFecha.Add(iMonth);
            lElementosFecha.Add(iYear);
            lElementosFecha.Add(iHour);
            lElementosFecha.Add(iMinute);

            double dPesoFijo = 999999;
            double dCodigo = 0.0;
            double dCodigoIntermedio = 0.0;

            int iSumaTotal = 0;
            int ICODIGO_FECHA = 0;

            Dictionary<int, int> dPesos = new Dictionary<int, int>();
            dPesos.Add(0, 30);
            dPesos.Add(1, 12);
            dPesos.Add(2, 2030);
            dPesos.Add(3, 24);
            dPesos.Add(4, 60);

            for (int i = 0; i <= lElementosFecha.Count - 1; i++)
            {
                dCodigo = Math.Ceiling(dPesoFijo / dPesos[i]);
                //dCodigo = Math.Ceiling(1.0 * dPesoFijo / dPesos[i]);
                int iCodigo2 = (int)dCodigo;
                lCodigos.Add(iCodigo2);
                iCodigoMultiplicado = lElementosFecha[i] * lCodigos[i];
                lCodigosMultiplicados.Add(iCodigoMultiplicado);
                iSumaTotal += lCodigosMultiplicados[i];

            }

            dCodigoIntermedio = Math.Ceiling(1.0 * iSumaTotal / 5);
            ICODIGO_FECHA = (int)dCodigoIntermedio;
            Console.WriteLine("codigoIntermedio: " + dCodigoIntermedio);
            Console.WriteLine("CODIGO_FECHA: " + ICODIGO_FECHA);

            return ICODIGO_FECHA;
        }

        public static List<char> convertStringToCharList(String email)
        {
            List<Char> lListaChar = new List<char>();
            foreach (char c in email)
            {
                lListaChar.Add(c);
            }
            return lListaChar;
        }

        private static List<int> intToListInt(int num)
        {
            List<int> listOfInt = new List<int>();
            while (num > 0)
            {
                listOfInt.Add(num % 10);
                num = num / 10;
            }
            listOfInt.Reverse();
            return listOfInt;
        }

        public static string getCodigoCompletoAutenticacion(string sEmailEntrada, DateTime dFecha)
        {
            string sCodigoReordenado = "";
            string sCadenaParesImpares = "";
            int iMinuto = dFecha.Minute % 10;
            string sCodigo;
            int sUnion = 0;
            sUnion = getCodigoByLetras(sEmailEntrada) + getCodigoByFecha(dFecha);
            sCodigo = Convert.ToString(sUnion);
            List<int> lListOfInt = new List<int>();
            lListOfInt = intToListInt(sUnion);

            switch (iMinuto)
            {
                case 0:
                    sCodigoReordenado = sCodigo;
                    break;

                case 1:
                    foreach (char cifra in sCodigo)
                        sCodigoReordenado = cifra + sCodigoReordenado;
                    break;

                case 2:
                case 3:
                case 4:
                case 5:

                    List<int> lPares = new List<int>();
                    List<int> lImpares = new List<int>();
                    string sResultadoPares = "";
                    string sResultadoImpares = "";

                    for (int i = 0; i < lListOfInt.Count; i++)
                    {
                        int valor = lListOfInt[i];
                        if (i % 2 == 0)
                            lImpares.Add(valor);
                        else
                            lPares.Add(valor);
                    }

                    if (iMinuto == 4 || iMinuto == 5)
                    {
                        lImpares.Reverse();
                        lPares.Reverse();
                    }

                    for (int i = 0; i < lPares.Count; i++)
                    {
                        sResultadoPares = sResultadoPares + Convert.ToString(lPares[i]);
                    }

                    for (int i = 0; i < lImpares.Count; i++)
                    {
                        sResultadoImpares = sResultadoImpares + Convert.ToString(lImpares[i]);
                    }

                    switch (iMinuto)
                    {
                        case 2:
                        case 4:
                            sCadenaParesImpares = sResultadoPares + sResultadoImpares;
                            break;
                        case 3:
                        case 5:
                            sCadenaParesImpares = sResultadoImpares + sResultadoPares;
                            break;
                    }
                    sCodigoReordenado = sCadenaParesImpares;
                    break;

                case 6:
                case 7:

                    for (int i = 0; i < lListOfInt.Count / 2; i++)
                    {
                        sCadenaParesImpares = sCadenaParesImpares + lListOfInt[i];
                        sCadenaParesImpares = sCadenaParesImpares + lListOfInt[lListOfInt.Count - 1 - i];
                    }

                    if (iMinuto == 7)
                    {
                        string sCadena = "";
                        foreach (char numero in sCadenaParesImpares)
                        {
                            sCadena = numero + sCadena;
                        }
                        sCadenaParesImpares = sCadena;
                    }

                    sCodigoReordenado = sCadenaParesImpares;
                    break;

                case 8:
                    for (int i = 0; i < lListOfInt.Count / 2; i++)
                    {
                        sCadenaParesImpares = sCadenaParesImpares + lListOfInt[lListOfInt.Count / 2 - 1 - i];
                        sCadenaParesImpares = sCadenaParesImpares + lListOfInt[lListOfInt.Count / 2 + i];
                    }

                    sCodigoReordenado = sCadenaParesImpares;
                    break;

                case 9:
                    for (int i = 0; i < 2; i++)
                    {
                        sCadenaParesImpares = sCadenaParesImpares + lListOfInt[lListOfInt.Count - 1 - i];
                        sCadenaParesImpares = sCadenaParesImpares + lListOfInt[i];
                    }

                    sCodigoReordenado = sCadenaParesImpares + lListOfInt[2] + lListOfInt[3];
                    break;
            }

            //Console.WriteLine("CÓDIGO VÁLIDO: " + iMinuto);
            return sCodigoReordenado;
        }


        #endregion

    }
}