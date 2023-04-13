using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TreeCore.Clases;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class ProductCatalogFrecuencias : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

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
                        List<Data.CoreFrecuencias> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.jsFrecuencia).ToString(), _Locale);
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

        //protected override void OnPreRenderComplete(EventArgs e)
        //{
        //    base.OnPreRenderComplete(e);
        //    if (!IsPostBack && !RequestManager.IsAjaxRequest)
        //    {
        //        List<Data.Vw_Funcionalidades> listFun = ((List<Data.Vw_Funcionalidades>)(this.Session["LISTAFUNCIONALIDADES"]));

        //        btnAnadir.Hidden = true;
        //        btnEditar.Hidden = true;
        //        btnEliminar.Hidden = true;
        //        btnRefrescar.Hidden = false;
        //        btnActivar.Hidden = true;
        //        btnDescargar.Hidden = true;

        //        if (Comun.ComprobarFuncionalidadSoloLectura(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun))
        //        {
        //            btnAnadir.Hidden = true;
        //            btnEditar.Hidden = true;
        //            btnEliminar.Hidden = true;
        //            btnRefrescar.Hidden = false;
        //            btnActivar.Hidden = true;
        //            btnDescargar.Hidden = true;
        //        }
        //        else if (Comun.ComprobarFuncionalidadTotal(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun))
        //        {
        //            btnAnadir.Hidden = false;
        //            btnEditar.Hidden = false;
        //            btnEliminar.Hidden = false;
        //            btnRefrescar.Hidden = false;
        //            btnActivar.Hidden = false;
        //            btnDescargar.Hidden = false;
        //        }
        //        if (Comun.ComprobarFuncionalidadDescarga(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun))
        //        {
        //            btnDescargar.Hidden = false;
        //        }
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_ProductCatalogFrecuencias_Lectura))
            {

            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.GLO_ProductCatalogFrecuencias_Total))
            {

            }
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

        private List<Data.CoreFrecuencias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.CoreFrecuencias> listaDatos;
            CoreProductCatalogFrecuenciasController cFrecuencias = new CoreProductCatalogFrecuenciasController();

            try
            {
                listaDatos = cFrecuencias.GetItemsWithExtNetFilterList<Data.CoreFrecuencias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);

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
            CoreProductCatalogFrecuenciasController cFrecuencias = new CoreProductCatalogFrecuenciasController();
            long lCliID = 0;
            InfoResponse oResponse;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.CoreFrecuencias oDato;

                    oDato = cFrecuencias.GetItem(lS);

                    if (oDato.Nombre == txtNombre.Text)
                    {
                        oDato.Nombre = txtNombre.Text;
                        if (oDato.Codigo == txtCodigo.Text)
                        {
                            oDato.Codigo = txtCodigo.Text;
                            oDato.Descripcion = txtDescripcion.Text;
                            oDato.Activo = true;
                        }
                        else
                        {
                            oDato.Codigo = txtCodigo.Text;
                            oDato.Descripcion = txtDescripcion.Text;
                            oDato.Activo = true;
                        }
                    }
                    else
                    {
                        oDato.Nombre = txtNombre.Text;

                        if (oDato.Codigo == txtCodigo.Text)
                        {
                            oDato.Codigo = txtCodigo.Text;
                            oDato.Descripcion = txtDescripcion.Text;
                            oDato.Activo = true;
                        }
                        else
                        {
                            oDato.Codigo = txtCodigo.Text;
                            oDato.Descripcion = txtDescripcion.Text;
                            oDato.Activo = true;
                        }
                    }

                    oResponse = cFrecuencias.Update(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cFrecuencias.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                        else
                        {
                            cFrecuencias.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cFrecuencias.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());
                    Data.CoreFrecuencias oDato = new Data.CoreFrecuencias();

                    oDato.Nombre = txtNombre.Text;
                    oDato.Codigo = txtCodigo.Text;
                    oDato.Descripcion = txtDescripcion.Text;
                    oDato.Activo = true;

                    oResponse = cFrecuencias.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cFrecuencias.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                        else
                        {
                            cFrecuencias.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cFrecuencias.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogFrecuenciasController cFrecuencias = new CoreProductCatalogFrecuenciasController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CoreFrecuencias oDato;
                oDato = cFrecuencias.GetItem(lS);
                txtNombre.Text = oDato.Nombre;
                txtCodigo.Text = oDato.Codigo;
                txtDescripcion.Text = oDato.Descripcion;

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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogFrecuenciasController cFrecuencias = new CoreProductCatalogFrecuenciasController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.CoreFrecuencias oDato = cFrecuencias.GetItem(lID);
                oResponse = cFrecuencias.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cFrecuencias.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                    else
                    {
                        cFrecuencias.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cFrecuencias.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogFrecuenciasController cFrecuencias = new CoreProductCatalogFrecuenciasController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.CoreFrecuencias oDato = cFrecuencias.GetItem(lID);
                oResponse = cFrecuencias.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cFrecuencias.SubmitChanges();

                    if (oResponse.Result)
                    {
                        storePrincipal.DataBind();
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                    }
                    else
                    {
                        cFrecuencias.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cFrecuencias.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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
    }
}