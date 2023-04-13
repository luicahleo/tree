using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using log4net;
using System.Data.Linq.Mapping;
using TreeCore.Clases;
using System.Reflection;

namespace TreeCore.ModGlobal
{
    public partial class Columnas : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> ListaFuncionalidades = new List<long>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                ListaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                #region FILTROS

                List<string> ListaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);

                List<string> ListaIgnoreDetalle = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridMaestro, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);
                Comun.Seleccionable(GridDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion
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
                        string sModuloID = Request.QueryString["aux"].ToString();
                        //long? cliID = null;
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.CategoriasTablas> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, ClienteID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                        }
                        #endregion

                        #region DETALLE
                        else
                        {
                            List<Data.Vw_GlobalColumnasTablas> ListaDatosDetalle = null;

                            ListaDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaDatosDetalle, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                LogController lController = new LogController();
                                lController.EscribeLog(Ip, Usuario.UsuarioID, ex.Message);
                                log.Error(ex.Message);
                            }
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
                ResourceManagerTreeCore.RegisterIcon(Icon.ChartCurve);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_COLUMNAS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;

                btnAnadirDetalle.Hidden = false;
                btnEditarDetalle.Hidden = false;
                btnEliminarDetalle.Hidden = false;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = false;
            }
        }

        #endregion

        #region STORES

        #region MAESTRO

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    long? lCliID = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                     

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro,  long.Parse(hdCliID.Value.ToString()));

                    if (vLista != null)
                    {
                        storePrincipal.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storePrincipal.Proxy[0];
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

        private List<Data.CategoriasTablas> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.CategoriasTablas> ListaDatos = new List<Data.CategoriasTablas>();
            CategoriasTablasController CCategoriasTablas = new CategoriasTablasController();

            try
            {
                
                    ListaDatos = CCategoriasTablas.GetItemsWithExtNetFilterList<Data.CategoriasTablas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount); 
                
            }

            catch (Exception ex)
            {
                ListaDatos = null;
                log.Error(ex.Message);
            }

            return ListaDatos;
        }


        #endregion

        #region DETALLE

        protected void storeDetalle_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersDetalle"];

                    if (!ModuloID.Value.Equals(""))
                    {
                        
                        lMaestroID = Convert.ToInt64(ModuloID.Value);
                        

                    }

                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID);
                    if (vLista != null)
                    {
                        storeDetalle.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storeDetalle.Proxy[0];
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

        private List<Data.Vw_GlobalColumnasTablas> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_GlobalColumnasTablas> ListaDatos = new List<Data.Vw_GlobalColumnasTablas>();

            try
            {
                GlobalColumnasTablasController CGlobalColumnasTablas = new GlobalColumnasTablasController();

                ListaDatos = CGlobalColumnasTablas.GetItemsWithExtNetFilterList<Data.Vw_GlobalColumnasTablas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "CategoriaID == " + lMaestroID);

                CGlobalColumnasTablas = null;
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return ListaDatos;
        }


        #endregion

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    List<Data.Clientes> listaClientes = new List<Data.Clientes>();

                    listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
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

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos = new List<Data.Clientes>();
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

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            
            CategoriasTablasController cCategoriasTablas = new CategoriasTablasController();
            long cliID = 0;
            try
            
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.CategoriasTablas dato = new Data.CategoriasTablas();
                    Data.CategoriasTablas datoAux = new Data.CategoriasTablas();
                    dato = cCategoriasTablas.GetItem(S);                   
                    
                    if (dato.CategoriaNombre == txtCategoria.Text)
                    {
                        dato.CategoriaNombre = txtCategoria.Text;
                        dato.EsComercial = chkEsComercial.Checked;
                        
                    }
                    else
                    {
                        
                        if (cCategoriasTablas.RegistroDuplicado(txtCategoria.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            dato.CategoriaNombre = txtCategoria.Text;
                            dato.EsComercial = chkEsComercial.Checked;
                            
                        }
                    }

                    if (cCategoriasTablas.UpdateItem(dato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {
                    if (cCategoriasTablas.RegistroDuplicado(txtCategoria.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {

                        Data.CategoriasTablas dato = new Data.CategoriasTablas();
                        dato.CategoriaNombre = txtCategoria.Text;
                        dato.Activo = true;
                        if (chkEsComercial.Checked)
                        {
                            dato.EsComercial = true;
                        }
                        else
                        {
                            dato.EsComercial = false;
                        }
                        if (cCategoriasTablas.AddItem(dato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
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
            

            CategoriasTablasController cCategoriasTablas = new CategoriasTablasController();


            try
            
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CategoriasTablas dato = new TreeCore.Data.CategoriasTablas();
                dato = cCategoriasTablas.GetItem(S);
                txtCategoria.Text = dato.CategoriaNombre;
                chkEsComercial.Checked = dato.EsComercial;

                winGestion.Show();

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                direct.Result = "";
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
            CategoriasTablasController CCategoriasTablas = new CategoriasTablasController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CCategoriasTablas.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                CCategoriasTablas = null;
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

        #region GESTION DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle(long moduloID)
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;

            try
            {
                storeDetalle.DataBind();
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
        public DirectResponse AgregarEditarDetalle(bool agregar)
        {
            DirectResponse ajax = new DirectResponse();
            
            GlobalColumnasTablasController cElemento = new GlobalColumnasTablasController();

            try
            
            {
                if (!agregar)
                {
                    long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.ColumnasTablas dato = null;
                    dato = cElemento.GetItem(ID);
                    dato.ComponenteID = Convert.ToInt32(cmbComponentes.SelectedItem.Value);
                    dato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                    dato.Literal = txtLiteral.Text;


                    if (cElemento.UpdateItem(dato))
                    {
                        storeDetalle.DataBind();

                    }



                    cElemento = null;
                }
                else
                {
                    long catID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                    Data.ColumnasTablas dato = new Data.ColumnasTablas();
                    dato.CategoriaID = catID;
                    dato.ComponenteID = Convert.ToInt32(cmbComponentes.SelectedItem.Value);
                    dato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                    dato.ColumnaNombre = cmbColumnas.SelectedItem.Text;
                    dato.NombreTable = cmbTablas.SelectedItem.Text;
                    dato.Valores = null;
                    dato.Visible = true;
                    dato.Literal = txtLiteral.Text;


                    // Adds the following information
                    if (cElemento.GetColumnasByNombre(dato.ColumnaNombre) == null)
                    {

                        if (cElemento.AddItem(dato) != null)
                        {
                            storeDetalle.DataBind();
                        }

                    }
                    else
                    {
                        MensajeBox(GetLocalResourceObject("jsAtencion").ToString(), GetLocalResourceObject("jsColumnaRepetida").ToString(), MessageBox.Icon.INFO, null);
                    }

                    //storePrincipal.DataBind();
                    cElemento = null;

                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetLocalResourceObject("strMensanjeGenerico").ToString();
                log.Error(ex.Message);
                return ajax;
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();
            


            try
            
            {
                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.ColumnasTablas dato = new Data.ColumnasTablas();
                GlobalColumnasTablasController fController = new GlobalColumnasTablasController();
                dato = fController.GetItem(ID);
                cmbTiposDatos.SetValue(dato.TipoDatoID);
                storeTablas.Reload();
                cmbTablas.SetValue(dato.NombreTable);
                storeColumnas.Reload();
                cmbColumnas.SetValue(dato.ColumnaNombre);
                cmbComponentes.SetValue(dato.ComponenteID);
                
                txtLiteral.Text = dato.Literal;
                winGestionDetalle.Show();
                fController = null;
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetLocalResourceObject("strMensanjeGenerico").ToString();
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalColumnasTablasController CGlobalColumnasTablas = new GlobalColumnasTablasController();

            direct.Result = "";
            direct.Success = true;

    
                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

                try
                {
                    if (CGlobalColumnasTablas.DeleteItem(lID))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }

                    CGlobalColumnasTablas = null;
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();

            CategoriasTablasController cCategoriasTablas = new CategoriasTablasController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CategoriasTablas dato = new Data.CategoriasTablas();
                dato = cCategoriasTablas.GetItem(S);
                dato.Activo = !dato.Activo;
                if (cCategoriasTablas.UpdateItem(dato))
                {
                    storePrincipal.DataBind();
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            cCategoriasTablas = null;

            direct.Success = true;
            direct.Result = "";
            return direct;
        }



        [DirectMethod()]
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalColumnasTablasController CGlobalColumnasTablas = new GlobalColumnasTablasController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.ColumnasTablas oDato = new Data.ColumnasTablas();
                oDato = CGlobalColumnasTablas.GetItem(lID);
                

                if (CGlobalColumnasTablas.UpdateItem(oDato))
                {
                    storeDetalle.DataBind();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
            }

            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            CGlobalColumnasTablas = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        #region TABLAS

        protected void storeTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Object> lista = new List<object>();
                Object objeto = null;
                int i = 1;
                try
                {
                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));
                    foreach (var mt in model.GetTables())
                    {
                        objeto = new { TablaID = i.ToString(), TablaNombre = mt.TableName.ToString() };
                        lista.Add(objeto);
                        i = i + 1;
                    }
                    if (lista != null)
                        storeTablas.DataSource = lista;


                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                }
            }
        }

        #endregion

        #region COLUMNAS

        protected void storeColumnas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Object> lista = new List<object>();
                Object objeto = null;
                string sNombreTabla = null;
                int i = 1;

                try
                {
                    sNombreTabla = cmbTablas.SelectedItem.Text;
                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));
                    foreach (var mt in model.GetTables())
                    {
                        if (sNombreTabla.Equals(mt.TableName))
                        {
                            foreach (var dm in mt.RowType.DataMembers)
                            {

                                objeto = new { ColumnaTablaID = i.ToString(), ColumnaNombre = dm.MappedName.ToString() };
                                lista.Add(objeto);
                                i = i + 1;
                            }

                        }
                    }

                    if (lista != null)
                        storeColumnas.DataSource = lista;


                }
                catch (Exception ex)
                {
                    log.Error(ex);                    
                }
            }
        }


        #endregion
        #region COMPONENTES

        protected void storeComponentes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ComponentesController cComponentes = new ComponentesController();
                try
                {

                    List<Data.Componentes> lista = new List<Data.Componentes>();
                    lista = cComponentes.GetItemList();

                    if (lista != null)
                        storeComponentes.DataSource = lista;
                    cComponentes = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                }
            }
        }

        #endregion



        #region TIPOS DATOS

        protected void storeTiposDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                TiposDatosController cTiposDatos = new TiposDatosController();
                try
                {

                    List<Data.TiposDatos> lista = new List<Data.TiposDatos>();
                    lista = cTiposDatos.GetItemList();

                    if (lista != null)
                        storeTiposDatos.DataSource = lista;
                    cTiposDatos = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                }
            }
        }

        #endregion

        [DirectMethod()]
        public DirectResponse VisibleChange()
        {
            DirectResponse direct = new DirectResponse();

            GlobalColumnasTablasController cColumnas = new GlobalColumnasTablasController();

            try
            {
                long S = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.ColumnasTablas dato = new Data.ColumnasTablas();
                dato = cColumnas.GetItem(S);
                dato.Visible = !dato.Visible;
                if (cColumnas.UpdateItem(dato))
                {
                    storeDetalle.DataBind();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MensajeBox(GetLocalResourceObject("jsAtencion").ToString(), GetLocalResourceObject("jsColumnaErrorVisible").ToString(), MessageBox.Icon.ERROR, null);
            }
            cColumnas = null;

            direct.Success = true;
            direct.Result = "";
            return direct;
        }



        #region STORE VALORES


        protected void storeValores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    //Recupera los parámetros para obtener los datos de la grilla               
                    sort = e.Sort.ToString();
                    dir = e.Sort.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters"];

                    //Recupera los datos y los establece
                    var ls = ListaValores(e.Start, e.Limit, sort, dir, ref count, filtro);
                    if (ls != null)
                    {
                        storeValores.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storeValores.Proxy[0];
                        temp.Total = count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                }
            }
        }



        /// <summary>
        /// Método que se usa para cargar los datos de la grilla
        /// </summary>
        /// <param name="start">Índice del primer elemento de la grilla</param>
        /// <param name="limit">Número de elementos a recuperar</param>
        /// <param name="sort">Columna por la que se desean ordenar los datos</param>
        /// <param name="dir">Dirección de la ordenación de los datos</param>
        /// <param name="count">Número de datos que se recuperan</param>
        /// <param name="filtro">Filtrado para los datos</param>
        /// <returns>Lista con los datos a mostrar en la grilla</returns>
        private List<Valores> ListaValores(int start, int limit, string sort, string dir, ref int count, string filtro)
        {
            List<Valores> datos = new List<Valores>();
            string sValor = null;
            Valores localValor = null;
            try
            {
                if (GridRowSelectDetalle.SelectedRecordID != null && !GridRowSelect.SelectedRecordID.Equals(""))
                {
                    GlobalColumnasTablasController mControl = new GlobalColumnasTablasController();
                    Data.ColumnasTablas campo = new Data.ColumnasTablas();
                    campo = mControl.GetItem("ColumnaTablaID ==" + GridRowSelectDetalle.SelectedRecordID);
                    sValor = campo.Valores;
                    if (sValor != null && sValor != "")
                    {
                        string[] localArray = sValor.Split(',');
                        for (int i = 0; i < localArray.Length; i = i + 1)
                        {
                            localValor = new Valores();
                            localValor.ValorID = i + 1;
                            localValor.Valor = localArray[i];
                            datos.Add(localValor);
                        }

                    }
                    mControl = null;
                }


            }
            catch (Exception ex)
            {
                log.Error(ex);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

            }
            return datos;
        }

        private void RefreshStoreValores()
        {
            storeValores.DataBind();
        }

        [DirectMethod]
        public DirectResponse AgregarValores()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;
            GlobalColumnasTablasController cCamposController = new GlobalColumnasTablasController();
            Data.ColumnasTablas campos = null;
            string sValores = null;

            try
            {

                long detalleID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                campos = cCamposController.GetItem("ColumnaTablaID == " + detalleID);
                if (campos != null)
                {
                    sValores = campos.Valores;
                    if (sValores != null && sValores != "")
                    {
                        sValores = sValores + "," + txtValor.Text;
                    }
                    else
                    {
                        sValores = txtValor.Text;
                    }
                    campos.Valores = sValores;
                    if (cCamposController.UpdateItem(campos))
                    {
                        storeValores.DataBind();
                    }
                }


                cCamposController = null;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

            }
            return ajax;
        }


        [DirectMethod]
        public DirectResponse EliminarValores()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;
            GlobalColumnasTablasController cCamposController = new GlobalColumnasTablasController();
            Data.ColumnasTablas campos = null;
            string sValores = null;
            string sNuevoValores = "";

            try
            {

                long detalleID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                campos = cCamposController.GetItem("ColumnaTablaID == " + detalleID);
                if (campos != null)
                {
                    sValores = campos.Valores;
                    string[] localArray = sValores.Split(',');
                    for (int i = 0; i < localArray.Length; i = i + 1)
                    {
                        if ((i + 1) != Int64.Parse(GridRowSelectValores.SelectedRecordID))
                        {
                            sNuevoValores = sNuevoValores + localArray[i] + ",";
                        }
                    }
                    if (sNuevoValores != null && sNuevoValores != "")
                    {
                        sNuevoValores = sNuevoValores.Substring(0, sNuevoValores.Length - 1);
                    }

                    campos.Valores = sNuevoValores;
                    if (cCamposController.UpdateItem(campos))
                    {
                        storeValores.RemoveAll();
                        storeValores.DataBind();
                    }
                }


                cCamposController = null;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

            }
            return ajax;
        }

        #endregion



    }
}