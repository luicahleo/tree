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
    public class BusinessProcessController : GeneralBaseController<CoreBusinessProcess, TreeCoreContext>, IGestionBasica<CoreBusinessProcess>
    {
        public BusinessProcessController()
            : base()
        { }


        public InfoResponse Add(CoreBusinessProcess oEntidad)
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

        public InfoResponse Update(CoreBusinessProcess oEntidad)
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

        public InfoResponse Delete(CoreBusinessProcess oEntidad)
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

        public bool RegistroDuplicado(CoreBusinessProcess oEntidad)
        {
            bool isExiste = false;
            List<CoreBusinessProcess> datos;

            datos = (from c in Context.CoreBusinessProcess where (c.Nombre == oEntidad.Nombre || c.Codigo == oEntidad.Codigo && c.ClienteID == oEntidad.ClienteID && c.CoreBusinessProcessID != oEntidad.CoreBusinessProcessID) select c).ToList<CoreBusinessProcess>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }




        public List<CoreBusinessProcess> getAllWorkflowsActivas()
        {
            List<CoreBusinessProcess> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreBusinessProcess where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreBusinessProcess> GetWorkflows(long lClienteID, bool Activo)
        {
            List<CoreBusinessProcess> listaDatos;

            try
            {
                if (Activo)
                {
                    listaDatos = (from c in Context.CoreBusinessProcess where c.ClienteID == lClienteID && c.Activo select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.CoreBusinessProcess where c.ClienteID == lClienteID select c).ToList();
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public string getCodigoByID (long lCoreBusinessProcessID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.CoreBusinessProcess where c.CoreBusinessProcessID == lCoreBusinessProcessID select c.Codigo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sCodigo = null;
            }

            return sCodigo;
        }

    }
}