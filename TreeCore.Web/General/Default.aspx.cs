using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Data;
using System.Reflection;

namespace TreeCore.General
{
    public partial class Default : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static long? pPerfilID = null;
        public List<long> lFuncionalidades = new List<long>();
        private Dictionary<string, Dictionary<string, string>> MenusModulos = new Dictionary<string, Dictionary<string, string>>();
        private string modulo;
        private string strModulo;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Title = Comun.TituloPaginas;
            lFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ParametrosController pController = new ParametrosController();

                //if (Usuario.Tema == null)
                //{
                //    tema = "10";
                //}
                //else
                //{
                //    tema = "10";
                //}


                List<string> pathsOfScripts = new List<string>();
                pathsOfScripts.Add(Comun.BundleConfigPaths.CONTENT_JS_DEFAULT);
                pathsOfScripts.Add(Comun.BundleConfigPaths.CONTENT_JS_GLOBAL);

                ResourceManagerOperaciones(ResourceManagerTreeCore, pathsOfScripts, new List<string>());
                ResourceManagerTreeCore.Theme = Tema;

                UsuariosController cUsuarios = new UsuariosController();

                if (Usuario != null)
                {
                    Usuario.Tema = "10";
                    Data.Usuarios oUsuario = cUsuarios.GetItem(Usuario.UsuarioID);

                    ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
                    Data.Vw_ToolServicios oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);

                    string instalacion = pController.GetItemValor("DESPLIEGUE_INICIAL");

                    ToolConexionesController cConex = new ToolConexionesController();
                    Data.Vw_ToolServicios oServ;
                    bool bIntegracion = false;
                    List<Data.ToolConexiones> listaConexiones;

                    oServ = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                    listaConexiones = cConex.getListaConexiones((long)oServ.IntegracionID);

                    if (oServ != null)
                    {
                        if ((bool)oServ.Activo && listaConexiones.Count > 0)
                        {
                            bIntegracion = true;
                        }
                    }

                    if (oUsuario != null && !oUsuario.EMail.Equals(Comun.TREE_SUPER_USER))
                    {
                        if (oDato != null && !(bool)oDato.Activo && !bIntegracion)
                        {
                            if (instalacion == "true")
                            {
                                winConfigInicial.Show();
                            }
                            else
                            {
                                if (oUsuario.FechaUltimoAcceso == null)
                                {
                                    txtCambiarClave.Clear();
                                    txtCambiarClave2.Clear();
                                    winCambiarClave.Show();
                                }
                                else
                                {
                                    oUsuario.FechaUltimoAcceso = DateTime.Today;
                                    cUsuarios.UpdateItem(oUsuario);
                                }
                            }
                        }
                        else if (oDato != null && bIntegracion)
                        {
                            oUsuario.FechaUltimoAcceso = DateTime.Today;
                            cUsuarios.UpdateItem(oUsuario);
                        }
                    }
                }

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.SetValue(ClienteID);
                    hdCliID.DataBind();
                }
                if (Usuario != null)
                {
                    hdUsuarioID.Value = Usuario.UsuarioID;
                }

                //if (Usuario != null)
                //{
                //    Comun.AccesoNoPermitidoAModulos(Response, Usuario, Comun.MODULOGLOBAL);
                //}

                #region Carga de página de inicio

                if (RouteData.Values.ContainsKey(Comun.VirtualPath.Default.modulo))
                {
                    modulo = RouteData.Values[Comun.VirtualPath.Default.modulo].ToString();
                    ComponenteMenu.NombreModulo = modulo;
                    ComponenteHeader.Modulo = GetGlobalResource($"str{modulo}");
                    this.Title = GetGlobalResource($"str{modulo}");

                    X.Js.Call($"LoadInicio{modulo}");
                }
                if (RouteData.Values.ContainsKey(Comun.VirtualPath.Default.pagina))
                {
                    string pagina = RouteData.Values[Comun.VirtualPath.Default.pagina].ToString();
                }

                ////Aqui se llama a la funcion JS que cargara en default la pagina que queremos que se muestre como inicio
                //if (RouteData.Values.ContainsKey(Comun.VirtualPath.Default.pagina))
                //{
                //    string pagina = RouteData.Values[Comun.VirtualPath.Default.pagina].ToString();
                //}

                //if (MenusModulos.ContainsKey(Culture) && MenusModulos[Culture].ContainsKey(modulo.ToLower().Replace(" ", "").Replace("/", "")))
                //{
                //    strModulo = MenusModulos[Culture][modulo.ToLower().Replace(" ", "").Replace("/", "")];
                //    this.Title = Comun.SafeResourceLookup(this, "PageTitle") + " - " + GetGlobalResource(strModulo);
                //}
                //else
                //{
                //    strModulo = "";
                //}


                #endregion

                if (Request.QueryString["openFavTab"] != null)
                {
                    string paramTab = Request.QueryString["openFavTab"];
                    MenusController cMenus = new MenusController();
                    Data.Vw_Menus tabFav = cMenus.GetItem<Vw_Menus>(long.Parse(paramTab));

                    string nombreAlias = cMenus.getNombreByAliasFromBasePageExtNet(tabFav, this);

                    if (tabFav != null)
                    {
                        string ruta = "";
                        if (!string.IsNullOrEmpty(tabFav.Parametros))
                        {
                            ruta = ((tabFav.RutaPagina == null) ? "../PaginasComunes/" + tabFav.Pagina : tabFav.RutaPagina) + '?' + tabFav.Parametros;
                        }
                        else
                        {
                            ruta = ((tabFav.RutaPagina == null) ? "../PaginasComunes/" + tabFav.Pagina : tabFav.RutaPagina);
                        }

                        X.Js.Call("addTabFromGlobal", "App.tabPpal", nombreAlias, nombreAlias, ruta);
                    }

                }



                ComponenteMenu.CargarMenu();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Usuario != null)
            {
                hdCliente.Value = Usuario.ClienteID;
            }
            this.Title = Comun.SafeResourceLookup(this, "PageTitle") + " - " + ComponenteHeader.Modulo/*Comun.SafeResourceLookup(this, "Global")*/;

            if (!RequestManager.IsAjaxRequest)
            {
                ResourceManagerTreeCore.Theme = Comun.Tema("10");

            }





        }


        protected void Logout(object sender, DirectEventArgs e)
        {
            #region ESTADISTICAS

            ProyectosTiposController cProyTip = new ProyectosTiposController();
            Data.ProyectosTipos oTipos = new Data.ProyectosTipos();
            oTipos = cProyTip.GetProyectosTiposByNombre("GLOBAL");
            Util.EscribeEstadistica(Usuario.UsuarioID, oTipos.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], TreeCore.Properties.Settings.Default.EscribeEstadistica, "LOGOUT");

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

            cControlAcceso.limpiarSesion(Usuario.UsuarioID);

            Session["LOGIN"] = false;
            Session["USUARIO"] = null;
            Session["TOKEN"] = null;
        }

        [DirectMethod]
        public DirectResponse CambiarClave()
        {
            DirectResponse ajax = new DirectResponse();
            try
            {
                try
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    Data.Usuarios oUsuario = cUsuarios.GetItem(Usuario.UsuarioID);
                    string temp = "";

                    if (txtCambiarClave.Text.ToString() == txtCambiarClave2.Text.ToString())
                    {
                        temp = cUsuarios.UsuariosCambiarClave(oUsuario.UsuarioID, txtCambiarClave.Text.ToString());

                        // ACTUALIZAR FECHA ACCESO UNA VEZ CAMBIADA LA CONTRASEÑA
                        oUsuario.FechaUltimoAcceso = DateTime.Today;
                        cUsuarios.UpdateItem(oUsuario);
                    }
                    else
                    {
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsClaveDiferente), Ext.Net.MessageBox.Icon.INFO, null);
                    }

                    ajax.Result = temp;
                    ajax.Success = true;
                    return ajax;
                }
                catch (Exception ex)
                {
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            return ajax;
        }


    }
}