using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.Contracts;

namespace TreeCore.ModWorkOrders.pages
{
    public partial class WorkOrders : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseAPIClient<ContractDTO> ApiClient;
        BaseAPIClient<ViewDTO> viewClient;
        List<ViewDTO> listViews;
        private object Traducciones;

        #region COLUMNAS

        List<string> listaCodEst = new List<string>() {
                    "colCode",
                    "colDetalles",
                    "colAcciones"
                };

        List<colObj> colGridObj = new List<colObj>() {
                    new colObj("Name", "strNombre"),
                    new colObj("Type", "strTipo"),
                    new colObj("Status", "strEstado"),
                    new colObj("Customer", "strEntidad"),
                    new colObj("Supplier", "strEntidad"),
                    new colObj("Start", "strFechaInicio", colObj.Date),
                    new colObj("Due", "strFechaFin", colObj.Date),
                    new colObj("Program", "strFechaEjecucion"),
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
                    Type = GetGlobalResource("strTipo"),
                    Status = GetGlobalResource("strEstado"),
                    Customer = GetGlobalResource("strEntidad"),
                    Supplier = GetGlobalResource("strEntidad"),
                    Progress = GetGlobalResource("strTipo"),
                    Start = GetGlobalResource("strFechaInicio"),
                    End = GetGlobalResource("strFechaFin"),
                    Program = GetGlobalResource("strFechaEjecucion")
                };

                hdTraducciones.Value = JsonConvert.SerializeObject(Traducciones);
                List<string> listaIgnore = new List<string>()
                { };

                //#region FILTROS

                //Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                //log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //#endregion

                //#region SELECCION COLUMNAS

                //Comun.Seleccionable(gridWO, storeWO, gridWO.ColumnModel, listaIgnore, _Locale);
                //log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                //#endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                GenerarColumnas();

                GenerarGrid();

                //if (hdJsonCols.Value != null && hdJsonCols.Value.ToString() != "")
                //{
                //    ActualizarGrid();
                //}
                //else
                //{
                //    GenerarGrid();
                //}

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

        protected void storeWO_Refresh(object sender, NodeLoadEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    Node NodeProy = new Node { 
                        NodeID = "Proy",
                        Leaf = false,
                        Expandable= true,
                        Expanded = true
                    };
                    NodeProy.CustomAttributes.Add(new ConfigItem("Code", "CodeProy"));
                    NodeProy.CustomAttributes.Add(new ConfigItem("Name", "NameProy"));

                    Node NodeWO = new Node
                    {
                        NodeID = "WO",
                        Leaf = true
                    };

                    NodeWO.CustomAttributes.Add(new ConfigItem("Code", "Code"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Name", "Name"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Type", "Type"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Status", "Status"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Customer", "Customer"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Supplier", "Supplier"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Progress", "60"));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Start", ""));
                    NodeWO.CustomAttributes.Add(new ConfigItem("End", ""));
                    NodeWO.CustomAttributes.Add(new ConfigItem("Program", "Program"));

                    NodeProy.Children.Add(NodeWO);
                    Node NodeWO2 = new Node
                    {
                        NodeID = "WO2",
                        Leaf = true
                    };

                    NodeWO2.CustomAttributes.Add(new ConfigItem("Code", "Code2"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Name", "Name"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Type", "Type"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Status", "Status"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Customer", "Customer"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Supplier", "Supplier"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Progress", "40"));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Start", ""));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("End", ""));
                    NodeWO2.CustomAttributes.Add(new ConfigItem("Program", "Program"));

                    NodeProy.Children.Add(NodeWO2);

                    e.Nodes.Add(NodeProy);
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        
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
        #endregion

        #region DIRECT METHOD



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

                TreeColumn oColCode = new TreeColumn
                {
                    ID = $"colCode",
                    Text = GetGlobalResource("strCodigo"),
                    DataIndex = "Code",
                    MinWidth = 100,
                    Sortable = true,
                    Align = ColumnAlign.Center,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };

                listaCols.Add(oColCode);

                #region Columnas Acciones

                //WidgetColumn oColEditar = new WidgetColumn
                //{
                //    ID = "colEditar",
                //    Align = ColumnAlign.Start,
                //    Sortable = false,
                //    Filterable = false,
                //    MinWidth = 50,
                //    MaxWidth = 50,
                //    DataIndex = "Editar"
                //};

                //Ext.Net.Button obEditar = new Ext.Net.Button
                //{
                //    PressedCls = "Pressed-none",
                //    FocusCls = "Focus-none",
                //    Cls = "btnColumna Hidden btnEditar"
                //};
                //obEditar.ToolTip = GetGlobalResource("jsEditar");

                //obEditar.Listeners.Click.Fn = "EditarContrato";
                //oColEditar.Widget.Add(obEditar);
                //listaCols.Add(oColEditar);

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

                obDetalles.Listeners.Click.Fn = "DetalleWO";
                oColDetalles.Widget.Add(obDetalles);
                listaCols.Add(oColDetalles);

                WidgetColumn oColAcciones = new WidgetColumn
                {
                    ID = "colAcciones",
                    Align = ColumnAlign.Start,
                    Sortable = false,
                    Filterable = false,
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

                obAcciones.Listeners.Click.Fn = "AccionesWO";
                oColAcciones.Widget.Add(obAcciones);
                listaCols.Add(oColAcciones);

                #endregion


                ColumnBase oColProgress = new Column
                {
                    ID = $"colProgress",
                    Text = "Progress",
                    DataIndex = "Progress",
                    MinWidth = 100,
                    Sortable = true,
                    Align = ColumnAlign.Center,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                oColProgress.Renderer = new Renderer { Fn = "RenderProgress" };
                listaCols.Add(oColProgress);

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
            //listViews = new List<ViewDTO>();
            //List<ColumnDTO> columns = new List<ColumnDTO>() {
            //    new ColumnDTO{ Order = 0, ColumnName = "ContractStatusCode", Column = "colContractStatusCode", Visible = true },
            //    new ColumnDTO{ Order = 1, ColumnName = "SiteCode", Column = "colSiteCode", Visible = true },
            //    new ColumnDTO{ Order = 2, ColumnName = "Name", Column = "colName", Visible = true },
            //    new ColumnDTO{ Order = 3, ColumnName = "ContractTypeCode", Column = "colContractTypeCode", Visible = true },
            //    new ColumnDTO{ Order = 4, ColumnName = "StartDate", Column = "colStartDate", Visible = true },
            //};

            //ViewDTO vista = new ViewDTO
            //{
            //    Code = GetGlobalResource("strDefecto"),
            //    Default = true,
            //    Columns = columns,
            //    Filters = new List<Shared.DTO.Query.FilterDTO>(),
            //    Page = System.IO.Path.GetFileName(Request.Url.AbsolutePath),
            //    Icon = "ico-heater.svg"
            //};
            //var FinalView = viewClient.AddEntity(vista).Result;
            //listViews.Add(FinalView.Value);
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
                List<string> listaCod = new List<string>() {
                    "colCode",
                    "colDetalles",
                    "colAcciones",
                    "colName",
                    "colType",
                    "colStatus",
                    "colCustomer",
                    "colSupplier",
                    "colProgress",
                    "colStart",
                    "colDue",
                    "colProgram"
                };
                List<ColumnBase> listaColsFinal = new List<ColumnBase>();
                foreach (var oCod in listaCod)
                {
                    listaColsFinal.Add(listaCols.FirstOrDefault(x => x.ID == oCod));
                }
                gridWO.ColumnModel.Columns.AddRange(listaColsFinal);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
            return null;
        }

        #endregion
    }
}