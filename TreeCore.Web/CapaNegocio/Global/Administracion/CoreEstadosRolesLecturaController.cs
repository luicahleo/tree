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
    public class CoreEstadosRolesLecturaController : GeneralBaseController<CoreEstadosRolesLectura, TreeCoreContext>, IGestionBasica<CoreEstadosRolesLectura>
    {
        public CoreEstadosRolesLecturaController()
            : base()
        { }

        public InfoResponse Add(CoreEstadosRolesLectura oEntidad)
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

        public InfoResponse Update(CoreEstadosRolesLectura oEntidad)
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

        public InfoResponse Delete(CoreEstadosRolesLectura oEntidad)
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


        public bool RegistroDuplicado(CoreEstadosRolesLectura oEntidad)
        {
            bool isExiste = false;
            List<CoreEstadosRolesLectura> datos;

            datos = (from c in Context.CoreEstadosRolesLectura where (c.CoreEstadoID == oEntidad.CoreEstadoID && c.RolID == oEntidad.RolID) select c).ToList<CoreEstadosRolesLectura>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }














        public List<CoreEstadosRolesLectura> getRolByEstadoID(long lEstadoID)
        {
            List<CoreEstadosRolesLectura> listaDatos;
            List<long> lRolID;

            try
            {
                lRolID = (from c in Context.CoreEstadosRolesLectura where c.CoreEstadoID == lEstadoID select c.CoreEstadoRolLecturaID).ToList();
                listaDatos = (from c in Context.CoreEstadosRolesLectura where (lRolID.Contains(c.CoreEstadoRolLecturaID))  select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreEstadosRolesLectura> getTablaRolesByEstadoID(long lEstadoID)
        {
            List<CoreEstadosRolesLectura> listaDatos;
            List<long> lRolID;

            try
            {
                lRolID = (from c in Context.CoreEstadosRolesLectura where c.CoreEstadoID == lEstadoID select c.CoreEstadoRolLecturaID).ToList();
                listaDatos = (from c in Context.CoreEstadosRolesLectura where (lRolID.Contains(c.CoreEstadoRolLecturaID)) select c).ToList();
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
            List<CoreEstadosRolesLectura> datos = new List<CoreEstadosRolesLectura>();

            datos = (from c in Context.CoreEstadosRolesLectura where (c.CoreEstadoID == lEstadoID && c.RolID == lRolID) select c).ToList<CoreEstadosRolesLectura>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }


        public CoreEstadosRolesLectura GetBuscado(long lEstadoID, long lRolID)
        {
            CoreEstadosRolesLectura oRol;

            try
            {
                oRol = (from c in Context.CoreEstadosRolesLectura where (c.CoreEstadoID == lEstadoID && c.RolID == lRolID) select c).First();
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