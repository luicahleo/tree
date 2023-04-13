using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;


namespace TreeCore.ModGlobal
{
    public partial class FuncionalidadesTipos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

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
                else {
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
                        List<Data.FuncionalidadesTipos> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

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

                     

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

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

        private List<Data.FuncionalidadesTipos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.FuncionalidadesTipos> listaDatos;
            FuncionalidadesTiposController CFuncionalidadesTipos = new FuncionalidadesTiposController();

            try
            {
                
                    listaDatos = CFuncionalidadesTipos.GetItemsWithExtNetFilterList<Data.FuncionalidadesTipos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            FuncionalidadesTiposController cFuncionalidadesTipos = new FuncionalidadesTiposController();

            long cliID = 0;

            try
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.FuncionalidadesTipos dato;
                    dato = cFuncionalidadesTipos.GetItem(S);

                    if (dato.Nombre == txtFuncionalidadTipo.Text)
                    {
                        dato.Nombre = txtFuncionalidadTipo.Text;
                        dato.Alias = txtAlias.Text;
                        dato.Codigo = txtCodigo.Text;
                        dato.Activo = true;

                        if (chkLectura.Checked)
                        {
                            dato.Lectura = true;
                        }
                        else
                        {
                            dato.Lectura = false;
                        }

                        if (chkUsuario.Checked)
                        {
                            dato.Usuario = true;
                        }
                        else
                        {
                            dato.Usuario = false;
                        }

                        if (chkCliente.Checked)
                        {
                            dato.Cliente = true;
                        }
                        else
                        {
                            dato.Cliente = false;
                        }

                        if (chkTotal.Checked)
                        {
                            dato.Total = true;
                        }
                        else
                        {
                            dato.Total = false;
                        }

                        if (chkExportar.Checked)
                        {
                            dato.Exportar = true;
                        }
                        else
                        {
                            dato.Exportar = false;
                        }

                        if (chkGestionAdicional.Checked)
                        {
                            dato.GestionAdicional = true;
                        }
                        else
                        {
                            dato.GestionAdicional = false;
                        }

                        if (chkOtro.Checked)
                        {
                            dato.Otro = true;
                        }
                        else
                        {
                            dato.Otro = false;
                        }

                        if (chkSuper.Checked)
                        {
                            dato.Super = true;
                        }
                        else
                        {
                            dato.Super = false;
                        }
                    }
                    else
                    {
                        //cliID = long.Parse(hdCliID.Value.ToString());
                        if (cFuncionalidadesTipos.RegistroDuplicadoNombre(txtFuncionalidadTipo.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            dato.Nombre = txtFuncionalidadTipo.Text;

                            dato.Alias = txtAlias.Text;
                            dato.Codigo = txtCodigo.Text;
                            dato.Activo = true;

                            if (chkLectura.Checked)
                            {
                                dato.Lectura = true;
                            }
                            else
                            {
                                dato.Lectura = false;
                            }

                            if (chkUsuario.Checked)
                            {
                                dato.Usuario = true;
                            }
                            else
                            {
                                dato.Usuario = false;
                            }

                            if (chkCliente.Checked)
                            {
                                dato.Cliente = true;
                            }
                            else
                            {
                                dato.Cliente = false;
                            }

                            if (chkTotal.Checked)
                            {
                                dato.Total = true;
                            }
                            else
                            {
                                dato.Total = false;
                            }

                            if (chkExportar.Checked)
                            {
                                dato.Exportar = true;
                            }
                            else
                            {
                                dato.Exportar = false;
                            }

                            if (chkGestionAdicional.Checked)
                            {
                                dato.GestionAdicional = true;
                            }
                            else
                            {
                                dato.GestionAdicional = false;
                            }

                            if (chkOtro.Checked)
                            {
                                dato.Otro = true;
                            }
                            else
                            {
                                dato.Otro = false;
                            }

                            if (chkSuper.Checked)
                            {
                                dato.Super = true;
                            }
                            else
                            {
                                dato.Super = false;
                            }

                        }


                        if (cFuncionalidadesTipos.UpdateItem(dato))
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                        }
                    }

                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cFuncionalidadesTipos.RegistroDuplicadoNombre(txtFuncionalidadTipo.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.FuncionalidadesTipos dato = new Data.FuncionalidadesTipos();
                        dato.Nombre = txtFuncionalidadTipo.Text;
                        dato.Alias = txtAlias.Text;
                        dato.Codigo = txtCodigo.Text;
                        dato.Activo = true;

                        if(chkLectura.Checked)
                        {
                            dato.Lectura = true;
                        }
                        else
                        {
                            dato.Lectura = false;
                        }

                        if (chkUsuario.Checked)
                        {
                            dato.Usuario = true;
                        }
                        else
                        {
                            dato.Usuario = false;
                        }

                        if (chkCliente.Checked)
                        {
                            dato.Cliente = true;
                        }
                        else
                        {
                            dato.Cliente = false;
                        }

                        if (chkTotal.Checked)
                        {
                            dato.Total = true;
                        }
                        else
                        {
                            dato.Total = false;
                        }

                        if (chkExportar.Checked)
                        {
                            dato.Exportar = true;
                        }
                        else
                        {
                            dato.Exportar = false;
                        }

                        if (chkGestionAdicional.Checked)
                        {
                            dato.GestionAdicional = true;
                        }
                        else
                        {
                            dato.GestionAdicional = false;
                        }

                        if (chkOtro.Checked)
                        {
                            dato.Otro = true;
                        }
                        else
                        {
                            dato.Otro = false;
                        }

                        if (chkSuper.Checked)
                        {
                            dato.Super = true;
                        }
                        else
                        {
                            dato.Super = false;
                        }


                        if (cFuncionalidadesTipos.AddItem(dato) != null)
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

            FuncionalidadesTiposController cFuncionalidadesTipos = new FuncionalidadesTiposController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.FuncionalidadesTipos dato;
                dato = cFuncionalidadesTipos.GetItem(S);
                txtFuncionalidadTipo.Text = dato.Nombre;
                txtAlias.Text = dato.Alias;
                txtCodigo.Text = dato.Codigo;

                if (dato.Lectura)
                {
                    chkLectura.SetValue(true);
                }
                else
                {
                    chkLectura.SetValue(false);
                }

                if (dato.Usuario)
                {
                    chkUsuario.SetValue(true);
                }
                else
                {
                    chkUsuario.SetValue(false);
                }

                if (dato.Cliente)
                {
                    chkCliente.SetValue(true);
                }
                else
                {
                    chkCliente.SetValue(false);
                }

                if (dato.Total)
                {
                    chkTotal.SetValue(true);
                }
                else
                {
                    chkTotal.SetValue(false);
                }

                if (dato.Exportar)
                {
                    chkExportar.SetValue(true);
                }
                else
                {
                    chkExportar.SetValue(false);
                }

                if (dato.GestionAdicional)
                {
                    chkGestionAdicional.SetValue(true);
                }
                else
                {
                    chkGestionAdicional.SetValue(false);
                }

                if (dato.Otro)
                {
                    chkOtro.SetValue(true);
                }
                else
                {
                    chkOtro.SetValue(false);
                }

                if (dato.Super)
                {
                    chkSuper.SetValue(true);
                }
                else
                {
                    chkSuper.SetValue(false);
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            FuncionalidadesTiposController cFuncionalidadesTipos = new FuncionalidadesTiposController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cFuncionalidadesTipos.DeleteItem(lID))
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
            FuncionalidadesTiposController cFuncionalidadesTipos = new FuncionalidadesTiposController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.FuncionalidadesTipos oDato;
                oDato = cFuncionalidadesTipos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cFuncionalidadesTipos.UpdateItem(oDato))
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