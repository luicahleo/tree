using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using TreeCore.CapaNegocio.Global.Administracion;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class Clientes : TreeCore.Page.BasePageExtNet
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
                Comun.CreateGridFilters(gridFiltersProyectosTipos, storeProyectosTipos, gridProyectosTipos.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersProyectoTipoLibre, storeProyectosTiposLibres, gridProyectosTiposLibres.ColumnModel, listaIgnore, _Locale);
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
                    //cmbClientes.Hidden = false;
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
                        List<Data.Vw_Clientes> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto, btnAnonimizar }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #endregion

        #region STORES

        #region STORE PRINCIPAL

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
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Vw_Clientes> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                if (Convert.ToInt32(hdCliID.Value) == 0)
                {
                    listaDatos = cClientes.GetItemsWithExtNetFilterList<Data.Vw_Clientes>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                }
                else
                {
                    listaDatos = cClientes.GetItemsWithExtNetFilterList<Data.Vw_Clientes>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
                    List<Data.Clientes> listaClientes;

                    listaClientes = ListaClientes();

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
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
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

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos

                try
                {
                    MonedasController cMonedas = new MonedasController();
                    var ls = cMonedas.GetAllActivos();
                    if (ls != null)
                    {
                        storeMonedas.DataSource = ls;
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

        #endregion

        #region TIPOS PROYECTOS

        protected void storeProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    List<Data.Vw_ClientesProyectosTipos> lista;
                    lista = cProyectosTipos.ProyectosTiposAsignados(Int64.Parse(GridRowSelect.SelectedRecordID));
                    lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, e.Start, e.Limit, ref iCount);

                    if (lista != null)
                    {
                        storeProyectosTipos.DataSource = lista;
                        PageProxy temp = (PageProxy)storeProyectosTipos.Proxy[0];
                        temp.Total = iCount;
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

        #endregion

        #region TIPOS PROYECTOS LIBRES

        protected void storeProyectosTiposLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    List<Data.ProyectosTipos> lista;
                    lista = cProyectosTipos.ProyectosTiposNoAsignado(Int64.Parse(GridRowSelect.SelectedRecordID));
                    lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, e.Start, e.Limit, ref iCount);

                    if (lista != null)
                    {
                        storeProyectosTiposLibres.DataSource = lista;
                        PageProxy temp = (PageProxy)storeProyectosTiposLibres.Proxy[0];
                        temp.Total = iCount;
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

        #endregion

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos

                try
                {
                    OperadoresController cOperadores = new OperadoresController();

                    var ls = cOperadores.GetActivos(long.Parse(hdCliID.Value.ToString()));
                    if (ls != null)
                    {
                        storeOperadores.DataSource = ls;
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

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();

            ClientesController cClientes = new ClientesController();
            long lCliID = 0;
            
            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.Clientes oDato;
                    oDato = cClientes.GetItem(lS);
                    
                    lCliID = long.Parse(hdCliID.Value.ToString());
                        
                    oDato.Cliente = txtCliente.Text;
                    if (FileImagen.HasFile)
                    {
                        oDato.Imagen = FormatoImagenCorrecto(FileImagen);
                    }
                    oDato.OperadorID = long.Parse(cmbOperadores.SelectedItem.Value.ToString());
                    oDato.MonedaID = long.Parse(cmbMoneda.SelectedItem.Value.ToString());
                    oDato.CIF = txtCIF.Text;
                    oDato.VexProyecto = txtVexProyecto.Text;
                    oDato.CodigoInstancia = txtCodigoInstancia.Text;
                    oDato.VexEntorno = txtVexEntorno.Text;
                        
                    InfoResponse infoResponse = cClientes.ActualizarCliente(oDato);

                    if (infoResponse.Result && GuardarImagenRuta() != null)
                    {
                        infoResponse = cClientes.SubmitChanges();
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                    }

                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    Data.Clientes oDato = new Data.Clientes
                    {
                        Cliente = txtCliente.Text,
                        CIF = txtCIF.Text,
                        Activo = true,
                        Imagen = (FileImagen.HasFile) ? FormatoImagenCorrecto(FileImagen) : "",
                        OperadorID = long.Parse(cmbOperadores.SelectedItem.Value.ToString()),
                        MonedaID = long.Parse(cmbMoneda.SelectedItem.Value.ToString()),
                        ClienteID = lCliID
                    };
                    if (!txtVexProyecto.Text.Equals(""))
                    {
                        oDato.VexProyecto = txtVexProyecto.Text;
                    }
                    if (!txtCodigoInstancia.Text.Equals(""))
                    {
                        oDato.CodigoInstancia = txtCodigoInstancia.Text;
                    }
                    if (!txtVexEntorno.Text.Equals(""))
                    {
                        oDato.VexEntorno = txtVexEntorno.Text;
                    }

                    InfoResponse infoResponse = cClientes.AgregarCliente(oDato);

                    if (infoResponse.Result && GuardarImagenRuta() != null)
                    {
                        infoResponse = cClientes.SubmitChanges();
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                    }

                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
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

            ClientesController cClientes = new ClientesController();

            try
            {
                long lS = Convert.ToInt64(GridRowSelect.SelectedRecordID);

                Data.Clientes oDato = default(Data.Clientes);
                oDato = cClientes.GetItem(lS);
                txtCliente.Text = oDato.Cliente;
                txtCIF.Text = oDato.CIF;
                cmbOperadores.SetValue(oDato.OperadorID.ToString());
                cmbMoneda.SetValue(oDato.MonedaID.ToString());

                if (!string.IsNullOrEmpty(oDato.Imagen))
                {
                    logoCliente.Hidden = false;
                    logoCliente.ImageUrl = ImagenCargada();
                }
                else
                {
                    logoCliente.Hidden = true;
                }
                if (oDato.VexEntorno != null)
                {
                    txtVexEntorno.Text = oDato.VexEntorno.ToString();
                }
                if (oDato.VexProyecto != null)
                {
                    txtVexProyecto.Text = oDato.VexProyecto.ToString();
                }
                if (oDato.CodigoInstancia != null)
                {
                    txtCodigoInstancia.Text = oDato.CodigoInstancia.ToString();
                }
                winGestion.Show();

                direct.Success = true;
                direct.Result = "";

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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            ClientesController CClientes = new ClientesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                InfoResponse infoResponse = CClientes.AsignarPorDefecto(lID, long.Parse(hdCliID.Value.ToString()));
                if (infoResponse.Result)
                {
                    infoResponse = CClientes.SubmitChanges();
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                    log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                }
                else
                {
                    CClientes.DiscardChanges();
                    direct.Success = false;
                    direct.Result = infoResponse.Description;
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            ClientesController CClientes = new ClientesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                InfoResponse infoResponse = CClientes.Delete(CClientes.GetItem(lID));
                if (infoResponse.Result)
                {
                    infoResponse = CClientes.SubmitChanges();
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                }
                else
                {
                    CClientes.DiscardChanges();
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
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
            ClientesController cController = new ClientesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Clientes oDato;
                oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;
                InfoResponse infoResponse = cController.Update(oDato);
                if (infoResponse.Result)
                {
                    infoResponse = cController.SubmitChanges();
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                    storePrincipal.DataBind();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
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

        #region TIPOS PROYECTOS

        [DirectMethod]
        public DirectResponse AgregarProyectoTipo()
        {
            DirectResponse direct = new DirectResponse();
            ClientesProyectosTiposController cClientesProyectosTipos = new ClientesProyectosTiposController();
            try
            {
                foreach (SelectedRow selec in GridRowSelectProyectosTiposLibres.SelectedRows)
                {
                    Data.ClientesProyectosTipos oDato = new Data.ClientesProyectosTipos
                    {
                        ProyectoTipoID = Int64.Parse(selec.RecordID),
                        ClienteID = Int64.Parse(GridRowSelect.SelectedRecordID)
                    };
                    cClientesProyectosTipos.AddItem(oDato);
                }

                storeProyectosTipos.DataBind();
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse QuitarProyectoTipo()
        {
            DirectResponse direct = new DirectResponse();
            ClientesProyectosTiposController cClientesProyectosTipos = new ClientesProyectosTiposController();

            try
            {
                foreach (SelectedRow selec in GridRowSelectProyectosTipos.SelectedRows)
                {
                    Data.ClientesProyectosTipos oDato;
                    oDato = cClientesProyectosTipos.getClientesProyectosTipos(Int64.Parse(GridRowSelect.SelectedRecordID), Int64.Parse(selec.RecordID));
                    if (oDato != null)
                        cClientesProyectosTipos.DeleteItem(oDato.ClientesProyectosTiposID);
                }

                storeProyectosTipos.DataBind();
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        #endregion

        #region ANONIMIZAR

        [DirectMethod()]
        public DirectResponse Anonimizar()
        {
            DirectResponse direct = new DirectResponse();

            ClientesController cClientes = new ClientesController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Clientes oDato;
                oDato = cClientes.GetItem(lS);
                oDato.Cliente = GetGlobalResourceObject("Comun", "strAnonimo") + " " + lS.ToString();
                oDato.CIF = GetGlobalResourceObject("Comun", "strAnonimo") + " " + lS.ToString();

                InfoResponse infoResponse = cClientes.Update(oDato);
                if (infoResponse.Result)
                {
                    infoResponse = cClientes.SubmitChanges();
                    storePrincipal.DataBind();
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                }
                else
                {
                    direct.Success = infoResponse.Result;
                    direct.Result = infoResponse.Description;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return direct;
        }

        #endregion

        #region IMAGENES

        [DirectMethod()]
        public string GuardarImagenRuta()
        {

            try
            {
                if (FileImagen.HasFile)
                {

                    Data.Usuarios oUsuario = (Data.Usuarios)this.Session["USUARIO"];

                    string[] extension = FileImagen.FileName.Split('.');

                    string nombreArchivo = oUsuario.ClienteID.ToString() + "." + extension[1];
                    string path = DirectoryMapping.GetImagenClienteDirectory();
                    string ruta = Path.Combine(path, nombreArchivo);
                    FileImagen.PostedFile.SaveAs(ruta);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

            return FileImagen.FileName;
        }

        public string ImagenCargada()
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            string imagenCargada = "";

            if (oUsuario.ClienteID != null)
            {

                var files = Directory.GetFiles(DirectoryMapping.GetImagenClienteDirectory(), oUsuario.ClienteID.ToString() + ".*");

                if (files.Length > 0)
                {
                    var tempFiles = Directory.GetFiles(DirectoryMapping.GetImagenClienteTempDirectory(), oUsuario.ClienteID.ToString() + ".*");
                    string extension = files[0].Split('.')[1];
                    string rutaTemp = Path.Combine(DirectoryMapping.GetImagenClienteTempDirectory(), oUsuario.ClienteID.ToString() + '.' + extension);

                    if (tempFiles.Length.Equals(0))
                    {
                        File.Copy(files[0], rutaTemp);
                    }

                    imagenCargada = "/" + Path.Combine(DirectoryMapping.GetImagenClienteTempDirectoryRelative(), oUsuario.ClienteID.ToString() + '.' + extension); ;

                }
            }
            else
            {
                imagenCargada = "";
            }

            if (imagenCargada == "")
            {
                imagenCargada = "../ima/clientes/LOGOAtrebo.svg";
            }

            return imagenCargada;
        }

        #endregion

        #endregion

        #region FUNCTIONS
        public string FormatoImagenCorrecto(FileUploadField file)
        {
            string nombreArchivo = file.FileName;
            return nombreArchivo;
        }
        #endregion
    }
}