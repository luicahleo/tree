using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;
using TreeCore.Componentes;
using TreeCore.Data;
using TreeCore.Page;

namespace TreeCore.ModGlobal.pages
{

    public partial class Emplazamientos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;
        //private const string constFilter = "f.";
        //private const string constFilterView = "e.";
        private const string operatorAND = " AND ";
        Data.Usuarios oUser;
        GridPanel panel = null;


        #region GESTION PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.SetValue(ClienteID);
                    hdCliID.DataBind();
                }
            }

            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            UsuariosController cUsuarios = new UsuariosController();

            if (oUsuario != null)
            {
                oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
            }

            #region REGISTRO DE ESTADISTICAS

            EstadisticasController cEstadisticasMod = new EstadisticasController();
            cEstadisticasMod.EscribeEstadisticaAccion(oUser.UsuarioID, oUser.ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
            log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

            #endregion


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
                        string sTabla = Request.QueryString["aux"];
                        string sVista = Request.QueryString["aux3"];
                        int iCount = 0;
                        Hidden CliID = (Hidden)X.GetCmp("hdCliID");

                        #region ESTADISTICAS
                        try
                        {
                            this.LoadUserControl(sTabla, sVista);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(oUser.UsuarioID, oUser.ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, Comun.DefaultLocale);
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
            }
            #endregion

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CurrentControl.Text))
            {
                this.LoadUserControl(hdTabName.Value.ToString(), hdPageName.Value.ToString());
            }

            // SAMPLE DATA
            this.grdLocations.Store.Primary.DataSource = DataLocations;
            this.GridIncluirenProyectos.Store.Primary.DataSource = DataProjectosIncluidos;
            this.gridIncludedProjects.Store.Primary.DataSource = DataProjectosIncluidosAdded;

        }

        private List<Data.Vw_CoreEmplazamientos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_CoreEmplazamientos> listaDatos;
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cEmplazamientos.GetItemsWithExtNetFilterList<Data.Vw_CoreEmplazamientos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region SAMPLE DATA WINDOWS / GRIDS


        public static List<object> DataLocations
        {
            get
            {
                List<object> Locations = new List<object>
            {
                new
                {
                    Id = 1,
                    Direccion = "Gloria Fuerte",
                    Provincia = "Sevilla",
                },

               new
                {
                    Id = 2,
                    Direccion = "Gloria Rio",
                    Provincia = "Granada",
                },

                   new
                {
                    Id = 3,
                    Direccion = "Santa Ana",
                    Provincia = "Málaga",
                },

            };

                return Locations;
            }
        }



        public static List<object> DataProjectosIncluidos
        {
            get
            {
                List<object> Projects = new List<object>
            {
                new
                {
                    Nombre = "Adquisitions 2020",
                    Tipo = "Maintenance",
                    SitioIncluido = true,
                },

                new
                {
                    Nombre = "Adquisitions 2020",
                    Tipo = "Maintenance",
                    SitioIncluido = true,
                },


                  new
                {
                    Nombre = "Adquisitions 2020",
                    Tipo = "Maintenance",
                    SitioIncluido = false,
                },


            };

                return Projects;
            }
        }



        public static List<object> DataProjectosIncluidosAdded
        {
            get
            {
                List<object> ProjectsAdded = new List<object>
            {
                new
                {
                    Codigo = "ERYUAW-REK",
                    Proceso = "Proceso 1",
                },

                new
                {
                    Codigo = "ASDHKAS-R",
                      Proceso = "Proceso 2",

                },


                  new
                {
                    Codigo = "AWDAWDJA-43",
                    Proceso = "Proceso 3",
                },


            };

                return ProjectsAdded;
            }
        }



        #endregion

        #region LOADER PARA NAV BAR SUPERIOR

        [DirectMethod()]
        public DirectResponse LoadUserControl(string tabName, string nombre, bool update = false)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                if (update && currentUC != null && hugeCt != null && hugeCt.ContentControls.Count > 0)
                {
                    this.hugeCt.ContentControls.Clear();
                }

                currentUC = (BaseUserControl)this.LoadControl(tabName);
                currentUC.ID = "UC" + nombre;
                this.hugeCt.ContentControls.Add(currentUC);

                if (update)
                {
                    CurrentControl.Text = nombre;
                    this.hugeCt.UpdateContent();
                }

                if (tabName == "../../Componentes/GridEmplazamientos.ascx")
                {
                    panel = ((GridEmplazamientos)currentUC).ComponetGrid;
                }
                if (tabName == "../../Componentes/GridEmplazamientosLocalizaciones.ascx")
                {
                    panel = ((GridEmplazamientosLocalizaciones)currentUC).ComponetGrid;
                }
                if (tabName == "../../Componentes/GridEmplazamientosAtributos.ascx")
                {
                    panel = ((GridEmplazamientosAtributos)currentUC).ComponetGrid;
                    //((GridEmplazamientosAtributos)currentUC).GenerarGridDinamico();
                }
                if (nombre == typeof(GridEmplazamientosContactos).Name)
                {
                    ((GridEmplazamientosContactos)currentUC).vista = "";
                }
                if (tabName == "../../Componentes/GridEmplazamientosInventarios.ascx")
                {
                    panel = ((GridEmplazamientosInventarios)currentUC).ComponetGrid;
                }
                if (tabName == "../../Componentes/GridEmplazamientosDocumentos.ascx")
                {
                    panel = ((GridEmplazamientosDocumentos)currentUC).ComponetGrid;
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

        #region Stores

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
                        cmbClientes.GetStore().DataSource = listaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
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

        #endregion

        #region DIRECT METHOD

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

                    if (cGestionFiltros.AddItem(newFiltro) != null)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
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

        private static DataTable RealizarConsulta(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado)
        {
            DataTable dt;
            try
            {
                string whereGeneral = string.Empty;
                string whereInventario = string.Empty;
                string whereDocumentos = string.Empty;
                string whereContactos = string.Empty;

                JObject json = JObject.Parse(filtrosAplicados);
                JArray arrayFiltrosAplicados = (JArray)json["items"];
                string tabSelected = (string)json["tab"];

                bool primerBloqueFiltro = true;

                #region Montar Where
                foreach (JObject filtroAplicado in arrayFiltrosAplicados)
                {
                    JArray arrayItemsFiltro = (JArray)filtroAplicado["filters"];
                    string queryTmpGeneral = string.Empty;
                    string queryTmpInventario = string.Empty;
                    string queryTmpDocumentos = string.Empty;
                    string queryTmpContactos = string.Empty;
                    bool primeraVueltaGeneral = true;
                    bool primeraVueltaInventario = true;
                    bool primeraVueltaDocumentos = true;
                    bool primeraVueltaContactos = true;

                    #region AND entre filtros
                    if (primerBloqueFiltro)
                    {
                        primerBloqueFiltro = false;
                    }
                    else
                    {
                        queryTmpGeneral += " AND ";
                    }
                    #endregion

                    foreach (JObject itemFiltro in arrayItemsFiltro)
                    {
                        string id = (string)itemFiltro["Id"];
                        //string name = (string)itemFiltro["Name"];
                        string value = (string)itemFiltro["Value"];
                        bool multi = (bool)itemFiltro["multi"];
                        string operador = (string)itemFiltro["operator"];
                        string typeData = (string)itemFiltro["typeData"];
                        //string valueLabel = (string)itemFiltro["valueLabel"];


                        string tab = Comun.columTagCoreEmplazamientosFiltros[id].tab;
                        string nameHeaderVw = Comun.columTagCoreEmplazamientosFiltros[id].nameHeaderVwTab;
                        string nameHeaderVwFilter = Comun.columTagCoreEmplazamientosFiltros[id].nameHeaderVwFilter;
                        string vista = Comun.columTagCoreEmplazamientosFiltros[id].vista;

                        #region concatenación de filtros
                        switch (vista)
                        {
                            case Comun.VISTAS_EMPLAZAMIENTOS.INVENTARIO:
                                queryTmpInventario += (primeraVueltaInventario) ? "(" : " AND ";
                                primeraVueltaInventario = false;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.CONTACTOS:
                                queryTmpContactos += (primeraVueltaContactos) ? "(" : " AND ";
                                primeraVueltaContactos = false;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.DOCUMENTOS:
                                queryTmpDocumentos += (primeraVueltaDocumentos) ? "(" : " AND ";
                                primeraVueltaDocumentos = false;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.GENERAL:
                            default:
                                queryTmpGeneral += (primeraVueltaGeneral) ? "(" : " AND ";
                                primeraVueltaGeneral = false;
                                break;
                        }
                        #endregion

                        string queryTmp = string.Empty;
                        if (multi)
                        {
                            string IDs = value.Replace(';', ',');
                            if (IDs.EndsWith(",", StringComparison.Ordinal))
                            {
                                IDs = IDs.Remove(IDs.Length - 1);
                            }

                            queryTmp += GetSentenceQueryMulti(nameHeaderVwFilter, IDs);

                            if (tab != Comun.TABS_EMPLAZAMIENTO.TAB_SITE && tab != Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES && tab == tabSelected)
                            {
                                queryTmp += ") AND (" + GetSentenceQueryMulti(nameHeaderVw, IDs);
                            }
                        }
                        else
                        {
                            queryTmp += GetSentenceQuery(nameHeaderVwFilter, operador, value, typeData);

                            if (tab != Comun.TABS_EMPLAZAMIENTO.TAB_SITE && tab != Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES && tab == tabSelected)
                            {
                                queryTmp += ") AND (" + GetSentenceQuery(nameHeaderVw, operador, value, typeData);
                            }
                        }

                        #region Concatenación del filtro
                        switch (vista)
                        {
                            case Comun.VISTAS_EMPLAZAMIENTOS.INVENTARIO:
                                queryTmpInventario += queryTmp;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.CONTACTOS:
                                queryTmpContactos += queryTmp;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.DOCUMENTOS:
                                queryTmpDocumentos += queryTmp;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.GENERAL:
                            default:
                                queryTmpGeneral += queryTmp;
                                break;
                        }
                        #endregion
                    }

                    if (arrayItemsFiltro.Count > 0)
                    {
                        #region Concatenación del filtro
                        if (!string.IsNullOrEmpty(queryTmpInventario))
                        {
                            queryTmpInventario += ")";
                            whereInventario += queryTmpInventario;
                        }
                        if (!string.IsNullOrEmpty(queryTmpContactos))
                        {
                            queryTmpContactos += ")";
                            whereContactos += queryTmpContactos;
                        }
                        if (!string.IsNullOrEmpty(queryTmpDocumentos))
                        {
                            queryTmpDocumentos += ")";
                            whereDocumentos += queryTmpDocumentos;
                        }
                        if (!string.IsNullOrEmpty(queryTmpGeneral))
                        {
                            queryTmpGeneral += ")";
                            whereGeneral += queryTmpGeneral;
                        } 
                        #endregion
                    }
                }

                //whereGeneral = addClienteIdToWhere(whereGeneral, sClienteID);

                whereGeneral = addActivoContacto(tabSelected, whereGeneral);

                whereGeneral = addSitesVisibles(whereGeneral, visible);

                whereGeneral = addIdsResultados(whereGeneral);

                #region Buscador
                string outWhereInventario, outWhereDocumentos, outWhereContactos;

                whereGeneral = filtroBuscador(whereGeneral, whereInventario, whereDocumentos, whereContactos, tabSelected, textoBuscado, IdBuscado, out outWhereInventario, out outWhereDocumentos, out outWhereContactos);
                
                whereInventario = outWhereInventario;
                whereDocumentos = outWhereDocumentos;
                whereContactos = outWhereContactos; 
                #endregion

                #endregion

                #region Realizar consulta
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                dt = null;
                //dt = cEmplazamientos.Sp_VisualizacionEmplazamientosNuevo(tabSelected, whereGeneral, whereInventario, whereDocumentos, whereContactos, long.Parse(sClienteID));
                #endregion

            }
            catch (Exception ex)
            {
                dt = null;
                log.Error(ex);
            }
            return dt;
        }

        private static string filtroBuscador(string whereGeneral, string whereInventario, string whereDocumentos, string whereContactos, string tab, string textoBuscado, long? IdBuscado,
            out string outWhereInventario, out string outWhereDocumentos, out string outWhereContactos)
        {
            outWhereInventario = whereInventario;
            outWhereDocumentos = whereDocumentos;
            outWhereContactos = whereContactos;

            if (textoBuscado != null || IdBuscado != null)
            {
                string separadorWhereGeneral = (!string.IsNullOrEmpty(whereGeneral)) ? " AND " : "";



                if (IdBuscado != null)
                {
                    whereGeneral += separadorWhereGeneral + "EmplazamientoID=" + IdBuscado;
                }
                else if (textoBuscado != null)
                {
                    switch (tab)
                    {
                        case Comun.TABS_EMPLAZAMIENTO.TAB_SITE:
                            whereGeneral += separadorWhereGeneral + "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_ATRIBUTOS:
                            whereGeneral += separadorWhereGeneral + "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_CONTACTO:
                            outWhereContactos += (!string.IsNullOrEmpty(outWhereContactos)) ? " OR " : "";
                            outWhereContactos += "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%' OR " +
                                            "Nombre LIKE '%" + textoBuscado + "%' OR Apellidos LIKE '%" + textoBuscado + "%' OR Email LIKE '%" + textoBuscado +
                                            "%' OR Direccion LIKE '%" + textoBuscado + "%' OR CP LIKE '%" + textoBuscado + "%' OR Telefono LIKE '%" + textoBuscado +
                                            "%' OR Telefono2 LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_DOCUMENTOS:
                            outWhereDocumentos += (!string.IsNullOrEmpty(outWhereDocumentos)) ? " OR " : "";
                            outWhereDocumentos += "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%' OR " +
                                                    "Documento LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_INVENTARIO:
                            outWhereInventario += (!string.IsNullOrEmpty(outWhereInventario)) ? " OR " : "";
                            outWhereInventario += "(Codigo LIKE '%" + textoBuscado + "%' OR NombreEmplazamiento LIKE '%" + textoBuscado + "%' OR " +
                                                    "NombreElemento LIKE '%" + textoBuscado + "%' OR NumeroInventario LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES:
                            whereGeneral += separadorWhereGeneral + "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%' OR " +
                                                                    "Direccion LIKE '%" + textoBuscado + "%' OR CodigoPostal LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_MAP:
                            break;
                        default:
                            break;
                    }
                    
                }
            }

            return whereGeneral;
        }

        public static List<JsonObject> GetDatos(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado, string sFiltro)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            JsonObject oDato;
            try
            {
                DataTable dt = RealizarConsulta(filtrosAplicados, sClienteID, visible, textoBuscado, IdBuscado);
                if (dt != null)
                {
                    listaDatos = getListAtributos(dt);
                    //listaDatos = getListJsonObject(dt);
                }

                listaDatos = Filtro(sFiltro, listaDatos);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                listaDatos = null;
            }
            return listaDatos;
        }
        public static List<Vw_CoreLocalizaciones> GetDatosLocalizaciones(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado)
        {
            List<Vw_CoreLocalizaciones> localizaciones = new List<Vw_CoreLocalizaciones>();
            try
            {
                DataTable dt = RealizarConsulta(filtrosAplicados, sClienteID, visible, textoBuscado, IdBuscado);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        localizaciones.Add(getElementOfView<Vw_CoreLocalizaciones>(new Vw_CoreLocalizaciones(), row));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                localizaciones = null;
            }
            return localizaciones;
        }
        public static List<Vw_ContactosGlobalesEmplazamientos> GetDatosContactos(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado)
        {
            List<Vw_ContactosGlobalesEmplazamientos> contactos = new List<Vw_ContactosGlobalesEmplazamientos>();
            try
            {
                DataTable dt = RealizarConsulta(filtrosAplicados, sClienteID, visible, textoBuscado, IdBuscado);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        contactos.Add(getElementOfView<Vw_ContactosGlobalesEmplazamientos>(new Vw_ContactosGlobalesEmplazamientos(), row));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                contactos = null;
            }
            return contactos;
        }
        public static List<Vw_CoreEmplazamientos> GetDatosEmplazamientos(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado)
        {
            List<Vw_CoreEmplazamientos> emplazamientos = new List<Vw_CoreEmplazamientos>();
            try
            {
                DataTable dt = RealizarConsulta(filtrosAplicados, sClienteID, visible, textoBuscado, IdBuscado);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        emplazamientos.Add(getElementOfView<Vw_CoreEmplazamientos>(new Vw_CoreEmplazamientos(), row));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                emplazamientos = null;
            }
            return emplazamientos;
        }
        public static List<Vw_DocumentosEmplazamientos> GetDatosDocumentos(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado)
        {
            List<Vw_DocumentosEmplazamientos> documentos = new List<Vw_DocumentosEmplazamientos>();
            try
            {
                DataTable dt = RealizarConsulta(filtrosAplicados, sClienteID, visible, textoBuscado, IdBuscado);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Vw_DocumentosEmplazamientos docEmpTemp = getElementOfView<Vw_DocumentosEmplazamientos>(new Vw_DocumentosEmplazamientos(), row);

                        if (docEmpTemp.UltimaVersion.Value && docEmpTemp.Activo)
                        {
                            documentos.Add(docEmpTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                documentos = null;
            }
            return documentos;
        }
        public static List<Vw_InventarioElementosEmplazamientos> GetDatosInventario(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado)
        {
            List<Vw_InventarioElementosEmplazamientos> inventario = new List<Vw_InventarioElementosEmplazamientos>();
            try
            {
                DataTable dt = RealizarConsulta(filtrosAplicados, sClienteID, visible, textoBuscado, IdBuscado);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        inventario.Add(getElementOfView<Vw_InventarioElementosEmplazamientos>(new Vw_InventarioElementosEmplazamientos(), row));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                inventario = null;
            }
            return inventario;
        }

        public List<JsonObject> AplicarFiltroInterno(bool interno, string filtrosAplicados, int pageSize, int curPage, out int total, DataSorter[] sorters, string s)
        {
            List<JsonObject> items = new List<JsonObject>();
            total = 0;

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDEmplazamientoBuscador.IsEmpty) ? Convert.ToInt64(hdIDEmplazamientoBuscador.Value) : new long?();

            JObject json = JObject.Parse(filtrosAplicados);
            string tabSelected = (string)json["tab"];
            bool visible = (bool)json["visible"];

            DataTable dt = RealizarConsulta(filtrosAplicados, hdCliID.Value.ToString(), visible, textoBuscado, IdBuscado);

            #region Parsear segun tab
            switch (tabSelected)
            {
                case Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES:
                    List<JsonObject> localizaciones = new List<JsonObject>();
                    if (dt != null)
                    {
                        localizaciones = getListJsonObject(dt);
                    }

                    #region Paginación, filtro y ordenación
                    localizaciones = Filtro(s, localizaciones);
                    total = localizaciones.Count;
                    localizaciones = PaginacionOrdenacion(localizaciones, sorters, curPage, pageSize);
                    #endregion

                    if (interno)
                    {
                        items = localizaciones;
                    }
                    else
                    {
                        ((GridEmplazamientosLocalizaciones)this.hugeCt.ContentControls[0]).SetDataSourceGridEmplazamientosLocalizaciones(localizaciones, total);
                    }
                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_CONTACTO:
                    List<JsonObject> contactos = new List<JsonObject>();
                    if (dt != null)
                    {
                        contactos = getListJsonObject(dt);
                    }

                    #region Paginación, filtro y ordenación
                    contactos = Filtro(s, contactos);
                    total = contactos.Count;
                    contactos = PaginacionOrdenacion(contactos, sorters, curPage, pageSize);
                    #endregion

                    if (interno)
                    {
                        items = contactos;
                    }
                    else
                    {
                        ((GridEmplazamientosContactos)this.hugeCt.ContentControls[0]).SetDataSourceGridEmplazamientosContactos(contactos, total);
                    }
                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_MAP:
                    List<Vw_EmplazamientosMapasCercanos> maps = new List<Vw_EmplazamientosMapasCercanos>();
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            maps.Add(getElementOfView<Vw_EmplazamientosMapasCercanos>(new Vw_EmplazamientosMapasCercanos(), row));
                        }
                    }
                    ((Mapas)this.hugeCt.ContentControls[0]).SetDataSourceGridMapas(maps);
                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_INVENTARIO:
                     List<JsonObject> inventario = new List<JsonObject>();
                     if (dt != null)
                     {
                         inventario = getListJsonObject(dt);
                     }

                    #region Paginación, filtro y ordenación
                    Filtro(s, inventario);
                    total = inventario.Count;
                    inventario = PaginacionOrdenacion(inventario, sorters, curPage, pageSize);
                    #endregion

                    if (interno)
                    {
                        items = inventario;
                    }
                    else
                    {
                        ((GridEmplazamientosInventarios)this.hugeCt.ContentControls[0]).SetDataSourceGridInventario(inventario, total);
                    }

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_DOCUMENTOS:
                    List<JsonObject> documentos = new List<JsonObject>();
                    if (dt != null)
                    {
                        documentos = getListJsonObjectDocumentosActivosYUltimaVersion(dt);
                    }

                    #region Paginación, filtro y ordenación
                    documentos = Filtro(s, documentos);
                    total = documentos.Count;
                    documentos = PaginacionOrdenacion(documentos, sorters, curPage, pageSize);
                    #endregion

                    if (interno)
                    {
                        items = documentos;
                    }
                    else
                    {
                        ((GridEmplazamientosDocumentos)this.hugeCt.ContentControls[0]).SetDataSourceGridDocumento(documentos, total);
                    }
                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_ATRIBUTOS:
                    List<JsonObject> listaAtributos = new List<JsonObject>();
                    if (dt != null)
                    {
                        listaAtributos = getListAtributos(dt);
                    }

                    #region Paginación, filtro y ordenación
                    listaAtributos = Filtro(s, listaAtributos);
                    total = listaAtributos.Count;
                    listaAtributos = PaginacionOrdenacion(listaAtributos, sorters, curPage, pageSize);
                    #endregion

                    if (interno)
                    {
                        items = listaAtributos;
                    }
                    else
                    {
                        ((GridEmplazamientosAtributos)this.hugeCt.ContentControls[0]).SetDataSourceGridAtributo(listaAtributos, total);
                    }
                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_SITE:
                default:
                    List<JsonObject> sites = new List<JsonObject>();
                    if (dt != null)
                    {
                        sites = getListJsonObject(dt);
                    }

                    #region Paginación, filtro y ordenación
                    sites = Filtro(s, sites);
                    total = sites.Count;
                    sites = PaginacionOrdenacion(sites, sorters, curPage, pageSize);
                    #endregion

                    if (interno)
                    {
                        items = sites;
                    }
                    else
                    {
                        ((GridEmplazamientos)this.hugeCt.ContentControls[0]).SetDataSourceGridEmplazamientos(sites, total);
                    }

                    break;
            }
            #endregion

            return items;
        }

        private List<JsonObject> PaginacionOrdenacion(List<JsonObject> lista, DataSorter[] sorters, int curPage, int pageSize) {
            
            if (sorters != null)
            {
                lista = LinqEngine.SortJson(lista, sorters);
            }

            if (curPage != -1 && pageSize != -1)
            {
                lista = lista.Skip(curPage * pageSize).Take(pageSize).ToList();
            }

            return lista;
        }

        private static List<JsonObject> Filtro(string s, List<JsonObject> lista)
        {
            if (!string.IsNullOrEmpty(s))
            {
                lista = LinqEngine.filtroJson(lista, s);
            }

            return lista;
        }

        [DirectMethod()]
        public DirectResponse AplicarFiltro(string filtrosAplicados, Ext.Net.StoreReadDataEventArgs e, int pageSize, int curPage)
        {
            DirectResponse direct = new DirectResponse();


            try
            {
                int total;
                List<JsonObject> lista = AplicarFiltroInterno(false, filtrosAplicados, pageSize, curPage, out total, null, null);

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

        private static T getElementOfView<T>(T temp, DataRow row)
        {

            PropertyInfo[] propiedades = temp.GetType().GetProperties();
            foreach (PropertyInfo propiedad in propiedades)
            {
                try
                {
                    dynamic valor = row.Field<dynamic>(propiedad.Name);
                    temp.GetType().GetProperty(propiedad.Name).SetValue(temp, valor, null);
                }
                catch (Exception) { }
            }

            return temp;
        }

        private static List<JsonObject> getListJsonObject(DataTable oDataTable)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            JsonObject oJson;
            try
            {
                foreach (DataRow row in oDataTable.Rows)
                {
                    oJson = new JsonObject();
                    for (int i = 0; i < oDataTable.Columns.Count; i++)
                    {
                        oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                    }
                    listaDatos.Add(oJson);
                }

            }
            catch (Exception)
            {
                listaDatos = null;
            }
            return listaDatos;
        }

        private static List<JsonObject> getListAtributos(DataTable oDataTable)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
            JsonObject oJson;
            JObject jsonAux;
            object nombreAtributo, valorAtributo = "";
            Data.EmplazamientosAtributosConfiguracion atr;
            try
            {
                foreach (DataRow row in oDataTable.Rows)
                {

                    oJson = new JsonObject();
                    for (int i = 0; i < oDataTable.Columns.Count; i++)
                    {
                        if (oDataTable.Columns[i].ToString() != "jsonAtributosDinamicos")
                        {
                            oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                        }
                        else
                        {
                            if (row[i] != null && row[i].ToString() != "")
                            {
                                jsonAux = JObject.Parse(row[i].ToString());
                                foreach (dynamic item in jsonAux)
                                {
                                    dynamic valor = item.Value;
                                    try
                                    {
                                        atr = cAtributosConf.GetItem(long.Parse(valor.AtributoID.ToString()));
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
                                            oJson.Add(atr.NombreAtributo.ToString().Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""), sValor);
                                        }
                                        //else if (valor.TipoDato != null && valor.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                                        //{
                                        //    if (valor.NombreAtributo != null)
                                        //    {
                                        //        oJson.Add(valor.NombreAtributo.ToString(), DateTime.Parse(valor.Valor.ToString()).ToString(Comun.FORMATO_FECHA));
                                        //    }
                                        //}
                                        else
                                        {
                                            if (valor.NombreAtributo != null)
                                            {
                                                if (atr != null)
                                                {
                                                    oJson.Add(atr.NombreAtributo.ToString().Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""), valor.Valor.ToString());
                                                }
                                                else
                                                {
                                                    oJson.Add(valor.NombreAtributo.ToString().Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""), valor.Valor.ToString());
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //jsonAux.Add(item.Key, item.Value);
                                        oJson.Add(valor.NombreAtributo.ToString().Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""), valor.Valor.ToString());
                                    }
                                    //oJson.Add(valor.NombreAtributo.ToString(), valor.Valor.ToString());
                                }
                            }
                        }
                    }
                    listaDatos.Add(oJson);
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }
            return listaDatos;
        }

        private List<JsonObject> getListJsonObjectDocumentosActivosYUltimaVersion(DataTable oDataTable)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            JsonObject oJson;
            try
            {
                foreach (DataRow row in oDataTable.Rows)
                {
                    oJson = new JsonObject();
                    for (int i = 0; i < oDataTable.Columns.Count; i++)
                    {
                        oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                    }

                    if (oJson["Activo"].ToString().ToLower() == "true" && oJson["UltimaVersion"].ToString().ToLower() == "true")
                    {
                        listaDatos.Add(oJson);
                    }
                }
            }
            catch (Exception)
            {
                listaDatos = null;
            }
            return listaDatos;
        }

        #region CONSTRUCCIÓN DE SENTENCIA WHERE
        private static string addClienteIdToWhere(string whereQuery, string ClienteID)
        {
            string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

            string[] elements = { whereQuery, operadorAnd, "ClienteID=", ClienteID };
            return string.Concat(elements);
        }

        private static string addActivoContacto(string tab, string whereQuery)
        {
            string query = "";
            if (tab == Comun.TABS_EMPLAZAMIENTO.TAB_CONTACTO)
            {
                string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

                string[] elements = { whereQuery, operadorAnd, "Activo=1" };
                query = string.Concat(elements);
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        public static string addIdsResultados(string whereQuery)
        {
            string query = "";
            
            if (!string.IsNullOrEmpty(sResultadoKPIid))
            {
                DQKpisMonitoringController cDQKpisMonitoring = new DQKpisMonitoringController();
                long idDQKpiMonitoring = long.Parse(sResultadoKPIid);
                DQKpisMonitoring DQKpiMonitoring = cDQKpisMonitoring.GetItem(idDQKpiMonitoring);
                

                if (DQKpiMonitoring != null)
                {
                    List<long> listaIDs;
                    DQKpisController cDQKpis = new DQKpisController();
                    cDQKpis.ejecutarConsulta(DQKpiMonitoring.Filtro, out listaIDs);

                    string sIDs = "";
                    listaIDs.ForEach(id =>
                    {
                        sIDs += (!string.IsNullOrEmpty(sIDs) ? ", " : "") + id;
                    });

                    string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);
                    string[] elements = { whereQuery, operadorAnd, "EmplazamientoID", " IN(", sIDs, ") " };
                    query = string.Concat(elements);
                }
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        private static string addSitesVisibles(string whereQuery, bool visible)
        {
            string query = "";
            string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

            if (visible)
            {
                string[] elements = { whereQuery, operadorAnd, "Visible=1" };
                query = string.Concat(elements);
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        private static string GetSentenceQuery(string columName, string operador, string valor, string tipoValor)
        {
            string sentence = string.Empty;

            if (string.IsNullOrEmpty(operador))
            {
                operador = Comun.OPERADOR_IGUAL;
            }

            Type type = Type.GetType(tipoValor);

            if (type == typeof(System.String))
            {
                string[] elements = { columName, " ", "LIKE", " '%", valor, "%'" };
                sentence = string.Concat(elements);
            }
            else if (tipoValor.Contains(typeof(System.DateTime).FullName))
            {
                //FORMAT(FechaActivacion, 'dd/MM/yyyy') = FORMAT(CONVERT(DATETIME, '01/01/2007'), 'dd/MM/yyyy')
                //FechaActivacion > CONVERT(DATETIME, '1/6/2021')
                string[] elements = { columName, " ", GetOperadorByString(operador), " CONVERT(DATETIME, '", valor, "')" };
                sentence = string.Concat(elements);
            }
            else if (tipoValor.Contains(typeof(System.Int64).FullName) || tipoValor.Contains(typeof(System.Double).FullName))
            {
                string[] elements = { columName, " ", GetOperadorByString(operador), " ", valor };
                sentence = string.Concat(elements);
            }

            return sentence;
        }

        private static string GetSentenceQueryMulti(string columName, string IDs)
        {
            string sentence = string.Empty;

            string[] elements = { columName, " IN (", IDs, ")" };
            sentence = string.Concat(elements);

            return sentence;
        }

        private static string GetOperadorByString(string operador)
        {
            string operadorResultado;
            switch (operador)
            {
                case Comun.OPERADOR_MAYOR:
                    operadorResultado = ">";
                    break;
                case Comun.OPERADOR_MENOR:
                    operadorResultado = "<";
                    break;
                case Comun.OPERADOR_IGUAL:
                default:
                    operadorResultado = "=";
                    break;
            }
            return operadorResultado;
        }
        #endregion

        #endregion

        #region FUNCTIONS
        public string GetValoresStringComboBox(List<string> lista)
        {
            string valores = "";

            for (int j = 0; j < lista.Count; j = j + 1)
            {
                if (j == 0)
                {
                    valores = lista[j].ToString();
                }
                else
                {
                    valores += ", " + lista[j].ToString();
                }
            }

            return valores;
        }

        public List<string> GetValoresPosibles()
        {
            List<string> lista = new List<string>();

            OperadoresController cOperadores = new OperadoresController();
            MonedasController cMonedas = new MonedasController();
            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
            EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();
            EmplazamientosTiposController cTipos = new EmplazamientosTiposController();
            EmplazamientosTiposEdificiosController cEdificios = new EmplazamientosTiposEdificiosController();
            EmplazamientosTiposEstructurasController cEstructuras = new EmplazamientosTiposEstructurasController();
            EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();

            long cliID = 0;

            if (ClienteID != null)
            {
                cliID = ClienteID.Value;
            }
            else
            {
                if (cmbClientes.SelectedItem.Value != "" && cmbClientes.SelectedItem.Value != null)
                {
                    cliID = Convert.ToInt32(cmbClientes.SelectedItem.Value);
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
            }

            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
            lista.Add(GetValoresStringComboBox(cOperadores.GetOperadoresActivosNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cMonedas.GetMonedasNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cEstadosGlobales.GetEstadosGlobalesNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cCategorias.GetCategoriasNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cTipos.GetTiposNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cEdificios.GetEdificiosNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cEstructuras.GetEstructurasNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cTamanos.GetTamanosNombre(cliID)));
            lista.Add("(DD/MM/AAAA)");
            lista.Add("(DD/MM/AAAA)");
            lista.Add(" ");
            lista.Add(" ");
            lista.Add(" ");
            lista.Add(" ");
            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "150"));
            lista.Add(GetGlobalResource("strMaximo"));
            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
            lista.Add(GetGlobalResource("strEntero") + "-" + GetGlobalResource("strDecimal"));
            lista.Add(GetGlobalResource("strEntero") + "-" + GetGlobalResource("strDecimal"));

            return lista;
        }

        [DirectMethod]
        public DirectResponse ExportarModeloDatos()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosEmplazamientos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;

                List<string> lTabs = new List<string>();
                lTabs.Add(GetGlobalResource("strEmplazamientos"));
                lTabs.Add(GetGlobalResource("strContactos"));

                #region Datos Emplazamientos
                List<string> filaTipoDatoEmplazamientos = new List<string>();
                List<string> filaValoresPosiblesEmplazamientos = new List<string>();
                List<string> filaCabeceraEmplazamientos = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strFecha"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strFecha"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strNumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strNumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesEmplazamientos = GetValoresPosibles();
                #endregion

                #region Fila Cabecera
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEmplazamientoCodigo") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strNombreSitio") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strOperador") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strMoneda") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEstadoGlobal") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCategoriaSitio") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTiposEmplazamientos") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTipoEdificio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTipoEstructura"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEmplazamientosTamanos"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strFechaActivacion") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strFechaDesactivacion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strPais"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strRegion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strProvincia"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strMunicipio") + "*");
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strBarrio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strDireccion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCodigoPostal"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strLatitud"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strLongitud"));
                #endregion

                #endregion

                #region Datos Contactos
                List<string> filaTipoDato = new List<string>();
                List<string> filaValoresPosibles = new List<string>();
                List<string> filaCabecera = new List<string>();

                #region Fila Tipos de datos
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                #endregion

                #region Fila Cabecera
                filaCabecera.Add(GetGlobalResource("strEmplazamientoCodigo") + "*");
                filaCabecera.Add(GetGlobalResource("strEmail") + "*");
                filaCabecera.Add(GetGlobalResource("strTelefono") + "*");
                #endregion

                #endregion

                long cont = 1;

                foreach (string elem in lTabs)
                {
                    if (cont == 1)
                    {
                        cEmplazamientos.ExportarModeloDatos(saveAs, elem, filaTipoDatoEmplazamientos, filaValoresPosiblesEmplazamientos, filaCabeceraEmplazamientos);
                    }
                    else
                    {
                        cEmplazamientos.ExportarModeloDatosOpen(saveAs, elem, filaTipoDato, filaValoresPosibles, filaCabecera, cont);
                    }
                    cont++;
                }

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

    }
}