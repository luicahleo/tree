using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class Regiones : TreeCore.Page.BasePageExtNet
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
                        List<Data.RegionesPaises> listaDatos = new List<Data.RegionesPaises>();
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        string[] param = Request.QueryString["aux"].ToString().Split(';');
                        long ProID = 0;
                        if (param[0] != "")
                            ProID = long.Parse(param[0]);
                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID, ProID);

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
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto, btnRadio }},
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

                    long paisID = 0;




                    if (cmbPais.SelectedItem.Value != null && cmbPais.SelectedItem.Value != "")
                    {
                        paisID = Convert.ToInt32(cmbPais.SelectedItem.Value);
                    }
                    else
                    {
                        paisID = 0;

                    }

                    var ls = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()), paisID);
                    if (ls != null)
                    {
                        storePrincipal.DataSource = ls;
                        PageProxy temp;
                        temp = (PageProxy)storePrincipal.Proxy[0];
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

        private List<Data.RegionesPaises> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long ClienteID, long paisID)
        {
            List<Data.RegionesPaises> listaDatos;
            RegionesPaisesController CRegionesPaises = new RegionesPaisesController();

            try
            {
                if (ClienteID != 0)
                {
                    if (paisID != 0)
                    {
                        listaDatos = CRegionesPaises.GetItemsWithExtNetFilterList<Data.RegionesPaises>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "PaisID ==" + paisID);
                    }
                    else
                    {
                        listaDatos = null;
                    }
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
                    List<Data.Clientes> listaClientes = new List<Data.Clientes>();

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
            List<Data.Clientes> listaDatos = new List<Data.Clientes>();
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

        #region PAISES
        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaPaises();
                    if (ls != null)
                        storePaises.DataSource = ls;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }

            }
        }

        private List<Data.Paises> ListaPaises()
        {
            List<Data.Paises> listaDatos;
            PaisesController cPaises = new PaisesController();
            try
            {
                listaDatos = cPaises.GetActivos(long.Parse(hdCliID.Value.ToString()));
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
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

            RegionesPaisesController cRegionesPaises = new RegionesPaisesController();

            try

            {
                long cliID = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                    Data.RegionesPaises oDato = cRegionesPaises.GetItem(S);

                    if (oDato.RegionPais == txtRegionPais.Text && oDato.Codigo == txtRegionPaisCodigo.Text)
                    {
                        oDato.RegionPais = txtRegionPais.Text;
                        oDato.Codigo = txtRegionPaisCodigo.Text;
                    }
                    else
                    {
                        if (cRegionesPaises.RegistroDuplicado(txtRegionPais.Text, cliID, long.Parse(cmbPais.Value.ToString())))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.RegionPais = txtRegionPais.Text;
                            oDato.Codigo = txtRegionPaisCodigo.Text;

                        }
                    }

                    if (cRegionesPaises.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    Data.RegionesPaises oDato = new Data.RegionesPaises();
                    List<RegionesPaises> lista = cRegionesPaises.GetListRegionPaisActivoByPaisID(long.Parse(cmbPais.Value.ToString()), cliID);
                    oDato.RegionPais = txtRegionPais.Text;
                    oDato.Codigo = txtRegionPaisCodigo.Text;
                    oDato.Activo = true;
                    if (lista.Count == 0)
                    {
                        oDato.Defecto = true;
                    }
                    else
                    {
                        oDato.Defecto = false;
                    }

                    oDato.PaisID = Convert.ToInt32(cmbPais.SelectedItem.Value);

                    if (cRegionesPaises.RegistroDuplicado(txtRegionPais.Text, cliID, long.Parse(cmbPais.Value.ToString())))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        return direct;
                    }
                    else
                    {
                        cRegionesPaises.AddItem(oDato);
                    }

                }

                storePrincipal.DataBind();
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
            DirectResponse result = new DirectResponse();

            RegionesPaisesController cMunicipalidad = new RegionesPaisesController();

            try

            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.RegionesPaises oDato = cMunicipalidad.GetItem(S);

                txtRegionPais.Text = oDato.RegionPais;
                txtRegionPaisCodigo.Text = oDato.Codigo;
                winGestion.Show();

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return result;
            }

            result.Success = true;
            result.Result = "";

            return result;
        }

        [DirectMethod()]
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            RegionesPaisesController CRegionesPaises = new RegionesPaisesController();
            ProvinciasController cPronvincia = new ProvinciasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.RegionesPaises oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = CRegionesPaises.GetDefault(long.Parse(cmbPais.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.PaisID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cPronvincia.EliminarDefecto(oDato.RegionPaisID);
                            CRegionesPaises.UpdateItem(oDato);
                        }

                        oDato = CRegionesPaises.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        CRegionesPaises.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = CRegionesPaises.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    CRegionesPaises.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

                CRegionesPaises = null;
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
            RegionesPaisesController CRegionesPaises = new RegionesPaisesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CRegionesPaises.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }

                else if (CRegionesPaises.DeleteItem(lID))
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
            RegionesPaisesController cController = new RegionesPaisesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.RegionesPaises oDato = cController.GetItem(lID);
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

            cController = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion


        #region Radio
        [DirectMethod()]
        public DirectResponse AsignarRadio()
        {
            DirectResponse direct = new DirectResponse();
            RegionesPaisesController cPaises = new RegionesPaisesController();
            try
            {
                if (GridRowSelect.SelectedRows.Count > 0)
                {
                    foreach (SelectedRow selec in GridRowSelect.SelectedRows)
                    {
                        Data.RegionesPaises oDato = cPaises.GetItem(long.Parse(selec.RecordID));
                        if (oDato != null)
                        {
                            oDato.Radio = numRadio.Value != null ? (double?)numRadio.Value : null;
                            cPaises.UpdateItem(oDato);
                        }
                    }
                    cPaises = null;
                }
                else
                {
                    long paisID = Convert.ToInt64(cmbPais.SelectedItem.Value);
                    List<Data.RegionesPaises> Municipalidades = cPaises.getAllRegionPaisByPaisID(paisID);

                    foreach (Data.RegionesPaises oDato in Municipalidades)
                    {
                        using (RegionesPaisesController controller = new RegionesPaisesController())
                        {
                            Data.RegionesPaises aux = controller.GetItem(oDato.RegionPaisID);
                            aux.Radio = numRadio.Value != null ? (double?)numRadio.Value : null; 
                            controller.UpdateItem(aux);
                        }
                    }
                }

                storePrincipal.DataBind();

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
        public DirectResponse MostrarEditarRadio()
        {
            DirectResponse result = new DirectResponse();
            RegionesPaisesController cMunicipalidad = new RegionesPaisesController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.RegionesPaises oDato = new Data.RegionesPaises();
                oDato = cMunicipalidad.GetItem(S);

                if (oDato != null && oDato.Radio != null)
                {
                    numRadio.Number = (double)oDato.Radio;
                }

                winRadio.Show();
                cMunicipalidad = null;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return result;
            }

            result.Success = true;
            result.Result = "";
            return result;
        }

        #endregion
    }
}