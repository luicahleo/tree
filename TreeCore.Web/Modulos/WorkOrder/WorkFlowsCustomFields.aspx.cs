using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Transactions;
using System.Linq;


namespace TreeCore.ModWorkFlow
{
    public partial class WorkFlowsCustomFields : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        Componentes.Atributos oComponente;
        protected bool SoloLectura = false;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                //#endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridCustomFields, storePrincipal, gridCustomFields.ColumnModel, listaIgnore, _Locale);
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

                hdAtrID.Value = 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PintarAtributos(false, false);
            List<Data.Vw_Funcionalidades> listFun = ((List<Data.Vw_Funcionalidades>)(this.Session["LISTAFUNCIONALIDADES"]));
            List<TreeCore.Data.Modulos> listaMod = ((List<TreeCore.Data.Modulos>)this.Session["LISTAMODULOS"]);
            SoloLectura = true;

            //if (Comun.ComprobarFuncionalidadSoloLectura(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun, listaMod))
            //{
            //    SoloLectura = true;
            //}
            //else if (Comun.ComprobarFuncionalidadTotal(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun, listaMod))
            //{
            //    SoloLectura = false;
            //}

            //AtributosConfiguracion.Modulo = (long)Comun.Modulos.INVENTARIO;
            AtributosConfiguracion.TipoAtributo = Comun.MODULO_WORKFLOW;
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            AtributosConfiguracion.TipoAtributo = Comun.MODULOINVENTARIO;
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected class CustomField
        {
            public long CustomFieldID { get; set; }
            public long CoreAtributoConfiguracionID { get; set; }
            public string Nombre { get; set; }
            public string Codigo { get; set; }
            public string TipoDato { get; set; }
            public bool Activo { get; set; }
        }

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var vLista = ListaPrincipal(long.Parse(hdCliID.Value.ToString()), btnActivos.Pressed);

                    if (vLista != null)
                    {
                        List<CustomField> listaAtr = new List<CustomField>();
                        foreach (var item in vLista)
                        {
                            listaAtr.Add(new CustomField
                            {
                                CustomFieldID = item.CoreCustomFieldID,
                                CoreAtributoConfiguracionID = item.CoreAtributoConfiguracionID,
                                Nombre = item.CoreAtributosConfiguraciones.Nombre,
                                Codigo = item.CoreAtributosConfiguraciones.Codigo,
                                TipoDato = item.CoreAtributosConfiguraciones.TiposDatos.TipoDato,
                                Activo = item.CoreAtributosConfiguraciones.Activo
                            });
                        }
                        storePrincipal.DataSource = listaAtr;
                        storePrincipal.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.CoreCustomFields> ListaPrincipal(long lClienteID, bool Activo)
        {
            List<string> listaValoresSeleccionados;
            List<Data.CoreCustomFields> listaDatos;
            CoreCustomFieldsController cAtr = new CoreCustomFieldsController();
            try
            {
                listaDatos = cAtr.GetCustomFields(lClienteID, Activo);
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

        #region TIPOS DATOS

        protected void storeTiposDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    TiposDatosController cTiposDatos = new TiposDatosController();
                    var listaDatos = cTiposDatos.GetActivos(long.Parse(hdCliID.Value.ToString()));
                    if (listaDatos != null)
                    {
                        storeTiposDatos.DataSource = listaDatos;
                        storeTiposDatos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message); ;
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
            long lCliID = long.Parse(hdCliID.Value.ToString());
            CoreCustomFieldsController cAtr = new CoreCustomFieldsController();
            CoreAtributosConfiguracionesController cAtrConf = new CoreAtributosConfiguracionesController();
            cAtrConf.SetDataContext(cAtr.Context);
            try
            {
                if (bAgregar)
                {
                    if (cAtr.NombreValido(lCliID, txtNombreCampo.Value.ToString()))
                    {
                        var oDato = cAtr.CreateCustomFields(txtNombreCampo.Value.ToString(), long.Parse(cmbTiposDatos.Value.ToString()), lCliID);
                        if (oDato != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);
                        return direct;
                    }
                }
                else
                {
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                    {
                        try
                        {
                            long RegID = long.Parse(GridRowSelect.SelectedRecordID);
                            var oDato = cAtr.GetItem(RegID);
                            var AtrConf = cAtrConf.GetItem(oDato.CoreAtributoConfiguracionID);
                            if (AtrConf.Codigo == txtNombreCampo.Value.ToString() || cAtr.NombreValido(lCliID, txtNombreCampo.Value.ToString()))
                            {
                                AtrConf.Codigo = txtNombreCampo.Value.ToString();
                                AtrConf.Nombre = txtNombreCampo.Value.ToString();
                                if (!cAtrConf.UpdateItem(AtrConf))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
                                }
                                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                                trans.Complete();
                            }
                            else
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                return direct;
                            }                            
                        }
                        catch (Exception ex)
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            log.Error(ex.Message);
                            return direct;
                        }
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long RegID = long.Parse(GridRowSelect.SelectedRecordID);
            CoreCustomFieldsController cAtr = new CoreCustomFieldsController();
            CoreAtributosConfiguracionesController cAtrConf = new CoreAtributosConfiguracionesController();
            try
            {
                var oDato = cAtr.GetItem(RegID);
                txtNombreCampo.SetValue(oDato.CoreAtributosConfiguraciones.Codigo);
                cmbTiposDatos.SetValue(oDato.CoreAtributosConfiguraciones.TipoDatoID);
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
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long RegID = long.Parse(GridRowSelect.SelectedRecordID);
            CoreCustomFieldsController cAtr = new CoreCustomFieldsController();
            CoreAtributosConfiguracionesController cAtrConf = new CoreAtributosConfiguracionesController();
            cAtrConf.SetDataContext(cAtr.Context);

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    var oDato = cAtr.GetItem(RegID);
                    if (!cAtr.DeleteItem(oDato.CoreCustomFieldID))
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                    if (!cAtrConf.DeleteItem(oDato.CoreAtributoConfiguracionID))
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
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
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long RegID = long.Parse(GridRowSelect.SelectedRecordID);
            CoreCustomFieldsController cAtr = new CoreCustomFieldsController();
            CoreAtributosConfiguracionesController cAtrConf = new CoreAtributosConfiguracionesController();
            try
            {
                var oDato = cAtr.GetItem(RegID);
                var AtrConf = cAtrConf.GetItem(oDato.CoreAtributoConfiguracionID);
                AtrConf.Activo = !AtrConf.Activo;
                if (!cAtrConf.UpdateItem(AtrConf))
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    return direct;
                }
                log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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
        public DirectResponse PintarAtributos(bool Update, bool Ordenar)
        {
            DirectResponse direct = new DirectResponse();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            CoreCustomFieldsController cAtr = new CoreCustomFieldsController();
            try
            {
                if (hdAtrID.Value.ToString() != "0")
                {
                    long RegID = long.Parse(hdAtrID.Value.ToString());
                    var AtrConf = cAtr.GetItem(RegID);
                    if (oComponente == null)
                    {
                        oComponente = (Componentes.Atributos)this.LoadControl("/Componentes/Atributos.ascx");
                        oComponente.ID = "ATR" + AtrConf.CoreAtributoConfiguracionID.ToString();
                        oComponente.Nombre = AtrConf.CoreAtributosConfiguraciones.Nombre;
                        oComponente.TipoAtributo = Comun.MODULO_WORKFLOW;
                        oComponente.AtributoID = AtrConf.CoreAtributoConfiguracionID;
                        oComponente.EsSoloLectura = SoloLectura;
                        pnConfigurador.ContentControls.Add(oComponente);
                    }
                    if (Update)
                    {
                        oComponente.PintarControl(false);
                        pnConfigurador.UpdateContent();
                        oComponente.SetAtributoUnico();
                    }
                }
                else
                {
                    if (Update)
                    {
                        //pnConfigurador.ClearContent();
                        //pnConfigurador.Render();
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

    }
}