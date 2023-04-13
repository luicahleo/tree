using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class ServiciosFrecuencias : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);


                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));


                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
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
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];
                string sEntidad = Request.QueryString["aux3"].ToString();

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.CoreServiciosFrecuencias> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.jsServiciosFrecuencias).ToString(), _Locale);
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
                storePrincipal.Reload();
                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion



        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.CoreServiciosFrecuencias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.CoreServiciosFrecuencias> listaDatos;
            CoreServiciosFrecuenciasController CCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();

            try
            {
                listaDatos = CCoreServiciosFrecuencias.GetItemsWithExtNetFilterList<Data.CoreServiciosFrecuencias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);

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


        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            CoreServiciosFrecuenciasController CCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            long lCliID = 0;

            try
            {

                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.CoreServiciosFrecuencias oDato;

                    oDato = CCoreServiciosFrecuencias.GetItem(lS);

                    //if (oDato.Nombre == conProgramador.Nombre)
                    //{
                    //    oDato.Nombre = conProgramador.Nombre;

                    //}
                    //else
                    //{

                    //    if (CCoreServiciosFrecuencias.RegistroDuplicado(conProgramador.Nombre))
                    //    {
                    //        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                    //        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    //    }
                    //    else
                    //    {
                    //        oDato.Nombre = conProgramador.Nombre;
                    //        oDato.CronFormat = conProgramador.CronFormat;
                    //        oDato.FechaInicio = conProgramador.FechaInicio;

                    //        if (conProgramador.FechaFin != null)
                    //        {
                    //            oDato.FechaFin = conProgramador.FechaFin;
                    //        }

                    //        if (CCoreServiciosFrecuencias.UpdateItem(oDato))
                    //        {
                    //            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                    //        }
                    //    }
                    //}
                }
                //else
                //{
                //    lCliID = long.Parse(lCliID.ToString());

                //    if (CCoreServiciosFrecuencias.RegistroDuplicado(conProgramador.Nombre))
                //    {
                //        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                //        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                //    }
                //    else
                //    {

                //        Data.CoreServiciosFrecuencias Data = new Data.CoreServiciosFrecuencias();

                //        Data.Nombre = conProgramador.Nombre;
                //        Data.CronFormat = conProgramador.CronFormat;
                //        Data.FechaInicio = conProgramador.FechaInicio;

                //        if (conProgramador.FechaFin != null && conProgramador.FechaFin != DateTime.MinValue)
                //        {
                //            Data.FechaFin = conProgramador.FechaFin;
                //        }

                //        Data.Activo = true;

                //        if (CCoreServiciosFrecuencias.AddItem(Data) != null)
                //        {
                //            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                //        }

                //    }
                //}

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
            CoreServiciosFrecuenciasController CCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CoreServiciosFrecuencias oDato;
                oDato = CCoreServiciosFrecuencias.GetItem(lS);

                //conProgramador.Nombre = oDato.Nombre;
                conProgramador.CronFormat = oDato.CronFormat;
                conProgramador.FechaInicio = (DateTime)oDato.FechaInicio;

                if (oDato.FechaFin != null)
                {
                    conProgramador.FechaFin = (DateTime)oDato.FechaFin;
                }

                winGestion.Show();
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosTiposController CCoreProductCatalogServiciosTipos = new CoreProductCatalogServiciosTiposController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.CoreProductCatalogServiciosTipos oDato;
                long lCliID = long.Parse(hdCliID.Value.ToString());

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = CCoreProductCatalogServiciosTipos.GetDefault();

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.CoreProductCatalogServicioTipoID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            CCoreProductCatalogServiciosTipos.UpdateItem(oDato);
                        }

                        oDato = CCoreProductCatalogServiciosTipos.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        CCoreProductCatalogServiciosTipos.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = CCoreProductCatalogServiciosTipos.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    CCoreProductCatalogServiciosTipos.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

                CCoreProductCatalogServiciosTipos = null;
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
            CoreServiciosFrecuenciasController CCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CCoreServiciosFrecuencias.DeleteItem(lID))
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosTiposController CCoreProductCatalogServiciosTipos = new CoreProductCatalogServiciosTiposController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CoreProductCatalogServiciosTipos oDato;
                oDato = CCoreProductCatalogServiciosTipos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CCoreProductCatalogServiciosTipos.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
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

            CCoreProductCatalogServiciosTipos = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion
    }
}