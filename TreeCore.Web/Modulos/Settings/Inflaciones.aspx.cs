using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Data.SqlClient;
using log4net;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;
using Newtonsoft.Json.Linq;
using TreeCore.Page;
using Newtonsoft.Json;

namespace TreeCore.ModGlobal
{
    public partial class Inflaciones : TreeCore.Page.BasePageExtNet
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

                //List<string> ListaIgnoreDetalle = new List<string>()
                //{ };

                //Comun.CreateGridFilters(gridFiltersDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridMaestro, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);
                //Comun.Seleccionable(GridDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);
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
                string sEntidad = Request.QueryString["aux3"].ToString();

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<InflationDTO> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        bool bActivo = Request.QueryString["aux"] == "true";

                        List<FilterDTO> filtros = new List<FilterDTO>();
                        if (sFiltro != "")
                        {
                            foreach (var oFiltro in JsonConvert.DeserializeObject<List<FilterExtNet>>(sFiltro))
                            {
                                filtros.Add(new FilterDTO
                                {
                                    Field = oFiltro.property,
                                    Value = oFiltro.value,
                                    Operator = oFiltro.@operator
                                });
                            }
                        }
                        List<string> orders = new List<string>();
                        if (sOrden != "")
                        {
                            orders.Add(sOrden);
                        }

                        QueryDTO queryDTO = new QueryDTO(filtros, orders, sDir);

                        BaseAPIClient<InflationDTO> aPIClient = new BaseAPIClient<InflationDTO>(TOKEN_API);

                        listaDatos = aPIClient.GetList(queryDTO).Result.Value;

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombreTask(gridMaestro.ColumnModel, listaDatos, Response, "", GetGlobalResource("strInflaciones").ToString(), _Locale).Wait();
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
                //storePrincipal.Reload();
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
                { "Put", new List<ComponentBase> { btnEditar, btnActivar }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #endregion

        #region STORES

        #region MAESTRO

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<InflationDTO> ApiClient = new BaseAPIClient<InflationDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        //#region DETALLE

        //protected void storeDetalle_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            string sSort, sDir;
        //            sSort = e.Sort[0].Property.ToString();
        //            sDir = e.Sort[0].Direction.ToString();
        //            int iCount = 0;
        //            string sFiltro = e.Parameters["gridFiltersDetalle"];

        //            if (!ModuloID.Value.Equals(""))
        //            {
        //                lMaestroID = Convert.ToInt64(ModuloID.Value);
        //            }

        //            var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID);

        //            if (vLista != null)
        //            {
        //                storeDetalle.DataSource = vLista;

        //                PageProxy temp;
        //                temp = (PageProxy)storeDetalle.Proxy[0];
        //                temp.Total = iCount;
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //            MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
        //        }
        //    }
        //}

        //private List<Data.InflacionesDetalles> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        //{
        //    List<Data.InflacionesDetalles> ListaDatos = new List<Data.InflacionesDetalles>();
        //    InflacionesDetallesController cInflacionesDetalles = new InflacionesDetallesController();

        //    try
        //    {
        //        ListaDatos = cInflacionesDetalles.GetItemsWithExtNetFilterList<Data.InflacionesDetalles>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "InflacionID == " + lMaestroID);
        //    }

        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //    }

        //    return ListaDatos;
        //}


        //#endregion

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CountryDTO> ApiClient = new BaseAPIClient<CountryDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
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

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<InflationDTO> ApiClient = new BaseAPIClient<InflationDTO>(TOKEN_API);
            InflationDTO oDato;

            try
            {
                if (!bAgregar)
                {
                    string oCode = GridRowSelect.SelectedRecordID;

                    oDato = ApiClient.GetByCode(oCode).Result.Value;

                    var originalCode = oDato.Code;

                    oDato.Name = txtInflacion.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.CountryName = cmbPais.SelectedItem.Text.ToString();
                    oDato.Description = txtDescripcion.Text;
                    oDato.Estandar = (bool)chkEstandar.Value;

                    var Result = ApiClient.UpdateEntity(originalCode, oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
                    }
                }
                else
                {
                    oDato = new InflationDTO
                    {
                        Name = txtInflacion.Text,
                        Code = txtCodigo.Text,
                        CountryName = cmbPais.SelectedItem.Text.ToString(),
                        Description = txtDescripcion.Text,
                        Estandar = (bool)chkEstandar.Value,
                        Active = true,
                    };

                    var Result = ApiClient.AddEntity(oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
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
            hdEditando.SetValue("Editar");

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                BaseAPIClient<InflationDTO> ApiClient = new BaseAPIClient<InflationDTO>(TOKEN_API);
                var oDato = ApiClient.GetByCode(oCode).Result;
                txtInflacion.Text = oDato.Value.Name;
                txtCodigo.Text = oDato.Value.Code;
                cmbPais.SetValue(oDato.Value.CountryName);
                txtDescripcion.Text = oDato.Value.Description;
                chkEstandar.SetValue(oDato.Value.Estandar);
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<InflationDTO> ApiClient = new BaseAPIClient<InflationDTO>(TOKEN_API);

            var lID = GridRowSelect.SelectedRecordID;

            try
            {
                var Result = ApiClient.DeleteEntity(lID).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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
            BaseAPIClient<InflationDTO> ApiClient = new BaseAPIClient<InflationDTO>(TOKEN_API);
            InflationDTO oDato;

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                oDato = ApiClient.GetByCode(oCode).Result.Value;
                oDato.Active = !oDato.Active;

                var Result = ApiClient.UpdateEntity(oDato.Code, oDato).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
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
        public DirectResponse CargarExcelInflacionYdetalles()
        {
            DirectResponse direct = new DirectResponse();

            string sLineaActual = String.Empty;
            string sDirectorio = DirectoryMapping.GetTemplatesTempDirectory();

            try
            {
                string sRandom = System.IO.Path.GetRandomFileName().Replace(".", "") + Comun.DOCUMENTO_EXTENSION_EXCEL;
                string sNombreArchivo;

                sNombreArchivo = "GLOBAL_Inflacion_Detall" + DateTime.Today.ToString("yyyyMMdd") + "-" + sRandom;

                sDirectorio = System.IO.Path.Combine(sDirectorio, sNombreArchivo);

                StreamWriter wwriter = new StreamWriter(sDirectorio, false, System.Text.Encoding.Unicode);

                // write in the file, what type of separator we use so this will open on various locales correctly
                wwriter.WriteLine(Comun.EXCEL_SEP_HEADER);

                if (wwriter != null)
                {
                    #region OBTENER DATOS PARA MOSTRAR EN EL EXCEL

                    /* Obtener la lista de Usuarios Perfiles */
                    List<Data.Vw_Inflaciones> lista_InflacionesDetalles = new List<Data.Vw_Inflaciones>();
                    lista_InflacionesDetalles = Lista_InflacionDetalles();

                    #endregion

                    #region OBTENER COLUMNAS DE LA GRILLA

                    sLineaActual = GetLocalResourceObject("jsTituloModulo").ToString() + ";" +
                            GetGlobalResource(Comun.strPais) + ";" +
                            GetLocalResourceObject("colEstandar.Text").ToString() + ";" +
                            GetGlobalResource(Comun.strValor) + ";" +
                            GetGlobalResource(Comun.strMes) + ";" +
                            GetLocalResourceObject("columnAnualidad.Text").ToString() + ";" +
                            GetGlobalResource(Comun.strActivo) + ";" +
                            GetLocalResourceObject("columFechaDesde.Text").ToString() + ";" +
                            GetLocalResourceObject("columFechaHasta.Text").ToString();

                    wwriter.WriteLine(sLineaActual);
                    sLineaActual = String.Empty;

                    #endregion

                    #region filas de contenido

                    int index = 0;
                    foreach (Data.Vw_Inflaciones item in lista_InflacionesDetalles)
                    {
                        sLineaActual = String.Empty;

                        sLineaActual += String.Format("{0};", item.Inflacion);
                        sLineaActual += String.Format("{0};", item.Pais);
                        sLineaActual += String.Format("{0};", item.Estandar);
                        sLineaActual += String.Format("{0};", item.Valor);
                        sLineaActual += String.Format("{0};", item.Mes);
                        sLineaActual += String.Format("{0};", item.Anualidad);
                        sLineaActual += String.Format("{0};", item.ActivoInflacionDetalle);
                        sLineaActual += String.Format("{0};", item.FechaDesde);
                        sLineaActual += String.Format("{0};", item.FechaHasta);

                        wwriter.WriteLine(sLineaActual);
                        sLineaActual = String.Empty;

                        index++;
                    }

                    #endregion

                    wwriter.Close();
                    /* Registrar comentarios de Exportacion de Excel en "Estadisticas" */
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                    wwriter = null;
                }
                Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(sNombreArchivo), false);

                direct.Result = "";
                direct.Success = true;

            }
            catch (Exception)
            {

            }

            return direct;
        }

        private List<Data.Vw_Inflaciones> Lista_InflacionDetalles()
        {
            List<Data.Vw_Inflaciones> listaInflacionesDetalles = null;

            try
            {
                InflacionesDetallesController cController = new InflacionesDetallesController();
                listaInflacionesDetalles = cController.GetItemsList<Vw_Inflaciones>("", "InflacionID");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return listaInflacionesDetalles;
        }

        #endregion

        //#region GESTION DETALLE

        //[DirectMethod()]
        //public DirectResponse AgregarEditarDetalle(bool bAgregar)
        //{
        //    DirectResponse ajax = new DirectResponse();
        //    InflacionesDetallesController cController = new InflacionesDetallesController();

        //    try
        //    {
        //        if (!bAgregar)
        //        {
        //            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
        //            long lIDMaestro = Int64.Parse(GridRowSelect.SelectedRecordID);
        //            Data.InflacionesDetalles oDato;                  

        //            bool bTieneAlquileres = true;
        //            bool bComprobarDuplicidadAlEditar = false;


        //            if (bTieneAlquileres == false)
        //            {
        //                oDato = cController.GetItem(lID);

        //                if (txtValor.Text != "")
        //                {
        //                    oDato.Valor = Convert.ToDouble(txtValor.Number);
        //                }
        //                else
        //                {
        //                    oDato.Valor = 0;
        //                }

        //                if (oDato.Anualidad != Convert.ToInt32(txtAnualidad.Text))
        //                {
        //                    bComprobarDuplicidadAlEditar = true;
        //                }

        //                if (oDato.Mes != Convert.ToInt32(txtMes.Text))
        //                {
        //                    bComprobarDuplicidadAlEditar = true;
        //                }
        //                if (oDato.FechaDesde != dateDesde.SelectedDate)
        //                {
        //                    bComprobarDuplicidadAlEditar = true;
        //                }
        //                if (oDato.FechaHasta != dateHasta.SelectedDate)
        //                {
        //                    bComprobarDuplicidadAlEditar = true;
        //                }

        //                oDato.Anualidad = Convert.ToInt32(txtAnualidad.Text);

        //                if (txtMes.Text != "")
        //                {
        //                    oDato.Mes = Convert.ToInt32(txtMes.Text);
        //                }
        //                else
        //                {
        //                    oDato.Mes = null;
        //                }

        //                oDato.FechaDesde = dateDesde.SelectedDate;
        //                oDato.FechaHasta = dateHasta.SelectedDate;

        //                if (txtValorAnual.Text != "")
        //                {
        //                    oDato.Anual = Convert.ToDouble(txtValorAnual.Number);
        //                }
        //                else
        //                {
        //                    oDato.Anual = 0;
        //                }
        //                if (txtValorInteranual.Text != "")
        //                {
        //                    oDato.Interanual = Convert.ToDouble(txtValorInteranual.Number);
        //                }
        //                else
        //                {
        //                    oDato.Interanual = 0;
        //                }
        //                if (txtValorTrimestral.Text != "")
        //                {
        //                    oDato.Trimestral = Convert.ToDouble(txtValorTrimestral.Number);
        //                }
        //                else
        //                {
        //                    oDato.Trimestral = 0;
        //                }
        //                if (txtValorCuatrimestral.Text != "")
        //                {
        //                    oDato.Cuatrimestral = Convert.ToDouble(txtValorCuatrimestral.Number);
        //                }
        //                else
        //                {
        //                    oDato.Cuatrimestral = 0;
        //                }
        //                if (txtValorSemestral.Text != "")
        //                {
        //                    oDato.Semestral = Convert.ToDouble(txtValorSemestral.Number);
        //                }
        //                else
        //                {
        //                    oDato.Semestral = 0;
        //                }
        //                if (txtValorAcumulado.Text != "")
        //                {
        //                    oDato.Acumulado = Convert.ToDouble(txtValorAcumulado.Number);
        //                }
        //                else
        //                {
        //                    oDato.Acumulado = 0;
        //                }
        //                if (bComprobarDuplicidadAlEditar)
        //                {
        //                    if (!cController.hasDuplicadosNuevoByFecha(oDato.InflacionID, dateDesde.SelectedDate, dateHasta.SelectedDate, Convert.ToInt32(txtValorAnual.Number), Convert.ToInt32(txtMes.Text)))
        //                    {
        //                        if (cController.UpdateItem(oDato))
        //                        {
        //                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //                            storeDetalle.DataBind();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
        //                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
        //                    }
        //                }
        //                if (cController.UpdateItem(oDato))
        //                {
        //                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //                    storeDetalle.DataBind();
        //                }
        //            }
        //            else
        //            {
        //                MensajeBox(GetGlobalResource(Comun.jsInfo), GetLocalResourceObject("jsAsociadoAlquiler").ToString(), MessageBox.Icon.WARNING, null);
        //            }
        //        }
        //        else
        //        {
        //            Data.InflacionesDetalles oDato = new Data.InflacionesDetalles();

        //            oDato.InflacionID = Convert.ToInt64(GridRowSelect.SelectedRecordID);

        //            if (txtValor.Text != "")
        //            {
        //                oDato.Valor = Convert.ToDouble(txtValor.Number);
        //            }
        //            else
        //            {
        //                oDato.Valor = 0;
        //            }

        //            oDato.Anualidad = Convert.ToInt32(txtAnualidad.Text);

        //            if (txtMes.Text != "")
        //            {
        //                oDato.Mes = Convert.ToInt32(txtMes.Text);
        //            }
        //            else
        //            {
        //                oDato.Mes = null;
        //            }

        //            oDato.FechaDesde = dateDesde.SelectedDate;
        //            oDato.FechaHasta = dateHasta.SelectedDate;

        //            if (txtValorAnual.Text != "")
        //            {
        //                oDato.Anual = Convert.ToDouble(txtValorAnual.Number);
        //            }
        //            else
        //            {
        //                oDato.Anual = 0;
        //            }
        //            if (txtValorInteranual.Text != "")
        //            {
        //                oDato.Interanual = Convert.ToDouble(txtValorInteranual.Number);
        //            }
        //            else
        //            {
        //                oDato.Interanual = 0;
        //            }
        //            if (txtValorTrimestral.Text != "")
        //            {
        //                oDato.Trimestral = Convert.ToDouble(txtValorTrimestral.Number);
        //            }
        //            else
        //            {
        //                oDato.Trimestral = 0;
        //            }
        //            if (txtValorCuatrimestral.Text != "")
        //            {
        //                oDato.Cuatrimestral = Convert.ToDouble(txtValorCuatrimestral.Number);
        //            }
        //            else
        //            {
        //                oDato.Cuatrimestral = 0;
        //            }
        //            if (txtValorSemestral.Text != "")
        //            {
        //                oDato.Semestral = Convert.ToDouble(txtValorSemestral.Number);
        //            }
        //            else
        //            {
        //                oDato.Semestral = 0;
        //            }
        //            if (txtValorAcumulado.Text != "")
        //            {
        //                oDato.Acumulado = Convert.ToDouble(txtValorAcumulado.Number);
        //            }
        //            else
        //            {
        //                oDato.Acumulado = 0;
        //            }

        //            oDato.Activo = true;
        //            oDato.InflacionID = Convert.ToInt64(GridRowSelect.SelectedRecordID);

        //            if (!cController.hasDuplicadosNuevoByFecha(oDato.InflacionID, dateDesde.SelectedDate, dateHasta.SelectedDate, Convert.ToInt32(txtValorAnual.Number), Convert.ToInt32(txtMes.Text)))
        //            {
        //                cController.AddItem(oDato);
        //                log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //            }
        //            else
        //            {
        //                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
        //                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
        //            }

        //            storeDetalle.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ajax.Success = false;
        //        ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return ajax;
        //    }

        //    return ajax;
        //}

        //[DirectMethod()]
        //public DirectResponse MostrarEditarDetalle()
        //{
        //    DirectResponse ajax = new DirectResponse();

        //    try
        //    {
        //        CultureInfo.CreateSpecificCulture("es-ES");
        //        long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
        //        Data.InflacionesDetalles oDato = new Data.InflacionesDetalles();
        //        InflacionesDetallesController cController = new InflacionesDetallesController();
        //        oDato = cController.GetItem(lID);

        //        txtValor.Number = oDato.Valor;
        //        txtValor.Text = oDato.Valor.ToString(CultureInfo.CreateSpecificCulture("es-ES"));

        //        if (oDato.Mes != null)
        //        {
        //            txtMes.Number = Convert.ToDouble(oDato.Mes);
        //        }

        //        txtAnualidad.Text = oDato.Anualidad.ToString();

        //        if (oDato.FechaDesde != null)
        //        {
        //            dateDesde.SelectedDate = oDato.FechaDesde.Value;
        //        }
        //        if (oDato.FechaHasta != null)
        //        {
        //            dateHasta.SelectedDate = oDato.FechaHasta.Value;
        //        }
        //        if (oDato.Anual != null)
        //        {
        //            txtValorAnual.Number = (double)oDato.Anual;
        //        }
        //        if (oDato.Interanual != null)
        //        {
        //            txtValorInteranual.Number = (double)oDato.Interanual;
        //        }
        //        if (oDato.Trimestral != null)
        //        {
        //            txtValorTrimestral.Number = (double)oDato.Trimestral;
        //        }
        //        if (oDato.Semestral != null)
        //        {
        //            txtValorSemestral.Number = (double)oDato.Semestral;
        //        }
        //        if (oDato.Cuatrimestral != null)
        //        {
        //            txtValorCuatrimestral.Number = (double)oDato.Cuatrimestral;
        //        }
        //        if (oDato.Acumulado != null)
        //        {
        //            txtValorAcumulado.Number = (double)oDato.Acumulado;
        //        }

        //        winGestionDetalle.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        ajax.Success = false;
        //        ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return ajax;
        //    }

        //    ajax.Success = true;
        //    ajax.Result = "";

        //    return ajax;
        //}

        //[DirectMethod()]
        //public DirectResponse EliminarDetalle()
        //{
        //    DirectResponse direct = new DirectResponse();
        //    InflacionesDetallesController cInflacionesDetalles = new InflacionesDetallesController();
        //    long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

        //    try
        //    {
        //        if (cInflacionesDetalles.DeleteItem(lID))
        //        {
        //            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
        //            direct.Success = true;
        //            direct.Result = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is SqlException Sql)
        //        {
        //            direct.Success = false;
        //            direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
        //            log.Error(Sql.Message);
        //            return direct;
        //        }
        //        else
        //        {
        //            direct.Success = false;
        //            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //            log.Error(ex.Message);
        //            return direct;
        //        }
        //    }

        //    return direct;
        //}

        //[DirectMethod()]
        //public DirectResponse ActivarDetalle()
        //{
        //    DirectResponse direct = new DirectResponse();
        //    InflacionesDetallesController cInflacionesDetalles = new InflacionesDetallesController();

        //    try
        //    {
        //        long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

        //        Data.InflacionesDetalles oDato = cInflacionesDetalles.GetItem(lID);
        //        oDato.Activo = !oDato.Activo;

        //        if (cInflacionesDetalles.UpdateItem(oDato))
        //        {
        //            storeDetalle.DataBind();
        //            log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return direct;
        //    }

        //    direct.Success = true;
        //    direct.Result = "";

        //    return direct;
        //}

        //#endregion

        #region FUNCTIONS

        #endregion
    }
}