using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using TreeCore.Data;
using System.Reflection;
using TreeCore.Page;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using TreeCore.Shared.DTO;
using TreeCore.Shared.DTO.Query;
using System.Linq;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.General;

namespace TreeCore.Componentes
{
    public partial class Header : BaseUserControl
    {
        private string _Modulo;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Modulo
        {
            get { return _Modulo; }
            set { 
                this._Modulo = value;
                lblModulo.Text = Modulo;
            }
        }

        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            imgUser.Src = ImagenCargada();
        }


        protected void Page_Init(object sender, EventArgs e)
        {

            
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

            try
            {
                if (!IsPostBack && !RequestManager.IsAjaxRequest)
                {
                    if (oUsuario != null)
                    {
                        UsuariosPerfilesController cPerfiles = new UsuariosPerfilesController();

                        if (oUsuario.ClienteID == null)
                        {

                        }
                        else
                        {
                            List<long?> lProyTip = new List<long?>();
                            lProyTip = cPerfiles.modulosByPerfilesAsignadosIDs(oUsuario.UsuarioID);

                            if (lProyTip != null)
                            {
                                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                                lblVersion.Text = Properties.Settings.Default.Version.ToString();
                                lblVersionAssembly.Text = assembly.GetName().Version.ToString();
                                lblVersionAssemblyFile.Text = fvi.FileVersion;
                                ParametrosController cParametros = new ParametrosController();
                                lblBBDDVersion.Text = cParametros.ObtenerParametro("BBDDVersion");
                            }
                        }
                    }

                    lblModulo.Text = Modulo;
                }

                if (oUsuario != null)
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);

                    lblNombre.Text = oUser.NombreCompleto;
                    lblEmail.Text = oUser.EMail;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }


        }

        #endregion
        public string ImagenCargada()
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            DocumentosController cDocumentos = new DocumentosController();
            string imagenCargada = "";
            if (oUsuario != null)
            {
                var files = Directory.GetFiles(TreeCore.DirectoryMapping.GetImagenUsuarioDirectory(), oUsuario.UsuarioID.ToString() + ".*");

                if (files.Length > 0)
                {
                    var tempFiles = Directory.GetFiles(TreeCore.DirectoryMapping.GetImagenUsuarioTempDirectory(), oUsuario.UsuarioID.ToString() + ".*");
                    string extension = files[0].Split('.')[1];
                    string rutaTemp = Path.Combine(DirectoryMapping.GetImagenUsuarioTempDirectory(), oUsuario.UsuarioID.ToString() + '.' + extension);

                    if (tempFiles.Length.Equals(0))
                    {
                        File.Copy(files[0], rutaTemp);
                    }

                    imagenCargada = "/" + Path.Combine(DirectoryMapping.GetImagenUsuarioTempDirectoryRelative(), oUsuario.UsuarioID.ToString() + '.' + extension); ;

                }
            }

            if (imagenCargada == "")
            {
                imagenCargada = "/ima/ico-user-empty.svg";
            }

            return imagenCargada;
        }

        #region FUNCTIONS

        protected void Logout(object sender, DirectEventArgs e)
        {
            #region ESTADISTICAS

            ProyectosTiposController cProyTip = new ProyectosTiposController();
            Data.ProyectosTipos oTipos = new Data.ProyectosTipos();
            oTipos = cProyTip.GetProyectosTiposByNombre("GLOBAL");
            //Util.EscribeEstadistica(Usuario.UsuarioID, oTipos.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], TreeCore.Properties.Settings.Default.EscribeEstadistica, "LOGOUT");

            #endregion

            Data.Usuarios oUser = ((Data.Usuarios)Session["USUARIO"]);
            UsuariosAccesosController cControlAcceso = new UsuariosAccesosController();

#if OAM_INTEGRATION
            #region OAM
            ParametrosController cParametros = new ParametrosController();
            Sites.Data.Parametros pOAMActivado = new Sites.Data.Parametros();
            Sites.Data.Parametros pURLOAMLogout = new Sites.Data.Parametros();
            Sites.Data.Parametros pURLTREE = new Sites.Data.Parametros();
            Sites.Data.Parametros pOAMEmisor = new Sites.Data.Parametros();

            pOAMActivado = cParametros.GetItemByName(Comun.PARAMETROS_OAM_ACTIVADO);
            pURLOAMLogout = cParametros.GetItemByName(Comun.PARAMETROS_OAM_URL_LOGOUT);
            pURLTREE = cParametros.GetItemByName(Comun.PARAMETROS_OAM_URL_TREE);
            pOAMEmisor = cParametros.GetItemByName(Comun.PARAMETROS_OAM_EMISOR);

            if (pOAMActivado.Valor != null)
            {
                if (pOAMActivado.Valor != "")
                {
                    if (pOAMActivado.Valor == "SI")
                    {
                        string uriRespuesta = pURLOAMLogout.Valor;
                        string uriReturn = pURLTREE.Valor;
                        System.Uri oURI = new System.Uri(uriRespuesta);
                        System.Uri oURIRetorno = new System.Uri(uriReturn);

                        Sustainsys.Saml2.Saml2P.Saml2LogoutRequest logoutRequest = new Sustainsys.Saml2.Saml2P.Saml2LogoutRequest();
                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Después de inicializar logoutRequest"));

                        logoutRequest.DestinationUrl = oURI;
                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Después de asignar la uri destino. oURI = " + oURI.ToString()));

                        logoutRequest.SessionIndex = Session["SESION_INDEX"].ToString();
                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Después de asignar el index de sesión. logoutRequest.SessionIndex = " + logoutRequest.SessionIndex));

                        logoutRequest.Issuer = new Sustainsys.Saml2.Metadata.EntityId(pOAMEmisor.Valor);
                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Después de asignar el logoutid. logoutRequest.Issuer = " + logoutRequest.Issuer.ToString()));

                        Microsoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier name2 = new Microsoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier(Session["LOGOUT_ID"].ToString());
                        string sFormat = Comun.OAM_FORMAT;
                        System.Uri oFormat = new System.Uri(sFormat);
                        name2.Format = oFormat;
                        logoutRequest.NameId = name2;

                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Después de asignar el Saml2NameIdentifier. logoutRequest.NameId = " + logoutRequest.NameId.ToString()));

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(logoutRequest.ToXml());
                        System.Xml.XmlElement xmlResponse = doc.DocumentElement;


                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Antes de SingleLogoutService = " + xmlResponse.ToString()));
                        ComponentSpace.SAML2.Profiles.SingleLogout.SingleLogoutService.SendLogoutRequestByHTTPPost(Response, uriRespuesta, xmlResponse, null);
                        
                        Comun.Log(Comun.MensajeLog("Login", "Logout", "Despues de SingleLogoutService " ));

                        Comun.Log(Comun.MensajeLog("Default", "Logout", "Antes de hacer SignOut. User.Identity.IsAuthenticated = " + User.Identity.IsAuthenticated));

                        System.Web.Security.FormsAuthentication.SignOut();
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(string.Empty), null);

                        Comun.Log(Comun.MensajeLog("Default", "Logout", "Después de hacer SignOut. User.Identity.IsAuthenticated = " + User.Identity.IsAuthenticated));
                        
                    }
                }
            }

            
            #endregion
#endif

            //cControlAcceso.limpiarSesion(Usuarios.UsuarioID);

            Session["LOGIN"] = false;
            Session["USUARIO"] = null;
            Session["TOKEN"] = null;

            Cookies.Clear("SesionGUID");

        }

        private void CargarMenuModulos(out List<MenusModulos> ModsDisponibles, out List<MenusModulos> ModsNoDisponibles)
        {
            List<MenusModulos> lMenuModulosDisponibles = new List<MenusModulos>();
            List<MenusModulos> lMenuModulosNoDisponibles = new List<MenusModulos>();
            MenusModulosController cMenuModulos = new MenusModulosController();

            try
            {
                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                if (oUsuario != null)
                {
                    if (oUsuario.ClienteID == null)
                    {
                        foreach (MenusModulos temp in cMenuModulos.GetActivos())
                        {
                            lMenuModulosDisponibles.Add(temp);
                        }
                    }
                    else
                    {
                        UsuariosRolesController cUsuariosRoles = new UsuariosRolesController();
                        List<long?> lProyTip = cUsuariosRoles.modulosByPerfilesAsignadosIDs(oUsuario.UsuarioID);

                        if (lProyTip != null)
                        {
                            foreach (long? proyTipID in lProyTip)
                            {
                                MenusModulos temp = cMenuModulos.GetMenuModuloByProyectoTipoId(proyTipID);
                                if (temp != null && !lMenuModulosDisponibles.Contains(temp))
                                {
                                    lMenuModulosDisponibles.Add(temp);
                                }
                            }

                            foreach (MenusModulos temp in cMenuModulos.GetActivos())
                            {
                                if (!lMenuModulosDisponibles.Contains(temp))
                                {
                                    if (!lMenuModulosNoDisponibles.Contains(temp))
                                    {
                                        lMenuModulosNoDisponibles.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                lMenuModulosDisponibles = null;
                lMenuModulosNoDisponibles = null;
            }

            ModsDisponibles = lMenuModulosDisponibles;
            ModsNoDisponibles = lMenuModulosNoDisponibles;
        }

        [DirectMethod]
        public DirectResponse PintarMenuModulos()
        {
            DirectResponse direct = new DirectResponse();
            List<JsonObject> lMenusModulos = new List<JsonObject>();

            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var listaInterfaces = ModulesController.GetUserInterfaces().Where(x => listFun.Where( c => c.Split('@')[1] == "Read").Select(c => c.Split('@')[0]).Contains(x.Code)).ToList();

            try
            {
                foreach (var item in ModulesController.GetModules().Where(x => x.UserInterfaces.Where(c => listaInterfaces.Select(e => e.Code).Contains(c.Code)).ToList().Count > 0))
                {
                    if (item.Name == "Global")
                        continue;
                    JsonObject jMenuModulo = new JsonObject();
                    jMenuModulo.Add("Modulo", item.Name);
                    jMenuModulo.Add("RutaModulo", item.Name);
                    jMenuModulo.Add("str", GetGlobalResource($"str{item.Name}"));
                    jMenuModulo.Add("disabled", false);
                    lMenusModulos.Add(jMenuModulo);
                }

                object mod;
                object mod2;

                lMenusModulos.Sort((x, y) => x.TryGetValue("str", out mod).CompareTo(y.TryGetValue("str", out mod2)));

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            direct.Success = true;
            direct.Result = lMenusModulos;

            return direct;
        }

        #endregion

    }
}