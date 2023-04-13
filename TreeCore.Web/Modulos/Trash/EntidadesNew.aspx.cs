using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.Data;
using TreeCore.Page;
using SortDirection = Ext.Net.SortDirection;
using Button = Ext.Net.Button;
using ListItem = Ext.Net.ListItem;
using ListItemCollection = Ext.Net.ListItemCollection;
using System.Transactions;
using TreeCore.CapaNegocio.Global.Administracion;
using System.Data.SqlClient;

namespace TreeCore.ModGlobal.pages
{


    public partial class EntidadesNew : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;

        #region GESTION DE PAGINA
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.SetValue(ClienteID);
                    hdCliID.DataBind();
                }


                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
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
                            string sModuloID = Request.QueryString["aux"].ToString();
                            long CliID = long.Parse(Request.QueryString["cliente"]);
                            string tipoEntidad = Request.QueryString["aux3"].ToString();
                            int iCount = 0;
                            List<Data.Vw_Entidades> ListaDatos = new List<Vw_Entidades>();
                            if (tipoEntidad == "null")
                            {
                                tipoEntidad = "";

                            }
                            switch (tipoEntidad)
                            {
                                case "":
                                    construirGridEntidades();
                                    ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, ListaDatos, Response, "Contactos", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;
                                case "Alls":
                                    construirGridEntidades();
                                    ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, ListaDatos, Response, "Contactos", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;
                                case "Operators":
                                    construirGridOperador();
                                    List<Vw_EntidadesOperadores> listaOperadores = new List<Vw_EntidadesOperadores>();
                                    listaOperadores = ListaOperadores(long.Parse(hdCliID.Value.ToString()));
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, listaOperadores, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;
                                case "Suppliers":
                                    construirGridProveedor();
                                    List<Vw_EntidadesProveedores> listaProveedores = new List<Vw_EntidadesProveedores>();
                                    listaProveedores = ListaProveedores(long.Parse(hdCliID.Value.ToString()));
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, listaProveedores, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;
                                case "Owners":
                                    construirGridPropietarios();
                                    ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, ListaDatos, Response, "Contactos", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;
                                case "Companies":
                                    construirGridEmpresaProveedora();
                                    ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, ListaDatos, Response, "Contactos", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;

                                default:
                                    construirGridEntidades();
                                    ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);
                                    Comun.ExportacionDesdeListaNombre(gridEntities.ColumnModel, ListaDatos, Response, "Contactos", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);

                                    break;


                            }

                            try
                            {
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            Response.Write("ERROR: " + ex.Message);
                        }

                        Response.End();
                    }

                    ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
                    ResourceManagerTreeCore.RegisterIcon(Icon.ChartCurve);
                }
                #endregion

                hdControlProveedor.SetValue("false");
                hdControlOperador.SetValue("false");
                hdControlEmpresaProveedora.SetValue("false");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                this.storePrincipal.Reload();
            }
        }

        #endregion

        #region STORES

        #region GRID DINÁMICO

        private void RefreshSet(object sender, StoreReadDataEventArgs e)
        {
            switch (this.CurrentSet.Text)
            {
                case "Alls":
                    this.construirGridEntidades();
                    break;
                case "Operators":
                    this.construirGridOperador();
                    break;
                case "Owners":
                    this.construirGridPropietarios();
                    this.CurrentSet.Text = "Owners";
                    break;
                case "Companies":
                    this.construirGridEmpresaProveedora();
                    this.CurrentSet.Text = "Companies";
                    break;
                case "Suppliers":
                    this.construirGridProveedor();
                    this.CurrentSet.Text = "Suppliers";
                    break;
                default:
                    this.RefreshSet(sender, e);
                    break;
            }

            CargarDatosStores(sender, e);

            #region FILTROS

            List<string> listaIgnore = new List<string>()
            { };
            Comun.CreateGridFilters(gridFilters, storePrincipal, gridEntities.ColumnModel, listaIgnore, _Locale);
            log.Info("Creados los filtros de los Entidades");

            log.Info(GetGlobalResource(Comun.LogFiltrosEntidades));

            #endregion

        }
        private void CargarDatosStores(object sender, StoreReadDataEventArgs e)
        {
            try
            {
                DataSorter ordenar = new DataSorter { Property = "EntidadID", Direction = SortDirection.ASC };
                string sSort, sDir = null;
                sSort = "EntidadID";
                sDir = "ASC";
                string sOrden = Request.QueryString["orden"];
                int iCount = 0;
                string sFiltro = e.Parameters["gridFilters"];
                var lista = new List<Vw_Entidades>();
                string tipoEntidad = "";
                if (cmbEntidades.SelectedItem.Value != null)
                {
                    tipoEntidad = cmbEntidades.SelectedItem.Value.ToString();

                }
                if (hdCliID.Value.Equals("0"))
                {
                    //if (cmpFiltro.ClienteID != 0)
                    //{

                    switch (tipoEntidad)
                    {
                        case "":
                            //lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cmpFiltro.ClienteID);
                            lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, 14);
                            this.storePrincipal.DataSource = lista;
                            break;
                        case "Alls":
                            lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, 14);
                            //lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cmpFiltro.ClienteID);
                            this.storePrincipal.DataSource = lista;
                            break;
                        case "Operators":
                            List<Vw_EntidadesOperadores> listaOperadores = new List<Vw_EntidadesOperadores>();
                            //listaOperadores = ListaOperadores(cmpFiltro.ClienteID);
                            listaOperadores = ListaOperadores(14);
                            this.storePrincipal.DataSource = listaOperadores;
                            break;
                        case "Suppliers":
                            List<Vw_EntidadesProveedores> listaProveedores = new List<Vw_EntidadesProveedores>();
                            //listaProveedores = ListaProveedores(cmpFiltro.ClienteID);
                            listaProveedores = ListaProveedores(14);
                            this.storePrincipal.DataSource = listaProveedores;
                            break;

                        default:
                            //lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cmpFiltro.ClienteID);
                            lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, 14);
                            this.storePrincipal.DataSource = lista;
                            break;

                    }



                    hdCliID.SetValue(14);
                    //hdCliID.SetValue(cmpFiltro.ClienteID);
                    //}
                }
                else
                {
                    switch (tipoEntidad)
                    {
                        case "":
                            lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));
                            this.storePrincipal.DataSource = lista;
                            break;
                        case "Alls":
                            lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));
                            this.storePrincipal.DataSource = lista;
                            break;
                        case "Operators":
                            List<Vw_EntidadesOperadores> listaOperadores = new List<Vw_EntidadesOperadores>();
                            listaOperadores = ListaOperadores(long.Parse(hdCliID.Value.ToString()));
                            this.storePrincipal.DataSource = listaOperadores;
                            break;
                        case "Suppliers":
                            List<Vw_EntidadesProveedores> listaProveedores = new List<Vw_EntidadesProveedores>();
                            listaProveedores = ListaProveedores(long.Parse(hdCliID.Value.ToString()));
                            this.storePrincipal.DataSource = listaProveedores;
                            break;

                        default:
                            lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));
                            this.storePrincipal.DataSource = lista;
                            break;

                    }
                }

                this.storePrincipal.DataBind();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

        }
        private void AddField(ModelField field)
        {
            if (X.IsAjaxRequest)
            {
                this.storePrincipal.AddField(field);
            }
            else
            {
                this.storePrincipal.Model[0].Fields.Add(field);
            }
        }


        #region ENTIDADES

        protected void storePrincipal_Refresh(object sender, StoreReadDataEventArgs e)
        {
            switch (cmbEntidades.SelectedItem.Value)
            {
                case "Alls":
                    this.construirGridEntidades();
                    this.CurrentSet.Text = "Alls";
                    break;
                case "Operators":
                    this.construirGridOperador();
                    this.CurrentSet.Text = "Operators";
                    break;
                case "Owners":
                    this.construirGridPropietarios();
                    this.CurrentSet.Text = "Owners";
                    break;
                case "Companies":
                    this.construirGridEmpresaProveedora();
                    this.CurrentSet.Text = "Companies";
                    break;
                case "Suppliers":
                    this.construirGridProveedor();
                    this.CurrentSet.Text = "Suppliers";
                    break;
                default:
                    this.RefreshSet(sender, e);
                    break;
            }

            CargarDatosStores(sender, e);
            #region FILTROS

            List<string> listaIgnore = new List<string>()
            { };
            Comun.CreateGridFilters(gridFilters, storePrincipal, gridEntities.ColumnModel, listaIgnore, _Locale);
            log.Info("Creados los filtros de los Entidades");

            log.Info(GetGlobalResource(Comun.LogFiltrosEntidades));
            #endregion
        }
        private List<Data.Vw_Entidades> ListaPrincipal(int start, int limit, string sSort, string sDir, ref int iCount, string sFiltro, long lClienteID)
        {
            List<Data.Vw_Entidades> listaDatos = new List<Data.Vw_Entidades>();
            EntidadesController cEntidades = new EntidadesController();
            string tipoEntidad = "";
            if (cmbEntidades.SelectedItem.Value != null)
            {
                tipoEntidad = cmbEntidades.SelectedItem.Value.ToString();

            }
            try
            {
                switch (tipoEntidad)
                {
                    case "":
                        listaDatos = cEntidades.GetItemsWithExtNetFilterList<Data.Vw_Entidades>(sFiltro, sSort, sDir, start, limit, ref iCount, "ClienteID ==" + lClienteID);
                        break;
                    case "Alls":
                        listaDatos = cEntidades.GetItemsWithExtNetFilterList<Data.Vw_Entidades>(sFiltro, sSort, sDir, start, limit, ref iCount, "ClienteID ==" + lClienteID);
                        break;
                    default:
                        listaDatos = cEntidades.GetEntidadPorTipo(tipoEntidad, lClienteID);
                        break;

                }


            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        private void construirGridEntidades()
        {
            if (X.IsAjaxRequest)
            {
                this.storePrincipal.RemoveFields();
            }
            this.AddField(new ModelField("EntidadID"));
            this.AddField(new ModelField("Nombre"));
            this.AddField(new ModelField("Codigo"));
            this.AddField(new ModelField("Alias"));
            this.AddField(new ModelField("Email"));
            this.AddField(new ModelField("Telefono"));
            this.AddField(new ModelField("Direccion"));
            this.AddField(new ModelField("Provincia"));
            this.AddField(new ModelField("Municipio"));
            this.AddField(new ModelField("EntidadTipo"));
            this.AddField(new ModelField("Contactos", ModelFieldType.Object));
            this.AddField(new ModelField("NumMaximoUsuarios", ModelFieldType.Int));
            this.AddField(new ModelField("EsPropietario", ModelFieldType.Boolean));
            this.AddField(new ModelField("EsProveedor", ModelFieldType.Boolean));
            this.AddField(new ModelField("EsEmpresaProveedora", ModelFieldType.Boolean));
            this.AddField(new ModelField("EsOperador", ModelFieldType.Boolean));

            this.storePrincipal.RebuildMeta();

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Codigo", Text = GetGlobalResource("strDniCif"), MinWidth=100, Flex=1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Nombre", Text = GetGlobalResource("strNombre"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Alias", Text = GetGlobalResource("strAlias") });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "EntidadTipo", Text = GetGlobalResource("strEntidadTipo"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Telefono", Text = GetGlobalResource("strTelefono"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Email", Text = GetGlobalResource("strEmail"), MinWidth = 100, Flex = 1 });

            Column columnaNumMaximoUsuario = new Column { DataIndex = "NumMaximoUsuarios", Text = GetGlobalResource("strNumMaximoUsuarios"), MinWidth = 100, Flex = 1 };
            columnaNumMaximoUsuario.Renderer.Fn = "numMaximo";
            this.gridEntities.ColumnModel.Columns.Add(columnaNumMaximoUsuario);

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Provincia", Text = GetGlobalResource("strProvincia"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Municipio", Text = GetGlobalResource("strMunicipio"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Direccion", Text = GetGlobalResource("strDireccion"), MinWidth = 100, Flex = 1 });

            Column columnaPropietario = new Column { DataIndex = "EsPropietario", Text = GetGlobalResource("strPropietario"), MinWidth = 100, Flex = 1 };
            columnaPropietario.Renderer.Fn = "ownerRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaPropietario);

            Column columnaProveedor = new Column { DataIndex = "EsProveedor", Text = GetGlobalResource("strProveedor"), MinWidth = 100, Flex = 1 };
            columnaProveedor.Renderer.Fn = "supplierRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaProveedor);

            Column columnaEmpresaProveedora = new Column { DataIndex = "EsEmpresaProveedora", Text = GetGlobalResource("strEmpresaProveedora"), MinWidth = 100, Flex = 1 };
            columnaEmpresaProveedora.Renderer.Fn = "companyRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaEmpresaProveedora);

            Column columnaOperador = new Column { DataIndex = "EsOperador", Text = GetGlobalResource("strOperador"), MinWidth = 100, Flex = 1 };
            columnaOperador.Renderer.Fn = "operatorRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaOperador);


            Button botonMore = new Button { };
            botonMore.Listeners.Click.Fn = "";
            botonMore.PressedCls = "Pressed-none";
            botonMore.Cls = "ico-masInfo-widget";
            botonMore.OverCls = "Over-btnMore";
            botonMore.FocusCls = "Focus-none";

            WidgetColumn columnaMore = new WidgetColumn {Text = GetGlobalResource("strMas"), ID="colVerMas", Draggable=false, Hideable=false, Width=50 };
            columnaMore.Widget.Add(botonMore);
            this.gridEntities.ColumnModel.Columns.Add(columnaMore);

            if (X.IsAjaxRequest)
            {
                this.gridEntities.Reconfigure();
                //this.gridEntities.UpdateLayout();
                //this.gridEntities.Update();
            }
        }

        #endregion

        #region OPERADORES
        private List<Data.Vw_EntidadesOperadores> ListaOperadores(long lClienteID)
        {
            List<Data.Vw_EntidadesOperadores> listaDatos = new List<Data.Vw_EntidadesOperadores>();
            EntidadesOperadoresController cEntidades = new EntidadesOperadoresController();
            try
            {
                listaDatos = cEntidades.GetAllOperadores();

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }

        private void construirGridOperador()
        {
            if (X.IsAjaxRequest)
            {
                this.storePrincipal.RemoveFields();
            }
            this.AddField(new ModelField("EntidadID"));
            this.AddField(new ModelField("Nombre"));
            this.AddField(new ModelField("Codigo"));
            this.AddField(new ModelField("Alias"));
            this.AddField(new ModelField("Email"));
            this.AddField(new ModelField("Telefono"));
            this.AddField(new ModelField("EntidadTipo"));
            this.AddField(new ModelField("EsCliente", ModelFieldType.Boolean));
            this.AddField(new ModelField("Torrero", ModelFieldType.Boolean));
            this.AddField(new ModelField("Friendly", ModelFieldType.Boolean));
            this.storePrincipal.RebuildMeta();

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Codigo", Text = GetGlobalResource("strDniCif"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Nombre", Text = GetGlobalResource("strNombre"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Alias", Text = GetGlobalResource("strAlias"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "EntidadTipo", Text = GetGlobalResource("strEntidadTipo"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Telefono", Text = GetGlobalResource("strTelefono"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Email", Text = GetGlobalResource("strEmail"), MinWidth = 100, Flex = 1 });

            Column columnaEsCliente = new Column { DataIndex = "EsCliente", Text = GetGlobalResource("strEsCliente"), MinWidth = 100, Flex = 1 };
            columnaEsCliente.Renderer.Fn = "DefectoRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaEsCliente);

            Column columnaTorrero = new Column { DataIndex = "Torrero", Text = GetGlobalResource("strTorreros"), MinWidth = 100, Flex = 1 };
            columnaTorrero.Renderer.Fn = "DefectoRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaTorrero);

            Column columnaFriendly = new Column { DataIndex = "Friendly", Text = GetGlobalResource("strFriendly"), MinWidth = 100, Flex = 1 };
            columnaFriendly.Renderer.Fn = "DefectoRender";
            this.gridEntities.ColumnModel.Columns.Add(columnaFriendly);

            if (X.IsAjaxRequest)
            {
                this.gridEntities.Reconfigure();
            }
        }

        #endregion

        #region PROPIETARIOS
        private void construirGridPropietarios()
        {
            if (X.IsAjaxRequest)
            {
                this.storePrincipal.RemoveFields();
            }
            this.AddField(new ModelField("EntidadID"));
            this.AddField(new ModelField("Nombre"));
            this.AddField(new ModelField("Codigo"));
            this.AddField(new ModelField("Alias"));
            this.AddField(new ModelField("Email"));
            this.AddField(new ModelField("Telefono"));
            this.AddField(new ModelField("Direccion"));
            this.AddField(new ModelField("Provincia"));
            this.AddField(new ModelField("Municipio"));
            this.AddField(new ModelField("EntidadTipo"));
            this.AddField(new ModelField("NumMaximoUsuarios", ModelFieldType.Int));

            this.storePrincipal.RebuildMeta();

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Codigo", Text = GetGlobalResource("strDniCif"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Nombre", Text = GetGlobalResource("strNombre"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Alias", Text = GetGlobalResource("strAlias"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "EntidadTipo", Text = GetGlobalResource("strEntidadTipo"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Telefono", Text = GetGlobalResource("strTelefono"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Email", Text = GetGlobalResource("strEmail"), MinWidth = 100, Flex = 1 });

            Column columnaNumMaximoUsuario = new Column { DataIndex = "NumMaximoUsuarios", Text = GetGlobalResource("strNumMaximoUsuarios"), MinWidth = 100, Flex = 1 };
            columnaNumMaximoUsuario.Renderer.Fn = "numMaximo";
            this.gridEntities.ColumnModel.Columns.Add(columnaNumMaximoUsuario);

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Provincia", Text = GetGlobalResource("strProvincia"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Municipio", Text = GetGlobalResource("strMunicipio"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Direccion", Text = GetGlobalResource("strDireccion"), MinWidth = 100, Flex = 1 });


            if (X.IsAjaxRequest)
            {
                this.gridEntities.Reconfigure();
            }
        }

        #endregion

        #region EMPRESAS PROVEEDORAS
        private void construirGridEmpresaProveedora()
        {
            try
            {
                if (X.IsAjaxRequest)
                {
                    this.storePrincipal.RemoveFields();
                }
                this.AddField(new ModelField("EntidadID"));
                this.AddField(new ModelField("Nombre"));
                this.AddField(new ModelField("Codigo"));
                this.AddField(new ModelField("Telefono"));
                this.AddField(new ModelField("Alias"));
                this.AddField(new ModelField("Email"));
                this.AddField(new ModelField("EntidadTipo"));
                this.AddField(new ModelField("Modulos", ModelFieldType.Object));

                this.storePrincipal.RebuildMeta();

                this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Codigo", Text = GetGlobalResource("strDniCif"), MinWidth = 100, Flex = 1 });
                this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Nombre", Text = GetGlobalResource("strNombre"), MinWidth = 100, Flex = 1 });
                this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Alias", Text = GetGlobalResource("strAlias"), MinWidth = 100, Flex = 1 });
                this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "EntidadTipo", Text = GetGlobalResource("strEntidadTipo"), MinWidth = 100, Flex = 1 });
                this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Telefono", Text = GetGlobalResource("strTelefono"), MinWidth = 100, Flex = 1 });
                this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Email", Text = GetGlobalResource("strEmail"), MinWidth = 100, Flex = 1 });

                Button botonModulo = new Button { };
                botonModulo.Listeners.Click.Fn = "ClickShowModulos";
                botonModulo.PressedCls = "Pressed-none";
                botonModulo.Cls = "btnColMoreX";
                botonModulo.OverCls = "Over-btnMore";
                botonModulo.FocusCls = "Focus-none";

                WidgetColumn columnaModulo = new WidgetColumn { DataIndex = "Modulos", Text = GetGlobalResource("strModulo"), MinWidth = 100, Flex = 1 };
                columnaModulo.Widget.Add(botonModulo);
                this.gridEntities.ColumnModel.Columns.Add(columnaModulo);

                if (X.IsAjaxRequest)
                {
                    this.gridEntities.Reconfigure();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

        }
        protected void storeClientesProyectosTipos_refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_ClientesProyectosTipos> lista = new List<Data.Vw_ClientesProyectosTipos>();
                    lista = ListaClientesProyectosTipos();
                    storeClientesProyectosTipos.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.Vw_ClientesProyectosTipos> ListaClientesProyectosTipos()
        {
            List<Data.Vw_ClientesProyectosTipos> listaDatos = new List<Data.Vw_ClientesProyectosTipos>();
            ClientesProyectosTiposController cClientesProyectosTipos = new ClientesProyectosTiposController();
            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cClientesProyectosTipos.GetProyectosByCliente(long.Parse(clienteID));
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

        #region PROVEEDORES
        private void construirGridProveedor()
        {
            if (X.IsAjaxRequest)
            {
                this.storePrincipal.RemoveFields();
            }
            this.AddField(new ModelField("EntidadID"));
            this.AddField(new ModelField("Nombre"));
            this.AddField(new ModelField("Codigo"));
            this.AddField(new ModelField("Telefono"));
            this.AddField(new ModelField("Alias"));
            this.AddField(new ModelField("Email"));
            this.AddField(new ModelField("EntidadTipo"));
            this.AddField(new ModelField("MetodoPago"));
            this.AddField(new ModelField("TipoContribuyente"));
            this.AddField(new ModelField("SAPTipoNIF"));
            this.AddField(new ModelField("SAPTratamiento"));
            this.AddField(new ModelField("SAPGrupoCuenta"));
            this.AddField(new ModelField("SAPCuentaAsociada"));
            this.AddField(new ModelField("SAPClaveClasificacion"));
            this.AddField(new ModelField("SAPGrupoTesoreria"));
            this.AddField(new ModelField("CondicionPago"));

            this.storePrincipal.RebuildMeta();

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Codigo", Text = GetGlobalResource("strDniCif"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Nombre", Text = GetGlobalResource("strNombre"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Alias", Text = GetGlobalResource("strAlias"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "EntidadTipo", Text = GetGlobalResource("strEntidadTipo"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Telefono", Text = GetGlobalResource("strTelefono"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "Email", Text = GetGlobalResource("strEmail"), MinWidth = 100, Flex = 1 });

            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "MetodoPago", Text = GetGlobalResource("strMetodoPago"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "TipoContribuyente", Text = GetGlobalResource("strTipoContribuyente"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "SAPTipoNIF", Text = GetGlobalResource("strTipoNIF"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "SAPTratamiento", Text = GetGlobalResource("strTratamiento"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "SAPGrupoCuenta", Text = GetGlobalResource("strGrupoCuenta"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "SAPCuentaAsociada", Text = GetGlobalResource("strCuentaAsociada"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "SAPClaveClasificacion", Text = GetGlobalResource("strClaveClasificacion"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "SAPGrupoTesoreria", Text = GetGlobalResource("strGrupoTesoreria"), MinWidth = 100, Flex = 1 });
            this.gridEntities.ColumnModel.Columns.Add(new Column { DataIndex = "CondicionPago", Text = GetGlobalResource("strCondicionPago"), MinWidth = 100, Flex = 1 });

            if (X.IsAjaxRequest)
            {
                this.gridEntities.Reconfigure();
            }
        }
        private List<Data.Vw_EntidadesProveedores> ListaProveedores(long lClienteID)
        {
            List<Data.Vw_EntidadesProveedores> listaDatos = new List<Data.Vw_EntidadesProveedores>();
            EntidadesProveedoresController cEntidades = new EntidadesProveedoresController();
            try
            {
                listaDatos = cEntidades.GetAllProveedores();

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

        #region ENTIDADES TIPOS
        protected void storeEntidadTipo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EntidadesTipos> lista = new List<Data.EntidadesTipos>();
                    lista = ListaEntidadesTipos();
                    storeEntidadTipo.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.EntidadesTipos> ListaEntidadesTipos()
        {
            List<Data.EntidadesTipos> listaDatos = new List<Data.EntidadesTipos>();
            EntidadesTiposController cEntidadesTipos = new EntidadesTiposController();

            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cEntidadesTipos.GetEntidadesTiposByCliente(long.Parse(clienteID));
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

        #region PROVEEDORES

        protected void storeMetodosPago_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.MetodosPagos> lista = new List<Data.MetodosPagos>();
                    lista = ListaMetodosPagos();
                    storeMetodosPago.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.MetodosPagos> ListaMetodosPagos()
        {
            List<Data.MetodosPagos> listaDatos = new List<Data.MetodosPagos>();
            MetodosPagosController cMetodosPago = new MetodosPagosController();

            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cMetodosPago.GetMetodosPagoByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeTiposContribuyentes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.TiposContribuyentes> lista = new List<Data.TiposContribuyentes>();
                    lista = ListaTiposContribuyentes();
                    storeTiposContribuyentes.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.TiposContribuyentes> ListaTiposContribuyentes()
        {
            List<Data.TiposContribuyentes> listaDatos = new List<Data.TiposContribuyentes>();
            TiposContribuyentesController cTiposContribuyentes = new TiposContribuyentesController();

            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cTiposContribuyentes.GetTiposContribuyentesByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeSAPTipoNIF_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.SAPTiposNIF> lista = new List<Data.SAPTiposNIF>();
                    lista = ListaSAPTiposNIF();
                    storeSAPTipoNIF.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.SAPTiposNIF> ListaSAPTiposNIF()
        {
            List<Data.SAPTiposNIF> listaDatos = new List<Data.SAPTiposNIF>();
            SAPTiposNIFController cSAPTiposNIF = new SAPTiposNIFController();

            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cSAPTiposNIF.GetSAPTiposNIFByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeSAPTratamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.SAPTratamientos> lista = new List<Data.SAPTratamientos>();
                    lista = ListaSAPTratamiento();
                    storeSAPTratamientos.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.SAPTratamientos> ListaSAPTratamiento()
        {
            List<Data.SAPTratamientos> listaDatos = new List<Data.SAPTratamientos>();
            SAPTratamientosController cSAPTratamientos = new SAPTratamientosController();

            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cSAPTratamientos.GetSAPTratamientosByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeSAPGruposCuentas_refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.SAPGruposCuentas> lista = new List<Data.SAPGruposCuentas>();
                    lista = ListaSAPGruposCuenta();
                    storeSAPGruposCuentas.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.SAPGruposCuentas> ListaSAPGruposCuenta()
        {
            List<Data.SAPGruposCuentas> listaDatos = new List<Data.SAPGruposCuentas>();
            SAPGruposCuentasController cSAPGruposCuenta = new SAPGruposCuentasController();
            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cSAPGruposCuenta.GetSAPGruposCuentasByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeSAPCuentasAsociadas_refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.SAPCuentasAsociadas> lista = new List<Data.SAPCuentasAsociadas>();
                    lista = ListaSAPCuentasAsociadas();
                    storeSAPCuentasAsociadas.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.SAPCuentasAsociadas> ListaSAPCuentasAsociadas()
        {
            List<Data.SAPCuentasAsociadas> listaDatos = new List<Data.SAPCuentasAsociadas>();
            SAPCuentasAsociadasController cSAPCuentasAsociadas = new SAPCuentasAsociadasController();
            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cSAPCuentasAsociadas.GetSAPCuentasAsociadasByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeSAPClavesClasificaciones_refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.SAPClavesClasificaciones> lista = new List<Data.SAPClavesClasificaciones>();
                    lista = ListaSAPClavesClasificaciones();
                    storeSAPClavesClasificaciones.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.SAPClavesClasificaciones> ListaSAPClavesClasificaciones()
        {
            List<Data.SAPClavesClasificaciones> listaDatos = new List<Data.SAPClavesClasificaciones>();
            SAPClavesClasificacionesController cSAPClavesClasificaciones = new SAPClavesClasificacionesController();
            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cSAPClavesClasificaciones.GetSAPClavesClasificacionesByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeSAPGruposTesorerias_refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.SAPGruposTesorerias> lista = new List<Data.SAPGruposTesorerias>();
                    lista = ListaSAPGruposTesorerias();
                    storeSAPGruposTesorerias.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.SAPGruposTesorerias> ListaSAPGruposTesorerias()
        {
            List<Data.SAPGruposTesorerias> listaDatos = new List<Data.SAPGruposTesorerias>();
            SAPGruposTesoreriasController cSAPGruposTesorerias = new SAPGruposTesoreriasController();
            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cSAPGruposTesorerias.GetSAPGruposTesoreriasByCliente(long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }
        protected void storeCondicionesPagos_refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CondicionesPagos> lista = new List<Data.CondicionesPagos>();
                    lista = ListaCondicionesPagos();
                    storeCondicionesPagos.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.CondicionesPagos> ListaCondicionesPagos()
        {
            List<Data.CondicionesPagos> listaDatos = new List<Data.CondicionesPagos>();
            CondicionesPagosController cCondicionesPagos = new CondicionesPagosController();
            try
            {
                if (hdCliID.Value != null)
                {
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cCondicionesPagos.GetCondicionesPagosByCliente(long.Parse(clienteID));
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

        #region EMPRESAS PROVEEDORAS
        protected void storeModulosEmpresaProveedora_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.ProyectosTipos> lista = new List<Data.ProyectosTipos>();
                    lista = ListaProyectosTipos();
                    storeModulosEmpresaProveedora.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.ProyectosTipos> ListaProyectosTipos()
        {
            EntidadesController cEntidades = new EntidadesController();
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
            EmpresasProveedorasProyectosTiposController cEmpresasProveedorasProyectosTipos = new EmpresasProveedorasProyectosTiposController();

            Data.Entidades dato = new Data.Entidades();
            long S = long.Parse(hdEntidadID.Value.ToString());
            dato = cEntidades.GetItem(S);

            List<Data.ProyectosTipos> listaDatos = new List<Data.ProyectosTipos>();
            List<EmpresasProveedorasProyectosTipos> listaEmpresasProveedorasProyectosTipos = new List<EmpresasProveedorasProyectosTipos>();

            listaEmpresasProveedorasProyectosTipos = cEmpresasProveedorasProyectosTipos.getByEmpresaProveedora(dato.EmpresaProveedoraID.Value);
            try
            {
                if (hdCliID.Value != null)
                {
                    foreach (EmpresasProveedorasProyectosTipos item in listaEmpresasProveedorasProyectosTipos)
                    {
                        listaDatos.Add(cProyectosTipos.GetItem(item.ProyectoTipoID));
                    }
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

        #region CONTACTOS GLOBALES

        protected void storeContactosGlobalesEntidad_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_ContactosGlobalesEntidades> lista = new List<Vw_ContactosGlobalesEntidades>();

                    lista = ListaContactos();
                    storeContactosGlobalesEntidad.DataSource = lista;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.Vw_ContactosGlobalesEntidades> ListaContactos()
        {
            List<Data.Vw_ContactosGlobalesEntidades> listaDatos = new List<Data.Vw_ContactosGlobalesEntidades>();
            ContactosGlobalesEntidadesController cContactosGlobalesEntidades = new ContactosGlobalesEntidadesController();
            try
            {
                if (hdCliID.Value != null)
                {
                    long S = long.Parse(hdEntidadID.Value.ToString());
                    string clienteID = hdCliID.Value.ToString();
                    listaDatos = cContactosGlobalesEntidades.GetContactosGlobalesByEntidad(S, long.Parse(clienteID));
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }

        protected void storeContactosGlobalesEntidadesVinculadas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_ContactosGlobalesEntidadesVinculadas> listaContactos = ListaContactosEntidad();

                    if (listaContactos != null)
                    {
                        storeContactosGlobalesEntidadesVinculadas.DataSource = listaContactos;
                        storeContactosGlobalesEntidadesVinculadas.DataBind();

                        PageProxy proxy = (PageProxy)storeContactosGlobalesEntidad.Proxy[0];
                        proxy.Total = listaContactos.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ContactosGlobalesEntidadesVinculadas> ListaContactosEntidad()
        {
            List<Data.Vw_ContactosGlobalesEntidadesVinculadas> listaEntidades = null;
            ContactosGlobalesEntidadesVinculadasController cContactos = new ContactosGlobalesEntidadesVinculadasController();

            try
            {
                if (txtBuscarMail.Text != "")
                {
                    listaEntidades = cContactos.getContactosNoAsignadosByEmail(txtBuscarMail.Text, long.Parse(hdEntidadID.Value.ToString()));
                }
                else if (searchTel.Text != "")
                {
                    listaEntidades = cContactos.getContactosNoAsignadosByTelefono(searchTel.Text, long.Parse(hdEntidadID.Value.ToString()));
                }
                else if (hdEntidadID.Value != null)
                {
                    listaEntidades = cContactos.GetContactosGlobalesVinculadosByEntidad(long.Parse(hdEntidadID.Value.ToString()));

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEntidades = null;
            }

            return listaEntidades;
        }

        #endregion

        #endregion

        #region DIRECTMETHOD

        #region ENTIDAD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar, List<Boolean> oOperador, List<long> oProveedor, List<long> oEmpresaProveedora)
        {
            DirectResponse ajax = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();

            OperadoresController cOperadores = new OperadoresController();
            ProveedoresController cProveedores = new ProveedoresController();
            EmpresasProveedorasController cEmpresaProveedora = new EmpresasProveedorasController();
            EmpresasProveedorasProyectosTiposController cEmpresasProveedorasProyectosTipos = new EmpresasProveedorasProyectosTiposController();
            PropietariosController cPropietario = new PropietariosController();

            try
            {
                Data.Entidades dato = new Data.Entidades();
                long cliID = long.Parse(hdCliID.Value.ToString());

                if (!agregar)
                {
                    long S = long.Parse(hdEntidadID.Value.ToString());
                    dato = cEntidades.GetItem(S);

                    if (dato != null)
                    {
                        if (dato.Codigo != txtCodigo.Text && cEntidades.RegistroDuplicado(txtCodigo.Text, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            dato.Nombre = txtNombre.Text;
                            dato.Alias = txtAlias.Text;
                            dato.Activo = true;
                            dato.Codigo = txtCodigo.Text;
                            dato.EntidadTipoID = long.Parse(cmbTipoEntidad.SelectedItem.Value);
                            dato.Telefono = txtTelefono1.Text;
                            dato.ClienteID = cliID;
                            dato.Email = txtEmail.Text;
                            dato.MunicipioID = locGeografica.MunicipioID.Value;
                            dato.Direccion = txtDireccion.Text;
                            dato.CodigoPostal = txtCP.Text;


                            if (numMaxUsuarios.Value != null)
                            {
                                dato.NumMaximoUsuarios = (int)numMaxUsuarios.Number;
                            }

                            if (cEntidades.UpdateItem(dato))
                            {
                                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                                storePrincipal.DataBind();
                            }
                        }
                    }
                }
                else
                {
                    if (cEntidades.RegistroDuplicado(txtCodigo.Text, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                    }
                    else
                    {
                        using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                        {
                            try
                            {
                                dato.Nombre = txtNombre.Text;
                                dato.Alias = txtAlias.Text;
                                dato.Activo = true;
                                dato.Codigo = txtCodigo.Text;
                                dato.EntidadTipoID = long.Parse(cmbTipoEntidad.SelectedItem.Value);
                                dato.Telefono = txtTelefono1.Text;
                                dato.ClienteID = cliID;
                                dato.Email = txtEmail.Text;
                                dato.MunicipioID = locGeografica.MunicipioID.Value;
                                dato.Direccion = txtDireccion.Text;
                                dato.CodigoPostal = txtCP.Text;

                                if (numMaxUsuarios.Value != null)
                                {
                                    dato.NumMaximoUsuarios = (int)numMaxUsuarios.Number;
                                }

                                if (cEntidades.AddItem(dato) != null)
                                {
                                    if (oOperador != null)
                                    {
                                        Data.Operadores operador = new Data.Operadores();
                                        operador.Friendly = (bool)chkFriendly.Value;
                                        operador.Torrero = (bool)chkTorre.Value;
                                        operador.EsCliente = (bool)chkCliente.Value;

                                        operador.ClienteID = long.Parse(hdCliID.Value.ToString());
                                        operador.Operador = txtNombre.Text;
                                        operador.Activo = true;
                                        operador = cOperadores.AddItem(operador);

                                        if (operador != null)
                                        {
                                            dato.OperadorID = operador.OperadorID;
                                        }
                                    }

                                    if (oProveedor != null)
                                    {
                                        Data.Proveedores proveedor = new Data.Proveedores();
                                        proveedor.FacturacionDNICIF = txtCodigo.Text;
                                        proveedor.FacturacionRazonSocial = txtNombre.Text;
                                        proveedor.FacturacionDireccion = txtDireccion.Text;
                                        proveedor.FacturacionCP = txtCP.Text;
                                        proveedor.FacturacionMunicipio = "";
                                        proveedor.FacturacionProvicia = "";
                                        proveedor.PaisID = locGeografica.PaisID.Value;
                                        proveedor.Activo = true;
                                        proveedor.ClienteID = long.Parse(hdCliID.Value.ToString());

                                        proveedor.Bloqueado = false;
                                        proveedor.Validado = false;

                                        proveedor.MetodoPagoID = oProveedor[0];
                                        proveedor.TipoContribuyenteID = oProveedor[1];
                                        proveedor.SAPTipoNIFID = oProveedor[2];
                                        proveedor.SAPTratamientoID = oProveedor[3];
                                        proveedor.SAPGrupoCuentaID = oProveedor[4];
                                        proveedor.SAPCuentaAsociadaID = oProveedor[5];
                                        proveedor.SAPClaveClasificacionID = oProveedor[6];
                                        proveedor.SAPGrupoTesoreriaID = oProveedor[7];
                                        proveedor.CondicionPagoID = oProveedor[8];
                                        proveedor = cProveedores.AddItem(proveedor);

                                        if (proveedor != null)
                                        {
                                            dato.ProveedorID = proveedor.ProveedorID;
                                        }
                                    }

                                    if (oEmpresaProveedora != null)
                                    {
                                        Data.EmpresasProveedoras empresaProveedora = new Data.EmpresasProveedoras();
                                        empresaProveedora.EmpresaProveedora = txtNombre.Text;
                                        empresaProveedora.Activo = true;
                                        empresaProveedora.ClienteID = long.Parse(hdCliID.Value.ToString());
                                        empresaProveedora.CIF = txtCodigo.Text;
                                        empresaProveedora = cEmpresaProveedora.AddItem(empresaProveedora);

                                        if (empresaProveedora != null)
                                        {
                                            EmpresasProveedorasProyectosTipos empresaProveedoraProyectoTipo = new EmpresasProveedorasProyectosTipos();
                                            ListItemCollection listaSeleccionada = cmbModulos.SelectedItems;
                                            foreach (var item in listaSeleccionada)
                                            {
                                                empresaProveedoraProyectoTipo = new EmpresasProveedorasProyectosTipos();
                                                long proyectoTipoID = long.Parse(item.Value);
                                                empresaProveedoraProyectoTipo.ProyectoTipoID = proyectoTipoID;
                                                empresaProveedoraProyectoTipo.EmpresaProveedoraID = (long)dato.EmpresaProveedoraID;
                                                empresaProveedoraProyectoTipo.Activo = true;
                                                cEmpresasProveedorasProyectosTipos.AddItem(empresaProveedoraProyectoTipo);
                                            }

                                            dato.EmpresaProveedoraID = empresaProveedora.EmpresaProveedoraID;
                                        }

                                    }

                                    if (btnPropietario.Pressed)
                                    {
                                        Data.Propietarios propietario = new Data.Propietarios();

                                        propietario.Nombre = txtNombre.Text;
                                        propietario.Email = txtEmail.Text;
                                        propietario.Activo = true;
                                        propietario.ClienteID = cliID;
                                        propietario.Defecto = true;
                                        propietario.DNIPropietario = txtCodigo.Text;


                                        try
                                        {

                                            propietario = cPropietario.AddItem(propietario);

                                            if (propietario != null)
                                            {
                                                dato.PropietarioID = propietario.PropietarioID;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex.Message);
                                        }
                                    }
                                }
                                cEntidades.UpdateItem(dato);
                                trans.Complete();

                            }
                            catch
                            {
                                trans.Dispose();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";
            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            MunicipiosController cMunicipios = new MunicipiosController();

            try
            {
                long S = long.Parse(hdEntidadID.Value.ToString());
                Data.Entidades dato = cEntidades.GetItem(S);
                Data.Municipios municipio = cMunicipios.GetItem(long.Parse(dato.MunicipioID.ToString()));

                txtNombre.Text = dato.Nombre;
                txtAlias.Text = dato.Alias;
                txtCodigo.Text = dato.Codigo;
                cmbTipoEntidad.Text = dato.EntidadesTipos.EntidadTipo.ToString();
                cmbTipoEntidad.Value = dato.EntidadesTipos.EntidadTipoID.ToString();
                txtTelefono1.Text = dato.Telefono;
                txtEmail.Text = dato.Email;

                locGeografica.MunicipioID = dato.MunicipioID;
                locGeografica.ProvinciaID = municipio.ProvinciaID;
                locGeografica.RegionPaisID = municipio.Provincias.RegionPaisID;
                locGeografica.PaisID = municipio.Provincias.RegionesPaises.PaisID;

                txtDireccion.Text = dato.Direccion;
                txtCP.Text = dato.CodigoPostal;

                if (dato.NumMaximoUsuarios != null)
                {
                    numMaxUsuarios.Number = (int)dato.NumMaximoUsuarios;
                }

                if (dato.OperadorID != 0 && dato.OperadorID != null)
                {
                    btnOperador.Pressed = true;
                    btnEditarOperador.Enable();
                }
                if (dato.ProveedorID != 0 && dato.ProveedorID != null)
                {
                    btnProveedor.Pressed = true;
                    btnEditarProveedor.Enable();
                    //hdControlProveedor.SetValue("false");
                }
                if (dato.PropietarioID != 0 && dato.PropietarioID != null)
                {
                    btnPropietario.Pressed = true;
                }
                if (dato.EmpresaProveedoraID != 0 && dato.EmpresaProveedoraID != null)
                {
                    btnCompania.Pressed = true;
                    hdControlEmpresaProveedora.SetValue("false");
                    btnEditarCompania.Enable();
                }

                locGeografica.RecargarCombos();
                winGestionProveedor.DataBind();

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
            EntidadesController cEntidades = new EntidadesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            Data.Entidades dato = cEntidades.GetItem(lID);
            try
            {
                if (cEntidades.DeleteItem(lID))
                {
                    if (dato.OperadorID != 0 && dato.OperadorID != null)
                    {
                        long operadorID = dato.OperadorID.Value;
                        dato.OperadorID = null;
                        cEntidades.UpdateItem(dato);
                        OperadoresController cOperadores = new OperadoresController();
                        cOperadores.DeleteItem(operadorID);
                    }
                    if (dato.ProveedorID != 0 && dato.ProveedorID != null)
                    {
                        long ProveedorID = dato.ProveedorID.Value;
                        dato.ProveedorID = null;
                        cEntidades.UpdateItem(dato);
                        ProveedoresController cProveedores = new ProveedoresController();
                        cProveedores.DeleteItem(ProveedorID);
                    }
                    if (dato.PropietarioID != 0 && dato.PropietarioID != null)
                    {
                        long PropietarioID = dato.PropietarioID.Value;
                        dato.PropietarioID = null;
                        cEntidades.UpdateItem(dato);
                        PropietariosController cPropietarios = new PropietariosController();
                        cPropietarios.DeleteItem(PropietarioID);
                    }
                    if (dato.EmpresaProveedoraID != 0 && dato.EmpresaProveedoraID != null)
                    {
                        long EmpresaProveedoraID = dato.EmpresaProveedoraID.Value;
                        dato.EmpresaProveedoraID = null;
                        cEntidades.UpdateItem(dato);
                        EmpresasProveedorasController cEmpresasProveedoras = new EmpresasProveedorasController();
                        BorrarEmpresasProveedorasProyectosTipoByEmpresa(EmpresaProveedoraID);
                        cEmpresasProveedoras.DeleteItem(EmpresaProveedoraID);
                    }
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
        public DirectResponse GenerarPlantillaEntidades()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosEntidades") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                #region CONTROLADORES
                EntidadesTiposController cEntidadesTipos = new EntidadesTiposController();
                String valoresTipoEntidad = String.Empty;
                foreach (var valor in cEntidadesTipos.GetEntidadesTiposByCliente(cliID))
                {
                    valoresTipoEntidad += valor.EntidadTipo + ",";
                }
                valoresTipoEntidad = valoresTipoEntidad.Remove(valoresTipoEntidad.Length - 1);

                MetodosPagosController cMetodosPagos = new MetodosPagosController();
                String valoresMetodosPagos = String.Empty;
                foreach (var valor in cMetodosPagos.GetMetodosPagoByCliente(cliID))
                {
                    valoresMetodosPagos += valor.MetodoPago + ",";
                }
                valoresMetodosPagos = valoresMetodosPagos.Remove(valoresMetodosPagos.Length - 1);

                TiposContribuyentesController contribuyentesController = new TiposContribuyentesController();
                String valoresTiposContribuyentes = String.Empty;
                foreach (var valor in contribuyentesController.GetTiposContribuyentesByCliente(cliID))
                {
                    valoresTiposContribuyentes += valor.TipoContribuyente + ",";
                }
                valoresTiposContribuyentes = valoresTiposContribuyentes.Remove(valoresTiposContribuyentes.Length - 1);

                CondicionesPagosController condPagController = new CondicionesPagosController();
                String valoresCondPagos = String.Empty;
                foreach (var valor in condPagController.GetCondicionesPagosByCliente(cliID))
                {
                    valoresCondPagos += valor.CondicionPago + ",";
                }
                valoresCondPagos = valoresCondPagos.Remove(valoresCondPagos.Length - 1);

                SAPGruposTesoreriasController gruposTesoreriasController = new SAPGruposTesoreriasController();
                String valoresTesorerias = String.Empty;
                foreach (var valor in gruposTesoreriasController.GetSAPGruposTesoreriasByCliente(cliID))
                {
                    valoresTesorerias += valor.SAPGrupoTesoreria + ",";
                }
                valoresTesorerias = valoresTesorerias.Remove(valoresTesorerias.Length - 1);

                SAPClavesClasificacionesController clavesClasificacionesController = new SAPClavesClasificacionesController();
                String valoresClavesClasif = String.Empty;
                foreach (var valor in clavesClasificacionesController.GetSAPClavesClasificacionesByCliente(cliID))
                {
                    valoresClavesClasif += valor.SAPClaveClasificacion + ",";
                }
                valoresClavesClasif = valoresClavesClasif.Remove(valoresClavesClasif.Length - 1);

                SAPCuentasAsociadasController cuentasAsociadasController = new SAPCuentasAsociadasController();
                String valoresCuentasAsoc = String.Empty;
                foreach (var valor in cuentasAsociadasController.GetSAPCuentasAsociadasByCliente(cliID))
                {
                    valoresCuentasAsoc += valor.SAPCuentaAsociada + ",";
                }
                valoresCuentasAsoc = valoresCuentasAsoc.Remove(valoresCuentasAsoc.Length - 1);

                SAPGruposCuentasController gruposCuentasController = new SAPGruposCuentasController();
                String valoresGruposCuentas = String.Empty;
                foreach (var valor in gruposCuentasController.GetSAPGruposCuentasByCliente(cliID))
                {
                    valoresGruposCuentas += valor.SAPGrupoCuenta + ",";
                }
                valoresGruposCuentas = valoresGruposCuentas.Remove(valoresGruposCuentas.Length - 1);

                SAPTratamientosController tratamientosController = new SAPTratamientosController();
                String valoresTratamientos = String.Empty;
                foreach (var valor in tratamientosController.GetSAPTratamientosByCliente(cliID))
                {
                    valoresTratamientos += valor.SAPTratamiento + ",";
                }
                valoresTratamientos = valoresTratamientos.Remove(valoresTratamientos.Length - 1);

                SAPTiposNIFController tiposNIFController = new SAPTiposNIFController();
                String valoresTiposNIF = String.Empty;
                foreach (var valor in tiposNIFController.GetSAPTiposNIFByCliente(cliID))
                {
                    valoresTiposNIF += valor.Codigo + ",";
                }
                valoresTiposNIF = valoresTiposNIF.Remove(valoresTiposNIF.Length - 1);

                ClientesProyectosTiposController clientesProyTiposController = new ClientesProyectosTiposController();
                String valoresClientesProyTipos = String.Empty;
                foreach (var valor in clientesProyTiposController.GetProyectosByCliente(cliID))
                {
                    valoresClientesProyTipos += valor.Alias + ",";
                }
                valoresClientesProyTipos = valoresClientesProyTipos.Remove(valoresClientesProyTipos.Length - 1);
                #endregion

                #region Datos Entidades
                List<string> filaTipoDatoEntidades = new List<string>();
                List<string> filaValoresPosiblesEntidades = new List<string>();
                List<string> filaCabeceraEntidades = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEntidades.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEntidades.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEntidades.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEntidades.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEntidades.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEntidades.Add(GetGlobalResource("strAlfanumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "200"));
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "200"));
                filaValoresPosiblesEntidades.Add(valoresTipoEntidad);
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "100"));
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                //pais
                filaValoresPosiblesEntidades.Add("");
                //region
                filaValoresPosiblesEntidades.Add("");
                //provincia
                filaValoresPosiblesEntidades.Add("");
                //municipio
                filaValoresPosiblesEntidades.Add("");
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosiblesEntidades.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                #endregion

                #region Fila Cabecera
                filaCabeceraEntidades.Add(GetGlobalResource("strNombre"));
                filaCabeceraEntidades.Add(GetGlobalResource("strCodigo"));
                filaCabeceraEntidades.Add(GetGlobalResource("strAlias"));
                filaCabeceraEntidades.Add(GetGlobalResource("strTipo"));
                filaCabeceraEntidades.Add(GetGlobalResource("strEmail"));
                filaCabeceraEntidades.Add(GetGlobalResource("strTelefono"));
                filaCabeceraEntidades.Add(GetGlobalResource("strPais"));
                filaCabeceraEntidades.Add(GetGlobalResource("strRegionPais"));
                filaCabeceraEntidades.Add(GetGlobalResource("strProvincia"));
                filaCabeceraEntidades.Add(GetGlobalResource("strMunicipio"));
                filaCabeceraEntidades.Add(GetGlobalResource("strCodigoPostal"));
                filaCabeceraEntidades.Add(GetGlobalResource("strDireccion"));
                #endregion

                #endregion

                #region Datos Propietarios
                List<string> filaTipoDatoPropietarios = new List<string>();
                List<string> filaValoresPosiblesPropietarios = new List<string>();
                List<string> filaCabeceraPropietarios = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoPropietarios.Add(GetGlobalResource("strAlfanumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesPropietarios.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                #endregion

                #region Fila Cabecera
                filaCabeceraPropietarios.Add(GetGlobalResource("strCodigo"));
                #endregion

                #endregion

                #region Datos Operadores
                List<string> filaTipoDatoOperadores = new List<string>();
                List<string> filaValoresPosiblesOperadores = new List<string>();
                List<string> filaCabeceraOperadores = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoOperadores.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoOperadores.Add(GetGlobalResource("strBooleano"));
                filaTipoDatoOperadores.Add(GetGlobalResource("strBooleano"));
                filaTipoDatoOperadores.Add(GetGlobalResource("strBooleano"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesOperadores.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosiblesOperadores.Add(GetGlobalResource("strTrueFalse"));
                filaValoresPosiblesOperadores.Add(GetGlobalResource("strTrueFalse"));
                filaValoresPosiblesOperadores.Add(GetGlobalResource("strTrueFalse"));
                #endregion

                #region Fila Cabecera
                filaCabeceraOperadores.Add(GetGlobalResource("strCodigo"));
                filaCabeceraOperadores.Add(GetGlobalResource("strTorreros"));
                filaCabeceraOperadores.Add(GetGlobalResource("strFriendly"));
                filaCabeceraOperadores.Add(GetGlobalResource("strCliente"));
                #endregion

                #endregion

                #region Datos Proveedores
                List<string> filaTipoDatoProveedores = new List<string>();
                List<string> filaValoresPosiblesProveedores = new List<string>();
                List<string> filaCabeceraProveedores = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoProveedores.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                filaTipoDatoProveedores.Add(GetGlobalResource("jsLista"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesProveedores.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosiblesProveedores.Add(valoresMetodosPagos);
                filaValoresPosiblesProveedores.Add(valoresTiposContribuyentes);
                filaValoresPosiblesProveedores.Add(valoresCondPagos);
                filaValoresPosiblesProveedores.Add(valoresTesorerias);
                filaValoresPosiblesProveedores.Add(valoresClavesClasif);
                filaValoresPosiblesProveedores.Add(valoresCuentasAsoc);
                filaValoresPosiblesProveedores.Add(valoresGruposCuentas);
                filaValoresPosiblesProveedores.Add(valoresTratamientos);
                filaValoresPosiblesProveedores.Add(valoresTiposNIF);
                #endregion

                #region Fila Cabecera
                filaCabeceraProveedores.Add(GetGlobalResource("strCodigo"));
                filaCabeceraProveedores.Add(GetGlobalResource("strMetodoPago"));
                filaCabeceraProveedores.Add(GetGlobalResource("strContribuyente"));
                filaCabeceraProveedores.Add(GetGlobalResource("strCondicionPago"));
                filaCabeceraProveedores.Add(GetGlobalResource("strGrupoTesoreria"));
                filaCabeceraProveedores.Add(GetGlobalResource("strClaveClasificacion"));
                filaCabeceraProveedores.Add(GetGlobalResource("strCuentaAsociada"));
                filaCabeceraProveedores.Add(GetGlobalResource("strGrupoCuenta"));
                filaCabeceraProveedores.Add(GetGlobalResource("strTratamiento"));
                filaCabeceraProveedores.Add(GetGlobalResource("strTipoNIF"));
                #endregion

                #endregion

                #region Datos Empresas Proveedoras
                List<string> filaTipoDatoEmpresasProveedoras = new List<string>();
                List<string> filaValoresPosiblesEmpresasProveedoras = new List<string>();
                List<string> filaCabeceraEmpresasProveedoras = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoEmpresasProveedoras.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmpresasProveedoras.Add(GetGlobalResource("jsLista"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesEmpresasProveedoras.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosiblesEmpresasProveedoras.Add(valoresClientesProyTipos);
                #endregion

                #region Fila Cabecera
                filaCabeceraEmpresasProveedoras.Add(GetGlobalResource("strCodigo"));
                filaCabeceraEmpresasProveedoras.Add(GetGlobalResource("strModulo"));
                #endregion

                #endregion

                cEmplazamientos.ExportarModeloDatos(saveAs, GetGlobalResource("strEntidades"), filaTipoDatoEntidades, filaValoresPosiblesEntidades, filaCabeceraEntidades);
                cEmplazamientos.ExportarModeloDatosOpen(saveAs, GetGlobalResource("strPropietarios"), filaTipoDatoPropietarios, filaValoresPosiblesPropietarios, filaCabeceraPropietarios, 2);
                cEmplazamientos.ExportarModeloDatosOpen(saveAs, GetGlobalResource("strOperadores"), filaTipoDatoOperadores, filaValoresPosiblesOperadores, filaCabeceraOperadores, 3);
                cEmplazamientos.ExportarModeloDatosOpen(saveAs, GetGlobalResource("strProveedores"), filaTipoDatoProveedores, filaValoresPosiblesProveedores, filaCabeceraProveedores, 4);
                cEmplazamientos.ExportarModeloDatosOpen(saveAs, GetGlobalResource("strEmpresasProveedoras"), filaTipoDatoEmpresasProveedoras, filaValoresPosiblesEmpresasProveedoras, filaCabeceraEmpresasProveedoras, 5);

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

        #region OPERADOR

        [DirectMethod()]
        public DirectResponse AgregarEditarOperador()
        {
            DirectResponse ajax = new DirectResponse();
            OperadoresController cOperadores = new OperadoresController();
            EntidadesController cEntidades = new EntidadesController();

            try
            {

                if (hdEntidadID.Value.ToString() != "")
                {
                    Data.Entidades entidad = new Data.Entidades();
                    entidad = cEntidades.GetItem(long.Parse(hdEntidadID.Value.ToString()));

                    if (entidad != null && entidad.OperadorID != null)
                    {
                        Data.Operadores operador = new Data.Operadores();
                        operador = cOperadores.GetItem((long)entidad.OperadorID);

                        if (operador != null)
                        {
                            operador.Friendly = (bool)chkFriendly.Value;
                            operador.Torrero = (bool)chkTorre.Value;
                            operador.EsCliente = (bool)chkCliente.Value;
                            cOperadores.UpdateItem(operador);
                        }
                    }
                    else if (entidad != null && entidad.OperadorID == null)
                    {
                        Data.Operadores operador = new Data.Operadores();
                        operador.Friendly = (bool)chkFriendly.Value;
                        operador.Torrero = (bool)chkTorre.Value;
                        operador.EsCliente = (bool)chkCliente.Value;

                        operador.ClienteID = long.Parse(hdCliID.Value.ToString());
                        operador.Operador = txtNombre.Text;
                        operador.Activo = true;
                        operador = cOperadores.AddItem(operador);

                        if (operador != null)
                        {
                            entidad.OperadorID = operador.OperadorID;
                            cEntidades.UpdateItem(entidad);
                        }

                    }
                }
                else
                {
                    //oEntidadOperador.Friendly = (bool)chkFriendly.Value;
                    //oEntidadOperador.Torrero = (bool)chkTorre.Value;
                    //oEntidadOperador.EsCliente = (bool)chkCliente.Value;
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";
            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarOperador(List<Boolean> oOperador)
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();

            try
            {
                if (hdEntidadID.Value != null && hdEntidadID.Value.ToString() != "")
                {
                    long S = long.Parse(hdEntidadID.Value.ToString());

                    Data.Entidades dato = cEntidades.GetItem(S);
                    if (dato.OperadorID != 0 && dato.OperadorID != null)
                    {
                        chkFriendly.Value = dato.Operadores.Friendly;
                        chkTorre.Value = dato.Operadores.Torrero;
                        chkCliente.Value = dato.Operadores.EsCliente;
                    }
                }
                else if (oOperador != null)
                {
                    chkFriendly.Value = oOperador[0];
                    chkTorre.Value = oOperador[1];
                    chkCliente.Value = oOperador[2];
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

        [DirectMethod]
        public DirectResponse EliminarOperador()
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            Data.Entidades dato = cEntidades.GetItem(lID);
            try
            {
                if (dato.OperadorID != 0 && dato.OperadorID != null)
                {
                    long operadorID = dato.OperadorID.Value;
                    dato.OperadorID = null;
                    cEntidades.UpdateItem(dato);
                    OperadoresController cOperadores = new OperadoresController();
                    cOperadores.DeleteItem(operadorID);
                }
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


        #endregion

        #region EMPRESA PROVEEDORA

        [DirectMethod()]
        public DirectResponse AgregarEditarEmpresaProveedora()
        {
            DirectResponse ajax = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            EmpresasProveedorasProyectosTiposController cEmpresasProveedorasProyectosTipos = new EmpresasProveedorasProyectosTiposController();
            EmpresasProveedorasController cEmpresasProveedoras = new EmpresasProveedorasController();

            try
            {

                if (hdEntidadID.Value.ToString() != "")
                {
                    Data.Entidades entidad = new Data.Entidades();
                    entidad = cEntidades.GetItem(long.Parse(hdEntidadID.Value.ToString()));

                    if (entidad != null && entidad.EmpresaProveedoraID != null)
                    {
                        EmpresasProveedorasProyectosTipos empresaProveedoraProyectoTipo = new EmpresasProveedorasProyectosTipos();
                        ListItemCollection listaSeleccionada = cmbModulos.SelectedItems;
                        BorrarEmpresasProveedorasProyectosTipoByEmpresa((long)entidad.EmpresaProveedoraID);
                        foreach (var item in listaSeleccionada)
                        {
                            empresaProveedoraProyectoTipo = new EmpresasProveedorasProyectosTipos();
                            long proyectoTipoID = long.Parse(item.Value);
                            empresaProveedoraProyectoTipo.ProyectoTipoID = proyectoTipoID;
                            empresaProveedoraProyectoTipo.EmpresaProveedoraID = (long)entidad.EmpresaProveedoraID;
                            empresaProveedoraProyectoTipo.Activo = true;
                            cEmpresasProveedorasProyectosTipos.AddItem(empresaProveedoraProyectoTipo);
                        }
                    }
                    else if (entidad != null && entidad.EmpresaProveedoraID == null)
                    {
                        Data.EmpresasProveedoras empresaProveedora = new Data.EmpresasProveedoras();
                        empresaProveedora.EmpresaProveedora = txtNombre.Text;
                        empresaProveedora.Activo = true;
                        empresaProveedora.ClienteID = long.Parse(hdCliID.Value.ToString());
                        empresaProveedora.CIF = txtCodigo.Text;
                        empresaProveedora = cEmpresasProveedoras.AddItem(empresaProveedora);


                        if (empresaProveedora != null)
                        {
                            entidad.EmpresaProveedoraID = empresaProveedora.EmpresaProveedoraID;
                            cEntidades.UpdateItem(entidad);
                            EmpresasProveedorasProyectosTipos empresaProveedoraProyectoTipo = new EmpresasProveedorasProyectosTipos();
                            ListItemCollection listaSeleccionada = cmbModulos.SelectedItems;
                            foreach (var item in listaSeleccionada)
                            {
                                empresaProveedoraProyectoTipo = new EmpresasProveedorasProyectosTipos();
                                long proyectoTipoID = long.Parse(item.Value);
                                empresaProveedoraProyectoTipo.ProyectoTipoID = proyectoTipoID;
                                empresaProveedoraProyectoTipo.EmpresaProveedoraID = (long)entidad.EmpresaProveedoraID;
                                empresaProveedoraProyectoTipo.Activo = true;
                                cEmpresasProveedorasProyectosTipos.AddItem(empresaProveedoraProyectoTipo);
                            }


                        }

                    }
                }
                else
                {
                    ListItemCollection listaSeleccionada = cmbModulos.SelectedItems;
                    List<long> lEmpresaProveedoraProyectoTipoID = new List<long>();

                    foreach (var item in listaSeleccionada)
                    {
                        long proyectoTipoID = long.Parse(item.Value);
                        lEmpresaProveedoraProyectoTipoID.Add(proyectoTipoID);
                    }

                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";
            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarEmpresaProveedora(List<long> oEmpresaProveedora)
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            EmpresasProveedorasProyectosTiposController cEmpresasProveedorasProyectosTipos = new EmpresasProveedorasProyectosTiposController();

            try
            {
                if (hdEntidadID.Value != null && hdEntidadID.Value.ToString() != "")
                {
                    long S = long.Parse(hdEntidadID.Value.ToString());
                    Data.Entidades dato = cEntidades.GetItem(S);

                    if (dato.EmpresaProveedoraID != 0 && dato.EmpresaProveedoraID != null)
                    {
                        List<EmpresasProveedorasProyectosTipos> empresasProveedorasProyectosTipos = cEmpresasProveedorasProyectosTipos.getByEmpresaProveedora(long.Parse(dato.EmpresaProveedoraID.Value.ToString()));
                        List<long> lProyTipoID = new List<long>();

                        foreach (EmpresasProveedorasProyectosTipos item in empresasProveedorasProyectosTipos)
                        {
                            lProyTipoID.Add(item.ProyectoTipoID);
                        }
                        cmbModulos.Value = lProyTipoID;
                    }
                    else
                    {
                        btnCompania.Pressed = false;
                        btnEditarCompania.Disable();
                    }
                }
                else
                {
                    cmbModulos.Value = oEmpresaProveedora;
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

        protected void BorrarEmpresasProveedorasProyectosTipoByEmpresa(long empresaProveedoraID)
        {
            EmpresasProveedorasProyectosTiposController cEmpresasProveedorasProyectosTipos = new EmpresasProveedorasProyectosTiposController();
            List<EmpresasProveedorasProyectosTipos> listaEmpresasProveedorasProyectosTipos = new List<EmpresasProveedorasProyectosTipos>();
            listaEmpresasProveedorasProyectosTipos = cEmpresasProveedorasProyectosTipos.getByEmpresaProveedora(empresaProveedoraID);
            foreach (var empresaProveedoraProyectoTipoBorrar in listaEmpresasProveedorasProyectosTipos)
            {
                cEmpresasProveedorasProyectosTipos.DeleteItem(empresaProveedoraProyectoTipoBorrar.EmpresaProveedoraProyectoTipoID);
            }
        }

        [DirectMethod()]
        public DirectResponse EliminarEmpresaProveedora()
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            Data.Entidades dato = cEntidades.GetItem(lID);
            try
            {
                if (dato.EmpresaProveedoraID != 0 && dato.EmpresaProveedoraID != null)
                {
                    long EmpresaProveedoraID = dato.EmpresaProveedoraID.Value;
                    dato.EmpresaProveedoraID = null;
                    cEntidades.UpdateItem(dato);
                    EmpresasProveedorasController cEmpresasProveedoras = new EmpresasProveedorasController();
                    BorrarEmpresasProveedorasProyectosTipoByEmpresa(EmpresaProveedoraID);
                    cEmpresasProveedoras.DeleteItem(EmpresaProveedoraID);
                }
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

        #endregion

        #region PROVEEDORES

        [DirectMethod()]
        public DirectResponse AgregarEditarProveedor()
        {
            DirectResponse ajax = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            ProveedoresController cProveedores = new ProveedoresController();

            try
            {
                if (hdEntidadID.Value.ToString() != "")
                {
                    Data.Entidades entidad = new Data.Entidades();
                    entidad = cEntidades.GetItem(long.Parse(hdEntidadID.Value.ToString()));

                    if (entidad != null && entidad.ProveedorID != null)
                    {
                        Data.Proveedores proveedor = new Data.Proveedores();
                        proveedor = cProveedores.GetItem((long)entidad.ProveedorID);

                        if (proveedor != null)
                        {
                            proveedor.MetodoPagoID = long.Parse(cmbMetodoPago.SelectedItem.Value);
                            proveedor.TipoContribuyenteID = long.Parse(cmbTipoContribuyente.SelectedItem.Value);
                            proveedor.SAPTipoNIFID = long.Parse(cmbIdentificacion.SelectedItem.Value);
                            proveedor.SAPTratamientoID = long.Parse(cmbTratamiento.SelectedItem.Value);
                            proveedor.SAPGrupoCuentaID = long.Parse(cmbGrupoCuenta.SelectedItem.Value);
                            proveedor.SAPCuentaAsociadaID = long.Parse(cmbCuenta.SelectedItem.Value);
                            proveedor.SAPClaveClasificacionID = long.Parse(cmbClaveClasificacion.SelectedItem.Value);
                            proveedor.SAPGrupoTesoreriaID = long.Parse(cmbTesoreria.SelectedItem.Value);
                            proveedor.CondicionPagoID = long.Parse(cmbCondicionPago.SelectedItem.Value);

                            cProveedores.UpdateItem(proveedor);

                        }
                    }
                    else if (entidad != null && entidad.ProveedorID == null)
                    {
                        Data.Proveedores proveedor = new Data.Proveedores();
                        proveedor.FacturacionDNICIF = txtCodigo.Text;
                        proveedor.FacturacionRazonSocial = txtNombre.Text;
                        proveedor.FacturacionDireccion = txtDireccion.Text;
                        proveedor.FacturacionCP = txtCP.Text;
                        proveedor.FacturacionMunicipio = "";
                        proveedor.FacturacionProvicia = "";
                        proveedor.PaisID = locGeografica.PaisID.Value;
                        proveedor.Activo = true;
                        proveedor.ClienteID = long.Parse(hdCliID.Value.ToString());

                        proveedor.Bloqueado = false;
                        proveedor.Validado = false;

                        proveedor.MetodoPagoID = long.Parse(cmbMetodoPago.SelectedItem.Value);
                        proveedor.TipoContribuyenteID = long.Parse(cmbTipoContribuyente.SelectedItem.Value);
                        proveedor.SAPTipoNIFID = long.Parse(cmbIdentificacion.SelectedItem.Value);
                        proveedor.SAPTratamientoID = long.Parse(cmbTratamiento.SelectedItem.Value);
                        proveedor.SAPGrupoCuentaID = long.Parse(cmbGrupoCuenta.SelectedItem.Value);
                        proveedor.SAPCuentaAsociadaID = long.Parse(cmbCuenta.SelectedItem.Value);
                        proveedor.SAPClaveClasificacionID = long.Parse(cmbClaveClasificacion.SelectedItem.Value);
                        proveedor.SAPGrupoTesoreriaID = long.Parse(cmbTesoreria.SelectedItem.Value);
                        proveedor.CondicionPagoID = long.Parse(cmbCondicionPago.SelectedItem.Value);
                        proveedor = cProveedores.AddItem(proveedor);

                        if (proveedor != null)
                        {
                            entidad.ProveedorID = proveedor.ProveedorID;
                            cEntidades.UpdateItem(entidad);
                        }

                    }
                }
                else
                {
                    //oProveedor[0] = long.Parse(cmbMetodoPago.SelectedItem.Value);
                    //oProveedor[1] = long.Parse(cmbTipoContribuyente.SelectedItem.Value);
                    //oProveedor[2] = long.Parse(cmbIdentificacion.SelectedItem.Value);
                    //oProveedor[3] = long.Parse(cmbTratamiento.SelectedItem.Value);
                    //oProveedor[4] = long.Parse(cmbGrupoCuenta.SelectedItem.Value);
                    //oProveedor[5] = long.Parse(cmbCuenta.SelectedItem.Value);
                    //oProveedor[6] = long.Parse(cmbClaveClasificacion.SelectedItem.Value);
                    //oProveedor[7] = long.Parse(cmbTesoreria.SelectedItem.Value);
                    //oProveedor[8] = long.Parse(cmbCondicionPago.SelectedItem.Value);
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";
            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarProveedor(List<long> oProveedor)
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();

            try
            {
                if (hdEntidadID.Value != null && hdEntidadID.Value.ToString() != "")
                {
                    long S = long.Parse(hdEntidadID.Value.ToString());
                    Data.Entidades dato = cEntidades.GetItem(S);

                    if (dato.ProveedorID != 0 && dato.ProveedorID != null)
                    {
                        cmbMetodoPago.SetValue(dato.Proveedores.MetodosPagos.MetodoPagoID);
                        cmbTipoContribuyente.SetValue(dato.Proveedores.TiposContribuyentes.TipoContribuyenteID);
                        cmbIdentificacion.SetValue(dato.Proveedores.SAPTiposNIF.SAPTipoNIFID);
                        cmbTratamiento.SetValue(dato.Proveedores.SAPTratamientos.SAPTratamientoID);
                        cmbGrupoCuenta.SetValue(dato.Proveedores.SAPGruposCuentas.SAPGrupoCuentaID);
                        cmbCuenta.SetValue(dato.Proveedores.SAPCuentasAsociadas.SAPCuentaAsociadaID);
                        cmbClaveClasificacion.SetValue(dato.Proveedores.SAPClavesClasificaciones.SAPClaveClasificacionID);
                        cmbTesoreria.SetValue(dato.Proveedores.SAPGruposTesorerias.SAPGrupoTesoreriaID);
                        cmbCondicionPago.SetValue(dato.Proveedores.CondicionesPagos.CondicionPagoID);

                    }
                    else
                    {
                        btnProveedor.Pressed = false;
                        btnEditarProveedor.Disable();
                    }
                }
                else
                {
                    cmbMetodoPago.SetValue(oProveedor[0]);
                    cmbTipoContribuyente.SetValue(oProveedor[1]);
                    cmbIdentificacion.SetValue(oProveedor[2]);
                    cmbTratamiento.SetValue(oProveedor[3]);
                    cmbGrupoCuenta.SetValue(oProveedor[4]);
                    cmbCuenta.SetValue(oProveedor[5]);
                    cmbClaveClasificacion.SetValue(oProveedor[6]);
                    cmbTesoreria.SetValue(oProveedor[7]);
                    cmbCondicionPago.SetValue(oProveedor[8]);
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
        public DirectResponse EliminarProveedor()
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            Data.Entidades dato = cEntidades.GetItem(lID);
            try
            {
                if (dato.ProveedorID != 0 && dato.ProveedorID != null)
                {
                    long ProveedorID = dato.ProveedorID.Value;
                    dato.ProveedorID = null;
                    cEntidades.UpdateItem(dato);
                    ProveedoresController cProveedores = new ProveedoresController();
                    cProveedores.DeleteItem(ProveedorID);
                }
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

        #endregion

        #region CONTACTOS

        [DirectMethod]
        public DirectResponse AsignarEntidad(long lContactoGlobalID, long lEntidadAsignada)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesEntidadesController cContactosEntidades = new ContactosGlobalesEntidadesController();

            try
            {
                if (hdEntidadID.Value != null && hdEntidadID.Value.ToString() != "")
                {
                    long lEntidadID = long.Parse(hdEntidadID.Value.ToString());

                    if (lEntidadAsignada != 0 && lEntidadID == lEntidadAsignada)
                    {
                        ContactosGlobalesEntidades oDato = cContactosEntidades.GetContactoGlobalEntidadByIDs(lEntidadID, lContactoGlobalID);

                        if (cContactosEntidades.DeleteItem(oDato.ContactoGlobalEntidadID))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                    }
                    else
                    {
                        Data.ContactosGlobalesEntidades oDato = new Data.ContactosGlobalesEntidades();
                        oDato.ContactoGlobalID = lContactoGlobalID;
                        oDato.EntidadID = lEntidadID;
                        cContactosEntidades.AddItem(oDato);
                        log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
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

        [DirectMethod]
        public DirectResponse MostrarEditarContacto(long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                //formAgregarEditarContacto.MostrarEditarContacto(lContactoGlobalID);
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

        #region PROPIETARIO
        [DirectMethod]
        public DirectResponse EliminarPropietario()
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            Data.Entidades dato;
            if (hdControlFormulario.Value.ToString() != "agregar")
            {
                long lID = long.Parse(hdEntidadID.Value.ToString());
                dato = cEntidades.GetItem(lID);
            }
            else
            {
                dato = new Data.Entidades();
            }

            try
            {
                if (dato.PropietarioID != 0 && dato.PropietarioID != null)
                {
                    long PropietarioID = dato.PropietarioID.Value;
                    dato.PropietarioID = null;
                    cEntidades.UpdateItem(dato);
                    PropietariosController cPropietarios = new PropietariosController();
                    cPropietarios.DeleteItem(PropietarioID);
                }
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
        [DirectMethod]
        public DirectResponse AñadirPropietario()
        {
            DirectResponse ajax = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            ProveedoresController cProveedores = new ProveedoresController();

            try
            {
                if (hdEntidadID.Value.ToString() != "")
                {
                    Data.Entidades entidad = new Data.Entidades();
                    long cliID = long.Parse(hdCliID.Value.ToString());
                    entidad = cEntidades.GetItem(long.Parse(hdEntidadID.Value.ToString()));

                    Data.Propietarios propietario = new Data.Propietarios();

                    propietario.Nombre = txtNombre.Text;
                    propietario.Email = txtEmail.Text;
                    propietario.Activo = true;
                    propietario.ClienteID = cliID;
                    propietario.Defecto = true;
                    propietario.DNIPropietario = txtCodigo.Text;

                    PropietariosController cPropietarios = new PropietariosController();
                    propietario = cPropietarios.AddItem(propietario);

                    if (propietario != null)
                    {
                        entidad.PropietarioID = propietario.PropietarioID;
                    }
                    cEntidades.UpdateItem(entidad);

                }

            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";
            return ajax;


        }




        #endregion
        #endregion

        #region LOADER PARA CARGAR COMPONENTES

        //[DirectMethod()]
        //public DirectResponse LoadUserControl(string tabName, string nombre, bool update = false)
        //{
        //    DirectResponse direct = new DirectResponse();
        //    try
        //    {
        //        if (update && currentUC != null)
        //        {
        //            this.hugeCt.ContentControls.Clear();
        //        }

        //        currentUC = new TreeCore.Componentes.FormContactos();
        //        currentUC.ID = "UCFormContactos";
        //        this.hugeCt.ContentControls.Add(currentUC);

        //        if (update)
        //        {
        //            CurrentControl.Text = nombre;
        //            this.hugeCt.UpdateContent();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return direct;
        //    }

        //    direct.Success = true;
        //    direct.Result = "";
        //    return direct;

        //}

        #endregion


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

            WrapFilterControls.Hide();
            pnMoreInfo.Hide();

            WrapGestionColumnas.Show();

            pnAsideR.AnimCollapse = true;
            pnAsideR.Expand();

        }



        [DirectMethod]
        public void DirectShowHidePnAsideRFilters()
        {


            btnCollapseAsRClosed.Show();
            //  X.Call("hidePnLiteDirect");

            WrapGestionColumnas.Hide();
            pnMoreInfo.Hide();
            WrapFilterControls.Show();


            pnAsideR.AnimCollapse = true;
            pnAsideR.Expand();

        }


        [DirectMethod]
        public void DirectShowHidePnAsideRInfo()
        {


            btnCollapseAsRClosed.Show();

            WrapGestionColumnas.Hide();
            WrapFilterControls.Hide();

            pnMoreInfo.Show();


            pnAsideR.AnimCollapse = true;
            pnAsideR.Expand();

        }

    }

}