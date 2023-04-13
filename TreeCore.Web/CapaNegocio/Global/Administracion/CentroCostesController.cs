using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CentroCostesController : GeneralBaseController<CentrosCostes, TreeCoreContext>, IGestionBasica<CentrosCostes>
    {
        public CentroCostesController()
            : base()
        { }

        public InfoResponse Add(CentrosCostes oCoste)
        {
            InfoResponse oResponse = new InfoResponse();

            try
            {
                if (RegistroDuplicado(oCoste))
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
                    oResponse = AddEntity(oCoste);
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

        public InfoResponse Update(CentrosCostes oCoste)
        {
            InfoResponse oResponse = new InfoResponse();

            try
            {
                if (RegistroDuplicado(oCoste))
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
                    oResponse = UpdateEntity(oCoste);
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

        public InfoResponse Delete(CentrosCostes oCoste)
        {
            InfoResponse oResponse;

            try
            {
                if (oCoste.Defecto)
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
                    oResponse = DeleteEntity(oCoste);
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

        public InfoResponse ModificarActivar(CentrosCostes oCoste)
        {
            InfoResponse oResponse;

            try
            {
                oCoste.Activo = !oCoste.Activo;
                oResponse = Update(oCoste);
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

        public InfoResponse SetDefecto(CentrosCostes oCoste)
        {
            InfoResponse oResponse;
            CentrosCostes oDefault;

            try
            {
                oDefault = GetDefault(oCoste.ClienteID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.CentroCosteID != oCoste.CentroCosteID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oCoste.Defecto = true;
                        oCoste.Activo = true;
                        oResponse = Update(oCoste);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oCoste
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oCoste.Defecto = true;
                    oCoste.Activo = true;
                    oResponse = Update(oCoste);
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

        public bool RegistroDuplicado(CentrosCostes oCoste)
        {
            bool isExiste = false;
            List<CentrosCostes> datos;

            datos = (from c in Context.CentrosCostes where (c.CentroCoste == oCoste.CentroCoste && c.ClienteID == oCoste.ClienteID && c.CentroCosteID != oCoste.CentroCosteID) select c).ToList<CentrosCostes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CentrosCostes GetDefault(long? ClienteID)
        {
            CentrosCostes oCoste;

            try
            {
                oCoste = (from c
                         in Context.CentrosCostes
                         where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oCoste = null;
            }

            return oCoste;
        }

    }
}