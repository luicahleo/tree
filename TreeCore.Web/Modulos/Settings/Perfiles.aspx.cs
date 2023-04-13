using System;
using CapaNegocio;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Ext.Net;
using TreeCore.Data;
using Label = Ext.Net.Label;
using TreeCore.Clases;
using TreeCore.Shared.DTO.General;
using TreeCore.APIClient;

namespace TreeCore.ModGlobal.pages
{

    public partial class Perfiles : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        private List<Container> listaContenedores;
        static bool VisorTreePClosed = false;
        private static bool VistaLectura = true;

        #region Direct & Methods LAYOUT

        [DirectMethod]
        public void VwUpdater()
        {
            this.CenterPanelMain.Update();

        }





        #endregion

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                /*#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion*/

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmpFiltro.ClienteID = 0;
                }
                else
                {
                    hdCliID.SetValue(ClienteID);
                    hdCliID.DataBind();
                }

                if (Usuario.EMail == Comun.TREE_SUPER_USER)
                {
                    lnkFuncionalidades.Show();
                }
                else
                {
                    lnkFuncionalidades.Hide();
                }

                hdPerfilSeleccionado.SetValue(0);
                storeRoles.Reload();
                //storeProyectosTipos.Reload();
                CargarArbolPerfiles();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_Perfiles_Lectura))
            {
                cnPerfilesRol.Disabled = true;

                VistaLectura = true;

            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_PERFILES))
            {
                cnPerfilesRol.Disabled = false;

                VistaLectura = false;
            }

            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { }},
                { "Post", new List<ComponentBase> { btnAnadirPerfiles, btnAnadirRol }},
                { "Put", new List<ComponentBase> { btnEditarPerfiles, btnActivarPerfiles, btnEditarRol, btnActivarRol }},
                { "Delete", new List<ComponentBase> { btnEliminarPerfiles, btnEliminarRol }}
            };
        }

        #region STORES

        private class AuxObject
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        #region MODULES

        protected void storeModules_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<AuxObject> listaObjects = new List<AuxObject>();
                    foreach (var oModule in ModulesController.GetModules())
                    {
                        listaObjects.Add(new AuxObject
                        {
                            Code = oModule.Code,
                            Name = GetGlobalResource($"str{oModule.Name}")
                        });
                    }
                    storeModules.DataSource = listaObjects;
                    storeModules.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region USER FUNCTIONALITY TYPES

        protected void storeUserFuntionalityTypes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<AuxObject> listaObjects = new List<AuxObject>();
                    foreach (var oModule in ModulesController.GetUserFuntionalityTypes())
                    {
                        listaObjects.Add(new AuxObject
                        {
                            Code = oModule.Code,
                            Name = GetGlobalResource(oModule.Resource.FirstOrDefault())
                        });
                    }
                    storeUserFuntionalityTypes.DataSource = listaObjects;
                    storeUserFuntionalityTypes.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region USER INTERFACES

        private class userInterAux
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public List<userFuntAux> Functionalities { get; set; }
        }
        private class userFuntAux
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Asignada { get; set; }
        }

        protected void storeUserInterfaces_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(TOKEN_API);
                try
                {
                    List<userInterAux> listaObjects = new List<userInterAux>();
                    List<userFuntAux> listaFuncionalities;
                    string sPerfil = hdPerfilSeleccionado.Value.ToString();
                    if (sPerfil != "")
                    {
                        ProfileDTO profile = baseAPI.GetByCode(sPerfil).Result.Value;
                        List<Clases.UserFunctionality> functionalities = new List<UserFunctionality>();
                        if (profile.UserFuntionalities != null)
                        {
                            functionalities = ModulesController.GetUserFunctionalitiesFromProfile(profile);
                        }
                        List<Clases.UserFuntionalityType> funtionalityTypes = ModulesController.GetUserFuntionalityTypes();
                        List<string> listCodes = functionalities.Select(x => x.Code).ToList();
                        foreach (var oModule in ModulesController.GetUserInterfacesFromModuleCode(profile.ModuleCode))
                        {
                            string sTra = "";
                            var listaFuncionalidaesModulo = ModulesController.GetUserFunctionalitiesFromUserInterface(oModule.Code);
                            listaFuncionalities = new List<userFuntAux>();
                            foreach (var oFuncionalidad in listaFuncionalidaesModulo)
                            {
                                sTra = "";
                                foreach (var item in funtionalityTypes.FirstOrDefault(x => x.Code == oFuncionalidad.Code.Split('@')[1]).Resource)
                                {
                                    if (sTra != "")
                                        sTra += "";
                                    sTra += GetGlobalResource(item);
                                }
                                listaFuncionalities.Add(new userFuntAux
                                {
                                    Code = oFuncionalidad.Code,
                                    Name = sTra,
                                    Asignada = (listCodes.Contains(oFuncionalidad.Code)) ? "checked" : ""
                                });
                            }
                            sTra = "";
                            foreach (var sKey in oModule.Resource)
                            {
                                if (sTra != "")
                                    sTra += " ";
                                sTra += GetGlobalResource(sKey);
                            }
                            listaFuncionalities.Sort((x, y) => x.Code.CompareTo(y.Code));
                            listaObjects.Add(new userInterAux
                            {
                                Code = oModule.Code,
                                Name = sTra,
                                Functionalities = listaFuncionalities
                            });
                        }
                        listaObjects.Sort((x, y) => x.Code.CompareTo(y.Code));
                    }
                    storeUserInterfaces.DataSource = listaObjects;
                    storeUserInterfaces.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ROLES

        protected void storeRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Roles> listaDatos;

                    listaDatos = ListaRoles(btnActivosRol.Pressed);

                    if (listaDatos != null)
                    {

                        storeRoles.DataSource = listaDatos;
                        storePerfiles.Reload();
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

        private List<Data.Roles> ListaRoles(bool activos)
        {
            List<Data.Roles> listaDatos;
            RolesController cRoles = new RolesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                if (activos)
                {
                    if (cmpFiltro.ClienteID != 0)
                    {
                        listaDatos = cRoles.GetActivos(long.Parse(cmpFiltro.ClienteID.ToString()));
                    }
                    else
                    {
                        listaDatos = cRoles.GetActivos(lCliID);
                    }

                }
                else
                {
                    if (cmpFiltro.ClienteID != 0)
                    {
                        listaDatos = cRoles.GetByClienteID(long.Parse(cmpFiltro.ClienteID.ToString()));
                    }
                    else
                    {
                        listaDatos = cRoles.GetByClienteID(lCliID);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region PERFILES

        protected void storePerfiles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Perfiles> listaDatos;

                    listaDatos = ListaPerfiles();

                    if (listaDatos != null)
                    {

                        storePerfiles.DataSource = listaDatos;
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

        private List<Data.Perfiles> ListaPerfiles()
        {
            List<Data.Perfiles> listaDatos;
            PerfilesController cRoles = new PerfilesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                if (lCliID == 0)
                {
                    lCliID = this.cmpFiltro.ClienteID;
                }
                listaDatos = cRoles.GetActivos(lCliID);


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region PERFILES ASIGNADOS

        protected void storePerfilesAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Perfiles> listaDatos;

                    listaDatos = ListaPerfilesAsignados();

                    if (listaDatos != null)
                    {

                        storePerfilesAsignados.DataSource = listaDatos;
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

        private List<Data.Perfiles> ListaPerfilesAsignados()
        {
            List<Data.Perfiles> listaDatos;
            PerfilesController cPerfiles = new PerfilesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long rolID = long.Parse(hdRolSeleccionado.Value.ToString());
            try
            {
                listaDatos = cPerfiles.GetPerfilesByRol(rolID);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region DOCUMENTOS TIPOS ROLES

        private class PermisosRoles
        {
            private long _ObjetoID;
            private string _Nombre;
            private bool _PermisoEscritura;
            private bool _PermisoLectura;
            private bool _PermisoDescarga;
            private string _Restriccion;


            public string Nombre
            {
                get => _Nombre;
                set
                {
                    _Nombre = value;

                }
            }
            public long ObjetoID
            {
                get => _ObjetoID;
                set
                {
                    _ObjetoID = value;

                }
            }
            public bool PermisoEscritura
            {
                get => _PermisoEscritura;
                set
                {
                    _PermisoEscritura = value;

                }
            }
            public bool PermisoLectura
            {
                get => _PermisoLectura;
                set
                {
                    _PermisoLectura = value;

                }
            }
            public bool PermisoDescarga
            {
                get => _PermisoDescarga;
                set
                {
                    _PermisoDescarga = value;

                }
            }
            public string Restriccion
            {
                get => _Restriccion;
                set
                {
                    _Restriccion = value;

                }
            }

            public PermisosRoles(long id, string nombre, bool escritura, bool lectura, bool descarga)
            {
                _ObjetoID = id;
                _Nombre = nombre;
                _PermisoEscritura = escritura;
                _PermisoLectura = lectura;
                _PermisoDescarga = descarga;
            }
            public PermisosRoles(long id, string nombre, string restriccion)
            {
                _ObjetoID = id;
                _Nombre = nombre;
                _Restriccion = restriccion;
            }
        }
        private class PermisosConjunto
        {
            private string _Tipo;
            private List<PermisosRoles> _ListaItem;

            public string Tipo
            {
                get => _Tipo;
                set
                {
                    _Tipo = value;

                }
            }
            public List<PermisosRoles> ListaItem
            {
                get => _ListaItem;
                set
                {
                    _ListaItem = value;

                }
            }


            public PermisosConjunto(string tipo, List<PermisosRoles> ListaItem)
            {
                _Tipo = tipo;
                _ListaItem = ListaItem;

            }
        }


        protected void storePermisosRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {

                    List<PermisosConjunto> listaGenerica = new List<PermisosConjunto>();
                    if (cmbFiltroPermisos.Value.ToString() == "Documentos")
                    {
                        listaGenerica.Add(DocumentosTiposRoles());
                    }
                    else if (cmbFiltroPermisos.Value.ToString() == "Emplazamientos")
                    {
                        listaGenerica.Add(EmplazamientoAtributosRoles());
                    }
                    else if (cmbFiltroPermisos.Value.ToString() == "Inventario")
                    {
                        listaGenerica.Add(InventarioAtributosRoles());
                    }
                    else if (cmbFiltroPermisos.Value.ToString() == "Accesos")
                    {
                        listaGenerica.Add(GruposAcceso());
                    }
                    else
                    {
                        listaGenerica.Add(DocumentosTiposRoles());
                        listaGenerica.Add(EmplazamientoAtributosRoles());
                        listaGenerica.Add(InventarioAtributosRoles());
                        listaGenerica.Add(GruposAcceso());
                    }


                    if (listaGenerica != null)
                    {
                        storePermisosRoles.DataSource = listaGenerica;

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

        private PermisosConjunto DocumentosTiposRoles()
        {
            List<Data.Vw_DocumentosTiposRoles> listaDatos = null;
            List<PermisosRoles> listaGenerica = new List<PermisosRoles>();
            PermisosConjunto itemPermiso = null;
            DocumentosRolesController cDocumentosRoles = new DocumentosRolesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long rolID = long.Parse(hdRolSeleccionado.Value.ToString());
            try
            {
                listaDatos = cDocumentosRoles.GetRolesByRolID(rolID);
                foreach (Vw_DocumentosTiposRoles item in listaDatos)
                {
                    listaGenerica.Add(new PermisosRoles(item.DocumentoTipoRoleID.Value, item.DocumentTipo, item.PermisoEscritura.Value, item.PermisoLectura.Value, item.PermisoDescarga.Value));
                }
                itemPermiso = new PermisosConjunto(GetGlobalResource("jsDocumentoTipo"), listaGenerica);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                itemPermiso = null;
            }

            return itemPermiso;
        }
        private PermisosConjunto EmplazamientoAtributosRoles()
        {
            List<Data.Vw_EmplazamientosAtributosRolesRestringidos> listaDatos = null;
            List<PermisosRoles> listaGenerica = new List<PermisosRoles>();
            PermisosConjunto itemPermiso = null;
            EmplazamientosAtributosRolesRestringidosController cDocumentosRoles = new EmplazamientosAtributosRolesRestringidosController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long rolID = long.Parse(hdRolSeleccionado.Value.ToString());
            string restriccion = "";
            try
            {
                listaDatos = cDocumentosRoles.GetRolesByRolID(rolID);
                foreach (Vw_EmplazamientosAtributosRolesRestringidos item in listaDatos)
                {

                    switch (item.Restriccion.Value)
                    {
                        case 1:
                            restriccion = Comun.RestriccionesAtributoDefecto.ACTIVE.ToString();
                            break;
                        case 2:
                            restriccion = Comun.RestriccionesAtributoDefecto.DISABLED.ToString();
                            break;
                        case 3:
                            restriccion = Comun.RestriccionesAtributoDefecto.HIDDEN.ToString();
                            break;

                    }


                    listaGenerica.Add(new PermisosRoles(item.EmplazamientoAtributoRolRestringidoID, item.NombreAtributo, restriccion));
                }
                itemPermiso = new PermisosConjunto(GetGlobalResource("jsEmplazamientoAtributos"), listaGenerica);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                itemPermiso = null;
            }

            return itemPermiso;
        }
        private PermisosConjunto InventarioAtributosRoles()
        {
            List<Data.CoreAtributosConfiguracionRolesRestringidos> listaDatos = null;
            List<PermisosRoles> listaGenerica = new List<PermisosRoles>();
            PermisosConjunto itemPermiso = null;
            InventarioAtributosRolesRestringidosController cDocumentosRoles = new InventarioAtributosRolesRestringidosController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long rolID = long.Parse(hdRolSeleccionado.Value.ToString());
            string restriccion = "";
            try
            {
                listaDatos = cDocumentosRoles.GetRolesByRolID(rolID);
                foreach (CoreAtributosConfiguracionRolesRestringidos item in listaDatos)
                {

                    switch (item.Restriccion)
                    {
                        case 1:
                            restriccion = Comun.RestriccionesAtributoDefecto.ACTIVE.ToString();
                            break;
                        case 2:
                            restriccion = Comun.RestriccionesAtributoDefecto.DISABLED.ToString();
                            break;
                        case 3:
                            restriccion = Comun.RestriccionesAtributoDefecto.HIDDEN.ToString();
                            break;

                    }


                    listaGenerica.Add(new PermisosRoles(item.CoreAtributoConfiguracionRolRestringidoID, item.CoreAtributosConfiguraciones.Nombre, restriccion));
                }
                itemPermiso = new PermisosConjunto(GetGlobalResource("jsInventarioElemento"), listaGenerica);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                itemPermiso = null;
            }

            return itemPermiso;
        }


        private PermisosConjunto GruposAcceso()
        {
            List<Data.GruposAccesosWebRoles> listaDatos = null;
            List<PermisosRoles> listaGenerica = new List<PermisosRoles>();
            PermisosConjunto itemPermiso = null;
            GruposAccesosWebRolesController cGruposDeAcceso = new GruposAccesosWebRolesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long rolID = long.Parse(hdRolSeleccionado.Value.ToString());
            try
            {
                listaDatos = cGruposDeAcceso.getGruposAccesoByRolID(rolID);
                foreach (GruposAccesosWebRoles item in listaDatos)
                {
                    foreach (PermisosRoles permisos in listaGenerica)
                    {
                        if (item.GruposAccesosWeb.GrupoAcceso != permisos.Nombre)
                        {
                            listaGenerica.Add(new PermisosRoles(item.GrupoAccesoWebRolID, item.GruposAccesosWeb.GrupoAcceso, null));
                        }
                    }
                    if (listaGenerica.Count == 0)
                    {
                        listaGenerica.Add(new PermisosRoles(item.GrupoAccesoWebRolID, item.GruposAccesosWeb.GrupoAcceso, null));
                    }

                }
                itemPermiso = new PermisosConjunto(GetGlobalResource("jsGruposAccesosWeb"), listaGenerica);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                itemPermiso = null;
            }

            return itemPermiso;
        }
        #endregion

        #endregion

        #region DIRECT METHODS

        #region PERFILES

        #region GESTION BASICA PERFILES

        [DirectMethod]
        public DirectResponse AgregarEditarPerfil(bool Agregar, string[] funcionalidades)
        {
            BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(TOKEN_API);
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";

                List<string> listaFuncionalidades = new List<string>();
                if (Agregar)
                {
                    string sModule = hdProyectoTipoSeleccionado.Value.ToString();
                    string sTipoFuncionalidad = (cmbPerfilesFuncionalidadesTipos.IsEmpty) ? "" : sTipoFuncionalidad = cmbPerfilesFuncionalidadesTipos.Value.ToString();
                    if (sTipoFuncionalidad != "")
                        listaFuncionalidades.AddRange(ModulesController.GetUserFunctionalitiesFromModuleCode(sModule).Where(x => x.Type == sTipoFuncionalidad).Select(x => x.Code).ToList());
                    ProfileDTO profile = new ProfileDTO
                    {
                        Active = true,
                        Code = txtNombrePerfil.Value.ToString(),
                        Description = txtaDescripcionPerfil.Value.ToString(),
                        ModuleCode = sModule,
                        UserFuntionalities = listaFuncionalidades
                    };
                    var Result = baseAPI.AddEntity(profile).Result;
                    if (Result.Success)
                    {
                        hdPerfilSeleccionado.SetValue(Result.Value.Code);
                        lbNombrePerfil.Text = Result.Value.Code;
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
                    }
                }
                else
                {
                    string sPerfil = hdPerfilSeleccionado.Value.ToString();
                    ProfileDTO profile = baseAPI.GetByCode(sPerfil).Result.Value;

                    if (winAddProfile.Hidden)
                    {
                        listaFuncionalidades.AddRange(funcionalidades);
                        profile.UserFuntionalities = listaFuncionalidades;
                    }
                    else
                    {
                        profile.Code = txtNombrePerfil.Value.ToString();
                        profile.Description = txtaDescripcionPerfil.Value.ToString();
                    }

                    var Result = baseAPI.UpdateEntity(sPerfil, profile).Result;
                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
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

        [DirectMethod]
        public DirectResponse MostrarEditarPerfil()
        {
            BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(TOKEN_API);
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                string sPerfil = hdPerfilSeleccionado.Value.ToString();
                ProfileDTO profile = baseAPI.GetByCode(sPerfil).Result.Value;
                txtNombrePerfil.SetValue(profile.Code);
                txtaDescripcionPerfil.SetValue(profile.Description);
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
        public DirectResponse EliminarPerfil()
        {
            BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(TOKEN_API);
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                string sPerfil = hdPerfilSeleccionado.Value.ToString();
                var Result = baseAPI.DeleteEntity(sPerfil).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
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

        [DirectMethod]
        public DirectResponse ActivarPerfiles()
        {
            BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(TOKEN_API);
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                string sPerfil = hdPerfilSeleccionado.Value.ToString();
                var oDato = baseAPI.GetByCode(sPerfil).Result.Value;
                oDato.Active = !oDato.Active;

                var Result = baseAPI.UpdateEntity(oDato.Code, oDato).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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

        [DirectMethod]
        public DirectResponse DescargarPerfiles()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strComun"),
                    Expanded = true
                };
                nodes.Add(nodoRaiz);
                nodoRaiz.Children.AddRange(ConstruirArbolFuncionalidades());
                TreePanelFuncionalidades.SetRootNode(nodoRaiz);
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

        #region ARBOL

        [DirectMethod]
        public DirectResponse CargarArbolPerfiles()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strComun"),
                    Expanded = true
                };
                nodes.Add(nodoRaiz);
                nodoRaiz.Children.AddRange(ConstruirArbolPerfiles());
                TreePanelPerfiles.SetRootNode(nodoRaiz);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        private NodeCollection ConstruirArbolPerfiles()
        {
            NodeCollection oNodes;
            BaseAPIClient<ProfileDTO> aPIClient = new BaseAPIClient<ProfileDTO>(TOKEN_API);
            try
            {
                List<Clases.Module> modules = ModulesController.GetModules();
                if (cmbTipoProyecto.Value != null && cmbTipoProyecto.Value.ToString() != "")
                    modules = modules.Where(x => x.Code == cmbTipoProyecto.Value.ToString()).ToList();
                List<ProfileDTO> profiles = aPIClient.GetList().Result.Value;
                oNodes = GetNodosModulos(modules, profiles);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNodes = null;
            }

            return oNodes;
        }
        private NodeCollection GetNodosModulos(List<Clases.Module> listaModulos, List<ProfileDTO> listaPerfiles)
        {
            NodeCollection oMenu = new NodeCollection(false);

            try
            {
                listaModulos.ForEach(oItem =>
                {
                    bool tieneHijos = false;
                    Node oNodo = new Node
                    {
                        Text = "Proyecto",
                        Expanded = false,
                        Expandable = true,
                        NodeID = $"MOD{oItem.Code}"
                    };
                    oNodo.CustomAttributes.Add(new ConfigItem("Nombre", GetGlobalResource($"str{oItem.Name}")));
                    oNodo.CustomAttributes.Add(new ConfigItem("Descripcion", ""));
                    oNodo.CustomAttributes.Add(new ConfigItem("ID", oItem.Code));

                    NodeCollection nodoshijos = GetNodosPerfiles(oItem.Code, listaPerfiles);
                    if (nodoshijos != null && nodoshijos.Count > 0)
                    {
                        tieneHijos = true;
                        oNodo.Children.AddRange(nodoshijos);
                    }
                    oNodo.Leaf = !tieneHijos;
                    oMenu.Add(oNodo);
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }

            return oMenu;
        }
        private NodeCollection GetNodosPerfiles(string CodigoModulo, List<ProfileDTO> listaPerfiles)
        {
            NodeCollection oMenu = new NodeCollection(false);
            try
            {
                listaPerfiles.Where(x => x.ModuleCode == CodigoModulo && ((btnSoloActivosPerfiles.Pressed && x.Active) || !btnSoloActivosPerfiles.Pressed)).ToList().ForEach(oItem =>
                {
                    Node oNodo = new Node
                    {
                        Text = "Perfil",
                        IconCls = "ico-lock-16px",
                        Expanded = false,
                        Expandable = true,
                        NodeID = $"PER{oItem.Code}"
                    };
                    if (!oItem.Active)
                    {
                        oNodo.Cls = "itemArbolDesactivo";
                    }
                    oNodo.CustomAttributes.Add(new ConfigItem("Nombre", oItem.Code));
                    oNodo.CustomAttributes.Add(new ConfigItem("Descripcion", oItem.Description));
                    oNodo.CustomAttributes.Add(new ConfigItem("ID", oItem.Code));
                    oNodo.Leaf = true;
                    oMenu.Add(oNodo);
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }

            return oMenu;
        }

        #endregion

        #region GESTION PERMISOS

        [DirectMethod]
        public DirectResponse PintarPermisosAsociados(bool Update = true)
        {
            List<Clases.Module> listaModulos;
            List<Clases.UserFunctionality> listaFuncionalidaesModulo;
            BaseAPIClient<ProfileDTO> baseAPI = new BaseAPIClient<ProfileDTO>(TOKEN_API);
            List<long> listaFuncionalidadesPerfil;
            Toolbar toolbar;
            Container contenedor;

            Ext.Net.Label labelNombreModulo;
            Ext.Net.Button btnCerrarModulo;
            Checkbox chkFuncionalidad;

            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                string sPerfil = hdPerfilSeleccionado.Value.ToString();
                listaContenedores = new List<Container>();
                string sTra;
                if (sPerfil != "")
                {
                    ProfileDTO profile = baseAPI.GetByCode(sPerfil).Result.Value;
                    List<Clases.UserInterface> interfaces = ModulesController.GetUserInterfacesFromModuleCode(profile.ModuleCode);
                    List<Clases.UserFunctionality> functionalities = new List<UserFunctionality>();
                    if (profile.UserFuntionalities != null)
                    {
                        functionalities = ModulesController.GetUserFunctionalitiesFromProfile(profile);
                    }
                    List<Clases.UserFuntionalityType> funtionalityTypes = ModulesController.GetUserFuntionalityTypes();
                    List<string> listCodes = functionalities.Select(x => x.Code).ToList();
                    foreach (var oModulo in interfaces)
                    {
                        sTra = "";
                        foreach (var item in oModulo.Resource)
                        {
                            if (sTra != "")
                                sTra += "";
                            sTra += GetGlobalResource(item);
                        }
                        contenedor = new Container();
                        contenedor.Cls = "ContenedorPerfil";
                        contenedor.Layout = "fitLayout";
                        contenedor.Padding = 12;
                        contenedor.CustomConfig.Add(new ConfigItem("ModuloID", oModulo.Code));
                        labelNombreModulo = new Ext.Net.Label();
                        labelNombreModulo.Text = sTra;
                        labelNombreModulo.Cls = "spanLbl";
                        toolbar = new Toolbar();
                        toolbar.MinWidth = 480;
                        toolbar.Items.Add(labelNombreModulo);
                        toolbar.Items.Add(new ToolbarFill());
                        toolbar.Layout = "HBoxLayout";
                        toolbar.LayoutConfig.Add(new HBoxLayoutConfig { Align = HBoxAlign.Stretch });
                        btnCerrarModulo = new Ext.Net.Button();
                        btnCerrarModulo.Listeners.Click.Fn = "EliminarPaginaPerfil";
                        btnCerrarModulo.ToolTip = GetGlobalResource("btnEliminar.ToolTip");
                        btnCerrarModulo.Cls = "bntBasuraGrey";
                        btnCerrarModulo.Width = 32;
                        btnCerrarModulo.Height = 32;
                        btnCerrarModulo.MarginSpec = "0 30 0 0";
                        btnCerrarModulo.Hidden = VistaLectura;
                        toolbar.Items.Add(btnCerrarModulo);

                        contenedor.Items.Add(toolbar);

                        listaFuncionalidaesModulo = ModulesController.GetUserFunctionalitiesFromUserInterface(oModulo.Code);
                        listaFuncionalidaesModulo.Sort((x, y) => x.Code.CompareTo(y.Code));

                        foreach (var oFuncionalidades in listaFuncionalidaesModulo)
                        {
                            sTra = "";
                            foreach (var item in funtionalityTypes.FirstOrDefault(x => x.Code == oFuncionalidades.Code.Split('@')[1]).Resource)
                            {
                                if (sTra != "")
                                    sTra += "";
                                sTra += GetGlobalResource(item);
                            }
                            chkFuncionalidad = new Checkbox();
                            chkFuncionalidad.LabelAlign = Ext.Net.LabelAlign.Right;
                            chkFuncionalidad.Cls = "chkboxLabelGrid";
                            chkFuncionalidad.PaddingSpec = "0 20 0 20";
                            chkFuncionalidad.CustomConfig.Add(new ConfigItem("IDFuncionalidad", oFuncionalidades.Code));
                            chkFuncionalidad.CustomConfig.Add(new ConfigItem("ValorInicial", (listCodes.Contains(oFuncionalidades.Code))));
                            chkFuncionalidad.CustomConfig.Add(new ConfigItem("DoChange", false));
                            chkFuncionalidad.Listeners.Change.Fn = "ModificarPermisos";
                            chkFuncionalidad.Listeners.Render.Fn = "RenderChkFuncionalidad";
                            //####Sustituir por TipoFuncionalidad Alias
                            chkFuncionalidad.BoxLabel = sTra;
                            if (chkFuncionalidad.BoxLabel.Length > 30)
                            {
                                chkFuncionalidad.BoxLabel = chkFuncionalidad.BoxLabel.Substring(0, 25) + "...";
                            }
                            chkFuncionalidad.Disabled = VistaLectura;
                            contenedor.ContentControls.Add(chkFuncionalidad);
                        }
                        ctMain2.ContentControls.Add(contenedor);
                    }
                    if (Update)
                    {
                        ctMain2.UpdateContent();
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

        [DirectMethod]
        public DirectResponse PintarPermisosNuevaPagina()
        {
            ModulosController cModulos = new ModulosController();
            FuncionalidadesController cFuncionalidades = new FuncionalidadesController();
            Data.Modulos oModulo;
            List<Data.Vw_Funcionalidades> listaFuncionalidaesModulo;
            Toolbar toolbar;
            Container contenedor;
            Ext.Net.Label labelNombreModulo;
            Ext.Net.Button btnCerrarModulo;
            Checkbox chkFuncionalidad;

            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                long lPerfilID = long.Parse(hdPerfilSeleccionado.Value.ToString());
                if (lPerfilID != 0)
                {
                    List<string> listaModulos = hdPaginaSeleccionada.Value.ToString().Split(',').ToList();
                    foreach (var Mod in listaModulos)
                    {
                        oModulo = cModulos.GetItem(long.Parse(Mod));
                        contenedor = new Container();
                        contenedor.Cls = "ContenedorPerfil";
                        contenedor.Padding = 12;
                        contenedor.CustomConfig.Add(new ConfigItem("ModuloID", oModulo.ModuloID));
                        labelNombreModulo = new Ext.Net.Label();
                        if (oModulo.Alias != null && oModulo.Alias != "")
                        {
                            labelNombreModulo.Text = (GetGlobalResource(oModulo.Alias) != "") ? GetGlobalResource(oModulo.Alias) : oModulo.Modulo;
                        }
                        else
                        {
                            labelNombreModulo.Text = oModulo.Modulo;
                        }
                        labelNombreModulo.Cls = "spanLbl";
                        toolbar = new Toolbar();
                        toolbar.MinWidth = 480;
                        toolbar.Items.Add(labelNombreModulo);
                        toolbar.Items.Add(new ToolbarFill());
                        toolbar.Layout = "HBoxLayout";
                        toolbar.LayoutConfig.Add(new HBoxLayoutConfig { Align = HBoxAlign.Stretch });
                        btnCerrarModulo = new Ext.Net.Button();
                        btnCerrarModulo.Listeners.Click.Fn = "EliminarPaginaPerfil";
                        btnCerrarModulo.ToolTip = GetGlobalResource("btnEliminar.ToolTip");
                        btnCerrarModulo.Cls = "bntBasuraGrey";
                        btnCerrarModulo.Width = 32;
                        btnCerrarModulo.Height = 32;
                        btnCerrarModulo.MarginSpec = "0 30 0 0";
                        toolbar.Items.Add(btnCerrarModulo);
                        contenedor.Items.Add(toolbar);
                        listaFuncionalidaesModulo = cFuncionalidades.GetFuncionalidadesByModulo(oModulo.ModuloID);
                        foreach (Data.Vw_Funcionalidades oFuncionalidades in listaFuncionalidaesModulo)
                        {
                            chkFuncionalidad = new Checkbox();
                            chkFuncionalidad.LabelAlign = Ext.Net.LabelAlign.Right;
                            chkFuncionalidad.Cls = "chkboxLabelGrid";
                            chkFuncionalidad.PaddingSpec = "0 20 0 20";
                            chkFuncionalidad.CustomConfig.Add(new ConfigItem("IDFuncionalidad", oFuncionalidades.FuncionalidadID));
                            chkFuncionalidad.CustomConfig.Add(new ConfigItem("DoChange", true));
                            chkFuncionalidad.Listeners.Change.Fn = "ModificarPermisos";
                            if (oFuncionalidades.Alias != null && oFuncionalidades.Alias != "" && GetGlobalResource(oFuncionalidades.Alias) != "")
                            {
                                chkFuncionalidad.BoxLabel = GetGlobalResource(oFuncionalidades.Alias);
                            }
                            else if (oFuncionalidades.AliasFuncionalidadTipo != null && oFuncionalidades.AliasFuncionalidadTipo != "" && GetGlobalResource(oFuncionalidades.AliasFuncionalidadTipo) != "")
                            {
                                chkFuncionalidad.BoxLabel = GetGlobalResource(oFuncionalidades.AliasFuncionalidadTipo);
                            }
                            else
                            {
                                chkFuncionalidad.BoxLabel = oFuncionalidades.Funcionalidad;
                            }
                            if (chkFuncionalidad.BoxLabel.Length > 30)
                            {
                                chkFuncionalidad.BoxLabel = chkFuncionalidad.BoxLabel.Substring(0, 25) + "...";
                            }
                            contenedor.ContentControls.Add(chkFuncionalidad);
                        }
                        ctMain2.ContentControls.Add(contenedor);
                    }
                    PintarPermisosAsociados(false);
                    ctMain2.UpdateContent();
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

        [DirectMethod]
        public DirectResponse CambiarAsignacionPermisos(long FuncionalidadID, bool Añadir)
        {
            PerfilesFuncionalidadesController cController = new PerfilesFuncionalidadesController();
            Data.PerfilesFuncionalidades oDato;
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                long lPerfilID = long.Parse(hdPerfilSeleccionado.Value.ToString());
                if (lPerfilID != 0)
                {
                    if (!Añadir)
                    {
                        oDato = cController.GetRelacion(lPerfilID, FuncionalidadID);
                        if (cController.DeleteItem(oDato.PerfilFuncionalidadID))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        }
                    }
                    else
                    {
                        oDato = new Data.PerfilesFuncionalidades();
                        oDato.PerfilID = lPerfilID;
                        oDato.FuncionalidadID = FuncionalidadID;
                        if (cController.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                }
            }
            return direct;
        }

        #endregion

        #endregion

        #region FUNCIONALDIADES

        #region GESTION BASICA FUNCIONALIDADES

        [DirectMethod]
        public DirectResponse AgregarEditarFuncioanlidad(bool Agregar)
        {
            FuncionalidadesController cFuncionaliades = new FuncionalidadesController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";

                long lCliID = long.Parse(hdCliID.Value.ToString());

                if (!Agregar)
                {
                    object FuncionalidadID;
                    TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out FuncionalidadID);

                    Data.Funcionalidades oDato = cFuncionaliades.GetItem(long.Parse(FuncionalidadID.ToString()));
                    if (oDato.Funcionalidad == txtNombreFuncionalidad.Text && oDato.Codigo == long.Parse(nbCodigoFuncionalidad.Value.ToString()))
                    {
                        oDato.FuncionalidadTipoID = long.Parse(cmbTipoFuncionalidadFuncionalidad.Value.ToString());
                        oDato.Descripcion = txtaDescripcionFuncionalidad.Text;
                        oDato.Alias = txtAliasFuncionalidad.Text;
                    }
                    else
                    {
                        if (cFuncionaliades.RegistroDuplicadoNombre(txtNombreFuncionalidad.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Funcionalidad = txtNombreFuncionalidad.Text;
                            oDato.Codigo = long.Parse(nbCodigoFuncionalidad.Value.ToString());
                            oDato.FuncionalidadTipoID = long.Parse(cmbTipoFuncionalidadFuncionalidad.Value.ToString());
                            oDato.Descripcion = txtaDescripcionFuncionalidad.Text;
                            oDato.Alias = txtAliasFuncionalidad.Text;
                        }
                    }
                    if (cFuncionaliades.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        CargarArbolModulos();
                    }
                }
                else
                {
                    if (cFuncionaliades.RegistroDuplicadoNombre(txtNombreFuncionalidad.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else if (cFuncionaliades.RegistroDuplicado(long.Parse(nbCodigoFuncionalidad.Value.ToString())))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsCodigoExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Funcionalidades oDato = new Data.Funcionalidades();
                        object lModuloID;
                        TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lModuloID);
                        oDato.ModuloID = long.Parse(lModuloID.ToString());
                        oDato.Funcionalidad = txtNombreFuncionalidad.Text;
                        oDato.Codigo = long.Parse(nbCodigoFuncionalidad.Value.ToString());
                        oDato.FuncionalidadTipoID = long.Parse(cmbTipoFuncionalidadFuncionalidad.Value.ToString());
                        oDato.Descripcion = txtaDescripcionFuncionalidad.Text;
                        oDato.Alias = txtAliasFuncionalidad.Text;
                        oDato.Activo = true;
                        if (cFuncionaliades.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            CargarArbolModulos();
                        }
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

        [DirectMethod]
        public DirectResponse MostrarEditarFuncioanlidad()
        {
            FuncionalidadesController cFuncionalidades = new FuncionalidadesController();
            DirectResponse direct = new DirectResponse();
            try
            {
                object lFuncioanlidadID;
                direct.Success = true;
                direct.Result = "";
                TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lFuncioanlidadID);
                Data.Funcionalidades oDato = cFuncionalidades.GetItem(long.Parse(lFuncioanlidadID.ToString()));
                txtNombreFuncionalidad.SetValue(oDato.Funcionalidad);
                nbCodigoFuncionalidad.SetValue(oDato.Codigo);
                txtAliasFuncionalidad.SetValue(oDato.Alias);
                cmbTipoFuncionalidadFuncionalidad.SetValue(oDato.FuncionalidadTipoID);
                txtaDescripcionFuncionalidad.SetValue(oDato.Descripcion);
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
        public DirectResponse EliminarFuncioanlidad()
        {
            FuncionalidadesController cFuncionalidades = new FuncionalidadesController();
            DirectResponse direct = new DirectResponse();
            try
            {
                object lFuncioanlidadID;
                direct.Success = true;
                direct.Result = "";
                TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lFuncioanlidadID);
                if (cFuncionalidades.DeleteItem(long.Parse(lFuncioanlidadID.ToString())))
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

        [DirectMethod]
        public DirectResponse ActivarFunciuonlidad()
        {
            FuncionalidadesController cFuncionalidades = new FuncionalidadesController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                object lFuncionalidad;
                TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lFuncionalidad);

                Data.Funcionalidades oDato;
                oDato = cFuncionalidades.GetItem(long.Parse(lFuncionalidad.ToString()));
                oDato.Activo = !oDato.Activo;

                if (cFuncionalidades.UpdateItem(oDato))
                {
                    CargarArbolModulos();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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

        #region GESTION BASICA MODULOS

        [DirectMethod]
        public DirectResponse AgregarEditarModulo(bool Agregar)
        {
            ModulosController cModulos = new ModulosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";

                long lCliID = long.Parse(hdCliID.Value.ToString());

                if (!Agregar)
                {
                    object lModuloID;
                    TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lModuloID);

                    Data.Modulos oDato = cModulos.GetItem(long.Parse(lModuloID.ToString()));
                    if (oDato.Modulo == txtNombreModulo.Text)
                    {
                        oDato.Pagina = txtPaginaModulo.Text;
                        oDato.Alias = txtAliasModulo.Text;
                        oDato.Descripcion = txtaDescripcionModulo.Text;

                        if (CheckProduccion.Checked)
                        {
                            oDato.Produccion = true;
                        }
                        else
                        {
                            oDato.Produccion = false;
                        }

                        if (CheckSuperUser.Checked)
                        {
                            oDato.SuperUser = true;
                        }
                        else
                        {
                            oDato.SuperUser = false;
                        }
                    }
                    else
                    {
                        if (cModulos.RegistroDuplicado(txtNombreModulo.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Modulo = txtNombreModulo.Text;
                            oDato.Pagina = txtPaginaModulo.Text;
                            oDato.Alias = txtAliasModulo.Text;
                            oDato.Descripcion = txtaDescripcionModulo.Text;
                            if (CheckProduccion.Checked)
                            {
                                oDato.Produccion = true;
                            }
                            else
                            {
                                oDato.Produccion = false;
                            }

                            if (CheckSuperUser.Checked)
                            {
                                oDato.SuperUser = true;
                            }
                            else
                            {
                                oDato.SuperUser = false;
                            }
                        }
                    }
                    if (cModulos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        CargarArbolPerfiles();
                    }
                }
                else
                {
                    if (cModulos.RegistroDuplicado(txtNombreModulo.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        object lProyectoRipoID;
                        TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lProyectoRipoID);

                        Data.Modulos oDato = new Data.Modulos();
                        oDato.Modulo = txtNombreModulo.Text;
                        oDato.Pagina = txtPaginaModulo.Text;
                        oDato.Alias = txtAliasModulo.Text;
                        oDato.Descripcion = txtaDescripcionModulo.Text;
                        oDato.Activo = true;
                        oDato.ProyectoTipoID = long.Parse(lProyectoRipoID.ToString());
                        if (CheckProduccion.Checked)
                        {
                            oDato.Produccion = true;
                        }
                        else
                        {
                            oDato.Produccion = false;
                        }

                        if (CheckSuperUser.Checked)
                        {
                            oDato.SuperUser = true;
                        }
                        else
                        {
                            oDato.SuperUser = false;
                        }
                        if (cModulos.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            CargarArbolPerfiles();
                        }
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

        [DirectMethod]
        public DirectResponse MostrarEditarModulo()
        {
            ModulosController cModulos = new ModulosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                object lModuloID;
                direct.Success = true;
                direct.Result = "";
                TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lModuloID);
                Data.Modulos oDato = cModulos.GetItem(long.Parse(lModuloID.ToString()));
                txtNombreModulo.SetValue(oDato.Modulo);
                txtAliasModulo.SetValue(oDato.Alias);
                txtPaginaModulo.SetValue(oDato.Pagina);
                txtaDescripcionModulo.SetValue(oDato.Descripcion);
                if (oDato.SuperUser == true)
                {
                    CheckSuperUser.Checked = true;
                }
                if (oDato.Produccion == true)
                {
                    CheckProduccion.Checked = true;
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

        [DirectMethod]
        public DirectResponse EliminarModulo()
        {
            ModulosController cModulos = new ModulosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                object lModuloID;
                direct.Success = true;
                direct.Result = "";
                TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lModuloID);
                if (cModulos.DeleteItem(long.Parse(lModuloID.ToString())))
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

        [DirectMethod]
        public DirectResponse ActivarModulo()
        {
            ModulosController cModulos = new ModulosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                object lModuloID;
                TreeFuncionalidadesSelectionModel.SelectedNodes.First().Attributes.TryGetValue("ID", out lModuloID);

                Data.Modulos oDato;
                oDato = cModulos.GetItem(long.Parse(lModuloID.ToString()));
                oDato.Activo = !oDato.Activo;

                if (cModulos.UpdateItem(oDato))
                {
                    CargarArbolModulos();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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

        [DirectMethod]
        public DirectResponse DescargarFuncionalidades()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #region ARBOL

        [DirectMethod]
        public DirectResponse CargarArbolModulos()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strComun"),
                    Expanded = true
                };
                nodes.Add(nodoRaiz);
                nodoRaiz.Children.AddRange(ConstruirArbolFuncionalidades());
                TreePanelFuncionalidades.SetRootNode(nodoRaiz);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        private NodeCollection ConstruirArbolFuncionalidades()
        {
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
            PerfilesController cPerfiles = new PerfilesController();
            NodeCollection oNodes;

            try
            {
                List<Data.ProyectosTipos> listaPro = cProyectosTipos.GetAllProyectosTipos();
                oNodes = GetNodosProyectosTiposFuncionalidades(listaPro);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNodes = null;
            }

            return oNodes;
        }
        private NodeCollection GetNodosProyectosTiposFuncionalidades(List<Data.ProyectosTipos> listaPro)
        {
            NodeCollection oMenu = new NodeCollection(false);

            try
            {
                listaPro.ForEach(oItem =>
                {
                    bool tieneHijos = false;
                    Node oNodo = new Node
                    {
                        Text = "Proyecto",
                        Expanded = false,
                        Expandable = true,
                        NodeID = "Proy" + oItem.ProyectoTipoID.ToString()
                    };
                    oNodo.CustomAttributes.Add(new ConfigItem("Nombre", (GetGlobalResource(oItem.Alias) == "") ? oItem.ProyectoTipo : GetGlobalResource(oItem.Alias)));
                    oNodo.CustomAttributes.Add(new ConfigItem("ID", oItem.ProyectoTipoID));

                    NodeCollection nodoshijos = GetNodosModulos(oItem.ProyectoTipoID);
                    if (nodoshijos != null && nodoshijos.Count > 0)
                    {
                        tieneHijos = true;
                        oNodo.Children.AddRange(nodoshijos);
                    }
                    oNodo.Leaf = !tieneHijos;
                    oMenu.Add(oNodo);
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }

            return oMenu;
        }
        private NodeCollection GetNodosModulos(long PadreID)
        {
            ModulosController cModulos = new ModulosController();
            List<Data.Modulos> listaModulos;
            NodeCollection oMenu = new NodeCollection(false);
            try
            {
                bool tieneHijos = false;
                listaModulos = cModulos.getModulosbyProyectoTipo(PadreID);
                listaModulos.ForEach(oItem =>
                {
                    if ((btnSoloActivosFuncionalidades.Pressed && oItem.Activo) || !btnSoloActivosFuncionalidades.Pressed)
                    {
                        Node oNodo = new Node
                        {
                            Text = "Modulo",
                            Expanded = false,
                            Expandable = true,
                            NodeID = "Mod" + oItem.ModuloID.ToString()
                        };
                        if (!oItem.Activo)
                        {
                            oNodo.Cls = "itemArbolDesactivo";
                        }
                        oNodo.CustomAttributes.Add(new ConfigItem("Nombre", oItem.Modulo));
                        oNodo.CustomAttributes.Add(new ConfigItem("Info", oItem.Pagina));
                        oNodo.CustomAttributes.Add(new ConfigItem("SuperUser", oItem.SuperUser));
                        oNodo.CustomAttributes.Add(new ConfigItem("Produccion", oItem.Produccion));
                        oNodo.CustomAttributes.Add(new ConfigItem("Descripcion", oItem.Descripcion));
                        oNodo.CustomAttributes.Add(new ConfigItem("ID", oItem.ModuloID));
                        NodeCollection nodoshijos = GetNodosFuncionalidades(oItem.ModuloID);
                        if (nodoshijos != null && nodoshijos.Count > 0)
                        {
                            tieneHijos = true;
                            oNodo.Children.AddRange(nodoshijos);
                        }
                        oNodo.Leaf = !tieneHijos;
                        oMenu.Add(oNodo);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }

            return oMenu;
        }

        private NodeCollection GetNodosFuncionalidades(long PadreID)
        {

            FuncionalidadesController cFuncionalidades = new FuncionalidadesController();
            List<Data.Vw_Funcionalidades> listaFuncionalidades;
            NodeCollection oMenu = new NodeCollection(false);
            try
            {
                bool tieneHijos = false;
                listaFuncionalidades = cFuncionalidades.GetFuncionalidadesByModuloAll(PadreID);
                listaFuncionalidades.ForEach(oItem =>
                {
                    if ((btnSoloActivosFuncionalidades.Pressed && oItem.Activo) || !btnSoloActivosFuncionalidades.Pressed)
                    {
                        Node oNodo = new Node
                        {
                            Text = "Funcionalidad",
                            Expanded = false,
                            Expandable = false,
                            NodeID = "Fun" + oItem.FuncionalidadID.ToString()
                        };
                        if (!oItem.Activo)
                        {
                            oNodo.Cls = "itemArbolDesactivo";
                        }
                        oNodo.CustomAttributes.Add(new ConfigItem("Nombre", oItem.Funcionalidad));
                        oNodo.CustomAttributes.Add(new ConfigItem("Info", oItem.Codigo));
                        if (oItem.AliasFuncionalidadTipo != null)
                        {
                            oNodo.CustomAttributes.Add(new ConfigItem("TipoFuncionalidad", (GetGlobalResource(oItem.AliasFuncionalidadTipo) == "") ? "TRADUCIR TIPO FUNCIONALIDAD" : GetGlobalResource(oItem.AliasFuncionalidadTipo)));
                        }
                        oNodo.CustomAttributes.Add(new ConfigItem("Descripcion", oItem.Descripcion));
                        oNodo.CustomAttributes.Add(new ConfigItem("ID", oItem.FuncionalidadID));
                        oNodo.Leaf = true;
                        oMenu.Add(oNodo);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }

            return oMenu;
        }

        #endregion

        #endregion


        #region ROLES

        [DirectMethod]
        public DirectResponse AgregarEditarRoles(bool Agregar)
        {
            RolesController cRoles = new RolesController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";

                long lCliID = long.Parse(hdCliID.Value.ToString());

                if (!Agregar)
                {

                    Data.Roles oDato = cRoles.GetItem(long.Parse(hdRolSeleccionado.Value.ToString()));
                    if (oDato.Nombre == txtNombre.Text)
                    {
                        if (oDato.Codigo != txtCodigo.Text)
                        {
                            if (cRoles.registroDuplicadoCodigo(txtCodigo.Text, lCliID))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            }
                            else
                            {
                                oDato.Nombre = txtNombre.Text;
                                oDato.Codigo = txtCodigo.Text;
                                oDato.Descripcion = txtaDescripcion.Text;
                            }
                        }
                        else
                        {
                            oDato.Nombre = txtNombre.Text;
                            oDato.Codigo = txtCodigo.Text;
                            oDato.Descripcion = txtaDescripcion.Text;
                        }
                    }
                    else
                    {
                        if (cRoles.registroDuplicadoNombre(txtNombre.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            if (oDato.Codigo != txtCodigo.Text)
                            {
                                if (cRoles.registroDuplicadoCodigo(txtCodigo.Text, lCliID))
                                {
                                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                }
                                else
                                {
                                    oDato.Nombre = txtNombre.Text;
                                    oDato.Codigo = txtCodigo.Text;
                                    oDato.Descripcion = txtaDescripcion.Text;
                                }
                            }
                            else
                            {
                                oDato.Nombre = txtNombre.Text;
                                oDato.Codigo = txtCodigo.Text;
                                oDato.Descripcion = txtaDescripcion.Text;
                            }
                        }
                    }

                    if (cRoles.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                }
                else
                {
                    if (cRoles.registroDuplicado(txtNombre.Text, txtCodigo.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Roles oDato = new Data.Roles();
                        oDato.Nombre = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.ClienteID = lCliID;
                        oDato.Descripcion = txtaDescripcion.Text;
                        oDato.Activo = true;
                        if (cRoles.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            CargarArbolPerfiles();
                        }
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


        [DirectMethod()]
        public DirectResponse MostrarEditarRoles()
        {
            DirectResponse direct = new DirectResponse();
            RolesController cRoles = new RolesController();

            try
            {
                Data.Roles dato = new Data.Roles();

                long rolID = long.Parse(hdRolSeleccionado.Value.ToString());
                dato = cRoles.GetItem(rolID);


                txtNombre.Value = dato.Nombre;
                txtCodigo.Value = dato.Codigo;

                if (dato.Descripcion != null)
                {
                    txtaDescripcion.Value = dato.Descripcion;
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
        public DirectResponse EliminarRoles()
        {
            DirectResponse direct = new DirectResponse();
            RolesController cRoles = new RolesController();
            long S = long.Parse(hdRolSeleccionado.Value.ToString());
            try
            {
                cRoles.DeleteItem(S);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Success = true;
                direct.Result = "";
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
        public DirectResponse ActivarRoles()
        {
            DirectResponse direct = new DirectResponse();
            RolesController cController = new RolesController();

            try
            {
                long lID = long.Parse(hdRolSeleccionado.Value.ToString());

                Data.Roles oDato;
                oDato = cController.GetItem(lID);

                if (oDato.Activo)
                {
                    oDato.Activo = false;
                }
                else
                {
                    oDato.Activo = true;
                }

                if (cController.UpdateItem(oDato))
                {
                    direct.Success = true;
                    direct.Result = "";
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
        public DirectResponse AgregarPerfilRol(long perfilID)
        {
            DirectResponse direct = new DirectResponse();
            RolesPerfilesController cController = new RolesPerfilesController();

            try
            {
                long lID = long.Parse(hdRolSeleccionado.Value.ToString());
                if (!cController.RegistroDuplicado(lID, perfilID))
                {
                    RolesPerfiles dato = new RolesPerfiles();
                    dato.PerfilID = perfilID;
                    dato.RolID = lID;
                    cController.AddItem(dato);
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
        public DirectResponse EliminarPerfilRol(long perfilID)
        {
            DirectResponse direct = new DirectResponse();
            RolesPerfilesController cController = new RolesPerfilesController();

            try
            {
                long lID = long.Parse(hdRolSeleccionado.Value.ToString());

                RolesPerfiles dato = cController.GetRolesPerfilesByPerfilRol(lID, perfilID);
                cController.DeleteItem(dato.RolPerfilID);
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
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

        #endregion

        #endregion

        #region FUNCTIONS

        #endregion
    }
}