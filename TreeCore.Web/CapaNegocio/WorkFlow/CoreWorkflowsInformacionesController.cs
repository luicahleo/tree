using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreWorkflowsInformacionesController : GeneralBaseController<CoreWorkflowsInformaciones, TreeCoreContext>, IGestionBasica<CoreWorkflowsInformaciones>
    {
        public CoreWorkflowsInformacionesController()
            : base()
        { }

        public InfoResponse Add(CoreWorkflowsInformaciones oEntidad)
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

        public InfoResponse Update(CoreWorkflowsInformaciones oEntidad)
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

        public InfoResponse Delete(CoreWorkflowsInformaciones oEntidad)
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

        public List<CoreWorkflowsInformaciones> GetInformaciones(long WorkFlowID)
        {
            List<CoreWorkflowsInformaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkflowsInformaciones where c.CoreWorkFlowID == WorkFlowID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<CoreAtributosConfiguraciones> GetatributosVinculados(long WorkFlowID, long lEstadoID)
        {
            List<CoreAtributosConfiguraciones> atrConfg;
            List<long> listaAsignados;

            try
            {
                listaAsignados = (from c in Context.CoreEstadosTareas where c.CoreEstadoID == lEstadoID select c.CoreWorkflowInformacionID).ToList();

                atrConfg = (from i in Context.CoreWorkflowsInformaciones 
                     join c in Context.CoreCustomFields on i.CoreCustomFieldID equals c.CoreCustomFieldID
                     join a in Context.CoreAtributosConfiguraciones on c.CoreAtributoConfiguracionID equals a.CoreAtributoConfiguracionID
                     where i.CoreWorkFlowID == WorkFlowID && !listaAsignados.Contains(i.CoreWorkflowInformacionID)
                     select a).ToList();
            }
            catch (Exception ex)
            {
                atrConfg = null;
                log.Error(ex.Message);
            }

            return atrConfg;
        }

        public List<CoreTiposInformaciones> GetTiposInfoVinculados(long WorkFlowID, long lEstadoID)
        {
            List<CoreTiposInformaciones> listaInfo;
            List<long> listaAsignados;

            try
            {
                listaAsignados = (from c in Context.CoreEstadosTareas where c.CoreEstadoID == lEstadoID select c.CoreWorkflowInformacionID).ToList();

                listaInfo = (from i in Context.CoreWorkflowsInformaciones
                            join a in Context.CoreTiposInformaciones on i.CoreTipoInformacionID equals a.CoreTipoInformacionID
                            where i.CoreWorkFlowID == WorkFlowID && i.CoreCustomFieldID == null && !listaAsignados.Contains(i.CoreWorkflowInformacionID)
                             select a).ToList();
            }
            catch (Exception ex)
            {
                listaInfo = null;
                log.Error(ex.Message);
            }

            return listaInfo;
        }

        public CoreWorkflowsInformaciones GetRelacion(long WorkFlowID, long CustomFieldID)
        {
            CoreWorkflowsInformaciones oDato;
            try
            {
                oDato = (from c in Context.CoreWorkflowsInformaciones where c.CoreWorkFlowID == WorkFlowID && c.CoreCustomFieldID == CustomFieldID select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                oDato = null;
            }
            return oDato;
        }

        public InfoResponse CreatePurchaseOrder(CoreWorkflows oWorkFlow)
        {
            InfoResponse oResponse = new InfoResponse();
            CoreWorkflowsInformaciones oDatoPO, oDatoChL;
            CoreTiposInformaciones oInfoPO, oInfoChL;
            try
            {
                oDatoPO = (from c in Context.CoreWorkflowsInformaciones
                           join tipo in Context.CoreTiposInformaciones on c.CoreTipoInformacionID equals tipo.CoreTipoInformacionID
                           where tipo.Codigo == Comun.TIPOINFO_PURCHASEORDER && c.CoreWorkFlowID == oWorkFlow.CoreWorkFlowID
                           select c).FirstOrDefault();
                oDatoChL = (from c in Context.CoreWorkflowsInformaciones
                            join tipo in Context.CoreTiposInformaciones on c.CoreTipoInformacionID equals tipo.CoreTipoInformacionID
                            where tipo.Codigo == Comun.TIPOINFO_CHECKLIST && c.CoreWorkFlowID == oWorkFlow.CoreWorkFlowID
                            select c).FirstOrDefault();
                if (oDatoPO == null)
                {
                    oInfoPO = (from c in Context.CoreTiposInformaciones where c.Codigo == Comun.TIPOINFO_PURCHASEORDER select c).FirstOrDefault();
                    if (oInfoPO == null)
                    {
                        oResponse = new InfoResponse
                        {
                            Result = false,
                            Code = "",
                            Description = Comun.strMensajeGenerico,
                            Data = null
                        };
                        return oResponse;
                    }
                    oDatoPO = new CoreWorkflowsInformaciones
                    {
                        CoreWorkflows = oWorkFlow,
                        CoreTipoInformacionID = oInfoPO.CoreTipoInformacionID,
                        Publico = oWorkFlow.Publico
                    };
                    oResponse = Add(oDatoPO);
                    if (!oResponse.Result)
                    {
                        return oResponse;
                    }
                }
                if (oDatoChL == null)
                {
                    oInfoChL = (from c in Context.CoreTiposInformaciones where c.Codigo == Comun.TIPOINFO_CHECKLIST select c).FirstOrDefault();
                    if (oInfoChL == null)
                    {
                        oResponse = new InfoResponse
                        {
                            Result = false,
                            Code = "",
                            Description = Comun.strMensajeGenerico,
                            Data = null
                        };
                        return oResponse;
                    }
                    oDatoChL = new CoreWorkflowsInformaciones
                    {
                        CoreWorkflows = oWorkFlow,
                        CoreTipoInformacionID = oInfoChL.CoreTipoInformacionID,
                        Publico = oWorkFlow.Publico
                    };
                    oResponse = Add(oDatoChL);
                    if (!oResponse.Result)
                    {
                        return oResponse;
                    }
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

        public InfoResponse DeletePurchaseOrder(CoreWorkflows oWorkFlow)
        {
            InfoResponse oResponse = new InfoResponse();
            CoreWorkflowsInformaciones oDatoPO, oDatoChL;
            bool correcto = true;
            try
            {
                oDatoPO = (from c in Context.CoreWorkflowsInformaciones
                           join tipo in Context.CoreTiposInformaciones on c.CoreTipoInformacionID equals tipo.CoreTipoInformacionID
                           where tipo.Codigo == Comun.TIPOINFO_PURCHASEORDER && c.CoreWorkFlowID == oWorkFlow.CoreWorkFlowID
                           select c).FirstOrDefault();
                oDatoChL = (from c in Context.CoreWorkflowsInformaciones
                            join tipo in Context.CoreTiposInformaciones on c.CoreTipoInformacionID equals tipo.CoreTipoInformacionID
                            where tipo.Codigo == Comun.TIPOINFO_CHECKLIST && c.CoreWorkFlowID == oWorkFlow.CoreWorkFlowID
                            select c).FirstOrDefault();
                oResponse = Delete(oDatoPO);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
                oResponse = Delete(oDatoChL);
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

        public CoreWorkflowsInformaciones getObjetoByID (long lInfoID, long lWorkflowID)
        {
            CoreWorkflowsInformaciones oInfo;

            try
            {
                oInfo = (from c in Context.CoreWorkflowsInformaciones where c.CoreTipoInformacionID == lInfoID && c.CoreWorkFlowID == lWorkflowID select c).First();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                oInfo = null;
            }

            return oInfo;
        }

        public CoreWorkflowsInformaciones getObjetoByIDs(long lInfoID, long lWorkflowID, long lCustomID)
        {
            CoreWorkflowsInformaciones oInfo;

            try
            {
                oInfo = (from c in Context.CoreWorkflowsInformaciones where c.CoreTipoInformacionID == lInfoID && c.CoreWorkFlowID == lWorkflowID && c.CoreCustomFieldID == lCustomID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oInfo = null;
            }

            return oInfo;
        }
    }
}