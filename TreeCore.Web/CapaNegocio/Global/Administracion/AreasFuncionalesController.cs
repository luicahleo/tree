using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class AreasFuncionalesController : GeneralBaseController<AreasFuncionales, TreeCoreContext>, IGestionBasica<AreasFuncionales>
    {
        public AreasFuncionalesController()
            : base()
        { }

        public InfoResponse Add(AreasFuncionales oEntidad)
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

        public InfoResponse Update(AreasFuncionales oEntidad)
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

        public InfoResponse Delete(AreasFuncionales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oEntidad.Defecto)
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
                    oResponse = DeleteEntity(oEntidad);
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

        public bool RegistroDuplicado(AreasFuncionales oAreaFuncional)
        {
            bool isExiste = false;
            List<AreasFuncionales> datos;

            datos = (from c in Context.AreasFuncionales where (c.AreaFuncional == oAreaFuncional.AreaFuncional && c.ClienteID == oAreaFuncional.ClienteID && c.AreaFuncionalID != oAreaFuncional.AreaFuncionalID) select c).ToList<AreasFuncionales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public AreasFuncionales GetDefault(long ClienteID)
        {
            AreasFuncionales areasFuncionales;
            try
            {
                areasFuncionales = (from c
                         in Context.AreasFuncionales
                                    where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                areasFuncionales = null;
            }
            return areasFuncionales;
        }

        public InfoResponse ModificarActivar(AreasFuncionales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oEntidad.Activo = !oEntidad.Activo;
                oResponse = Update(oEntidad);
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

        public InfoResponse SetDefecto(AreasFuncionales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            AreasFuncionales oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.AreaFuncionalID != oEntidad.AreaFuncionalID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }
                        oEntidad.Defecto = true;
                        oEntidad.Activo = true;
                        oResponse = Update(oEntidad);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oEntidad
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oEntidad.Defecto = true;
                    oEntidad.Activo = true;
                    oResponse = Update(oEntidad);
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

        public AreasFuncionales GetAreaFuncionalByNombre(String nombre)
        {
            List<TreeCore.Data.AreasFuncionales> lista = new List<AreasFuncionales>();
            AreasFuncionales dato = null;
            try
            {
                lista = (from c in Context.AreasFuncionales where (c.AreaFuncional == nombre) select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }
    }
}