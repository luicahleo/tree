using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.APIClient;
using TreeCore.Clases;
using TreeCore.Page;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.ModGlobal
{
    public partial class Monedas : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> ListaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> ListaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);

                List<string> ListaIgnoreDetalle = new List<string>()
                { };

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
                        List<CurrencyDTO> listaDatos = null;
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

                        BaseAPIClient<CurrencyDTO> aPIClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);

                        listaDatos = aPIClient.GetList(queryDTO).Result.Value;

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombreTask(gridMaestro.ColumnModel, listaDatos, Response, "", GetGlobalResource("strMonedas").ToString(), _Locale).Wait();
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
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CurrencyDTO> ApiClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
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

        //private List<Data.MonedasEvoluciones> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        //{
        //    List<Data.MonedasEvoluciones> listaDatos = null;
        //    MonedasEvolucionesController cMonedas = new MonedasEvolucionesController();

        //    try
        //    {
        //        listaDatos = cMonedas.GetItemsWithExtNetFilterList<Data.MonedasEvoluciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "MonedaID == " + lMaestroID.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //    }

        //    return listaDatos;
        //}


        //#endregion

        #endregion

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<CurrencyDTO> ApiClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
            CurrencyDTO oDato;

            try
            {
                if (!bAgregar)
                {
                    string oCode = GridRowSelect.SelectedRecordID;

                    oDato = ApiClient.GetByCode(oCode).Result.Value;

                    var originalCode = oDato.Code;

                    oDato.Symbol = txtSimbolo.Text;
                    oDato.Code = txtMoneda.Text;
                    oDato.DollarChange = Convert.ToSingle(txtDolar.Text);
                    oDato.EuroChange = Convert.ToSingle(txtEuro.Text);

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
                    oDato = new CurrencyDTO
                    {
                        Symbol = txtSimbolo.Text,
                        Code = txtMoneda.Text,
                        DollarChange = Convert.ToSingle(txtDolar.Text),
                        EuroChange = Convert.ToSingle(txtEuro.Text),
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
            hdEditando.SetValue("Editar");

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                BaseAPIClient<CurrencyDTO> ApiClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
                var oDato = ApiClient.GetByCode(oCode).Result;
                txtMoneda.Text = oDato.Value.Code;
                txtDolar.Text = oDato.Value.DollarChange.ToString();
                txtEuro.Text = oDato.Value.EuroChange.ToString();
                txtSimbolo.Text = oDato.Value.Symbol;
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
            BaseAPIClient<CurrencyDTO> ApiClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);

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
            BaseAPIClient<CurrencyDTO> ApiClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
            CurrencyDTO oDato;

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
            BaseAPIClient<CurrencyDTO> ApiClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
            CurrencyDTO oDato;
            ResultDto<CurrencyDTO> oResult;

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

        //#region DIRECT METHOD DETALLE

        //[DirectMethod]
        //public DirectResponse mostrarDetalle(long lMonedaID)
        //{
        //    DirectResponse ajax = new DirectResponse();

        //    try
        //    {
        //        storeDetalle.Reload();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        string codTit = Util.ExceptionHandler(ex);
        //        MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
        //    }

        //    return ajax;
        //}

        //[DirectMethod()]
        //public DirectResponse AgregarEditarDetalle(bool bAgregar)
        //{
        //    DirectResponse ajax = new DirectResponse();
        //    InfoResponse infoResponse;
        //    MonedasEvolucionesController cMonedas = new MonedasEvolucionesController();

        //    try
        //    {
        //        if (!bAgregar)
        //        {
        //            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
        //            Data.MonedasEvoluciones oDato = cMonedas.GetItem(lID);

        //            oDato.CambioDollarUS = float.Parse(txtDolarDetalle.Text.Replace('.', ','));
        //            oDato.CambioEuro = float.Parse(txtEuroDetalle.Text.Replace('.', ','));
        //            oDato.FechaCambio = DateTime.Now.Date;

        //            infoResponse = cMonedas.Update(oDato);
        //        }
        //        else
        //        {
        //            Data.MonedasEvoluciones oDato = new Data.MonedasEvoluciones()
        //            {
        //                CambioDollarUS = float.Parse(txtDolarDetalle.Text.Replace('.', ',')),
        //                CambioEuro = float.Parse(txtEuroDetalle.Text.Replace('.', ',')),
        //                FechaCambio = DateTime.Now.Date,
        //                MonedaID = Int64.Parse(GridRowSelect.SelectedRecordID)
        //            };

        //            infoResponse = cMonedas.Add(oDato);
        //        }

        //        if (infoResponse.Result)
        //        {
        //            infoResponse = cMonedas.SubmitChanges();
        //            ajax.Success = infoResponse.Result;
        //            ajax.Result = infoResponse.Description;
        //        }
        //        else
        //        {
        //            cMonedas.DiscardChanges();
        //            ajax.Success = infoResponse.Result;
        //            ajax.Result = infoResponse.Description;
        //        }

        //        storeDetalle.DataBind();
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
        //    MonedasEvolucionesController cMonedas = new MonedasEvolucionesController();

        //    try
        //    {
        //        long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
        //        Data.MonedasEvoluciones oDato = cMonedas.GetItem(lID);

        //        txtDolarDetalle.Value = oDato.CambioDollarUS;
        //        txtEuroDetalle.Value = oDato.CambioEuro;
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
        //    MonedasEvolucionesController cMonedasEvoluciones = new MonedasEvolucionesController();
        //    long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

        //    try
        //    {
        //        InfoResponse infoResponse = cMonedasEvoluciones.Delete(cMonedasEvoluciones.GetItem(lID));
        //        if (infoResponse.Result)
        //        {
        //            infoResponse = cMonedasEvoluciones.SubmitChanges();
        //            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
        //            direct.Success = infoResponse.Result;
        //            direct.Result = infoResponse.Description;
        //        }
        //        else
        //        {
        //            cMonedasEvoluciones.DiscardChanges();
        //            direct.Success = infoResponse.Result;
        //            direct.Result = infoResponse.Description;
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

        //#endregion

    }
}
