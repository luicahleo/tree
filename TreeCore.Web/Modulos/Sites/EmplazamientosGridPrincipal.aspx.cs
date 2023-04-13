using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;
using TreeCore.Data;
using TreeCore.Page;
using TreeCore.Componentes;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosGridPrincipal : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        Data.Usuarios oUser;

        #region Gestión Página (Init/Load)

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                UsuariosController cUsuarios = new UsuariosController();
                if (oUsuario != null)
                {
                    oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                }

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { "Proyectos", "Contratos" };
                Comun.CreateGridFilters(gridFilters, storeEmplazamientos, gridEmplazamientos.ColumnModel, listaIgnore, _Locale);

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridEmplazamientos, storeEmplazamientos, gridEmplazamientos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

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
                            string sCliente = Request.QueryString["cliente"];
                            string sFiltro = Request.QueryString["filtro"];
                            string sFiltro2 = Request.QueryString["aux3"];
                            string sTextoBuscado = Request.QueryString["aux4"];
                            string sIdBuscado = Request.QueryString["aux5"];
                            sResultadoKPIid = Request.QueryString["aux6"];

                            bool bDescarga = true;

                            hdStringBuscador.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                            hdIDEmplazamientoBuscador.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                            List<JsonObject> lista;
                            EmplazamientosController cEmplazamientos = new EmplazamientosController();
                            string sVariablesExcluidas = "Proyectos, Contratos,  ";
                            int total = 0;
                            lista = cEmplazamientos.AplicarFiltroInterno(true, sFiltro2, -1, -1, out total, null, sFiltro, sCliente, hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, bDescarga, gridEmplazamientos.ColumnModel, sVariablesExcluidas);
                            
                            Comun.ExportacionDesdeListaNombreTask(gridEmplazamientos.ColumnModel, lista, Response, sVariablesExcluidas, GetGlobalResource(Comun.strEmplazamientos).ToString(), Comun.DefaultLocale);

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

                formAgregarEditarContactoEmplazamiento.ClienteID = ClienteID.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "EmplazamientosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnContactos }},
            { "Delete", new List<ComponentBase> { }}
        };

        }

        #endregion

        #region STORES

        #region Store Emplazamientos

        protected void storeEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();

                    #region KPI
                    if (hdnameIndiceID.Value != null && hdidsResultados.Value != null)
                    {
                        nameIndiceID = hdnameIndiceID.Value.ToString();
                        sResultadoKPIid = hdidsResultados.Value.ToString();
                    }
                    hdResultadoKPIid.SetValue(sResultadoKPIid);
                    #endregion

                    lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), pageSize, curPage, out total, e.Sort, s, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid , false, gridEmplazamientos.ColumnModel, "");

                    if (storeEmplazamientos != null && lista != null)
                    {
                        e.Total = total;
                        hdTotalCountGrid.SetValue(total);
                        storeEmplazamientos.DataSource = lista;
                        storeEmplazamientos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        #endregion

        #region CONTACTOS GLOBALES EMPLAZAMIENTOS VINCULADOS

        protected void storeContactosGlobalesEmplazamientosVinculados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_ContactosGlobalesEmplazamientosVinculados> listaContactos = ListaContactos();

                    if (listaContactos != null)
                    {
                        storeContactosGlobalesEmplazamientosVinculados.DataSource = listaContactos;
                        storeContactosGlobalesEmplazamientosVinculados.DataBind();

                        PageProxy proxy = (PageProxy)storeContactosGlobalesEmplazamientosVinculados.Proxy[0];
                        proxy.Total = listaContactos.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ContactosGlobalesEmplazamientosVinculados> ListaContactos()
        {
            List<Data.Vw_ContactosGlobalesEmplazamientosVinculados> listaEmplazamientos = null;
            ContactosGlobalesEmplazamientosVinculadosController cEmplazamientos = new ContactosGlobalesEmplazamientosVinculadosController();

            try
            {
                if (txtBuscarEmail.Text != "")
                {
                    listaEmplazamientos = cEmplazamientos.getContactosNoAsignadosByEmail(txtBuscarEmail.Text, long.Parse(hdEmplazamientoSeleccionado.Value.ToString()));
                }
                else if (searchTel.Text != "")
                {
                    listaEmplazamientos = cEmplazamientos.getContactosNoAsignadosByTelefono(searchTel.Text, long.Parse(hdEmplazamientoSeleccionado.Value.ToString()));
                }
                else if (hdEmplazamientoSeleccionado.Value != null && hdEmplazamientoSeleccionado.Value.ToString() != "")
                {
                    listaEmplazamientos = cEmplazamientos.GetListaContactosByEmplazamientoID(long.Parse(hdEmplazamientoSeleccionado.Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEmplazamientos = null;
            }

            return listaEmplazamientos;
        }

        #endregion

        #endregion

        #region Direct Methods

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
                lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), -1, -1, out total, null, null, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, gridEmplazamientos.ColumnModel, "");
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


        [DirectMethod]
        public DirectResponse MostrarEditarContacto(long lContactoGlobalID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                formAgregarEditarContactoEmplazamiento.MostrarEditarContacto(lContactoGlobalID);
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
        public DirectResponse AsignarEmplazamiento(long lContactoGlobalID, long lEmplazamientoIDAsignado)
        {
            DirectResponse direct = new DirectResponse();
            ContactosGlobalesEmplazamientosController cEmplazamientos = new ContactosGlobalesEmplazamientosController();

            try
            {
                long lEmplazamientoID = long.Parse(hdEmplazamientoSeleccionado.Value.ToString());

                if (lEmplazamientoIDAsignado != 0 && lEmplazamientoID == lEmplazamientoIDAsignado)
                {
                    ContactosGlobalesEmplazamientos oDato = cEmplazamientos.GetContactosByID(lEmplazamientoID, lContactoGlobalID);

                    if (cEmplazamientos.DeleteItem(oDato.ContactoGlobalEmplazamientoID))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }
                }
                else
                {

                    Data.ContactosGlobalesEmplazamientos oDato = new Data.ContactosGlobalesEmplazamientos();
                    oDato.ContactoGlobalID = lContactoGlobalID;
                    oDato.EmplazamientoID = lEmplazamientoID;

                    cEmplazamientos.AddItem(oDato);
                    log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
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

        #region LoadPrefijos
        [DirectMethod]
        public string LoadPrefijos()
        {
            DirectResponse direct = new DirectResponse();
            List<Ext.Net.MenuItem> items = new List<Ext.Net.MenuItem>();

            #region Controllers
            PaisesController cPaises = new PaisesController();
            #endregion

            try
            {
                items = cPaises.GetMenuItemsPrefijos(ClienteID.Value);
                items.ForEach(i =>
                {
                    Icon icono = (Icon)Enum.Parse(typeof(Icon), i.Icon.ToString());
                    ResourceManagerTreeCore.RegisterIcon(icono);
                });
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return ComponentLoader.ToConfig(items);
        }
        #endregion

        #endregion

    }
}