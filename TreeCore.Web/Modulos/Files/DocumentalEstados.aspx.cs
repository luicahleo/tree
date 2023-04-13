using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Data.SqlClient;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.ModDocumental
{
    public partial class DocumentalEstados : TreeCore.Page.BasePageExtNet
    {
        public ILog log = LogManager.GetLogger("");
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
                    cmbClientes.Hidden = false;
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

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.DocumentosEstados> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strDocumentosEstados"), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.DOCUMENTAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto }},
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.DocumentosEstados> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.DocumentosEstados> listaDatos;
            DocumentosEstadosController CDocumentosEstados = new DocumentosEstadosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CDocumentosEstados.GetItemsWithExtNetFilterList<Data.DocumentosEstados>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    listaDatos = null;
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();

            DocumentosEstadosController cDocEstados = new DocumentosEstadosController();

            try

            {
                if (!bAgregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.DocumentosEstados dato = new Data.DocumentosEstados();
                    dato = cDocEstados.GetItem(S);
                    dato.Codigo = txtCodigo.Text;
                    dato.Nombre = txtNombre.Text;


                    InfoResponse infoResponse = cDocEstados.Update(dato);
                    if (infoResponse.Result)
                    {
                        cDocEstados.SubmitChanges();
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        cDocEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = infoResponse.Description;
                    }
                }
                else
                {
                    long lCliID = long.Parse(hdCliID.Value.ToString());

                    Data.DocumentosEstados dato = new Data.DocumentosEstados()
                    {
                        Codigo = txtCodigo.Text,
                        Nombre = txtNombre.Text,
                        Activo = true,
                        ClienteID = lCliID
                    };


                    InfoResponse infResponse = cDocEstados.Add(dato);
                    if (infResponse.Result)
                    {
                        cDocEstados.SubmitChanges();
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        cDocEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = infResponse.Description;
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

            DocumentosEstadosController cSoporte = new DocumentosEstadosController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DocumentosEstados dato = new Data.DocumentosEstados();
                dato = cSoporte.GetItem(S);

                txtCodigo.Text = dato.Codigo;
                txtNombre.Text = dato.Nombre;

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
            DocumentosEstadosController CDocumentosEstados = new DocumentosEstadosController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                DocumentosEstados docEstado = CDocumentosEstados.GetItem(lID);

                InfoResponse infoResponse = CDocumentosEstados.Delete(docEstado);

                if (infoResponse.Result)
                {
                    CDocumentosEstados.SubmitChanges();
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }
                else
                {
                    CDocumentosEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = infoResponse.Description;
                }

                CDocumentosEstados = null;
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
            DocumentosEstadosController cController = new DocumentosEstadosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DocumentosEstados oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                InfoResponse response = cController.Update(oDato);
                if (response.Result)
                {
                    cController.SubmitChanges();
                    storePrincipal.DataBind();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = response.Description;
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosEstadosController CDocumentosEstados = new DocumentosEstadosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                InfoResponse infoREsponse = CDocumentosEstados.AsignarPorDefecto(lID, ClienteID.Value);

                if (infoREsponse.Result)
                {
                    CDocumentosEstados.SubmitChanges();
                    direct.Success = true;
                    direct.Result = infoREsponse.Description;
                    log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                }
                else
                {
                    CDocumentosEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = infoREsponse.Description;
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

        #endregion


    }
}