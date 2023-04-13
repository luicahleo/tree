using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TreeCore.APIClient;
using TreeCore.Page;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.ModGlobal
{
    public partial class EntidadesV2 : BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseAPIClient<CompanyDTO> ApiClient;
        private object Traducciones;

        #region COLUMNAS

        private class colObj
        {
            public static string Text = "Text";
            public static string Date = "Date";
            public static string Bool = "Bool";

            public string Name { get; set; }
            public string Resource { get; set; }
            public string Tipo { get; set; }

            public colObj(string name, string resource)
            {
                Name = name;
                Resource = resource;
                Tipo = "Text";
            }
            public colObj(string name, string resource, string tipo)
            {
                Name = name;
                Resource = resource;
                Tipo = tipo;
            }
        }

        List<colObj> colGridObj = new List<colObj>() {
                    new colObj("Code", "strCodigo"),
                    new colObj("Name", "strNombre"),
                    new colObj("Alias", "strAlias"),
                    new colObj("Email", "strEmail"),
                    new colObj("Phone", "strTelefono"),
                    new colObj("Active", "strActivo", colObj.Bool),
                    new colObj("TaxIdentificationNumber", "strTiposNIF"),
                    new colObj("CompanyTypeCode", "strEntidadesTipos"),
                    new colObj("TaxpayerTypeCode", "strTipoContribuyente"),
                    new colObj("TaxIdentificationNumberCategoryCode", "strNumIdentificacion"),
                    new colObj("PaymentTermCode", "strCondicionPago"),
                    new colObj("CurrencyCode", "strMoneda"),
                    new colObj("CreationDate", "strFechaCreacion"),
                    new colObj("LastModificationDate", "jsUltimaModificacion"),
                    new colObj("CreationUserEmail", "strUsuarioCreador"),
                    new colObj("LastModificationUserEmail", "strUsuarioUltimaModificacion")
                };

        #endregion

        List<ColumnBase> listaCols;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            ApiClient = new BaseAPIClient<CompanyDTO>(TOKEN_API);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                Traducciones = new
                {
                    Code = GetGlobalResource("strCodigo"),
                    Name = GetGlobalResource("strNombre"),
                    Alias = GetGlobalResource("strAlias"),
                    Email = GetGlobalResource("strEmail"),
                    Phone = GetGlobalResource("strTelefono"),
                    CompanyTypeCode = GetGlobalResource("strTipo"),
                    CurrencyCode = GetGlobalResource("strMoneda"),
                    Active = GetGlobalResource("strActivo"),
                    Owner = GetGlobalResource("strPropietario"),
                    Supplier = GetGlobalResource("strProveedores"),
                    Customer = GetGlobalResource("strEsCliente"),
                    Payee = GetGlobalResource("strBeneficiario"),
                    TaxIdentificationNumber = GetGlobalResource("strNumIdentificacion"),
                    TaxpayerTypeCode = GetGlobalResource("strContribuyentes"),
                    TaxIdentificationNumberCategoryCode = GetGlobalResource("strTiposNIF"),
                    PaymentTermCode = GetGlobalResource("strCondicionPago"),
                    CreationDate = GetGlobalResource("strFechaCreacion"),
                    LastModificationDate = GetGlobalResource("jsUltimaModificacion"),
                    CreationUserEmail = GetGlobalResource("strUsuarioCreador"),
                    LastModificationUserEmail = GetGlobalResource("strUsuarioUltimaModificacion"),
                    BankCode = GetGlobalResource("strBanco"),
                    IBAN = GetGlobalResource("strIBAN"),
                    Description = GetGlobalResource("strDescripcion"),
                    SWIFT = GetGlobalResource("strSWIFT"),
                    PaymentMethodCode = GetGlobalResource("strMetodoPago"),
                    Default = GetGlobalResource("strDefecto"),
                    Address = GetGlobalResource("strDireccion"),
                    Address1 = GetGlobalResource("strDireccion") + 1,
                    Address2 = GetGlobalResource("strDireccion") + 2,
                    PostalCode = GetGlobalResource("strCodigoPostal"),
                    Locality = GetGlobalResource("strLocality"),
                    Sublocality = GetGlobalResource("strSublocality"),
                    Country = GetGlobalResource("strPais")
                };

                hdTraducciones.Value = JsonConvert.SerializeObject(Traducciones);

                List<string> listaIgnore = new List<string>()
                { };

                //#region FILTROS

                Comun.CreateGridFilters(gridFilters, storeCompany, gridCompany.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //#endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridCompany, storeCompany, gridCompany.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                GenerarColumnas();
                GenerarGrid("");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> {  }},
                { "Post", new List<ComponentBase> {  }},
                { "Put", new List<ComponentBase> { }},
                { "Delete", new List<ComponentBase> {  }}
            };
        }


        #endregion

        #region STORES

        protected void storeCompany_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    int curPage = e.Page;
                    int pageSize = Convert.ToInt32(e.Limit);
                    var sSort = e.Sort;
                    bool bActivo = btnAllCompanies.Pressed;
                    bool bOwner = btnOwner.Pressed;
                    bool bSupplier = btnSupplier.Pressed;
                    bool bCustomer = btnCustomer.Pressed;
                    bool bPayee = btnPayee.Pressed;

                    QueryDTO query = new QueryDTO(sSort.First().Property, sSort.First().Direction.ToString(), pageSize, curPage);
                    if (!cmbFiltroEntidadesTipos.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.CompanyTypeCode), nameof(Operators.eq), cmbFiltroEntidadesTipos.Value.ToString(), null));
                    if (!bActivo)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.Active), nameof(Operators.ineq), btnAllCompanies.Pressed.ToString(), null));
                    if (bOwner)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.Owner), nameof(Operators.eq), btnOwner.Pressed.ToString(), null));
                    if (bSupplier)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.Supplier), nameof(Operators.eq), btnSupplier.Pressed.ToString(), null));
                    if (bCustomer)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.Customer), nameof(Operators.eq), btnCustomer.Pressed.ToString(), null));
                    if (bPayee)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.Payee), nameof(Operators.eq), btnPayee.Pressed.ToString(), null));
                    if (!txtFiltroCompany.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(CompanyDTO.Code), nameof(Operators.like), $"%{txtFiltroCompany.Value.ToString()}%", null));
                    foreach (var oFilter in e.Filter)
                    {
                        query.AddFilter(new FilterDTO(oFilter.Property, nameof(Operators.like), $"%{oFilter.Value}%", null));
                    }
                    var lista = ApiClient.GetList(query).Result;
                    if (lista != null)
                    {
                        e.Total = lista.TotalItems;
                        storeCompany.DataSource = lista.Value;
                        storeCompany.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #region TIPOS ENTIDAD

        protected void storeTiposEntidad_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CompanyTypeDTO> ApiClient = new BaseAPIClient<CompanyTypeDTO>(TOKEN_API);
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse DeleteCompany(string code)
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

        [DirectMethod()]
        public DirectResponse ActivarCompany(string code)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            CompanyDTO oDato;

            try
            {
                oDato = ApiClient.GetByCode(code).Result.Value;
                oDato.Active = !oDato.Active;

                var Result = ApiClient.UpdateEntity(code, oDato).Result;

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

                obEditar.Listeners.Click.Fn = "EditarCompany";
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

                obDetalles.Listeners.Click.Fn = "DetalleCompany";
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
                    DataIndex = "Acciones",
                    Hidden = true
                };

                Ext.Net.Button obAcciones = new Ext.Net.Button
                {
                    PressedCls = "Pressed-none",
                    FocusCls = "Focus-none",
                    Cls = "btnColumna Hidden btnMoreOptions"
                };

                obAcciones.Listeners.Click.Fn = "AccionesCompany";
                oColAcciones.Widget.Add(obAcciones);
                listaCols.Add(oColAcciones);

                ColumnBase oColActive = new Column
                {
                    ID = "colActive",
                    Align = ColumnAlign.Start,
                    Sortable = true,
                    MinWidth = 55,
                    MaxWidth = 55,
                    Flex = 1,
                    Cls = "col-activo",
                    DataIndex = "Active",
                    Hidden = true
                };
                oColActive.CustomConfig.Add(new ConfigItem("Tipo", colObj.Bool));
                oColActive.CustomConfig.Add(new ConfigItem("Indice", oColActive.DataIndex));
                oColActive.Renderer = new Renderer { Fn = "RenderColDinamica" };
                listaCols.Add(oColActive);

                ColumnBase oColRole = new Column
                {
                    ID = "colRol",
                    Text = GetGlobalResource("strOperationalRole"),
                    Align = ColumnAlign.Center,
                    MinWidth = 150,
                    Sortable = true,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                oColRole.Renderer = new Renderer { Fn = "RendersRole" };
                listaCols.Add(oColRole);

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


        [DirectMethod(Timeout = 120000)]
        public DirectResponse GenerarGrid(string JsonColumnas)
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
                    "colEditar",
                    "colDetalles",
                    "colAcciones",
                    "colActive",
                    "colName",
                    "colRol",
                    "colCompanyTypeCode",
                    "colTaxIdentificationNumber",
                    "colPhone",
                    "colEmail"
                };
                List<ColumnBase> listaColsFinal = new List<ColumnBase>();
                foreach (var oCod in listaCod)
                {
                    listaColsFinal.Add(listaCols.FirstOrDefault(x => x.ID == oCod));
                }
                gridCompany.ColumnModel.Columns.AddRange(listaColsFinal);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storeCompany, gridCompany.ColumnModel, listaIgnore, _Locale);
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