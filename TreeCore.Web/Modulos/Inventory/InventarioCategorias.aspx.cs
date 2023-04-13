using Ext.Net;
using TreeCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using TreeCore.Clases;

namespace TreeCore.ModInventario
{
    public partial class InventarioCategorias : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public static List<object> ListaIconos;
        public List<Componentes.CategoriasAtributos> listaCategorias;
        private List<long> listaIDsCategorias;
        public Control PanelLimpio;
        protected bool SoloLectura = false;

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);
                hdListaCategorias.Value = "";
                listaCategorias = new List<Componentes.CategoriasAtributos>();

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storeCategorias, GridPanelCategorias.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (ListaIconos == null || ListaIconos.Count <= 0)
                {
                    ListaIconos = GetIconsOfInvetory();
                }

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.SetValue(ClienteID);
                    hdCliID.DataBind();
                }
                //CargarMenu();



            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SoloLectura = true;

            sPagina = "InventarioCategoriasContenedor.aspx";
            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var UserInterface = ModulesController.GetUserInterfaces().FirstOrDefault(x => x.Page.ToLower() == sPagina.ToLower());
            var listFunPag = listFun.Where(x => $"{x.Split('@')[0]}" == UserInterface.Code);

            if (listFunPag.Where(x => x.Split('@')[1] == "Put").ToList().Count > 0)
            {
                SoloLectura = false;
            }
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnRestriccionActive, btnRestriccionDisabled, btnRestriccionHidden, cmbNuevaCategorias }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
            };

            PintarCategorias(false);
   		 	AtributosConfiguracion.TipoAtributo = Comun.MODULOINVENTARIO;
        }

        #region STORES

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
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region CATEGORIAS ATRIBUTOS LIBRES

        protected void storeCategoriasLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<InventarioAtributosCategorias> listaFinal = new List<InventarioAtributosCategorias>();
                InventarioAtributosCategoriasController cCategorias = new InventarioAtributosCategoriasController();
                try
                {
                    List<InventarioAtributosCategorias> listaCategoriasN = cCategorias.getUnselectInventarioAtributosCategorias(long.Parse(hdCatSelect.Value.ToString()), long.Parse(hdCliID.Value.ToString()));

                    if (listaCategoriasN != null)
                    {
                        storeCategoriasLibres.DataSource = listaCategoriasN;
                    }
                    else
                    {
                        storeCategoriasLibres.DataSource = new List<InventarioAtributosCategorias>(); ;
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

        #region IMAGENES

        protected void storeIconos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (ListaIconos == null || ListaIconos.Count <= 0)
                    {
                        ListaIconos = GetIconsOfInvetory();
                    }
                    storeIconos.DataSource = ListaIconos;
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

        private List<object> GetIconsOfInvetory()
        {
            List<object> lista = new List<object>();

            string prefix = "ico-*";
            try
            {
                string[] fileEntries = Directory.GetFiles(Comun.rutaIconoInventario(), prefix);
                foreach (string filePath in fileEntries)
                {
                    //string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string fileNameExtension = Path.GetFileName(filePath);

                    lista.Add(new object[] { Comun.rutaIconoWebInventario(fileNameExtension), fileNameExtension });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        #endregion

        #region CATEGORIA

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                List<Data.InventarioCategorias> listaDatos;
                List<JsonObject> listaJson;
                JsonObject oJson;
                try
                {
                    listaDatos = cCategorias.GetInventarioCategoriasByTipoEmplazamientoSoloActivos(cmbTipoEmplazamientos.SelectedItem.Value, long.Parse(hdCliID.Value.ToString()), btnActivos.Pressed);
                    if (listaDatos != null)
                    {
                        listaJson = new List<JsonObject>();
                        foreach (var item in listaDatos)
                        {
                            oJson = new JsonObject();
                            oJson.Add("InventarioCategoriaID", item.InventarioCategoriaID);
                            oJson.Add("InventarioCategoria", item.InventarioCategoria);
                            oJson.Add("Activo", item.Activo);
                            oJson.Add("Icono", Comun.rutaIconoWebInventario(item.Icono));
                            listaJson.Add(oJson);
                        }

                        storeCategorias.DataSource = listaJson;
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

        #region ATRIBUTOS CATEGORIA

        [DirectMethod()]
        public DirectResponse RecargarItemsNuevasCategorias()
        {
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                long cliID = long.Parse(hdCliID.Value.ToString());
                Componentes.CategoriasAtributos compCatAtri;
                hdListaCategorias.SetValue("");
                listaCategorias.Clear();
                pnConfigurador.ContentControls.Clear();
                if (hdCatSelect.Value.ToString() != "root" && hdCatSelect.Value.ToString() != "")
                {
                    foreach (var item in cCategoriasVin.GetCategoriasAtributosVinculadas(long.Parse(hdCatSelect.Value.ToString())))
                    {
                        compCatAtri = (Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                        compCatAtri.ID = "CAT" + item.CoreInventarioCategoriaAtributoCategoriaID.ToString();
                        compCatAtri.CategoriaAtributoID = item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                        compCatAtri.CategoriaID = long.Parse(hdCatSelect.Value.ToString());
                        compCatAtri.CategoriaAtributoAsignacionID = item.CoreInventarioCategoriaAtributoCategoriaID;
                        compCatAtri.Nombre = item.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.InventarioAtributoCategoria;
                        compCatAtri.Orden = (long)item.Orden;
                        compCatAtri.Modulo = (long)Comun.Modulos.INVENTARIO;
                        compCatAtri.EsPlantilla = item.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsSubcategoria;
                        compCatAtri.EsSoloLectura = this.SoloLectura;
                        if (!item.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsSubcategoria)
                        {
                            compCatAtri.TipoCategoria = "Seccion";
                        }
                        else if (!item.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsPlantilla)
                        {
                            compCatAtri.TipoCategoria = "Subcategoria";
                        }
                        else
                        {
                            compCatAtri.TipoCategoria = "SubcategoriaPlantilla";
                        }
                        listaCategorias.Add(compCatAtri);
                    }
                }
                PintarCategorias(true);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        #region DIRECT METHOD ARBOL

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            InventarioCategoriasController cCategoria = new InventarioCategoriasController();
            InventarioAtributosCategoriasController cCategoriaAtr = new InventarioAtributosCategoriasController();
            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            cVinculaciones.SetDataContext(cCategoria.Context);
            Data.InventarioCategoriasVinculaciones oVinculacion;

            try
            {
                direct.Success = true;
                direct.Result = "";

                long? TipoID;
                long cliID = long.Parse(hdCliID.Value.ToString());
                try
                {
                    TipoID = long.Parse(cmbTipoEmplazamientos.SelectedItem.Value);

                }
                catch (Exception)
                {
                    TipoID = null;
                }
                if (!agregar)
                {
                    long S = long.Parse(hdCatSelect.Value.ToString());
                    Data.InventarioCategorias oDato = cCategoria.GetItem(S);
                    if (oDato.InventarioCategoria == txtNombre.Text && oDato.Codigo == txtCodigo.Text)
                    {
                        oDato.InventarioCategoria = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.Icono = cmbIconos.SelectedItem.Value;
                        oDato.EspacioEnSuelo = chkEspacioEnSuelo.Checked;
                        oDato.BloqueadoTerceros = chkBloqueoTerceros.Checked;
                        oDato.EsContenedor = chkEsContenedor.Checked;
                    }
                    else
                    {
                        if (oDato.InventarioCategoria != txtNombre.Text && cCategoria.NombreDuplicado(txtNombre.Text, TipoID, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                        }
                        else if (oDato.Codigo != txtCodigo.Text && (cCategoria.CodigoDuplicado(txtCodigo.Text, TipoID, cliID) || cCategoriaAtr.RegistroDuplicado(txtCodigo.Text, cliID)))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsCodigoExiste);
                        }
                        else
                        {
                            oDato.InventarioCategoria = txtNombre.Text;
                            oDato.Codigo = txtCodigo.Text;
                            oDato.Icono = cmbIconos.SelectedItem.Value;
                            oDato.EspacioEnSuelo = chkEspacioEnSuelo.Checked;
                            oDato.BloqueadoTerceros = chkBloqueoTerceros.Checked;
                            oDato.EsContenedor = chkEsContenedor.Checked;
                        }
                    }

                    if (cCategoria.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        //CargarMenu();
                    }
                }
                else
                {
                    if (cCategoria.NombreDuplicado(txtNombre.Text, TipoID, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);
                    }
                    else if ((cCategoria.CodigoDuplicado(txtCodigo.Text, TipoID, cliID) || cCategoriaAtr.RegistroDuplicado(txtCodigo.Text, cliID)))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsCodigoExiste);
                    }
                    else
                    {
                        Data.InventarioCategorias oDato = new Data.InventarioCategorias();
                        oDato.InventarioCategoria = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.Icono = cmbIconos.SelectedItem.Value;
                        oDato.EspacioEnSuelo = chkEspacioEnSuelo.Checked;
                        oDato.BloqueadoTerceros = chkBloqueoTerceros.Checked;
                        oDato.EsContenedor = chkEsContenedor.Checked;
                        oDato.EmplazamientoTipoID = TipoID;
                        oDato.ClienteID = cliID;
                        oDato.Activo = true;

                        using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                        {
                            if ((oDato = cCategoria.AddItem(oDato)) != null)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            }
                            else
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                            trans.Complete();
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


            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cCategoria = new InventarioCategoriasController();

            try

            {
                long S = long.Parse(hdCatSelect.Value.ToString());

                Data.InventarioCategorias oDato = cCategoria.GetItem(S);
                txtNombre.Text = oDato.InventarioCategoria;
                txtCodigo.Text = oDato.Codigo;
                cmbIconos.SetValue(oDato.Icono);
                chkEspacioEnSuelo.Checked = oDato.EspacioEnSuelo;
                chkBloqueoTerceros.Checked = (bool)oDato.BloqueadoTerceros;
                chkEsContenedor.Checked = oDato.EsContenedor;
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
            InventarioCategoriasController cCategoria = new InventarioCategoriasController();
            InventarioCategoriasVinculacionesController cVinculaciones = new InventarioCategoriasVinculacionesController();
            cVinculaciones.SetDataContext(cCategoria.Context);
            List<Data.InventarioCategoriasVinculaciones> listaVinculaciones;

            long lID = long.Parse(GridRowSelectArbol.SelectedRecordID);


            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    if (!cCategoria.TieneRegistroAsociado(lID))
                    {

                        listaVinculaciones = cVinculaciones.GetVinculacionesFromCategoria(lID);
                        foreach (var vin in listaVinculaciones)
                        {
                            if (!cVinculaciones.DeleteItem(vin.InventarioCategoriaVinculacionID))
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                        }
                        if (cCategoria.DeleteItem(lID))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                    }
                    else
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                        return direct;
                    }
                    trans.Complete();
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

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cCategoria = new InventarioCategoriasController();

            try
            {
                long lID = long.Parse(GridRowSelectArbol.SelectedRecordID);
                Data.InventarioCategorias oDato;
                oDato = cCategoria.GetItem(lID);
                if (!cCategoria.TieneRegistroAsociadoActivar(lID) || !oDato.Activo)
                {
                    oDato.Activo = !oDato.Activo;

                    if (cCategoria.UpdateItem(oDato))
                    {
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsActTieneRegistros);
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
        public DirectResponse RecargarMenu()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                //CargarMenu();
                direct.Success = true;
                direct.Result = "";
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


        [DirectMethod]
        public DirectResponse ExportarModeloDatos()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cController = new InventarioCategoriasController();
            List<Data.InventarioCategorias> listaCategorias = new List<Data.InventarioCategorias>();
            InventarioAtributosController cAtributos = new InventarioAtributosController();

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosInventario") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;

                List<string> lTabs = new List<string>();

                long cliID = 0;

                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else
                {
                    if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                    }
                }

                string ltipo = "0";
                bool ltipologiaID = false;
                if (cmbTipoEmplazamientos.SelectedItem.Value != null && cmbTipoEmplazamientos.SelectedItem.Value != "")
                {
                    ltipo = cmbTipoEmplazamientos.SelectedItem.Value.ToString();
                }

                ltipologiaID = cController.IsCategoriasByEmplazamientoTipo(Convert.ToInt32(ltipo));

                if (!ltipo.Equals("0") && ltipologiaID)
                {
                    listaCategorias = cController.GetInventarioCategoriasByTipoEmplazamiento2(Convert.ToInt32(ltipo), cliID);
                }
                else
                {
                    listaCategorias = cController.GetInventarioCategoriasByTipoEmplazamiento2(null, cliID);
                }

                cController.ExportarModeloDatos(listaCategorias, saveAs, GetGlobalResource("strEmplazamientoCodigo"), GetGlobalResource("strNombreElemento"), GetGlobalResource("strCodigoElemento"), GetGlobalResource("strCodigoElementoPadre"), GetGlobalResource("strEstadoInventario"), GetGlobalResource("strEntidad"), GetGlobalResource("strCuadroTexto") + " - " + GetGlobalResource("strTexto"));

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);

                Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        #region DIRECT METHOD ATRIBUTOS - CATEGORIAS

        [DirectMethod()]
        public DirectResponse CambiarRestriccionDefecto(string sBtn)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                switch (sBtn)
                {
                    case "Active":
                        Comun.SetRestriccionDefectoInventario((long)Comun.RestriccionesAtributoDefecto.ACTIVE);
                        break;
                    case "Disabled":
                        Comun.SetRestriccionDefectoInventario((long)Comun.RestriccionesAtributoDefecto.DISABLED);
                        break;
                    case "Hidden":
                        Comun.SetRestriccionDefectoInventario((long)Comun.RestriccionesAtributoDefecto.HIDDEN);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        #region DIRECT METHOD CATEGORIAS

        [DirectMethod()]
        public DirectResponse SeleccionarNuevaCategoria(long lCatID)
        {
            DirectResponse direct = new DirectResponse();
            InventarioAtributosCategoriasController cCategoriasAtributos = new InventarioAtributosCategoriasController();
            Data.InventarioAtributosCategorias oInventarioAtributosCategorias;
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasAtributosVin = new CoreInventarioCategoriasAtributosCategoriasController();
            Data.CoreInventarioCategoriasAtributosCategorias oInventarioAtributosCategoriasVin;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasAtributosConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oInventarioAtributosCategoriasConf;
            try
            {
                direct.Success = true;
                direct.Result = "";
                oInventarioAtributosCategorias = cCategoriasAtributos.GetItem(lCatID);
                if (oInventarioAtributosCategorias.EsSubcategoria)
                {
                    oInventarioAtributosCategoriasConf = cCategoriasAtributosConf.GetPlantilla(lCatID);
                }
                else
                {
                    oInventarioAtributosCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                    {
                        InventarioCategoriaID = long.Parse(hdCatSelect.Value.ToString()),
                        InventarioAtributoCategoriaID = lCatID
                    };
                    oInventarioAtributosCategoriasConf = cCategoriasAtributosConf.AddItem(oInventarioAtributosCategoriasConf);
                }
                if (oInventarioAtributosCategoriasConf != null)
                {
                    if (cCategoriasAtributosConf.ComprobarDuplicidadNombresCategorias(long.Parse(hdCatSelect.Value.ToString()), oInventarioAtributosCategoriasConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID, oInventarioAtributosCategoriasConf.InventarioAtributosCategorias.InventarioAtributoCategoria))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource("strSubcategoriaNombreDuplicado");
                        return direct;
                    }
                    oInventarioAtributosCategoriasVin = new CoreInventarioCategoriasAtributosCategorias
                    {
                        InventarioCategoriaID = long.Parse(hdCatSelect.Value.ToString()),
                        CoreInventarioCategoriaAtributoCategoriaConfiguracionID = oInventarioAtributosCategoriasConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID,
                        Orden = listaCategorias.Count()
                    };
                    oInventarioAtributosCategoriasVin = cCategoriasAtributosVin.AddItem(oInventarioAtributosCategoriasVin);
                    if (oInventarioAtributosCategoriasVin != null)
                    {
                        Componentes.CategoriasAtributos Comp = (Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                        Comp.ID = "CAT" + oInventarioAtributosCategoriasVin.CoreInventarioCategoriaAtributoCategoriaID.ToString();
                        Comp.CategoriaAtributoID = oInventarioAtributosCategoriasVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                        Comp.CategoriaID = long.Parse(hdCatSelect.Value.ToString());
                        Comp.CategoriaAtributoAsignacionID = oInventarioAtributosCategoriasVin.CoreInventarioCategoriaAtributoCategoriaID;
                        Comp.Nombre = oInventarioAtributosCategoriasVin.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.InventarioAtributoCategoria;
                        Comp.Orden = (long)oInventarioAtributosCategoriasVin.Orden;
                        Comp.Modulo = (long)Comun.Modulos.INVENTARIO;
                        Comp.EsPlantilla = oInventarioAtributosCategoriasVin.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsSubcategoria;
                        Comp.EsSoloLectura = this.SoloLectura;
                        if (!oInventarioAtributosCategoriasVin.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsSubcategoria)
                        {
                            Comp.TipoCategoria = "Seccion";
                        }
                        else if (!oInventarioAtributosCategoriasVin.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsPlantilla)
                        {
                            Comp.TipoCategoria = "Subcategoria";
                        }
                        else
                        {
                            Comp.TipoCategoria = "SubcategoriaPlantilla";
                        }
                        listaCategorias.Add(Comp);
                        PintarCategorias(true);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        #endregion

        #region DIRECT METHOD ATRIBUTOS

        #endregion

        #endregion

        #endregion

        #region FUNTIONS

        #region CATEGORIAS

        [DirectMethod(Timeout = 120000)]
        public DirectResponse PintarCategorias(bool Update, bool Ordenar = false)
        {
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            Componentes.CategoriasAtributos oComponente;
            Data.CoreInventarioCategoriasAtributosCategorias oDato;
            List<Ext.Net.AbstractComponent> listaBotonAñadirCategorias;
            long cliID = long.Parse(hdCliID.Value.ToString());
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            if (listaCategorias == null)
            {
                try
                {
                    listaCategorias = new List<Componentes.CategoriasAtributos>();
                    if (hdListaCategorias.Value.ToString() != "")
                    {
                        long catID = long.Parse(hdCatSelect.Value.ToString());
                        foreach (var idCate in hdListaCategorias.Value.ToString().Split(','))
                        {
                            oDato = cCategoriasVin.GetRelacion(catID, long.Parse(idCate));
                            if (oDato != null)
                            {
                                oComponente = (Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                                oComponente.ID = "CAT" + oDato.CoreInventarioCategoriaAtributoCategoriaID.ToString();
                                oComponente.CategoriaAtributoID = oDato.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                oComponente.CategoriaAtributoAsignacionID = oDato.CoreInventarioCategoriaAtributoCategoriaID;
                                oComponente.CategoriaID = long.Parse(hdCatSelect.Value.ToString());
                                oComponente.Nombre = oDato.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.InventarioAtributoCategoria;
                                oComponente.Orden = (long)oDato.Orden;
                                oComponente.Modulo = (long)Comun.Modulos.INVENTARIO;
                                oComponente.EsPlantilla = oDato.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsSubcategoria;
                                oComponente.EsSoloLectura = this.SoloLectura;
                                if (!oDato.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsSubcategoria)
                                {
                                    oComponente.TipoCategoria = "Seccion";
                                }
                                else if (!oDato.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsPlantilla)
                                {
                                    oComponente.TipoCategoria = "Subcategoria";
                                }
                                else
                                {
                                    oComponente.TipoCategoria = "SubcategoriaPlantilla";
                                }
                                listaCategorias.Add(oComponente);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            try
            {
                listaIDsCategorias = new List<long>();

                listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();

                foreach (var item in listaCategorias)
                {
                    pnConfigurador.ContentControls.Add(item);
                    listaIDsCategorias.Add(item.CategoriaAtributoID);
                }
                if (Update)
                {
                    storeCategoriasLibres.Reload();


                    string categorias = "";
                    foreach (var item in listaCategorias)
                    {
                        item.PintarAtributos(false);
                        if (categorias != "")
                            categorias += ",";
                        categorias += item.CategoriaAtributoID.ToString();
                    }
                    hdListaCategorias.SetValue(categorias);

                    pnConfigurador.UpdateContent();

                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        #endregion

        #endregion
    }
}