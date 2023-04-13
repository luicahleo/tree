using CapaNegocio;
using Ext.Net;
using System.Collections;
using log4net;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TreeCore.Page;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;




namespace TreeCore.ModGlobal
{

    public partial class GeneradorCodigos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        public static List<Object> listaFormularios;

        static bool VisorTreePClosed = false;

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
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

                UsuariosController cUsuarios = new UsuariosController();
                Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                hdUsuarioID.Value = oUser.UsuarioID;

                #region FILTROS
                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storeGlobalCondicionReglaConfiguracion, gridReglasCodigos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

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

                hdFormulario.SetValue(0);

                CargarMenu();

                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            List<Data.Vw_GlobalCondicionesReglasConfiguraciones> listaDatos;
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            int iCount = 0;
                            long seleccionadoID = long.Parse(Request.QueryString["aux"]);


                            listaDatos = ListaGlobalCondicionReglaConfiguracion(0, 0, sOrden, sDir, ref iCount, sFiltro, seleccionadoID);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridReglasCodigos.ColumnModel, listaDatos, Response, "", "Generador Codigos", _Locale);
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargarDetalle }},
                { "Post", new List<ComponentBase> { btnAnadir, btnAnadirDetalle }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto, btnEditarDetalle }},
                { "Delete", new List<ComponentBase> { btnEliminar, btnEliminarDetalle }}
            };
        }

        protected void btnCloseShowVisorTreeP_DirectClick(object sender, DirectEventArgs e)
        {



            if (VisorTreePClosed == true)
            {

                this.CenterPanelMain.Visible = true;
                this.PanelVisorMain.Visible = true;
                this.TreePanelSideL.Visible = true;

                pnAsideR.Collapse();

                this.MainVwP.Render();
                X.Call("ActiveResizer");

                VisorTreePClosed = false;


            }
            else
            {

                this.CenterPanelMain.Visible = true;
                this.PanelVisorMain.Visible = true;
                this.TreePanelSideL.Visible = false;

                pnAsideR.Collapse();


                this.MainVwP.Render();
                X.Call("ActiveResizer");


                VisorTreePClosed = true;

            }

        }


        protected void ShowHidePnAsideR(object sender, DirectEventArgs e)
        {


            btnCollapseAsRClosed.Show();
            X.Call("hidePnLiteDirect");


            pnAsideR.AnimCollapse = true;
            pnAsideR.ToggleCollapse();

        }


        protected void ShowHidePnAsideRColumnas(object sender, DirectEventArgs e)
        {


            btnCollapseAsRClosed.Show();
            // X.Call("hidePnLiteDirect");

            //WrapMainContent1.Hide();
            //WrapGestionColumnas.Show();

            pnAsideR.AnimCollapse = true;
            pnAsideR.Expand();

        }

        #region STORES

        #region STORE GLOBAL CONDICION CODIGO EMPLAZAMIENTO

        protected void storeGlobalCondicionReglaConfiguracion_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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

                    JsonObject obj;
                    List<JsonObject> GlobalCondicionReglaConfiguracion = new List<JsonObject>();

                    long seleccionadoID = long.Parse(hd_MenuSeleccionado.Value.ToString());
                    var lista = ListaGlobalCondicionReglaConfiguracion(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, seleccionadoID);

                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {
                            obj = new JsonObject();
                            obj.Add("GlobalCondicionReglaConfiguracionID", item.GlobalCondicionReglaConfiguracionID);
                            obj.Add("NombreCampo", item.NombreCampo);
                            obj.Add("Valor", item.Valor);
                            obj.Add("LongitudCadena", item.LongitudCadena);
                            obj.Add("Orden", item.Orden);

                            if (item.TipoCondicion == "Auto_Caracter")
                            {
                                obj.Add("TipoCondicion", (GetGlobalResource("strAutoCaracter")));
                            }
                            else if (item.TipoCondicion == "Auto_Numerico")
                            {
                                obj.Add("TipoCondicion", (GetGlobalResource("strAutoNumerico")));
                            }
                            else if (item.TipoCondicion == "Constante")
                            {
                                obj.Add("TipoCondicion", (GetGlobalResource("strConstante")));
                            }
                            else if (item.TipoCondicion == "Formulario")
                            {
                                obj.Add("TipoCondicion", (GetGlobalResource("strFormulario")));
                            }
                            else if (item.TipoCondicion == "Separador")
                            {
                                obj.Add("TipoCondicion", (GetGlobalResource("strSeparador")));
                            }
                            else if (item.TipoCondicion == "Tabla")
                            {
                                obj.Add("TipoCondicion", (GetGlobalResource("strTabla")));
                            }

                            obj.Add("ClaveRecursoTabla", (GetGlobalResource(item.ClaveRecursoTabla) == "") ? item.ClaveRecursoTabla : GetGlobalResource(item.ClaveRecursoTabla));

                            obj.Add("ClaveRecursoColumna", (GetGlobalResource(item.ClaveRecursoColumna) == "") ? item.ClaveRecursoColumna : GetGlobalResource(item.ClaveRecursoColumna));

                            GlobalCondicionReglaConfiguracion.Add(obj);
                        }
                    }

                    if (GlobalCondicionReglaConfiguracion != null)
                    {
                        storeGlobalCondicionReglaConfiguracion.DataSource = GlobalCondicionReglaConfiguracion;

                        PageProxy temp = new PageProxy();
                        temp = (PageProxy)storeGlobalCondicionReglaConfiguracion.Proxy[0];
                        temp.Total = lista.Count;
                    }


                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);


                }

            }

        }

        private List<Data.Vw_GlobalCondicionesReglasConfiguraciones> ListaGlobalCondicionReglaConfiguracion(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long seleccionadoID)
        {
            List<Data.Vw_GlobalCondicionesReglasConfiguraciones> datos;
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            try
            {
                //datos = cCondicionesConfiguraciones.VistaGlobalCondicionesReglasConfiguracionesBySeleccionadoID(seleccionadoID);
                datos = cCondicionesConfiguraciones.GetItemsWithExtNetFilterList<Data.Vw_GlobalCondicionesReglasConfiguraciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "GlobalCondicionReglaID == " + seleccionadoID);

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);

                datos = null;
            }
            cCondicionesConfiguraciones = null;
            return datos;
        }

        private List<Data.Vw_GlobalCondicionesReglasConfiguraciones> ListaGlobalCondicionReglaConfiguracionEditar(long seleccionadoID)
        {
            List<Data.Vw_GlobalCondicionesReglasConfiguraciones> datos;
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            try
            {
                datos = cCondicionesConfiguraciones.VistaGlobalCondicionesReglasConfiguracionesBySeleccionadoID(seleccionadoID);

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);

                datos = null;
            }
            cCondicionesConfiguraciones = null;
            return datos;
        }

        #endregion

        #region Proyectos Tipos
        private List<Data.Vw_ClientesProyectosTipos> ListaProyectosTipos()
        {
            List<Data.Vw_ClientesProyectosTipos> datos = new List<Data.Vw_ClientesProyectosTipos>();
            ProyectosTiposController cProyetosTipos = new ProyectosTiposController();
            try
            {
                long CliID = 0;
                if (ClienteID.HasValue)
                {
                    CliID = ClienteID.Value;
                }
                else
                {
                    if (cmbClientes.SelectedItem.Value != null && Int32.Parse(cmbClientes.SelectedItem.Value.ToString()) != 0)
                    {
                        CliID = Int32.Parse(cmbClientes.SelectedItem.Value.ToString());
                    }
                }
                datos = cProyetosTipos.ProyectosTiposAsignados(CliID);

                UsuariosController usua = new UsuariosController();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return datos;
        }

        protected void storeProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos

                try
                {
                    var ls = ListaProyectosTipos();
                    if (ls != null)
                        storeProyectosTipos.DataSource = ls;

                }
                catch (Exception ex)
                {
                    this.MensajeErrorGenerico(ex);
                }

            }
        }
        #endregion

        #region Global Condiciones Tablas
        private List<Data.Vw_GlobalCondicionesTablas> ListaGlobalCondicionesTablas()
        {
            List<Data.Vw_GlobalCondicionesTablas> datos = new List<Data.Vw_GlobalCondicionesTablas>();
            GlobalCondicionesTablasController cGlobalCondicionesTablas = new GlobalCondicionesTablasController();

            String lCampoDestino = hdCampoDestino.Value.ToString() + "_TABLA";
            try
            {
                datos = cGlobalCondicionesTablas.GetAllGlobalCondicionesTablasByCampoDestino(lCampoDestino);

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);

                datos = null;
            }
            cGlobalCondicionesTablas = null;
            return datos;
        }

        protected void storeGlobalCondicionesTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject obj;
                    List<JsonObject> GlobalCondicionesTablas = new List<JsonObject>();
                    var ls = ListaGlobalCondicionesTablas();
                    if (ls != null)
                    {
                        foreach (var item in ls)
                        {
                            obj = new JsonObject();
                            obj.Add("GlobalCondicionTablaID", item.GlobalCondicionTablaID);
                            obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                            obj.Add("CampoDestino", item.CampoDestino);
                            obj.Add("Activo", item.Activo);
                            obj.Add("NombreTabla", (GetGlobalResource(item.ClaveRecurso) == "") ? item.NombreTabla : GetGlobalResource(item.ClaveRecurso));
                            GlobalCondicionesTablas.Add(obj);
                        }
                    }

                    if (GlobalCondicionesTablas != null)
                        storeGlobalCondicionesTablas.DataSource = GlobalCondicionesTablas;

                }
                catch (Exception ex)
                {
                    this.MensajeErrorGenerico(ex);
                }

            }
        }
        #endregion

        #region Global Condiciones Columnas Tablas
        private List<Data.ColumnasModeloDatos> ListaGlobalCondicionesColumnasTablas()
        {
            List<Data.ColumnasModeloDatos> datos = new List<Data.ColumnasModeloDatos>();
            GlobalCondicionesColumnasTablasController cGlobalCondicionesColumnasTablas = new GlobalCondicionesColumnasTablasController();


            try
            {
                if (cmbTabla.Value.ToString() != "")
                {
                    long lTabla = Convert.ToInt32(cmbTabla.Value.ToString());
                    datos = cGlobalCondicionesColumnasTablas.GetAllGlobalCondicionesColumnasTablasByTabla(lTabla);
                }
                else if (hdTabla.Value != null && hdTabla.Value.ToString() != "")
                {
                    long lTabla = long.Parse(hdTabla.Value.ToString());
                    datos = cGlobalCondicionesColumnasTablas.GetAllGlobalCondicionesColumnasTablasByTabla(lTabla);
                }
                else
                {
                    long lTabla = 0;
                    datos = cGlobalCondicionesColumnasTablas.GetAllGlobalCondicionesColumnasTablasByTabla(lTabla);
                }

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);

                datos = null;
            }
            cGlobalCondicionesColumnasTablas = null;
            return datos;
        }

        protected void storeGlobalCondicionesColumnasTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject obj;
                    List<JsonObject> GlobalCondicionesColumnasTablas = new List<JsonObject>();
                    List<Data.ColumnasModeloDatos> ls;
                    ls = ListaGlobalCondicionesColumnasTablas();
                    if (ls != null)
                    {
                        foreach (var item in ls)
                        {
                            obj = new JsonObject();
                            obj.Add("ColumnaModeloDatosID", item.ColumnaModeloDatosID);
                            obj.Add("Activo", item.Activo);
                            obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                            obj.Add("NombreColumna", (GetGlobalResource(item.ClaveRecurso) == "") ? item.NombreColumna : GetGlobalResource(item.ClaveRecurso));
                            GlobalCondicionesColumnasTablas.Add(obj);
                        }
                    }
                    if (GlobalCondicionesColumnasTablas != null)
                        storeGlobalCondicionesColumnasTablas.DataSource = GlobalCondicionesColumnasTablas;

                }
                catch (Exception ex)
                {
                    this.MensajeErrorGenerico(ex);
                }

            }
        }
        #endregion


        #region Global Condiciones Formularios
        private List<Data.Vw_GlobalCondicionesTablas> ListaGlobalCondicionesFormularios()
        {
            List<Data.Vw_GlobalCondicionesTablas> datos = new List<Data.Vw_GlobalCondicionesTablas>();
            GlobalCondicionesTablasController cGlobalCondicionesTablas = new GlobalCondicionesTablasController();

            String lCampoDestino = hdCampoDestino.Value.ToString() + "_FORMULARIO";
            try
            {
                datos = cGlobalCondicionesTablas.GetAllGlobalCondicionesTablasByCampoDestino(lCampoDestino);

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);

                datos = null;
            }
            cGlobalCondicionesTablas = null;
            return datos;
        }

        protected void storeGlobalCondicionesFormularios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject obj;
                    List<JsonObject> GlobalCondicionesTablas = new List<JsonObject>();
                    var ls = ListaGlobalCondicionesFormularios();
                    if (ls != null)
                    {
                        foreach (var item in ls)
                        {
                            obj = new JsonObject();
                            obj.Add("GlobalCondicionTablaID", item.GlobalCondicionTablaID);
                            obj.Add("FormularioModeloDatosID", item.TablaModeloDatosID);
                            obj.Add("CampoDestino", item.CampoDestino);
                            obj.Add("Activo", item.Activo);
                            obj.Add("NombreFormulario", (GetGlobalResource(item.ClaveRecurso) == "") ? item.NombreTabla : GetGlobalResource(item.ClaveRecurso));
                            GlobalCondicionesTablas.Add(obj);
                        }
                    }

                    if (GlobalCondicionesTablas != null)
                        storeGlobalCondicionesFormularios.DataSource = GlobalCondicionesTablas;

                }
                catch (Exception ex)
                {
                    this.MensajeErrorGenerico(ex);
                }

            }
        }
        #endregion

        #region Global Condiciones Columnas Formularios
        private List<Data.ColumnasModeloDatos> ListaGlobalCondicionesColumnasFormularios()
        {
            List<Data.ColumnasModeloDatos> datos = new List<Data.ColumnasModeloDatos>();
            GlobalCondicionesColumnasTablasController cGlobalCondicionesColumnasTablas = new GlobalCondicionesColumnasTablasController();


            try
            {
                if (cmbFormulario.Value.ToString() != "")
                {
                    long lTabla = Convert.ToInt32(cmbFormulario.Value.ToString());
                    datos = cGlobalCondicionesColumnasTablas.GetAllGlobalCondicionesColumnasTablasByTabla(lTabla);
                }
                else if (hdFormulario.Value != null && hdFormulario.Value.ToString() != "")
                {
                    long lTabla = long.Parse(hdFormulario.Value.ToString());
                    datos = cGlobalCondicionesColumnasTablas.GetAllGlobalCondicionesColumnasTablasByTabla(lTabla);
                }
                else
                {
                    long lTabla = 0;
                    datos = cGlobalCondicionesColumnasTablas.GetAllGlobalCondicionesColumnasTablasByTabla(lTabla);
                }

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);

                datos = null;
            }
            cGlobalCondicionesColumnasTablas = null;
            return datos;
        }

        protected void storeGlobalCondicionesColumnasFormularios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject obj;
                    List<JsonObject> GlobalCondicionesColumnasTablas = new List<JsonObject>();
                    List<Data.ColumnasModeloDatos> ls;
                    ls = ListaGlobalCondicionesColumnasFormularios();
                    if (ls != null)
                    {
                        foreach (var item in ls)
                        {
                            obj = new JsonObject();
                            obj.Add("ColumnaModeloDatosID", item.ColumnaModeloDatosID);
                            obj.Add("Activo", item.Activo);
                            obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                            obj.Add("NombreColumna", (GetGlobalResource(item.ClaveRecurso) == "") ? item.NombreColumna : GetGlobalResource(item.ClaveRecurso));
                            GlobalCondicionesColumnasTablas.Add(obj);
                        }
                    }
                    if (GlobalCondicionesColumnasTablas != null)
                        storeGlobalCondicionesColumnasFormularios.DataSource = GlobalCondicionesColumnasTablas;

                }
                catch (Exception ex)
                {
                    this.MensajeErrorGenerico(ex);
                }

            }
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

        #region CARGAR TREE
        private void CargarMenu()
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz, NodoCodigoEmplazamiento, NodoNombreInventario, NodoCodigoInventario, NodoNombreDocumento, NodoCodigoProductCatalogServicios, NodoCodigoCatalogos;
                //string lCampoDestino = (hdCampoDestino.Value.ToString());

                nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strComun"),
                    Expanded = true
                };

                //NodoCodigoContrato = new Node
                //{
                //    Text = GetGlobalResource("strCodigoContrato"),
                //    NodeID = "CODIGO_CONTRATOS",
                //    Expanded = false
                //};

                //NodoCodigoContratoMarcas = new Node
                //{
                //    Text = GetGlobalResource("strCodigoContratoMarcas"),
                //    NodeID = "CODIGO_CONTRATOS_MARCOS",
                //    Expanded = false
                //};

                NodoCodigoEmplazamiento = new Node
                {
                    Text = GetGlobalResource("strCodigoEmplazamiento"),
                    NodeID = "CODIGO_EMPLAZAMIENTO",
                    Expanded = true
                };
                NodoCodigoEmplazamiento.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));
                NodoCodigoEmplazamiento.CustomAttributes.Add(new ConfigItem("Maximo", "40"));

                NodoCodigoProductCatalogServicios = new Node
                {
                    Text = GetGlobalResource("strCodigoProductCatalogServicios"),
                    NodeID = "CODIGO_PRODUCT_CATALOG_SERVICIOS",
                    Expanded = true
                };
                NodoCodigoProductCatalogServicios.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));
                NodoCodigoProductCatalogServicios.CustomAttributes.Add(new ConfigItem("Maximo", "40"));
                
                NodoCodigoCatalogos = new Node
                {
                    Text = GetGlobalResource("strCodigoProductCatalog"),
                    NodeID = "CODIGO_CATALOG",
                    Expanded = true
                };
                NodoCodigoCatalogos.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));
                NodoCodigoCatalogos.CustomAttributes.Add(new ConfigItem("Maximo", "40"));

                NodoCodigoInventario = new Node
                {
                    Text = GetGlobalResource("strCodigoInventario"),
                    NodeID = "CODIGO_INVENTARIO",
                    Expanded = true
                };
                NodoCodigoInventario.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));
                NodoCodigoInventario.CustomAttributes.Add(new ConfigItem("Maximo", "40"));

                NodoNombreInventario = new Node
                {
                    Text = GetGlobalResource("strNombreInventario"),
                    NodeID = "NOMBRE_INVENTARIO",
                    Expanded = true
                };
                NodoNombreInventario.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));
                NodoNombreInventario.CustomAttributes.Add(new ConfigItem("Maximo", "40"));

                NodoNombreDocumento = new Node
                {
                    Text = GetGlobalResource("strNombreDocumento"),
                    NodeID = "NOMBRE_DOCUMENTO",
                    Expanded = true
                };
                NodoNombreDocumento.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));
                NodoNombreDocumento.CustomAttributes.Add(new ConfigItem("Maximo", "40"));

                //NodoNombreContratos = new Node
                //{
                //    Text = GetGlobalResource("strNombreContratos"),
                //    NodeID = "CODIGO_NOMBRE_CONTRATOS",
                //    Expanded = false
                //};

                nodes.Add(nodoRaiz);
                //nodoRaiz.Children.Add(NodoCodigoContrato);
                //nodoRaiz.Children.Add(NodoCodigoContratoMarcas);
                nodoRaiz.Children.Add(NodoCodigoEmplazamiento);
                nodoRaiz.Children.Add(NodoCodigoProductCatalogServicios);
                nodoRaiz.Children.Add(NodoCodigoCatalogos);
                nodoRaiz.Children.Add(NodoNombreInventario);
                nodoRaiz.Children.Add(NodoCodigoInventario);
                //nodoRaiz.Children.Add(NodoNombreDocumento);
                //nodoRaiz.Children.Add(NodoNombreContratos);
                NodoCodigoEmplazamiento.Children.AddRange(ConstruirArbol("CODIGO_EMPLAZAMIENTO", 40));
                NodoCodigoProductCatalogServicios.Children.AddRange(ConstruirArbol("CODIGO_PRODUCT_CATALOG_SERVICIOS", 40));
                NodoCodigoCatalogos.Children.AddRange(ConstruirArbol("CODIGO_CATALOG", 40));
                NodoNombreInventario.Children.AddRange(ConstruirArbol("NOMBRE_INVENTARIO", 40));
                NodoCodigoInventario.Children.AddRange(ConstruirArbol("CODIGO_INVENTARIO", 40));
                NodoNombreDocumento.Children.AddRange(ConstruirArbol("NOMBRE_DOCUMENTO", 40));

                //NodoCodigoContrato.Children.AddRange(ConstruirArbol("CODIGO_CONTRATOS"));
                //NodoCodigoContratoMarcas.Children.AddRange(ConstruirArbol("CODIGO_CONTRATOS_MARCOS"));
                //NodoNombreContratos.Children.AddRange(ConstruirArbol("CODIGO_NOMBRE_CONTRATOS"));
                TreePanelSideL.SetRootNode(nodoRaiz);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private NodeCollection ConstruirArbol(string lCampoDestino, long Maximo)
        {
            GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
            NodeCollection oNodes;
            //hdCampoDestino.Value = lCampoDestino;

            try
            {
                List<Data.Vw_GlobalCondicionesReglas> listaMenus = cGlobalCondicionesReglas.GetVwByCampoDestino((lCampoDestino));

                oNodes = GetNodosHijos(lCampoDestino, listaMenus, 0, Maximo);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNodes = null;
            }

            return oNodes;
        }

        private NodeCollection GetNodosHijos(string lPadre, List<Data.Vw_GlobalCondicionesReglas> listaMenus, long nivel, long lMaximo)
        {
            NodeCollection oMenu = new NodeCollection(false);
            nivel++;

            try
            {
                listaMenus.ForEach(oItem =>
                {
                    if (oItem.CampoDestino == lPadre)
                    {
                        Node oNodo = new Node
                        {
                            Text = (!string.IsNullOrEmpty(oItem.NombreRegla)) ? (oItem.NombreRegla) : oItem.NombreCompleto,
                            Expanded = true,
                            Expandable = true,
                            NodeID = oItem.GlobalCondicionReglaID.ToString(),
                        };

                        oNodo.CustomAttributes.Add(new ConfigItem("NombreRegla", oItem.NombreRegla));
                        oNodo.CustomAttributes.Add(new ConfigItem("ProyectoTipo", oItem.ProyectoTipo));
                        oNodo.CustomAttributes.Add(new ConfigItem("CampoDestino", oItem.CampoDestino));
                        oNodo.CustomAttributes.Add(new ConfigItem("UltimoGenerado", oItem.UltimoGenerado));
                        oNodo.CustomAttributes.Add(new ConfigItem("Activo", oItem.Activo ? GetGlobalResource("strActivo") : ""));
                        oNodo.CustomAttributes.Add(new ConfigItem("Defecto", oItem.Defecto ? GetGlobalResource("strDefecto") : ""));
                        oNodo.CustomAttributes.Add(new ConfigItem("Modificada", oItem.Modificada ? GetGlobalResource("strModificado") : ""));
                        oNodo.CustomAttributes.Add(new ConfigItem("FechaCreacióN", oItem.FechaCreacióN));
                        oNodo.CustomAttributes.Add(new ConfigItem("NombreCompleto", oItem.NombreCompleto));
                        oNodo.CustomAttributes.Add(new ConfigItem("Tipo", "Hijo"));
                        oNodo.CustomAttributes.Add(new ConfigItem("Maximo", lMaximo));


                        NodeCollection nodoshijos = GetNodosHijos(oItem.GlobalCondicionReglaID.ToString(), listaMenus, nivel, lMaximo);
                        if (oItem.CampoDestino != "" || (nodoshijos != null && nodoshijos.Count > 0))
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

        [DirectMethod()]
        public string RefreshMenu()
        {
            NodeCollection nodes = new NodeCollection();
            Node nodoRaiz, NodoCodigoEmplazamiento, NodoNombreInventario, NodoCodigoInventario, NodoNombreDocumento, NodoCodigoProductCatalogServicios, NodoCodigoCatalogos;
            //string lCampoDestino = (hdCampoDestino.Value.ToString());

            try
            {

                nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strComun"),
                    Expanded = true
                };

                //NodoCodigoContrato = new Node
                //{
                //    Text = GetGlobalResource("strCodigoContrato"),
                //    NodeID = "CODIGO_CONTRATOS",
                //    Expanded = false
                //};

                //NodoCodigoContratoMarcas = new Node
                //{
                //    Text = GetGlobalResource("strCodigoContratoMarcas"),
                //    NodeID = "CODIGO_CONTRATOS_MARCOS",
                //    Expanded = false
                //};

                NodoCodigoEmplazamiento = new Node
                {
                    Text = GetGlobalResource("strCodigoEmplazamiento"),
                    NodeID = "CODIGO_EMPLAZAMIENTO",
                    Expanded = true
                };
                NodoCodigoEmplazamiento.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));

                NodoCodigoProductCatalogServicios = new Node
                {
                    Text = GetGlobalResource("strCodigoProductCatalogServicios"),
                    NodeID = "CODIGO_PRODUCT_CATALOG_SERVICIOS",
                    Expanded = true
                };
                NodoCodigoProductCatalogServicios.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));

                NodoCodigoCatalogos = new Node
                {
                    Text = GetGlobalResource("strCodigoProductCatalog"),
                    NodeID = "CODIGO_CATALOG",
                    Expanded = true
                };
                NodoCodigoCatalogos.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));

                NodoCodigoInventario = new Node
                {
                    Text = GetGlobalResource("strCodigoInventario"),
                    NodeID = "CODIGO_INVENTARIO",
                    Expanded = true
                };
                NodoCodigoInventario.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));

                NodoNombreInventario = new Node
                {
                    Text = GetGlobalResource("strNombreInventario"),
                    NodeID = "NOMBRE_INVENTARIO",
                    Expanded = true
                };
                NodoNombreInventario.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));

                NodoNombreDocumento = new Node
                {
                    Text = GetGlobalResource("strNombreDocumento"),
                    NodeID = "NOMBRE_DOCUMENTO",
                    Expanded = true
                };
                NodoNombreDocumento.CustomAttributes.Add(new ConfigItem("Tipo", "Padre"));

                //NodoNombreContratos = new Node
                //{
                //    Text = GetGlobalResource("strNombreContratos"),
                //    NodeID = "CODIGO_NOMBRE_CONTRATOS",
                //    Expanded = false
                //};

                nodes.Add(nodoRaiz);
                //nodoRaiz.Children.Add(NodoCodigoContrato);
                //nodoRaiz.Children.Add(NodoCodigoContratoMarcas);
                nodoRaiz.Children.Add(NodoCodigoEmplazamiento);
                nodoRaiz.Children.Add(NodoCodigoProductCatalogServicios);
                nodoRaiz.Children.Add(NodoCodigoCatalogos);
                nodoRaiz.Children.Add(NodoNombreInventario);
                nodoRaiz.Children.Add(NodoCodigoInventario);
                //nodoRaiz.Children.Add(NodoNombreDocumento);

                //nodoRaiz.Children.Add(NodoNombreContratos);
                NodoCodigoEmplazamiento.Children.AddRange(ConstruirArbol("CODIGO_EMPLAZAMIENTO", 40));
                NodoCodigoProductCatalogServicios.Children.AddRange(ConstruirArbol("CODIGO_PRODUCT_CATALOG_SERVICIOS", 40));
                NodoCodigoCatalogos.Children.AddRange(ConstruirArbol("CODIGO_CATALOG", 40));
                NodoNombreInventario.Children.AddRange(ConstruirArbol("NOMBRE_INVENTARIO", 40));
                NodoCodigoInventario.Children.AddRange(ConstruirArbol("CODIGO_INVENTARIO", 40));
                NodoNombreDocumento.Children.AddRange(ConstruirArbol("NOMBRE_DOCUMENTO", 40));


                //NodoCodigoContrato.Children.AddRange(ConstruirArbol("CODIGO_CONTRATOS"));
                //NodoCodigoContratoMarcas.Children.AddRange(ConstruirArbol("CODIGO_CONTRATOS_MARCOS"));
                //NodoNombreContratos.Children.AddRange(ConstruirArbol("CODIGO_NOMBRE_CONTRATOS"));
                TreePanelSideL.SetRootNode(nodoRaiz);
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return nodes.ToJson();
        }

        #endregion

        #region AGREGAR EDITAR

        [DirectMethod]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            Data.GlobalCondicionesReglas oDato = new Data.GlobalCondicionesReglas();

            try
            {
                if (!bAgregar)
                {
                    #region Editar
                    //long.Parse(hd_MenuSeleccionado.Value.ToString());
                    Data.GlobalCondicionesReglas dato;
                    dato = cGlobalCondicionesReglas.GetItem(long.Parse(hd_MenuSeleccionado.Value.ToString()));

                    if (dato != null)
                    {

                        if (dato.NombreRegla != txtNombreRegla.Text && cGlobalCondicionesReglas.RegistroDuplicadoNombre(txtNombreRegla.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente) + ": " + txtNombreRegla.Text);
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            return direct;
                        }

                        long VarProyectoTipoID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                        String VarCampoDestino = hdCampoDestino.Value.ToString();

                        if (dato.ProyectoTipoID != VarProyectoTipoID && cGlobalCondicionesReglas.RegistroDuplicadoProyecto(VarProyectoTipoID, VarCampoDestino))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsCategoriaExiste);
                            return direct;
                        }

                        dato.ProyectoTipoID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                        if (dato.ProyectoTipoID == 0)
                        {
                            dato.ProyectoTipoID = 0;
                        }


                        dato.NombreRegla = txtNombreRegla.Value.ToString();

                        dato.CampoDestino = hdCampoDestino.Value.ToString();


                        if (cGlobalCondicionesReglas.UpdateItem(dato))
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        }

                    }
                }

                #endregion

                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "0")
                {
                    if (oDato.NombreRegla != txtNombreRegla.Text && cGlobalCondicionesReglas.RegistroDuplicadoNombre(txtNombreRegla.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente) + ": " + txtNombreRegla.Text);
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);
                        return direct;
                    }
                    long VarProyectoTipoID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                    String VarCampoDestino = hdCampoDestino.Value.ToString();


                    if (oDato.ProyectoTipoID != VarProyectoTipoID && cGlobalCondicionesReglas.RegistroDuplicadoProyecto(VarProyectoTipoID, VarCampoDestino))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsCategoriaExiste);
                        return direct;
                    }

                    oDato.Activo = true;
                    oDato.Defecto = false;
                    oDato.ClienteID = long.Parse(hdCliID.Value.ToString());
                    oDato.UsuarioID = Convert.ToInt32(hdUsuarioID.Value);
                    oDato.FechaCreacióN = DateTime.Now;

                    try
                    {
                        oDato.ProyectoTipoID = Convert.ToInt32(cmbModulo.SelectedItem.Value);
                        if (oDato.ProyectoTipoID == 0)
                        {
                            oDato.ProyectoTipoID = 0;
                        }
                    }
                    catch (Exception)
                    {
                        oDato.ProyectoTipoID = 0;
                    }

                    oDato.NombreRegla = txtNombreRegla.Value.ToString();
                    oDato.CampoDestino = hdRaiz.Value.ToString();


                    if (cGlobalCondicionesReglas.AddItem(oDato) != null)
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
            return direct;
        }

        [DirectMethod]
        public DirectResponse AgregarEditarDetalle(bool bAgregar)
        {
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            Data.GlobalCondicionesReglasConfiguraciones oDato = new Data.GlobalCondicionesReglasConfiguraciones();
            int iLongitudAnterior = 0;

            try
            {
                #region EDITAR
                if (!bAgregar)
                {
                    long lID = long.Parse(GridRowSelectGrid.SelectedRecordID);
                    Data.GlobalCondicionesReglasConfiguraciones dato;
                    dato = cCondicionesConfiguraciones.GetItem(lID);


                    if (dato != null)
                    {
                        long varReglaID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                        var lista = ListaGlobalCondicionReglaConfiguracionEditar(varReglaID);

                        if (lista != null)
                        {
                            long contador = 0;
                            foreach (var item in lista)
                            {
                                contador = contador + item.LongitudCadena;
                            }

                            iLongitudAnterior = dato.LongitudCadena;

                            if ((contador - dato.LongitudCadena) + long.Parse(txtLongitud.Text.Length.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                            {

                                if (dato.NombreCampo != txtNombreCodigo.Text && cCondicionesConfiguraciones.RegistroDuplicadoNombre(txtNombreCodigo.Text, varReglaID))
                                {
                                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                    return direct;
                                }

                                dato.NombreCampo = txtNombreCodigo.Text;


                                try
                                {
                                    dato.TipoCondicion = cmbTipoCondicion.SelectedItem.Value.ToString();
                                    if (dato.TipoCondicion == null)
                                    {
                                        dato.TipoCondicion = null;
                                    }
                                }
                                catch (Exception)
                                {
                                    dato.TipoCondicion = null;
                                }


                                dato.LongitudCadena = txtValor.Text.Length;

                                dato.Valor = txtValor.Text;
                                dato.Codigo = txtNombreCodigo.Text;
                                dato.GlobalCondicionReglaID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                                if (dato.TipoCondicion.ToString() == "Formulario")
                                {
                                    try
                                    {
                                        if ((contador - iLongitudAnterior) + long.Parse(txtLongitud.Value.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                                        {
                                            dato.ColumnaModeloDatoID = Convert.ToInt32(cmbColumnaFormulario.SelectedItem.Value.ToString());
                                            dato.LongitudCadena = Convert.ToInt32(txtLongitud.Value.ToString());
                                            if (dato.ColumnaModeloDatoID == null)
                                            {
                                                dato.ColumnaModeloDatoID = null;
                                            }
                                        }
                                        else
                                        {
                                            direct.Success = false;
                                            direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                                            return direct;
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        dato.ColumnaModeloDatoID = null;
                                    }
                                }
                                else if (dato.TipoCondicion.ToString() == "Tabla")
                                {
                                    try
                                    {
                                        if ((contador - iLongitudAnterior) + long.Parse(txtLongitud.Value.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                                        {
                                            dato.ColumnaModeloDatoID = Convert.ToInt32(cmbColumnaTabla.SelectedItem.Value.ToString());
                                            dato.LongitudCadena = Convert.ToInt32(txtLongitud.Value.ToString());
                                            if (dato.ColumnaModeloDatoID == null)
                                            {
                                                dato.ColumnaModeloDatoID = null;
                                            }
                                        }
                                        else
                                        {
                                            direct.Success = false;
                                            direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                                            return direct;
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        dato.ColumnaModeloDatoID = null;
                                    }
                                }
                                else if (txtLongitud.Text == "")
                                {
                                    try
                                    {
                                        if ((contador - iLongitudAnterior) + long.Parse(dato.Valor.Length.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                                        {
                                            dato.ColumnaModeloDatoID = null;
                                        }
                                        else
                                        {
                                            direct.Success = false;
                                            direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                                            return direct;
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        dato.ColumnaModeloDatoID = null;
                                    }
                                }
                                else
                                {
                                    dato.ColumnaModeloDatoID = null;
                                }

                                if (cCondicionesConfiguraciones.UpdateItem(dato) != null)
                                {
                                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                                    GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
                                    Data.GlobalCondicionesReglas Odato;
                                    Odato = cGlobalCondicionesReglas.GetItem(long.Parse(hd_MenuSeleccionado.Value.ToString()));

                                    if (Odato.UltimoGenerado != null || Odato.UltimoGenerado != "")
                                    {
                                        Odato.Modificada = true;
                                    }

                                    cGlobalCondicionesReglas.UpdateItem(Odato);

                                    direct.Result = "";
                                    direct.Success = true;
                                    return direct;
                                }
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                                return direct;
                            }
                        }

                    }
                }
                #endregion

                #region AGREGAR
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "0")
                {
                    long varReglaID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                    List<Data.GlobalCondicionesReglasConfiguraciones> GlobalCondicionReglaConfiguracion = new List<Data.GlobalCondicionesReglasConfiguraciones>();
                    var lista = ListaGlobalCondicionReglaConfiguracionEditar(varReglaID);

                    if (lista != null)
                    {
                        long contador = 0;
                        foreach (var item in lista)
                        {
                            contador = contador + item.LongitudCadena;
                        }

                        //long longituMaxima = long.Parse(hdLongitudMaxima.Value.ToString());

                        if (contador + long.Parse(txtValor.Text.Length.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                        {
                            if (oDato.NombreCampo != txtNombreCodigo.Text && cCondicionesConfiguraciones.RegistroDuplicadoNombre(txtNombreCodigo.Text, varReglaID))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                return direct;
                            }

                            oDato.NombreCampo = txtNombreCodigo.Text;

                            oDato.LongitudCadena = txtValor.Text.Length;



                            try
                            {
                                oDato.TipoCondicion = cmbTipoCondicion.SelectedItem.Value.ToString();
                                if (oDato.TipoCondicion == null)
                                {
                                    oDato.TipoCondicion = null;
                                }
                            }
                            catch (Exception)
                            {
                                oDato.TipoCondicion = null;
                            }

                            Data.GlobalCondicionesReglasConfiguraciones datoOrden = new Data.GlobalCondicionesReglasConfiguraciones();
                            datoOrden = cCondicionesConfiguraciones.GetUltimoOrden(varReglaID);

                            if (datoOrden != null)
                            {
                                oDato.Orden = datoOrden.Orden + 1;
                            }
                            else
                            {
                                oDato.Orden = 1;
                            }

                            oDato.Valor = txtValor.Text;
                            oDato.Codigo = txtNombreCodigo.Text;
                            oDato.GlobalCondicionReglaID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                            if (oDato.TipoCondicion.ToString() == "Formulario")
                            {
                                try
                                {
                                    if (contador + long.Parse(txtLongitud.Value.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                                    {
                                        oDato.ColumnaModeloDatoID = Convert.ToInt32(cmbColumnaFormulario.SelectedItem.Value.ToString());

                                        oDato.LongitudCadena = Convert.ToInt32(txtLongitud.Value.ToString());
                                        if (oDato.ColumnaModeloDatoID == null)
                                        {
                                            oDato.ColumnaModeloDatoID = null;
                                        }
                                    }
                                    else
                                    {
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                                        return direct;
                                    }

                                }

                                catch (Exception)
                                {
                                    oDato.ColumnaModeloDatoID = null;
                                }
                            }
                            else if (oDato.TipoCondicion.ToString() == "Tabla")
                            {
                                try
                                {
                                    if (contador + long.Parse(txtLongitud.Value.ToString()) <= long.Parse(hdLongitudMaxima.Value.ToString()))
                                    {
                                        oDato.ColumnaModeloDatoID = Convert.ToInt32(cmbColumnaTabla.SelectedItem.Value.ToString());

                                        oDato.LongitudCadena = Convert.ToInt32(txtLongitud.Value.ToString());
                                        if (oDato.ColumnaModeloDatoID == null)
                                        {
                                            oDato.ColumnaModeloDatoID = null;
                                        }
                                    }
                                    else
                                    {
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                                        return direct;
                                    }

                                }
                                catch (Exception)
                                {
                                    oDato.ColumnaModeloDatoID = null;
                                }
                            }
                            else
                            {
                                oDato.ColumnaModeloDatoID = null;
                            }

                            if (cCondicionesConfiguraciones.AddItem(oDato) != null)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                                GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
                                Data.GlobalCondicionesReglas dato;
                                dato = cGlobalCondicionesReglas.GetItem(long.Parse(hd_MenuSeleccionado.Value.ToString()));

                                if (dato.UltimoGenerado != null || dato.UltimoGenerado != "")
                                {
                                    dato.Modificada = true;
                                }
                                cGlobalCondicionesReglas.UpdateItem(dato);

                                direct.Result = "";
                                direct.Success = true;
                                return direct;
                            }

                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsTamanoCodigoExcedido) + " " + hdLongitudMaxima.Value.ToString() + " " + GetGlobalResource(Comun.jsCaracteres);
                            return direct;
                        }
                    }

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

        #endregion

        #region DRAG DROP
        //[DirectMethod()]
        //public DirectResponse DropNodo(string targetID, string destinationID)
        //{
        //    DirectResponse direct = new DirectResponse();
        //    GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();

        //    try
        //    {
        //        Data.GlobalCondicionesReglasConfiguraciones oDato = cCondicionesConfiguraciones.GetItem(Convert.ToInt64(targetID));

        //        if (destinationID == "root")
        //        {
        //            oDato.Orden = oDato.Orden;
        //        }
        //        else
        //        {
        //            oDato.Orden = Convert.ToInt64(destinationID);
        //        }

        //        if (cCondicionesConfiguraciones.UpdateItem(oDato))
        //        {
        //            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return direct;
        //    }

        //    direct.Success = true;
        //    direct.Result = "";

        //    return direct;
        //}

        //[DirectMethod()]
        //public DirectResponse BeforeDropNodo(string targetID, string destinationID)
        //{
        //    DirectResponse direct = new DirectResponse();
        //    GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();

        //    bool bTodoOK = true;

        //    try
        //    {
        //        if (GridRowSelectGrid.SelectedRecordID != "")
        //        {
        //            long lID = long.Parse(GridRowSelectGrid.SelectedRecordID);
        //            Data.GlobalCondicionesReglasConfiguraciones oTarget = cCondicionesConfiguraciones.GetItem<Data.GlobalCondicionesReglasConfiguraciones>(lID);
        //            Data.GlobalCondicionesReglasConfiguraciones oDestino = null;


        //            if (destinationID != "root")
        //            {
        //                oDestino = cCondicionesConfiguraciones.GetItem(Convert.ToInt64(destinationID));

        //                //#region Nivel maximo permitido

        //                //int iNivel = cMenu.GetNivelNodo(oDestino.MenuID);
        //                //int iMaxNivel = Convert.ToInt32(hd_NivelMaxPermitido.Value);

        //                //if (cMenu.HasChildren(oTarget.MenuID))
        //                //{
        //                //    iNivel += cMenu.GetMaxDepth(oTarget.MenuID, iMaxNivel);
        //                //}

        //                //if (iNivel >= iMaxNivel)
        //                //{
        //                //    bTodoOK = false;
        //                //    direct.Success = false;
        //                //    direct.Result = GetGlobalResource(Comun.jsNivelMenuNoPermitido);
        //                //}

        //                //#endregion

        //                //#region Drop en hoja

        //                //if (oDestino.PaginaMenuModuloID != null)
        //                //{
        //                //    bTodoOK = false;
        //                //    direct.Success = false;
        //                //    direct.Result = GetGlobalResource(Comun.jsAgregarEnPaginaNoPermitido);
        //                //}

        //                //#endregion
        //            }
        //        }

        //        if (bTodoOK)
        //        {
        //            direct.Success = true;
        //            direct.Result = "";

        //            DropNodo(targetID, destinationID);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return direct;
        //    }

        //    return direct;
        //}

        [DirectMethod()]
        public DirectResponse CambiarOrden(List<long> filas)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                int Cont = 1;
                Data.GlobalCondicionesReglasConfiguraciones dato;
                GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
                foreach (var item in filas)
                {
                    dato = cCondicionesConfiguraciones.GetItem(item);
                    dato.Orden = Cont;
                    cCondicionesConfiguraciones.UpdateItem(dato);
                    Cont++;
                }

                storeGlobalCondicionReglaConfiguracion.Reload();
            }
            catch (Exception ex)
            {
                storeGlobalCondicionReglaConfiguracion.Reload();
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


        #region MOSTRAR EDITAR

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();

            try
            {
                Data.GlobalCondicionesReglas dato;
                dato = cGlobalCondicionesReglas.GetItem<Data.GlobalCondicionesReglas>(long.Parse(hd_MenuSeleccionado.Value.ToString()));

                if (dato != null)
                {

                    txtNombreRegla.Text = dato.NombreRegla;
                    cmbModulo.SetValue(dato.ProyectoTipoID.ToString());

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
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            ColumnasModeloDatosController cCondicionesColumnasTablas = new ColumnasModeloDatosController();
            TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
            long lID = long.Parse(GridRowSelectGrid.SelectedRecordID);

            try
            {
                Data.Vw_GlobalCondicionesReglasConfiguraciones dato;
                Data.ColumnasModeloDatos datoColumnaTabla;
                dato = cCondicionesConfiguraciones.GetItem<Data.Vw_GlobalCondicionesReglasConfiguraciones>(lID);

                if (dato != null)
                {

                    txtNombreCodigo.Text = dato.NombreCampo;

                    cmbTipoCondicion.SetValue(dato.TipoCondicion.ToString());
                    txtValor.Regex = null;
                    txtValor.RegexText = null;
                    if (dato.TipoCondicion.ToString() != "Formulario" && dato.TipoCondicion.ToString() != "Tabla")
                    {

                        cmbFormulario.Hide();
                        cmbColumnaFormulario.Hide();
                        cmbTabla.Hide();
                        cmbColumnaTabla.Hide();
                        txtValor.Show();
                        txtValor.Text = dato.Valor;
                        cmbTipoCondicion.Disable();
                        cmbFormulario.AllowBlank = true;
                        cmbColumnaFormulario.AllowBlank = true;
                        cmbTabla.AllowBlank = true;
                        cmbColumnaTabla.AllowBlank = true;

                        if (dato.TipoCondicion.ToString() == "Auto_Caracter")
                        {
                            txtValor.Regex = "^[a-zA-Z]*$";
                            txtValor.RegexText = GetGlobalResource(Comun.strRegexCaracter);
                        }
                        else if (dato.TipoCondicion.ToString() == "Separador")
                        {
                            txtValor.Regex = "[^A-Za-z0-9_]$";
                            txtValor.RegexText = GetGlobalResource(Comun.strRegexSeparador);
                        }
                        else if (dato.TipoCondicion.ToString() == "Auto_Numerico")
                        {
                            txtValor.Regex = "^[0-9]*$";
                            txtValor.RegexText = GetGlobalResource(Comun.strRegexNumerico);
                        }
                        else if (dato.TipoCondicion.ToString() == "Constante")
                        {
                            txtValor.Regex = @"/^[^$%&|<>\^'/#\\]*$/";
                            txtValor.RegexText = GetGlobalResource(Comun.regexNombreText);
                        }

                    }
                    else if (dato.TipoCondicion.ToString() == "Formulario")
                    {
                        txtValor.Hide();
                        txtValor.AllowBlank = true;
                        cmbFormulario.Show();
                        cmbColumnaFormulario.Show();
                        cmbTipoCondicion.Disable();
                        txtLongitud.Show();

                        cmbTabla.AllowBlank = true;
                        cmbColumnaTabla.AllowBlank = true;
                        cmbTabla.Hide();
                        cmbColumnaTabla.Hide();



                        if (dato.ColumnaModeloDatoID.Value.ToString() != "")
                        {
                            //storeGlobalCondicionesColumnasTablas.Reload();

                            storeGlobalCondicionesColumnasFormularios.Reload(hdCampoDestino.Value.ToString());
                            long ColID = long.Parse(dato.ColumnaModeloDatoID.Value.ToString());
                            datoColumnaTabla = cCondicionesColumnasTablas.GetItem(ColID);
                            hdFormulario.Value = datoColumnaTabla.TablaModeloDatosID.ToString();

                            cmbFormulario.SetValue(datoColumnaTabla.TablaModeloDatosID);
                            txtLongitud.SetValue(dato.LongitudCadena);
                            storeGlobalCondicionesFormularios.Reload(datoColumnaTabla.TablaModeloDatosID.ToString());

                            cmbColumnaFormulario.SetValue(dato.ColumnaModeloDatoID.Value);


                        }


                    }
                    else
                    {
                        txtValor.Hide();
                        txtValor.AllowBlank = true;
                        cmbTabla.Show();
                        cmbColumnaTabla.Show();
                        txtLongitud.Show();
                        cmbTipoCondicion.Disable();


                        cmbFormulario.Hide();
                        cmbColumnaFormulario.Hide();
                        cmbFormulario.AllowBlank = true;
                        cmbColumnaFormulario.AllowBlank = true;

                        if (dato.ColumnaModeloDatoID.Value.ToString() != "")
                        {

                            storeGlobalCondicionesColumnasTablas.Reload(hdCampoDestino.Value.ToString());
                            long ColID = long.Parse(dato.ColumnaModeloDatoID.Value.ToString());
                            datoColumnaTabla = cCondicionesColumnasTablas.GetItem(ColID);
                            hdTabla.Value = datoColumnaTabla.TablaModeloDatosID.ToString();

                            cmbTabla.SetValue(datoColumnaTabla.TablaModeloDatosID);

                            txtLongitud.SetValue(dato.LongitudCadena);

                            storeGlobalCondicionesTablas.Reload(datoColumnaTabla.TablaModeloDatosID.ToString());

                            cmbColumnaTabla.SetValue(dato.ColumnaModeloDatoID.Value);


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

        #endregion

        #region REGEX

        [DirectMethod()]
        public DirectResponse RegexTextFieldCaracter()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                txtValor.Regex = "^[a-zA-Z]*$";
                txtValor.RegexText = GetGlobalResource(Comun.strRegexCaracter);
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
        public DirectResponse RegexTextFieldNumerico()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                txtValor.Regex = "^[0-9]*$";
                txtValor.RegexText = GetGlobalResource(Comun.strRegexNumerico);
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
        public DirectResponse RegexTextFieldSeparador()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                txtValor.Regex = "[^A-Za-z0-9/_]$";
                txtValor.RegexText = GetGlobalResource(Comun.strRegexSeparador);
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
        public DirectResponse RegexTextFieldConstante()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                txtValor.Regex = @"/^[^$%&|<>%\^'/#\\]*$/";
                txtValor.RegexText = GetGlobalResource(Comun.regexNombreText);
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

        #region ELIMINAR

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
            try
            {
                string filtro = hd_MenuSeleccionado.Value.ToString();
                Data.GlobalCondicionesReglas dato = new Data.GlobalCondicionesReglas();
                dato = cGlobalCondicionesReglas.GetItem(long.Parse(hd_MenuSeleccionado.Value.ToString()));

                if (dato.Defecto != true)
                {
                    if (cGlobalCondicionesReglas.DeleteItem(long.Parse(hd_MenuSeleccionado.Value.ToString())))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }

                }
                else
                {
                    log.Warn(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsPorDefecto), Ext.Net.MessageBox.Icon.INFO, null);
                    direct.Success = false;
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
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            long lID = long.Parse(GridRowSelectGrid.SelectedRecordID);
            try
            {

                if (cCondicionesConfiguraciones.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";

                    GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
                    Data.GlobalCondicionesReglas dato;
                    dato = cGlobalCondicionesReglas.GetItem(long.Parse(hd_MenuSeleccionado.Value.ToString()));

                    if (dato.UltimoGenerado != null || dato.UltimoGenerado != "")
                    {
                        dato.Modificada = true;
                    }

                    cGlobalCondicionesReglas.UpdateItem(dato);

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

        #endregion

        #region SIMULAR CODIGO

        [DirectMethod()]
        public DirectResponse SimularCodigo()
        {
            DirectResponse direct = new DirectResponse();
            Vw_GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new Vw_GlobalCondicionesReglasConfiguracionesController();
            List<Data.Vw_GlobalCondicionesReglasConfiguraciones> lista = new List<Data.Vw_GlobalCondicionesReglasConfiguraciones>();
            ParametrosController cParam = new ParametrosController();
            string CodigoFinal = null;
            string CodigoSiguienteFinal = null;
            txtSimulacionCodido.Text = "";
            txtSimulacionCodidoSiguinete.Text = "";
            string UltimoRellenoGenerado = null;
            bool bTipoCondicion = false;
            bool bRelleno = false;


            try
            {
                UltimoRellenoGenerado = cParam.GetItemValor("CONSTANTE_GENERACION_CODIGO_EMPLAZAMIENTO");

                if (UltimoRellenoGenerado != null && UltimoRellenoGenerado != "" && UltimoRellenoGenerado.Equals("X"))
                {
                    bRelleno = true;
                }


                long seleccionadoID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                lista = cCondicionesConfiguraciones.getListByOrder(seleccionadoID);



                if (bTipoCondicion == false)
                {

                    foreach (Data.Vw_GlobalCondicionesReglasConfiguraciones condciones in lista)
                    {

                        string Codigo = null;
                        string CodigoSiguiente = null;

                        switch (condciones.TipoCondicion)
                        {
                            case "Auto_Numerico":
                                {
                                    if (condciones.Valor.Any(x => char.IsNumber(x)) && condciones.Valor.Count() == condciones.LongitudCadena)
                                    {
                                        int longitud = condciones.LongitudCadena;
                                        int valor = Convert.ToInt32(condciones.Valor);
                                        int valorSiguiente = valor + 1;
                                        string longValor = "D" + longitud;
                                        Codigo = valor.ToString(longValor);
                                        CodigoSiguiente = valorSiguiente.ToString(longValor);
                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Separador":
                                {
                                    if (condciones.Valor.Count() == condciones.LongitudCadena)
                                    {
                                        Codigo = condciones.Valor;
                                        CodigoSiguiente = condciones.Valor;
                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Auto_Caracter":
                                {
                                    if (condciones.Valor.Any(x => !char.IsNumber(x) && condciones.Valor.Count() == condciones.LongitudCadena))
                                    {
                                        Codigo = condciones.Valor;
                                        CodigoSiguiente = Next(condciones.Valor).ToUpper();

                                        #region INCREMENTAR CARACTERES

                                        string Next(string str)
                                        {
                                            string result = String.Empty;
                                            int index = str.Length - 1;
                                            bool carry;
                                            do
                                            {
                                                result = Increment(str[index--], out carry) + result;
                                            }
                                            while (carry && index >= 0);
                                            if (index >= 0) result = str.Substring(0, index + 1) + result;
                                            if (carry) result = "a" + result;
                                            return result;
                                        }

                                        char Increment(char value, out bool carry)
                                        {
                                            carry = false;
                                            if (value >= 'a' && value < 'z' || value >= 'A' && value < 'Z')
                                            {
                                                return (char)((int)value + 1);
                                            }
                                            if (value == 'z') return 'A';
                                            if (value == 'Z')
                                            {
                                                carry = true;
                                                return 'a';
                                            }
                                            throw new Exception(String.Format("Invalid character value: {0}", value));
                                        }

                                        #endregion

                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Constante":
                                {
                                    if (condciones.Valor.Count() == condciones.LongitudCadena)
                                    {
                                        Codigo = condciones.Valor;
                                        CodigoSiguiente = condciones.Valor;
                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Formulario":
                                {

                                    ColumnasModeloDatosController cGlobalCondicionesColumnasTablas = new ColumnasModeloDatosController();
                                    Data.ColumnasModeloDatos DatoColumnas;

                                    TablasModeloDatosController cGlobalCondicionesTablas = new TablasModeloDatosController();
                                    Data.TablasModeloDatos DatoTablas;


                                    DatoColumnas = cGlobalCondicionesColumnasTablas.GetItem(condciones.ColumnaModeloDatoID.Value);

                                    DatoTablas = cGlobalCondicionesTablas.GetItem(long.Parse(DatoColumnas.TablaModeloDatosID.ToString()));


                                    String NombreTablaGenerico = DatoTablas.NombreTabla.ToString();
                                    String NombreTabla = "";

                                    if (NombreTablaGenerico.Contains("vw_"))
                                    {
                                        NombreTabla = NombreTablaGenerico.Remove(0, 7);
                                    }
                                    else
                                    {
                                        NombreTabla = NombreTablaGenerico.Remove(0, 4);
                                    }

                                    String NombreTablaController = NombreTabla + "Controller";

                                    try
                                    {
                                        String sMetodo = "GetDefault";

                                        NombreTabla = "TreeCore.Data." + NombreTabla;
                                        Type clase = Type.GetType(NombreTabla);
                                        Type controller = Type.GetType("CapaNegocio." + NombreTablaController);

                                        ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                                        object objetoConstructor = constuctorController.Invoke(new object[] { });

                                        MethodInfo m = controller.GetMethod(sMetodo);

                                        if (m == null)
                                        {
                                            Codigo = "[" + condciones.NombreCampo + "]";
                                            CodigoSiguiente = "[" + condciones.NombreCampo + "]";
                                        }
                                        else
                                        {
                                            ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
                                            object objetoClase = constuctorClase.Invoke(new object[] { });

                                            Object invocacion = m.Invoke(objetoConstructor, new object[] { long.Parse(hdCliID.Value.ToString()) });
                                            PropertyInfo[] properties = clase.GetProperties();
                                            foreach (PropertyInfo property in properties)
                                            {
                                                if (property.Name.Equals(DatoColumnas.NombreColumna))
                                                {
                                                    string CodigoSimular = Convert.ToString(property.GetValue(invocacion));

                                                    if (CodigoSimular != "")
                                                    {
                                                        Codigo = CodigoSimular.Substring(0, Convert.ToInt32(condciones.LongitudCadena.ToString()));
                                                        CodigoSiguiente = CodigoSimular.Substring(0, Convert.ToInt32(condciones.LongitudCadena.ToString()));
                                                    }
                                                    else
                                                    {
                                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                                        return direct;
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        return null;
                                    }

                                }
                                break;
                            case "Tabla":
                                {

                                    ColumnasModeloDatosController cGlobalCondicionesColumnasTablas = new ColumnasModeloDatosController();
                                    Data.ColumnasModeloDatos DatoColumnas;

                                    TablasModeloDatosController cGlobalCondicionesTablas = new TablasModeloDatosController();
                                    Data.TablasModeloDatos DatoTablas;


                                    DatoColumnas = cGlobalCondicionesColumnasTablas.GetItem(condciones.ColumnaModeloDatoID.Value);

                                    DatoTablas = cGlobalCondicionesTablas.GetItem(long.Parse(DatoColumnas.TablaModeloDatosID.ToString()));


                                    String NombreTablaGenerico = DatoTablas.NombreTabla.ToString();
                                    String NombreTabla = NombreTablaGenerico.Remove(0, 4);
                                    String NombreTablaController = NombreTabla + "Controller";

                                    try
                                    {
                                        String sMetodo = "GetDefault";

                                        NombreTabla = "TreeCore.Data." + NombreTabla;
                                        Type clase = Type.GetType(NombreTabla);
                                        Type controller = Type.GetType("CapaNegocio." + NombreTablaController);

                                        ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                                        object objetoConstructor = constuctorController.Invoke(new object[] { });
                                        ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
                                        object objetoClase = constuctorClase.Invoke(new object[] { });

                                        MethodInfo m = controller.GetMethod(sMetodo);

                                        if (m == null)
                                        {
                                            Codigo = "[" + condciones.NombreCampo + "]";
                                            CodigoSiguiente = "[" + condciones.NombreCampo + "]";
                                        }
                                        else
                                        {
                                            Object invocacion = m.Invoke(objetoConstructor, new object[] { long.Parse(hdCliID.Value.ToString()) });
                                            PropertyInfo[] properties = clase.GetProperties();
                                            foreach (PropertyInfo property in properties)
                                            {
                                                if (property.Name.Equals(DatoColumnas.NombreColumna))
                                                {
                                                    string CodigoSimular = Convert.ToString(property.GetValue(invocacion));

                                                    if (CodigoSimular != "")
                                                    {
                                                        Codigo = CodigoSimular.Substring(0, Convert.ToInt32(condciones.LongitudCadena.ToString()));
                                                        CodigoSiguiente = CodigoSimular.Substring(0, Convert.ToInt32(condciones.LongitudCadena.ToString()));
                                                    }
                                                    else
                                                    {
                                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                                        return direct;
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        return null;
                                    }

                                }
                                break;
                        }


                        CodigoFinal += Codigo;
                        CodigoSiguienteFinal += CodigoSiguiente;
                    }

                    txtSimulacionCodido.Text = CodigoFinal;

                    txtSimulacionCodidoSiguinete.Text = CodigoSiguienteFinal;

                }
                else
                {

                    foreach (Data.Vw_GlobalCondicionesReglasConfiguraciones condciones in lista)
                    {

                        string Codigo = null;
                        string CodigoSiguiente = null;

                        switch (condciones.TipoCondicion)
                        {
                            case "Auto_Numerico":
                                {
                                    if (condciones.Valor.Any(x => char.IsNumber(x)) && condciones.Valor.Count() == condciones.LongitudCadena)
                                    {
                                        int longitud = condciones.LongitudCadena;
                                        int valor = Convert.ToInt32(condciones.Valor);
                                        int valorSiguiente = valor + 1;
                                        string longValor = "D" + longitud;
                                        Codigo = valor.ToString(longValor);
                                        CodigoSiguiente = valorSiguiente.ToString(longValor);
                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Separador":
                                {
                                    if (condciones.Valor.Count() == condciones.LongitudCadena)
                                    {
                                        Codigo = condciones.Valor;
                                        CodigoSiguiente = condciones.Valor;
                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Auto_Caracter":
                                {
                                    if (condciones.Valor.Any(x => !char.IsNumber(x) && condciones.Valor.Count() == condciones.LongitudCadena))
                                    {
                                        Codigo = condciones.Valor;
                                        CodigoSiguiente = Next(condciones.Valor).ToUpper();

                                        #region INCREMENTAR CARACTERES

                                        string Next(string str)
                                        {
                                            string result = String.Empty;
                                            int index = str.Length - 1;
                                            bool carry;
                                            do
                                            {
                                                result = Increment(str[index--], out carry) + result;
                                            }
                                            while (carry && index >= 0);
                                            if (index >= 0) result = str.Substring(0, index + 1) + result;
                                            if (carry) result = "a" + result;
                                            return result;
                                        }

                                        char Increment(char value, out bool carry)
                                        {
                                            carry = false;
                                            if (value >= 'a' && value < 'z' || value >= 'A' && value < 'Z')
                                            {
                                                return (char)((int)value + 1);
                                            }
                                            if (value == 'z') return 'A';
                                            if (value == 'Z')
                                            {
                                                carry = true;
                                                return 'a';
                                            }
                                            throw new Exception(String.Format("Invalid character value: {0}", value));
                                        }

                                        #endregion

                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;
                            case "Constante":
                                {
                                    if (condciones.Valor.Count() == condciones.LongitudCadena)
                                    {
                                        Codigo = condciones.Valor;
                                        CodigoSiguiente = condciones.Valor;
                                    }
                                    else
                                    {
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                break;


                        }

                        CodigoFinal += Codigo;
                        CodigoSiguienteFinal += CodigoSiguiente;
                    }


                    txtSimulacionCodido.Text = CodigoFinal;


                    txtSimulacionCodidoSiguinete.Text = CodigoSiguienteFinal;

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

        [DirectMethod]
        public void DirectShowHidePnAsideR()
        {


            btnCollapseAsRClosed.Show();
            //  X.Call("hidePnLiteDirect");

            //WrapGestionColumnas.Hide();
            //WrapMainContent1.Show();


            pnAsideR.AnimCollapse = true;
            pnAsideR.Expand();

        }


        [DirectMethod()]
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            GlobalCondicionesReglasController cGlobalCondicionesReglas = new GlobalCondicionesReglasController();
            Data.GlobalCondicionesReglas oDato;

            try
            {
                long lID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                String lCampoDestino = hdCampoDestino.Value.ToString();



                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cGlobalCondicionesReglas.GetDefault(lCampoDestino);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.GlobalCondicionReglaID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cGlobalCondicionesReglas.UpdateItem(oDato);
                        }                        
                    }
                    oDato = cGlobalCondicionesReglas.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cGlobalCondicionesReglas.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cGlobalCondicionesReglas.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cGlobalCondicionesReglas.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalCondicionesReglasController cController = new GlobalCondicionesReglasController();

            try
            {
                long lID = long.Parse(hd_MenuSeleccionado.Value.ToString());

                Data.GlobalCondicionesReglas oDato;
                oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cController.UpdateItem(oDato))
                {
                    CargarMenu();
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

    }
}