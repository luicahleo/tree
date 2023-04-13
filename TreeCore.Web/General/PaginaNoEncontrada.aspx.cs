using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.Page;

namespace TreeCore
{
    public partial class PaginaNoEncontrada : System.Web.UI.Page
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string _Locale = Comun.DefaultLocale;
        public String paginaJS = "";

        #region Gestión Página (Init/Load)
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                //string _Locale = this.Session["LOCALE"].ToString();
                Comun.SetCulture(_Locale);
                
                //this.accesoTitulo.Text = GetGlobalResourceObject("Comun", "strAccesoNoPermitido").ToString();
                //this.accesoMensaje.Text = GetGlobalResourceObject("Comun", "strAccesoDenegadoMensaje").ToString();
                //this.btnCerrarSesion.Text = GetGlobalResourceObject("Comun", "strCerrarSesion").ToString();

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

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
        }
       
    }
}