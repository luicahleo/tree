using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class MetodosPagosController : GeneralBaseController<MetodosPagos, TreeCoreContext>, IGestionBasica<MetodosPagos>
    {
        public MetodosPagosController()
            : base()
        { }

        public InfoResponse Add(MetodosPagos oMetodo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oMetodo))
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
                    oResponse = AddEntity(oMetodo);
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

        public InfoResponse Update(MetodosPagos oMetodo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oMetodo))
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
                    oResponse = UpdateEntity(oMetodo);
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

        public InfoResponse Delete(MetodosPagos oMetodo)
        {
            InfoResponse oResponse;
            try
            {
                if (oMetodo.Defecto)
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
                    oResponse = DeleteEntity(oMetodo);
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

        public InfoResponse ModificarActivar(MetodosPagos oMetodo)
        {
            InfoResponse oResponse;
            try
            {
                oMetodo.Activo = !oMetodo.Activo;
                oResponse = Update(oMetodo);
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

        public InfoResponse SetDefecto(MetodosPagos oMetodo)
        {
            InfoResponse oResponse;
            MetodosPagos oDefault;

            try
            {
                oDefault = GetDefault((long)oMetodo.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.MetodoPagoID != oMetodo.MetodoPagoID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oMetodo.Defecto = true;
                        oMetodo.Activo = true;
                        oResponse = Update(oMetodo);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oMetodo
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oMetodo.Defecto = true;
                    oMetodo.Activo = true;
                    oResponse = Update(oMetodo);
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

        public bool RegistroDuplicado(MetodosPagos oMetodo)
        {
            bool isExiste = false;
            List<MetodosPagos> datos;

            datos = (from c in Context.MetodosPagos where ((c.MetodoPago == oMetodo.MetodoPago || c.CodigoMetodoPago == oMetodo.CodigoMetodoPago) && c.ClienteID == oMetodo.ClienteID && c.MetodoPagoID != oMetodo.MetodoPagoID) select c).ToList<MetodosPagos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<MetodosPagos> GetMetodosPagoByCliente(long clienteID)
        {
            List<MetodosPagos> datos = new List<MetodosPagos>();

            datos = (from c in Context.MetodosPagos where (c.ClienteID == clienteID) orderby c.MetodoPago select c).ToList<MetodosPagos>();

            return datos;
        }

        public MetodosPagos GetDefault(long clienteID)
        {
            MetodosPagos oMetodo;
            try
            {
                oMetodo = (from c 
                           in Context.MetodosPagos 
                           where c.Defecto &&
                                c.ClienteID == clienteID
                           select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMetodo = null;
            }

            return oMetodo;
        }

        public long GetMetodoByCodigoAll(string Codigo)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.MetodosPagos where c.CodigoMetodoPago.Equals(Codigo) select c.MetodoPagoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }
    }
}