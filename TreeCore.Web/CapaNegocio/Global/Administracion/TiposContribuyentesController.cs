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
    public class TiposContribuyentesController : GeneralBaseController<TiposContribuyentes, TreeCoreContext>, IGestionBasica<TiposContribuyentes>
    {
        public TiposContribuyentesController()
            : base()
        { }

        public bool RegistroVinculado(long TipoContribuyenteID)
        {
            bool existe = true;


            return existe;
        }

        public InfoResponse Add(TiposContribuyentes oEntidad)
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

        public InfoResponse Update(TiposContribuyentes oEntidad)
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

        public InfoResponse Delete(TiposContribuyentes oEntidad)
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

        public InfoResponse ModificarActivar(TiposContribuyentes oEntidad)
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

        public InfoResponse SetDefecto(TiposContribuyentes oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            TiposContribuyentes oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.TipoContribuyenteID != oEntidad.TipoContribuyenteID)
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

        public bool RegistroDuplicado(TiposContribuyentes oEntidad)
        {
            bool isExiste = false;
            List<TiposContribuyentes> datos;

            datos = (from c in Context.TiposContribuyentes where (c.TipoContribuyente == oEntidad.TipoContribuyente && c.ClienteID == oEntidad.ClienteID && c.TipoContribuyenteID != oEntidad.TipoContribuyenteID) select c).ToList<TiposContribuyentes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public TiposContribuyentes GetDefault(long ClienteID)
        {
            TiposContribuyentes TipoContribuyente;
            try
            {
                TipoContribuyente = (from c
                         in Context.TiposContribuyentes
                                     where c.Defecto &&
                                c.ClienteID == ClienteID
                                     select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                TipoContribuyente = null;
            }
            return TipoContribuyente;
        }

        public List<TiposContribuyentes> GetTiposContribuyentesByCliente(long clienteID)
        {
            List<TiposContribuyentes> datos = new List<TiposContribuyentes>();

            datos = (from c in Context.TiposContribuyentes where (c.ClienteID == clienteID) orderby c.TipoContribuyente select c).ToList<TiposContribuyentes>();

            return datos;
        }

        public long GetTiposContribuyentes(string tipoContribuyente)
        {
            long tContr = 0;

            try
            {

                tContr = (from c in Context.TiposContribuyentes where c.TipoContribuyente.Equals(tipoContribuyente) select c.TipoContribuyenteID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tContr = -1;

            }

            return tContr;

        }
    }
}