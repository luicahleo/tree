using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Reflection;

namespace TreeCore.ModGlobal
{
    public partial class Calidad : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region GESTIÓN DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
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
                        List<Data.Vw_Calidad> listaDatos;
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_CALIDAD))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_CALIDAD))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;
            }
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
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Vw_Calidad> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Calidad> listaDatos;
            CalidadController cCalidad = new CalidadController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_Calidad>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
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

        #region COLUMNAS
        protected void storeColumnas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Object> lista = new List<object>();
                Object objeto;
                string sNombreTabla;
                int i = 1;

                try
                {
                    sNombreTabla = Comun.VISTA_GENERAL_MODULO_GLOBAL;
                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(Data.TreeCoreContext));
                    foreach (var mt in model.GetTables())
                    {
                        if (sNombreTabla.Equals(mt.TableName))
                        {
                            foreach (var dm in mt.RowType.DataMembers)
                            {

                                objeto = new { ColumnaTablaID = i.ToString(), ColumnaNombre = dm.MappedName.ToString() };
                                lista.Add(objeto);
                                i = i + 1;
                            }

                        }
                    }

                    storeColumnas.DataSource = lista;

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

        #region TIPO DATOS        
        protected void storeTipoDato_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                TiposDatosController cTipoDato = new TiposDatosController();

                try
                {
                    var ls = cTipoDato.GetAllTipoDatos();
                    if (ls != null)
                        storeTipoDato.DataSource = ls;

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

        #region OPERADRES
        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<object> ls = new List<object>();
                    if (hdOperadores.Value != null)
                    {
                        string[] Operador = hdOperadores.Value.ToString().Split('-');
                        foreach (string items in Operador)
                        {
                            string[] OperadorValores = items.Split(';');
                            ls.Add(new { valor = Convert.ToInt32(OperadorValores[1]), nombre = OperadorValores[0] });
                        }

                        storeOperador.DataSource = ls;

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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            CalidadController cCalidad = new CalidadController();

            long cliID = 0;

            try
            {
                if (!agregar)
                {
                    long lIdSelect = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.Calidad oDato;
                    oDato = cCalidad.GetItem(lIdSelect);

                    if (oDato.NombreCampo == cmbColumnas.SelectedItem.Text)
                    {
                        oDato.NombreCampo = cmbColumnas.SelectedItem.Text;
                        oDato.Operador = cmbOperador.SelectedItem.Text;
                        oDato.Descripcion = txtDescripcion.Text;
                        oDato.TipoDatoID = Convert.ToInt32(cmbTipoDato.SelectedItem.Value);
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cCalidad.RegistroDuplicado(txtDescripcion.Text, cmbOperador.SelectedItem.Text, Convert.ToInt32(cmbTipoDato.SelectedItem.Value), cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NombreCampo = cmbColumnas.SelectedItem.Text;
                            oDato.Operador = cmbOperador.SelectedItem.Text;
                            oDato.Descripcion = txtDescripcion.Text;
                            oDato.TipoDatoID = Convert.ToInt32(cmbTipoDato.SelectedItem.Value);
                        }
                    }

                    TiposDatosController cTipos = new TiposDatosController();
                    Data.TiposDatos tipoDato = cTipos.GetItem(oDato.TipoDatoID);


                    switch (tipoDato.TipoDato)
                    {
                        case Comun.TiposDatos.Texto:
                            oDato.Valor = txtValor.Text;
                            oDato.FechaCondicion = null;
                            break;
                        case Comun.TiposDatos.Numerico:
                        case Comun.TiposDatos.Entero:
                        case Comun.TiposDatos.GeoPosicion:
                        case Comun.TiposDatos.Moneda:
                        case Comun.TiposDatos.Decimal:
                            oDato.Valor = numValor.Text;
                            oDato.FechaCondicion = null;
                            break;
                        case Comun.TiposDatos.Booleano:
                            oDato.Valor = radSi.Checked.ToString().ToLower();
                            oDato.FechaCondicion = null;
                            break;
                        case Comun.TiposDatos.Fecha:
                            oDato.Valor = "";
                            oDato.FechaCondicion = dateFecha.SelectedDate;
                            break;
                    }

                    if (cCalidad.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cCalidad.RegistroDuplicado(txtDescripcion.Text, cmbOperador.SelectedItem.Text, Convert.ToInt32(cmbTipoDato.SelectedItem.Value), cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Calidad oDato = new Data.Calidad
                        {
                            Activa = true,
                            NombreCampo = cmbColumnas.SelectedItem.Text,
                            Operador = cmbOperador.SelectedItem.Text,
                            Descripcion = txtDescripcion.Text,
                            Activo = true,
                            ModuloID = Convert.ToInt32(Comun.Modulos.GLOBAL),
                            TipoDatoID = Convert.ToInt32(cmbTipoDato.SelectedItem.Value)
                        };
                        TiposDatosController cTipos = new TiposDatosController();
                        Data.TiposDatos oTipoDato = cTipos.GetItem(oDato.TipoDatoID);

                        switch (oTipoDato.TipoDato)
                        {
                            case Comun.TiposDatos.Texto:
                                oDato.Valor = txtValor.Text;
                                break;
                            case Comun.TiposDatos.Numerico:
                            case Comun.TiposDatos.Entero:
                            case Comun.TiposDatos.GeoPosicion:
                            case Comun.TiposDatos.Moneda:
                            case Comun.TiposDatos.Decimal:
                                oDato.Valor = numValor.Text;
                                break;
                            case Comun.TiposDatos.Booleano:
                                oDato.Valor = radSi.Checked.ToString().ToLower();
                                break;
                            case Comun.TiposDatos.Fecha:
                                oDato.Valor = "";
                                oDato.FechaCondicion = dateFecha.SelectedDate;
                                break;
                        }

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = cliID;
                        }

                        if (cCalidad.AddItem(oDato) != null)
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

            CalidadController cCalidad = new CalidadController();

            try

            {
                long lIdSelect = Convert.ToInt64(GridRowSelect.SelectedRecordID);

                Data.Calidad dato = default(Data.Calidad);
                dato = cCalidad.GetItem(lIdSelect);

                txtValor.Text = dato.Valor;
                txtDescripcion.Text = dato.Descripcion;
                cmbTipoDato.SetValue(dato.TipoDatoID.ToString());
                cmbTipoDato.SetValueAndFireSelect(dato.TipoDatoID);

                cmbOperador.SetValue(dato.Operador);
                cmbColumnas.SetValue(dato.NombreCampo);

                TiposDatosController cTipos = new TiposDatosController();
                Data.TiposDatos tipo = cTipos.GetItem(dato.TipoDatoID);

                switch (tipo.TipoDato)
                {
                    case Comun.TiposDatos.Texto:
                        txtValor.Text = dato.Valor;
                        break;

                    case Comun.TiposDatos.Numerico:
                        numValor.Text = dato.Valor;
                        break;
                    case Comun.TiposDatos.Entero:
                        numValor.Text = dato.Valor;
                        break;
                    case Comun.TiposDatos.GeoPosicion:
                        numValor.Text = dato.Valor;
                        break;
                    case Comun.TiposDatos.Moneda:
                        numValor.Text = dato.Valor;
                        break;
                    case Comun.TiposDatos.Decimal:
                        numValor.Text = dato.Valor;
                        break;
                    case Comun.TiposDatos.Booleano:
                        if (dato.Valor == Comun.TiposDatos.BoolValues.True)
                        {
                            radSi.Checked = true;
                            radNo.Checked = false;
                        }
                        else
                        {
                            radNo.Checked = true;
                            radSi.Checked = false;
                        }
                        break;

                    case Comun.TiposDatos.Fecha:
                        dateFecha.SelectedDate = dato.FechaCondicion.Value;
                        break;
                }

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
            CalidadController CCalidad = new CalidadController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Calidad oDato;
                long lCliID = 0;

                if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                {
                    lCliID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                }
                else if (ClienteID.HasValue)
                {
                    lCliID = Convert.ToInt32(ClienteID);
                }

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = CCalidad.GetDefault(lCliID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.CalidadID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            CCalidad.UpdateItem(oDato);
                        }

                        oDato = CCalidad.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        CCalidad.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = CCalidad.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    CCalidad.UpdateItem(oDato);
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
            CalidadController CCalidad = new CalidadController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CCalidad.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (CCalidad.DeleteItem(lID))
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
            CalidadController cController = new CalidadController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Calidad oDato;
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

        #endregion


    }
}