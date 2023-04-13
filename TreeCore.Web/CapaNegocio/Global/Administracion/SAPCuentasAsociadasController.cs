using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class SAPCuentasAsociadasController : GeneralBaseController<SAPCuentasAsociadas, TreeCoreContext>, IGestionBasica<SAPCuentasAsociadas>
    {
        public SAPCuentasAsociadasController()
            : base()
        { }

        public InfoResponse Add(SAPCuentasAsociadas oCuenta)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCuenta))
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
                    oResponse = AddEntity(oCuenta);
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

        public InfoResponse Update(SAPCuentasAsociadas oCuenta)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCuenta))
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
                    oResponse = UpdateEntity(oCuenta);
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

        public InfoResponse Delete(SAPCuentasAsociadas oCuenta)
        {
            InfoResponse oResponse;
            try
            {
                if (oCuenta.Defecto)
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
                    oResponse = DeleteEntity(oCuenta);
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

        public InfoResponse ModificarActivar(SAPCuentasAsociadas oCuenta)
        {
            InfoResponse oResponse;

            try
            {
                oCuenta.Activo = !oCuenta.Activo;
                oResponse = Update(oCuenta);
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

        public InfoResponse SetDefecto(SAPCuentasAsociadas oCuenta)
        {
            InfoResponse oResponse;
            SAPCuentasAsociadas oDefault;

            try
            {
                oDefault = GetDefault((long)oCuenta.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.SAPCuentaAsociadaID != oCuenta.SAPCuentaAsociadaID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oCuenta.Defecto = true;
                        oCuenta.Activo = true;
                        oResponse = Update(oCuenta);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oCuenta
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oCuenta.Defecto = true;
                    oCuenta.Activo = true;
                    oResponse = Update(oCuenta);
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

        public bool RegistroDuplicado(SAPCuentasAsociadas oCuenta)
        {
            bool isExiste = false;
            List<SAPCuentasAsociadas> datos;

            datos = (from c in Context.SAPCuentasAsociadas where (c.SAPCuentaAsociada == oCuenta.SAPCuentaAsociada && c.ClienteID == oCuenta.ClienteID && c.SAPCuentaAsociadaID != oCuenta.SAPCuentaAsociadaID) select c).ToList<SAPCuentasAsociadas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public SAPCuentasAsociadas GetDefault(long clienteID)
        {
            SAPCuentasAsociadas cuentaAsociada;
            try
            {
                cuentaAsociada = (from c 
                                  in Context.SAPCuentasAsociadas 
                                  where c.Defecto &&
                                        c.ClienteID == clienteID
                                  select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                cuentaAsociada = null;
            }
            return cuentaAsociada;
        }

        public List<SAPCuentasAsociadas> GetSAPCuentasAsociadasByCliente(long clienteID)
        {
            List<SAPCuentasAsociadas> datos = new List<SAPCuentasAsociadas>();

            datos = (from c in Context.SAPCuentasAsociadas where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPCuentasAsociadas>();

            return datos;
        }

        public SAPCuentasAsociadas GetCuentaAsociadaByNombre(string sNombre)
        {
            List<SAPCuentasAsociadas> lista = null;
            SAPCuentasAsociadas dato = null;

            try
            {

                lista = (from c in Context.SAPCuentasAsociadas where c.SAPCuentaAsociada == sNombre select c).ToList();
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