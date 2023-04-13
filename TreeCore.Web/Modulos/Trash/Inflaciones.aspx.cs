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

namespace TreeCore.PaginasComunes
{
    public partial class Inflaciones : TreeCore.Page.BasePageExtNet
    {
        // SE TIENEN QUE CREAR LAS PAGINAS: DIRECTMETHODS, PAGEFUNCTIONS Y STORES
        // DE MOMENTO ESTA TODO AQUI

        public List<long> ListaFuncionalidades = new List<long>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                ListaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                if (!ClienteID.HasValue)
                {
                    cmbClientes.Hidden = false;
                }

                #region FILTROS
                // CREAMOS LOS FILTROS PARA TODAS LAS COLUMNAS, TANTO PARA DETALLE COMO PARA MAESTRO
                List<string> ListaIgnoreList = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnoreList, _Locale);

                List<string> ListaIgnoreListDetalle = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters2, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreListDetalle, _Locale);

                #endregion

                if (ListaFuncionalidades.Contains((int)Comun.ModFun.ACCESO_TOTAL_INFLACIONES))
                {
                    btnAnadir.Enable();
                    btnDescargar.Enable();
                }

                #region REGISTRO DE ESTADISTICAS

                ProyectosTiposController cProyTip = new ProyectosTiposController();
                Data.ProyectosTipos oPtip = new Data.ProyectosTipos();
                oPtip = cProyTip.GetProyectosTiposByNombre(Comun.MODULOGLOBAL);

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
                            if (sOrden == "")
                            {
                                sOrden = "Inflacion";
                            }

                            List<Data.Vw_Inflaciones> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, ClienteID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        #endregion

                        #region DETALLE
                        else
                        {
                            List<Data.InflacionesDetalles> ListaDatosDetalle = null;

                            if (sOrden == "")
                            {
                                sOrden = "Anualidad";
                            }

                            ListaDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaDatosDetalle, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string codTit = "";
                        codTit = Util.ExceptionHandler(ex);
                        Response.Write("ERROR: " + codTit + "<br>" + Comun.ERRORAJAXGENERICO);
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

                    if (ClienteID != null)
                    {
                        lCliID = ClienteID.Value;
                    }
                    else
                    {
                        if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                        {
                            lCliID = long.Parse(hdCliID.Value.ToString());
                        }
                    }

                    if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                    {
                        lCliID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                    }

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lCliID);

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
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Vw_Inflaciones> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Inflaciones> ListaDatos = new List<Data.Vw_Inflaciones>();
            InflacionesController cInflaciones = new InflacionesController();

            try
            {
                if (ClienteID.HasValue)
                {
                    ListaDatos = cInflaciones.GetItemsWithExtNetFilterList<Data.Vw_Inflaciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    ListaDatos = cInflaciones.GetItemsWithExtNetFilterList<Data.Vw_Inflaciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                }
            }

            catch (Exception)
            {
                ListaDatos = null;
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
                    string sFiltro = e.Parameters["gridFilters2"];

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
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.InflacionesDetalles> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.InflacionesDetalles> ListaDatos = new List<Data.InflacionesDetalles>();

            try
            {
                InflacionesDetallesController cDetalles = new InflacionesDetallesController();

                ListaDatos = cDetalles.GetItemsWithExtNetFilterList<Data.InflacionesDetalles>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "InflacionID == " + lMaestroID);

                cDetalles = null;
            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return ListaDatos;
        }


        #endregion

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                PaisesController cPaises = new PaisesController();

                try
                {
                    List<Data.Paises> ListaPaises = new List<Data.Paises>();

                    ListaPaises = cPaises.GetItemsList<Data.Paises>("Activo", "Pais");

                    if (ListaPaises != null)
                    {
                        storePaises.DataSource = ListaPaises;
                    }

                    cPaises = null;
                }

                catch (Exception ex)
                {
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        #endregion

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ClientesController cClientes = new ClientesController();

                try
                {
                    List<Data.Clientes> ListaClientes = new List<Data.Clientes>();

                    ListaClientes = cClientes.GetItemsList<Data.Clientes>("", "Cliente");

                    if (ListaClientes != null)
                    {
                        storeClientes.DataSource = ListaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    }

                    cClientes = null;
                }

                catch (Exception ex)
                {
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        #endregion

        #endregion

        #region GESTION MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            InflacionesController cInflaciones = new InflacionesController();

            try
            {
                #region EDITAR
                if (!bAgregar)
                {
                    bool bEstandarAntes = false;
                    string sDescripcionAntes = "";
                    string sNombreAntes = "";
                    long lPaisAntesID = 0;
                    long lID = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.Inflaciones oDato = new Data.Inflaciones();
                    oDato = cInflaciones.GetItem(lID);

                    sNombreAntes = oDato.Inflacion;
                    lPaisAntesID = oDato.PaisID;
                    bEstandarAntes = oDato.Estandar;
                    sDescripcionAntes = oDato.Descripcion;

                    oDato.Inflacion = txtInflacion.Text;
                    oDato.PaisID = Convert.ToInt32(cmbPais.SelectedItem.Value);
                    oDato.Descripcion = txtDescripcion.Text;

                    if (ckEstandar.Checked)
                    {
                        oDato.Estandar = true;
                    }
                    else
                    {
                        oDato.Estandar = false;
                    }

                    if (!cInflaciones.hasDuplicadoInflacionEnPaisCliente(oDato.Inflacion, oDato.PaisID, oDato.ClienteID))
                    {
                        if (cInflaciones.UpdateItem(oDato))
                        {
                            storePrincipal.DataBind();
                        }
                    }
                    else
                    {
                        if (oDato.Inflacion == sNombreAntes && oDato.PaisID == lPaisAntesID)
                        {
                            if (oDato.Descripcion != sDescripcionAntes || oDato.Estandar != bEstandarAntes)
                            {
                                if (cInflaciones.UpdateItem(oDato))
                                {
                                    storePrincipal.DataBind();
                                }
                            }
                        }
                        else
                        {
                            MensajeBox(GetLocalResourceObject("jsTituloAtencion").ToString(), GetLocalResourceObject("jsTieneDuplicado").ToString(), MessageBox.Icon.WARNING, null);
                        }
                    }
                }
                #endregion

                #region AGREGAR
                else
                {
                    Data.Inflaciones oDato = new Data.Inflaciones();

                    oDato.Inflacion = txtInflacion.Text;
                    oDato.PaisID = Convert.ToInt32(cmbPais.SelectedItem.Value);
                    oDato.Descripcion = txtDescripcion.Text;

                    if (ckEstandar.Checked)
                    {
                        oDato.Estandar = true;
                    }
                    else
                    {
                        oDato.Estandar = false;
                    }

                    if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                    {
                        oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                    }

                    if (!cInflaciones.hasDuplicadoInflacionEnPaisCliente(oDato.Inflacion, oDato.PaisID, oDato.ClienteID))
                    {
                        if (cInflaciones.AddItem(oDato) != null)
                        {
                            storePrincipal.DataBind();
                        }
                    }
                    else
                    {
                        MensajeBox(GetLocalResourceObject("jsTituloAtencion").ToString(), GetLocalResourceObject("jsTieneDuplicado").ToString(), MessageBox.Icon.WARNING, null);
                    }
                }
                #endregion

                cInflaciones = null;
            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            InflacionesController cInflaciones = new InflacionesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Inflaciones oDato = new Data.Inflaciones();

                oDato = cInflaciones.GetItem(lID);
                txtInflacion.Text = oDato.Inflacion;
                cmbPais.SetValue(oDato.PaisID);
                txtDescripcion.Text = oDato.Descripcion;

                if (oDato.Estandar)
                {
                    ckEstandar.Checked = true;
                }
                else
                {
                    ckEstandar.Checked = false;
                }

                winGestion.Show();
                cInflaciones = null;
            }

            catch (Exception ex)
            { 
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            InflacionesController cInflaciones = new InflacionesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cInflaciones.tieneRegistrosAsociados(lID) == false)
                {
                    try
                    {
                        if (cInflaciones.DeleteItem(lID))
                        {
                            storePrincipal.DataBind();
                        }
                    }
                    catch (Exception)
                    {

                    }

                    cInflaciones = null;
                }

                else
                {
                    MensajeBox(GetLocalResourceObject("jsTituloAtencion").ToString(), GetLocalResourceObject("jsTieneRegistro").ToString(), MessageBox.Icon.WARNING, null);
                    direct.Result = GetLocalResourceObject("jsTieneRegistro").ToString();
                    direct.Success = false;
                    cInflaciones = null;
                }
            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                direct.Success = true;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        #region GESTION DETALLE

        
        [DirectMethod()]
        public DirectResponse mostrarDetalle(long moduloID)
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                storeDetalle.DataBind();
            }
            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse AgregarEditarDetalle(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                #region EDITAR
                if (!bAgregar)
                {
                    long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    long lIDMaestro = Int64.Parse(GridRowSelect.SelectedRecordID);

                    Data.InflacionesDetalles oDato = new Data.InflacionesDetalles();
                    InflacionesDetallesController cDetalles = new InflacionesDetallesController();

                    bool btieneAlquileres = true;
                    bool bComprobarDuplicidadAlEditar = false;

                    if (btieneAlquileres == false)
                    {
                        oDato = cDetalles.GetItem(ID);
                        oDato.FechaDesde = dateDesde.SelectedDate;
                        oDato.FechaHasta = dateHasta.SelectedDate;

                        if (txtValor.Text != "")
                        {
                            oDato.Valor = Convert.ToDouble(txtValor.Number);
                        }
                        else
                        {
                            oDato.Valor = 0;
                        }

                        if (txtMes.Text != "")
                        {
                            oDato.Mes = Convert.ToInt32(txtMes.Text);
                        }
                        else
                        {
                            oDato.Mes = null;
                        }

                        if (txtValorAnual.Text != "")
                        {
                            oDato.Anual = Convert.ToDouble(txtValorAnual.Number);
                        }
                        else
                        {
                            oDato.Anual = 0;
                        }

                        if (txtValorInteranual.Text != "")
                        {
                            oDato.Interanual = Convert.ToDouble(txtValorInteranual.Number);
                        }
                        else
                        {
                            oDato.Interanual = 0;
                        }

                        if (txtValorTrimestral.Text != "")
                        {
                            oDato.Trimestral = Convert.ToDouble(txtValorTrimestral.Number);
                        }
                        else
                        {
                            oDato.Trimestral = 0;
                        }

                        if (txtValorCuatrimestral.Text != "")
                        {
                            oDato.Cuatrimestral = Convert.ToDouble(txtValorCuatrimestral.Number);
                        }
                        else
                        {
                            oDato.Cuatrimestral = 0;
                        }

                        if (txtValorSemestral.Text != "")
                        {
                            oDato.Semestral = Convert.ToDouble(txtValorSemestral.Number);
                        }
                        else
                        {
                            oDato.Semestral = 0;
                        }

                        if (txtValorAcumulado.Text != "")
                        {
                            oDato.Acumulado = Convert.ToDouble(txtValorAcumulado.Number);
                        }
                        else
                        {
                            oDato.Acumulado = 0;
                        }

                        if (oDato.Anualidad != Convert.ToInt32(txtAnualidad.Text))
                        {
                            bComprobarDuplicidadAlEditar = true;
                        }

                        if (oDato.Mes != Convert.ToInt32(txtMes.Text))
                        {
                            bComprobarDuplicidadAlEditar = true;
                        }

                        if (oDato.FechaDesde != dateDesde.SelectedDate)
                        {
                            bComprobarDuplicidadAlEditar = true;
                        }

                        if (oDato.FechaHasta != dateHasta.SelectedDate)
                        {
                            bComprobarDuplicidadAlEditar = true;
                        }

                        //if (bComprobarDuplicidadAlEditar)
                        //{
                        //    if (!cDetalles.hasDuplicadosNuevoByFecha(dato.InflacionID, Convert.ToDateTime(dato.FechaDesde), Convert.ToDateTime(dato.FechaHasta), dato.Anualidad, dato.Mes))
                        //    {
                                if (cDetalles.UpdateItem(oDato))
                                {
                                    storeDetalle.DataBind();
                                }
                            //}
                            //else
                            //{
                            //    MensajeBox(GetLocalResourceObject("jsInfo").ToString(), GetLocalResourceObject("jsYaExiste").ToString(), MessageBox.Icon.WARNING, null);
                            //}
                        //}
                        if (cDetalles.UpdateItem(oDato))
                        {
                            storeDetalle.DataBind();
                        }
                    }
                    else
                    {
                        MensajeBox(GetLocalResourceObject("jsTituloAtencion").ToString(), GetLocalResourceObject("jsAsociadoAlquiler").ToString(), MessageBox.Icon.WARNING, null);
                    }

                    cDetalles = null;
                }
                #endregion

                #region AGREGAR
                else
                {
                    Data.InflacionesDetalles oDato = new Data.InflacionesDetalles();
                    InflacionesDetallesController cDetalles = new InflacionesDetallesController();

                    oDato.InflacionID = Convert.ToInt64(GridRowSelect.SelectedRecordID);
                    oDato.Anualidad = Convert.ToInt32(txtAnualidad.Text);
                    oDato.FechaDesde = dateDesde.SelectedDate;
                    oDato.FechaHasta = dateHasta.SelectedDate;
                    oDato.Activo = true;

                    if (txtMes.Text != "")
                    {
                        oDato.Mes = Convert.ToInt32(txtMes.Text);
                    }
                    else
                    {
                        oDato.Mes = 0;
                    }

                    if (txtValor.Text != "")
                    {
                        oDato.Valor = Convert.ToDouble(txtValor.Number);
                    }
                    else
                    {
                        oDato.Valor = 0;
                    }

                    if (txtValorAnual.Text != "")
                    {
                        oDato.Anual = Convert.ToDouble(txtValorAnual.Number);
                    }
                    else
                    {
                        oDato.Anual = 0;
                    }

                    if (txtValorInteranual.Text != "")
                    {
                        oDato.Interanual = Convert.ToDouble(txtValorInteranual.Number);
                    }
                    else
                    {
                        oDato.Interanual = 0;
                    }

                    if (txtValorTrimestral.Text != "")
                    {
                        oDato.Trimestral = Convert.ToDouble(txtValorTrimestral.Number);
                    }
                    else
                    {
                        oDato.Trimestral = 0;
                    }

                    if (txtValorCuatrimestral.Text != "")
                    {
                        oDato.Cuatrimestral = Convert.ToDouble(txtValorCuatrimestral.Number);
                    }
                    else
                    {
                        oDato.Cuatrimestral = 0;
                    }

                    if (txtValorSemestral.Text != "")
                    {
                        oDato.Semestral = Convert.ToDouble(txtValorSemestral.Number);
                    }
                    else
                    {
                        oDato.Semestral = 0;
                    }

                    if (txtValorAcumulado.Text != "")
                    {
                        oDato.Acumulado = Convert.ToDouble(txtValorAcumulado.Number);
                    }
                    else
                    {
                        oDato.Acumulado = 0;
                    }

                    if (!cDetalles.hasDuplicadosNuevoByFecha(oDato.InflacionID, Convert.ToDateTime(oDato.FechaDesde), Convert.ToDateTime(oDato.FechaHasta), oDato.Anualidad, oDato.Mes))
                    {
                        cDetalles.AddItem(oDato);
                    }
                    else
                    {
                        MensajeBox(GetLocalResourceObject("jsInfo").ToString(), GetLocalResourceObject("jsYaExiste").ToString(), MessageBox.Icon.WARNING, null);
                    }

                    storeDetalle.DataBind();
                    cDetalles = null;
                }
                #endregion

            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                Data.InflacionesDetalles oDato = new Data.InflacionesDetalles();
                InflacionesDetallesController cDetalles = new InflacionesDetallesController();

                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                oDato = cDetalles.GetItem(lID);

                txtValor.Number = oDato.Valor;
                txtAnualidad.Text = oDato.Anualidad.ToString();

                if (oDato.Mes != null)
                {
                    txtMes.Number = Convert.ToDouble(oDato.Mes);
                }

                if (oDato.FechaDesde != null)
                {
                    dateDesde.SelectedDate = oDato.FechaDesde.Value;
                }

                if (oDato.FechaHasta != null)
                {
                    dateHasta.SelectedDate = oDato.FechaHasta.Value;
                }

                if (oDato.Anual != null)
                {
                    txtValorAnual.Number = (double)oDato.Anual;
                }

                if (oDato.Interanual != null)
                {
                    txtValorInteranual.Number = (double)oDato.Interanual;
                }

                if (oDato.Trimestral != null)
                {
                    txtValorTrimestral.Number = (double)oDato.Trimestral;
                }

                if (oDato.Semestral != null)
                {
                    txtValorSemestral.Number = (double)oDato.Semestral;
                }

                if (oDato.Cuatrimestral != null)
                {
                    txtValorCuatrimestral.Number = (double)oDato.Cuatrimestral;
                }

                if (oDato.Acumulado != null)
                {
                    txtValorAcumulado.Number = (double)oDato.Acumulado;
                }

                winGestionDetalle.Show();
                cDetalles = null;
            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse EliminarDetalle()
        {
            DirectResponse ajax = new DirectResponse();
            InflacionesDetallesController cDetalles = new InflacionesDetallesController();

            ajax.Result = "";
            ajax.Success = true;

            try
            {
                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                long lIDMaestro = Int64.Parse(GridRowSelect.SelectedRecordID);

                try
                {
                    bool bTieneAlquileres = true;

                    if (bTieneAlquileres == false)
                    {
                        if (cDetalles.DeleteItem(lID))
                        {
                            storeDetalle.DataBind();
                        }
                    }
                    else
                    {
                        MensajeBox(GetLocalResourceObject("jsTituloAtencion").ToString(), GetLocalResourceObject("jsAsociadoAlquiler").ToString(), MessageBox.Icon.WARNING, null);
                    }
                }

                catch (Exception ex)
                {
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                    ajax.Success = true;
                    return ajax;
                }
            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            InflacionesDetallesController cDetalles = new InflacionesDetallesController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.InflacionesDetalles oDato = new Data.InflacionesDetalles();
                oDato = cDetalles.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cDetalles.UpdateItem(oDato))
                {
                    storeDetalle.DataBind();
                }
            }

            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            cDetalles = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

    }
}