using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;


namespace TreeCore.ModGlobal
{
    public partial class ContactosTipos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
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
                        List<Data.ContactosTipos> listaDatos;
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

        //protected override void OnPreRenderComplete(EventArgs e)
        //{
        //    base.OnPreRenderComplete(e);
        //    if (!IsPostBack && !RequestManager.IsAjaxRequest)
        //    {
        //        List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
        //        string pagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
        //        var UserInterface = ModulesController.GetUserInterfaces().FirstOrDefault(x => x.Page.ToLower() == pagina.ToLower());
        //        var listFunPag = listFun.Where(x => $"{x.Split('@')[0]}" == UserInterface.Code);

        //        btnAnadir.Hidden = true;
        //        btnEditar.Hidden = true;
        //        btnEliminar.Hidden = true;
        //        btnRefrescar.Hidden = false;
        //        btnActivar.Hidden = true;
        //        btnDefecto.Hidden = true;
        //        btnDescargar.Hidden = true;

        //        if (listFunPag.Where(x => x.Split('@')[1] == "Read").ToList().Count > 0)
        //        {

        //        }
        //        if (listFunPag.Where(x => x.Split('@')[1] == "Download").ToList().Count > 0)
        //        {
        //            btnDescargar.Hidden = false;
        //        }
        //        if (listFunPag.Where(x => x.Split('@')[1] == "Post").ToList().Count > 0)
        //        {
        //            btnAnadir.Hidden = false;
        //        }
        //        if (listFunPag.Where(x => x.Split('@')[1] == "Put").ToList().Count > 0)
        //        {
        //            btnEditar.Hidden = false;
        //            btnActivar.Hidden = false;
        //            btnDefecto.Hidden = false;
        //        }
        //        if (listFunPag.Where(x => x.Split('@')[1] == "Delete").ToList().Count > 0)
        //        {
        //            btnEliminar.Hidden = false;
        //        }
        //    }
        //}

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

        private List<Data.ContactosTipos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.ContactosTipos> listaDatos;
            ContactosTiposController CContactosTipos = new ContactosTiposController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CContactosTipos.GetItemsWithExtNetFilterList<Data.ContactosTipos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            ContactosTiposController cContactosTipos = new ContactosTiposController();
            InfoResponse oResponse;
            long cliID = 0;

            try
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.ContactosTipos dato;
                    dato = cContactosTipos.GetItem(S);


                    dato.ContactoTipo = txtContactoTipo.Text;


                    cliID = long.Parse(hdCliID.Value.ToString());

                    oResponse = cContactosTipos.Update(dato);

                    if (oResponse.Result)
                    {
                        oResponse = cContactosTipos.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        string error = oResponse.Description;
                        oResponse = cContactosTipos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(error);
                        return direct;
                    }
                }


                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                    List<Data.ContactosTipos> listaTipo;
                    listaTipo = cContactosTipos.GetAllTipoDatos();

                    Data.ContactosTipos dato = new Data.ContactosTipos();
                    dato.ContactoTipo = txtContactoTipo.Text;
                    dato.Activo = true;

                    if (listaTipo.Count == 0)
                    {
                        dato.Defecto = true;
                    }
                    else
                    {
                        dato.Defecto = false;
                    }

                    dato.ClienteID = cliID;

                    oResponse = cContactosTipos.Add(dato);
                    if (oResponse.Result)
                    {
                        oResponse = cContactosTipos.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        string error = oResponse.Description;
                        oResponse = cContactosTipos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(error);
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

            ContactosTiposController cContactosTipos = new ContactosTiposController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ContactosTipos dato;
                dato = cContactosTipos.GetItem(S);
                txtContactoTipo.Text = dato.ContactoTipo;

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
            ContactosTiposController cContactosTipos = new ContactosTiposController();
            InfoResponse oResponse;
            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            try
            {
                Data.ContactosTipos oDato = cContactosTipos.GetItem(lID);
                oResponse = cContactosTipos.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = cContactosTipos.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }
                }
                else
                {
                    string error = oResponse.Description;
                    oResponse = cContactosTipos.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(error);
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
            ContactosTiposController cContactosTipos = new ContactosTiposController();
            InfoResponse oResponse;
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ContactosTipos oDato;
                oDato = cContactosTipos.GetItem(lID);
                oResponse = cContactosTipos.ModificarActivar(oDato);
                if (oResponse.Result)
                {
                    oResponse = cContactosTipos.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    oResponse = cContactosTipos.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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
            ContactosTiposController cContactosTipos = new ContactosTiposController();
            InfoResponse oResponse;
            Data.ContactosTipos oDato;
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                oDato = cContactosTipos.GetItem(lID);
                oResponse = cContactosTipos.SetDefecto(oDato);
                if (oResponse.Result)
                {
                    oResponse = cContactosTipos.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    oResponse = cContactosTipos.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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