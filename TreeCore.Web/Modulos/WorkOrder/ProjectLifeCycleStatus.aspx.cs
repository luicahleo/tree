using CapaNegocio;

using Ext.Net;

using log4net;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

using TreeCore.APIClient;
using TreeCore.Clases;
using TreeCore.Page;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.Modulos.WorkOrder
{
    public partial class ProjectLifeCycleStatus : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region GESTIÓN DE PÁGINA

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
                        List<ProjectLifeCycleStatusDTO> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        bool bActivo = Request.QueryString["aux"] == "true";

                        List<FilterDTO> filtros = new List<FilterDTO>();
                        if (sFiltro != "")
                        {
                            foreach (var oFiltro in JsonConvert.DeserializeObject<List<FilterExtNet>>(sFiltro))
                            {
                                filtros.Add(new FilterDTO
                                {
                                    Field = oFiltro.property,
                                    Value = oFiltro.value,
                                    Operator = oFiltro.@operator
                                });
                            }
                        }
                        List<string> orders = new List<string>();
                        if (sOrden != "")
                        {
                            orders.Add(sOrden);
                        }

                        QueryDTO queryDTO = new QueryDTO(filtros, orders, sDir);

                        BaseAPIClient<ProjectLifeCycleStatusDTO> aPIClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);

                        listaDatos = aPIClient.GetList(queryDTO).Result.Value;

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strBusinessProcessType").ToString(), _Locale).Wait();
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
                //storePrincipal.Reload();
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
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<ProjectLifeCycleStatusDTO> ApiClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar, string sColor)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<ProjectLifeCycleStatusDTO> ApiClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);
            ProjectLifeCycleStatusDTO oDato;

            try
            {
                if (!bAgregar)
                {
                    string oCode = GridRowSelect.SelectedRecordID;

                    oDato = ApiClient.GetByCode(oCode).Result.Value;

                    var originalCode = oDato.Code;

                    oDato.Name = txtNombreProject.Text;
                    oDato.Code = txtCodigoProject.Text;
                    oDato.Description = txtDescripcionProject.Text;
                    oDato.Colour = sColor;

                    var Result = ApiClient.UpdateEntity(originalCode, oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
                    }
                }
                else
                {
                    oDato = new ProjectLifeCycleStatusDTO
                    {
                        Code = txtCodigoProject.Text,
                        Name = txtNombreProject.Text,
                        Description = txtDescripcionProject.Text,
                        Active = true,
                        Colour = sColor
                    };

                    var Result = ApiClient.AddEntity(oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
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
            hdEditando.SetValue("Editar");

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                BaseAPIClient<ProjectLifeCycleStatusDTO> ApiClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);
                var oDato = ApiClient.GetByCode(oCode).Result;

                txtNombreProject.Text = oDato.Value.Name;
                txtCodigoProject.Text = oDato.Value.Code;
                txtDescripcionProject.Text = oDato.Value.Description;
                direct.Result = oDato.Value.Colour;
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
            //direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<ProjectLifeCycleStatusDTO> ApiClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);
            ProjectLifeCycleStatusDTO oDato;
            ResultDto<ProjectLifeCycleStatusDTO> oResult;

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                oDato = ApiClient.GetByCode(oCode).Result.Value;
                oDato.Colour = "colorPrueba";
                oDato.Active = true;

                var Result = ApiClient.UpdateEntity(oDato.Code, oDato).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<ProjectLifeCycleStatusDTO> ApiClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);

            var lID = GridRowSelect.SelectedRecordID;

            try
            {
                var Result = ApiClient.DeleteEntity(lID).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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
            BaseAPIClient<ProjectLifeCycleStatusDTO> ApiClient = new BaseAPIClient<ProjectLifeCycleStatusDTO>(TOKEN_API);
            ProjectLifeCycleStatusDTO oDato;

            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                oDato = ApiClient.GetByCode(oCode).Result.Value;
                oDato.Active = !oDato.Active;

                var Result = ApiClient.UpdateEntity(oDato.Code, oDato).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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

        #region FUNCTIONS

        #endregion

    }
}