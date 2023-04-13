using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;
using TreeCore.Page;

namespace TreeCore.ModGlobal
{
    public partial class MenuLateral : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region Variables
        private static List<object> listaIconos;
        #endregion

        #region GESTIÓN DE PÁGINA

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

                #region NIVEL MAXIMO PERMITIDO
                ParametrosController cParametros = new ParametrosController();
                string sNivel = cParametros.GetItemValor(Comun.MENU_NIVEL_MAXIMO);
                if (!string.IsNullOrEmpty(sNivel))
                {
                    hd_NivelMaxPermitido.Value = sNivel;
                }
                else
                {
                    hd_NivelMaxPermitido.Value = 3;
                }

                #endregion
            }

            if (listaIconos == null || listaIconos.Count <= 0)
            {
                listaIconos = GetIconsOfDataSystem();
            }

            if (!ClienteID.HasValue)
            {
                cmbModulo.Hidden = false;
                cmbPaginaModulo.Hidden = false;
                txtParametro.Hidden = false;
                txtKeyTraduccion.Hidden = false;

                colKey.Hidden = false;
                colPagina.Hidden = false;
                colNombreModulo.Hidden = false;
                colParametros.Hidden = false;
            }
            else
            {
                cmbModulo.Hidden = true;
                cmbPaginaModulo.Hidden = true;
                txtParametro.Hidden = true;
                txtKeyTraduccion.Hidden = true;

                colKey.Hidden = true;
                colPagina.Hidden = true;
                colNombreModulo.Hidden = true;
                colParametros.Hidden = true;
                btnEliminar.Hidden = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { }},
                { "Post", new List<ComponentBase> { btnAnadir }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #endregion

        #region STORES
        protected void storeMenuModulo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Data.MenusModulos> lista;

                try
                {
                    MenusModulosController cMenusModulos = new MenusModulosController();
                    lista = cMenusModulos.GetActivos();

                    lista.ForEach(m => {
                        string valor = GetGlobalResource(m.Modulo);
                        m.Modulo = (valor == "") ? "" : valor; 
                    });

                    lista.RemoveAll(m => m.Modulo == "");

                    storeMenuModulo.DataSource = lista;

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

        protected void storeModulo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Data.Modulos> lista;

                try
                {
                    ModulosController cModulos = new ModulosController();
                    UsuariosController cUsuarios = new UsuariosController();

                    if (cUsuarios.isSuper(Usuario.UsuarioID))
                    {
                        lista = cModulos.GetItemList();
                    }
                    else
                    {
                        lista = cModulos.getModulosNoSuper();
                    }

                    lista = cModulos.GetItemList();

                    storeModulo.DataSource = lista;

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

        protected void storePaginaModulo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Data.MenusModulos> lista;

                try
                {
                    MenusModulosController cMenusModulos = new MenusModulosController();
                    lista = cMenusModulos.GetActivos();

                    lista.ForEach(m => {
                        string valor = GetGlobalResource(m.Modulo);
                        m.Modulo = (valor == "") ? "" : valor;
                    });

                    lista.RemoveAll(m => m.Modulo == "");

                    storePaginaModulo.DataSource = lista;

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

        protected void storeIcono_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    if (listaIconos == null || listaIconos.Count <= 0)
                    {
                        listaIconos = GetIconsOfDataSystem();
                    }
                    storeIcono.DataSource = listaIconos;
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            MenusController cMenus = new MenusController();
            long lModuloGeneral = 0;

            try
            {
                if (cmbMenuModulo.SelectedItem.Value != null)
                {
                    lModuloGeneral = Convert.ToInt32(cmbMenuModulo.SelectedItem.Value);
                }

                if (!bAgregar)
                {
                    long lS = Convert.ToInt64(hd_MenuSeleccionado.Value);

                    Data.Menus oDato;
                    oDato = cMenus.GetItem(lS);

                    if (oDato.Nombre == txtNombre.Text)
                    {
                        oDato.Alias = txtKeyTraduccion.Text;
                        oDato.MenuModuloID = Convert.ToInt32(cmbMenuModulo.SelectedItem.Value);
                        oDato.Parametros = txtParametro.Text;
                        oDato.Expandido = chkExpandido.Checked;
                        oDato.Nuevo = chkNuevo.Checked;
                        oDato.Actualizado = chkActualizado.Checked;

                        if (cmbIcono.SelectedItem.Value != null)
                        {
                            //iconoConExtension
                            oDato.Icono = cmbIcono.SelectedItem.Value;
                        }
                        else
                        {
                            oDato.Icono = string.Empty;
                        }
                        if (cmbModulo.SelectedItem.Value != null)
                        {
                            oDato.ModuloID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                        }
                        else
                        {
                            oDato.ModuloID = null;
                        }
                        if (cmbPaginaModulo.SelectedItem.Value != null)
                        {
                            oDato.PaginaMenuModuloID = Convert.ToInt32(cmbPaginaModulo.SelectedItem.Value);
                        }
                        else
                        {
                            oDato.PaginaMenuModuloID = null;
                        }
                    }
                    else
                    {

                        oDato.Nombre = txtNombre.Text;
                        oDato.Alias = txtKeyTraduccion.Text;
                        oDato.MenuModuloID = Convert.ToInt32(cmbMenuModulo.SelectedItem.Value);
                        oDato.Parametros = txtParametro.Text;
                        oDato.Expandido = chkExpandido.Checked;
                        oDato.Nuevo = chkNuevo.Checked;
                        oDato.Actualizado = chkActualizado.Checked;

                        if (cmbIcono.SelectedItem.Value != null)
                        {
                            //iconoConExtension
                            oDato.Icono = cmbIcono.SelectedItem.Value;
                        }
                        else
                        {
                            oDato.Icono = string.Empty;
                        }

                        if (cmbModulo.SelectedItem.Value != null)
                        {
                            oDato.ModuloID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                        }
                        if (cmbPaginaModulo.SelectedItem.Value != null)
                        {
                            oDato.PaginaMenuModuloID = Convert.ToInt32(cmbPaginaModulo.SelectedItem.Value);
                        }

                    }

                    if (cMenus.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                }
                else
                {
                    long? menuPadreID = null;
                    if (HasSeleccionado())
                    {
                        menuPadreID = Convert.ToInt64(hd_MenuSeleccionado.Value);
                    }

                    Data.Menus oDato = new Data.Menus
                    {
                        Nombre = txtNombre.Text,
                        Alias = txtKeyTraduccion.Text,
                        MenuModuloID = Convert.ToInt32(cmbMenuModulo.SelectedItem.Value),
                        Parametros = txtParametro.Text,
                        Expandido = chkExpandido.Checked,
                        Nuevo = chkNuevo.Checked,
                        Actualizado = chkActualizado.Checked,
                        Activo = true,
                        MenuPadreID = menuPadreID
                    };

                    if (cmbIcono.SelectedItem.Value != null)
                    {
                        oDato.Icono = cmbIcono.SelectedItem.Value;
                    }
                    else
                    {
                        oDato.Icono = string.Empty;
                    }
                    if (cmbModulo.SelectedItem.Value != null)
                    {
                        oDato.ModuloID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                    }
                    if (cmbPaginaModulo.SelectedItem.Value != null)
                    {
                        oDato.PaginaMenuModuloID = Convert.ToInt32(cmbPaginaModulo.SelectedItem.Value);
                    }

                    if (cMenus.AddItem(oDato) != null)
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
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            MenusController cBancos = new MenusController();

            try
            {
                if (HasSeleccionado())
                {
                    long lS = Convert.ToInt64(hd_MenuSeleccionado.Value);

                    Data.Menus oDato;
                    oDato = cBancos.GetItem(lS);

                    txtNombre.Text = oDato.Nombre;
                    txtKeyTraduccion.Text = oDato.Alias;
                    cmbIcono.SetValue(oDato.Icono);
                    cmbModulo.SetValue(oDato.ModuloID.ToString());
                    cmbMenuModulo.SetValue(oDato.MenuModuloID.ToString());
                    cmbPaginaModulo.SetValue(oDato.PaginaMenuModuloID.ToString());
                    txtParametro.Text = oDato.Parametros;
                    chkExpandido.Checked = oDato.Expandido;
                    chkNuevo.Checked = oDato.Nuevo;
                    chkActualizado.Checked = oDato.Actualizado;

                    winGestion.Show();
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            MenusController cMenus = new MenusController();

            if (HasSeleccionado())
            {
                long lID = Convert.ToInt64(hd_MenuSeleccionado.Value);

                try
                {
                    if (cMenus.HasChildren(lID))
                    {
                        direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                        direct.Success = false;
                    }
                    else if (cMenus.DeleteItem(lID))
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
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            MenusController cController = new MenusController();

            try
            {
                if (HasSeleccionado())
                {
                    long lID = Convert.ToInt64(hd_MenuSeleccionado.Value);

                    Data.Menus oDato;
                    oDato = cController.GetItem(lID);
                    oDato.Activo = !oDato.Activo;

                    if (cController.UpdateItem(oDato))
                    {
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
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
        public string RefreshMenu()
        {
            bool rootVacio = true;
            NodeCollection nodes = new NodeCollection();
            Node nodoRaiz = new Node
            {
                Text = GetGlobalResource(Comun.strRaiz),
                Expanded = true
            };

            nodes.Add(nodoRaiz);

            if (cmbMenuModulo.SelectedItem != null && cmbMenuModulo.SelectedItem.Value != null && cmbMenuModulo.SelectedItem.Value != "")
            {
                nodoRaiz.Children.AddRange(this.ConstruirArbol());
            }
            else
            {
                rootVacio = false;
                nodoRaiz.Children.AddRange(new NodeCollection());
            }

            nodoRaiz.CustomAttributes.Add(new ConfigItem(Comun.ROOT_VACIO, (rootVacio) ? Comun.TRUE : Comun.FALSE, ParameterMode.Raw));

            return nodes.ToJson();
        }

        [DirectMethod()]
        public DirectResponse DropNodo(string targetID, string destinationID)
        {
            DirectResponse direct = new DirectResponse();
            MenusController cMenu = new MenusController();

            try
            {
                Data.Menus oDato = cMenu.GetItem(Convert.ToInt64(targetID));

                if (destinationID == "root")
                {
                    oDato.MenuPadreID = null;
                }
                else
                {
                    oDato.MenuPadreID = Convert.ToInt64(destinationID);
                }

                if (cMenu.UpdateItem(oDato))
                {
                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
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
        public DirectResponse BeforeDropNodo(string targetID, string destinationID)
        {
            DirectResponse direct = new DirectResponse();
            MenusController cMenu = new MenusController();

            bool bTodoOK = true;

            try
            {
                Data.Menus oTarget = cMenu.GetItem(Convert.ToInt64(targetID));
                Data.Menus oDestino = null;

                if (destinationID != "root")
                {
                    oDestino = cMenu.GetItem(Convert.ToInt64(destinationID));

                    #region Nivel maximo permitido

                    int iNivel = cMenu.GetNivelNodo(oDestino.MenuID);
                    int iMaxNivel = Convert.ToInt32(hd_NivelMaxPermitido.Value);

                    if (cMenu.HasChildren(oTarget.MenuID))
                    {
                        iNivel += cMenu.GetMaxDepth(oTarget.MenuID, iMaxNivel);
                    }

                    if (iNivel >= iMaxNivel)
                    {
                        bTodoOK = false;
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsNivelMenuNoPermitido);
                    }

                    #endregion

                    #region Drop en hoja

                    if (oDestino.PaginaMenuModuloID != null)
                    {
                        bTodoOK = false;
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsAgregarEnPaginaNoPermitido);
                    }

                    #endregion
                }

                if (bTodoOK)
                {
                    direct.Success = true;
                    direct.Result = "";

                    DropNodo(targetID, destinationID);
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

        #region FUNCTIONS
        private NodeCollection ConstruirArbol()
        {
            MenusController cMenus = new MenusController();
            NodeCollection oNodes;

            try
            {
                List<Data.Vw_Menus> listaMenus = cMenus.GetVwByModuloID(long.Parse(cmbMenuModulo.SelectedItem.Value), Usuario.EMail);

                oNodes = GetNodosHijos(null, listaMenus, 0);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNodes = null;
            }

            return oNodes;
        }

        private NodeCollection GetNodosHijos(long? lPadre, List<Data.Vw_Menus> listaMenus, long nivel)
        {
            NodeCollection oMenu = new NodeCollection(false);
            MenusController cMenus = new MenusController();
            nivel++;

            try
            {
                listaMenus.ForEach(oItem =>
                {
                    if (oItem.MenuPadreID == lPadre)
                    {
                        string nombreAlias = cMenus.getNombreByAliasFromBasePageExtNet(oItem, this);

                        Node oNodo = new Node
                        {
                            Text = nombreAlias,
                            Expanded = true,
                            Expandable = true,
                            NodeID = oItem.MenuID.ToString(),
                            IconFile = Comun.rutaIconoWeb(oItem.Icono)
                        };

                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.ACTIVO, (oItem.ActivoMenu) ? Comun.TRUE : Comun.FALSE, ParameterMode.Raw));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.NUEVO, (oItem.Nuevo) ? Comun.TRUE : Comun.FALSE, ParameterMode.Raw));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.ACTUALIZADO, (oItem.Actualizado) ? Comun.TRUE : Comun.FALSE, ParameterMode.Raw));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.EXPANDIDO, (oItem.Expandido) ? Comun.TRUE : Comun.FALSE, ParameterMode.Raw));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.ICONO, Comun.rutaIconoWeb(oItem.Icono), ParameterMode.Value));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.ALIAS, oItem.Alias, ParameterMode.Value));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.NOMBRE_MODULO, oItem.NombreModulo, ParameterMode.Value));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.PAGINA, oItem.Pagina, ParameterMode.Value));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.PARAMETROS, oItem.Parametros, ParameterMode.Value));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.NIVEL_SELECCIONADO, nivel.ToString(), ParameterMode.Raw));


                        NodeCollection nodoshijos = GetNodosHijos(oItem.MenuID, listaMenus, nivel);
                        if (oItem.Pagina != "" || (nodoshijos != null && nodoshijos.Count > 0))
                        {
                            oNodo.Leaf = false;
                            oNodo.Children.AddRange(nodoshijos);
                        }
                        else
                        {
                            oNodo.Leaf = false;
                        }

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

        private List<object> GetIconsOfDataSystem()
        {
            List<object> lista = new List<object>();

            string prefix = "ico-*";
            try
            {
                string[] fileEntries = Directory.GetFiles(Comun.rutaIconoSystem(), prefix);
                foreach (string filePath in fileEntries)
                {
                    //string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string fileNameExtension = Path.GetFileName(filePath);

                    lista.Add(new object[] { Comun.rutaIconoWeb(fileNameExtension), fileNameExtension });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        private bool HasSeleccionado()
        {
            return Convert.ToString(hd_MenuSeleccionado.Value) != "" && Convert.ToString(hd_MenuSeleccionado.Value) != Comun.ROOT;
        }

        //public static string rutaIconoWeb(string archivo)
        //{
        //    string ruta = string.Empty;
        //    try
        //    {
        //        ruta = string.Concat(GetDirectorioICono(), archivo);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        ruta = archivo;
        //    }

        //    return ruta;
        //}

        //public static string rutaIconoSystem()
        //{
        //    string ruta = string.Empty;
        //    try
        //    {
        //        string parametro = GetDirectorioICono();
        //        ruta = HttpContext.Current.Server.MapPath(parametro);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        ruta = string.Empty;
        //    }

        //    return ruta;
        //}

        //public static string GetDirectorioICono()
        //{
        //    ParametrosController cParametros = new ParametrosController();
        //    return cParametros.GetItemByName(Comun.RUTA_DIRECTORIO_ICONOS).Valor;
        //}
        #endregion

    }
}