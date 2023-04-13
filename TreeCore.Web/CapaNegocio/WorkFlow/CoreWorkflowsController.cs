using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreWorkflowsController : GeneralBaseController<CoreWorkflows, TreeCoreContext>, IGestionBasica<CoreWorkflows>
    {
        public CoreWorkflowsController()
            : base()
        { }

        public InfoResponse Add(CoreWorkflows oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = AddEntity(oEntidad);
                }
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

        public InfoResponse Update(CoreWorkflows oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oEntidad);
                }
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

        public InfoResponse Delete(CoreWorkflows oEntidad)
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

        public bool RegistroDuplicado(CoreWorkflows oEntidad)
        {
            bool isExiste = false;
            List<CoreWorkflows> datos;

            datos = (from c in Context.CoreWorkflows where ((c.Nombre == oEntidad.Nombre || c.Codigo == oEntidad.Codigo) && c.ClienteID == oEntidad.ClienteID && c.CoreWorkFlowID != oEntidad.CoreWorkFlowID) select c).ToList();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<CoreWorkflows> getAllWorkflowsActivas()
        {
            List<CoreWorkflows> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreWorkflows where c.Activo orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreWorkflows> GetWorkflows(long lClienteID, bool Activo)
        {
            List<CoreWorkflows> listaDatos;

            try
            {
                if (Activo)
                {
                    listaDatos = (from c in Context.CoreWorkflows where c.ClienteID == lClienteID && c.Activo select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.CoreWorkflows where c.ClienteID == lClienteID select c).ToList();
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public string getCodigoByID(long lWorkflowID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.CoreWorkflows where c.CoreWorkFlowID == lWorkflowID select c.Codigo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sCodigo = null;
            }

            return sCodigo;
        }

        public string getIDByName(string lWorkflow)
        {
            string sNombre;

            try
            {
                sNombre = (from c in Context.CoreWorkflows where c.Nombre == lWorkflow select c.CoreWorkFlowID.ToString()).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombre = null;
            }

            return sNombre;
        }

        public long getObjetoByName (string sWorkflow)
        {
            long lWorkflowID;

            try
            {
                lWorkflowID = (from c in Context.CoreWorkflows where c.Nombre == sWorkflow select c.CoreWorkFlowID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lWorkflowID = 0;
            }

            return lWorkflowID;
        }

        public string getWorkFlowByEstadoID(long lEstadoID)
        {
            string sID;

            try
            {
                sID = (from c in Context.CoreEstados where c.CoreEstadoID == lEstadoID select c.CoreWorkFlowID.ToString()).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sID = null;
            }

            return sID;
        }

        public bool SeEstanAplicandoRoles(long lWorkflowID)
        {
            bool Uso = false;
            List<CoreWorkflowsRoles> lRoles;
            List<CoreWorkflowsInformacionesRoles> lRolesInfo;
            List<CoreEstadosRolesEscrituras> lRolesEsc;
            List<CoreEstadosRolesLectura> lRolesLec;

            try
            {
                lRoles = (from c in Context.CoreWorkflowsRoles where c.CoreWorkflowID == lWorkflowID select c).ToList();
                lRolesInfo = (from c in Context.CoreWorkflowsInformacionesRoles join info in Context.CoreWorkflowsInformaciones on c.CoreWorkflowInformacionID equals info.CoreWorkflowInformacionID where info.CoreWorkFlowID == lWorkflowID select c).ToList();
                lRolesEsc = (from c in Context.CoreEstadosRolesEscrituras join info in Context.CoreEstados on c.CoreEstadoID equals info.CoreEstadoID where info.CoreWorkFlowID == lWorkflowID select c).ToList();
                lRolesLec = (from c in Context.CoreEstadosRolesLectura join info in Context.CoreEstados on c.CoreEstadoID equals info.CoreEstadoID where info.CoreWorkFlowID == lWorkflowID select c).ToList();
                if (lRoles.Count > 0 || lRolesInfo.Count > 0 || lRolesEsc.Count > 0 || lRolesLec.Count > 0)
                {
                    Uso = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Uso = true;
            }

            return Uso;
        }

        public InfoResponse AddWorkFlow(CoreWorkflows oWorkFlow)
        {
            CoreWorkflowsInformacionesController cInfo = new CoreWorkflowsInformacionesController();
            cInfo.SetDataContext(Context);
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oWorkFlow.AplicaPO)
                {
                    oResponse = cInfo.CreatePurchaseOrder(oWorkFlow);
                    if (!oResponse.Result)
                    {
                        return oResponse;
                    }
                }
                oResponse = Add(oWorkFlow);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
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

        public InfoResponse UpdateWorkFlow(CoreWorkflows oWorkFlow)
        {
            CoreWorkflowsInformacionesController cInfo = new CoreWorkflowsInformacionesController();
            cInfo.SetDataContext(Context);
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oWorkFlow.AplicaPO)
                {
                    oResponse = cInfo.CreatePurchaseOrder(oWorkFlow);
                    if (!oResponse.Result)
                    {
                        return oResponse;
                    }
                }
                else
                {
                    oResponse = cInfo.DeletePurchaseOrder(oWorkFlow);
                    if (!oResponse.Result)
                    {
                        return oResponse;
                    }
                }
                oResponse = Update(oWorkFlow);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
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

        public InfoResponse DeleteWorkFlow(CoreWorkflows oWorkFlow)
        {
            CoreWorkflowsInformacionesController cInfo = new CoreWorkflowsInformacionesController();
            cInfo.SetDataContext(Context);
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = cInfo.DeletePurchaseOrder(oWorkFlow);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
                oResponse = Delete(oWorkFlow);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
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

        public List<CoreWorkflows> getWorkFlowsByBusinessID(long lBusinessID)
        {
            List<CoreWorkflows> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreBusinessProcessWorkflows
                              join a in Context.CoreWorkflows on c.CoreWorkflowID equals a.CoreWorkFlowID
                              where c.CoreBusinessProcessID == lBusinessID
                              select a).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }
    }
}