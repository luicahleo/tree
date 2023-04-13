using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;
using TreeCore.Shared.DTO.Query;
using Newtonsoft.Json;
using TreeCore.Page;

namespace TreeCore.ModGlobal
{
    public partial class Impuestos : TreeCore.Page.BasePageExtNet
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

                //Comun.CreateGridFilters(gridFilters2, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

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

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<TaxesDTO> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        //string sModuloID = Request.QueryString["aux"].ToString();

                        #region MAESTRO

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

                        BaseAPIClient<TaxesDTO> aPIClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);

                        listaDatos = aPIClient.GetList(queryDTO).Result.Value;

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombreTask<TaxesDTO>(gridMaestro.ColumnModel, listaDatos, Response, "", GetGlobalResource("strImpuestos").ToString(), _Locale).Wait();
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        #endregion

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

        #region MAESTRO

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<TaxesDTO> ApiClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);
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
        //            string sFiltro = e.Parameters["gridFilters2"];


        //            if (!hdModuloID.Value.Equals(""))
        //            {
        //                lMaestroID = Convert.ToInt64(hdModuloID.Value);
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

        //private List<Data.ImpuestosDetalles> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        //{
        //    List<Data.ImpuestosDetalles> ListaDatos;

        //    try
        //    {
        //        ImpuestosDetallesController cImpuestosDetalles = new ImpuestosDetallesController();

        //        ListaDatos = cImpuestosDetalles.GetItemsWithExtNetFilterList<Data.ImpuestosDetalles>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ImpuestoID == " + lMaestroID);
        //    }

        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        ListaDatos = null;
        //    }

        //    return ListaDatos;
        //}


        //#endregion

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                PaisesController cPaises = new PaisesController();
                try
                {
                    List<Data.Paises> lista = cPaises.GetItemsList<Data.Paises>("", "Pais");

                    if (lista != null)
                    {
                        storePaises.DataSource = lista;
                    }
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

        #endregion

        #endregion

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<TaxesDTO> ApiClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);
            TaxesDTO oDato;

            try
            {
                if (!bAgregar)
                {
                    string oCode = GridRowSelect.SelectedRecordID;

                    oDato = ApiClient.GetByCode(oCode).Result.Value;

                    var originalCode = oDato.Code;

                    oDato.Name = txtImpuesto.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.Description = txtDescripcion.Text;
                    oDato.Valor = Convert.ToInt32(nmbValor.Value);
                    oDato.CountryCode = cmbPais.Value.ToString();

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
                    oDato = new TaxesDTO
                    {
                        Name = txtImpuesto.Text,
                        Code = txtCodigo.Text,
                        Description = txtDescripcion.Text,
                        Valor = Convert.ToInt32(nmbValor.Value),
                        CountryCode = cmbPais.Value.ToString(),
                        Active = true,
                        Default = false
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
            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                BaseAPIClient<TaxesDTO> ApiClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);
                var oDato = ApiClient.GetByCode(oCode).Result;
                txtImpuesto.Text = oDato.Value.Name;
                nmbValor.Text = oDato.Value.Valor.ToString();
                cmbPais.SetValue(oDato.Value.CountryCode);
                txtCodigo.Text = oDato.Value.Code;
                txtDescripcion.Text = oDato.Value.Description;
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
            BaseAPIClient<TaxesDTO> ApiClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);

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
            BaseAPIClient<TaxesDTO> ApiClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);
            TaxesDTO oDato;

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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<TaxesDTO> ApiClient = new BaseAPIClient<TaxesDTO>(TOKEN_API);
            TaxesDTO oDato;
            ResultDto<TaxesDTO> oResult;

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                oDato = ApiClient.GetByCode(oCode).Result.Value;
                oDato.Default = true;
                oDato.Active = true;

                var Result = ApiClient.UpdateEntity(oDato.Code, oDato).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
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

        //#region GESTION DETALLE

        //[DirectMethod()]
        //public DirectResponse mostrarDetalle(long moduloID)
        //{
        //    DirectResponse direct = new DirectResponse();
        //    direct.Result = "";
        //    direct.Success = true;

        //    try
        //    {
        //        storeDetalle.DataBind();
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

        //[DirectMethod()]
        //public DirectResponse AgregarEditarDetalle(bool bAgregar)
        //{
        //    DirectResponse ajax = new DirectResponse();

        //    ImpuestosDetallesController fController = new ImpuestosDetallesController();

        //    try
        //    {
        //        long grupoID = Int64.Parse(GridRowSelect.SelectedRecordID);

        //        if (!bAgregar)
        //        {
        //            long ID = long.Parse(GridRowSelectDetalle.SelectedRecordID);
        //            Data.ImpuestosDetalles dato;
        //            dato = fController.GetItem(ID);

        //            if (dato.Valor == Int64.Parse(txtValorDetalle.Text))
        //            {
        //                dato.Valor = Int64.Parse(txtValorDetalle.Text);
        //                dato.FechaCambio = txtFechaRevion.SelectedDate;
        //            }
        //            else
        //            {
        //                if (fController.RegistroDuplicadoDetalle(Int64.Parse(txtValorDetalle.Text), grupoID))
        //                {
        //                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
        //                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
        //                }
        //                else
        //                {
        //                    dato.Valor = Int64.Parse(txtValorDetalle.Text);
        //                    dato.FechaCambio = txtFechaRevion.SelectedDate;
        //                }
        //            }

        //            if (fController.UpdateItem(dato))
        //            {
        //                log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //                storeDetalle.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            if (fController.RegistroDuplicadoDetalle(Int64.Parse(txtValorDetalle.Text), grupoID))
        //            {
        //                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
        //                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
        //            }
        //            else
        //            {
        //                Data.ImpuestosDetalles dato = new Data.ImpuestosDetalles
        //                {
        //                    Valor = Int64.Parse(txtValorDetalle.Text),
        //                    FechaCambio = txtFechaRevion.SelectedDate,
        //                    ImpuestoID = Int64.Parse(GridRowSelect.SelectedRecordID)
        //                };
        //                if (fController.AddItem(dato) != null)
        //                {
        //                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
        //                    storeDetalle.DataBind();
        //                }
        //            }
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
        //        long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
        //        Data.ImpuestosDetalles oDato;
        //        ImpuestosDetallesController fController = new ImpuestosDetallesController();
        //        oDato = fController.GetItem(lID);
        //        txtValorDetalle.Value = oDato.Valor;
        //        txtFechaRevion.SelectedDate = oDato.FechaCambio;
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
        //    ImpuestosDetallesController cImpuestosDetalles = new ImpuestosDetallesController();

        //    direct.Result = "";
        //    direct.Success = true;

        //    long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

        //    try
        //    {
        //        if (cImpuestosDetalles.DeleteItem(lID))
        //        {
        //            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
        //            direct.Success = true;
        //            direct.Result = "";
        //            storeDetalle.DataBind();
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
        //    ImpuestosDetallesController cController = new ImpuestosDetallesController();

        //    try
        //    {
        //        long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

        //        Data.ImpuestosDetalles oDato;
        //        oDato = cController.GetItem(lID);
        //        oDato.Activo = !oDato.Activo;

        //        if (cController.UpdateItem(oDato))
        //        {
        //            storePrincipal.DataBind();
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

        //[DirectMethod()]
        //public DirectResponse AsignarPorDefectoDetalle()
        //{
        //    DirectResponse direct = new DirectResponse();
        //    ImpuestosDetallesController cImpuestos = new ImpuestosDetallesController();

        //    try
        //    {
        //        long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);
        //        Data.ImpuestosDetalles oDato;

        //        // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
        //        oDato = cImpuestos.GetDefault(lID);

        //        // SI HAY Y ES DISTINTO AL SELECCIONADO
        //        if (oDato != null)
        //        {
        //            if (oDato.ImpuestoID != lID)
        //            {
        //                if (oDato.Defecto)
        //                {
        //                    oDato.Defecto = !oDato.Defecto;
        //                    cImpuestos.UpdateItem(oDato);
        //                }

        //                oDato = cImpuestos.GetItem(lID);
        //                oDato.Defecto = true;
        //                oDato.Activo = true;
        //                cImpuestos.UpdateItem(oDato);
        //            }
        //        }
        //        // SI NO HAY ELEMENTO POR DEFECTO
        //        else
        //        {
        //            oDato = cImpuestos.GetItem(lID);
        //            oDato.Defecto = true;
        //            oDato.Activo = true;
        //            cImpuestos.UpdateItem(oDato);
        //        }

        //        log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
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

    }
}