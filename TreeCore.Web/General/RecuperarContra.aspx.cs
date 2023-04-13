using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaNegocio;
using Ext.Net;

namespace TreeCore.PaginasComunes
{
    public partial class RecuperarContra : System.Web.UI.Page
    {
        string emailUser = "";
        string _Locale = Comun.DefaultLocale;

        #region IDIOMAS

        bool _Español = false;
        bool _Ingles = false;
        bool _Frances = false;
        bool _Aleman = false;
        bool _Portugues = false;
        bool _Ecuador = false;
        bool _Chile = false;
        bool _Panama = false;
        bool _Argentina = false;
        bool _Colombia = false;
        bool _Peru = false;
        bool _Uruguay = false;
        bool _Venezuela = false;
        bool _Noruego = false;

        public bool Español
        {
            get { return _Español; }
            set { _Español = value; }
        }

        public bool Ingles
        {
            get { return _Ingles; }
            set { _Ingles = value; }
        }

        public bool Frances
        {
            get { return _Frances; }
            set { _Frances = value; }
        }

        public bool Aleman
        {
            get { return _Aleman; }
            set { _Aleman = value; }
        }

        public bool Portugues
        {
            get { return _Portugues; }
            set { _Portugues = value; }
        }

        public bool Ecuador
        {
            get { return _Ecuador; }
            set { _Ecuador = value; }
        }

        public bool Chile
        {
            get { return _Chile; }
            set { _Chile = value; }
        }

        public bool Panama
        {
            get { return _Panama; }
            set { _Panama = value; }
        }

        public bool Argentina
        {
            get { return _Argentina; }
            set { _Argentina = value; }
        }

        public bool Colombia
        {
            get { return _Colombia; }
            set { _Colombia = value; }
        }

        public bool Peru
        {
            get { return _Peru; }
            set { _Peru = value; }
        }

        public bool Uruguay
        {
            get { return _Uruguay; }
            set { _Uruguay = value; }
        }

        public bool Venezuela
        {
            get { return _Venezuela; }
            set { _Venezuela = value; }
        }

        public bool Noruego
        {
            get { return _Noruego; }
            set { _Noruego = value; }
        }

        private void CargarLocale()
        {
            string idioma = "";
            string valor = Request.QueryString["locale"];

            if (valor == null)
            {
                switch (idioma)
                {
                    case
                        "es":
                        idioma = "es-ES";
                        this._Español = true;
                        break;
                    case
                        "en":
                        idioma = "en-US";
                        this._Ingles = true;
                        break;
                    case
                        "fr":
                        idioma = "fr-FR";
                        this._Frances = true;
                        break;
                    case
                        "pt":
                        idioma = "pt-PT";
                        this._Portugues = true;
                        break;
                    case
                        "de":
                        idioma = "de-DE";
                        this._Aleman = true;
                        break;
                    case
                        "ec":
                        idioma = "es-EC";
                        this._Ecuador = true;
                        break;
                    case
                        "cl":
                        idioma = "es-CL";
                        this._Chile = true;
                        break;
                    case
                        "pa":
                        idioma = "es-PA";
                        this._Panama = true;
                        break;
                    case
                        "ar":
                        idioma = "es-AR";
                        this._Argentina = true;
                        break;
                    case
                        "co":
                        idioma = "es-CO";
                        this._Colombia = true;
                        break;
                    case
                        "pe":
                        idioma = "es-PE";
                        this._Peru = true;
                        break;
                    case
                        "uy":
                        idioma = "es-UY";
                        this._Uruguay = true;
                        break;
                    case
                        "ve":
                        idioma = "es-VE";
                        this._Venezuela = true;
                        break;
                    case
                        "no":
                        idioma = "no-NO";
                        this._Noruego = true;
                        break;
                    default:
                        idioma = Comun.DefaultLocale;
                        this._Español = true;
                        break;
                }
            }
            else
            {
                switch (valor)
                {
                    case
                        "es-ES":
                        idioma = "es-ES";
                        _Español = true;
                        break;
                    case
                        "en-US":
                        idioma = "en-US";
                        _Ingles = true;
                        break;
                    case
                        "fr-FR":
                        idioma = "fr-FR";
                        _Frances = true;
                        break;
                    case
                        "pt-PT":
                        idioma = "pt-PT";
                        _Portugues = true;
                        break;
                    case
                        "de-DE":
                        idioma = "de-DE";
                        _Aleman = true;
                        break;
                    case
                        "es-EC":
                        idioma = "es-EC";
                        _Ecuador = true;
                        break;
                    case
                        "es-CL":
                        idioma = "es-CL";
                        _Chile = true;
                        break;
                    case
                        "es-PA":
                        idioma = "es-PA";
                        _Panama = true;
                        break;
                    case
                        "es-AR":
                        idioma = "es-AR";
                        _Argentina = true;
                        break;
                    case
                        "es-CO":
                        idioma = "es-CO";
                        _Colombia = true;
                        break;
                    case
                        "es-PE":
                        idioma = "es-PE";
                        _Peru = true;
                        break;
                    case
                        "es-UY":
                        idioma = "es-UY";
                        _Uruguay = true;
                        break;
                    case
                        "es-VE":
                        idioma = "es-VE";
                        _Venezuela = true;
                        break;
                    case
                        "no-NO":
                        idioma = "no-NO";
                        _Noruego = true;
                        break;
                    default:
                        valor = Comun.DefaultLocale;
                        _Español = true;
                        break;
                }
            }

            if (valor != null)
            {
                _Locale = valor;
                Session["LOCALE"] = valor;
            }
            else
            {
                _Locale = idioma;
                Session["LOCALE"] = idioma;
            }
        }

        #endregion

        #region EVENTOS DE PAGINA

        protected void Page_Init(object sender, EventArgs e)
        {
            string AccesoSinMail = GetLocalResourceObject("strAccesoRestringido").ToString();
            this.Title = Comun.TituloPaginas;
            _Español = false;
            _Ingles = false;
            _Frances = false;
            _Aleman = false;
            _Portugues = false;
            _Ecuador = false;
            _Chile = false;
            _Panama = false;
            _Argentina = false;
            _Colombia = false;
            _Peru = false;
            _Uruguay = false;
            _Venezuela = false;
            _Noruego = false;

            if (Request.QueryString["locale"] != null)
            {
                jsClaveValidacionMsg1.Value = GetLocalResourceObject("jsClaveValidacionMsg1").ToString();
                jsClaveValidacionMsg2.Value = GetLocalResourceObject("jsClaveValidacionMsg2").ToString();
                jsClaveNoCorrespondencia.Value = GetLocalResourceObject("jsClaveNoCorrespondencia").ToString();
                jsTituloAtencion.Value = GetLocalResourceObject("jsTituloAtencion").ToString();

                winCambiarClave.Title = GetLocalResourceObject("winCambiarClave.Title").ToString();
                txtCambiarClave.FieldLabel = GetLocalResourceObject("txtCambiarClave.FieldLabel").ToString();
                txtCambiarClave2.FieldLabel = GetLocalResourceObject("txtCambiarClave2.FieldLabel").ToString();
                btnCambiar.Text = GetLocalResourceObject("btnCambiar.Text").ToString();
            }

            if (Request.QueryString["emailuser"] != null)
            {
                UsuariosController cUsuarios = new UsuariosController();
                Data.Usuarios dato = new Data.Usuarios();

                dato = cUsuarios.getUsuarioByEmail(Request.QueryString["emailuser"]);

                if (dato != null)
                {
                    emailUser = Request.QueryString["emailuser"];
                }
                else
                {
                    winCambiarClave.Hidden = true;
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        { }

        #endregion

        [DirectMethod]
        public DirectResponse ResetearContraseña()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            string temp = null;
            Data.Usuarios UsNuevaContraseña = new Data.Usuarios();
            UsuariosController cUsuarios = new UsuariosController();
            UsNuevaContraseña = cUsuarios.getUsuarioByEmail(emailUser);

            try
            {
                temp = cUsuarios.UsuariosCambiarClave(UsNuevaContraseña.UsuarioID, txtCambiarClave.Text, false);

                if (temp == "")
                {
                    ProyectosTiposController cProyTipo = new ProyectosTiposController();
                    Data.ProyectosTipos ptip = new Data.ProyectosTipos();
                    ptip = cProyTipo.GetProyectosTiposByNombre("GLOBAL");
                    Util.EscribeEstadistica(UsNuevaContraseña.UsuarioID, ptip.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], TreeCore.Properties.Settings.Default.EscribeEstadistica, "RESETEO CONTRASEÑA");
                }
                else
                {
                    // MENSAJE DE ERROR
                }
            }
            catch (Exception)
            {
                ajax.Success = false;
                return ajax;
            }

            return ajax;
        }
    }
}