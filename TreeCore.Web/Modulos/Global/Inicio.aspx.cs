using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using TreeCore.Data;
using System.Reflection;
using log4net;

namespace TreeCore.ModGlobal.pages
{
    public partial class Inicio : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public List<string> listaClavesModulos = new List<string>();
        public long lProyectoID;

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

                Comun.CreateGridFilters(gridFilters, storeTareas, grdTask.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grdTask, storeTareas, grdTask.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                if (Usuario != null)
                {
                    cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                }
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                if (Request.Params["aux3"] != null)
                {
                    lProyectoID = long.Parse(Request.Params["aux3"]);
                    hdProyectoID.Value = lProyectoID.ToString();
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
                        long lModulo = Convert.ToInt32(hdProyectoID.Value.ToString());
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

        }

        #endregion

        #region STORES

        #region TAREAS

        protected void storeTareas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];                   
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region DEPARTAMENTOS
        protected void storeDepartamentos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            try
            {
                DepartamentosController cDepartamentos = new DepartamentosController();

                var vLista = cDepartamentos.GetAll();

                if (vLista != null)
                {
                    storeDepartamentos.DataSource = vLista;
                }

                cDepartamentos = null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion

        #region PROYECTOS

        protected void storeProyectos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            try
            {
                ProyectosController cProyectos = new ProyectosController();

                var vLista = cProyectos.GetAll();

                if (vLista != null)
                {
                    storeProyectos.DataSource = vLista;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion

        #region NOTIFICACIONES

        protected void storeNotificaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            try
            {
                NotificacionesController cNotificaciones = new NotificacionesController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                var vLista = cNotificaciones.GetAll(lCliID);

                if (vLista != null)
                {
                    storeNotificaciones.DataSource = vLista;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region FAVORITOS
        protected void storeGestFav_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            MenusController cMenus = new MenusController();
            try
            {
                string clave = String.Empty;
                listaClavesModulos.ForEach(mod => { if (GetGlobalResource(mod).Equals(cbGestFav.Text)) { clave = mod; } });
                List<Data.Vw_Menus> listaMenus = cMenus.GetVwActivosByNombreModuloAndFuncionalidades(clave, ((List<long>)(this.Session["FUNCIONALIDADES"])));
                List<long> listaMenusID = new List<long>();
                List<object> nombresMenus = new List<object>();

                
                listaMenus.ForEach(menu =>
                {
                    if (menu.ModuloID != null)
                    {
                        nombresMenus.Add(new { Pagina = cMenus.getNombreByAliasFromBasePageExtNet(menu, this), menuID = menu.MenuID });
                        listaMenusID.Add(menu.MenuID);
                    }
                });
                cMenus.GetMenusFavoritosByUsuarioID(Usuario.UsuarioID).ForEach(menu =>
                {
                    if (!listaMenusID.Contains(menu.MenuID))
                    {
                        if (GetGlobalResource(cMenus.GetItem<Vw_Menus>(menu.MenuID).NombreModulo).Equals(cbGestFav.Text))
                        {
                            nombresMenus.Add(new { Pagina = cMenus.getNombreByAliasFromBasePageExtNet(cMenus.GetItem<Vw_Menus>(menu.MenuID), this), menuID = menu.MenuID });
                        }
                    }
                });

                storeGestFav.DataSource = nombresMenus;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        protected void storeModulos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            MenusController cMenus = new MenusController();
            try
            {
                List<string> listaNombreModulos = cMenus.GetNombreModulosByFuncionalidades(((List<long>)(this.Session["FUNCIONALIDADES"]))).Distinct().ToList();
                List<object> nombresModulos = new List<object>();

                listaNombreModulos.ForEach(mod =>
                {
                    string clave = GetGlobalResource(mod);
                    if (!GetGlobalResource(mod).StartsWith("ext-"))
                    {
                        nombresModulos.Add(new { Modulo = GetGlobalResource(mod) });
                    }
                });
                cMenus.GetMenusFavoritosByUsuarioID(Usuario.UsuarioID).ForEach(menu =>
                {
                    if (!listaNombreModulos.Contains(cMenus.GetItem<Vw_Menus>(menu.MenuID).NombreModulo))
                    {
                        if (!GetGlobalResource(cMenus.GetItem<Vw_Menus>(menu.MenuID).NombreModulo).StartsWith("ext-"))
                        {
                            nombresModulos.Add(new { Modulo = GetGlobalResource(cMenus.GetItem<Vw_Menus>(menu.MenuID).NombreModulo) });
                        }
                    }
                });

                nombresModulos.Sort((x, y) => x.GetType().GetProperty("Modulo").GetValue(x).ToString().CompareTo(y.GetType().GetProperty("Modulo").GetValue(y).ToString()));
                storeModulos.DataSource = nombresModulos;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse CalcularMedia(string sModulo)
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;
            return direct;
        }

        [DirectMethod()]
        public DirectResponse AgregarEditarNotif(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            NotificacionesController cNotificaciones = new NotificacionesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());

            try
            {
                if (!bAgregar)
                {

                }
                else
                {
                    Data.Notificaciones oNotificacion = new Data.Notificaciones();

                    oNotificacion.Notificacion = txtTitle.Text;
                    oNotificacion.Contenido = txaText.Text;
                    oNotificacion.ClienteID = lCliID;
                    oNotificacion.FechaCreacion = DateTime.Now;

                    if (cNotificaciones.AddItem(oNotificacion) != null)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storeNotificaciones.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarNotif()
        {
            DirectResponse direct = new DirectResponse();
            NotificacionesController cNotificaciones = new NotificacionesController();

            try
            {
                Notificaciones oNotificacion = new Notificaciones();
                oNotificacion = cNotificaciones.GetItem("");

                txtTitle.Text = oNotificacion.Notificacion;
                txaText.Text = oNotificacion.Contenido;
                txtUser.Text = oNotificacion.Clientes.Cliente;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public void CargarPaginasFavoritas()
        {
            MenusController cMenus = new MenusController();
            try
            {
                RowSelectionModel smGestFav = gridGestFav.GetSelectionModel() as RowSelectionModel;

                List<MenuFavoritosUsuarios> listaMenusFav = cMenus.GetMenusFavoritosByUsuarioID(Usuario.UsuarioID);

                listaMenusFav.ForEach(fav => {
                    smGestFav.SelectedRows.Add(new SelectedRow(fav.MenuID.ToString()));
                });
                smGestFav.UpdateSelection();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod()]
        public void AñadirPaginasFavoritas(object elementosSeleccionados)
        {
            MenusController cMenus = new MenusController();
            MenuFavoritosUsuariosController cMenusFav = new MenuFavoritosUsuariosController();
            try
            {
                #region TRANSFORMAR DE OBJECT A LIST
                List<String> datos = new List<string>();
                string stringArray = elementosSeleccionados.ToString().Substring(1);
                stringArray = stringArray.Substring(0, stringArray.Length - 1);
                foreach (string s1 in stringArray.Split(','))
                {
                    foreach (string s2 in s1.Split('"'))
                    {
                        if (!s2.Equals("") && !s2.Contains("\r\n"))
                        {
                            datos.Add(s2);
                        }
                    }
                }
                #endregion
                string clave = String.Empty;
                listaClavesModulos.ForEach(mod => { if (GetGlobalResource(mod).Equals(cbGestFav.Text)) { clave = mod; } });
                List<Vw_Menus> listaMenus = cMenus.GetVwActivosByNombreModuloAndFuncionalidades(clave, ((List<long>)(this.Session["FUNCIONALIDADES"])));
                cMenus.GetMenusFavoritosByUsuarioID(Usuario.UsuarioID).ForEach(menu =>
                {
                    if (!listaMenus.Contains(cMenus.GetItem<Vw_Menus>(menu.MenuID))) { listaMenus.Add(cMenus.GetItem<Vw_Menus>(menu.MenuID)); }
                });
                #region ELIMINAR ANTERIORES FAVORITOS
                listaMenus.ForEach(vwMenu =>
                {
                    if (cMenus.GetMenuFavoritoByUsuarioIDAndMenuID(Usuario.UsuarioID, vwMenu.MenuID) != null && GetGlobalResource(vwMenu.NombreModulo).Equals(cbGestFav.Text))
                    {
                        cMenusFav.DeleteItem(cMenus.GetMenuFavoritoByUsuarioIDAndMenuID(Usuario.UsuarioID, vwMenu.MenuID).MenuFavoritoUsuarioID);
                    }
                });
                #endregion
                int cont = 0;
                foreach (string row in datos)
                {
                    MenuFavoritosUsuarios newMenuFav = new MenuFavoritosUsuarios();
                    listaMenus.ForEach(menu =>
                    {
                        if (menu.Nombre == row)
                        {
                            newMenuFav.MenuID = menu.MenuID;
                        }
                    });
                    if (newMenuFav.MenuID == 0)
                    {
                        listaMenus.ForEach(menu =>
                        {
                            if (GetGlobalResource(menu.Alias) == row)
                            {
                                newMenuFav.MenuID = menu.MenuID;
                            }
                        });
                    }
                    if (newMenuFav.MenuID == 0)
                    {
                        listaMenus.ForEach(menu =>
                        {
                            if (GetGlobalResource(menu.AliasModulo) == row)
                            {
                                newMenuFav.MenuID = menu.MenuID;
                            }
                        });
                    }
                    if (newMenuFav.MenuID == 0)
                    {
                        listaMenus.ForEach(menu =>
                        {
                            if (menu.DescripcionPagina == row)
                            {
                                newMenuFav.MenuID = menu.MenuID;
                            }
                        });
                    }
                    newMenuFav.UsuarioID = Usuario.UsuarioID;
                    if (cont < 4)
                    {
                        bool existe = false;
                        cMenusFav.GetFavoritosByUsuarioID(Usuario.UsuarioID).ForEach(menuFav =>
                        {
                            if (menuFav.MenuID == newMenuFav.MenuID) { existe = true; }
                        });
                        if (!existe) { newMenuFav = cMenusFav.AddItem(newMenuFav); }
                        cont++;
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeBox("ERROR", ex.Message, MessageBox.Icon.ERROR, ex);
                log.Error(ex.Message);
            }
        }

        #endregion

        #region FUNCTIONS

        #region CARGAR FAVORITOS

        [DirectMethod()]
        public string cargarFavoritos()
        {
            MenuFavoritosUsuariosController cFavoritos = new MenuFavoritosUsuariosController();
            List<Vw_MenuFavoritosUsuarios> listaVista;
            MenusController cMenus = new MenusController();
            NodeCollection nodes = new NodeCollection();

            try
            {
                listaVista = cFavoritos.GetAllOrderByMenuModulo(Usuario.UsuarioID);

                listaVista.ForEach(oItem =>
                {
                    Node oNodo = new Node
                    {
                        NodeID = oItem.MenuID.ToString()
                    };

                    Vw_Menus menu = cMenus.GetItem<Vw_Menus>(oItem.MenuID.Value);

                    oNodo.CustomAttributes.Add(new ConfigItem(Comun.MenuID, oItem.MenuID.ToString(), ParameterMode.Value));
                    oNodo.CustomAttributes.Add(new ConfigItem(Comun.MenuModulo, GetGlobalResource(oItem.MenuModulo), ParameterMode.Value));
                    oNodo.CustomAttributes.Add(new ConfigItem(Comun.Nombre, cMenus.getNombreByAliasFromBasePageExtNet(menu, this), ParameterMode.Value));
                    if (oItem.RutaPagina != null)
                    {
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.RutaPagina, oItem.RutaPagina, ParameterMode.Value));
                    }
                    else
                    {
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.RutaPagina, Comun.PaginasComunes + menu.Pagina, ParameterMode.Value));

                        string sPadre = "";
                        Comun.NombresModulos.TryGetValue(menu.NombreModulo, out sPadre);

                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.RutaPaginaPadre, sPadre, ParameterMode.Value));
                    }
                    oNodo.CustomAttributes.Add(new ConfigItem(Comun.ICONO, Comun.rutaIconoWeb(oItem.Icono), ParameterMode.Value));

                    nodes.Add(oNodo);

                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return nodes.ToJson();
        }

        #endregion

        #endregion
    }
}