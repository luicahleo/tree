using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class SAPClavesClasificacionesController : GeneralBaseController<SAPClavesClasificaciones, TreeCoreContext>, IGestionBasica<SAPClavesClasificaciones>
    {
        public SAPClavesClasificacionesController()
            : base()
        { }

        public InfoResponse Add(SAPClavesClasificaciones oClave)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oClave))
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
                    oResponse = AddEntity(oClave);
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

        public InfoResponse Update(SAPClavesClasificaciones oClave)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oClave))
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
                    oResponse = UpdateEntity(oClave);
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

        public InfoResponse Delete(SAPClavesClasificaciones oClave)
        {
            InfoResponse oResponse;
            try
            {
                if (oClave.Defecto)
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.jsPorDefecto,
                        Data = null
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oClave);
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

        public InfoResponse ModificarActivar(SAPClavesClasificaciones oClave)
        {
            InfoResponse oResponse;

            try
            {
                oClave.Activo = !oClave.Activo;
                oResponse = Update(oClave);
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

        public InfoResponse SetDefecto(SAPClavesClasificaciones oClave)
        {
            InfoResponse oResponse;
            SAPClavesClasificaciones oDefault;

            try
            {
                oDefault = GetDefault((long)oClave.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.SAPClaveClasificacionID != oClave.SAPClaveClasificacionID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oClave.Defecto = true;
                        oClave.Activo = true;
                        oResponse = Update(oClave);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oClave
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oClave.Defecto = true;
                    oClave.Activo = true;
                    oResponse = Update(oClave);
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

        public bool RegistroDuplicado(SAPClavesClasificaciones oClave)
        {
            bool isExiste = false;
            List<SAPClavesClasificaciones> datos;

            datos = (from c in Context.SAPClavesClasificaciones where (c.SAPClaveClasificacion == oClave.SAPClaveClasificacion && c.ClienteID == oClave.ClienteID && c.SAPClaveClasificacionID != oClave.SAPClaveClasificacionID) select c).ToList<SAPClavesClasificaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public SAPClavesClasificaciones GetDefault(long clienteID)
        {
            SAPClavesClasificaciones claveClasificacion;

            try
            {
                claveClasificacion = (from c 
                                      in Context.SAPClavesClasificaciones 
                                      where c.Defecto &&
                                            c.ClienteID == clienteID
                                      select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                claveClasificacion = null;
            }

            return claveClasificacion;
        }

        public List<SAPClavesClasificaciones> GetSAPClavesClasificacionesByCliente(long clienteID)
        {
            List<SAPClavesClasificaciones> datos = new List<SAPClavesClasificaciones>();

            datos = (from c in Context.SAPClavesClasificaciones where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPClavesClasificaciones>();

            return datos;
        }

        public SAPClavesClasificaciones GetClaveClasificacionByNombre(string sNombre)
        {
            List<SAPClavesClasificaciones> lista = null;
            SAPClavesClasificaciones dato = null;

            try
            {

                lista = (from c in Context.SAPClavesClasificaciones where c.SAPClaveClasificacion == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }
    }
}