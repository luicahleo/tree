using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Transactions;
using System.Linq;


namespace TreeCore.ModInventario
{
    public partial class InventarioCategoriasVinculaciones : TreeCore.Page.BasePageExtNet
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
                else
                {
                    hdCliID.Value = ClienteID;
                }
                hdCategoriaPadre.SetValue(0);
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.InventarioAtributosCategorias> listaDatos;
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
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
            sPagina = "InventarioCategoriasContenedor.aspx";
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

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
                List<Data.Vw_InventarioCategoriasVinculaciones> listaDatos;
                List<JsonObject> listaJson;
                JsonObject oJson;
                try
                {
                    if (cmbTipoEmplazamientos.SelectedItem.Text != null && cmbTipoEmplazamientos.SelectedItem.Text != "")
                    {
                        if (long.Parse(hdCategoriaPadre.Value.ToString()) != 0)
                        {
                            listaDatos = cVinculaciones.GetVwVinculacionesFromCategoriaPadre(long.Parse(hdCategoriaPadre.Value.ToString()), long.Parse(cmbTipoEmplazamientos.SelectedItem.Value), btnActivos.Pressed);
                        }
                        else
                        {
                            listaDatos = cVinculaciones.GetVwVinculacionesFromCategoriaPadre(null, long.Parse(cmbTipoEmplazamientos.SelectedItem.Value), btnActivos.Pressed);
                        }
                    }
                    else
                    {
                        if (long.Parse(hdCategoriaPadre.Value.ToString()) != 0)
                        {
                            listaDatos = cVinculaciones.GetVwVinculacionesFromCategoriaPadre(long.Parse(hdCategoriaPadre.Value.ToString()), null, btnActivos.Pressed);
                        }
                        else
                        {
                            listaDatos = cVinculaciones.GetVwVinculacionesFromCategoriaPadre(null, null, btnActivos.Pressed);
                        }
                    }
                    if (listaDatos != null)
                    {
                        listaJson = new List<JsonObject>();
                        foreach (var item in listaDatos)
                        {
                            oJson = new JsonObject();
                            oJson.Add("InventarioCategoriaVinculacionID", item.InventarioCategoriaVinculacionID);
                            oJson.Add("InventarioCategoriaID", item.InventarioCategoriaID);
                            oJson.Add("InventarioCategoria", item.InventarioCategoria);
                            oJson.Add("InventarioCategoriaPadre", item.InventarioCategoriaPadre);
                            oJson.Add("Tipo", (item.Tipo == null || item.Tipo == "") ? GetGlobalResource("strComun") : item.Tipo);
                            oJson.Add("Activo", item.Activo);
                            oJson.Add("Icono", Comun.rutaIconoWebInventario(item.Icono));
                            oJson.Add("Padres", GetPadresCatHija(item.InventarioCategoriaID).Result);
                            oJson.Add("Vinculaciones", GetVinculaciones(item.InventarioCategoriaVinculacionID).Result);
                            switch (int.Parse(item.TipoRelacion))
                            {
                                case (int)Comun.TiposVinculaciones.Rel_1_1:
                                    oJson.Add("TipoVinculacion", "1-1");
                                    break;
                                case (int)Comun.TiposVinculaciones.Rel_1_N:
                                    oJson.Add("TipoVinculacion", "1-N");
                                    break;
                                case (int)Comun.TiposVinculaciones.Rel_N_1:
                                    oJson.Add("TipoVinculacion", "N-1");
                                    break;
                                case (int)Comun.TiposVinculaciones.Rel_N_M:
                                    oJson.Add("TipoVinculacion", "N-M");
                                    break;
                                default:
                                    break;
                            }
                            listaJson.Add(oJson);
                        }
                        storePrincipal.DataSource = listaJson;
                        storePrincipal.DataBind();
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

        private List<Data.InventarioAtributosCategorias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.InventarioAtributosCategorias> listaDatos;
            InventarioAtributosCategoriasController cInventarioAtributosCategorias = new InventarioAtributosCategoriasController();

            try
            {
                if (lClienteID != null)
                {
                    listaDatos = cInventarioAtributosCategorias.GetItemsWithExtNetFilterList<Data.InventarioAtributosCategorias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region EMPLAZAMIENTOS TIPOS

        protected void storeTipoEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
                try
                {

                    var lista = cEmplazamientosTipos.GetEmplazamientosTiposActivos(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeTipoEmplazamientos.DataSource = lista;
                        storeTipoEmplazamientos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region VINCULACIONES TIPOS

        protected void storeTipoVinculacioness_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioTiposVinculacionesController cVinculaciones = new InventarioTiposVinculacionesController();
                try
                {

                    var lista = cVinculaciones.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeTipoVinculaciones.DataSource = lista;
                        storeTipoVinculaciones.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        [DirectMethod()]
        public DirectResponse GetVinculaciones(long lVinID)
        {
            DirectResponse direct = new DirectResponse();
            JsonObject listaPadres = new JsonObject();
            JsonObject aux;

            InventarioTiposVinculacionesController cVinculaciones = new InventarioTiposVinculacionesController();
            List<Data.InventarioTiposVinculaciones> listaVin;

            try
            {
                listaVin = cVinculaciones.GetTiposFromVinculacion(lVinID);
                foreach (var item in listaVin)
                {
                    aux = new JsonObject();
                    aux.Add("InventarioTipoVinculacionID", item.InventarioTipoVinculacionID);
                    aux.Add("Nombre", item.Nombre);
                    listaPadres.Add(item.InventarioTipoVinculacionID.ToString(), aux);
                }

                direct.Success = true;
                direct.Result = listaPadres.ToJsonString();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            return direct;
        }

        #endregion

        #region CATEGORIAS

        #region ALL CATEGORIAS

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                List<Data.InventarioCategorias> listaDatos;
                try
                {
                    listaDatos = cCategorias.GetInventarioCategoriasComunesYPorEmplazamiento(cmbTipoEmplazamientos.SelectedItem.Value, long.Parse(hdCliID.Value.ToString()));
                    if (listaDatos != null)
                    {
                        storeCategorias.DataSource = listaDatos;
                        storeCategorias.DataBind();
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

        #region CATEGORIAS FORM

        protected void storeCategoriasForm_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                List<Data.InventarioCategorias> listaDatos;
                try
                {
                    listaDatos = cCategorias.GetInventarioCategoriasNoAsociadasComunesYPorEmplazamiento(cmbTipoEmplazamientos.SelectedItem.Value, long.Parse(hdCliID.Value.ToString()), long.Parse(hdCategoriaPadre.Value.ToString()));
                    if (listaDatos != null)
                    {
                        storeCategoriasForm.DataSource = listaDatos;
                        storeCategoriasForm.DataBind();
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

        #region CATEGORIAS PADRES

        [DirectMethod()]
        public DirectResponse GetPadres()
        {
            DirectResponse direct = new DirectResponse();
            JsonObject listaPadres = new JsonObject();
            JsonObject aux;

            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            List<Data.InventarioCategorias> listaVin;

            try
            {
                if (cmbTipoEmplazamientos.SelectedItem.Text != null && cmbTipoEmplazamientos.SelectedItem.Text != "")
                {
                    listaVin = cCategorias.GetCategoriasPadres(long.Parse(hdCategoriaPadre.Value.ToString()), long.Parse(cmbTipoEmplazamientos.Value.ToString()));
                }
                else
                {
                    listaVin = cCategorias.GetCategoriasPadres(long.Parse(hdCategoriaPadre.Value.ToString()), 0);
                }
                foreach (var item in listaVin)
                {
                    aux = new JsonObject();
                    aux.Add("InventarioCategoriaID", item.InventarioCategoriaID);
                    aux.Add("InventarioCategoria", item.InventarioCategoria);
                    listaPadres.Add(item.InventarioCategoriaID.ToString(), aux);
                }

                direct.Success = true;
                direct.Result = listaPadres.ToJsonString();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse GetPadresCatHija(long lCatID)
        {
            DirectResponse direct = new DirectResponse();
            JsonObject listaPadres = new JsonObject();
            JsonObject aux;

            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            List<Data.InventarioCategorias> listaVin;

            try
            {
                if (cmbTipoEmplazamientos.SelectedItem.Text != null && cmbTipoEmplazamientos.SelectedItem.Text != "")
                {
                    listaVin = cCategorias.GetCategoriasPadres(lCatID, long.Parse(cmbTipoEmplazamientos.Value.ToString()));
                }
                else
                {
                    listaVin = cCategorias.GetCategoriasPadres(lCatID, 0);
                }
                foreach (var item in listaVin)
                {
                    aux = new JsonObject();
                    aux.Add("InventarioCategoriaID", item.InventarioCategoriaID);
                    aux.Add("InventarioCategoria", item.InventarioCategoria);
                    listaPadres.Add(item.InventarioCategoriaID.ToString(), aux);
                }

                direct.Success = true;
                direct.Result = listaPadres.ToJsonString();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            return direct;
        }

        #endregion

        #endregion

        #endregion

        #region DIRECT METHOD

        #region VINCULACION

        [DirectMethod()]
        public DirectResponse AgregarEditarVinculacion(bool Agregar)
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            InventarioCategoriasVinculacionesTiposVinculacionesController cVinculacioenesTipos = new InventarioCategoriasVinculacionesTiposVinculacionesController();
            cVinculacioenesTipos.SetDataContext(cVinculaciones.Context);
            Data.InventarioCategoriasVinculaciones oDato;
            Data.InventarioCategoriasVinculacionesTiposVinculaciones oDatoTiposVinculaciones;
            long lCategoriaPadreID = long.Parse(hdCategoriaPadre.Value.ToString());
            try
            {
                if (Agregar)
                {
                    oDato = new Data.InventarioCategoriasVinculaciones();
                    if (lCategoriaPadreID == 0)
                    {
                        oDato.InventarioCategoriaPadreID = null;
                    }
                    else
                    {
                        oDato.InventarioCategoriaPadreID = lCategoriaPadreID;
                    }
                    oDato.InventarioCategoriaID = long.Parse(cmbCategorias.Value.ToString());
                    oDato.Activo = true;
                    oDato.TipoRelacion = cmbTipoVin.Value.ToString();
                    if (cmbTipoEmplazamientos.SelectedItem.Text != null && cmbTipoEmplazamientos.SelectedItem.Text != "")
                    {
                        oDato.EmplazamientoTipoID = long.Parse(cmbTipoEmplazamientos.Value.ToString());
                    }
                    else
                    {
                        oDato.EmplazamientoTipoID = null;
                    }
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                    {
                        try
                        {
                            oDato = cVinculaciones.AddItem(oDato);
                            if (oDato != null)
                            {
                                foreach (var item in cmbTipoVinculaciones.SelectedItems)
                                {
                                    oDatoTiposVinculaciones = new Data.InventarioCategoriasVinculacionesTiposVinculaciones();
                                    oDatoTiposVinculaciones.InventarioCategoriaVinculacionID = oDato.InventarioCategoriaVinculacionID;
                                    oDatoTiposVinculaciones.InventarioTipoVinculacionID = long.Parse(item.Value.ToString());
                                    if (cVinculacioenesTipos.AddItem(oDatoTiposVinculaciones) == null)
                                    {
                                        trans.Dispose();
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                trans.Complete();
                            }
                            else
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            log.Error(ex.Message);
                            return direct;
                        }
                    }
                }
                else
                {
                    long lCategoriaID = long.Parse(GridRowSelect.SelectedRecordID);
                    oDato = cVinculaciones.GetItem(lCategoriaID);
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                    {
                        try
                        {
                            if (oDato != null)
                            {
                                oDato.TipoRelacion = cmbTipoVin.Value.ToString();
                                List<string> codigosInvalidos;
                                if (cVinculaciones.ComprobarCambioVinculacion(int.Parse(cmbTipoVin.Value.ToString()), oDato.InventarioCategoriaVinculacionID, oDato.InventarioCategoriaPadreID, out codigosInvalidos))
                                {
                                    if (cVinculaciones.UpdateItem(oDato))
                                    {
                                        if (!cVinculacioenesTipos.DeleteTiposVinculacionesFromVinculacion(lCategoriaID))
                                        {
                                            trans.Dispose();
                                            direct.Success = false;
                                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                            return direct;
                                        }
                                        foreach (var item in cmbTipoVinculaciones.SelectedItems)
                                        {
                                            oDatoTiposVinculaciones = new Data.InventarioCategoriasVinculacionesTiposVinculaciones();
                                            oDatoTiposVinculaciones.InventarioCategoriaVinculacionID = oDato.InventarioCategoriaVinculacionID;
                                            oDatoTiposVinculaciones.InventarioTipoVinculacionID = long.Parse(item.Value.ToString());
                                            if (cVinculacioenesTipos.AddItem(oDatoTiposVinculaciones) == null)
                                            {
                                                trans.Dispose();
                                                direct.Success = false;
                                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                                return direct;
                                            }
                                        }
                                        log.Info(GetGlobalResource(Comun.LogEstructuraActualizada));
                                        trans.Complete();
                                    }
                                    else
                                    {
                                        trans.Dispose();
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                else
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource("strErrorCambiarCardinalidad") + ": " + codigosInvalidos.First();
                                    log.Info(GetGlobalResource("strElementosInvalidos") + String.Join(", ", codigosInvalidos.ToArray()));
                                    return direct;
                                }
                            }
                            else
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Dispose();
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
        public DirectResponse EliminarVinculacion()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            InventarioCategoriasVinculacionesTiposVinculacionesController cVinculacionesTipos = new InventarioCategoriasVinculacionesTiposVinculacionesController();
            cVinculacionesTipos.SetDataContext(cVinculaciones.Context);
            Data.InventarioCategoriasVinculaciones oDato;
            long lCategoriaID = long.Parse(GridRowSelect.SelectedRecordID);
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    if (cVinculacionesTipos.DeleteTiposVinculacionesFromVinculacion(lCategoriaID))
                    {
                        if (cVinculaciones.DeleteItem(lCategoriaID))
                        {
                            trans.Complete();
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                    }
                    else
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
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
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ActivarVinculacion()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            Data.InventarioCategoriasVinculaciones oDato;
            long lCategoriaID = long.Parse(GridRowSelect.SelectedRecordID);
            try
            {
                oDato = cVinculaciones.GetItem(lCategoriaID);
                oDato.Activo = !oDato.Activo;
                if (cVinculaciones.UpdateItem(oDato))
                {
                    log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            InventarioCategoriasVinculacionesTiposVinculacionesController cVinculacionesTipos = new InventarioCategoriasVinculacionesTiposVinculacionesController();
            Data.InventarioCategoriasVinculaciones oDato;
            long lCategoriaID = long.Parse(GridRowSelect.SelectedRecordID);
            List<long> listaIDs = new List<long>(); ;
            try
            {
                oDato = cVinculaciones.GetItem(lCategoriaID);
                cmbCategorias.SetRawValue(oDato.InventarioCategorias.InventarioCategoria);
                var listaTip = cVinculacionesTipos.GetTiposFromVinculaciones(lCategoriaID);
                if (oDato.TipoRelacion != "")
                {
                    cmbTipoVin.SetValue(oDato.TipoRelacion);
                }
                foreach (var item in listaTip)
                {
                    listaIDs.Add(item.InventarioTipoVinculacionID);
                }
                cmbTipoVinculaciones.SetValue(listaIDs);
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        [DirectMethod()]
        public DirectResponse BuscarCategoria()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cCategoria = new InventarioCategoriasController();
            Data.InventarioCategorias oDato;
            string sCategoria = txtSearch.Value.ToString();

            direct.Success = true;
            direct.Result = "";

            try
            {
                oDato = cCategoria.GetCategoriaByNombre(sCategoria);
                if (oDato != null)
                {
                    direct.Result = oDato;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion
    }
}