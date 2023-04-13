using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Data;
using TreeCore.Page;
using System.Linq;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosGridAtributosDinamicos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Data.Usuarios oUser;
        public List<long> listaFuncionalidades = new List<long>();

        #region Gestion Pagina

        private void Page_Init(object sender, EventArgs e)
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            UsuariosController cUsuarios = new UsuariosController();

            if (oUsuario != null)
            {
                oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
            }

            hdMaxColumns.SetValue(7);
            ResourceManagerOperaciones(ResourceManagerTreeCore);

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

                        DataSorter[] ord = new DataSorter[1];
                        ord[0] = new DataSorter
                        {
                            Property = sOrden,
                            Direction = ((sDir == "ASC") ? SortDirection.ASC : SortDirection.DESC)
                        };

                        string sCliente = Request.QueryString["cliente"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sFiltro2 = Request.QueryString["aux3"];
                        string sTextoBuscado = Request.QueryString["aux4"];
                        string sIdBuscado = Request.QueryString["aux5"];
                        sResultadoKPIid = Request.QueryString["aux6"];

                        hdStringBuscador.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                        hdIDEmplazamientoBuscador.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();
                        hdCliID.SetValue(sCliente);
                        hdMaxColumns.SetValue(999);

                        GenerarGridDinamico();

                        List<JsonObject> lista;
                        List<string> listaVacia = new List<string>();
                        EmplazamientosController cEmplazamientos = new EmplazamientosController();
                        string sVariablesExcluidas = "EmplazamientoID";
                        int total = 0;
                        lista = cEmplazamientos.AplicarFiltroInterno(true, sFiltro2, -1, -1, out total, ord, sFiltro, sCliente, hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, true, grdEmplazamientosAtributos.ColumnModel, sVariablesExcluidas);
                        Comun.ExportacionDesdeListaNombreTask(grdEmplazamientosAtributos.ColumnModel, lista, Response, sVariablesExcluidas, GetGlobalResource(Comun.strEmplazamientos).ToString(), Comun.DefaultLocale);

                        #region ESTADISTICAS
                        try
                        {
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
            sPagina = "EmplazamientosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };

        }

        #endregion

        #region STORES

        #region ATRIBUTOS DINAMICOS EMPLAZAMIENTOS

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
                    List<string> listaVacia = new List<string>();
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();

                    #region KPI
                    if (hdnameIndiceID.Value != null && hdidsResultados.Value != null)
                    {
                        nameIndiceID = hdnameIndiceID.Value.ToString();
                        sResultadoKPIid = hdidsResultados.Value.ToString();
                    }
                    hdResultadoKPIid.SetValue(sResultadoKPIid);
                    #endregion

                    lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), pageSize, curPage, out total, e.Sort, s, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, grdEmplazamientosAtributos.ColumnModel, "");

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

        #endregion

        #endregion

        #region Direct Methods

        [DirectMethod()]
        public DirectResponse GenerarGridDinamico()
        {
            DirectResponse direct = new DirectResponse();
            long lClid = (long)((TreeCore.Data.Usuarios)this.Session["USUARIO"]).ClienteID;
            EmplazamientosAtributosConfiguracionController cAtributos = new EmplazamientosAtributosConfiguracionController();
            TiposDatosController cTipoDatos = new TiposDatosController();
            List<Data.EmplazamientosAtributosConfiguracion> listaAtributos = cAtributos.GetAtributosFromCliente(lClid);
            long cols = 0;
            try
            {
                EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
                var listaAtrVisibles = cAtributosConf.GetAtributosVisibles(lClid, ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID);
                foreach (var atr in listaAtributos)
                {
                    ModelField modelField = new ModelField
                    {
                        Name = "Atr" + atr.EmplazamientoAtributoConfiguracionID
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
                            Format = "dd/m/Y",
                            DataIndex = "Atr"+atr.EmplazamientoAtributoConfiguracionID,
                            Filterable = false,
                            Hidden = true,
                            MinWidth = 100,
                            Flex = 1,
                        };

                        if (cols <= long.Parse(hdMaxColumns.Value.ToString()))
                        {
                            colDate.Hidden = false;
                            cols++;
                        }

                        if (!listaAtrVisibles.Select(c => c.EmplazamientoAtributoConfiguracionID).Contains(atr.EmplazamientoAtributoConfiguracionID))
                        {
                            colDate.Hidden = true;
                            colDate.Fixed = true;
                        }                        
                        if (!colDate.Fixed)
                        {
                            grdEmplazamientosAtributos.ColumnModel.Columns.Add(colDate);
                        }

                    }
                    else
                    {
                        Column col = new Column
                        {
                            ID = "col" + atr.EmplazamientoAtributoConfiguracionID,
                            Text = atr.NombreAtributo,
                            DataIndex = "Atr" + atr.EmplazamientoAtributoConfiguracionID,
                            Hidden = true,
                            Filterable = false,
                            MinWidth = 110,
                            Flex = 1,
                        };

                        if (cols <= long.Parse(hdMaxColumns.Value.ToString()))
                        {
                            col.Hidden = false;
                            cols++;
                        }
                        if (!listaAtrVisibles.Select(c => c.EmplazamientoAtributoConfiguracionID).Contains(atr.EmplazamientoAtributoConfiguracionID))
                        {
                            col.Hidden = true;
                            col.Fixed = true;
                        }
                        if (!col.Fixed)
                        {
                            grdEmplazamientosAtributos.ColumnModel.Columns.Add(col);
                        }
                    }

                    #region SELECCION COLUMNAS
                    List<string> listaIgnore = new List<string>();
                    Comun.Seleccionable(grdEmplazamientosAtributos, grdEmplazamientosAtributos.GetStore(), grdEmplazamientosAtributos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);
                    log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                    #endregion

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
        public DirectResponse GetDatosBuscador()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();
                lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), -1, -1, out total, null, null, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, grdEmplazamientosAtributos.ColumnModel, "");
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

    }
}