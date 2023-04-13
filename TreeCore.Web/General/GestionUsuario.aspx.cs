using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using TreeCore.Clases;

namespace TreeCore.PaginasComunes
{
    public partial class GestionUsuario : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        string _Locale = Comun.DefaultLocale;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

                if (this.Session["LOCALE"] != null)
                {
                    _Locale = this.Session["LOCALE"].ToString();
                    Comun.AddCurrentCultureJavascript(ResourceManagerTreeCore, this.Culture, "GestionUsuario", null);
                    Comun.cargarVariablesSTRJavascript("Comun", _Locale, false);
                    Comun.CargarVariablesJavascript("Comun", _Locale, false, null);
                    Comun.SetCulture(_Locale);
                }

                lblEditarPerfil.Text = GetGlobalResourceObject("Comun", "strEditarPerfil").ToString();
                hdSeleccionarImagen.Value = GetGlobalResourceObject("Comun", "strSeleccionarImagen").ToString();
                //hdGuardar.Value = GetGlobalResourceObject("Comun", "strGuardar").ToString();
                btnGuardar.Text = GetGlobalResourceObject("Comun", "strGuardar").ToString();
                txtName.FieldLabel = GetGlobalResourceObject("Comun", "strNombre").ToString();
                txtApellidos.FieldLabel = GetGlobalResourceObject("Comun", "strApellidos").ToString();
                txtEmail.FieldLabel = GetGlobalResourceObject("Comun", "strEmail").ToString();
                txtPasswordField.FieldLabel = GetGlobalResourceObject("Comun", "strContraseña").ToString();
                txtPasswordField.Regex = GetGlobalResourceObject("Comun", "strPasswordStrengthRegExp").ToString();
                txtPasswordField.RegexText = GetGlobalResourceObject("Comun", "strPasswordStrengthText").ToString();
                txtPasswordConfirm.FieldLabel = GetGlobalResourceObject("Comun", "strConfirmarContraseña").ToString();
                txtPasswordConfirm.Regex = GetGlobalResourceObject("Comun", "strPasswordStrengthRegExp").ToString();
                txtPasswordConfirm.RegexText = GetGlobalResourceObject("Comun", "strPasswordStrengthText").ToString();
                //FileUpload_TT.Html = GetGlobalResourceObject("Comun", "strSeleccioneImagen").ToString();
                //FileUpload.ButtonText = GetGlobalResourceObject("Comun", "strExaminar").ToString();
                btnPassword.ToolTip = GetGlobalResourceObject("Comun", "strMostrarContraseña").ToString();
                PassMode.ToolTip = GetGlobalResourceObject("Comun", "strMostrarContraseña").ToString();
                hdEditarUsuario.SetValue(GetGlobalResourceObject("Comun", "jsGestionUsuario").ToString());
                hdMensajeEditarUsuario.SetValue(GetGlobalResourceObject("Comun", "jsMensajeEditarUsuario").ToString());
                hdYes.SetValue(GetGlobalResourceObject("Comun", "strYes").ToString());
                hdMensajeClaveDiferente.SetValue(GetGlobalResourceObject("Comun", "jsClaveDiferente").ToString());
                hdMensajeClaveRepetida.SetValue(GetGlobalResourceObject("Comun", "jsClaveRepetida").ToString());
                hdMensajeClaveIncorrecta.SetValue(GetGlobalResourceObject("Comun", "jsClaveIncorrecta").ToString());
                hdMensajePassword.SetValue(GetGlobalResourceObject("Comun", "strPasswordStrengthText").ToString());

                if (oUsuario != null)
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);

                    txtName.Text = oUser.Nombre;
                    txtApellidos.Text = oUser.Apellidos;
                    txtEmail.Text = oUser.EMail;
                    txtPasswordConfirm.Text = txtPasswordField.Text;

                    if (oUser.FechaCaducidadClave != null)
                    {
                        lblFechaClave.Text = GetGlobalResourceObject("Comun", "jsFechaCaducidadClave").ToString();
                        lblFechaClave.Text += " " + ((DateTime)oUser.FechaCaducidadClave).ToString(Comun.FORMATO_FECHA);
                    }

                    string ruta = TreeCore.DirectoryMapping.GetImagenUsuarioDirectory();

                    var files = Directory.GetFiles(ruta, oUser.UsuarioID.ToString() + ".*");

                    if (files.Length > 0)
                    {

                        var tempFiles = Directory.GetFiles(TreeCore.DirectoryMapping.GetImagenUsuarioTempDirectory(), oUsuario.UsuarioID.ToString() + ".*");
                        string extension = files[0].Split('.')[1];
                        string rutaTemp = Path.Combine(DirectoryMapping.GetImagenUsuarioTempDirectory(), oUsuario.UsuarioID.ToString() + '.' + extension);

                        if (tempFiles.Length.Equals(0))
                        {
                            File.Copy(files[0], rutaTemp);
                        }


                        string imagenCargada = "/" + Path.Combine(DirectoryMapping.GetImagenUsuarioTempDirectoryRelative(), oUsuario.UsuarioID.ToString() + '.' + extension);
                        imgUser.Src = imagenCargada;
                    }
                    else
                    {
                        imgUser.Src = "../ima/ico-user-empty.svg";
                    }

                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse EditarPerfil()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            InfoResponse oResponse;
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            var temp = "";

            try
            {
                Regex rgx = new Regex(txtPasswordField.Regex);
                Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);

                if (oUser != null)
                {
                    if (oUser.Nombre != txtName.Text)
                    {
                        oUser.Nombre = txtName.Text;
                        log.Info(GetGlobalResourceObject("Comun", "LogActualizacionNombre"));
                    }

                    if (oUser.Apellidos != txtApellidos.Text)
                    {
                        oUser.Apellidos = txtApellidos.Text;
                        log.Info(GetGlobalResourceObject("Comun", "LogActualizacionApellidos"));
                    }

                    if (txtPasswordField.Text != "" && txtPasswordField.Text != null && txtPasswordConfirm.Text != ""
                        && txtPasswordConfirm.Text != null && (!rgx.IsMatch(txtPasswordField.Text) || !rgx.IsMatch(txtPasswordConfirm.Text)))
                    {
                        temp = "ClaveIncorrecta";
                    }

                    else if (oUser.EMail != txtEmail.Text && txtPasswordField.Text != "" && txtPasswordField.Text != null
                        && txtPasswordConfirm.Text != "" && txtPasswordConfirm.Text != null
                        && txtPasswordField.Text == txtPasswordConfirm.Text)
                    {
                        temp = "logout";
                    }

                    else if (txtPasswordField.Text != "" && txtPasswordField.Text != null && txtPasswordConfirm.Text != ""
                        && txtPasswordConfirm.Text != null && txtPasswordField.Text == txtPasswordConfirm.Text)
                    {
                        temp = "logout";
                    }

                    else if (txtPasswordField.Text != "" && txtPasswordField.Text != null && txtPasswordConfirm.Text != ""
                        && txtPasswordConfirm.Text != null && txtPasswordField.Text != txtPasswordConfirm.Text)
                    {
                        temp = "ClaveDistinta";
                    }

                    else if (oUser.EMail != txtEmail.Text)
                    {
                        temp = "logout";
                    }

                    oResponse = cUsuarios.Update(oUser);

                    if (oResponse.Result)
                    {
                        oResponse = cUsuarios.SubmitChanges();

                        if (oResponse.Result)
                        {
                            hdTieneImagen.Value = "";
                            log.Warn(GetGlobalResourceObject("Comun", "LogActualizacionRealizada"));

                            direct.Success = true;
                            direct.Result = "";
                            direct.ErrorMessage = temp;
                        }
                        else
                        {
                            cUsuarios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                            direct.ErrorMessage = temp;
                        }
                    }
                    else
                    {
                        cUsuarios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                        direct.ErrorMessage = temp;
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResourceObject("Comun", "strMensajeGenerico");
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Logout()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            UsuariosAccesosController cAccesos = new UsuariosAccesosController();
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            string sAviso;
            InfoResponse oResponse;

            direct.Success = true;
            direct.Result = "";

            try
            {
                Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                if (oUser != null)
                {
                    if (oUser.EMail != txtEmail.Text && txtPasswordField.Text != "" && txtPasswordField.Text != null
                        && txtPasswordConfirm.Text != "" && txtPasswordConfirm.Text != null
                        && txtPasswordField.Text == txtPasswordConfirm.Text)
                    {
                        sAviso = cUsuarios.UsuariosCambiarClave(oUser.UsuarioID, txtPasswordConfirm.Text);
                        log.Info(GetGlobalResourceObject("Comun", "LogActualizacionClave"));

                        oUser.EMail = txtEmail.Text;
                        log.Info(GetGlobalResourceObject("Comun", "LogActualizacionEmail"));

                        if (sAviso != "ClaveRepetida" && sAviso != "ClaveIncorrecta")
                        {
                            cAccesos.limpiarSesion(oUsuario.UsuarioID);
                            this.Session["LOGIN"] = false;
                            this.Session["USUARIO"] = null;
                            this.Session["TOKEN"] = null;
                            Cookies.Clear("SesionGUID");

                            oResponse = cUsuarios.Update(oUser);

                            if (oResponse.Result)
                            {
                                oResponse = cUsuarios.SubmitChanges();

                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResourceObject("Comun", "LogActualizacionRealizada"));
                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cUsuarios.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                                }
                            }
                            else
                            {
                                cUsuarios.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                            }
                        }
                        else if (sAviso == "ClaveRepetida")
                        {
                            direct.Result = sAviso;
                            direct.Success = true;
                            direct.ErrorMessage = GetGlobalResourceObject("Comun", "jsClaveRepetida").ToString();
                            return direct;
                        }
                        else if (sAviso == "ClaveIncorrecta")
                        {
                            direct.Result = sAviso;
                            direct.Success = true;
                            direct.ErrorMessage = GetGlobalResourceObject("Comun", "jsClaveIncorrecta").ToString();
                            return direct;
                        }
                    }

                    else if (txtPasswordField.Text != "" && txtPasswordField.Text != null && txtPasswordConfirm.Text != ""
                        && txtPasswordConfirm.Text != null && txtPasswordField.Text == txtPasswordConfirm.Text)
                    {
                        sAviso = cUsuarios.UsuariosCambiarClave(oUser.UsuarioID, txtPasswordConfirm.Text);
                        log.Info(GetGlobalResourceObject("Comun", "LogActualizacionClave"));

                        if (sAviso != "ClaveRepetida" && sAviso != "ClaveIncorrecta")
                        {
                            cAccesos.limpiarSesion(oUsuario.UsuarioID);
                            this.Session["LOGIN"] = false;
                            this.Session["USUARIO"] = null;
                            this.Session["TOKEN"] = null;
                            Cookies.Clear("SesionGUID");

                            oResponse = cUsuarios.Update(oUser);

                            if (oResponse.Result)
                            {
                                oResponse = cUsuarios.SubmitChanges();

                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResourceObject("Comun", "LogActualizacionRealizada"));
                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cUsuarios.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                                }
                            }
                            else
                            {
                                cUsuarios.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                            }
                        }
                        else if (sAviso == "ClaveRepetida")
                        {
                            direct.Result = sAviso;
                            direct.Success = true;
                            direct.ErrorMessage = GetGlobalResourceObject("Comun", "jsClaveRepetida").ToString();
                            return direct;
                        }
                        else if (sAviso == "ClaveIncorrecta")
                        {
                            direct.Result = sAviso;
                            direct.Success = true;
                            direct.ErrorMessage = GetGlobalResourceObject("Comun", "jsClaveIncorrecta").ToString();
                            return direct;
                        }
                    }

                    else if (oUser.EMail != txtEmail.Text)
                    {
                        oUser.EMail = txtEmail.Text;
                        log.Info(GetGlobalResourceObject("Comun", "LogActualizacionEmail"));

                        cAccesos.limpiarSesion(oUsuario.UsuarioID);
                        this.Session["LOGIN"] = false;
                        this.Session["USUARIO"] = null;
                        this.Session["TOKEN"] = null;
                        Cookies.Clear("SesionGUID");

                        oResponse = cUsuarios.Update(oUser);

                        if (oResponse.Result)
                        {
                            oResponse = cUsuarios.SubmitChanges();

                            if (oResponse.Result)
                            {
                                log.Warn(GetGlobalResourceObject("Comun", "LogActualizacionRealizada"));
                                direct.Success = true;
                                direct.Result = "";
                            }
                            else
                            {
                                cUsuarios.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                            }
                        }
                        else
                        {
                            cUsuarios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResourceObject("Comun", oResponse.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResourceObject("Comun", "strMensajeGenerico");
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion

        #region FUNCTIONS

        [DirectMethod]
        public DirectResponse AsignarImagenUsuario()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

            try
            {

                if (hdSRC.Value.ToString() != "data:,")
                {
                    string[] imagen = hdSRC.Value.ToString().Split(',');
                    byte[] bytes = Convert.FromBase64String(imagen[1]);

                    System.Drawing.Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = System.Drawing.Image.FromStream(ms);
                    }

                    cDocumentos.GuardarImagenUsuario(image, oUsuario.UsuarioID, ".jpg");
                }

            }

            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResourceObject("Comun", "strMensajeGenerico");
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        public string ImagenCargada()
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            DocumentosController cDocumentos = new DocumentosController();
            string imagenCargada = "";

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

            if (imagenCargada == "")
            {
                imagenCargada = "../ima/ico-user-empty.svg";
            }

            return imagenCargada;
        }


        [DirectMethod]
        public DirectResponse AsignarSrc()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                //if (!FileUpload.HasFile)
                //{
                //    uploaded-input.Src = ImagenCargada();

                //}
                //else
                //{
                //    direct.Success = false;
                //    return direct;
                //}
            }

            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResourceObject("Comun", "strMensajeGenerico");
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

    }
}