using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Data;
using TreeCore.Componentes;
using TreeCore.Data;
using System.Linq;
using TreeCore.Clases;
using System.Data.Linq.Mapping;
using System.Globalization;
using Newtonsoft.Json;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosContenedor : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        List<TreeCore.Componentes.GestionCategoriasAtributos> listaCategorias;
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        #region GESTION PAGINA (Init/Load)

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                hdAdicionalCargado.Value = false;

                EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cController = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaAllowBlank = cController.getAtributosByNombre("Allow Blank");

                if (listaAllowBlank.Count > 0)
                {
                    hdAllowBlank.Value = true;
                }
                else
                {
                    hdAllowBlank.Value = false;
                }

                #region KPI
                if (Request.QueryString[Comun.PARAM_IDS_RESULTADOS] != null && Request.QueryString[Comun.PARAM_NAME_INDICE_ID] != null)
                {
                    hdidsResultados.SetValue(Request.QueryString[Comun.PARAM_IDS_RESULTADOS]);
                    hdnameIndiceID.SetValue(Request.QueryString[Comun.PARAM_NAME_INDICE_ID]);
                }
                #endregion
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Usuario.ClienteID != null)
            {
                hdCliID.Value = Usuario.ClienteID.ToString();
            }
            PintarCategorias(false);
        }

        #endregion

        #region STORES

        #region FORMULARIO EMPLAZAMIENTOS

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Entidades> listaOperadores = ListaOperadores();

                    if (listaOperadores != null)
                    {
                        this.storeOperadores.DataSource = listaOperadores;
                        this.storeOperadores.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Entidades> ListaOperadores()
        {
            List<Data.Entidades> listaOperadores;
            EntidadesController cOperadores = new EntidadesController();

            try
            {
                listaOperadores = cOperadores.getEntidadesOperadores(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaOperadores = null;
            }

            return listaOperadores;
        }

        #endregion

        #region ESTADOS GLOBALES

        protected void storeEstadosGlobales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EstadosGlobales> listaEstadosGlobales = ListaEstadosGlobales();

                    if (listaEstadosGlobales != null)
                    {
                        this.storeEstadosGlobales.DataSource = listaEstadosGlobales;
                        this.storeEstadosGlobales.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EstadosGlobales> ListaEstadosGlobales()
        {
            List<Data.EstadosGlobales> listaEstadosGlobales;
            EstadosGlobalesController cEstados = new EstadosGlobalesController();

            try
            {
                listaEstadosGlobales = cEstados.GetEstadosGlobalesActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEstadosGlobales = null;
            }

            return listaEstadosGlobales;
        }

        #endregion

        #region CATEGORIAS

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosCategoriasSitios> listaCategorias = ListaCategorias();

                    if (listaCategorias != null)
                    {
                        this.storeCategorias.DataSource = listaCategorias;
                        this.storeCategorias.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosCategoriasSitios> ListaCategorias()
        {
            List<Data.EmplazamientosCategoriasSitios> listaCategorias;
            EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();

            try
            {
                listaCategorias = cCategorias.GetCategoriasSitiosActivas(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaCategorias = null;
            }

            return listaCategorias;
        }

        #endregion

        #region TIPOS

        protected void storeTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTipos> listaTipos = ListaTipos();

                    if (listaTipos != null)
                    {
                        this.storeTipos.DataSource = listaTipos;
                        this.storeTipos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTipos> ListaTipos()
        {
            List<Data.EmplazamientosTipos> listaTipos;
            EmplazamientosTiposController cTipos = new EmplazamientosTiposController();

            try
            {
                listaTipos = cTipos.GetEmplazamientosTiposActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        #endregion

        #region TAMAÑOS

        protected void storeTamanos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTamanos> listaTamanos = ListaTamanos();

                    if (listaTamanos != null)
                    {
                        this.storeTamanos.DataSource = listaTamanos;
                        this.storeTamanos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTamanos> ListaTamanos()
        {
            List<Data.EmplazamientosTamanos> listaTamanos;
            EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();

            try
            {
                listaTamanos = cTamanos.GetEmplazamientosTamanosActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTamanos = null;
            }

            return listaTamanos;
        }

        #endregion

        #region TIPOS ESTRUCTURAS

        protected void storeTiposEstructuras_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTiposEstructuras> listaTiposEstructuras = ListaTiposEstructuras();

                    if (listaTiposEstructuras != null)
                    {
                        this.storeTiposEstructuras.DataSource = listaTiposEstructuras;
                        this.storeTiposEstructuras.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTiposEstructuras> ListaTiposEstructuras()
        {
            List<Data.EmplazamientosTiposEstructuras> listaTiposEstructuras;
            EmplazamientosTiposEstructurasController cTiposEstructuras = new EmplazamientosTiposEstructurasController();

            try
            {
                listaTiposEstructuras = cTiposEstructuras.GetActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTiposEstructuras = null;
            }

            return listaTiposEstructuras;
        }

        #endregion

        #region TIPOS EDIFICIOS

        protected void storeTiposEdificios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTiposEdificios> listaTiposEdificios = ListaTiposEdificios();

                    if (listaTiposEdificios != null)
                    {
                        this.storeTiposEdificios.DataSource = listaTiposEdificios;
                        this.storeTiposEdificios.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTiposEdificios> ListaTiposEdificios()
        {
            List<Data.EmplazamientosTiposEdificios> listaTiposEdificios;
            EmplazamientosTiposEdificiosController cTiposEdificios = new EmplazamientosTiposEdificiosController();

            try
            {
                listaTiposEdificios = cTiposEdificios.GetActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTiposEdificios = null;
            }

            return listaTiposEdificios;
        }

        #endregion

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Monedas> listaMonedas = ListaMonedas();

                    if (listaMonedas != null)
                    {
                        this.storeMonedas.DataSource = listaMonedas;
                        this.storeMonedas.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Monedas> ListaMonedas()
        {
            List<Data.Monedas> listaMonedas;
            MonedasController cMonedas = new MonedasController();

            try
            {
                listaMonedas = cMonedas.GetActivosCliente(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMonedas = null;
            }

            return listaMonedas;
        }

        #endregion

        #endregion

        #endregion

        #region FILTROS

        #region CAMPOS
        protected void storeCampos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            List<object> listaDatos;
            if (RequestManager.IsAjaxRequest)
            {
                try
                {

                    AttributeMappingSource oModelo = new AttributeMappingSource();
                    var vModel = oModelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));
                    MetaTable mt = vModel.GetTable(typeof(Vw_CoreEmplazamientosFiltros));

                    List<string> fieldsIgnorados = new List<string> {
                        "EmplazamientoID",
                        "ClienteID",
                        "Proyectos",
                        "Contratos",
                        "Municipio",
                        "Provincia",
                        "Region",
                        "RegionPais",
                        "RegionID",
                        "RegionPaisID",
                        "ProvinciaID",
                        "MunicipioID",
                        "InventarioElementoID"
                    };

                    listaDatos = new List<object>();
                    List<CampoField> listTemp = new List<CampoField>();
                    foreach (MetaDataMember mdm in mt.RowType.DataMembers)
                    {
                        if (!fieldsIgnorados.Contains(mdm.Name))
                        {
                            try
                            {
                                listTemp.Add(new CampoField(mdm.Name, getCampoByColum(mdm.Name), mdm.Type.FullName));
                            }
                            catch (Exception)
                            {
                                log.Warn("Column not found: " + mdm.Name);
                            }
                        }
                    }

                    listTemp.Sort((x, y) => x.Name.CompareTo(y.Name));

                    listTemp.ForEach(x => { listaDatos.Add(new { Id = x.Id, Name = x.Name, typeData = x.typeData }); });

                    Store store = cmbField.GetStore();
                    if (store != null)
                    {

                        store.DataSource = listaDatos;
                        store.DataBind();
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        public string getCampoByColum(string colum)
        {
            string result;
            try
            {
                Comun.TiposDinamicosFiltro tipo = Comun.columTagCoreEmplazamientosFiltros[colum];
                result = GetGlobalResource(tipo.tagTraduction);
            }
            catch (Exception)
            {
                result = "";
            }

            return result;
        }

        public class CampoField
        {
            public CampoField(string Id, string Name, string typeData)
            {
                this.Id = Id;
                this.Name = Name;
                this.typeData = typeData;
            }

            public string Id;
            public string Name;
            public string typeData;
        }
        #endregion

        #region MIS FILTROS
        protected void storeMyFilters_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            List<GestionFiltros> misFiltros;
            GestionFiltrosController cGestionFiltros = new GestionFiltrosController();

            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    Store store = GridMyFilters.GetStore();

                    misFiltros = cGestionFiltros.GetFiltros(this.Usuario.UsuarioID, this.GetType().Name);

                    store.DataSource = misFiltros;
                    store.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #region TiposDinamicos
        protected void storeTiposDinamicos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            List<object> tiposDinamicos;

            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    Store store = cmbTiposDinamicos.GetStore();

                    string selecction = cmbField.SelectedItem.Value;
                    string nombreTabla = Comun.columTagCoreEmplazamientosFiltros[selecction].tabla;
                    string text = getCampoByColum(selecction);

                    cmbTiposDinamicos.FieldLabel = text;
                    cmbTiposDinamicos.SetEmptyText(text);

                    Type tipo = Type.GetType("TreeCore.Data." + nombreTabla);
                    Type tipoController = Type.GetType("CapaNegocio." + nombreTabla + "Controller");

                    var instance = Activator.CreateInstance(tipoController);

                    MethodInfo method = tipoController.GetMethod("GetActivos");

                    dynamic list = method.Invoke(instance, new object[] { ClienteID });



                    tiposDinamicos = new List<object>();
                    foreach (object obj in list)
                    {
                        PropertyInfo propID = tipo.GetProperty(Comun.columTagCoreEmplazamientosFiltros[selecction].nameTableId);
                        PropertyInfo propName = tipo.GetProperty(Comun.columTagCoreEmplazamientosFiltros[selecction].name);
                        long id = (long)propID.GetValue(obj, null);
                        string name = (string)propName.GetValue(obj, null);

                        tiposDinamicos.Add(new { Name = name, Id = id });
                    }


                    store.DataSource = tiposDinamicos;
                    store.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private object GetInstanceOfListType(Type t)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);

            return Activator.CreateInstance(constructedListType);
        }
        #endregion

        [DirectMethod()]
        public DirectResponse AgregarFiltro(string filtroAGuardar)
        {
            DirectResponse direct = new DirectResponse();
            GestionFiltrosController cGestionFiltros = new GestionFiltrosController();

            try
            {
                JObject filtroAplicado = JObject.Parse(filtroAGuardar);

                JArray arrayItemsFiltro = (JArray)filtroAplicado["filters"];
                string nombreFiltro = (string)filtroAplicado["name"];
                string idFiltro = (string)filtroAplicado["id"];


                GestionFiltros filtroExistente = null;
                if (!idFiltro.Contains("temp-"))
                {
                    filtroExistente = cGestionFiltros.GetItem(long.Parse(idFiltro));
                }

                if (filtroExistente != null)
                {
                    filtroExistente.NombreFiltro = nombreFiltro;
                    filtroExistente.JsonItemsFiltro = arrayItemsFiltro.ToString();

                    if (cGestionFiltros.UpdateItem(filtroExistente))
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }

                    direct.Success = true;
                    direct.Result = "";
                }
                else
                {
                    GestionFiltros newFiltro = new GestionFiltros()
                    {
                        Pagina = this.GetType().Name,
                        UsuarioID = this.Usuario.UsuarioID,
                        NombreFiltro = nombreFiltro,
                        JsonItemsFiltro = arrayItemsFiltro.ToString(),
                    };

                    newFiltro = cGestionFiltros.AddItem(newFiltro);
                    if (newFiltro != null)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        direct.Success = true;
                        direct.Result = "{\"oldID\": \"" + idFiltro + "\", \"newID\": \"" + newFiltro.GestionFiltroID + "\"}";
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
        public DirectResponse EliminarFiltro(long id)
        {
            DirectResponse direct = new DirectResponse();
            GestionFiltrosController cGestionFiltros = new GestionFiltrosController();

            try
            {
                if (cGestionFiltros.DeleteItem(id))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
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

        #region Direct Methods

        [DirectMethod()]
        public DirectResponse AplicarFiltro(string filtrosAplicados, Ext.Net.StoreReadDataEventArgs e, int pageSize, int curPage)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            try
            {
                int total;
                List<string> listaVacia = new List<string>();
                List<JsonObject> lista = cEmplazamientos.AplicarFiltroInterno(true, filtrosAplicados, pageSize, curPage, out total, null, null, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, null, "");

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


        #region FORMULARIO EMPLAZAMIENTOS

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool SobrescribirEdicion)

        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            List<Object> listaAtributos = new List<object>();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.Emplazamientos oDato;
            UsuariosController cUsuarios = new UsuariosController();
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

            HistoricosCoreEmplazamientosController cHistorico = new HistoricosCoreEmplazamientosController();
            Data.HistoricosCoreEmplazamientos DatoHistorico;

            long lCliID = 0;
            long lMunicipioID = 0;
            long? lPaisID = 0;
            direct.Success = true;
            direct.Result = "";

            try
            {

                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
                Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);

                if (hdEmplazamientoID.Value != null && hdEmplazamientoID.Value.ToString() != "")
                {
                    oDato = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));

                    DatoHistorico = cHistorico.getHistoricoByID(oDato.EmplazamientoID);


                    if (oDato.Codigo != txtCodigo.Text || oDato.EntidadID != long.Parse(cmbOperadores.SelectedItem.Value))
                    {
                        if (!cEmplazamientos.EmplazamientoDuplicadoCodigoOperador(long.Parse(cmbOperadores.SelectedItem.Value), txtCodigo.Text))
                        {
                            if (SobrescribirEdicion || hdHistoricoEmplazamiento.Value.ToString() == "" || (hdHistoricoEmplazamiento.Value.ToString() == DatoHistorico.HistoricoCoreEmplazamientoID.ToString()))
                            {
                                #region UPDATE
                                DateTime? dateDeactivationSave = null;
                                if (dateDeactivation.SelectedDate > DateTime.MinValue)
                                {
                                    dateDeactivationSave = dateDeactivation.SelectedDate;
                                }

                                foreach (var item in listaCategorias)
                                {
                                    if (!item.GuardarValor(listaAtributos, cEmplazamientos.Context))
                                    {
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }

                                if (geoEmplazamiento.Municipio != "")
                                {
                                    lPaisID = cMunicipios.getPaisByMunicipio(geoEmplazamiento.Municipio.Split(',')[0]);

                                    if (lPaisID != 0)
                                    {
                                        lMunicipioID = cMunicipios.GetMunicipioByNombre(geoEmplazamiento.Municipio.Split(',')[0]);
                                    }
                                }

                                ResponseCreateController responseCreate = cEmplazamientos.CreateSite(true, oDato.EmplazamientoID, oUser, lCliID,
                                    txtCodigo.Text,
                                    txtNombre.Text,
                                    Convert.ToInt64(cmbOperadores.SelectedItem.Value),
                                    Convert.ToInt64(cmbEstadosGlobales.SelectedItem.Value),
                                    Convert.ToInt64(cmbMonedas.SelectedItem.Value),
                                    Convert.ToInt64(cmbCategorias.SelectedItem.Value),
                                    Convert.ToInt64(cmbTipos.SelectedItem.Value),
                                    (cmbTiposEstructuras.Value != null && cmbTiposEstructuras.Value.ToString() != "") ? Convert.ToInt64(cmbTiposEstructuras.Value) : 0,
                                    Convert.ToInt64(cmbTiposEdificios.SelectedItem.Value),
                                    (cmbTamanos.Value != null && cmbTamanos.Value.ToString() != "") ? Convert.ToInt64(cmbTamanos.Value) : 0,
                                    dateActivation.SelectedDate,
                                    dateDeactivationSave,
                                    (long)lPaisID,
                                    lMunicipioID,
                                    geoEmplazamiento.Direccion,
                                    geoEmplazamiento.Barrio,
                                    geoEmplazamiento.CodigoPostal,
                                    Convert.ToDouble(geoEmplazamiento.Longitud),
                                    Convert.ToDouble(geoEmplazamiento.Latitud),
                                    listaAtributos);

                                if (responseCreate.Data == null)
                                {
                                    direct.Success = false;
                                    direct.Result = GetLocalResourceObject("strErrorGenerarHistorico").ToString();
                                    return direct;
                                }
                                #endregion
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = "Editado";
                            }
                        }
                        else
                        {
                            hdCodigoDuplicado.SetValue("Duplicado");
                            direct.Success = false;
                            direct.Result = "Codigo";
                            return direct;
                        }
                    }
                    else
                    {
                        if (SobrescribirEdicion || hdHistoricoEmplazamiento.Value.ToString() == "" || (hdHistoricoEmplazamiento.Value.ToString() == DatoHistorico.HistoricoCoreEmplazamientoID.ToString()))
                        {
                            #region UPDATE

                            DateTime? dateDeactivationSave = null;
                            if (dateDeactivation.SelectedDate > DateTime.MinValue)
                            {
                                dateDeactivationSave = dateDeactivation.SelectedDate;
                            }

                            foreach (var item in listaCategorias)
                            {
                                if (!item.GuardarValor(listaAtributos, cEmplazamientos.Context))
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
                                }
                            }

                            if (geoEmplazamiento.Municipio != "" && geoEmplazamiento.Municipio != null)
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoEmplazamiento.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoEmplazamiento.Municipio.Split(',')[0]);
                                }

                            }

                            ResponseCreateController responseCreate = cEmplazamientos.CreateSite(true, oDato.EmplazamientoID, oUser, lCliID,
                                txtCodigo.Text,
                                txtNombre.Text,
                                Convert.ToInt64(cmbOperadores.SelectedItem.Value),
                                Convert.ToInt64(cmbEstadosGlobales.SelectedItem.Value),
                                Convert.ToInt64(cmbMonedas.SelectedItem.Value),
                                Convert.ToInt64(cmbCategorias.SelectedItem.Value),
                                Convert.ToInt64(cmbTipos.SelectedItem.Value),
                                (cmbTiposEstructuras.Value != null && cmbTiposEstructuras.Value.ToString() != "") ? Convert.ToInt64(cmbTiposEstructuras.Value) : 0,
                                Convert.ToInt64(cmbTiposEdificios.SelectedItem.Value),
                                (cmbTamanos.Value != null && cmbTamanos.Value.ToString() != "") ? Convert.ToInt64(cmbTamanos.Value) : 0,
                                dateActivation.SelectedDate,
                                dateDeactivationSave,
                                (long)lPaisID,
                                lMunicipioID,
                                geoEmplazamiento.Direccion,
                                geoEmplazamiento.Barrio,
                                geoEmplazamiento.CodigoPostal,
                                Convert.ToDouble(geoEmplazamiento.Longitud),
                                Convert.ToDouble(geoEmplazamiento.Latitud),
                                listaAtributos);

                            if (responseCreate.Data == null)
                            {
                                direct.Success = false;
                                direct.Result = GetLocalResourceObject("strErrorGenerarHistorico").ToString();
                                return direct;
                            }
                            #endregion
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = "Editado";
                        }
                    }
                }
                else
                {
                    if (!cEmplazamientos.EmplazamientoDuplicadoCodigoOperador(long.Parse(cmbOperadores.SelectedItem.Value), txtCodigo.Text))
                    {
                        #region ADD
                        DateTime? dateDeactivationSave = null;
                        if (dateDeactivation.SelectedDate > DateTime.MinValue)
                        {
                            dateDeactivationSave = dateDeactivation.SelectedDate;
                        }

                        foreach (var item in listaCategorias)
                        {
                            if (!item.GuardarValor(listaAtributos, cEmplazamientos.Context))
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                        }

                        if (geoEmplazamiento.Municipio != "")
                        {
                            lPaisID = cMunicipios.getPaisByMunicipio(geoEmplazamiento.Municipio.Split(',')[0]);

                            if (lPaisID != 0)
                            {
                                lMunicipioID = cMunicipios.GetMunicipioByNombre(geoEmplazamiento.Municipio.Split(',')[0]);
                            }
                        }

                        ResponseCreateController responseCreate = cEmplazamientos.CreateSite(false, null, oUser, lCliID,
                            txtCodigo.Text,
                            txtNombre.Text,
                            Convert.ToInt64(cmbOperadores.SelectedItem.Value),
                            Convert.ToInt64(cmbEstadosGlobales.SelectedItem.Value),
                            Convert.ToInt64(cmbMonedas.SelectedItem.Value),
                            Convert.ToInt64(cmbCategorias.SelectedItem.Value),
                            Convert.ToInt64(cmbTipos.SelectedItem.Value),
                            (cmbTiposEstructuras.Value != null && cmbTiposEstructuras.Value.ToString() != "") ? Convert.ToInt64(cmbTiposEstructuras.Value) : 0,
                            Convert.ToInt64(cmbTiposEdificios.SelectedItem.Value),
                            (cmbTamanos.Value != null && cmbTamanos.Value.ToString() != "") ? Convert.ToInt64(cmbTamanos.Value) : 0,
                            dateActivation.SelectedDate,
                            dateDeactivationSave,
                            (long)lPaisID,
                            lMunicipioID,
                            geoEmplazamiento.Direccion,
                            geoEmplazamiento.Barrio,
                            geoEmplazamiento.CodigoPostal,
                            Convert.ToDouble(geoEmplazamiento.Longitud),
                            Convert.ToDouble(geoEmplazamiento.Latitud),
                            listaAtributos);

                        if (responseCreate.Data == null || responseCreate.infoResponse.Data == null)
                        {
                            direct.Success = false;
                            direct.Result = GetLocalResourceObject("strErrorGenerarHistorico").ToString();
                            return direct;
                        }
                        else
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                            Data.Emplazamientos oEmp = cEmplazamientos.GetByCodigo(txtCodigo.Text);
                            if (oEmp != null)
                            {
                                hdEmplazamientoID.Value = oEmp.EmplazamientoID.ToString();
                            }

                            if (hdCondicionReglaID.Value.ToString() != "")
                            {
                                GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
                                if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionReglaID.Value.ToString()), txtCodigo.Text))
                                {
                                    direct.Success = false;
                                    direct.Result = GetLocalResourceObject("strErrorActualizarCodigoAutomatico").ToString();
                                    return direct;
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        hdCodigoDuplicado.SetValue("Duplicado");
                        direct.Success = false;
                        direct.Result = "Codigo";
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


            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar(long lSeleccionado)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            PaisesController cPaises = new PaisesController();
            MunicipiosController cMunicipios = new MunicipiosController();

            EmplazamientosAtributosJsonController cAtributos = new EmplazamientosAtributosJsonController();
            HistoricosCoreEmplazamientosController cHistorico = new HistoricosCoreEmplazamientosController();
            Data.HistoricosCoreEmplazamientos DatoHistorico;
            Data.Paises oPais;
            string sMunicipio = "";
            string sProvincia = "";

            try
            {
                direct.Success = true;

                Data.Emplazamientos oDato = cEmplazamientos.GetItem(lSeleccionado);
                hdEmplazamientoID.Value = oDato.EmplazamientoID.ToString();
                DatoHistorico = cHistorico.getHistoricoByID(oDato.EmplazamientoID);

                if (DatoHistorico != null)
                {
                    hdHistoricoEmplazamiento.Value = DatoHistorico.HistoricoCoreEmplazamientoID.ToString();
                }
                else
                {
                    hdHistoricoEmplazamiento.Value = "";
                }

                #region DATOS PRINCIPALES

                txtCodigo.Text = oDato.Codigo;
                txtNombre.Text = oDato.NombreSitio;
                cmbOperadores.SetValue(oDato.EntidadID);
                cmbMonedas.SetValue(oDato.MonedaID);
                cmbEstadosGlobales.SetValue(oDato.EstadoGlobalID);
                cmbCategorias.SetValue(oDato.CategoriaEmplazamientoID);
                cmbTipos.SetValue(oDato.EmplazamientoTipoID);
                cmbTamanos.SetValue(oDato.EmplazamientoTamanoID);
                cmbTiposEstructuras.SetValue(oDato.EmplazamientoTipoEstructuraID);
                cmbTiposEdificios.SetValue(oDato.TipoEdificacionID);
                dateActivation.SelectedDate = (DateTime)oDato.FechaActivacion;

                if (oDato.FechaDesactivacion != null && oDato.FechaDesactivacion.Value != DateTime.MinValue)
                {
                    dateDeactivation.SelectedDate = (DateTime)oDato.FechaDesactivacion;
                }
                #endregion

                #region DATOS LOCALIZACIONES

                if (oDato.Latitud != 0 || oDato.Longitud != 0 || oDato.Direccion != ""
                    || oDato.Barrio != "" || oDato.CodigoPostal != "" || oDato.MunicipioID != null
                    || oDato.MunicipioID != 0)
                {
                    geoEmplazamiento.Latitud = oDato.Latitud.ToString();
                    geoEmplazamiento.Longitud = oDato.Longitud.ToString();
                    geoEmplazamiento.Direccion = oDato.Direccion;
                    geoEmplazamiento.Barrio = oDato.Barrio;
                    geoEmplazamiento.CodigoPostal = oDato.CodigoPostal;

                    oPais = cPaises.GetPaisByMunicipioID((long)oDato.MunicipioID);
                    sMunicipio = cMunicipios.GetMunicipioByID(oDato.MunicipioID);
                    sProvincia = cMunicipios.getNombreProvinciaByMunicipioID(sMunicipio);
                    geoEmplazamiento.Municipio = sMunicipio + ", " + sProvincia + " (" + oPais.PaisCod + ")";
                }
                #endregion

                #region DATOS ADICIONALES

                List<object> listaValoresAtributos = new List<object>();
                JsonObject jsDatos = new JsonObject();

                foreach (var oAtr in cAtributos.Deserializacion(oDato.JsonAtributosDinamicos))
                {
                    listaValoresAtributos.Add(new
                    {
                        AtributoID = oAtr.AtributoID,
                        Valor = oAtr.Valor
                    });
                }

                if (listaValoresAtributos.Count > 0)
                {
                    foreach (var item in listaCategorias)
                    {
                        item.MostrarEditar(listaValoresAtributos, jsDatos);
                    }
                }
                direct.Result = jsDatos;

                #endregion
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

        private class objCategoriaAtributo
        {
            public string ID;
            public long CategoriaAtributoID;
            public string Nombre;
            public long Orden;
            public long Modulo;
            public bool EsSubCategoria;
        }

        public void CargarListaCategorias() {
            try
            {
                EmplazamientosAtributosCategoriasController cCategorias = new EmplazamientosAtributosCategoriasController();
                TreeCore.Componentes.GestionCategoriasAtributos oComponente;
                long cliID = long.Parse(hdCliID.Value.ToString());
                listaCategorias = new List<GestionCategoriasAtributos>();
                List<Data.EmplazamientosAtributosCategorias> listaAtributos = cCategorias.getCategoriasSeleccionadas(cliID);
                foreach (var idCate in listaAtributos)
                {
                    oComponente = (GestionCategoriasAtributos)this.LoadControl("/Componentes/GestionCategoriasAtributos.ascx");
                    oComponente.ID = "CAT" + idCate.EmplazamientoAtributoCategoriaID;
                    oComponente.CategoriaAtributoID = idCate.EmplazamientoAtributoCategoriaID;
                    oComponente.Nombre = idCate.Nombre;
                    oComponente.Orden = cCategorias.GetOrdenCategoria(idCate.EmplazamientoAtributoCategoriaID, cliID);
                    oComponente.Modulo = (long)Comun.Modulos.GLOBAL;
                    listaCategorias.Add(oComponente);
                }
                listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod()]
        public DirectResponse PintarCategorias(bool Update)
        {
            EmplazamientosAtributosCategoriasController cCategorias = new EmplazamientosAtributosCategoriasController();
            TreeCore.Componentes.GestionCategoriasAtributos oComponente;
            Data.EmplazamientosAtributosCategorias oDato;
            long cliID = long.Parse(hdCliID.Value.ToString());
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (listaCategorias == null || listaCategorias.Count == 0 || Update)
                {
                    #region No hay hd
                    if (X.GetCmp("hdPageLoad") == null)
                    {
                        CargarListaCategorias();
                    }
                    #endregion
                    else
                    {
                        Hidden hdPL = (Hidden)X.GetCmp("hdPageLoad");
                        hdPageLoadController hdController = new hdPageLoadController(hdPL);
                        string oValor = hdController.GetValor(System.IO.Path.GetFileName(Request.Url.AbsolutePath));
                        if (oValor == "")
                        {
                            CargarListaCategorias();
                            List<objCategoriaAtributo> listaObjCategorias = new List<objCategoriaAtributo>();
                            foreach (var oCat in listaCategorias)
                            {
                                listaObjCategorias.Add(new objCategoriaAtributo
                                {
                                    ID = oCat.ID,
                                    CategoriaAtributoID = oCat.CategoriaAtributoID,
                                    Nombre = oCat.Nombre,
                                    Orden = oCat.Orden,
                                    Modulo = oCat.Modulo,
                                    EsSubCategoria = oCat.EsSubCategoria
                                });
                            }
                            oValor = JsonConvert.SerializeObject(listaObjCategorias);
                            hdController.SetValor(System.IO.Path.GetFileName(Request.Url.AbsolutePath), oValor);
                        }
                        else
                        {
                            var listaObjCategorias = JsonConvert.DeserializeObject<List<objCategoriaAtributo>>(oValor);
                            listaCategorias = new List<GestionCategoriasAtributos>();
                            foreach (var oCat in listaObjCategorias)
                            {
                                oComponente = (GestionCategoriasAtributos)this.LoadControl("/Componentes/GestionCategoriasAtributos.ascx");
                                oComponente.ID = oCat.ID;
                                oComponente.CategoriaAtributoID = oCat.CategoriaAtributoID;
                                oComponente.Nombre = oCat.Nombre;
                                oComponente.Orden = oCat.Orden;
                                oComponente.Modulo = oCat.Modulo;
                                oComponente.EsSubCategoria = oCat.EsSubCategoria;
                                listaCategorias.Add(oComponente);
                            }
                        }
                        listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();

                        if (contenedorCategorias != null && contenedorCategorias.ContentControls != null && contenedorCategorias.ContentControls.Count > 0)
                        {
                            contenedorCategorias.ContentControls.Clear();
                        }
                        if (contenedorCategorias != null && contenedorCategorias.ContentControls != null)
                        {
                            foreach (var item in listaCategorias)
                            {
                                contenedorCategorias.ContentControls.Add(item);
                            }
                        }
                    }

                }
                else
                {
                    if (listaCategorias != null)
                    {
                        listaCategorias.Clear();
                    }
                    if (contenedorCategorias != null)
                    {
                        contenedorCategorias.ContentControls.Clear();
                    }
                }
                if (Update)
                {
                    if (listaCategorias != null && listaCategorias.Count != 0)
                    {
                        foreach (var item in listaCategorias)
                        {
                            item.PintarAtributos(true);
                        }
                    }
                    contenedorCategorias.UpdateContent();
                }

                EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cController = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaAllowBlank = cController.getAtributosByNombre("Allow Blank");

                if (listaAllowBlank.Count > 0)
                {
                    hdAllowBlank.Value = true;
                }
                else
                {
                    hdAllowBlank.Value = false;
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

        [DirectMethod()]
        public DirectResponse ComprobarEmplazamientoExiste()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmp = new EmplazamientosController();

            try
            {
                if (cEmp.EmplazamientoDuplicadoCodigoOperador(long.Parse(cmbOperadores.SelectedItem.Value), txtCodigo.Text))
                {
                    if (hdEmplazamientoID.Value != null && hdEmplazamientoID.Value.ToString() != "")
                    {
                        string sCodigo = cEmp.GetItem(long.Parse(hdEmplazamientoID.Value.ToString())).Codigo;

                        if (sCodigo == txtCodigo.Text)
                        {
                            direct.Success = false;
                            direct.Result = "Editado";
                        }
                        else
                        {
                            Data.Emplazamientos oDato = cEmp.GetByCodigo(sCodigo);

                            if (oDato != null)
                            {
                                hdCodigoDuplicado.SetValue("Duplicado");
                                direct.Success = false;
                                direct.Result = "Codigo";
                            }
                        }
                    }
                    else
                    {
                        hdCodigoDuplicado.SetValue("Duplicado");
                        direct.Success = false;
                        direct.Result = "Codigo";
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = "";
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #region GENERACION AUTOMÁTICA CÓDIGO

        [DirectMethod()]
        public DirectResponse ComprobarCodigoEmplazamientoDuplicado()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.GlobalCondicionesReglas aplicarRegla;
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            try
            {
                #region COMPROBAR CODIGO
                if (cEmplazamientos.CodigoDuplicadoGeneradorCodigos(hdCodigoEmplazamientoAutogenerado.Value.ToString()))
                {

                    hdCodigoDuplicado.Value = "Duplicado";
                }
                else
                {
                    hdCodigoDuplicado.Value = "No_Duplicado";
                }
                #endregion

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
        public DirectResponse GenerarCodigoEmplazamiento()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            long lCliID = 0;

            try
            {
                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                Data.GlobalCondicionesReglas aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("CODIGO_EMPLAZAMIENTO", (long)Comun.Modulos.GLOBAL);

                if (aplicarRegla != null)
                {
                    configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                    if (configuraciones != null && configuraciones.Count > 0)
                    {
                        string siguienteCodigo;
                        siguienteCodigo = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, lCliID);

                        if (siguienteCodigo == null)
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                            return direct;
                        }
                        else
                        {
                            txtCodigo.SetValue(siguienteCodigo);

                            hdCodigoEmplazamientoAutogenerado.SetValue(siguienteCodigo);
                            hdCondicionReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);

                            JsonObject listaIDs = new JsonObject();
                            direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID, listaIDs);
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                        return direct;
                    }

                }
                //else
                //{
                //    direct.Success = false;
                //    direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                //    return direct;
                //}
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion

        #endregion

        #endregion

        #region Function

        [DirectMethod()]
        public DirectResponse CargarStores()
        {
            DirectResponse direct = new DirectResponse();
            JsonObject listas, lista, jsonAux, defectos;
            try
            {
                direct.Success = true;
                listas = new JsonObject();
                lista = new JsonObject();
                defectos = new JsonObject();

                long cliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                #region Operadores

                lista = new JsonObject();

                EntidadesController cOperadores = new EntidadesController();

                foreach (var oDato in cOperadores.getEntidadesOperadores(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EntidadID", oDato.EntidadID);
                    jsonAux.Add("Nombre", oDato.Nombre);
                    jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EntidadID.ToString(), jsonAux);
                }

                listas.Add("storeOperadores", lista);

                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

                long? EntidadCliente = cOperadores.getOperadorCliente(oUsuario.UsuarioID);
                if (EntidadCliente != null)
                {
                    defectos.Add("cmbOperadores", EntidadCliente);
                }

                #endregion

                #region EstadosGlobales

                lista = new JsonObject();

                EstadosGlobalesController cEstados = new EstadosGlobalesController();

                foreach (var oDato in cEstados.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EstadoGlobalID", oDato.EstadoGlobalID);
                    jsonAux.Add("EstadoGlobal", oDato.EstadoGlobal);
                    //jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EstadoGlobalID.ToString(), jsonAux);
                }
                listas.Add("storeEstadosGlobales", lista);

                Data.EstadosGlobales oEstadoGlobal = cEstados.GetDefault(cliID);
                if (oEstadoGlobal != null)
                {
                    defectos.Add("cmbEstadosGlobales", oEstadoGlobal.EstadoGlobalID);
                }

                #endregion

                #region Moneda

                lista = new JsonObject();

                MonedasController cMonedas = new MonedasController();

                foreach (var oDato in cMonedas.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("MonedaID", oDato.MonedaID);
                    jsonAux.Add("Moneda", oDato.Moneda);
                    jsonAux.Add("Simbolo", oDato.Simbolo);
                    //jsonAux.Add("Codigo", oDato.);
                    lista.Add(oDato.MonedaID.ToString(), jsonAux);
                }
                listas.Add("storeMonedas", lista);

                Data.Monedas oMonedas = cMonedas.GetDefault(cliID);
                if (oMonedas != null)
                {
                    defectos.Add("cmbMonedas", oMonedas.MonedaID);
                }

                #endregion

                #region EmplazamientoCategoriaSitio

                lista = new JsonObject();

                EmplazamientosCategoriasSitiosController cEmpCategorias = new EmplazamientosCategoriasSitiosController();

                foreach (var oDato in cEmpCategorias.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EmplazamientoCategoriaSitioID", oDato.EmplazamientoCategoriaSitioID);
                    jsonAux.Add("CategoriaSitio", oDato.CategoriaSitio);
                    jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EmplazamientoCategoriaSitioID.ToString(), jsonAux);
                }
                listas.Add("storeCategorias", lista);

                Data.EmplazamientosCategoriasSitios oEmplCategorias = cEmpCategorias.GetDefault(cliID);
                if (oEmplCategorias != null)
                {
                    defectos.Add("cmbCategorias", oEmplCategorias.EmplazamientoCategoriaSitioID);
                }

                #endregion

                #region EmplazamientoTipo

                lista = new JsonObject();

                EmplazamientosTiposController cTipos = new EmplazamientosTiposController();

                foreach (var oDato in cTipos.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EmplazamientoTipoID", oDato.EmplazamientoTipoID);
                    jsonAux.Add("Tipo", oDato.Tipo);
                    jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EmplazamientoTipoID.ToString(), jsonAux);
                }
                listas.Add("storeTipos", lista);

                Data.EmplazamientosTipos oTipos = cTipos.GetDefault(cliID);
                if (oTipos != null)
                {
                    defectos.Add("cmbTipos", oTipos.EmplazamientoTipoID);
                }

                #endregion

                #region EmplazamientoTipoEdificio

                lista = new JsonObject();

                EmplazamientosTiposEdificiosController cTiposEdificion = new EmplazamientosTiposEdificiosController();

                foreach (var oDato in cTiposEdificion.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EmplazamientoTipoEdificioID", oDato.EmplazamientoTipoEdificioID);
                    jsonAux.Add("TipoEdificio", oDato.TipoEdificio);
                    jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EmplazamientoTipoEdificioID.ToString(), jsonAux);
                }
                listas.Add("storeTiposEdificios", lista);

                Data.EmplazamientosTiposEdificios oTiposEdificion = cTiposEdificion.GetDefault(cliID);
                if (oTiposEdificion != null)
                {
                    defectos.Add("cmbTiposEdificios", oTiposEdificion.EmplazamientoTipoEdificioID);
                }

                #endregion

                #region EmplazamientoTipoEstructura

                lista = new JsonObject();

                EmplazamientosTiposEstructurasController cTiposEstructura = new EmplazamientosTiposEstructurasController();

                foreach (var oDato in cTiposEstructura.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EmplazamientoTipoEstructuraID", oDato.EmplazamientoTipoEstructuraID);
                    jsonAux.Add("TipoEstructura", oDato.TipoEstructura);
                    jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EmplazamientoTipoEstructuraID.ToString(), jsonAux);
                }
                listas.Add("storeTiposEstructuras", lista);

                Data.EmplazamientosTiposEstructuras oTiposEstructura = cTiposEstructura.GetDefault(cliID);
                if (oTiposEstructura != null)
                {
                    defectos.Add("cmbTiposEstructuras", oTiposEstructura.EmplazamientoTipoEstructuraID);
                }

                #endregion

                #region EmplazamientoTamano

                lista = new JsonObject();

                EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();

                foreach (var oDato in cTamanos.GetActivos(cliID))
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("EmplazamientoTamanoID", oDato.EmplazamientoTamanoID);
                    jsonAux.Add("Tamano", oDato.Tamano);
                    //jsonAux.Add("Codigo", oDato.Codigo);
                    lista.Add(oDato.EmplazamientoTamanoID.ToString(), jsonAux);
                }
                listas.Add("storeTamanos", lista);

                Data.EmplazamientosTamanos oTamano = cTamanos.GetDefault(cliID);
                if (oTamano != null)
                {
                    defectos.Add("cmbTamanos", oTamano.EmplazamientoTamanoID);
                }

                #endregion

                jsonAux = new JsonObject();

                jsonAux.Add("listas", listas);
                jsonAux.Add("defectos", defectos);

                direct.Result = jsonAux;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarInfoEmplazamiento(long lEmplazamientoID)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
            EmplazamientosAtributosJsonController cAtrJson = new EmplazamientosAtributosJsonController();
            Data.Emplazamientos oEmplazamiento;
            JsonObject resultado, jsonAux;
            try
            {
                direct.Success = true;
                resultado = new JsonObject();
                oEmplazamiento = cEmplazamientos.GetItem(lEmplazamientoID);

                #region General

                jsonAux = new JsonObject();

                jsonAux.Add(GetGlobalResource("strNombre"), oEmplazamiento.NombreSitio);
                jsonAux.Add(GetGlobalResource("strCodigo"), oEmplazamiento.Codigo);
                jsonAux.Add(GetGlobalResource("strOperador"), oEmplazamiento.Entidades.Nombre);
                jsonAux.Add(GetGlobalResource("strEstadoGlobal"), oEmplazamiento.EstadosGlobales.EstadoGlobal);
                jsonAux.Add(GetGlobalResource("strMoneda"), oEmplazamiento.Monedas.Moneda);
                jsonAux.Add(GetGlobalResource("strCategoria"), oEmplazamiento.EmplazamientosCategoriasSitios.CategoriaSitio);
                jsonAux.Add(GetGlobalResource("strTipo"), oEmplazamiento.EmplazamientosTipos.Tipo);
                jsonAux.Add(GetGlobalResource("strTipoEdificio"), oEmplazamiento.EmplazamientosTiposEdificios.TipoEdificio);
                jsonAux.Add(GetGlobalResource("strFechaActivacion"), oEmplazamiento.FechaActivacion.Value.ToString(Comun.FORMATO_FECHA));

                if (oEmplazamiento.FechaDesactivacion != null)
                {
                    jsonAux.Add(GetGlobalResource("strFechaDesactivacion"), oEmplazamiento.FechaDesactivacion.Value.ToString(Comun.FORMATO_FECHA));
                }
                else
                {
                    jsonAux.Add(GetGlobalResource("strFechaDesactivacion"), "");
                }

                if (oEmplazamiento.EmplazamientoTipoEstructuraID != null)
                {
                    jsonAux.Add(GetGlobalResource("strTipoEstructura"), oEmplazamiento.EmplazamientosTiposEstructuras.TipoEstructura);
                }
                else
                {
                    jsonAux.Add(GetGlobalResource("strTipoEstructura"), "");
                }
                if (oEmplazamiento.EmplazamientoTamanoID != null)
                {
                    jsonAux.Add(GetGlobalResource("strTamano"), oEmplazamiento.EmplazamientosTamanos.Tamano);
                }
                else
                {
                    jsonAux.Add(GetGlobalResource("strTamano"), "");
                }

                resultado.Add("General", jsonAux);

                #endregion

                #region Location

                jsonAux = new JsonObject();

                jsonAux.Add(GetGlobalResource("strDireccion"), oEmplazamiento.Direccion);
                jsonAux.Add(GetGlobalResource("strMunicipiosProvincias"), oEmplazamiento.Municipios.Municipio);
                jsonAux.Add(GetGlobalResource("strBarrio"), oEmplazamiento.Barrio);
                jsonAux.Add(GetGlobalResource("strCodigoPostal"), oEmplazamiento.CodigoPostal);
                jsonAux.Add(GetGlobalResource("strLatitud"), oEmplazamiento.Latitud);
                jsonAux.Add(GetGlobalResource("strLongitud"), oEmplazamiento.Longitud);

                resultado.Add("Localizacion", jsonAux);

                #endregion

                #region Adicional

                jsonAux = new JsonObject();
                var listaAtrVisibles = cAtributosConf.GetAtributosVisibles(long.Parse(hdCliID.Value.ToString()), Usuario.UsuarioID);
                if (oEmplazamiento.JsonAtributosDinamicos != null && oEmplazamiento.JsonAtributosDinamicos != "")
                {
                    foreach (var valor in cAtrJson.Deserializacion(oEmplazamiento.JsonAtributosDinamicos))
                    {
                        if (listaAtrVisibles.Select(c => c.EmplazamientoAtributoConfiguracionID).ToList().Contains(valor.AtributoID))
                        {
                            try
                            {
                                var atr = cAtributosConf.GetItem(long.Parse(valor.AtributoID.ToString()));
                                if (valor.TipoDato != null && (valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA || valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                                {
                                    string sValor = "";
                                    JsonObject listaItems = new JsonObject();
                                    JsonObject auxJson;
                                    dynamic auxDina;
                                    if (atr.TablaModeloDatoID != null)
                                    {
                                        listaItems = cAtributosConf.GetJsonItemsComboBoxByColumnaModeloDatosID(atr.EmplazamientoAtributoConfiguracionID);
                                    }
                                    else if (atr.FuncionControlador != null && atr.FuncionControlador != "")
                                    {
                                        listaItems = cAtributosConf.GetJsonItemsComboBoxByFuncion(atr.FuncionControlador, null, null, atr.EmplazamientoAtributoConfiguracionID);
                                    }
                                    else if (atr.ValoresPosibles != null && atr.ValoresPosibles != "")
                                    {
                                        listaItems = new JsonObject();
                                        foreach (var val in atr.ValoresPosibles.Split(';'))
                                        {
                                            try
                                            {
                                                auxJson = new JsonObject();
                                                auxJson.Add("Value", val);
                                                auxJson.Add("Text", val);
                                                listaItems.Add(val, auxJson);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }
                                    }
                                    if (listaItems != null)
                                    {
                                        string Auxstr;
                                        if (valor.Valor.ToString() != "")
                                        {
                                            foreach (var val in valor.Valor.ToString().Split(','))
                                            {
                                                if (listaItems.TryGetValue(val, out auxDina))
                                                {
                                                    foreach (var aux in auxDina)
                                                    {
                                                        if (aux.Key == "Text")
                                                        {
                                                            sValor += ", " + aux.Value;
                                                        }
                                                    }
                                                }
                                            }
                                            sValor = sValor.Remove(0, 2);
                                        }
                                        else
                                        {
                                            sValor = valor.Valor.ToString();
                                        }
                                    }
                                    jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, sValor);
                                }
                                else
                                {
                                    if (valor.NombreAtributo != null)
                                    {
                                        if (atr != null)
                                        {
                                            jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, valor.Valor.ToString());
                                        }
                                        else
                                        {
                                            jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, valor.Valor.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, valor.Valor.ToString());
                            }
                        }
                    }
                }

                resultado.Add("Adicional", jsonAux);

                #endregion

                direct.Result = resultado;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse CargarDatosElementos(long ElementoID)
        {
            DirectResponse direct = new DirectResponse();
            InventarioElementosController cElementos = new InventarioElementosController();
            Data.Vw_InventarioElementosReducida oElemento;
            Data.InventarioElementos oElementoObj;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCatConf;
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            CoreInventarioElementosAtributosController cAtributos = new CoreInventarioElementosAtributosController();
            InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            List<Data.InventarioElementos> listaElementosVinculados;
            List<long> listaPlaIds;
            string sValor = "";
            JsonObject oDato = new JsonObject();
            try
            {
                oElemento = cElementos.GetElementoVistaReducida(ElementoID);
                oElementoObj = cElementos.GetItem(ElementoID);
                oDato.Add(GetGlobalResource("strCodigo"), oElemento.NumeroInventario);
                oDato.Add(GetGlobalResource("strNombre"), oElemento.Nombre);
                if (!oElemento.Plantilla)
                {
                    oDato.Add(GetGlobalResource("strEmplazamiento"), oElemento.Codigo);
                    oDato.Add(GetGlobalResource("strOperador"), oElemento.NombreEntidad);
                    int ContCodigo = 1;
                    oDato.Add(GetGlobalResource("strEstado"), oElemento.EstadoInventarioElemento);
                }

                CoreAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new CoreAtributosConfiguracionRolesRestringidosController();
                List<Data.Vw_CoreAtributosConfiguracionRolesRestringidos> listaRestriccionRoles;

                #region Cargas Listas

                CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();

                JsonObject listasItems = new JsonObject();
                JsonObject listaItems = new JsonObject();
                JsonObject auxJson;

                foreach (var oAtr in cCategoriasVin.GetAtributosByInventarioCategoriaID(oElementoObj.InventarioCategoriaID))
                {
                    if (oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA || oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        if (oAtr.TablaModeloDatoID != null)
                        {
                            listaItems = cAtributosConf.GetJsonItemsComboBox((long)oAtr.CoreAtributoConfiguracionID);

                        }
                        else if (oAtr.ValoresPosibles != null && oAtr.ValoresPosibles != "")
                        {
                            listaItems = new JsonObject();
                            foreach (var val in oAtr.ValoresPosibles.Split(';'))
                            {
                                try
                                {
                                    auxJson = new JsonObject();
                                    auxJson.Add("Value", val);
                                    auxJson.Add("Text", val);
                                    listaItems.Add(val, auxJson);
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        listasItems.Add(oAtr.CoreAtributoConfiguracionID.ToString(), listaItems);
                    }
                }

                #endregion

                var listaAtrVis = cCategoriasVin.GetAtributosVisiblesByInventarioCategoriaID(oElementoObj.InventarioCategoriaID, Usuario.UsuarioID);
                foreach (var oAtr in cAtrJson.Deserializacion(oElementoObj.JsonAtributosDinamicos))
                {
                    try
                    {
                        if (oAtr.TipoDato != null && (oAtr.TipoDato == Comun.TIPODATO_CODIGO_LISTA || oAtr.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                        {
                            sValor = "";
                            dynamic auxDina;
                            if (listasItems.TryGetValue(oAtr.AtributoID.ToString(), out auxDina))
                            {
                                listaItems = (JsonObject)auxDina;
                                string Auxstr;
                                if (oAtr.Valor.ToString() != "")
                                {
                                    foreach (var val in oAtr.Valor.ToString().Split(','))
                                    {
                                        if (listaItems.TryGetValue(val, out auxDina))
                                        {
                                            foreach (var aux in auxDina)
                                            {
                                                if (aux.Key == "Text")
                                                {
                                                    sValor += ", " + aux.Value;
                                                }
                                            }
                                        }
                                    }
                                    sValor = sValor.Remove(0, 2);
                                }
                                else
                                {
                                    sValor = oAtr.Valor.ToString();
                                }
                            }
                            else
                            {
                                sValor = oAtr.Valor.ToString();
                            }
                        }
                        else if (oAtr.TipoDato != null && oAtr.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                        {
                            if (oAtr.Valor != "")
                            {
                                sValor = DateTime.Parse(oAtr.Valor).ToString(Comun.FORMATO_FECHA);
                            }
                            else
                            {
                                sValor = oAtr.Valor;
                            }
                        }
                        else
                        {
                            sValor = oAtr.Valor;
                        }

                        if (listaAtrVis.Select(c => c.CoreAtributoConfiguracionID).Contains(oAtr.AtributoID))
                        {
                            oDato.Add(listaAtrVis.Where(c => c.CoreAtributoConfiguracionID == oAtr.AtributoID).First().Codigo, sValor);
                        }

                    }
                    catch (Exception)
                    {
                        //jsonAux.Add(oAtr.AtributoID, valor.Valor);
                    }
                }
                listaPlaIds = new List<long>();
                foreach (var oPla in cPlaJson.Deserializacion(oElementoObj.JsonPlantillas))
                {
                    if (oPla.NombreCategoria != "" && oPla.NombreCategoria != null)
                    {
                        oDato.Add(oPla.NombreCategoria, oPla.NombrePlantilla);
                    }
                    else
                    {
                        oCatConf = cCatConf.GetItem(oPla.InvCatConfID);
                        oDato.Add(oCatConf.InventarioAtributosCategorias.InventarioAtributoCategoria, oPla.NombrePlantilla);
                    }
                    listaPlaIds.Add(oPla.PlantillaID);
                }
                direct.Success = true;
                direct.Result = oDato;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }


            return direct;

        }


        [DirectMethod]
        public DirectResponse CargarDatosDocumento(long docID)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDoc = new DocumentosController();
            Vw_CoreDocumentos doc;
            JsonObject oDato = new JsonObject();
            try
            {
                doc = cDoc.GetItem<Vw_CoreDocumentos>(docID);
                if (doc != null)
                {
                    cDoc.GetObjectDataByDocumentID(doc.DocumentoID, out long? id, out string tipo);
                    string sObjectCode = cDoc.GetObjectCodeByObjectID(id, tipo);
                    string sObjectName = cDoc.GetObjectNameByObjectID(id, tipo);

                    Documentos docum = cDoc.GetItem(doc.DocumentoID);
                    string tipoObj = cDoc.getObjetoTipo(docum);

                    oDato.Add(GetGlobalResource("strNombre"), doc.Documento);
                    oDato.Add(GetGlobalResource("strExtension"), doc.Extension);
                    oDato.Add(GetGlobalResource("strEstado"), doc.NombreEstado);

                    if (tipoObj == Comun.ObjetosTipos.Emplazamiento)
                    {
                        oDato.Add(GetGlobalResource("strObjetoTipo"), GetGlobalResource("strEmplazamiento"));
                    }
                    else if (tipoObj == Comun.ObjetosTipos.InventarioElemento)
                    {
                        oDato.Add(GetGlobalResource("strObjetoTipo"), GetGlobalResource("strInventario"));
                    }

                    oDato.Add(GetGlobalResource("strCodigoObjeto"), sObjectCode);
                    oDato.Add(GetGlobalResource("strNombreObjeto"), sObjectName);
                    oDato.Add(GetGlobalResource("strCreador"), doc.Creador);
                    oDato.Add(GetGlobalResource("strVersion_"), doc.Version);
                    //oDato.Add(GetGlobalResource("strProyectoTipo"), doc.Alias);
                    //oDato.Add(GetGlobalResource("strProyecto"), doc.Proyecto);
                    if (doc.FechaDocumento.HasValue)
                    {
                        oDato.Add(GetGlobalResource("strFecha"), Comun.DateTimeToString(doc.FechaDocumento.Value));
                        oDato.Add(GetGlobalResource("strHora"), doc.FechaDocumento.Value.Hour.ToString() + ":" + doc.FechaDocumento.Value.Minute.ToString());
                    }
                    if (doc.Tamano.HasValue)
                    {
                        oDato.Add(GetGlobalResource("strTamano"), SizeSuffix(doc.Tamano.Value));
                    }

                    Documentos docTem = cDoc.GetItem(doc.DocumentoID);
                    if (docTem != null && !string.IsNullOrEmpty(docTem.Observaciones))
                    {
                        oDato.Add(GetGlobalResource("strDescripcion"), docTem.Observaciones);
                    }


                    direct.Success = true;
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

        private static string SizeSuffix(Int64 value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        [DirectMethod]
        public DirectResponse CargaDatosContacto(long cotID)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesController cContactos = new ContactosGlobalesController();
            Data.ContactosGlobales oContacto;
            JsonObject oDato = new JsonObject();
            try
            {
                oContacto = cContactos.GetItem(cotID);
                oDato.Add(GetGlobalResource("strNombre"), oContacto.Nombre);
                oDato.Add(GetGlobalResource("strApellidos"), oContacto.Apellidos);
                oDato.Add(GetGlobalResource("strTelefono"), oContacto.Telefono);
                oDato.Add(GetGlobalResource("strTelefono" + " 2"), oContacto.Telefono2);
                oDato.Add(GetGlobalResource("strEmail"), oContacto.Email);
                oDato.Add(GetGlobalResource("strDireccion"), oContacto.Direccion);
                oDato.Add(GetGlobalResource("strProvincia"), oContacto.Municipios.Provincias.Provincia);
                oDato.Add(GetGlobalResource("strMunicipio"), oContacto.Municipios.Municipio);
                oDato.Add(GetGlobalResource("strTipo"), oContacto.ContactosTipos.ContactoTipo);
                direct.Success = true;
                direct.Result = oDato;
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