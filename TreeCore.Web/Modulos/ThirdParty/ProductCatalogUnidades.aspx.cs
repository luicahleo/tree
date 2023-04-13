using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TreeCore.Clases;
using System.Reflection;

namespace TreeCore.ModGlobal
{
    public partial class ProductCatalogUnidades : TreeCore.Page.BasePageExtNet
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
                        List<Data.CoreUnidades> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.jsUnidades).ToString(), _Locale);
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
                sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar }},
                { "Post", new List<ComponentBase> { btnAnadir }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
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

        private List<Data.CoreUnidades> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.CoreUnidades> listaDatos;
            CoreProductCatalogUnidadesController cUnidades = new CoreProductCatalogUnidadesController();

            try
            {
                listaDatos = cUnidades.GetItemsWithExtNetFilterList<Data.CoreUnidades>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);

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
            CoreProductCatalogUnidadesController cUnidades = new CoreProductCatalogUnidadesController();
            InfoResponse oResponse;
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.CoreUnidades oDato;

                    oDato = cUnidades.GetItem(lS);

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

                    oResponse = cUnidades.Update(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cUnidades.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                        else
                        {
                            cUnidades.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cUnidades.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    Data.CoreUnidades oDato = new Data.CoreUnidades();

                    oDato.Nombre = txtNombre.Text;
                    oDato.Codigo = txtCodigo.Text;
                    oDato.Descripcion = txtDescripcion.Text;
                    oDato.Activo = true;

                    oResponse = cUnidades.Add(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cUnidades.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                        else
                        {
                            cUnidades.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cUnidades.DiscardChanges();
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
            CoreProductCatalogUnidadesController cUnidades = new CoreProductCatalogUnidadesController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.CoreUnidades oDato;
                oDato = cUnidades.GetItem(lS);
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
            CoreProductCatalogUnidadesController cUnidades = new CoreProductCatalogUnidadesController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.CoreUnidades oDato = cUnidades.GetItem(lID);
                oResponse = cUnidades.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cUnidades.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                    else
                    {
                        cUnidades.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cUnidades.DiscardChanges();
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
            CoreProductCatalogUnidadesController cUnidades = new CoreProductCatalogUnidadesController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.CoreUnidades oDato = cUnidades.GetItem(lID);
                oResponse = cUnidades.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cUnidades.SubmitChanges();

                    if (oResponse.Result)
                    {
                        storePrincipal.DataBind();
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                    }
                    else
                    {
                        cUnidades.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cUnidades.DiscardChanges();
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