using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class SAPGruposCuentasController : GeneralBaseController<SAPGruposCuentas, TreeCoreContext>, IGestionBasica<SAPGruposCuentas>
    {
        public SAPGruposCuentasController()
            : base()
        { }

        public InfoResponse Add(SAPGruposCuentas oCuenta)
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

        public InfoResponse Update(SAPGruposCuentas oCuenta)
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

        public InfoResponse Delete(SAPGruposCuentas oCuenta)
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

        public InfoResponse ModificarActivar(SAPGruposCuentas oCuenta)
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

        public InfoResponse SetDefecto(SAPGruposCuentas oCuenta)
        {
            InfoResponse oResponse;
            SAPGruposCuentas oDefault;

            try
            {
                oDefault = GetDefault((long)oCuenta.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.SAPGrupoCuentaID != oCuenta.SAPGrupoCuentaID)
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

        public bool RegistroDuplicado(SAPGruposCuentas oCuenta)
        {
            bool isExiste = false;
            List<SAPGruposCuentas> datos;

            datos = (from c in Context.SAPGruposCuentas where (c.SAPGrupoCuenta == oCuenta.SAPGrupoCuenta && c.ClienteID == oCuenta.ClienteID && c.SAPGrupoCuentaID != oCuenta.SAPGrupoCuentaID) select c).ToList<SAPGruposCuentas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public SAPGruposCuentas GetDefault(long clienteID)
        {
            SAPGruposCuentas oGrupo;

            try
            {
                oGrupo = (from c in Context.SAPGruposCuentas where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGrupo = null;
            }

            return oGrupo;
        }

        public List<SAPGruposCuentas> GetSAPGruposCuentasByCliente(long clienteID)
        {
            List<SAPGruposCuentas> datos = new List<SAPGruposCuentas>();

            datos = (from c in Context.SAPGruposCuentas where (c.ClienteID == clienteID) orderby c.SAPGrupoCuenta select c).ToList<SAPGruposCuentas>();

            return datos;
        }

        public SAPGruposCuentas GetGrupoCuentaByNombre(string sNombre)
        {
            List<SAPGruposCuentas> lista = null;
            SAPGruposCuentas dato = null;

            try
            {

                lista = (from c in Context.SAPGruposCuentas where c.SAPGrupoCuenta == sNombre select c).ToList();
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