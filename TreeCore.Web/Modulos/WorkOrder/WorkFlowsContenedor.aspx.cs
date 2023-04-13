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

namespace TreeCore.ModWorkFlow
{
    public partial class WorkFlowsContenedor : TreeCore.Page.BasePageExtNet
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

        #region FUNCTIONS

        [DirectMethod()]
        public DirectResponse RecargarPanelLateral(string sPanel)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosGlobalesController cEstados = new CoreEstadosGlobalesController();
            CoreEstadosRolesController cRoles = new CoreEstadosRolesController();
            RolesController cRol = new RolesController();
            CoreEstadosRolesEscriturasController cRolesAsignados = new CoreEstadosRolesEscriturasController();
            CoreEstadosRolesLecturaController cRolesSeguimiento = new CoreEstadosRolesLecturaController();
            EstadosController cEst = new EstadosController();
            RolesController cRols = new RolesController();
            CoreEstadosTareasController cEstadosTareas = new CoreEstadosTareasController();
            UsuariosController cUsers = new UsuariosController();
            CoreEstadosNotificacionesController cNot = new CoreEstadosNotificacionesController();
            CoreCustomFieldsController cAtributosController = new CoreCustomFieldsController();
            CoreTiposInformacionesController cTipoController = new CoreTiposInformacionesController();
            CoreTareasAccionesController cAccionesController = new CoreTareasAccionesController();
            CoreEstadosNotificacionesRolesController cNotRoles = new CoreEstadosNotificacionesRolesController();
            CoreEstadosNotificacionesUsuariosController cNotUsers = new CoreEstadosNotificacionesUsuariosController();
            CoreTiposInformacionesAccionesController cTipoAccionesController = new CoreTiposInformacionesAccionesController();
            var lista = "";
            List<Object> listaObjetos = new List<Object>();
            Object oJson;

            try
            {
                switch (sPanel)
                {
                    case "pnEstadosGlobales":

                        List<Data.CoreEstadosGlobales> listaDatos;
                        Data.CoreObjetosNegocioTipos oNegocioTipo;
                        Object oEstadoGlob;
                        DataTable oEstadoGlobale;
                        int i = 1;
                        string sNombreTabla = "";

                        CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                        TablasModeloDatosController cTablas = new TablasModeloDatosController();

                        listaDatos = cEstados.getCoreEstadosGlobales(long.Parse(hdEstadoPadreID.Value.ToString()));

                        if (listaDatos != null)
                        {
                            foreach (Data.CoreEstadosGlobales oDato in listaDatos)
                            {
                                oNegocioTipo = cObjetos.GetItem((long)oDato.CoreObjetoNegocioTipoID);

                                if (oNegocioTipo != null)
                                {
                                    sNombreTabla = cTablas.getClaveByID(oNegocioTipo.TablaModeloDatoID);
                                    oEstadoGlobale = cObjetos.getEstadoByID(oDato.CoreObjetoNegocioTipoID, oDato.ObjetoEstadoID);

                                    foreach (DataRow item in oEstadoGlobale.Rows)
                                    {
                                        oEstadoGlob = new
                                        {
                                            ID = i,
                                            CoreEstadoGlobalID = oDato.CoreEstadoGlobalID,
                                            Tabla = GetGlobalResource(sNombreTabla),
                                            Estado = item[0].ToString(),
                                        };

                                        listaObjetos.Add(oEstadoGlob);
                                    }
                                }
                                i++;
                            }
                        }

                        break;

                    case "pnEstadosSiguientes":

                        List<Data.CoreEstadosSiguientes> listaEst;

                        listaEst = cEst.getEstadosSiguientes(long.Parse(hdEstadoPadreID.Value.ToString()));

                        if (listaEst != null)
                        {
                            foreach (Data.CoreEstadosSiguientes oDato in listaEst)
                            {
                                Data.CoreEstados oValor = cEst.GetItem(oDato.CoreEstadoPosibleID);

                                oJson = new
                                {
                                    NombreEstado = oValor.Nombre,
                                    CodigoEstado = oValor.Codigo,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;
                    case "pnTareas":

                        List<Data.CoreEstadosTareas> listaEstadosTareas;
                        Object oTarea;
                        string sClaveInfo = "";

                        listaEstadosTareas = cEstadosTareas.GetByEstado(long.Parse(hdEstadoPadreID.Value.ToString())); 

                        if (listaEstadosTareas != null)
                        {
                            foreach (Data.CoreEstadosTareas estadoTarea in listaEstadosTareas)
                            {
                                CoreTiposInformacionesAcciones oTipoAccion = cTipoAccionesController.GetItem(estadoTarea.CoreTipoInformacionAccionID);
                                CoreTiposInformaciones oTipoInfo = cTipoController.GetItem(oTipoAccion.CoreTipoInformacionID);

                                if (oTipoInfo != null)
                                {
                                    if (oTipoInfo.Codigo == "CUSTOMFIELD")
                                    {
                                        CoreAtributosConfiguraciones oAtributo = cAtributosController.getAtributoByCodigo((long)estadoTarea.CoreWorkflowsInformaciones.CoreCustomFieldID);
                                        if (oAtributo != null)
                                        {
                                            sClaveInfo = oAtributo.Codigo;
                                        }
                                    }
                                    else
                                    {
                                        sClaveInfo = GetGlobalResource(oTipoInfo.ClaveRecurso);
                                    }
                                    CoreTareasAcciones oTareaAccion = cAccionesController.GetItem(oTipoAccion.CoreTareaAccionID);

                                    if (oTareaAccion != null)
                                    {
                                        string sClaveAccion = GetGlobalResource(oTareaAccion.ClaveRecurso);

                                        oTarea = new
                                        {
                                            CoreEstadoTareaID = estadoTarea.CoreEstadoTareaID,
                                            Informacion = sClaveInfo,
                                            Accion = sClaveAccion,
                                            Obligatorio = estadoTarea.Obligatorio,
                                            Descripcion = estadoTarea.Descripcion
                                        };

                                        listaObjetos.Add(oTarea);
                                    }
                                }
                            }
                        }

                        break;
                    case "pnRoles":

                        List<Data.CoreEstadosRolesEscrituras> listaRolesEstadosAsignados;
                        List<Data.Roles> listaRoles = new List<Data.Roles>();

                        listaRolesEstadosAsignados = cRolesAsignados.getRolByEstadoID(long.Parse(hdEstadoPadreID.Value.ToString()));

                        Data.Roles oRol = new Data.Roles();

                        foreach (Data.CoreEstadosRolesEscrituras RolEsc in listaRolesEstadosAsignados)
                        {
                            oRol = cRol.GetItem(RolEsc.RolID);

                            listaRoles.Add(oRol);
                        }

                        if (listaRoles != null)
                        {
                            foreach (Data.Roles oDato in listaRoles)
                            {
                                oJson = new
                                {
                                    Name = oDato.Nombre,
                                    Code = oDato.Codigo,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;
                    case "pnRolesSeguimiento":

                        List<Data.CoreEstadosRolesLectura> listaRolesEstadosSeguimiento;
                        List<Data.Roles> listaRolesSegui = new List<Data.Roles>();

                        listaRolesEstadosSeguimiento = cRolesSeguimiento.getRolByEstadoID(long.Parse(hdEstadoPadreID.Value.ToString()));

                        Data.Roles oRolSegui = new Data.Roles();                    

                        foreach (Data.CoreEstadosRolesLectura RolLec in listaRolesEstadosSeguimiento)
                        {
                            oRolSegui = cRol.GetItem(RolLec.RolID);

                            listaRolesSegui.Add(oRolSegui);
                        }

                        if (listaRolesSegui != null)
                        {
                            foreach (Data.Roles oDato in listaRolesSegui)
                            {
                                oJson = new
                                {
                                    Name = oDato.Nombre,
                                    Code = oDato.Codigo,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;
                    case "pnNotificaciones":

                        List<Data.CoreEstadosNotificaciones> listaNot;

                        listaNot = cNot.getNotificacionesByEstado(long.Parse(hdEstadoPadreID.Value.ToString()));

                        if (listaNot != null)
                        {
                            foreach (Data.CoreEstadosNotificaciones oNot in listaNot)
                            {
                                List<long> listaUser = cNotUsers.getUserByNotificacionID(oNot.CoreEstadoNotificacionID);
                                string sUser = "";

                                foreach (long lUser in listaUser)
                                {
                                    string sUsuarios = cUsers.GetItem(lUser).EMail;

                                    if (sUser == "")
                                    {
                                        sUser = sUsuarios;
                                    }
                                    else
                                    {
                                        sUser = sUser + ", " + sUsuarios;
                                    }
                                }

                                List<long> listaRols = cNotRoles.getRolByNotificacionID(oNot.CoreEstadoNotificacionID);
                                string sRole = "";

                                foreach (long lRol in listaRols)
                                {
                                    string sRol = cRols.GetItem(lRol).Codigo;

                                    if (sRole == "")
                                    {
                                        sRole = sRol;
                                    }
                                    else
                                    {
                                        sRole = sRole + ", " + sRol;
                                    }
                                }

                                oJson = new
                                {
                                    User = sUser,
                                    Rol = sRole,
                                    Contenido = oNot.Contenido,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;
                    case "pnMoreInfo":

                        Data.Vw_CoreEstados oEstado;
                        Data.CoreEstados oStatus;

                        oEstado = cEst.getVistaEstado(long.Parse(hdEstadoPadreID.Value.ToString()), long.Parse(hdWorkflowPadreID.Value.ToString()));
                        oStatus = cEst.GetItem(oEstado.CoreEstadoID);

                        if (oEstado != null)
                        {
                            if (oEstado.EstadosSiguientes != null && oEstado.EstadosSiguientes != "")
                            {
                                long lEstadoID = long.Parse(oEstado.EstadosSiguientes.Split('(')[0]);
                                string sEstado = cEst.getNombreEstado(lEstadoID);
                                string sCuenta = oEstado.EstadosSiguientes.Split('(')[1];
                                oEstado.EstadosSiguientes = sEstado + " (" + sCuenta;
                            }

                            oJson = new
                            {
                                Defecto = oEstado.Defecto,
                                Porcentaje = oEstado.Porcentaje,
                                NombreEstado = oEstado.NombreEstado,
                                Codigo = oEstado.Codigo,
                                Descripcion = oStatus.Descripcion,
                                EstadosSiguientes = oEstado.EstadosSiguientes,
                                NombreAgrupacion = oEstado.NombreAgrupacion,
                                Departamento = oEstado.Departamento,
                                TieneDocumento = oEstado.TieneDocumento,
                                TieneRol = oEstado.TieneRol,
                                Completado = oStatus.Completado,
                                PublicoLectura = oStatus.PublicoLectura,
                                PublicoEscritura = oStatus.PublicoEscritura
                            };

                            listaObjetos.Add(oJson);
                        }

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
    }
}