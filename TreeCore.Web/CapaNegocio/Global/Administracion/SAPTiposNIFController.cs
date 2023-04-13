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
    public class SAPTiposNIFController : GeneralBaseController<SAPTiposNIF, TreeCoreContext>, IGestionBasica<SAPTiposNIF>
    {
        public SAPTiposNIFController()
            : base()
        { }

        public InfoResponse Add(SAPTiposNIF oTipo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oTipo))
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
                    oResponse = AddEntity(oTipo);
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

        public InfoResponse Update(SAPTiposNIF oTipo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oTipo))
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
                    oResponse = UpdateEntity(oTipo);
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

        public InfoResponse Delete(SAPTiposNIF oTipo)
        {
            InfoResponse oResponse;
            try
            {
                if (oTipo.Defecto)
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
                    oResponse = DeleteEntity(oTipo);
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

        public InfoResponse ModificarActivar(SAPTiposNIF oTipo)
        {
            InfoResponse oResponse;

            try
            {
                oTipo.Activo = !oTipo.Activo;
                oResponse = Update(oTipo);
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

        public InfoResponse SetDefecto(SAPTiposNIF oTipo)
        {
            InfoResponse oResponse;
            SAPTiposNIF oDefault;

            try
            {
                oDefault = GetDefault((long)oTipo.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.SAPTipoNIFID != oTipo.SAPTipoNIFID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }

                        oTipo.Defecto = true;
                        oTipo.Activo = true;
                        oResponse = Update(oTipo);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oTipo
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oTipo.Defecto = true;
                    oTipo.Activo = true;
                    oResponse = Update(oTipo);
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

        public bool RegistroDuplicado(SAPTiposNIF oTipo)
        {
            bool isExiste = false;
            List<SAPTiposNIF> datos;

            datos = (from c in Context.SAPTiposNIF where (c.Codigo == oTipo.Codigo && c.ClienteID == oTipo.ClienteID && c.SAPTipoNIFID != oTipo.SAPTipoNIFID) select c).ToList<SAPTiposNIF>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public SAPTiposNIF GetDefault(long clienteID) {
            SAPTiposNIF oSAPTiposNIF;
            try
            {
                oSAPTiposNIF = (from c 
                                in Context.SAPTiposNIF 
                                where c.Defecto && 
                                        c.ClienteID == clienteID 
                                select c).First();
            }
            catch (Exception)
            {
                oSAPTiposNIF = null;
                throw;
            }
            return oSAPTiposNIF;
        }

        public List<SAPTiposNIF> GetSAPTiposNIFByCliente(long clienteID)
        {
            List<SAPTiposNIF> datos = new List<SAPTiposNIF>();

            datos = (from c in Context.SAPTiposNIF where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPTiposNIF>();

            return datos;
        }

        public SAPTiposNIF GetTipoNifByCodigo(string sCodigo)
        {
            List<SAPTiposNIF> lista = null;
            SAPTiposNIF dato = null;

            try
            {

                lista = (from c in Context.SAPTiposNIF where c.Codigo == sCodigo select c).ToList();
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