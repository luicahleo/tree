using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using TreeCore.Data;
using System.Reflection;
using System.Data;
using Tree.Linq.GenericExtensions;
using System.Linq;
using Tree.Linq;



namespace TreeCore.ModGlobal
{
    public partial class MigradorExpImp : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>() { };
                List<string> listaIgnoreTiposContratos = new List<string>() { };
                List<string> listaIgnoreTiposEstructurasLibres = new List<string>();

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersTiposContratos, storeTiposContratos, gridTiposContratos.ColumnModel, listaIgnoreTiposContratos, _Locale);
                Comun.CreateGridFilters(gridFiltersTiposEstructurasLibres, storeTiposContratosLibres, gridTiposEstructurasLibres.ColumnModel, listaIgnoreTiposEstructurasLibres, _Locale);

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
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.AlquileresTiposContrataciones> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

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

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
        };
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.AlquileresTiposContrataciones> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.AlquileresTiposContrataciones> listaDatos;
            AlquileresTiposContratacionesController CAlquileresTiposContrataciones = new AlquileresTiposContratacionesController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CAlquileresTiposContrataciones.GetItemsWithExtNetFilterList<Data.AlquileresTiposContrataciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes;

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

        #region TIPOS CONTRATOS
        /// <summary>
        /// Cargar el Store de los contratos de la tipo de contratacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void storeTiposContratos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                AlquileresTiposContratacionesTiposContratosController cContratacionesContratos = new AlquileresTiposContratacionesTiposContratosController();
                try
                {
                    List<Data.Vw_AlquileresTiposContratacionesTiposContratos> lista;
                    lista = cContratacionesContratos.tiposContratosAsignados(Int64.Parse(GridRowSelect.SelectedRecordID));

                    if (lista != null)
                    {
                        storeTiposContratos.DataSource = lista;
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

        public List<Object> ListaCampos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Object> lista = null;
            int i = 1;
            Object objeto = null;

            // Takes the information from the object
            try
            {
                long lgrupoID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                AlquileresTiposContratacionesController cGrupo = new AlquileresTiposContratacionesController();
                var grupo = cGrupo.GetItem(lgrupoID);
                lista = new List<object>();
                if (grupo.TipoContratacion != null && !grupo.TipoContratacion.Equals(""))
                {

                    string[] sCampo = grupo.TipoContratacion.Split(';');
                    foreach (string visor in sCampo)
                    {
                        objeto = new { AlquilerTipoContratacionID = i.ToString(), Campo = visor };
                        lista.Add(objeto);
                        i++;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            // Returns the result

            return lista;
        }


        /// <summary>
        /// Cargar los contratos Sin asignar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void storeTiposContratosLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                AlquileresTiposContratacionesTiposContratosController cContratacionesContratos = new AlquileresTiposContratacionesTiposContratosController();
                try
                {
                    List<Data.AlquileresTiposContratos> lista;
                    lista = cContratacionesContratos.tiposContratosNoAsignado(Int64.Parse(GridRowSelect.SelectedRecordID));
                    if (lista != null)
                    {
                        storeTiposContratosLibres.DataSource = lista;
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
        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();

            AlquileresTiposContratacionesController cAlquileresTiposContrataciones = new AlquileresTiposContratacionesController();
            long cliID = 0;

            try

            {
                if (!agregar)
                {
                    long lIdSelecionado = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.AlquileresTiposContrataciones oDato;
                    oDato = cAlquileresTiposContrataciones.GetItem(lIdSelecionado);

                    if (oDato.TipoContratacion == txtTipoContratacion.Text)
                    {
                        oDato.TipoContratacion = txtTipoContratacion.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cAlquileresTiposContrataciones.RegistroDuplicado(txtTipoContratacion.Text, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.TipoContratacion = txtTipoContratacion.Text;
                        }
                    }


                    if (cAlquileresTiposContrataciones.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cAlquileresTiposContrataciones.RegistroDuplicado(txtTipoContratacion.Text, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.AlquileresTiposContrataciones oDato = new Data.AlquileresTiposContrataciones
                        {
                            TipoContratacion = txtTipoContratacion.Text,
                            Defecto = false,
                            Activo = true,
                            ClienteID = Convert.ToInt32(cmbClientes.SelectedItem.Value)
                        };
                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = cliID;
                        }

                        if (cAlquileresTiposContrataciones.AddItem(oDato) != null)
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

            AlquileresTiposContratacionesController cAlquileresTiposContrataciones = new AlquileresTiposContratacionesController();

            try
            {
                long lIdSelecionado = long.Parse(GridRowSelect.SelectedRecordID);

                Data.AlquileresTiposContrataciones oDato;
                oDato = cAlquileresTiposContrataciones.GetItem(lIdSelecionado);
                txtTipoContratacion.Text = oDato.TipoContratacion;

                winGestion.Show();
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            AlquileresTiposContratacionesController cAlquileresTiposContrataciones = new AlquileresTiposContratacionesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.AlquileresTiposContrataciones oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cAlquileresTiposContrataciones.GetDefault(Convert.ToInt32(ClienteID));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.AlquilerTipoContratacionID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cAlquileresTiposContrataciones.UpdateItem(oDato);
                        }

                        oDato = cAlquileresTiposContrataciones.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cAlquileresTiposContrataciones.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cAlquileresTiposContrataciones.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cAlquileresTiposContrataciones.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
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
            AlquileresTiposContratacionesController cAlquileresTiposContrataciones = new AlquileresTiposContratacionesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cAlquileresTiposContrataciones.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cAlquileresTiposContrataciones.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }
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
            AlquileresTiposContratacionesController cController = new AlquileresTiposContratacionesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.AlquileresTiposContrataciones oDato;
                oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cController.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #region ASIGNAR TIPOS CONTRATOS
        /// <summary>
        /// Agregar tipo Contratos al tipo de contratacion
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse AgregarTiposContratos()
        {
            DirectResponse direct = new DirectResponse();
            AlquileresTiposContratacionesTiposContratosController cContratacionesContratos = new AlquileresTiposContratacionesTiposContratosController();

            try
            {
                foreach (SelectedRow selec in GridRowSelectTiposContratosLibre.SelectedRows)
                {
                    Data.AlquileresTiposContratacionesTiposContratos dato = new Data.AlquileresTiposContratacionesTiposContratos();
                    dato.AlquilerTipoContratoID = Int64.Parse(selec.RecordID);
                    dato.AlquilerTipoContratacionID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    dato.Activo = true;
                    cContratacionesContratos.AddItem(dato);
                }

                storeTiposContratos.DataBind();
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                direct.Success = true;
            }

            return direct;
        }

        /// <summary>
        /// Quitar tipo Contratos al tipo de contratacion
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse QuitarTipoContrato()
        {
            DirectResponse direct = new DirectResponse();
            AlquileresTiposContratacionesTiposContratosController cContratacionesContratos = new AlquileresTiposContratacionesTiposContratosController();

            try
            {
                foreach (SelectedRow selec in GridRowSelectTiposContratos.SelectedRows)
                {
                    cContratacionesContratos.DeleteItem(Int64.Parse(selec.RecordID));
                }

                storeTiposContratos.DataBind();
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                direct.Success = true;
            }

            return direct;
        }
        #endregion

        [DirectMethod()]
        public DirectResponse CargarExcelContratacionesTiposContratos()
        {
            DirectResponse direct = new DirectResponse();

            string sLineaActual = string.Empty;
            string sDirectorio = TreeCore.DirectoryMapping.GetTemplatesTempDirectory();

            List<Data.Vw_AlquileresTiposContratacionesTiposContratos> lista_Impresion = new List<Data.Vw_AlquileresTiposContratacionesTiposContratos>();
            long cliID = 0;
            try
            {

                string resultado = "";
                MigradorController cController = new MigradorController();

                // Datos del archivo resultante
                string nombreArchivoRes = "Res_Prueba.json"; //+ Path.GetFileName(fileUploadCarga.PostedFile.FileName).Substring(0, Path.GetFileName(fileUploadCarga.PostedFile.FileName).LastIndexOf(".xlsx")) + ".txt";
                string path = "C:\\Desarrollo\\Tree Telefonica\\documentos" + "\\" + nombreArchivoRes;
                bool Configuracion = true;
                bool Datos = false;
                long proyectoTipoID = (long)Comun.Modulos.GLOBAL;

                //Carga del archivo
                List<MigradorTablas> migradorTablas = new List<MigradorTablas>();
                migradorTablas = cController.GetItemTablas(Configuracion, Datos, proyectoTipoID, true);
                foreach (MigradorTablas mt in migradorTablas)
                {
                    if (mt.TablaVistaMigracion != null && mt.TablaVistaMigracion != "")
                    {
                        resultado = resultado + cController.JsonTabla(mt.TablaVistaMigracion);
                    }
                    else
                    {
                        resultado = resultado + cController.JsonTabla(mt.Tabla);
                    }
                }


                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(resultado);

                    }
                }
                resultado = "";




                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, TreeCore.Global.DirectoryMapping.GetFileTempDirectoryRelative((long)Comun.Modulos.GLOBAL, path), false);
                direct.Result = "";
                direct.Success = true;

            }
            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                Response.Write("ERROR: " + codTit + "<br>" + Comun.ERRORAJAXGENERICO);
                log.Error(ex.Message);

                direct.Success = true;
                return direct;
            }

            return direct;
        }

        #endregion

        #region FUNCTIONS

        #endregion

    }
}