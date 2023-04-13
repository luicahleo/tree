using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class GlobalActaTecnicasFabricantesModelos : TreeCore.Page.BasePageExtNet
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
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sModuloID = Request.QueryString["aux"].ToString();
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.GlobalAntenasFabricantes> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, CliID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
                            List<Data.GlobalAntenasModelos> ListaDatosDetalle = null;

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
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_GLOBAL_ACTA_TECNICAS_FABRICANTES_MODELOS))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;

                btnAnadirDetalle.Hidden = true;
                btnEditarDetalle.Hidden = true;
                btnEliminarDetalle.Hidden = true;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = true;
            }
            else if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_GLOBAL_ACTA_TECNICAS_FABRICANTES_MODELOS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;

                btnAnadirDetalle.Hidden = false;
                btnEditarDetalle.Hidden = false;
                btnEliminarDetalle.Hidden = false;
                btnActivarDetalle.Hidden = false;
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
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.GlobalAntenasFabricantes> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.GlobalAntenasFabricantes> ListaDatos;
            GlobalAntenasFabricantesController CGlobalAntenasFabricantes = new GlobalAntenasFabricantesController();

            try
            {
                if (ClienteID.HasValue)
                {
                    ListaDatos = CGlobalAntenasFabricantes.GetItemsWithExtNetFilterList<Data.GlobalAntenasFabricantes>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    ListaDatos = null;
                }
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

                    if (!hdModuloID.Value.Equals(""))
                    {
                        lMaestroID = Convert.ToInt64(hdModuloID.Value);
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

        private List<Data.GlobalAntenasModelos> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.GlobalAntenasModelos> ListaDatos;

            try
            {
                GlobalAntenasModelosController CGlobalAntenasModelos = new GlobalAntenasModelosController();

                ListaDatos = CGlobalAntenasModelos.GetItemsWithExtNetFilterList<Data.GlobalAntenasModelos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "GlobalAntenaFabricanteID == " + lMaestroID);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ListaDatos = null;
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

        #endregion

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();

            GlobalAntenasFabricantesController cGlobalAntenasFabricantes = new GlobalAntenasFabricantesController();

            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.GlobalAntenasFabricantes oDato;
                    oDato = cGlobalAntenasFabricantes.GetItem(lS);

                    if (oDato.Fabricante == txtFabricante.Text)
                    {
                        oDato.Fabricante = txtFabricante.Text;
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());
                        if (cGlobalAntenasFabricantes.RegistroDuplicado(txtFabricante.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Fabricante = txtFabricante.Text;
                        }
                    }
                    if (cGlobalAntenasFabricantes.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cGlobalAntenasFabricantes.RegistroDuplicado(txtFabricante.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalAntenasFabricantes oDato = new Data.GlobalAntenasFabricantes
                        {
                            Fabricante = txtFabricante.Text,
                            Activo = true,
                            ClienteID = lCliID
                        };

                        if (cGlobalAntenasFabricantes.AddItem(oDato) != null)
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

            GlobalAntenasFabricantesController cGlobalAntenasFabricantes = new GlobalAntenasFabricantesController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalAntenasFabricantes oDato;
                oDato = cGlobalAntenasFabricantes.GetItem(lS);
                txtFabricante.Text = oDato.Fabricante;

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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalAntenasFabricantesController cController = new GlobalAntenasFabricantesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalAntenasFabricantes oDato;
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

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalAntenasFabricantesController CGlobalAntenasFabricantes = new GlobalAntenasFabricantesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CGlobalAntenasFabricantes.DeleteItem(lID))
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
        public DirectResponse CargarExcelFabricanteYmodelos()
        {
            DirectResponse direct = new DirectResponse();

            string sLineaActual = String.Empty;
            string sDirectorio = DirectoryMapping.GetTemplatesTempDirectory();

            try
            {
                string sRandom = System.IO.Path.GetRandomFileName().Replace(".", "") + Comun.DOCUMENTO_EXTENSION_EXCEL;
                string sNombreArchivo;

                sNombreArchivo = "GLOBAL_Fabricante_Modelo" + DateTime.Today.ToString("yyyyMMdd") + "-" + sRandom;

                sDirectorio = System.IO.Path.Combine(sDirectorio, sNombreArchivo);

                StreamWriter wwriter = new StreamWriter(sDirectorio, false, System.Text.Encoding.Unicode);

                // write in the file, what type of separator we use so this will open on various locales correctly
                wwriter.WriteLine(Comun.EXCEL_SEP_HEADER);


                #region OBTENER DATOS PARA MOSTRAR EN EL EXCEL

                /* Obtener la lista de Usuarios Perfiles */
                List<Data.Vw_GlobalAntenasFabricantes> lista_GlobalAntenasModelos;
                lista_GlobalAntenasModelos = Lista_GlobalAntenaModelo();

                #endregion

                #region OBTENER COLUMNAS DE LA GRILLA

                /* Escribir los títulos de las columnas del Excel */

                sLineaActual = GetGlobalResource(Comun.strFabricante) + ";" +
                        GetGlobalResource(Comun.strFabricante) + " " + GetGlobalResource(Comun.strActivo) + ";" +
                        GetGlobalResource(Comun.strModelo) + ";" +
                        GetGlobalResource(Comun.strActivo) + ";" +
                        GetGlobalResource(Comun.strDimensiones) + ";" +
                        GetGlobalResource(Comun.strAEV) + ";" +
                        GetGlobalResource(Comun.strAEVCA) + ";" +
                        GetGlobalResource(Comun.strCA);

                wwriter.WriteLine(sLineaActual);

                #endregion

                #region filas de contenido

                int index = 0;
                foreach (Data.Vw_GlobalAntenasFabricantes item in lista_GlobalAntenasModelos)
                {
                    sLineaActual = string.Empty;

                    sLineaActual += string.Format("{0};", item.Fabricante);
                    sLineaActual += string.Format("{0};", item.ActivoFabricante);
                    sLineaActual += string.Format("{0};", item.Modelo);
                    sLineaActual += string.Format("{0};", item.Activo);
                    sLineaActual += string.Format("{0};", item.Dimensiones);
                    sLineaActual += string.Format("{0};", item.AEV);
                    sLineaActual += string.Format("{0};", item.AEVCA);
                    sLineaActual += string.Format("{0};", item.CA);

                    wwriter.WriteLine(sLineaActual);

                    index++;
                }

                #endregion

                wwriter.Close();
                /* Registrar comentarios de Exportacion de Excel en "Estadisticas" */
                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);

                Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(sNombreArchivo), false);

                direct.Result = "";
                direct.Success = true;
            }
            catch (Exception ex)
            {
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                Response.Write("ERROR: " + codTit + "<br>" + Comun.ERRORAJAXGENERICO);
                log.Error(ex.Message);

                direct.Success = true;
                return direct;
            }

            return direct;
        }

        #endregion

        #region GESTION DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle(long lModuloID)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };

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
        public DirectResponse AgregarEditarDetalle(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();

            GlobalAntenasModelosController cGlobalAntenasModelos = new GlobalAntenasModelosController();

            try
            {
                long lGrupoID = Int64.Parse(GridRowSelect.SelectedRecordID);
                if (!bAgregar)
                {

                    long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.GlobalAntenasModelos oDato;
                    oDato = cGlobalAntenasModelos.GetItem(lID);

                    if (oDato.Modelo == txtModelo.Text)
                    {
                        oDato.AEV = Comun.ConvertPuntosEnComasFloat(txtAEV.Text);
                        oDato.AEVCA = Comun.ConvertPuntosEnComasFloat(txtAEVCA.Text);
                        oDato.CA = Comun.ConvertPuntosEnComasFloat(txtCA.Text);
                        oDato.Modelo = txtModelo.Text;
                        oDato.Dimensiones = txtDimensiones.Text;

                    }
                    else
                    {
                        if (cGlobalAntenasModelos.RegistroDuplicado(txtModelo.Text, lGrupoID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.AEV = Comun.ConvertPuntosEnComasFloat(txtAEV.Text);
                            oDato.AEVCA = Comun.ConvertPuntosEnComasFloat(txtAEVCA.Text);
                            oDato.CA = Comun.ConvertPuntosEnComasFloat(txtCA.Text);
                            oDato.Modelo = txtModelo.Text;
                            oDato.Dimensiones = txtDimensiones.Text;
                        }
                    }

                    if (cGlobalAntenasModelos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storeDetalle.DataBind();
                    }
                }
                else
                {

                    if (cGlobalAntenasModelos.RegistroDuplicado(txtModelo.Text, lGrupoID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalAntenasModelos oDato = new Data.GlobalAntenasModelos
                        {
                            AEV = Comun.ConvertPuntosEnComasFloat(txtAEV.Text),
                            AEVCA = Comun.ConvertPuntosEnComasFloat(txtAEVCA.Text),
                            CA = Comun.ConvertPuntosEnComasFloat(txtCA.Text),
                            Modelo = txtModelo.Text,
                            Dimensiones = txtDimensiones.Text,
                            Activo = true,
                            GlobalAntenaFabricanteID = Convert.ToInt64(GridRowSelect.SelectedRecordID)
                        };

                        if (cGlobalAntenasModelos.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeDetalle.DataBind();
                        }
                    }
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

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();

            try
            {
                long lId = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.GlobalAntenasModelos oDato;
                GlobalAntenasModelosController cGlobalAntenasModelos = new GlobalAntenasModelosController();
                oDato = cGlobalAntenasModelos.GetItem(lId);
                txtModelo.Text = oDato.Modelo.ToString();
                txtDimensiones.Text = oDato.Dimensiones.ToString();
                txtAEV.Text = oDato.AEV.ToString();
                txtAEVCA.Text = oDato.AEVCA.ToString();
                txtCA.Text = oDato.CA.ToString();

                winGestionDetalle.Show();
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

        [DirectMethod()]
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalAntenasModelosController cGlobalAntenasModelos = new GlobalAntenasModelosController();

            direct.Result = "";
            direct.Success = true;


            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (cGlobalAntenasModelos.DeleteItem(lID))
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
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalAntenasModelosController cGlobalAntenasModelos = new GlobalAntenasModelosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.GlobalAntenasModelos oDato;
                oDato = cGlobalAntenasModelos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cGlobalAntenasModelos.UpdateItem(oDato))
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        #region FUNCTIONS

        private List<Data.Vw_GlobalAntenasFabricantes> Lista_GlobalAntenaModelo()
        {
            List<Data.Vw_GlobalAntenasFabricantes> lista_GlobalAntenasModelos;

            try
            {
                GlobalAntenasModelosController cController = new GlobalAntenasModelosController();

                /* Obtener la lista de GlobalAntenasFabricantes */
                lista_GlobalAntenasModelos = cController.GetListVwOrderByGlobalAntenaFabricanteID(); //Inflacion
            }
            catch (Exception ex)
            {

                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                lista_GlobalAntenasModelos = null;
                log.Error(ex.Message);
            }

            return lista_GlobalAntenasModelos;
        }

        #endregion

    }
}