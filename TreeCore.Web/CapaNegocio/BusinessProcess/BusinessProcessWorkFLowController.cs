using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class BusinessProcessWorkFlowController : GeneralBaseController<CoreBusinessProcessWorkflows, TreeCoreContext>, IGestionBasica<CoreBusinessProcessWorkflows>
    {
        public BusinessProcessWorkFlowController()
            : base()
        { }


        public InfoResponse Add(CoreBusinessProcessWorkflows oEntidad)
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

        public InfoResponse Update(CoreBusinessProcessWorkflows oEntidad)
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

        public InfoResponse Delete(CoreBusinessProcessWorkflows oEntidad)
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

        public bool RegistroDuplicado(CoreBusinessProcessWorkflows oEntidad)
        {
            bool isExiste = false;
            List<CoreBusinessProcessWorkflows> datos;

            datos = (from c in Context.CoreBusinessProcessWorkflows where (c.CoreBusinessProcessID == oEntidad.CoreBusinessProcessID && c.CoreWorkflowID == oEntidad.CoreWorkflowID && c.CoreBusinessProcessWorkflowID != oEntidad.CoreBusinessProcessWorkflowID) select c).ToList<CoreBusinessProcessWorkflows>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<CoreBusinessProcessWorkflows> getAllByBusinessID(long lBusinessID)
        {
            List<CoreBusinessProcessWorkflows> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreBusinessProcessWorkflows where c.CoreBusinessProcessID == lBusinessID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }


    }
}