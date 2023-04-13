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
    public class CoreServiciosFrecuenciasController : GeneralBaseController<CoreServiciosFrecuencias, TreeCoreContext>, IGestionBasica<CoreServiciosFrecuencias>
    {
        public CoreServiciosFrecuenciasController()
            : base()
        { }

        public InfoResponse Add(CoreServiciosFrecuencias oEntidad)
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

        public InfoResponse Update(CoreServiciosFrecuencias oEntidad)
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

        public InfoResponse Delete(CoreServiciosFrecuencias oEntidad)
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

        public bool RegistroDuplicado(CoreServiciosFrecuencias oEntidad)
        {
            bool isExiste = false;
            List<CoreServiciosFrecuencias> datos;

            datos = (from c in Context.CoreServiciosFrecuencias where (c.Nombre == oEntidad.Nombre && c.CoreServicioFrecuenciaID != oEntidad.CoreServicioFrecuenciaID) select c).ToList<CoreServiciosFrecuencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        //public List<CoreFrecuencias> getFrecuenciasActivas()
        //{
        //    List<CoreFrecuencias> listaFrecuencias;

        //    try
        //    {
        //        listaFrecuencias = (from c in Context.CoreFrecuencias where c.Activo select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaFrecuencias = null;
        //    }

        //    return listaFrecuencias;
        //}

        //public string getCodigoByID(long? lID)
        //{
        //    string sCodigo;

        //    try
        //    {
        //        sCodigo = (from c in Context.CoreFrecuencias where c.CoreFrecuenciaID == lID select c.Codigo).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        sCodigo = null;
        //    }

        //    return sCodigo;
        //}

    }
}