using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreWorkflowsRolesController : GeneralBaseController<CoreWorkflowsRoles, TreeCoreContext>, IGestionBasica<CoreWorkflowsRoles>
    {
        public CoreWorkflowsRolesController()
            : base()
        { }

        public InfoResponse Add(CoreWorkflowsRoles oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = AddEntity(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(CoreWorkflowsRoles oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = UpdateEntity(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(CoreWorkflowsRoles oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = DeleteEntity(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public CoreWorkflowsRoles getRelacion(long lWorkFlowID, long lRolID)
        {
            CoreWorkflowsRoles oDato;
            try
            {
                oDato = (from c in Context.CoreWorkflowsRoles where c.CoreWorkflowID == lWorkFlowID && c.RolID == lRolID select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public List<CoreWorkflowsRoles> getRolesFromWorkFlow(long WorkFlowID)
        {
            List<CoreWorkflowsRoles> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkflowsRoles
                              join rol in Context.Roles on c.RolID equals rol.RolID
                              where c.CoreWorkflowID == WorkFlowID && rol.Activo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public bool ComprobarUsoRolWorkFlow(long WorkFlowID, long RolID)
        {
            bool bUso = false;
            List<CoreWorkflowsInformacionesRoles> listaDatosInfo;
            List<CoreEstadosRolesLectura> listaDatosEstadosLec;
            List<CoreEstadosRolesEscrituras> listaDatosEstadosEsc;
            try
            {
                listaDatosInfo = (from c in Context.CoreWorkflowsInformacionesRoles
                                  join info in Context.CoreWorkflowsInformaciones on c.CoreWorkflowInformacionID equals info.CoreWorkflowInformacionID
                                  where info.CoreWorkFlowID == WorkFlowID && c.RolID == RolID
                                  select c).ToList();
                listaDatosEstadosLec = (from c in Context.CoreEstadosRolesLectura
                                        join info in Context.CoreEstados on c.CoreEstadoID equals info.CoreEstadoID
                                        where info.CoreWorkFlowID == WorkFlowID && c.RolID == RolID
                                        select c).ToList();
                listaDatosEstadosEsc = (from c in Context.CoreEstadosRolesEscrituras
                                        join info in Context.CoreEstados on c.CoreEstadoID equals info.CoreEstadoID
                                        where info.CoreWorkFlowID == WorkFlowID && c.RolID == RolID
                                        select c).ToList();
                if (listaDatosInfo.Count > 0 || listaDatosEstadosLec.Count > 0 || listaDatosEstadosEsc.Count > 0)
                {
                    bUso = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bUso = true;
            }
            return bUso;
        }

        public InfoResponse AddWorkFlowRol(CoreWorkflowsRoles oDato) {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oDato.CoreWorkflows.Publico && ComprobarUsoRolWorkFlow(oDato.CoreWorkflows.CoreWorkFlowID, oDato.RolID))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = "strErrorAnadirRolWFPublic",
                        Data = null
                    };
                    return oResponse;
                }
                oResponse = Add(oDato);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }
        public InfoResponse DeleteWorkFlowRol(CoreWorkflowsRoles oDato) {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (!oDato.CoreWorkflows.Publico && ComprobarUsoRolWorkFlow(oDato.CoreWorkflows.CoreWorkFlowID, oDato.RolID))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = "strErrorEliminarRolWFPrivate",
                        Data = null
                    };
                    return oResponse;
                }
                oResponse = Delete(oDato);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

    }
}