using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;
using TreeCore.Data;
using TreeCore.Page;

namespace TreeCore.Componentes
{
    public partial class GridEmplazamientos : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        private int defaultItems = 20;
        BaseUserControl currentUC;
        private string _vista = "";
        Data.Usuarios oUser;

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
            set
            {
                this._vista = value;
                hdNombre.SetValue(value);
            }
        }

        public GridPanel ComponetGrid
        {
            get { return gridEmplazamientos; }

        }

        #region Gestión Página (Init/Load)

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IDComponente = this.ID;
            //this.LoadUserControl("", "");
            
        }

        private void Page_Init(object sender, EventArgs e)
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            UsuariosController cUsuarios = new UsuariosController();

            if (oUsuario != null)
            {
                oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
            }

            listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
            
            if (!(listaFuncionalidades.Contains(500100) || listaFuncionalidades.Contains(500100)))
            {
                ShowHistorial.Hidden = true;
            }

            if ((listaFuncionalidades.Contains((long)Comun.ModFun.GLO_MapaEmplazamientosCercanos_Lectura)) || listaFuncionalidades.Contains((long)Comun.ModFun.GLO_MapaEmplazamientosCercanos_Total))
            {
                ShowMap.Hidden = false;
            }
            else
            {
                ShowMap.Hidden = true;
            }

            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_EMPLAZAMIENTOS))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnDescargar.Hidden = true;
                btnContactos.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_EMPLAZAMIENTOS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnDescargar.Hidden = false;
                btnContactos.Hidden = false;
            }

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
                        string sFiltro2 = Request.QueryString["aux3"];
                        string sTextoBuscado = Request.QueryString["aux4"];
                        string sIdBuscado = Request.QueryString["aux5"];
                        int iCount = 0;
                        Hidden CliID = (Hidden)X.GetCmp("hdCliID");

                        string textoBuscado = (!string.IsNullOrEmpty(sTextoBuscado))? sTextoBuscado : "";
                        long? IdBuscado = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                        List<Vw_CoreEmplazamientos> listaDatos = ModGlobal.pages.Emplazamientos.GetDatosEmplazamientos(sFiltro2, Request.QueryString["cliente"], true, textoBuscado, IdBuscado);
                        listaDatos = Clases.LinqEngine.PagingItemsListWithExtNetFilter(listaDatos, sFiltro, "", sOrden, sDir, 0, 0, ref iCount);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(gridEmplazamientos.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strEmplazamientos).ToString(), Comun.DefaultLocale);
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

        #region LOADER PARA CARGAR COMPONENTES

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

                currentUC = new Componentes.FormEmplazamientos();
                currentUC.ID = "UCAgragarEditar";
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

        [DirectMethod()]
        public DirectResponse CargarGrid()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                #region FILTROS
                List<string> listaIgnore = new List<string>()
            { "Proyectos", "Contratos" };
                //{};

                Comun.CreateGridFilters(gridFilters, gridEmplazamientos.GetStore(), gridEmplazamientos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridEmplazamientos, gridEmplazamientos.GetStore(), gridEmplazamientos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion
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

        #region Store Emplazamientos

        protected void storeEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int pageSize = Convert.ToInt32(cmbNumRegistros.Value);
                    int curPage = e.Page - 1;
                    int total;

                    string s = e.Parameters["filter"];

                    List<JsonObject> lista;
                    lista = ((ModGlobal.pages.Emplazamientos)Parent.Page).AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), pageSize, curPage, out total, e.Sort, s);

                    if (storeEmplazamientos != null && lista != null)
                    {
                        e.Total = total;
                        hdTotalCountGrid.SetValue(total);
                        nfPaginNumber.SetValue(e.Page);
                        storeEmplazamientos.DataSource = lista;
                        storeEmplazamientos.DataBind();

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public void SetDataSourceGridEmplazamientos(List<JsonObject> lista, int count)
        {
            try
            {

                if (storeEmplazamientos != null && lista != null)
                {

                    storeEmplazamientos.SetData(lista);

                    PageProxy temp = (PageProxy)storeEmplazamientos.Proxy[0];
                    temp.Total = count;
                    hdTotalCountGrid.SetValue(count);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private List<Data.Vw_CoreEmplazamientos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_CoreEmplazamientos> listaDatos;
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cEmplazamientos.GetItemsWithExtNetFilterList<Data.Vw_CoreEmplazamientos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }


        #endregion

        #region CONTACTOS GLOBALES EMPLAZAMIENTOS VINCULADOS

        protected void storeContactosGlobalesEmplazamientosVinculados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_ContactosGlobalesEmplazamientosVinculados> listaContactos = ListaContactos();

                    if (listaContactos != null)
                    {
                        storeContactosGlobalesEmplazamientosVinculados.DataSource = listaContactos;
                        storeContactosGlobalesEmplazamientosVinculados.DataBind();

                        PageProxy proxy = (PageProxy)storeContactosGlobalesEmplazamientosVinculados.Proxy[0];
                        proxy.Total = listaContactos.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ContactosGlobalesEmplazamientosVinculados> ListaContactos()
        {
            List<Data.Vw_ContactosGlobalesEmplazamientosVinculados> listaEmplazamientos = null;
            ContactosGlobalesEmplazamientosVinculadosController cEmplazamientos = new ContactosGlobalesEmplazamientosVinculadosController();

            try
            {
                if (txtBuscarEmail.Text != "")
                {
                    listaEmplazamientos = cEmplazamientos.getContactosNoAsignadosByEmail(txtBuscarEmail.Text, long.Parse(hdEmplazamientoSeleccionado.Value.ToString()));
                }
                else if (searchTel.Text != "")
                {
                    listaEmplazamientos = cEmplazamientos.getContactosNoAsignadosByTelefono(searchTel.Text, long.Parse(hdEmplazamientoSeleccionado.Value.ToString()));
                }
                else if (hdEmplazamientoSeleccionado.Value != null && hdEmplazamientoSeleccionado.Value.ToString() != "")
                {
                    listaEmplazamientos = cEmplazamientos.GetListaContactosByEmplazamientoID(long.Parse(hdEmplazamientoSeleccionado.Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEmplazamientos = null;
            }

            return listaEmplazamientos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public DirectResponse MostrarEditar(long IDEmplazamiento)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                direct.Success = true;
                direct.Result = "";

                direct.Result = formAgregarEditar.MostrarEditar(IDEmplazamiento).Result;
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
        public DirectResponse MostrarEditarContacto(long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                formAgregarEditarContactoEmplazamiento.MostrarEditarContacto(lContactoGlobalID);
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
        public DirectResponse AsignarEmplazamiento(long lContactoGlobalID, long lEmplazamientoIDAsignado)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesEmplazamientosController cEmplazamientos = new ContactosGlobalesEmplazamientosController();

            try
            {
                long lEmplazamientoID = long.Parse(hdEmplazamientoSeleccionado.Value.ToString());

                if (lEmplazamientoIDAsignado != 0 && lEmplazamientoID == lEmplazamientoIDAsignado)
                {
                    ContactosGlobalesEmplazamientos oDato = cEmplazamientos.GetContactosByID(lEmplazamientoID, lContactoGlobalID);

                    if (cEmplazamientos.DeleteItem(oDato.ContactoGlobalEmplazamientoID))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }
                }
                else
                {

                    Data.ContactosGlobalesEmplazamientos oDato = new Data.ContactosGlobalesEmplazamientos();
                    oDato.ContactoGlobalID = lContactoGlobalID;
                    oDato.EmplazamientoID = lEmplazamientoID;

                    cEmplazamientos.AddItem(oDato);
                    log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
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
        public DirectResponse MostrarProyectosAsignados(long lEmplazamientoID)
        {
            DirectResponse direct = new DirectResponse();
            long localProyectoTipoID = 0;
            string localProyectoTipo = null;
            try
            {
                Store store = this.GridProjectsD.GetStore();
                if (store != null)
                {
                    EmplazamientosProyectosController cEmplazamientosProyectosController = new EmplazamientosProyectosController();
                    List<Vw_EmplazamientosProyectosFiltrados> lista = cEmplazamientosProyectosController.GetViewByEmplazamientoID(lEmplazamientoID);

                    if (lista != null)
                    {
                        store.DataSource = lista;
                        store.DataBind();
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod]
        public DirectResponse MostrarContratosAsignados(long lEmplazamientoID)
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod]
        public DirectResponse GetDatosBuscador()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                int total;
                List<JsonObject> lista;
                lista = ((ModGlobal.pages.Emplazamientos)Parent.Page).AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), -1, -1, out total, null, null);

                direct.Result = lista;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;

            return direct;
        }

        #endregion

    }
}