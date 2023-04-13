using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaNegocio;
using System.Reflection;
using Ext.Net;
using log4net;

namespace TreeCore.ModInventario.pages
{
    public partial class InventarioCategoryView : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region Control Página (Init/Load)

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                if (Request["VistaPlantilla"] != null && Request["VistaPlantilla"] != "")
                {
                    hdVistaPlantilla.SetValue(Request["VistaPlantilla"]);
                    TreePanel2.Title = GetGlobalResource("strTituloInventario") + " " + GetGlobalResource("strTituloPlantillas");
                }
                if (Request["EmplazamientoID"] != null && Request["EmplazamientoID"] != "" && Request["EmplazamientoID"] != "0")
                {
                    hdEmplazamientoID.SetValue(Request["EmplazamientoID"]);
                }
                else
                {
                    hdEmplazamientoID.SetValue(0);
                    tbEmplazamientosTipo.Hidden = false;
                }
                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    tbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                    tbClientes.Hidden = true;
                }
                //CargarMenu();

                //Filtro de resultados KPI
                if (Request.QueryString[Comun.PARAM_IDS_RESULTADOS] != null && Request.QueryString[Comun.PARAM_NAME_INDICE_ID] != null)
                {
                    hdIdsResultados.SetValue(Request[Comun.PARAM_IDS_RESULTADOS]);
                    hdNameIndiceID.SetValue(Request[Comun.PARAM_NAME_INDICE_ID]);
                }
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
        }

        #endregion

        #region STORES

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
                    string codTit = Util.ExceptionHandler(ex);
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

        #region EMPLAZAMIENTOS TIPOS

        protected void storeTipoEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
                try
                {

                    var lista = cEmplazamientosTipos.GetEmplazamientosTiposActivos(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeTipoEmplazamientos.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region CATEGORIAS

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                InventarioElementosController cElementos = new InventarioElementosController();
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                Data.Emplazamientos oEmplazamiento;
                List<Data.InventarioCategorias> listaDatos = new List<Data.InventarioCategorias>();
                List<JsonObject> listaJson;
                JsonObject oJson;
                try
                {
                    long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                    oEmplazamiento = cEmplazamientos.GetItem(EmplazamientoID);

                    if (oEmplazamiento != null)
                    {
                        if (ClienteID.HasValue)
                        {
                            listaDatos = cCategorias.GetInventarioCategoriasByTipoEmplazamiento(oEmplazamiento.EmplazamientoTipoID.ToString(), (long)ClienteID);
                        }
                        else
                        {
                            listaDatos = cCategorias.GetInventarioCategoriasByTipoEmplazamiento(oEmplazamiento.EmplazamientoTipoID.ToString(), long.Parse(cmbClientes.SelectedItem.Value));
                        }
                    }
                    else
                    {
                        if (ClienteID.HasValue)
                        {
                            listaDatos = cCategorias.GetInventarioCategoriasByTipoEmplazamiento((cmbTipoEmplazamientos.SelectedItem.Value == null) ? "0" : cmbTipoEmplazamientos.SelectedItem.Value.ToString(), (long)ClienteID);
                        }
                        else
                        {
                            listaDatos = cCategorias.GetInventarioCategoriasByTipoEmplazamiento((cmbTipoEmplazamientos.SelectedItem.Value == null) ? "0" : cmbTipoEmplazamientos.SelectedItem.Value.ToString(), long.Parse(cmbClientes.SelectedItem.Value));
                        }
                    }
                    if (listaDatos != null)
                    {


                        //Filtro resultados KPI
                        if (listaDatos != null && listIdsResultadosKPI != null)
                        {
                            if (nameIndiceID == "InventarioCategoriaID")
                            {
                                listaDatos = cCategorias.FiltroListaPrincipalByIDs(listaDatos.Cast<object>().ToList(), listIdsResultadosKPI, nameIndiceID).Cast<Data.InventarioCategorias>().ToList();
                            }
                            else if (nameIndiceID == "InventarioElementoID")
                            {
                                List<Data.InventarioCategorias> tempCategorias = cCategorias.GetByElementos(listIdsResultadosKPI);
                                if (tempCategorias != null)
                                {
                                    listaDatos = tempCategorias;
                                }
                            }
                        }

                        listaJson = new List<JsonObject>();
                        foreach (var item in listaDatos)
                        {
                            oJson = new JsonObject();
                            oJson.Add("InventarioCategoriaID", item.InventarioCategoriaID);
                            oJson.Add("InventarioCategoria", item.InventarioCategoria);
                            oJson.Add("Activo", item.Activo);
                            //oJson.Add("NumElementos", cElementos.GetGridDinamicoInventariov2(item.InventarioCategoriaID, EmplazamientoID, (hdVistaPlantilla.Value != null && hdVistaPlantilla.Value.ToString() != "")).Count);
                            oJson.Add("Icono", Comun.rutaIconoWebInventario(item.Icono));
                            listaJson.Add(oJson);
                        }
                        storeCategorias.DataSource = listaJson;
                        storeCategorias.DataBind();
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

        #region Datos Grid My filters

        private List<Job> Jobs
        {
            get
            {
                List<Job> jobs = new List<Job>();

                for (int i = 1; i <= 50; i++)
                {
                    jobs.Add(new Job(
                                i,
                                "Task" + i.ToString(),
                                DateTime.Today.AddDays(i),
                                DateTime.Today.AddDays(i + i),
                                (i % 3 == 0)));
                }

                return jobs;
            }
        }

        public class Job
        {
            public Job(int id, string name, DateTime start, DateTime end, bool completed)
            {
                this.ID = id;
                this.Name = name;
                this.Start = start;
                this.End = end;
                this.Completed = completed;
            }

            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public bool Completed { get; set; }
        }

        #endregion

        #region Methods

        [DirectMethod()]
        public DirectResponse CargarStores()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            JsonObject listas, lista, jsonAux;

            try
            {
                direct.Success = true;
                listas = new JsonObject();
                lista = new JsonObject();

                long cliID = long.Parse(hdCliID.Value.ToString());
                long EmplazamientoID = long.Parse(hdEmplazamientoID.Value.ToString());
                
                if (EmplazamientoID == 0)
                {
                    #region EmplazamientoTipo

                    EmplazamientosTiposController cTipos = new EmplazamientosTiposController();

                    foreach (var oDato in cTipos.GetEmplazamientosTiposActivos(cliID))
                    {
                        jsonAux = new JsonObject();
                        jsonAux.Add("EmplazamientoTipoID", oDato.EmplazamientoTipoID);
                        jsonAux.Add("Tipo", oDato.Tipo);
                        lista.Add(oDato.EmplazamientoTipoID.ToString(), jsonAux);
                    }
                    listas.Add("storeTipoEmplazamientos", lista);

                    #endregion

                    #region Categorias Inventario

                    lista = new JsonObject();

                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();

                    foreach (var oDato in cCategorias.GetInventarioCategoriasByTipoEmplazamiento("0", cliID))
                    {
                        jsonAux = new JsonObject();
                        jsonAux.Add("InventarioCategoriaID", oDato.InventarioCategoriaID);
                        jsonAux.Add("InventarioCategoria", oDato.InventarioCategoria);
                        jsonAux.Add("Icono", Comun.rutaIconoWebInventario(oDato.Icono));
                        lista.Add(oDato.InventarioCategoriaID.ToString(), jsonAux);
                    }
                    listas.Add("storeCategorias", lista);

                    #endregion
                }
                else
                {
                    Data.Emplazamientos oEmplazamiento = cEmplazamientos.GetItem(EmplazamientoID);
                    if(oEmplazamiento != null)
                    {
                        #region Categorias Inventario

                        lista = new JsonObject();

                        InventarioCategoriasController cCategorias = new InventarioCategoriasController();

                        foreach (var oDato in cCategorias.GetInventarioCategoriasByTipoEmplazamiento(oEmplazamiento.EmplazamientoTipoID.ToString(), cliID))
                        {
                            jsonAux = new JsonObject();
                            jsonAux.Add("InventarioCategoriaID", oDato.InventarioCategoriaID);
                            jsonAux.Add("InventarioCategoria", oDato.InventarioCategoria);
                            jsonAux.Add("Icono", Comun.rutaIconoWebInventario(oDato.Icono));
                            lista.Add(oDato.InventarioCategoriaID.ToString(), jsonAux);
                        }

                        listas.Add("storeCategorias", lista);

                        #endregion
                    }

                }

                jsonAux = new JsonObject();
                jsonAux.Add("listas", listas);
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

        protected void AddTab(object sender, DirectEventArgs e)
        {

            Ext.Net.Panel panel = new Ext.Net.Panel
            {
                Title = "New Tab",
                Closable = true,
                Layout = "Fit",
                Items = {
                      new UserControlLoader{
                          Path="../../Componentes/GridInventarioDinamico.ascx"
                      }
                }
            };

            InventoryTabPanel.Add(panel);
            panel.Render();

            InventoryTabPanel.SetLastTabAsActive();
        }

        #endregion


    }
}