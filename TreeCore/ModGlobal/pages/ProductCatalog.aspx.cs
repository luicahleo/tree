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
    public partial class ProductCatalog : TreeCore.Page.BasePageExtNet
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

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridCatalogos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //             //#endregion

                //             #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                //             #endregion


                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
                storePrincipal.Reload();
                //}

                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            List<Data.Vw_CoreProductCatalogs> listaDatos = null;
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            long CliID = long.Parse(Request.QueryString["cliente"]);
                            bool bActivo = Request.QueryString["aux"] == "true";
                            int iCount = 0;

                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombreTemplate(gridCatalogos.ColumnModel, listaDatos, Response, "", GetGlobalResource("strCatalogos").ToString(), _Locale);
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

                }
                else if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_ProductCatalog_Lectura))
                {


                }
            }
        }

        #region STORES
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

        private List<Data.Vw_CoreProductCatalogs> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_CoreProductCatalogs> listaDatos;
            CoreProductCatalogsController cProductos = new CoreProductCatalogsController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cProductos.GetItemsWithExtNetFilterList<Data.Vw_CoreProductCatalogs>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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



        #region STORE ENTIDADES

        protected void storeEntidades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //Recupera los datos y los establece
                    var ls = ListaEntidades();
                    if (ls != null)
                    {
                        storeEntidades.DataSource = ls;
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

        private List<Data.Entidades> ListaEntidades()
        {
            List<Data.Entidades> listadatos;
            try
            {
                EntidadesController mControl = new EntidadesController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                if (lCliID != 0)
                {
                    listadatos = mControl.GetEmpresasProveedorasYOperadoresByClienteID(lCliID);
                }
                else
                {
                    listadatos = mControl.GetAllEntidades();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region STORE PRODUCT CATALOG TIPOS

        protected void storeProductCatalogTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaProductCatalogTipos();
                    if (ls != null)
                    {
                        storeProductCatalogTipos.DataSource = ls;
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

        private List<Data.CoreProductCatalogTipos> ListaProductCatalogTipos()
        {
            List<Data.CoreProductCatalogTipos> listadatos;
            try
            {
                CoreProductCatalogTiposController mControl = new CoreProductCatalogTiposController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                listadatos = mControl.GetAllCoreProductCatalogTiposByClienteID(lCliID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region STORE MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaMonedas();
                    if (ls != null)
                    {
                        storeMonedas.DataSource = ls;
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

        private List<Data.Monedas> ListaMonedas()
        {
            List<Data.Monedas> listadatos;
            try
            {
                MonedasController mControl = new MonedasController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                listadatos = mControl.GetActivos(lCliID);
                Monedas defecto = mControl.GetDefault(lCliID);
                cmbMonedas.Value = defecto.MonedaID;
                //txtPrecio.IndicatorText = defecto.Simbolo+ " /";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region STORE Servicios

        protected void storeProductCatalogServicios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaServicios();
                    if (ls != null)
                    {
                        storeProductCatalogServicios.DataSource = ls;
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

        private List<Data.Vw_CoreProductCatalogServicios> ListaServicios()
        {
            List<Data.Vw_CoreProductCatalogServicios> listadatos;
            try
            {
                CoreProductCatalogServiciosController mControl = new CoreProductCatalogServiciosController();
                CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();
                long lCliID = long.Parse(hdCliID.Value.ToString());
                string entidad = "";
                if (hdProductCatalogID.Value.ToString() != null)
                {
                    entidad = cCatalogos.getEntidad();
                }
                else
                {
                    entidad = cCatalogos.getEntidad();
                }

                entidad = "";
                listadatos = mControl.ListaServicio();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #endregion


        #region DIRECTMETHOD

        #region PRODUCTCATALOG
        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogsController cProduct = new CoreProductCatalogsController();
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.CoreProductCatalogs oDato;
                    oDato = cProduct.GetItem(lS);

                    //if (oDato.Nombre != txtNombre.Text && oDato.Codigo != txtCodigo.Text)
                    //{

                    //    oDato.EntidadID = long.Parse(cmbEntidad.Value.ToString());
                    //    oDato.CoreProductCatalogTipoID = long.Parse(cmbProductCatalogTipo.Value.ToString());
                    //    if (txtFechaFin.SelectedValue != null)
                    //    {
                    //        oDato.FechaFinVigencia = DateTime.Parse(txtFechaFin.Value.ToString());
                    //    }
                    //    oDato.MonedaID = long.Parse(cmbMonedas.Value.ToString());
                    //}
                    //else
                    //{
                    //    lCliID = long.Parse(hdCliID.Value.ToString());
                    if (oDato.Nombre != txtNombre.Text && oDato.Codigo != txtCodigo.Text && cProduct.RegistroDuplicado(txtNombre.Text, txtCodigo.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        oDato.Nombre = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.EntidadID = long.Parse(cmbEntidad.Value.ToString());
                        oDato.CoreProductCatalogTipoID = long.Parse(cmbProductCatalogTipo.Value.ToString());
                        if (txtFechaFin.SelectedValue != null)
                        {
                            oDato.FechaFinVigencia = DateTime.Parse(txtFechaFin.Value.ToString());
                        }
                        oDato.MonedaID = long.Parse(cmbMonedas.Value.ToString());
                    }
                    //}
                    CoreReajustePrecios precios = new CoreReajustePrecios();
                    ReajustesPreciosController cReajustes = new ReajustesPreciosController();
                    if (oDato.CoreReajustePrecioID != null)
                    {
                        long reajustesID = oDato.CoreReajustePrecioID.Value;
                        oDato.CoreReajustePrecioID = null;
                        cProduct.UpdateItem(oDato);
                        cReajustes.DeleteItem(reajustesID);
                    }


                    switch (cmpReajustes.Tipo)
                    {
                        case 1:
                            oDato.CoreReajustePrecioID = null;
                            break;
                        case 2:
                            precios.Tipo = int.Parse(cmpReajustes.Tipo.ToString());
                            precios.FechaProxima = cmpReajustes.FechaProxima;
                            if (cmpReajustes.ControlFechaFin)
                            {
                                precios.FechaFin = DateTime.Parse(txtFechaFin.Value.ToString());
                            }
                            else
                            {
                                precios.FechaFin = cmpReajustes.FechaFin;
                            }
                            precios.Periodicidad = cmpReajustes.Periodicidad;
                            precios.InflacionID = cmpReajustes.Inflacion;
                            precios = cReajustes.AddItem(precios);
                            oDato.CoreReajustePrecioID = precios.CoreReajustePrecioID;
                            break;
                        case 3:
                            precios.Tipo = int.Parse(cmpReajustes.Tipo.ToString());
                            precios.FechaProxima = cmpReajustes.FechaProxima;
                            if (cmpReajustes.ControlFechaFin)
                            {
                                precios.FechaFin = DateTime.Parse(txtFechaFin.Value.ToString());
                            }
                            else
                            {
                                precios.FechaFin = cmpReajustes.FechaFin;
                            }
                            precios.Periodicidad = cmpReajustes.Periodicidad;
                            precios.CantidadFija = cmpReajustes.CantidadFija;
                            precios = cReajustes.AddItem(precios);
                            oDato.CoreReajustePrecioID = precios.CoreReajustePrecioID;
                            break;
                        case 4:
                            precios.Tipo = int.Parse(cmpReajustes.Tipo.ToString());
                            precios.FechaProxima = cmpReajustes.FechaProxima;
                            if (cmpReajustes.ControlFechaFin)
                            {
                                precios.FechaFin = DateTime.Parse(txtFechaFin.Value.ToString());
                            }
                            else
                            {
                                precios.FechaFin = cmpReajustes.FechaFin;
                            }
                            precios.Periodicidad = cmpReajustes.Periodicidad;
                            precios.PorcentajeFijo = cmpReajustes.Periodicidad;
                            precios = cReajustes.AddItem(precios);
                            oDato.CoreReajustePrecioID = precios.CoreReajustePrecioID;
                            break;
                    }
                    if (cProduct.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cProduct.RegistroDuplicado(txtNombre.Text, txtCodigo.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {

                        CoreReajustePrecios precios = new CoreReajustePrecios();
                        ReajustesPreciosController cReajustes = new ReajustesPreciosController();
                        CoreProductCatalogs oDato = new CoreProductCatalogs();

                        precios.Tipo = int.Parse(cmpReajustes.Tipo.ToString());

                        switch (precios.Tipo)
                        {
                            case 1:
                                oDato.CoreReajustePrecioID = null;
                                break;
                            case 2:
                                precios.FechaInicio = cmpReajustes.FechaInicio;
                                precios.FechaProxima = cmpReajustes.FechaInicio;
                                if (cmpReajustes.ControlFechaFin)
                                {
                                    precios.FechaFin = DateTime.Parse(txtFechaFin.Value.ToString());
                                }
                                else
                                {
                                    precios.FechaFin = cmpReajustes.FechaFin;
                                }

                                precios.Periodicidad = cmpReajustes.Periodicidad;
                                precios.InflacionID = cmpReajustes.Inflacion;
                                precios = cReajustes.AddItem(precios);
                                oDato.CoreReajustePrecioID = precios.CoreReajustePrecioID;
                                break;
                            case 3:
                                precios.FechaInicio = cmpReajustes.FechaInicio;
                                precios.FechaProxima = cmpReajustes.FechaInicio;
                                if (cmpReajustes.ControlFechaFin)
                                {
                                    precios.FechaFin = DateTime.Parse(txtFechaFin.Value.ToString());
                                }
                                else
                                {
                                    precios.FechaFin = cmpReajustes.FechaFin;
                                }
                                precios.Periodicidad = cmpReajustes.Periodicidad;
                                precios.CantidadFija = cmpReajustes.CantidadFija;
                                precios = cReajustes.AddItem(precios);
                                oDato.CoreReajustePrecioID = precios.CoreReajustePrecioID;
                                break;
                            case 4:
                                precios.FechaInicio = cmpReajustes.FechaInicio;
                                precios.FechaProxima = cmpReajustes.FechaInicio;
                                if (cmpReajustes.ControlFechaFin)
                                {
                                    precios.FechaFin = DateTime.Parse(txtFechaFin.Value.ToString());
                                }
                                else
                                {
                                    precios.FechaFin = cmpReajustes.FechaFin;
                                }
                                precios.Periodicidad = cmpReajustes.Periodicidad;
                                precios.PorcentajeFijo = cmpReajustes.Periodicidad;
                                precios = cReajustes.AddItem(precios);
                                oDato.CoreReajustePrecioID = precios.CoreReajustePrecioID;
                                break;
                        }



                        oDato.Nombre = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.EntidadID = long.Parse(cmbEntidad.Value.ToString());
                        oDato.CoreProductCatalogTipoID = long.Parse(cmbProductCatalogTipo.Value.ToString());
                        oDato.FechaInicioVigencia = DateTime.Parse(txtFechaInicio.Value.ToString());
                        if (txtFechaFin.SelectedValue != null)
                        {
                            oDato.FechaFinVigencia = DateTime.Parse(txtFechaFin.Value.ToString());
                        }
                        oDato.MonedaID = long.Parse(cmbMonedas.Value.ToString());
                        oDato.ClienteID = lCliID;

                        if (cProduct.AddItem(oDato) != null)
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
            CoreProductCatalogsController cProduct = new CoreProductCatalogsController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CoreProductCatalogs oDato;
                oDato = cProduct.GetItem(lS);

                txtNombre.Text = oDato.Nombre;
                txtCodigo.Text = oDato.Codigo;
                cmbEntidad.SetValue(oDato.EntidadID);
                cmbProductCatalogTipo.SetValue(oDato.CoreProductCatalogTipoID);
                cmbMonedas.SetValue(oDato.MonedaID);
                txtFechaInicio.SetValue(oDato.FechaInicioVigencia);
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
            CoreProductCatalogsController cProduct = new CoreProductCatalogsController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cProduct.DeleteItem(lID))
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


        #endregion


        #endregion



    }


}