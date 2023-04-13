using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Ext.Net;
using TreeCore.APIClient;
using TreeCore.Page;
using TreeCore.Shared.DTO.Contracts;
using CapaNegocio;
using TreeCore.Data;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.General;
using Newtonsoft.Json;
using System.IO;

namespace TreeCore.Modulos.Contracts
{
    public partial class Contratos : BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseAPIClient<ContractDTO> ApiClient;
        BaseAPIClient<ViewDTO> viewClient;
        List<ViewDTO> listViews;
        private object Traducciones;

        #region COLUMNAS
        
        List<string> listaCodEst = new List<string>() {
                    "colCode",
                    "colEditar",
                    "colDetalles",
                    "colAcciones"
                };

        List<colObj> colGridObj = new List<colObj>() {
                    new colObj("Code", "strCodigo"),
                    new colObj("Name", "strNombre"),
                    new colObj("ContractStatusCode", "strEstado"),
                    new colObj("SiteCode", "strEmplazamiento"),
                    new colObj("CurrencyCode", "strMoneda"),
                    new colObj("ContractGroupCode", "strGrupo"),
                    new colObj("ContractTypeCode", "strTipo"),
                    new colObj("Description", "strDescripcion"),
                    new colObj("MasterContractNumber", "strContratoMarco"),
                    new colObj("ExecutionDate", "strFechaEjecucion", colObj.Date),
                    new colObj("StartDate", "strFechaInicio", colObj.Date),
                    new colObj("FirsEndDate", "strFechaFin", colObj.Date),
                    new colObj("ClosedAtExpiration", "strCerradoExpirar", colObj.Bool),
                    new colObj("RenewalClause_Type", "strTipoReajustePrecio"),
                    new colObj("RenewalClause_Frequencymonths", "strFrecuenciaReajustePrecio"),
                    new colObj("RenewalClause_TotalRenewalNumber", "strNumeroProrrogas"),
                    new colObj("RenewalClause_CurrentRenewalNumber", "strNumeroRenovacion"),
                    new colObj("RenewalClause_RenewalDate", "strFechaRenovacion", colObj.Date),
                    new colObj("RenewalClause_ExpirationDate", "strFechaExpiracionReajustePrecio", colObj.Date)
                };

        #endregion

        List<ColumnBase> listaCols;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            ApiClient = new BaseAPIClient<ContractDTO>(TOKEN_API);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                Traducciones = new
                {
                    Code = GetGlobalResource("strCodigo"),
                    Name = GetGlobalResource("strNombre"),
                    ContractStatusCode = GetGlobalResource("strEstado"),
                    SiteCode = GetGlobalResource("strEmplazamiento"),
                    CurrencyCode = GetGlobalResource("strMoneda"),
                    ContractGroupCode = GetGlobalResource("strGrupo"),
                    ContractTypeCode = GetGlobalResource("strTipo"),
                    Description = GetGlobalResource("strDescripcion"),
                    MasterContractNumber = GetGlobalResource("strContratoMarco"),
                    ExecutionDate = GetGlobalResource("strFechaEjecucion"),
                    StartDate = GetGlobalResource("strFechaInicio"),
                    FirsEndDate = GetGlobalResource("strFechaFin"),
                    ClosedAtExpiration = GetGlobalResource("strCerradoExpirar"),
                    RenewalClause_Type = GetGlobalResource("strTipoProrroga"),
                    RenewalClause_Frequencymonths = GetGlobalResource("strFrecuenciaReajustePrecio"),
                    RenewalClause_TotalRenewalNumber = GetGlobalResource("strNumeroProrrogas"),
                    RenewalClause_CurrentRenewalNumber = GetGlobalResource("strNumeroRenovacion"),
                    RenewalClause_RenewalExpirationDate = GetGlobalResource("strFechaExpiracionProrroga"),
                    RenewalClause_ContractExpirationDate = GetGlobalResource("strExpiracionContrato"),
                    lineTypeCode = GetGlobalResource("strTipoLinea"),
                    Frequency = GetGlobalResource("strFrecuencia"),
                    Value = GetGlobalResource("strValor"),
                    ValidFrom = GetGlobalResource("strValidoDesde"),
                    ValidTo = GetGlobalResource("strValidoHasta"),
                    NextPaymentDate = GetGlobalResource("strFechaProximoPago"),
                    ApplyRenewals = GetGlobalResource("strAplicaRenovacion"),
                    Apportionment = GetGlobalResource("strProrrateo"),
                    Prepaid = GetGlobalResource("strPagoAnticipado"),
                    PricesReadjustment = GetGlobalResource("strReajustes"),
                    ContractLineTaxes = GetGlobalResource("strImpuestos"),
                    TaxCode = GetGlobalResource("strImpuesto"),
                    ContractLineEntidad = GetGlobalResource("strEntidades"),
                    CompanyCode = GetGlobalResource("strEntidad"),
                    PaymentMethodCode = GetGlobalResource("strMetodoPago"),
                    BankAcountCode = GetGlobalResource("strBankAccounts"),
                    currencyCode = GetGlobalResource("strMoneda"),
                    Percent = GetGlobalResource("strPorcentaje"),
                    Type = GetGlobalResource("strTipoReajustePrecio"),
                    CodeInflation = GetGlobalResource("strCodigoInflacion"),
                    FixedAmount = GetGlobalResource("strCantidadFija"),
                    FixedPercentage = GetGlobalResource("strPorcentajeFijo"),
                    NextDate = GetGlobalResource("strFechaProximaRevision"),
                    EndDate = GetGlobalResource("strFechaFinReajuste"),
                };

                hdTraducciones.Value = JsonConvert.SerializeObject(Traducciones);
                List<string> listaIgnore = new List<string>()
                { };

                //#region FILTROS

                //Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                //log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //#endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridContratos, storeContratos, gridContratos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                GenerarColumnas();

                if (hdJsonCols.Value != null && hdJsonCols.Value.ToString() != "")
                {
                    ActualizarGrid();
                }
                else
                {
                    GenerarGrid();
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };
            
        }


        #endregion

        #region STORES

        #region CONTRATOS

        protected void storeContratos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {

                    int curPage = e.Page;
                    int pageSize = Convert.ToInt32(e.Limit);
                    var sSort = e.Sort;
                    QueryDTO query = new QueryDTO(sSort.First().Property, sSort.First().Direction.ToString(), pageSize, curPage);
                    if (!cmbFiltroEstados.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(ContractDTO.ContractStatusCode), nameof(Operators.eq), cmbFiltroEstados.Value.ToString(), null));
                    if (!cmbFiltroTipos.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(ContractDTO.ContractTypeCode), nameof(Operators.eq), cmbFiltroTipos.Value.ToString(), null));
                    if (!cmbFiltroGrupos.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(ContractDTO.ContractGroupCode), nameof(Operators.eq), cmbFiltroGrupos.Value.ToString(), null));
                    if (!cmbFiltroMonedas.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(ContractDTO.CurrencyCode), nameof(Operators.eq), cmbFiltroMonedas.Value.ToString(), null));
                    if (!txtFiltroContratos.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(ContractDTO.Code), nameof(Operators.like), $"%{txtFiltroContratos.Value.ToString()}%", null));
                    foreach (var oFilter in e.Filter)
                    {
                        query.AddFilter(new FilterDTO(oFilter.Property, nameof(Operators.like), $"%{oFilter.Value}%", null));
                    }
                    var lista = ApiClient.GetList(query).Result;
                    if (lista != null)
                    {
                        e.Total = lista.TotalItems;
                        storeContratos.DataSource = lista.Value;
                        storeContratos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ESTADOS

        protected void storeEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractStatusDTO> apiCl = new BaseAPIClient<ContractStatusDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeEstados.DataSource = lista.Value;
                        storeEstados.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS

        protected void storeTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractTypeDTO> apiCl = new BaseAPIClient<ContractTypeDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeTipos.DataSource = lista.Value;
                        storeTipos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region GRUPOS

        protected void storeGrupos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractGroupDTO> apiCl = new BaseAPIClient<ContractGroupDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeGrupos.DataSource = lista.Value;
                        storeGrupos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<CurrencyDTO> apiCl = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeMonedas.DataSource = lista.Value;
                        storeMonedas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region VIEWS

        protected void storeViews_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                    FilterDTO filtro = new FilterDTO { Field = "Page", Operator = "eq", Value = System.IO.Path.GetFileName(Request.Url.AbsolutePath) };
                    QueryDTO query = new QueryDTO(new List<FilterDTO>() { filtro });
                    var lista = viewClient.GetList(query).Result.Value;
                    if (lista != null)
                    {
                        storeViews.DataSource = lista;
                        storeViews.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region COLUMNAS

        private class ColumnaStore
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
            public string Active { get; set; }
        }

        protected void storeColumnas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                    FilterDTO filtro = new FilterDTO { Field = "Page", Operator = "eq", Value = System.IO.Path.GetFileName(Request.Url.AbsolutePath) };
                    QueryDTO query = new QueryDTO(new List<FilterDTO>() { filtro });
                    listViews = viewClient.GetList(query).Result.Value;
                    var vistaDefecto = listViews.FirstOrDefault(x => x.Default);
                    var listaColumnas = new List<ColumnaStore>();
                    foreach (var oCol in colGridObj)
                    {
                        if (oCol.Name == "Code")
                        {
                            continue;
                        }
                        listaColumnas.Add(new ColumnaStore
                        {
                            Code = $"col{oCol.Name}",
                            Name = GetGlobalResource(oCol.Resource),
                            Active = "",
                            Order = 0
                        });
                    }
                    foreach (var sCol in vistaDefecto.Columns.Where(x => x.Visible).OrderBy(x => x.Order))
                    {
                        var oCol = listaColumnas.FirstOrDefault(x => x.Code == sCol.Column);
                        oCol.Active = "checked";
                        oCol.Order = sCol.Order;
                    }
                    if (listaColumnas != null)
                    {
                        storeColumnas.DataSource = listaColumnas;
                        storeColumnas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ICONOS

        public static List<object> ListaIconos;

        protected void storeIconos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (ListaIconos == null || ListaIconos.Count <= 0)
                    {
                        ListaIconos = GetIconsOfInvetory();
                    }
                    storeIconos.DataSource = ListaIconos;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<object> GetIconsOfInvetory()
        {
            List<object> lista = new List<object>();

            string prefix = "ico-*";
            try
            {
                string[] fileEntries = Directory.GetFiles(Comun.rutaIconoInventario(), prefix);
                foreach (string filePath in fileEntries)
                {
                    //string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string fileNameExtension = Path.GetFileName(filePath);

                    lista.Add(new object[] { Comun.rutaIconoWebInventario(fileNameExtension), fileNameExtension });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        #region CONTRACTS



        [DirectMethod]
        public DirectResponse DeleteContract(string code)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                var Result = ApiClient.DeleteEntity(code).Result;

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
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        #region Views


        [DirectMethod]
        public DirectResponse ActualizarGrid()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                GenerarColumnas();
                List<ColumnaStore> listaColsJson = JsonConvert.DeserializeObject<List<ColumnaStore>>(hdJsonColsNew.Value.ToString());
                if (listaCols.Count == 0)
                {
                    GenerarViewDefault();
                }
                List<ColumnBase> listaColsFinal = new List<ColumnBase>();
                foreach (var oCod in listaCodEst)
                {
                    listaColsFinal.Add(listaCols.FirstOrDefault(x => x.ID == oCod));
                }

                foreach (var col in listaColsJson)
                {
                    listaColsFinal.Add(listaCols.FirstOrDefault(x => x.ID == col.Code));
                }
                if (hdJsonColsNew.Value.ToString() != hdJsonCols.Value.ToString())
                {
                    gridContratos.RemoveAllColumns();
                }
                foreach (var oCol in listaColsFinal)
                {
                    gridContratos.AddColumn(oCol);
                }
                hdJsonCols.Value = hdJsonColsNew.Value;

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storeContratos, gridContratos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse DefaultView(string code)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                FilterDTO filtro = new FilterDTO { Field = "Page", Operator = "eq", Value = System.IO.Path.GetFileName(Request.Url.AbsolutePath) };
                FilterDTO filtro2 = new FilterDTO { Field = "Code", Operator = "eq", Value = code };
                QueryDTO query = new QueryDTO(new List<FilterDTO>() { filtro, filtro2 });
                listViews = viewClient.GetList(query).Result.Value;

                List<ColumnDTO> listaCols = new List<ColumnDTO>();
                var viewAct = listViews.FirstOrDefault();
                viewAct.Default = true;
                var Result = viewClient.UpdateEntity(code, viewAct).Result;

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
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse DeleteView(string code)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                var Result = viewClient.DeleteEntity(code).Result;

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
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse GuardarView(bool Editar, string code)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                ActualizarGrid();
                List<ColumnaStore> listaColsJson = JsonConvert.DeserializeObject<List<ColumnaStore>>(hdJsonColsNew.Value.ToString());
                FilterDTO filtro = new FilterDTO { Field = "Page", Operator = "eq", Value = System.IO.Path.GetFileName(Request.Url.AbsolutePath) };
                FilterDTO filtro2 = new FilterDTO { Field = "Code", Operator = "eq", Value = code };
                QueryDTO query = new QueryDTO(new List<FilterDTO>() { filtro, filtro2 });
                listViews = viewClient.GetList(query).Result.Value;

                List<ColumnDTO> listaCols = new List<ColumnDTO>();
                foreach (ColumnaStore col in listaColsJson)
                {
                    listaCols.Add(new ColumnDTO
                    {
                        Column = col.Code,
                        ColumnName = col.Name,
                        Order = col.Order,
                        Visible = col.Active != ""
                    });
                }
                var viewAct = listViews.FirstOrDefault();
                if (Editar)
                {
                    viewAct.Code = txtCodigo.Text;
                    viewAct.Icon = cmbIconos.Value.ToString();
                }
                if (code == hdActiveView.Value.ToString())
                {
                    viewAct.Columns = listaCols;
                }
                var Result = viewClient.UpdateEntity(code, viewAct).Result;

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
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }



        [DirectMethod]
        public DirectResponse AddNewView()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                ActualizarGrid();
                List<ColumnaStore> listaColsJson = JsonConvert.DeserializeObject<List<ColumnaStore>>(hdJsonColsNew.Value.ToString());

                List<ColumnDTO> listaCols = new List<ColumnDTO>();
                foreach (ColumnaStore col in listaColsJson)
                {
                    listaCols.Add(new ColumnDTO
                    {
                        Column = col.Code,
                        ColumnName = col.Name,
                        Order = col.Order,
                        Visible = col.Active != ""
                    });
                }
                var view = new ViewDTO
                {
                    Code = txtCodigo.Text,
                    Columns = listaCols,
                    Filters = new List<FilterDTO>() { },
                    Default = false,
                    Icon = cmbIconos.Value.ToString(),
                    Page = System.IO.Path.GetFileName(Request.Url.AbsolutePath)
                };
                var Result = viewClient.AddEntity(view).Result;

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
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        #endregion

        #region FUNCTIONS

        public DirectResponse GenerarColumnas()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                listaCols = new List<ColumnBase>();

                #region Columnas Acciones

                WidgetColumn oColEditar = new WidgetColumn
                {
                    ID = "colEditar",
                    Align = ColumnAlign.Start,
                    Sortable = false,
                    Filterable = false,
                    MinWidth = 50,
                    MaxWidth = 50,
                    DataIndex = "Editar"
                };

                Ext.Net.Button obEditar = new Ext.Net.Button
                {
                    PressedCls = "Pressed-none",
                    FocusCls = "Focus-none",
                    Cls = "btnColumna Hidden btnEditar"
                };
                obEditar.ToolTip = GetGlobalResource("jsEditar");

                obEditar.Listeners.Click.Fn = "EditarContrato";
                oColEditar.Widget.Add(obEditar);
                listaCols.Add(oColEditar);

                WidgetColumn oColDetalles = new WidgetColumn
                {
                    ID = "colDetalles",
                    Align = ColumnAlign.Start,
                    Sortable = false,
                    Filterable = false,
                    MinWidth = 50,
                    MaxWidth = 50,
                    DataIndex = "Detalles"
                };

                Ext.Net.Button obDetalles = new Ext.Net.Button
                {
                    PressedCls = "Pressed-none",
                    FocusCls = "Focus-none",
                    Cls = "btnColumna Hidden btnInfo"
                };

                obDetalles.ToolTip = GetGlobalResource("strVerMas");

                obDetalles.Listeners.Click.Fn = "DetalleContrato";
                oColDetalles.Widget.Add(obDetalles);
                listaCols.Add(oColDetalles);

                WidgetColumn oColAcciones = new WidgetColumn
                {
                    ID = "colAcciones",
                    Align = ColumnAlign.Start,
                    Sortable = false,
                    Filterable= false,
                    MinWidth = 50,
                    MaxWidth = 50,
                    DataIndex = "Acciones"
                };

                Ext.Net.Button obAcciones = new Ext.Net.Button
                {
                    PressedCls = "Pressed-none",
                    FocusCls = "Focus-none",
                    Cls = "btnColumna Hidden btnMoreOptions"
                };

                obAcciones.Listeners.Click.Fn = "AccionesContrato";
                oColAcciones.Widget.Add(obAcciones);
                listaCols.Add(oColAcciones);

                #endregion

                foreach (var oCol in colGridObj)
                {
                    ColumnBase oColBase = new Column
                    {
                        ID = $"col{oCol.Name}",
                        Text = GetGlobalResource(oCol.Resource),
                        DataIndex = oCol.Name.Split('_')[0],
                        MinWidth = 100,
                        Sortable = true,
                        Align = ColumnAlign.Center,
                        Filterable = false,
                        Resizable = true,
                        Flex = 1
                    };
                    oColBase.CustomConfig.Add(new ConfigItem("Tipo", oCol.Tipo));
                    oColBase.CustomConfig.Add(new ConfigItem("Indice", oCol.Name));
                    oColBase.Renderer = new Renderer { Fn = "RenderColDinamica" };
                    listaCols.Add(oColBase);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        public void GenerarViewDefault()
        {
            listViews = new List<ViewDTO>();
            List<ColumnDTO> columns = new List<ColumnDTO>() {
                new ColumnDTO{ Order = 0, ColumnName = "ContractStatusCode", Column = "colContractStatusCode", Visible = true },
                new ColumnDTO{ Order = 1, ColumnName = "SiteCode", Column = "colSiteCode", Visible = true },
                new ColumnDTO{ Order = 2, ColumnName = "Name", Column = "colName", Visible = true },
                new ColumnDTO{ Order = 3, ColumnName = "ContractTypeCode", Column = "colContractTypeCode", Visible = true },
                new ColumnDTO{ Order = 4, ColumnName = "StartDate", Column = "colStartDate", Visible = true },
            };

            ViewDTO vista = new ViewDTO
            {
                Code = GetGlobalResource("strDefecto"),
                Default = true,
                Columns = columns,
                Filters = new List<Shared.DTO.Query.FilterDTO>(),
                Page = System.IO.Path.GetFileName(Request.Url.AbsolutePath),
                Icon = "ico-heater.svg"
            };
            var FinalView = viewClient.AddEntity(vista).Result;
            listViews.Add(FinalView.Value);
        }

        public DirectResponse GenerarGrid()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                viewClient = new BaseAPIClient<ViewDTO>(TOKEN_API);
                FilterDTO filtro = new FilterDTO { Field = "Page", Operator = "eq", Value = System.IO.Path.GetFileName(Request.Url.AbsolutePath) };
                QueryDTO query = new QueryDTO(new List<FilterDTO>() { filtro });
                listViews = viewClient.GetList(query).Result.Value;
                if (listViews.Count == 0)
                {
                    GenerarViewDefault();
                }
                List<ColumnBase> listaColsFinal = new List<ColumnBase>();
                foreach (var oCod in listaCodEst)
                {
                    listaColsFinal.Add(listaCols.FirstOrDefault(x => x.ID == oCod));
                }

                var vistaDefecto = listViews.FirstOrDefault(x => x.Default);

                List<ColumnaStore> listaHd = new List<ColumnaStore>();

                foreach (var col in vistaDefecto.Columns)
                {
                    listaHd.Add(new ColumnaStore
                    {
                        Code = col.Column,
                        Name = col.ColumnName,
                        Order = col.Order,
                        Active = (col.Visible) ? "checked" : ""
                    });
                }

                hdJsonCols.Value = JsonConvert.SerializeObject(listaHd);
                hdJsonColsNew.Value = JsonConvert.SerializeObject(listaHd);

                hdActiveView.Value = vistaDefecto.Code;
                foreach (var col in vistaDefecto.Columns.Where(x => x.Visible).OrderBy(x => x.Order))
                {
                    listaColsFinal.Add(listaCols.FirstOrDefault(x => x.ID == col.Column));
                }

                gridContratos.ColumnModel.Columns.AddRange(listaColsFinal);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storeContratos, gridContratos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

    }
}