using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using TreeCore.Data;
using System.Globalization;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class Usuarios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersDocumentos, storeDocumentos, gridAddRemoveDocumentos.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFilters_PerfilesAgregados, storeRoles, gridPerfilesAgregados.ColumnModel, listaIgnore, _Locale);
                //Comun.CreateGridFilters(gridFilters_PermisosAgregados, storePermisosAgregados, gridPermisos.ColumnModel, listaIgnore, _Locale);


                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));


                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmpFiltro.ClienteID = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                    cmpFiltro.ClienteID = ClienteID.Value;
                }
                storePrincipal.Reload();
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];
                string sEntidad = Request.QueryString["aux3"].ToString();

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        bool bActivo = Request.QueryString["aux"] == "true";

                        var listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID, bActivo, sEntidad);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        Response.Write("ERROR: " + ex.Message);
                    }

                    Response.End();
                }
                storePrincipal.Reload();
                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion

            #region DESCARGAR DOCUMENTO
            if (Request.QueryString["DescargarDocumento"] != null)
            {
                DocumentosController cDocumentos = new DocumentosController();
                Data.Documentos doc = cDocumentos.GetItem(long.Parse(Request.QueryString["DescargarDocumento"]));

                if (doc != null)
                {
                    string ruta = cDocumentos.ObtenerRutaDocumento((long)doc.DocumentTipoID);
                    string sPathDebug = Path.Combine(ruta, doc.Archivo);

                    if (File.Exists(sPathDebug))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(sPathDebug);

                        Response.Clear();
                        Response.ContentType = Comun.GetMimeType(file.Extension);
                        Response.AddHeader("content-disposition", "attachment; filename=" + doc.Documento + file.Extension);
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.TransmitFile(sPathDebug);
                        Response.Flush();
                    }

                }

                Response.SuppressContent = true;
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

            }

            #endregion


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar }},
                { "Post", new List<ComponentBase> { btnAnadir, btnDuplicarUsuario }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnPassword, btnDesvincularDispositivoMovil, btnDuplicarConfig }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #endregion

        #region DIRECT METHODS

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar, bool bDuplicar)
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            InfoResponse oResponse;
            long lCliID;

            try
            {
                if (!bAgregar)
                {
                    #region Editar

                    long lS = 0;

                    if (hdUsuarioID.Value.ToString() != "")
                    {
                        lS = long.Parse(hdUsuarioID.Value.ToString());
                    }
                    else
                    {
                        lS = long.Parse(GridRowSelect.SelectedRecordID);
                    }

                    Data.Usuarios oDato;
                    oDato = cUsuarios.GetItem(lS);

                    if (oDato != null)
                    {
                        oDato.Nombre = txtNombre.Text;
                        oDato.Apellidos = txtApellidos.Text;
                        oDato.Telefono = txtTelefono.Text;
                        oDato.EMail = txtEMail.Text;

                        txtClave.Disable();
                        txtClave.AllowBlank = true;
                        txtClave.Hidden = true;
                        oDato.EntidadID = long.Parse(cmbEntidad.SelectedItem.Value.ToString());

                        if (!txtFechaCaducidadUsuario.SelectedDate.Equals(DateTime.MinValue))
                        {
                            oDato.FechaCaducidadUsuario = txtFechaCaducidadUsuario.SelectedDate;
                        }

                        oResponse = cUsuarios.UpdateUser(oDato, GetLocalResourceObject("strEditarUsuario").ToString());

                        if (oResponse.Result)
                        {
                            oResponse = cUsuarios.SubmitChanges();

                            if (oResponse.Result)
                            {
                                direct.Success = true;
                                direct.Result = GetGlobalResource(oResponse.Description);
                                log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            }
                            else
                            {
                                cUsuarios.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            cUsuarios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Agregar

                    lCliID = long.Parse(hdCliID.Value.ToString());
                    txtClave.Enable();
                    txtClave.AllowBlank = false;
                    txtClave.Hidden = false;

                    Data.Usuarios dato = new Data.Usuarios();
                    dato.Nombre = txtNombre.Text;
                    dato.Apellidos = txtApellidos.Text;
                    dato.Activo = true;
                    dato.EMail = txtEMail.Text;
                    dato.Telefono = txtTelefono.Text;
                    dato.CambiarClave = true;
                    dato.FechaCreacion = DateTime.Now;
                    dato.UltimasClaves = "";
                    dato.EntidadID = long.Parse(cmbEntidad.SelectedItem.Value);

                    if (!txtFechaCaducidadUsuario.SelectedDate.Equals(DateTime.MinValue))
                    {
                        dato.FechaCaducidadUsuario = txtFechaCaducidadUsuario.SelectedDate;
                    }

                    if (Usuario.ClienteID == null)
                    {

                        if (cmpFiltro.ClienteID != 0 && (cmpFiltro.ClienteID.ToString() != "" || cmpFiltro.ClienteID.ToString() != "0"))
                        {
                            dato.ClienteID = cmpFiltro.ClienteID;
                        }
                    }
                    else if (ClienteID.HasValue)
                    {
                        dato.ClienteID = ClienteID.Value;
                    }

                    dato.FechaUltimoCambio = DateTime.Today.Date;
                    dato.Clave = Tree.Utiles.md5.MD5String(txtClave.Text);

                    oResponse = cUsuarios.AddUser(dato, GetLocalResourceObject("strAgregarUsuario").ToString());

                    if (oResponse.Result)
                    {
                        oResponse = cUsuarios.SubmitChanges();

                        if (oResponse.Result)
                        {
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);

                            string contenidoEmail = TreeCore.Email.GetPlantillaEmailAddUser(txtNombre.Text, txtEMail.Text, txtClave.Text);
                            string correcto = TreeCore.Email.SendMail(txtEMail.Text, "TREE", "", "Welcome to TREE", "", contenidoEmail, "");

                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            hdUsuarioID.Value = long.Parse(dato.UsuarioID.ToString());
                            txtClave.Disable();
                            txtClave.AllowBlank = true;
                            txtClave.Hidden = true;

                            if (bDuplicar)
                            {
                                long lS = long.Parse(GridRowSelect.SelectedRecordID);
                                oResponse = cUsuarios.AddDuplicate(lS, dato);

                                if (oResponse.Result)
                                {
                                    oResponse = cUsuarios.SubmitChanges();

                                    if (oResponse.Result)
                                    {
                                        direct.Success = true;
                                        direct.Result = GetGlobalResource(oResponse.Description);
                                    }
                                    else
                                    {
                                        cUsuarios.DiscardChanges();
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(oResponse.Description);
                                    }
                                }
                                else
                                {
                                    cUsuarios.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                        }
                        else
                        {
                            cUsuarios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cUsuarios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }

                    #endregion
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

        [DirectMethod()]
        public DirectResponse ComprobarUsuarioExiste()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();

            try
            {
                Data.Usuarios oUser = cUsuarios.getUserByEmail(txtEMail.Text);

                if (oUser != null)
                {
                    if (cUsuarios.RegistroDuplicado(oUser))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsEmailExiste);
                        return direct;
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = "";
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            hdUsuarioID.Value = "";

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Vw_Usuarios oDato;
                oDato = cUsuarios.GetItem<Data.Vw_Usuarios>(lS);

                if (oDato != null)
                {
                    txtNombre.Text = oDato.Nombre;
                    txtApellidos.Text = oDato.Apellidos;
                    txtTelefono.Text = oDato.Telefono;
                    txtEMail.Text = oDato.EMail;
                    cmbEntidad.SetValue(oDato.EntidadID.ToString());
                    txtFechaCaducidadUsuario.Text = oDato.FechaCaducidadUsuario.ToString();

                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Usuarios oUser = cUsuarios.GetItem(lID);
                oResponse = cUsuarios.Delete(oUser);

                if (oResponse.Result)
                {
                    oResponse = cUsuarios.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.Reload();
                    }
                    else
                    {
                        cUsuarios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cUsuarios.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cController = new UsuariosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Usuarios oDato = cController.GetItem(lID);
                oResponse = cController.ModificarActivar(oDato, GetLocalResourceObject("strActivarUsuario").ToString(), GetLocalResourceObject("strDesactivarUsuario").ToString());

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada) + ": " + oDato.EMail);
                        storePrincipal.Reload();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod]
        public DirectResponse CambiarClave()
        {
            DirectResponse ajax = new DirectResponse();
            try
            {
                try
                {

                    long S = Convert.ToInt64(GridRowSelect.SelectedRecordID);
                    UsuariosController cUsuarios = new UsuariosController();
                    string temp = "";
                    if (txtCambiarClave.Text.ToString() == txtCambiarClave2.Text.ToString())

                    {
                        temp = cUsuarios.UsuariosCambiarClave(S, txtCambiarClave.Text.ToString());
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

            ajax.Success = true;
            ajax.Result = "";
            return ajax;
        }

        [DirectMethod]
        public DirectResponse DuplicarConfig()
        {
            DirectResponse ajax = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            InfoResponse oResponse;
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                long lIDOrigen = Int64.Parse(GridRowSelect.SelectedRecordID);
                long lIDDestino = Int64.Parse(cmbUsuarios.SelectedItem.Value);

                oResponse = cUsuarios.AddDuplicateConfig(lIDOrigen, lIDDestino);

                if (oResponse.Result)
                {
                    oResponse = cUsuarios.SubmitChanges();

                    if (oResponse.Result)
                    {
                        ajax.Success = true;
                        ajax.Result = GetGlobalResource(oResponse.Description);

                        storePrincipal.Reload();
                        winDuplicarConfig.Hidden = true;
                    }
                    else
                    {
                        cUsuarios.DiscardChanges();
                        ajax.Success = false;
                        ajax.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cUsuarios.DiscardChanges();
                    ajax.Success = false;
                    ajax.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod]
        public DirectResponse GuardarPermiso()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuario = new UsuariosController();
            ProyectosTiposController cProyTip = new ProyectosTiposController();
            UsuariosProyectosController cUsuariosProyectos = new UsuariosProyectosController();

            try
            {
                long lCliID = 0;
                if (Usuario.ClienteID != null)
                {
                    lCliID = (long)Usuario.ClienteID;
                }
                else if (cmpFiltro.ClienteID.ToString() != "" && cmpFiltro.ClienteID.ToString() != null)
                {
                    lCliID = Convert.ToInt32(cmpFiltro.ClienteID);
                }

                long lUsuID = 0;
                if (hdUsuarioID.Value.ToString() != "")
                {
                    lUsuID = Convert.ToInt32(hdUsuarioID.Value);
                }
                else
                {
                    lUsuID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                }

                //TAB PERMISOS

                //if (lCliID != 0 && lUsuID != 0)
                //{
                //    if (cmbProyectos.SelectedItem != null && cmbProyectos.SelectedItem.Value != null && cmbProyectos.SelectedItems.Count > 0)
                //    {
                //        foreach (Ext.Net.ListItem proyecto in cmbProyectos.SelectedItems)
                //        {
                //            Data.UsuariosProyectos dato = new TreeCore.Data.UsuariosProyectos();
                //            dato.ClienteID = lCliID;
                //            dato.UsuarioID = lUsuID;
                //            dato.ProyectoID = long.Parse(proyecto.Value);
                //            if (cmbProyectosAgrupaciones.SelectedItem.Value != null)
                //            {
                //                dato.ProyectoAgrupacionID = long.Parse(cmbProyectosAgrupaciones.SelectedItem.Value.ToString());
                //            }

                //            dato = cUsuariosProyectos.AddItem(dato);
                //        }
                //    }
                //    else if (cmbProyectosAgrupaciones.SelectedItem.Value != null && cmbProyectosAgrupaciones.SelectedItem.Value.ToString() != "")
                //    {
                //        Data.UsuariosProyectos dato = new TreeCore.Data.UsuariosProyectos();
                //        dato.ClienteID = lCliID;
                //        dato.UsuarioID = lUsuID;
                //        dato.ProyectoAgrupacionID = long.Parse(cmbProyectosAgrupaciones.SelectedItem.Value.ToString());
                //        dato = cUsuariosProyectos.AddItem(dato);
                //    }

                //}

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

        [DirectMethod]
        public DirectResponse QuitarPermisos(long lS)
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuario = new UsuariosController();
            UsuariosProyectosController cUsuariosProyectos = new UsuariosProyectosController();
            Data.UsuariosProyectos dato = new TreeCore.Data.UsuariosProyectos();

            try
            {

                Data.UsuariosProyectos oDatoPermiso = default(Data.UsuariosProyectos);

                oDatoPermiso = cUsuariosProyectos.GetItem(lS);
                dato = (from c in cUsuariosProyectos.Context.UsuariosProyectos where c.UsuariosProyectosID == lS select c).First();
                cUsuariosProyectos.DeleteItem(lS);

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

        [DirectMethod]
        public DirectResponse QuitarRoles(long lS)
        {
            DirectResponse direct = new DirectResponse();
            UsuariosRolesController cUsuariosRoles = new UsuariosRolesController();
            InfoResponse oResponse;

            try
            {
                Data.UsuariosRoles oRolUser = cUsuariosRoles.GetItem(lS);

                if (oRolUser != null)
                {
                    oResponse = cUsuariosRoles.Delete(oRolUser);

                    if (oResponse.Result)
                    {
                        oResponse = cUsuariosRoles.SubmitChanges();

                        if (oResponse.Result)
                        {
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        }
                        else
                        {
                            cUsuariosRoles.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cUsuariosRoles.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            //winGestionUsuarios.DoAutoRender();
            //winGestionUsuarios.Update();
            //winGestionUsuarios.Reload();

            return direct;
        }

        [DirectMethod]
        public DirectResponse AgregarRoles()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosRolesController cUsuarioRoles = new UsuariosRolesController();
            InfoResponse oResponse;

            direct.Success = true;
            direct.Result = "";
            try
            {
                long usID = 0;

                if (hdUsuarioID.Value.ToString() != "")
                {
                    usID = Convert.ToInt32(hdUsuarioID.Value.ToString());
                }
                else
                {
                    usID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                }

                if (cmbRolesLibres.SelectedItems.Count > 0)
                {
                    foreach (Ext.Net.ListItem rol in cmbRolesLibres.SelectedItems)
                    {
                        Data.UsuariosRoles dato = new Data.UsuariosRoles();
                        dato.RolID = long.Parse(rol.Value);
                        dato.UsuarioID = usID;

                        oResponse = cUsuarioRoles.Add(dato);

                        if (oResponse.Result)
                        {
                            oResponse = cUsuarioRoles.SubmitChanges();

                            if (oResponse.Result)
                            {
                                direct.Success = true;
                                direct.Result = GetGlobalResource(oResponse.Description);
                                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            }
                            else
                            {
                                cUsuarioRoles.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                                return direct;
                            }
                        }
                        else
                        {
                            cUsuarioRoles.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                            return direct;
                        }
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource("strSeleccionePerfil");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse AgregarDocumento()
        {
            DocumentosController cDocumentos = new DocumentosController();
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            try
            {
                long lUsuarioID = 0;
                if (hdUsuarioID.Value.ToString() != "")
                {
                    lUsuarioID = long.Parse(hdUsuarioID.Value.ToString());
                }
                else
                {
                    lUsuarioID = long.Parse(GridRowSelect.SelectedRecordID);
                }

                if (cmbDocumentosTipos.SelectedItem.Value != null && cmbDocumentosTipos.SelectedItem.Value.ToString() != "")
                {
                    if (uploadFieldDocumento.HasFile)
                    {
                        DocumentosEstadosController cDocumentosEstados = new DocumentosEstadosController();
                        DocumentosEstados DocEstado = cDocumentosEstados.GetDefecto();

                        if (DocEstado != null)
                        {
                            if (cDocumentos.GuardarDocumento(uploadFieldDocumento, null, Usuario.UsuarioID, (long)Comun.Modulos.GLOBAL, null, lUsuarioID, uploadFieldDocumento.PostedFile.FileName, long.Parse(cmbDocumentosTipos.SelectedItem.Value.ToString()), uploadFieldDocumento.PostedFile.FileName, null, DocEstado, ""))
                            {

                            }
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strDocumentoEstadoPorDefectoNoEncontrado);
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource("strSeleccioneDocumento");
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource("strSeleccioneTipoDocumento");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse DesvincularDispositivoMovil()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cController = new UsuariosController();
            InfoResponse oResponse;
            MonitoringModificacionesUsuariosController cMonitoringModificacionesUsuarios = new MonitoringModificacionesUsuariosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Usuarios oDato = cController.GetItem(lID);
                oDato.MacDispositivo = null;

                oResponse = cController.UpdateUser(oDato, GetGlobalResource("strDesvincularDispositivoMovil").ToString());

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                    return direct;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod]
        public string LoadPrefijos()
        {
            DirectResponse direct = new DirectResponse();
            List<Ext.Net.MenuItem> items = new List<Ext.Net.MenuItem>();
            PaisesController cPaises = new PaisesController();

            try
            {
                items = cPaises.GetMenuItemsPrefijos(ClienteID.Value);
                items.ForEach(i =>
                {
                    Icon icono = (Icon)Enum.Parse(typeof(Icon), i.Icon.ToString());
                    ResourceManagerTreeCore.RegisterIcon(icono);
                });
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return ComponentLoader.ToConfig(items);
        }

        #endregion

        #region STORES

        #region STORE PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    bool bEstado = btnActivo.Pressed;
                    string sEntidad = "";

                    if (cmbEntidades.SelectedItem.Value != null && cmbEntidades.SelectedItem.Value != "")
                    {
                        sEntidad = cmbEntidades.SelectedItem.Value;
                    }

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cmpFiltro.ClienteID, bEstado, sEntidad);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;
                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<object> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lClienteID, bool bEstado, string sEntidad)
        {
            List<Vw_Usuarios> listaDatos;
            List<Data.Usuarios> lUsuarios;
            List<object> listaResultado;
            UsuariosController cUsuarios = new UsuariosController();


            try
            {
                if (long.Parse(lClienteID.ToString()) != 0)
                {
                    lUsuarios = cUsuarios.GetActivos(long.Parse(lClienteID.ToString()));

                    if (bEstado)
                    {
                        if (sEntidad != "" && sEntidad != "null")
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == True && ClienteID == " + lClienteID + "&& EntidadID == " + sEntidad);
                        }
                        else
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == True && ClienteID == " + lClienteID);
                        }

                    }
                    else
                    {
                        if (sEntidad != "" && sEntidad != "null")
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID + "&& EntidadID == " + sEntidad);
                        }
                        else
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                        }

                    }
                }
                else
                {
                    lUsuarios = cUsuarios.GetActivos(0);
                    if (bEstado)
                    {
                        if (sEntidad != "" && sEntidad != "null")
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == True && ClienteID == NULL && EntidadID == " + sEntidad);
                        }
                        else
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == True && ClienteID == NULL");
                        }

                    }
                    else
                    {
                        if (sEntidad != "" && sEntidad != "null")
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == NULL && EntidadID == " + sEntidad);
                        }
                        else
                        {
                            listaDatos = cUsuarios.GetItemsWithExtNetFilterList<Data.Vw_Usuarios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == NULL");
                        }

                    }
                }

                //Filtro resultados KPI
                if (listaDatos != null && listIdsResultadosKPI != null)
                {
                    listaDatos = cUsuarios.FiltroListaPrincipalByIDs(listaDatos.Cast<object>().ToList(), listIdsResultadosKPI, nameIndiceID).Cast<Vw_Usuarios>().ToList();
                }

                listaResultado = (from c in listaDatos
                                  join d in lUsuarios on c.UsuarioID equals d.UsuarioID
                                  select new
                                  {
                                      UsuarioID = c.UsuarioID,
                                      Activo = c.Activo,
                                      Nombre = c.Nombre,
                                      Apellidos = c.Apellidos,
                                      Telefono = c.Telefono,
                                      EMail = c.EMail,
                                      NombreEntidad = c.NombreEntidad,
                                      Clave = c.Clave,
                                      LDAP = c.LDAP,
                                      Interno = c.Interno,
                                      Soporte = c.Soporte,
                                      UsuarioOAMID = c.UsuarioOAMID,
                                      FechaUltimoAcceso = c.FechaUltimoAcceso,
                                      FechaUltimoCambio = c.FechaUltimoCambio,
                                      FechaCaducidadUsuario = c.FechaCaducidadUsuario,
                                      FechaCaducidadClave = c.FechaCaducidadClave,
                                      MacDispositivo = d.MacDispositivo,
                                  }).ToList<object>();
            }
            catch (Exception ex)
            {
                listaResultado = null;
                log.Error(ex.Message);
            }

            return listaResultado;
        }

        #endregion

        #region STORE ENTIDADES

        protected void storeEntidades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //Recupera los datos y los establece
                    var ls = ListaEntidades();
                    if (ls != null)
                    {
                        storeEntidades.DataSource = ls;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Entidades> ListaEntidades()
        {
            List<Data.Entidades> listadatos;
            try
            {
                EntidadesController mControl = new EntidadesController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                if (lCliID != 0)
                {
                    listadatos = mControl.GetAllEntidadesByClienteID(lCliID);
                }
                else
                {
                    listadatos = mControl.GetAllEntidades();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region Proyectos
        protected void storeProyectos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                PerfilesController cUsuariosRoles = new PerfilesController();
                ProyectosController cProyectos = new ProyectosController();

                try
                {
                    List<Data.Proyectos> lista = new List<Data.Proyectos>();
                    List<long?> lProyectosTiposID = new List<long?>();
                    long lUsuarioID = 0;
                    long CliID = 0;
                    long? agrupacionID = null;

                    if (ClienteID.HasValue)
                    {
                        CliID = ClienteID.Value;
                    }
                    else
                    {
                        if (cmpFiltro.ClienteID.ToString() != null && cmpFiltro.ClienteID != 0)
                        {
                            CliID = cmpFiltro.ClienteID;
                        }
                    }

                    // TAB PERMISOS

                    //if (cmbProyectosAgrupaciones.SelectedItem.Value != null && Int32.Parse(cmbProyectosAgrupaciones.SelectedItem.Value.ToString()) != 0)
                    //{
                    //    agrupacionID = Int32.Parse(cmbProyectosAgrupaciones.SelectedItem.Value.ToString());
                    //}

                    //Get Proyectos By Agrupacion y ProyectosTiposByPerfil
                    if (hdUsuarioID.Value.ToString() != "")
                    {
                        lUsuarioID = long.Parse(hdUsuarioID.Value.ToString());
                    }
                    else
                    {
                        lUsuarioID = long.Parse(GridRowSelect.SelectedRecordID);
                    }

                    if (CliID != 0 && lUsuarioID != 0)
                    {
                        lProyectosTiposID = cUsuariosRoles.GetProyectosTiposByPerfilesUsuario(lUsuarioID);

                        if (lProyectosTiposID != null && lProyectosTiposID.Count > 0)
                        {
                            List<long?> lProyectosAsignados = new List<long?>();
                            lProyectosAsignados = cProyectos.GetProyectosAsignadosByUsuario(lUsuarioID);

                            lista = cProyectos.GetProyectosNoAsignadosByClienteAgrupacionListaProyectosTipos(CliID, agrupacionID, lProyectosTiposID, lProyectosAsignados);
                        }
                    }

                    if (lista != null)
                    {
                        storeProyectos.DataSource = lista;
                    }

                    cProyectos = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region "PROYECTOS AGRUPACIONES"

        private List<Data.ProyectosAgrupaciones> ListaProyectosAgrupaciones()
        {
            List<Data.ProyectosAgrupaciones> datos = new List<Data.ProyectosAgrupaciones>();
            ProyectosAgrupacionesController cProyectosAgrupaciones = new ProyectosAgrupacionesController();
            try
            {
                long CliID = 0;
                if (ClienteID.HasValue)
                {
                    CliID = ClienteID.Value;
                }
                else
                {
                    if (cmpFiltro.ClienteID.ToString() != null && cmpFiltro.ClienteID != 0)
                    {
                        CliID = cmpFiltro.ClienteID;
                    }
                }

                long lUsuarioID = 0;
                if (hdUsuarioID.Value.ToString() != "")
                {
                    lUsuarioID = long.Parse(hdUsuarioID.Value.ToString());
                }
                else
                {
                    lUsuarioID = long.Parse(GridRowSelect.SelectedRecordID);
                }

                if (lUsuarioID != 0 && CliID != 0)
                {
                    List<long?> lAgrupacionesAsignadasID = new List<long?>();
                    lAgrupacionesAsignadasID = cProyectosAgrupaciones.GetAgrupacionesAsignadasByUsuario(lUsuarioID);
                    datos = cProyectosAgrupaciones.getClientesProyectosAgrupacionesPermisosNoAsignados(CliID, lAgrupacionesAsignadasID);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return datos;
        }

        protected void storeProyectosAgrupaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos

                try
                {
                    var ls = ListaProyectosAgrupaciones();
                    if (ls != null)
                        storeProyectosAgrupaciones.DataSource = ls;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

            }
        }

        #endregion

        #region PERMISOS AGREGADOS
        protected void storePermisosAgregados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (hdUsuarioID.Value.ToString() != "")
                    {
                        long s = long.Parse(hdUsuarioID.Value.ToString());
                        var ls = ListaClientesProyectos(s);
                        if (ls != null)
                        {
                            storePermisosAgregados.DataSource = ls;
                        }
                    }
                    else
                    {
                        long s = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                        var ls = ListaClientesProyectos(s);
                        if (ls != null)
                        {
                            storePermisosAgregados.DataSource = ls;
                        }
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ClientesProyectosZonas> ListaClientesProyectos(long usuarioID)
        {
            List<Data.Vw_ClientesProyectosZonas> datos = new List<Data.Vw_ClientesProyectosZonas>();
            UsuariosController cUsuarios = new UsuariosController();
            try
            {
                datos = cUsuarios.GetClientesProyectosZonas(usuarioID);

            }
            catch (Exception)
            {
                datos = null;
            }
            return datos;
        }

        #endregion

        #region STORE USUARIOS

        protected void storeUsuarios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                string sort;
                List<Data.Vw_Usuarios> datos = new List<Data.Vw_Usuarios>();
                UsuariosController uController = new UsuariosController();
                try
                {
                    long cliID = 0;
                    if (ClienteID == null || ClienteID == 0)
                    {

                        cliID = 0;
                    }
                    else
                    {
                        cliID = (long)ClienteID;
                    }
                    sort = e.Sort.ToString();

                    datos = uController.GetUsersActivos(cliID);

                    if (datos != null)
                        storeUsuarios.DataSource = datos;

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

            }
        }

        #endregion

        #region ROLES
        protected void storeRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                RolesController cUsuariosRoles = new RolesController();
                try
                {

                    List<Data.Vw_UsuariosRoles> lista = new List<Data.Vw_UsuariosRoles>();

                    if (hdUsuarioID.Value.ToString() != "")
                    {
                        long lS = long.Parse(hdUsuarioID.Value.ToString());
                        lista = cUsuariosRoles.GetAllRolesByUsuario(lS);
                    }
                    else
                    {
                        long lS = long.Parse(GridRowSelect.SelectedRecordID);
                        lista = cUsuariosRoles.GetAllRolesByUsuario(lS);
                    }

                    if (lista != null)
                    {
                        storeRoles.DataSource = lista;
                        storeRoles.DataBind();

                        if (lista.Count > 0)
                        {
                            btnNext.Text = GetGlobalResource("jsGuardado");
                            btnNext.IconCls = "ico-tic-wh";
                            btnNext.Enable();
                        }
                        else
                        {
                            btnNext.Text = GetGlobalResource("strGuardar");
                            btnNext.IconCls = "";
                            btnNext.Disable();
                        }
                    }

                    cUsuariosRoles = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region ROLES LIBRES
        protected void storeRolesLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                UsuariosRolesController cUsuariosRoles = new UsuariosRolesController();
                try
                {
                    List<Data.Roles> lista = new List<Data.Roles>();
                    long cliID = 0;
                    if (hdUsuarioID.Value.ToString() != "")
                    {
                        long lS = long.Parse(hdUsuarioID.Value.ToString());
                        cliID = long.Parse(hdCliID.Value.ToString());
                        lista = cUsuariosRoles.perfilesNoAsignado(lS, cliID);
                    }
                    else
                    {
                        long lS = long.Parse(GridRowSelect.SelectedRecordID);
                        cliID = long.Parse(hdCliID.Value.ToString());
                        lista = cUsuariosRoles.perfilesNoAsignado(lS, cliID);
                    }

                    if (lista != null)
                    {
                        storeRolesLibres.DataSource = lista;
                    }

                    cUsuariosRoles = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                }
            }
        }
        #endregion

        #region DOCUMENTOS TIPOS
        protected void storeTiposDocumentos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentTiposController cDocTipos = new DocumentTiposController();
                try
                {
                    List<Data.Vw_DocumentTipos> lDocumentTipos = new List<Data.Vw_DocumentTipos>();
                    lDocumentTipos = cDocTipos.GetDocumentosTiposByProyectoTipoID((long)Comun.Modulos.GLOBAL);

                    if (lDocumentTipos != null)
                    {
                        storeDocumentosTipos.DataSource = lDocumentTipos;
                        storeDocumentosTipos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region DOCUMENTOS
        protected void storeDocumentos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentosController cDoc = new DocumentosController();
                try
                {
                    long lUsuarioID = 0;
                    if (hdUsuarioID.Value.ToString() != "")
                    {
                        lUsuarioID = long.Parse(hdUsuarioID.Value.ToString());
                    }
                    else
                    {
                        lUsuarioID = long.Parse(GridRowSelect.SelectedRecordID);
                    }

                    if (lUsuarioID != 0)
                    {
                        List<Data.Vw_Documentos> lDocumentos = new List<Data.Vw_Documentos>();
                        lDocumentos = cDoc.GetDocumentosByUsuarioID(lUsuarioID);

                        if (lDocumentos != null)
                        {
                            storeDocumentos.DataSource = lDocumentos;
                            storeDocumentos.DataBind();
                        }
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


    }
}