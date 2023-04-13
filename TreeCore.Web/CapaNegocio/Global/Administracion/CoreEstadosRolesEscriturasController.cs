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
    public class CoreEstadosRolesEscriturasController : GeneralBaseController<CoreEstadosRolesEscrituras, TreeCoreContext>, IGestionBasica<CoreEstadosRolesEscrituras>
    {
        public CoreEstadosRolesEscriturasController()
            : base()
        { }

        public InfoResponse Add(CoreEstadosRolesEscrituras oEntidad)
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

        public InfoResponse Update(CoreEstadosRolesEscrituras oEntidad)
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

        public InfoResponse Delete(CoreEstadosRolesEscrituras oEntidad)
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


        public bool RegistroDuplicado(CoreEstadosRolesEscrituras oEntidad)
        {
            bool isExiste = false;
            List<CoreEstadosRolesEscrituras> datos;

            datos = (from c in Context.CoreEstadosRolesEscrituras where (c.CoreEstadoID == oEntidad.CoreEstadoID && c.RolID == oEntidad.RolID) select c).ToList<CoreEstadosRolesEscrituras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }


        public List<CoreEstadosRolesEscrituras> getRolByEstadoID(long lEstadoID)
        {
            List<CoreEstadosRolesEscrituras> listaDatos;
            List<long> lRolID;

            try
            {
                lRolID = (from c in Context.CoreEstadosRolesEscrituras where c.CoreEstadoID == lEstadoID select c.CoreEstadoRolEscrituraID).ToList();
                listaDatos = (from c in Context.CoreEstadosRolesEscrituras where (lRolID.Contains(c.CoreEstadoRolEscrituraID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreEstadosRolesEscrituras> getTablaRolesByEstadoID(long lEstadoID)
        {
            List<CoreEstadosRolesEscrituras> listaDatos;
            List<long> lRolID;

            try
            {
                lRolID = (from c in Context.CoreEstadosRolesEscrituras where c.CoreEstadoID == lEstadoID select c.CoreEstadoRolEscrituraID).ToList();
                listaDatos = (from c in Context.CoreEstadosRolesEscrituras where (lRolID.Contains(c.CoreEstadoRolEscrituraID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool RegistroDuplicado2(long lEstadoID, long lRolID)
        {
            bool isExiste = false;
            List<CoreEstadosRolesEscrituras> datos = new List<CoreEstadosRolesEscrituras>();

            datos = (from c in Context.CoreEstadosRolesEscrituras where (c.CoreEstadoID == lEstadoID && c.RolID == lRolID) select c).ToList<CoreEstadosRolesEscrituras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }


        public CoreEstadosRolesEscrituras GetBuscado(long lEstadoID, long lRolID)
        {
            CoreEstadosRolesEscrituras oRol;

            try
            {
                oRol = (from c in Context.CoreEstadosRolesEscrituras where (c.CoreEstadoID == lEstadoID && c.RolID == lRolID) select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oRol = null;
            }

            return oRol;
        }
    }
}