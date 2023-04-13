using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Data;
using TreeCore.Page;
namespace TreeCore.Componentes
{
    public partial class GridEmplazamientosAtributos : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Data.Usuarios oUser;
        public List<long> listaFuncionalidades = new List<long>();

        public string IDComponente
        {
            get { return this.hdIDComponente.Value.ToString(); }
            set { this.hdIDComponente.SetValue(value); }
        }

        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IDComponente = this.ID;
            

            listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_EMPLAZAMIENTOS))
            {
                btnDescargar.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_EMPLAZAMIENTOS))
            {
                btnDescargar.Hidden = false;
            }
        }

        private void Page_Init(object sender, EventArgs e)
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            UsuariosController cUsuarios = new UsuariosController();

            if (oUsuario != null)
            {
                oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
            }

            hdMaxColumns.SetValue(7);

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
                        string sFiltro2 = Request.QueryString["aux3"];
                        string sTextoBuscado = Request.QueryString["aux4"];
                        string sIdBuscado = Request.QueryString["aux5"];
                        int iCount = 0;

                        hdMaxColumns.SetValue(999);
                        GenerarGridDinamico();

                        string textoBuscado = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                        long? IdBuscado = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                        List<JsonObject> listaAtributos = ModGlobal.pages.Emplazamientos.GetDatos(sFiltro2, Request.QueryString["cliente"], true, textoBuscado, IdBuscado, sFiltro);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grdEmplazamientosAtributos.ColumnModel, listaAtributos, Response, "", GetGlobalResource("strAtributos"), Comun.DefaultLocale);
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

        #endregion

        #region STORES

        #region INVENTARIO ELEMENTOS EMPLAZAMIENTOS

        protected void storeEmplazamientosAtributos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int pageSize = Convert.ToInt32(cmbNumRegistros.Value);
                    int curPage = e.Page - 1;
                    int total;

                    string s = e.Parameters["filter"];

                    List<JsonObject> lista;
                    lista = ((ModGlobal.pages.Emplazamientos)Parent.Page).AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), pageSize, curPage, out total, e.Sort, s);

                    Store store = this.grdEmplazamientosAtributos.GetStore();
                    if (store != null && lista != null)
                    {
                        e.Total = total;
                        hdTotalCountGrid.SetValue(total);
                        store.DataSource = lista;
                        store.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        public void SetDataSourceGridAtributo(List<JsonObject> lista, int count)
        {
            try
            {
                Store store = this.grdEmplazamientosAtributos.GetStore();
                if (store != null && lista != null)
                {
                    store.SetData(lista);

                    PageProxy temp = (PageProxy)store.Proxy[0];
                    temp.Total = count;
                    hdTotalCountGrid.SetValue(count);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion

        #endregion

        #region DIRECT METHODS

        [DirectMethod()]
        public DirectResponse CargarGrid()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                UsuariosController cUsuarios = new UsuariosController();

                if (oUsuario != null)
                {
                    oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                }

                List<string> listaIgnore = new List<string>();

                GenerarGridDinamico();

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grdEmplazamientosAtributos, grdEmplazamientosAtributos.GetStore(), grdEmplazamientosAtributos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticasMod = new EstadisticasController();
                cEstadisticasMod.EscribeEstadisticaAccion(oUser.UsuarioID, oUser.ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

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
        public DirectResponse GetDatosBuscador()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                int total;
                List<JsonObject> lista;
                lista = ((ModGlobal.pages.Emplazamientos)Parent.Page).AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), -1, -1, out total, null, null);

                direct.Result = lista;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;

            return direct;
        }

        #endregion

        #region FUNCIONES
        public GridPanel ComponetGrid
        {
            get { return grdEmplazamientosAtributos; }

        }

        public void GenerarGridDinamico()
        {
            long lClid = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
            EmplazamientosAtributosConfiguracionController cAtributos = new EmplazamientosAtributosConfiguracionController();
            TiposDatosController cTipoDatos = new TiposDatosController();
            List<EmplazamientosAtributosConfiguracion> listaAtributos = cAtributos.GetAtributosFromCliente(lClid);
            long cols = 0;
            try
            {
                foreach (var atr in listaAtributos)
                {
                    ModelField modelField = new ModelField
                    {
                        Name = atr.NombreAtributo.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", "")
                    };
                    Data.TiposDatos oTipoDato;
                    oTipoDato = cTipoDatos.GetItem(atr.TipoDatoID);
                    EmplazamientosAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                    List<Data.Vw_EmplazamientosAtributosRolesRestringidos> listaRestriccionRoles = cRestriccionRoles.GetRolesRestringidosAtributo(atr.EmplazamientoAtributoConfiguracionID);
                    switch (oTipoDato.Codigo)
                    {
                        case "TEXTO":
                            modelField.Type = ModelFieldType.String;
                            break;
                        case "NUMERICO":
                            modelField.Type = ModelFieldType.Int;
                            break;
                        case "FECHA":
                            modelField.Type = ModelFieldType.Date;
                            break;
                        case "LISTA":
                            modelField.Type = ModelFieldType.String;
                            break;
                        case "LISTAMULTIPLE":
                            modelField.Type = ModelFieldType.String;
                            break;
                        case "BOOLEANO":
                            modelField.Type = ModelFieldType.Boolean;
                            break;
                        //case "ENTERO":

                        //    break;
                        //case "FLOTANTE":

                        //    break;
                        //case "MONEADA":

                        //    break;
                        //case "GEOPOSICION":

                        //    break;
                        case "TEXTAREA":
                            modelField.Type = ModelFieldType.String;
                            break;
                        default:
                            modelField.Type = ModelFieldType.String;
                            break;
                    }
                    storeEmplazamientosAtributos.ModelInstance.Fields.Add(modelField);
                    if (oTipoDato.Codigo == "FECHA")
                    {
                        DateColumn colDate = new DateColumn
                        {
                            ID = "col" + atr.EmplazamientoAtributoConfiguracionID,
                            Text = atr.NombreAtributo,
                            Format = Comun.FORMATO_FECHA,
                            DataIndex = atr.NombreAtributo.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""),
                            Hidden = true,
                            Flex = 1,
                        };

                        if (cols <= long.Parse(hdMaxColumns.Value.ToString()))
                        {
                            colDate.Hidden = false;
                            cols++;
                        }
                        if (listaRestriccionRoles != null)
                        {
                            RolesController cRoles = new RolesController();
                            List<Data.Roles> listaRoles = cRoles.GetRolesFromUsuario(((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            List<long> listaRolesIDs = new List<long>();
                            foreach (var item in listaRoles) { listaRolesIDs.Add(item.RolID); }
                            if (listaRestriccionRoles.Count > 0)
                            {
                                foreach (var oRestriccionRol in listaRestriccionRoles)
                                {
                                    if (oRestriccionRol.RolID != null)
                                    {
                                        if (listaRolesIDs.Contains(oRestriccionRol.RolID.Value))
                                        {
                                            switch (oRestriccionRol.Restriccion)
                                            {
                                                case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                    colDate.Hidden = true;
                                                    colDate.Fixed = true;
                                                    break;
                                                case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                                case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //mainConstainer.Hidden = true;
                                            //ControlAtributo.Disabled = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var cInvResDefe = cRestriccionRoles.GetDefault(atr.EmplazamientoAtributoConfiguracionID);
                                if (cInvResDefe != null)
                                {
                                    switch (cInvResDefe.Restriccion)
                                    {
                                        case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                            colDate.Hidden = true;
                                            colDate.Fixed = true;
                                            break;
                                        case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                            break;
                                        case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                        default:
                                            break;
                                    }

                                }
                            }
                        }


                        if (!colDate.Fixed)
                        {
                            grdEmplazamientosAtributos.InsertColumn(grdEmplazamientosAtributos.ColumnModel.Columns.Count, colDate);
                        }

                    }
                    else
                    {
                        Column col = new Column
                        {
                            ID = "col" + atr.EmplazamientoAtributoConfiguracionID,
                            Text = atr.NombreAtributo,
                            DataIndex = atr.NombreAtributo.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""),
                            Hidden = true,
                            Flex = 1,
                        };

                        if (cols <= long.Parse(hdMaxColumns.Value.ToString()))
                        {
                            col.Hidden = false;
                            cols++;
                        }

                        if (listaRestriccionRoles != null)
                        {
                            RolesController cRoles = new RolesController();
                            List<Data.Roles> listaRoles = cRoles.GetRolesFromUsuario(((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            List<long> listaRolesIDs = new List<long>();
                            foreach (var item in listaRoles) { listaRolesIDs.Add(item.RolID); }
                            if (listaRestriccionRoles.Count > 0)
                            {
                                foreach (var oRestriccionRol in listaRestriccionRoles)
                                {
                                    if (oRestriccionRol.RolID != null)
                                    {
                                        if (listaRolesIDs.Contains(oRestriccionRol.RolID.Value))
                                        {
                                            switch (oRestriccionRol.Restriccion)
                                            {
                                                case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                    col.Hidden = true;
                                                    col.Fixed = true;
                                                    break;
                                                case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                                case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            //mainConstainer.Hidden = true;
                                            //ControlAtributo.Disabled = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var cInvResDefe = cRestriccionRoles.GetDefault(atr.EmplazamientoAtributoConfiguracionID);
                                if (cInvResDefe != null)
                                {
                                    switch (cInvResDefe.Restriccion)
                                    {
                                        case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                            col.Hidden = true;
                                            col.Fixed = true;
                                            break;
                                        case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                            break;
                                        case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                        default:
                                            break;
                                    }

                                }
                            }
                        }

                        if (!col.Fixed)
                        {
                            grdEmplazamientosAtributos.InsertColumn(grdEmplazamientosAtributos.ColumnModel.Columns.Count, col);
                        }
                    }

                    #region FILTROS

                    List<string> listaIgnore = new List<string>();

                    Comun.CreateGridFilters(gridFiltersEmplazamientosAtributos, grdEmplazamientosAtributos.GetStore(), grdEmplazamientosAtributos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);

                    List<string> listaIgnoreContactos = new List<string>();
                    log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        #endregion
    }
}