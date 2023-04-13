﻿using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using TreeCore.Page;
using TreeCore.Data;
namespace TreeCore.Componentes
{
    public partial class GridEmplazamientosDocumentos : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Data.Usuarios oUser;
        public List<long> listaFuncionalidades = new List<long>();

        public string IDComponente
        {
            get { return this.hdIDComponente.Value.ToString(); }
            set { this.hdIDComponente.SetValue(value); }
        }

        public long ClienteID
        {
            get { return long.Parse(hdClienteID.Value.ToString()); }
            set { hdClienteID.SetValue(value); }
        }


        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {
            this.IDComponente = this.ID;
            
            hdClienteID = (Hidden)X.GetCmp("hdCliID");

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

                        string textoBuscado = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                        long? IdBuscado = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                        List<Vw_DocumentosEmplazamientos> listaDocumento = ModGlobal.pages.Emplazamientos.GetDatosDocumentos(sFiltro2, Request.QueryString["cliente"], true, textoBuscado, IdBuscado);
                        listaDocumento = Clases.LinqEngine.PagingItemsListWithExtNetFilter(listaDocumento, sFiltro, "", sOrden, sDir, 0, 0, ref iCount);
                        listaDocumento = listaDocumento.FindAll(doc => doc.UltimaVersion==true);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grdDocumentosEmplazamientos.ColumnModel, listaDocumento, Response, "", GetGlobalResource(Comun.strDocumentos).ToString(), Comun.DefaultLocale);
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

        #region DOCUMENTOS ELEMENTOS EMPLAZAMIENTOS

        protected void storeDocumentosEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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

                    Store store = this.grdDocumentosEmplazamientos.GetStore();
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
        public void SetDataSourceGridDocumento(List<JsonObject> lista, int count)
        {
            try
            {
                Store store = this.grdDocumentosEmplazamientos.GetStore();
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
        private List<Data.Vw_DocumentosEmplazamientos> ListaDocumentoEmplazamiento(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_DocumentosEmplazamientos> listaDocumento;
            DocumentosEmplazamientosController cDocumento = new DocumentosEmplazamientosController();

            try
            {

                listaDocumento = cDocumento.GetItemsWithExtNetFilterList<Data.Vw_DocumentosEmplazamientos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true && UltimaVersion == true");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDocumento = null;
            }

            return listaDocumento;
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

                #region FILTROS
                List<string> listaIgnore = new List<string>();

                Comun.CreateGridFilters(gridFiltersDocumentos, grdDocumentosEmplazamientos.GetStore(), grdDocumentosEmplazamientos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);

                List<string> listaIgnoreContactos = new List<string>();
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grdDocumentosEmplazamientos, grdDocumentosEmplazamientos.GetStore(), grdDocumentosEmplazamientos.ColumnModel, listaIgnore, ((BasePageExtNet)this.Page)._Locale);
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
            get { return grdDocumentosEmplazamientos; }

        }
        #endregion
    }
}