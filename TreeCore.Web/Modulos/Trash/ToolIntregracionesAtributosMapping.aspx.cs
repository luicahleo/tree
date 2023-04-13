using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace TreeCore.ModGlobal
{
    public partial class ToolIntregracionesAtributosMapping : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        string Emplazamientos = "dbo.Emplazamientos";

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
                    cmbClientes.Hidden = true;
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
                        List<Data.ToolIntregracionesAtributosMapping> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_ToolIntregracionesAtributosMapping_Lectura))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_CONFIGURACION_MAXIMO))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
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
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.ToolIntregracionesAtributosMapping> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.ToolIntregracionesAtributosMapping> listaDatos;
            ToolIntregracionesAtributosMappingController CToolIntregracionesAtributosMapping = new ToolIntregracionesAtributosMappingController();

            try
            {
                listaDatos = CToolIntregracionesAtributosMapping.GetItemsWithExtNetFilterList<Data.ToolIntregracionesAtributosMapping>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
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
                    List<Data.Clientes> listaClientes = ListaClientes();

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

        #region STORE CAMPOS TREE

        protected void storeCamposTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Object> lista = new List<object>();

                    Object objeto = null;

                    int i = 1;

                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(Data.TreeCoreContext));
                    foreach (var mt in model.GetTables())
                    {
                        if (Emplazamientos.Equals(mt.TableName))
                        {
                            foreach (var dm in mt.RowType.DataMembers)
                            {
                                if (!dm.MappedName.ToString().Contains("ID") && !dm.MappedName.ToString().Contains("FK"))
                                {
                                    objeto = new { CampoTablaID = i.ToString(), CampoTabla = dm.MappedName.ToString() };
                                    lista.Add(objeto);
                                    i = i + 1;
                                }

                            }
                        }
                    }

                    if (lista.Count != 0)
                    {
                        storeCamposTablas.DataSource = lista;
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            ToolIntregracionesAtributosMappingController cAtributosMapping = new ToolIntregracionesAtributosMappingController();
            try

            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.ToolIntregracionesAtributosMapping oDato = cAtributosMapping.GetItem(S);



                    if (oDato.AtributoMaximo == txtCampoMaximo.Text)
                    {
                        oDato.AtributoMaximo = txtCampoMaximo.Text;
                        oDato.Tabla = txtTabla.Text;
                        oDato.AtributoTree = cmbCampoTree.SelectedItem.Text;
                    }
                    else
                    {

                        if (cAtributosMapping.RegistroDuplicado(txtCampoMaximo.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.AtributoMaximo = txtCampoMaximo.Text;
                            oDato.Tabla = txtTabla.Text;
                            oDato.AtributoTree = cmbCampoTree.SelectedItem.Text;
                        }
                    }

                    if (cAtributosMapping.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    if (cAtributosMapping.RegistroDuplicado(txtCampoMaximo.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }


                    else
                    {
                        Data.ToolIntregracionesAtributosMapping oDato = new Data.ToolIntregracionesAtributosMapping();
                        oDato.AtributoMaximo = txtCampoMaximo.Text;
                        oDato.Tabla = txtTabla.Text;
                        oDato.AtributoTree = cmbCampoTree.SelectedItem.Text;
                        oDato.Activo = true;

                        if (cAtributosMapping.AddItem(oDato) != null)
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
        public DirectResponse ComprobarDuplicidadAtributoTree(bool Agregar)
        {
            DirectResponse direct = new DirectResponse();
            ToolIntregracionesAtributosMappingController cAtributosMapping = new ToolIntregracionesAtributosMappingController();

            bool Duplicado = false;

            try
            {
                             

                if (Agregar == true)
                {
                    
                    if (cAtributosMapping.ComprobarDuplicadoAtributoTree(cmbCampoTree.SelectedItem.Text, Agregar, 0))
                    {
                        Duplicado = true;
                    }
                    else
                    {
                        Duplicado = false;
                    }
                }
                else
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.ToolIntregracionesAtributosMapping oDato = cAtributosMapping.GetItem(S);
                    long ToolIntegracionesAtributoID = oDato.ToolIntegracionAtrbitutoMappingID;

                    if (cAtributosMapping.ComprobarDuplicadoAtributoTree(cmbCampoTree.SelectedItem.Text, Agregar, ToolIntegracionesAtributoID))
                    {
                        Duplicado = true;
                    }
                    else
                    {
                        Duplicado = false;
                    }
                }

                

                direct.Success = true;
                direct.Result = Duplicado;
                return direct;
            }

            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;

            }

        }

            [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();

            ToolIntregracionesAtributosMappingController cAtributosMapping = new ToolIntregracionesAtributosMappingController();

            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ToolIntregracionesAtributosMapping oDato = cAtributosMapping.GetItem(S);
                txtCampoMaximo.Text = oDato.AtributoMaximo;
                cmbCampoTree.SetValue(oDato.AtributoTree);
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
            ToolIntregracionesAtributosMappingController CToolIntregracionesAtributosMapping = new ToolIntregracionesAtributosMappingController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {

                if (CToolIntregracionesAtributosMapping.DeleteItem(lID))
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
            ToolIntregracionesAtributosMappingController cController = new ToolIntregracionesAtributosMappingController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ToolIntregracionesAtributosMapping oDato = cController.GetItem(lID);
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