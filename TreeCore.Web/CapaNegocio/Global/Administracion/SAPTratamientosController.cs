using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class SAPTratamientosController : GeneralBaseController<SAPTratamientos, TreeCoreContext>, IGestionBasica<SAPTratamientos>
    {
        public SAPTratamientosController()
            : base()
        { }

        public InfoResponse Add(SAPTratamientos oTratamiento)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oTratamiento))
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
                    oResponse = AddEntity(oTratamiento);
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

        public InfoResponse Update(SAPTratamientos oTratamiento)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oTratamiento))
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
                    oResponse = UpdateEntity(oTratamiento);
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

        public InfoResponse Delete(SAPTratamientos oTratamiento)
        {
            InfoResponse oResponse;
            try
            {
                if (oTratamiento.Defecto)
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
                    oResponse = DeleteEntity(oTratamiento);
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

        public InfoResponse ModificarActivar(SAPTratamientos oTratamiento)
        {
            InfoResponse oResponse;

            try
            {
                oTratamiento.Activo = !oTratamiento.Activo;
                oResponse = Update(oTratamiento);
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

        public InfoResponse SetDefecto(SAPTratamientos oTratamiento)
        {
            InfoResponse oResponse;
            SAPTratamientos oDefault;

            try
            {
                oDefault = GetDefault((long)oTratamiento.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.SAPTratamientoID != oTratamiento.SAPTratamientoID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oTratamiento.Defecto = true;
                        oTratamiento.Activo = true;
                        oResponse = Update(oTratamiento);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oTratamiento
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oTratamiento.Defecto = true;
                    oTratamiento.Activo = true;
                    oResponse = Update(oTratamiento);
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

        public bool RegistroDuplicado(SAPTratamientos oTratamiento)
        {
            bool isExiste = false;
            List<SAPTratamientos> datos;

            datos = (from c in Context.SAPTratamientos where (c.SAPTratamiento == oTratamiento.SAPTratamiento && c.ClienteID == oTratamiento.ClienteID && c.SAPTratamientoID != oTratamiento.SAPTratamientoID) select c).ToList<SAPTratamientos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public SAPTratamientos GetDefault(long lClienteID)
        {
            SAPTratamientos oTratamiento;

            try
            {
                oTratamiento = (from c in Context.SAPTratamientos where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTratamiento = null;
            }

            return oTratamiento;
        }

        public List<SAPTratamientos> GetSAPTratamientosByCliente(long clienteID)
        {
            List<SAPTratamientos> datos = new List<SAPTratamientos>();

            datos = (from c in Context.SAPTratamientos where (c.ClienteID == clienteID) orderby c.SAPTratamiento select c).ToList<SAPTratamientos>();

            return datos;
        }

        public SAPTratamientos GetTratamientoByNombre(string sNombre)
        {
            List<SAPTratamientos> lista = null;
            SAPTratamientos dato = null;

            try
            {

                lista = (from c in Context.SAPTratamientos where c.SAPTratamiento == sNombre select c).ToList();
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