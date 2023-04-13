using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.Data;
using TreeCore.Page;
using ListItemCollection = Ext.Net.ListItemCollection;

namespace TreeCore.PaginasComunes
{
    public partial class ProyectosGestion : TreeCore.Page.BasePageExtNet
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //             //#region FILTROS

                //             //List<string> listaIgnore = new List<string>()
                //             //{ };

                //             //Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                //             //log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //             //#endregion



                //             #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                //             #endregion


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
                //}

                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            List<Data.Vw_Proyectos> listaDatos;
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            long CliID = long.Parse(Request.QueryString["cliente"]);
                            bool bActivo = Request.QueryString["aux"] == "true";
                            int iCount = 0;

                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID, bActivo);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombreTemplate(grdProjects.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strProyectos).ToString(), _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
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

                    ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
                }
            }
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_ProductCatalog_Total))
                {
                    btnAnadir.Hidden = false;
                    btnEditar.Hidden = false;
                    btnEliminar.Hidden = false;
                    btnRefrescar.Hidden = false;
                    btnDescargar.Hidden = false;
                    btnDuplicar.Hidden = false;

                    btnAnadirFases.Hidden = false;
                    btnEliminarFases.Hidden = false;

                    btnAnadirEmpresaProveedora.Hidden = false;
                    btnEliminarEmpresaProveedora.Hidden = false;
                    btnActivarEmpresaProveedora.Hidden = false;

                    btnAnadirGlobalZonas.Hidden = false;
                    btnEliminarGlobalZonas.Hidden = false;
                    btnActivarGlobalZonas.Hidden = false;

                    btnAnadirProyectosTipos.Hidden = false;
                    btnEliminarProyectosTipos.Hidden = false;
                    btnActivar.Hidden = false;
                }
                else if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_ProductCatalog_Lectura))
                {
                    btnAnadir.Hidden = true;
                    btnEditar.Hidden = true;
                    btnEliminar.Hidden = true;
                    btnRefrescar.Hidden = false;
                    btnDescargar.Hidden = true;
                    btnDuplicar.Hidden = true;

                    btnAnadirFases.Hidden = true;
                    btnEliminarFases.Hidden = true;

                    btnAnadirEmpresaProveedora.Hidden = true;
                    btnEliminarEmpresaProveedora.Hidden = true;
                    btnActivarEmpresaProveedora.Hidden = true;

                    btnAnadirGlobalZonas.Hidden = true;
                    btnEliminarGlobalZonas.Hidden = true;
                    btnActivarGlobalZonas.Hidden = true;

                    btnAnadirProyectosTipos.Hidden = true;
                    btnEliminarProyectosTipos.Hidden = true;
                    btnActivar.Hidden = true;

                }
                this.storeProyectos.Reload();
            }
        }

        #region STORES

        #region PRINCIPAL

        protected void storeProyectos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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
                    List<Vw_Proyectos> lista = null;
                    if (cmpFiltro.ClienteID.ToString() != null)
                    {
                        long cliID = cmpFiltro.ClienteID;
                        hdCliID.SetValue(cmpFiltro.ClienteID);
                        lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cliID, btnActivo.Pressed);
                    }
                    

                    if (lista != null)
                    {
                        storeProyectos.DataSource = lista;

                        PageProxy temp = (PageProxy)storeProyectos.Proxy[0];
                        temp.Total = iCount;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Vw_Proyectos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID, bool activo)
        {
            List<Data.Vw_Proyectos> listaDatos;
            ProyectosController cProyectos = new ProyectosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    if (activo)
                    {
                        listaDatos = cProyectos.GetItemsWithExtNetFilterList<Data.Vw_Proyectos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true && ClienteID == " + lClienteID);
                    }
                    else
                    {
                        listaDatos = cProyectos.GetItemsWithExtNetFilterList<Data.Vw_Proyectos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                    }
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

        #region FASES
        protected void storeFases_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaFases(long.Parse(hdProyectoSeleccionado.Value.ToString()));

                    if (lista != null)
                    {
                        storeFases.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<ProyectosFases> ListaFases(long proyectoID)
        {
            List<Data.ProyectosFases> listaDatos;
            ProyectosController cProyectos = new ProyectosController();

            try
            {
                listaDatos = cProyectos.GetAllProyectosFases(proyectoID);
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region ProyectosAgrupaciones
        protected void storeProyectoAgrupacion_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaAgrupaciones(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeProyectoAgrupacion.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<ProyectosAgrupaciones> ListaAgrupaciones(long clienteID)
        {
            List<Data.ProyectosAgrupaciones> listaDatos;
            ProyectosAgrupacionesController cProyectos = new ProyectosAgrupacionesController();

            try
            {
                listaDatos = cProyectos.getClientesProyectosAgrupaciones(clienteID);
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region ProyectosTiposLibres
        protected void storeProyectosTiposLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaProyectosTiposLibres();

                    if (lista != null)
                    {
                        storeProyectosTiposLibres.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<ProyectosTipos> ListaProyectosTiposLibres()
        {
            List<Data.ProyectosTipos> listaDatos;
            ProyectosTiposController cProyectos = new ProyectosTiposController();

            try
            {
                listaDatos = cProyectos.GetAllProyectosTiposLibres(long.Parse(hdProyectoSeleccionado.Value.ToString()));
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region ProyectosProyectosTipos
        protected void storeProyectosProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaProyectosTipos(long.Parse(hdProyectoSeleccionado.Value.ToString()));

                    if (lista != null)
                    {
                        storeProyectosProyectosTipos.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Vw_ProyectosProyectosTipos> ListaProyectosTipos(long proyectoID)
        {
            List<Data.Vw_ProyectosProyectosTipos> listaDatos;
            ProyectosProyectosTiposController cProyectos = new ProyectosProyectosTiposController();

            try
            {
                listaDatos = cProyectos.getVWProyectosProyectosTipos(proyectoID);
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region ProyectosZonas
        protected void storeProyectosGlobalZona_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaProyectosZonas(long.Parse(hdProyectoSeleccionado.Value.ToString()));

                    if (lista != null)
                    {
                        storeProyectosGlobalZona.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Vw_ProyectosGlobalZonas> ListaProyectosZonas(long proyectoID)
        {
            List<Data.Vw_ProyectosGlobalZonas> listaDatos;
            ProyectosGlobalZonasController cProyectos = new ProyectosGlobalZonasController();

            try
            {
                listaDatos = cProyectos.getVWProyectosGlobalZonas(proyectoID);
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region Empresas PRoveedoras
        protected void storeProyectosEmpresaProveedora_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaProyectosEmpresaProveedoras(long.Parse(hdProyectoSeleccionado.Value.ToString()));

                    if (lista != null)
                    {
                        storeProyectosEmpresaProveedora.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Vw_ProyectosEmpresasProveedoras> ListaProyectosEmpresaProveedoras(long proyectoID)
        {
            List<Data.Vw_ProyectosEmpresasProveedoras> listaDatos;
            ProyectosEmpresasProveedorasController cProyectos = new ProyectosEmpresasProveedorasController();

            try
            {
                listaDatos = cProyectos.getVWProyectosEmpresasProveedoras(proyectoID);
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region ProyectosEmpresaProveedoraLibresLibre
        protected void storeEmpresasProveedorasLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaEmpresasProveedorasLibres();

                    if (lista != null)
                    {
                        storeEmpresasProveedorasLibres.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Entidades> ListaEmpresasProveedorasLibres()
        {
            List<Data.Entidades> listaDatos;
            EntidadesController cEntidades = new EntidadesController();

            try
            {
                listaDatos = cEntidades.GetAllEmpresasProveedorasLibres(long.Parse(hdProyectoSeleccionado.Value.ToString()));
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region ProyectosGlobalZonasLibre
        protected void storeGlobalZonasLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaProyectosGlobalZonasLibres();

                    if (lista != null)
                    {
                        storeGlobalZonasLibres.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<GlobalZonas> ListaProyectosGlobalZonasLibres()
        {
            List<Data.GlobalZonas> listaDatos;
            GlobalZonasController cZonas = new GlobalZonasController();

            try
            {
                listaDatos = cZonas.GetAllGlobalZonasLibres(long.Parse(hdProyectoSeleccionado.Value.ToString()));
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region MONEDAS
        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaMonedas();

                    if (lista != null)
                    {
                        storeMonedas.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Monedas> ListaMonedas()
        {
            List<Data.Monedas> listaDatos;
            MonedasController cMonedas = new MonedasController();
            long cliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                listaDatos = cMonedas.GetActivosCliente(cliID);
                cmbMoneda.Value = cMonedas.GetDefault(cliID).MonedaID;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        

        #region PROYECTOSESTADOS
        protected void storeProyectosEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaEstados();

                    if (lista != null)
                    {
                        storeProyectosEstados.DataSource = lista;

                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<ProyectosEstados> ListaEstados()
        {
            List<Data.ProyectosEstados> listaDatos;
            ProyectosEstadosController cProyectosEstados = new ProyectosEstadosController();
            long cliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                listaDatos = cProyectosEstados.GetActivos();
                cmbEstado.Value = cProyectosEstados.GetDefault().ProyectoEstadoID;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes = ListaClientes();

                    //if (listaClientes != null)
                    //{
                    //	storeClientes.DataSource = listaClientes;
                    //}
                    //if (ClienteID.HasValue)
                    //{
                    //	cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    //}
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECMETHOD

        #region AGREGAR PROYECTO
        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            ProyectosController cProyectos = new ProyectosController();
            long cliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                Data.Proyectos dato = new Data.Proyectos();

                if (!agregar)
                {
                    long proyectoID = long.Parse(hdProyectoSeleccionado.Value.ToString());
                    dato = cProyectos.GetItem(proyectoID);
                    if (txtNombre.Value.ToString() != dato.Proyecto || txtCodigo.Value.ToString() != dato.Referencia)
                    {
                        if (cProyectos.controlDuplicidad(txtNombre.Text, txtCodigo.Text, cliID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            return direct;
                        }
                    }
                    dato.Proyecto = txtNombre.Value.ToString();
                    dato.Referencia = txtCodigo.Value.ToString();
                    if (txtDescripcion.Value != null)
                    {
                        dato.Descripcion = txtDescripcion.Value.ToString();
                    }
                    if (cmbGrupo.Value.ToString() != "")
                    {
                        dato.ProyectoAgrupacionID = long.Parse(cmbGrupo.Value.ToString());
                    }
                    dato.Multiflujo = btnMultiproceso.Pressed;
                    dato.MonedaID = long.Parse(cmbMoneda.Value.ToString());
                    dato.ProyectoEstadoID = long.Parse(cmbEstado.Value.ToString());
                    if (!txtFechaInicio.SelectedDate.Equals(DateTime.MinValue))
                    {
                        dato.FechaInicio = txtFechaInicio.SelectedDate;
                    }

                    if (!txtFechaFin.SelectedDate.Equals(DateTime.MinValue))
                    {
                        dato.FechaFin = txtFechaFin.SelectedDate;
                    }
                    cProyectos.UpdateItem(dato);

                }
                else
                {

                    if (!cProyectos.controlDuplicidad(txtNombre.Text, txtCodigo.Text, cliID))
                    {
                        dato.Proyecto = txtNombre.Value.ToString();
                        dato.Referencia = txtCodigo.Value.ToString();
                        if (txtDescripcion.Value.ToString() != "")
                        {
                            dato.Descripcion = txtDescripcion.Value.ToString();
                        }
                        if (cmbGrupo.Value.ToString() != "")
                        {
                            dato.ProyectoAgrupacionID = long.Parse(cmbGrupo.Value.ToString());
                        }

                        dato.Multiflujo = btnMultiproceso.Pressed;
                        dato.MonedaID = long.Parse(cmbMoneda.Value.ToString());
                        dato.ProyectoEstadoID = long.Parse(cmbEstado.Value.ToString());
                        dato.Activo = true;
                        dato.ClienteID = cliID;

                        if (!txtFechaInicio.SelectedDate.Equals(DateTime.MinValue))
                        {
                            dato.FechaInicio = txtFechaInicio.SelectedDate;
                        }

                        if (!txtFechaFin.SelectedDate.Equals(DateTime.MinValue))
                        {
                            dato.FechaFin = txtFechaFin.SelectedDate;
                        }


                        dato.ProyectoEstadoID = 1;

                        dato.ProyectoComercial = false;
                        dato.ProyectoTipoID = 45;
                        dato.Cerrado = false;
                        dato.Horas = 0;
                        dato.Propio = false;
                        dato.Sharing = false;
                        dato.Torreros = false;
                        dato.TorrerosCollo = false;

                        dato = cProyectos.AddItem(dato);
                        hdProyectoSeleccionado.SetValue(dato.ProyectoID);
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);
                        return direct;
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

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosController cProyectos = new ProyectosController();

            try
            {
                Data.Proyectos dato = new Data.Proyectos();

                long proyectoID = long.Parse(hdProyectoSeleccionado.Value.ToString());
                dato = cProyectos.GetItem(proyectoID);


                txtNombre.Value = dato.Proyecto;
                txtCodigo.Value = dato.Referencia;
                if (dato.Descripcion != null)
                {
                    txtDescripcion.Value = dato.Descripcion;
                }

                if (dato.ProyectoAgrupacionID != null)
                {
                    cmbGrupo.Value = dato.ProyectoAgrupacionID;
                }

                cmbMoneda.Value = dato.MonedaID;
                cmbEstado.Value = dato.ProyectoEstadoID;
                btnMultiproceso.Pressed = dato.Multiflujo;
                txtFechaInicio.Text = dato.FechaInicio.ToString();
                txtFechaFin.Text = dato.FechaFin.ToString();


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
            ProyectosController cProyectos = new ProyectosController();
            long S = long.Parse(hdProyectoSeleccionado.Value.ToString());
            try
            {
                cProyectos.DeleteItem(S);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse Duplicar()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosController cProyectos = new ProyectosController();
            long cliID = long.Parse(hdCliID.Value.ToString());
            long proyectoIDConfiguracion = long.Parse(hdProyectoSeleccionado.Value.ToString());
            try
            {
                //AGREGAR
                Data.Proyectos dato = new Data.Proyectos();

                if (!cProyectos.controlDuplicidad(txtNombre.Text, txtCodigo.Text, cliID))
                {
                    dato.Proyecto = txtNombre.Value.ToString();
                    dato.Referencia = txtCodigo.Value.ToString();
                    if (txtDescripcion.Value.ToString() != null)
                    {
                        dato.Descripcion = txtDescripcion.Value.ToString();
                    }
                    if (cmbGrupo.Value.ToString() != null)
                    {
                        dato.ProyectoAgrupacionID = long.Parse(cmbGrupo.Value.ToString());
                    }
                    dato.Multiflujo = btnMultiproceso.Pressed;
                    dato.MonedaID = long.Parse(cmbMoneda.Value.ToString());
                    dato.ProyectoEstadoID = long.Parse(cmbEstado.Value.ToString());
                    dato.Activo = true;
                    dato.ClienteID = cliID;

                    if (!txtFechaInicio.SelectedDate.Equals(DateTime.MinValue))
                    {
                        dato.FechaInicio = txtFechaInicio.SelectedDate;
                    }

                    if (!txtFechaFin.SelectedDate.Equals(DateTime.MinValue))
                    {
                        dato.FechaFin = txtFechaFin.SelectedDate;
                    }


                    dato.ProyectoEstadoID = 1;

                    dato.ProyectoComercial = false;
                    dato.ProyectoTipoID = 45;
                    dato.Cerrado = false;
                    dato.Horas = 0;
                    dato.Propio = false;
                    dato.Sharing = false;
                    dato.Torreros = false;
                    dato.TorrerosCollo = false;

                    dato = cProyectos.AddItem(dato);
                    long proyectoID = dato.ProyectoID;
                    hdProyectoSeleccionado.SetValue(proyectoID);



                    //Configuracion
                    ProyectosFasesController cFases = new ProyectosFasesController();
                    ProyectosProyectosTiposController cProyectosTipos = new ProyectosProyectosTiposController();
                    ProyectosGlobalZonasController cZonas = new ProyectosGlobalZonasController();
                    ProyectosEmpresasProveedorasController cEmpresas = new ProyectosEmpresasProveedorasController();

                    List<ProyectosFases> listaFases = cFases.getFasesByProyectoID(proyectoIDConfiguracion);
                    List<ProyectosProyectosTipos> listaProyectosTipos = cProyectosTipos.getProyectoTipoByProyectoID(proyectoIDConfiguracion);
                    List<ProyectosGlobalZonas> listaZonas = cZonas.getGlobalZonasByProyectoID(proyectoIDConfiguracion);
                    List<ProyectosEmpresasProveedoras> listaEmpresasProveedoras = cEmpresas.getEmpresasProveedorasByProyectoID(proyectoIDConfiguracion);

                    ProyectosFases proyFases = null;
                    foreach (ProyectosFases item in listaFases)
                    {
                        proyFases = new ProyectosFases();
                        proyFases.ProyectoID = proyectoID;
                        proyFases.Fase = item.Fase;
                        cFases.AddItem(proyFases);
                    }

                    ProyectosProyectosTipos proyTipos = null;
                    foreach (ProyectosProyectosTipos item in listaProyectosTipos)
                    {
                        proyTipos = new ProyectosProyectosTipos();
                        proyTipos.ProyectoID = proyectoID;
                        proyTipos.ProyectoTipoID = item.ProyectoTipoID;
                        cProyectosTipos.AddItem(proyTipos);
                    }

                    ProyectosGlobalZonas proyZonas = null;
                    foreach (ProyectosGlobalZonas item in listaZonas)
                    {
                        proyZonas = new ProyectosGlobalZonas();
                        proyZonas.GlobalZonaID = item.GlobalZonaID;
                        proyZonas.ProyectoID = proyectoID;
                        cZonas.AddItem(proyZonas);
                    }

                    ProyectosEmpresasProveedoras proyEmpresa = null;
                    foreach (ProyectosEmpresasProveedoras item in listaEmpresasProveedoras)
                    {
                        proyEmpresa = new ProyectosEmpresasProveedoras();
                        proyEmpresa.EmpresaProveedoraID = item.EmpresaProveedoraID;
                        proyEmpresa.ProyectoID = proyectoID;
                        cEmpresas.AddItem(proyEmpresa);
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strDocTipoExistente);
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosController cController = new ProyectosController();

            try
            {
                long lID = long.Parse(hdProyectoSeleccionado.Value.ToString());

                Data.Proyectos oDato;
                oDato = cController.GetItem(lID);

                if (oDato.Activo)
                {
                    oDato.Activo = false;
                }
                else
                {
                    oDato.Activo = true;
                }

                if (cController.UpdateItem(oDato))
                {
                    direct.Success = true;
                    direct.Result = "";
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

        #region AGREGAR/EDITAR/ELIMINAR FASES
        [DirectMethod()]
        public DirectResponse AgregarEditarFases()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosFasesController cProyectosFases = new ProyectosFasesController();

            try
            {
                Data.ProyectosFases dato = new Data.ProyectosFases();
                long proyectoID = long.Parse(hdProyectoSeleccionado.Value.ToString());

                if (!cProyectosFases.controlDuplicadoFasesByProyecto(txtNombreFases.Text, proyectoID))
                {
                    dato.ProyectoID = proyectoID;
                    dato.Fase = txtNombreFases.Text;
                    cProyectosFases.AddItem(dato);

                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsYaExiste);
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

        [DirectMethod()]
        public DirectResponse EliminarFases()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosFasesController cProyectosFases = new ProyectosFasesController();
            long S = long.Parse(hdProyectoFaseID.Value.ToString());
            try
            {
                cProyectosFases.DeleteItem(S);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }
            return direct;
        }


        #endregion

        #region AGREGAR/ELIMINAR PROYECTOSTIPOS

        [DirectMethod]
        public DirectResponse AgregarProyectosTipos()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosProyectosTiposController cProyectosProyectosTipos = new ProyectosProyectosTiposController();

            direct.Success = true;
            direct.Result = "";
            try
            {
                long proyectoID = 0;

                if (hdProyectoSeleccionado.Value.ToString() != "")
                {
                    proyectoID = Convert.ToInt32(hdProyectoSeleccionado.Value.ToString());
                }
                else
                {
                    proyectoID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                }




                ListItemCollection listaSeleccionada = cmbTipoProyecto.SelectedItems;
                Data.ProyectosProyectosTipos dato = new Data.ProyectosProyectosTipos();
                foreach (var item in listaSeleccionada)
                {
                    dato = new Data.ProyectosProyectosTipos();
                    dato.ProyectoID = proyectoID;
                    dato.ProyectoTipoID = long.Parse(item.Value);
                    dato.Activo = true;
                    cProyectosProyectosTipos.AddItem(dato);
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
        public DirectResponse EliminarProyectoTipo()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosProyectosTiposController cProyectosTipos = new ProyectosProyectosTiposController();
            long S = long.Parse(hdProyectoProyectoTipoID.Value.ToString());
            try
            {
                cProyectosTipos.DeleteItem(S);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }
            return direct;
        }


        [DirectMethod()]
        public DirectResponse ActivarProyectoTipo()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosProyectosTiposController cController = new ProyectosProyectosTiposController();

            try
            {
                long lID = long.Parse(hdProyectoProyectoTipoID.Value.ToString());

                Data.ProyectosProyectosTipos oDato;
                oDato = cController.GetItem(lID);

                if (oDato.Activo)
                {
                    oDato.Activo = false;
                }
                else
                {
                    oDato.Activo = true;
                }

                if (cController.UpdateItem(oDato))
                {
                    direct.Success = true;
                    direct.Result = "";
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

        #region AGREGAR/ELIMINAR GLOBALZONAS

        [DirectMethod]
        public DirectResponse AgregarGlobalZonas()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosGlobalZonasController cProyectosGlobalZonas = new ProyectosGlobalZonasController();

            direct.Success = true;
            direct.Result = "";
            try
            {
                long proyectoID = 0;

                if (hdProyectoSeleccionado.Value.ToString() != "")
                {
                    proyectoID = Convert.ToInt32(hdProyectoSeleccionado.Value.ToString());
                }
                else
                {
                    proyectoID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                }
                ListItemCollection listaSeleccionada = cmbGlobalZonasLibres.SelectedItems;
                Data.ProyectosGlobalZonas dato = new Data.ProyectosGlobalZonas();
                foreach (var item in listaSeleccionada)
                {
                    dato = new Data.ProyectosGlobalZonas();
                    dato.ProyectoID = proyectoID;
                    dato.GlobalZonaID = long.Parse(item.Value.ToString());
                    dato.Activo = true;
                    cProyectosGlobalZonas.AddItem(dato);
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
        public DirectResponse EliminarGlobalZonas()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosGlobalZonasController cProyectosGlobalZonas = new ProyectosGlobalZonasController();
            long S = long.Parse(hdProyectoGlobalZonasID.Value.ToString());
            try
            {
                cProyectosGlobalZonas.DeleteItem(S);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }
            return direct;
        }


        [DirectMethod()]
        public DirectResponse ActivarGlobalZonas()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosGlobalZonasController cController = new ProyectosGlobalZonasController();

            try
            {
                long lID = long.Parse(hdProyectoGlobalZonasID.Value.ToString());

                Data.ProyectosGlobalZonas oDato;
                oDato = cController.GetItem(lID);

                if (oDato.Activo)
                {
                    oDato.Activo = false;
                }
                else
                {
                    oDato.Activo = true;
                }

                if (cController.UpdateItem(oDato))
                {
                    direct.Success = true;
                    direct.Result = "";
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

        #region AGREGAR/ELIMINAR EMPRESAPROVEEDORAS

        [DirectMethod]
        public DirectResponse AgregarEmpresasProveedoras()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosEmpresasProveedorasController cProyectosEmpresaProveedora = new ProyectosEmpresasProveedorasController();

            direct.Success = true;
            direct.Result = "";
            try
            {
                long proyectoID = 0;

                if (hdProyectoSeleccionado.Value.ToString() != "")
                {
                    proyectoID = Convert.ToInt32(hdProyectoSeleccionado.Value.ToString());
                }
                else
                {
                    proyectoID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                }

                ListItemCollection listaSeleccionada = cmbEmpresasProveedorasLibres.SelectedItems;
                Data.ProyectosEmpresasProveedoras dato = new Data.ProyectosEmpresasProveedoras();
                foreach (var item in listaSeleccionada)
                {
                    dato = new Data.ProyectosEmpresasProveedoras();
                    dato.ProyectoID = proyectoID;
                    dato.EmpresaProveedoraID = long.Parse(item.Value);
                    dato.Activo = true;
                    cProyectosEmpresaProveedora.AddItem(dato);
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
        public DirectResponse EliminarEmpresaProveedora()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosEmpresasProveedorasController cProyectosEmpresaProveedora = new ProyectosEmpresasProveedorasController();
            long S = long.Parse(hdProyectoEmpresasProveedorasID.Value.ToString());
            try
            {
                cProyectosEmpresaProveedora.DeleteItem(S);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }
            return direct;
        }


        [DirectMethod()]
        public DirectResponse ActivarEmpresaProveedora()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosEmpresasProveedorasController cController = new ProyectosEmpresasProveedorasController();

            try
            {
                long lID = long.Parse(hdProyectoEmpresasProveedorasID.Value.ToString());

                Data.ProyectosEmpresasProveedoras oDato;
                oDato = cController.GetItem(lID);

                if (oDato.Activo)
                {
                    oDato.Activo = false;
                }
                else
                {
                    oDato.Activo = true;
                }

                if (cController.UpdateItem(oDato))
                {
                    direct.Success = true;
                    direct.Result = "";
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


        

        #endregion

    }


}