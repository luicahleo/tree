using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Reflection;
using TreeCore.Componentes;
using TreeCore.Data;
using TreeCore.Page;

namespace TreeCore.ModGlobal
{
    public partial class ProductCatalogServiciosContenedor : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region GESTION DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
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
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Usuario.ClienteID != null)
            {
                hdCliID.Value = Usuario.ClienteID.ToString();
            }
        }

        #endregion

        #region STORES

        #region SERVICIOS ASIGNADOS

        protected void storeCoreProductCatalogServiciosAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_CoreProductCatalogServiciosAsignados> listaAsign = null;
                    CoreProductCatalogServiciosAsignadosController cAsigns = new CoreProductCatalogServiciosAsignadosController();

                    if (hdServicioPadreID.Value.ToString() != "")
                    {
                        listaAsign = cAsigns.getItemsByServicioID(long.Parse(hdServicioPadreID.Value.ToString()));
                    }

                    if (listaAsign != null)
                    {
                        storeCoreProductCatalogServiciosAsignados.DataSource = listaAsign;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #endregion

        #region FUNCTIONS

        [DirectMethod()]
        public DirectResponse RecargarPanelLateral(long lServicio)
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();
            JsonObject oDato = new JsonObject();
            Vw_CoreProductCatalogServicios oVista;

            try
            {
                oVista = cServicios.GetItem<Vw_CoreProductCatalogServicios>(lServicio);

                if (oVista != null)
                {
                    lblServicios.Text = GetGlobalResource("strServicio") + ":  " + oVista.Nombre;
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
        public DirectResponse PanelLateralServicios(string sPanel)
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();
            CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();
            CoreProductCatalogServiciosAsignadosController cAsignados = new CoreProductCatalogServiciosAsignadosController();
            var lista = "";
            List<Object> listaObjetos = new List<Object>();

            try
            {
                switch (sPanel)
                {
                    //case "pnPrecios":

                    //    List<Data.Vw_CoreProductCatalogServiciosAsignados> listaServiciosAsignados = null;

                    //    listaServiciosAsignados = cAsignados.getItemsByServicioID(long.Parse(hdServicioPadreID.Value.ToString()));

                    //    if (listaServiciosAsignados != null)
                    //    {
                    //        foreach(Data.Vw_CoreProductCatalogServiciosAsignados oDato in listaServiciosAsignados)
                    //        {
                    //            listaObjetos.Add(oDato);
                    //        }
                    //    }

                    //    break;

                    case "pnClausulas":

                        Data.Vw_CoreProductCatalogs oAsign = null;

                        if (hdServicioPadreID.Value.ToString() != "")
                        {
                            oAsign = cCatalogos.getVistaByID(long.Parse(hdServicioPadreID.Value.ToString()));
                        }

                        if (oAsign != null)
                        {
                            listaObjetos.Add(oAsign);
                        }

                        break;

                    case "pnClausulasServicios":

                        List<Data.Vw_CoreProductCatalogServiciosAsignados> listaServiciosAsig = null;

                        listaServiciosAsig = cAsignados.getItemsByServicioID(long.Parse(hdServicioPadreID.Value.ToString()));

                        if (listaServiciosAsig != null)
                        {
                            foreach (Data.Vw_CoreProductCatalogServiciosAsignados oDato in listaServiciosAsig)
                            {
                                listaObjetos.Add(oDato);
                            }
                        }

                        break;

                    case "pnServicios":

                        Data.Vw_CoreProductCatalogServicios oServ = null;

                        if (hdServicioPadreID.Value.ToString() != "")
                        {
                            oServ = cServicios.GetItem<Vw_CoreProductCatalogServicios>(long.Parse(hdServicioPadreID.Value.ToString()));
                        }

                        if (oServ != null)
                        {
                            listaObjetos.Add(oServ);
                        }

                        break;
                    case "pnLink":

                        //List<Data.Vw_CoreProductCatalogServiciosObjetosNegocioTipos> listaServObj = null;

                        //if (hdServicioPadreID.Value.ToString() != "")
                        //{
                        //    listaServObj = cServObj.getVistaByServicioID(long.Parse(hdServicioPadreID.Value.ToString()));
                        //}

                        //if (listaServObj != null)
                        //{
                        //    foreach (Data.Vw_CoreProductCatalogServiciosObjetosNegocioTipos oValor in listaServObj)
                        //    {
                        //        string sNombre = "";

                        //        if (oValor.NombreObjeto != null && oValor.NombreObjeto.Contains(" "))
                        //        {
                        //            sNombre = oValor.NombreObjeto.Split(' ')[1];
                        //        }
                        //        else
                        //        {
                        //            sNombre = GetGlobalResource(oValor.NombreObjeto);
                        //        }

                        //        JsonObject oJson = new JsonObject();
                        //        oJson.Add("Nombre", sNombre);
                        //        oJson.Add("CoreProductCatalogServicioObjetoNegocioTipoID", oValor.CoreProductCatalogServicioObjetoNegocioTipoID);
                        //        listaObjetos.Add(oJson);
                        //    }
                        //}

                        break;
                }

                lista = Newtonsoft.Json.JsonConvert.SerializeObject(listaObjetos);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = lista;

            return direct;
        }

        #endregion

        #region DISEÑO
        [DirectMethod]
        public void VwUpdater()
        {
            this.CenterPanelMain.Update();
        }

        protected void ShowHidePnAsideR(object sender, DirectEventArgs e)
        {
            pnAsideR.AnimCollapse = true;
            pnAsideR.ToggleCollapse();
        }

        protected void ShowHidePnAsideRColumnas(object sender, DirectEventArgs e)
        {
            btnCollapseAsRClosed.Show();
        }

        [DirectMethod]
        public void DirectShowHidePnAsideR()
        {
            btnCollapseAsRClosed.Show();
        }
        #endregion
    }
}