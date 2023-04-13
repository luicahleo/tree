using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreWorkflowsInformacionesRolesController : GeneralBaseController<CoreWorkflowsInformacionesRoles, TreeCoreContext>, IGestionBasica<CoreWorkflowsInformacionesRoles>
    {
        public CoreWorkflowsInformacionesRolesController()
            : base()
        { }

        public InfoResponse Add(CoreWorkflowsInformacionesRoles oEntidad)
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

        public InfoResponse Update(CoreWorkflowsInformacionesRoles oEntidad)
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

        public InfoResponse Delete(CoreWorkflowsInformacionesRoles oEntidad)
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

        public CoreWorkflowsInformacionesRoles GetRelacion(long lInfoID, long lRolID)
        {
            CoreWorkflowsInformacionesRoles oDato;
            try
            {
                oDato = (from c in Context.CoreWorkflowsInformacionesRoles where c.CoreWorkflowInformacionID == lInfoID && c.RolID == lRolID select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public List<Roles> GetRolesFromInfo(long lInfoID)
        {
            List<Roles> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkflowsInformacionesRoles
                              join rol in Context.Roles on c.RolID equals rol.RolID
                              where c.CoreWorkflowInformacionID == lInfoID && rol.Activo
                              select rol).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

    }
}