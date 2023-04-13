using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class IFRS16TratamientosPreviosController : GeneralBaseController<IFRS16TratamientosPrevios, TreeCoreContext>, IGestionBasica<IFRS16TratamientosPrevios>
    {
        public IFRS16TratamientosPreviosController()
            : base()
        { }

        public InfoResponse Add(IFRS16TratamientosPrevios oTratamiento)
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

        public InfoResponse Update(IFRS16TratamientosPrevios oTratamiento)
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

        public InfoResponse Delete(IFRS16TratamientosPrevios oTratamiento)
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

        public InfoResponse ModificarActivar(IFRS16TratamientosPrevios oTratamiento)
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

        public InfoResponse SetDefecto(IFRS16TratamientosPrevios oTratamiento)
        {
            InfoResponse oResponse;
            IFRS16TratamientosPrevios oDefault;

            try
            {
                oDefault = GetDefault((long)oTratamiento.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.IFRS16TratamientoPrevioID != oTratamiento.IFRS16TratamientoPrevioID)
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

        public bool RegistroDuplicado(IFRS16TratamientosPrevios oTratamiento)
        {
            bool isExiste = false;
            List<IFRS16TratamientosPrevios> datos;

            datos = (from c in Context.IFRS16TratamientosPrevios where (c.IFRS16TratamientoPrevio == oTratamiento.IFRS16TratamientoPrevio && c.ClienteID == oTratamiento.ClienteID && c.IFRS16TratamientoPrevioID != oTratamiento.IFRS16TratamientoPrevioID) select c).ToList<IFRS16TratamientosPrevios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public IFRS16TratamientosPrevios GetDefault(long clienteID) {
            IFRS16TratamientosPrevios oIFRS16TratamientosPrevios;
            try
            {
                oIFRS16TratamientosPrevios = (from c in Context.IFRS16TratamientosPrevios where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oIFRS16TratamientosPrevios = null;
            }
            return oIFRS16TratamientosPrevios;
        }
    }
}