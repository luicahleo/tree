using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using TreeCore.Page;
using TreeCore.Data;

namespace TreeCore.Componentes
{
    public partial class GridEmplazamientosContactosResp : BaseUserControl
    {
        public List<long> listaFuncionalidades = new List<long>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseUserControl currentUC;
        Data.Usuarios oUser;
        private string _vista = "";

        public string IDComponente
        {
            get { return this.hdIDComponente.Value.ToString(); }
            set { this.hdIDComponente.SetValue(value); }
        }

        public long ClienteID
        {
            get { return long.Parse(hdClienteID.Value.ToString()); }
            set { hdClienteID.SetValue(value); }
        }

        public string vista
        {
            get { return _vista; }
            set { this._vista = value;
                hdNombre.SetValue(value);
            }
        }

        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IDComponente = this.ID;
            
            hdClienteID = (Hidden)X.GetCmp("hdCliID");

            this.LoadUserControl("", "");

            listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

            if (vista == "ContactoGlobal")
            {
                if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_Contactos_Lectura))
                {
                    btnAnadir.Hidden = true;
                    btnEditar.Hidden = true;
                    btnEliminar.Hidden = true;
                    btnRefrescar.Hidden = false;
                    btnDescargar.Hidden = true;
                    btnActivar.Hidden = true;
                }
                else if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_Contactos_Total))
                {
                    btnAnadir.Hidden = false;
                    btnEditar.Hidden = false;
                    btnEliminar.Hidden = false;
                    btnRefrescar.Hidden = false;
                    btnDescargar.Hidden = false;
                    btnActivar.Hidden = false;
                }

            }
            else if (vista != "ContactosEntidades")
            {
                if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_Entidades_Lectura))
                {
                    btnDescargar.Hidden = true;
                }
                else if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_Entidades_Total))
                {
                    btnDescargar.Hidden = false;
                }
            }
            else
            {
                if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_EMPLAZAMIENTOS))
                {
                    btnDescargar.Hidden = true;
                }
                else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_EMPLAZAMIENTOS))
                {
                    btnDescargar.Hidden = false;
                }
            }

            formAgregarEditarContacto.ClienteID = ClienteID;
        }

        private void Page_Init(object sender, EventArgs e)
        {
            listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            UsuariosController cUsuarios = new UsuariosController();

            if (oUsuario != null)
            {
                oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
            }

            if (vista == "ContactoGlobal")
            {
                RemoveColumEmplazamiento();
                RemoveColumEntidades();
            }
            else if (vista == "ContactosEntidades")
            {
                RemoveColumEmplazamiento();
                this.btnEditar.Hidden = true;
                this.btnEliminar.Hidden = true;
                this.btnAnadir.Hidden = true;
                this.btnActivar.Hidden = true;
            }
            else
            {
                RemoveColumEntidades();
            }

            #region FILTROS
            List<string> listaIgnore = new List<string>();

            Comun.CreateGridFilters(gridFiltersContactos, grdContactos.GetStore(), grdContactos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);

            List<string> listaIgnoreContactos = new List<string>();
            log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

            #endregion

            #region SELECCION COLUMNAS

            Comun.Seleccionable(grdContactos, grdContactos.GetStore(), grdContactos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);
            log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

            #endregion

            #region REGISTRO DE ESTADISTICAS

            EstadisticasController cEstadisticasMod = new EstadisticasController();
            cEstadisticasMod.EscribeEstadisticaAccion(oUser.UsuarioID, oUser.ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
            log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

            #endregion

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sVista = Request.QueryString["aux3"];
                        int iCount = 0;

                        if (sVista == "ContactoGlobal")
                        {
                            List<Data.Vw_ContactosGlobales> listaDatos;
                            listaDatos = ListaContactosGlobales(0, 0, sOrden, sDir, ref iCount, sFiltro);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(grdContactos.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strContactos).ToString(), Comun.DefaultLocale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(oUser.UsuarioID, oUser.ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, Comun.DefaultLocale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion
                        }
                        else if (sVista == "ContactosEntidades")
                        {
                            List<Data.Vw_ContactosGlobalesEntidades> listaEntidades;
                            listaEntidades = ListaContactosEntidades(0, 0, sOrden, sDir, ref iCount, sFiltro);
                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(grdContactos.ColumnModel, listaEntidades, Response, "", GetGlobalResource(Comun.strContactos).ToString(), Comun.DefaultLocale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(oUser.UsuarioID, oUser.ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, Comun.DefaultLocale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                        }
                        else
                        {
                            List<Data.Vw_ContactosGlobalesEmplazamientos> listaContactos;
                            listaContactos = ListaContactos(0, 0, sOrden, sDir, ref iCount, sFiltro);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(grdContactos.ColumnModel, listaContactos, Response, "", GetGlobalResource(Comun.strContactos).ToString(), Comun.DefaultLocale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(oUser.UsuarioID, oUser.ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, Comun.DefaultLocale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        Response.Write("ERROR: " + ex.Message);
                    }

                    Response.End();
                }
            }
            #endregion
        }

        #endregion

        #region LOADER CARGAR COMPONENTES

        [DirectMethod()]
        public DirectResponse LoadUserControl(string tabName, string nombre, bool update = false)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                if (update && currentUC != null)
                {
                    this.hugeCt.ContentControls.Clear();
                }

                currentUC = new Componentes.FormContactos();
                currentUC.ID = "UCAgregarEditar";
                this.hugeCt.ContentControls.Add(currentUC);

                if (update)
                {
                    CurrentControl.Text = nombre;
                    this.hugeCt.UpdateContent();
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

        #endregion

        #region STORES

        #region CONTACTOS

        protected void storeContactosGlobalesEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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

                    if (vista == "ContactoGlobal")
                    {
                        var listaContactos = ListaContactosGlobales(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                        if (listaContactos != null)
                        {
                            Store store = this.grdContactos.GetStore();
                            store.DataSource = listaContactos;
                            store.DataBind();

                            PageProxy proxy = (PageProxy)store.Proxy[0];
                            proxy.Total = listaContactos.Count;
                        }
                    }
                    else if (vista == "ContactosEntidades")
                    {
                        var listaContactos = ListaContactosEntidades(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                        if (listaContactos != null)
                        {
                            Store store = this.grdContactos.GetStore();
                            store.DataSource = listaContactos;
                            store.DataBind();

                            PageProxy proxy = (PageProxy)store.Proxy[0];
                            proxy.Total = listaContactos.Count;
                        }
                    }
                    else
                    {
                        var listaContactos = ListaContactos(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                        if (listaContactos != null)
                        {
                            Store store = this.grdContactos.GetStore();
                            store.DataSource = listaContactos;
                            store.DataBind();

                            PageProxy proxy = (PageProxy)store.Proxy[0];
                            proxy.Total = listaContactos.Count;
                        }

                        vista = "";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ContactosGlobalesEmplazamientos> ListaContactos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_ContactosGlobalesEmplazamientos> listaContactos;
            ContactosGlobalesEmplazamientosController cContactos = new ContactosGlobalesEmplazamientosController();

            try
            {
                listaContactos = cContactos.GetItemsWithExtNetFilterList<Data.Vw_ContactosGlobalesEmplazamientos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        private List<Data.Vw_ContactosGlobales> ListaContactosGlobales(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_ContactosGlobales> listaContactos;
            ContactosGlobalesController cContactos = new ContactosGlobalesController();

            try
            {
                listaContactos = cContactos.GetItemsWithExtNetFilterList<Data.Vw_ContactosGlobales>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }
        private List<Data.Vw_ContactosGlobalesEntidades> ListaContactosEntidades(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_ContactosGlobalesEntidades> listaContactos;
            ContactosGlobalesEntidadesController cContactos = new ContactosGlobalesEntidadesController();

            try
            {
                listaContactos = cContactos.GetItemsWithExtNetFilterList<Data.Vw_ContactosGlobalesEntidades>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public void SetDataSourceGridEmplazamientosContactos(List<Vw_ContactosGlobalesEmplazamientos> lista)
        {
            try
            {
                Store store = this.grdContactos.GetStore();
                if (store != null && lista != null)
                {
                    store.SetData(lista);

                    PageProxy temp = (PageProxy)store.Proxy[0];
                    temp.Total = lista.Count;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public DirectResponse MostrarEditar(long lEmplazamientoID, long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                formAgregarEditarContacto.MostrarEditar(lEmplazamientoID, lContactoGlobalID);
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
        public DirectResponse AgregarEditar()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                formAgregarEditarContacto.AgregarEditarContacto();
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
        public DirectResponse MostrarEditarContacto(long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                formAgregarEditarContacto.MostrarEditarContacto(lContactoGlobalID);
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
        public DirectResponse Activar(long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                formAgregarEditarContacto.ActivarContacto(lContactoGlobalID);
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
        public DirectResponse Eliminar(long lContactoGlobalID)
        {
            DirectResponse direct;

            try
            {
                direct = formAgregarEditarContacto.Eliminar(lContactoGlobalID);
            }
            catch (Exception ex)
            {
                direct = new DirectResponse();
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion

        #region FUNCTIONS

        private void RemoveColumEmplazamiento()
        {
            this.grdContactos.ColumnModel.Columns.Remove(colCodigo);
            this.grdContactos.ColumnModel.Columns.Remove(colNombreSitio);
            this.btnEditar.Hidden = false;
            this.btnEliminar.Hidden = false;
            this.btnAnadir.Hidden = false;
            this.btnActivar.Hidden = false;
        }
        private void RemoveColumEntidades()
        {
            this.grdContactos.ColumnModel.Columns.Remove(colCodigoEntidad);
            this.grdContactos.ColumnModel.Columns.Remove(colNombreEntidad);
            
        }

        #endregion
    }
}