using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class TiposDatosController : GeneralBaseController<TiposDatos, TreeCoreContext>, IGestionBasica<TiposDatos>
    {
        public TiposDatosController()
            : base()
        { }

        public InfoResponse Add(TiposDatos oTipo)
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

        public InfoResponse Update(TiposDatos oTipo)
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

        public InfoResponse Delete(TiposDatos oTipo)
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

        public InfoResponse ModificarActivar(TiposDatos oTipo)
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

        public bool RegistroDuplicado(TiposDatos oTipo)
        {
            bool isExiste = false;
            List<TiposDatos> datos;

            datos = (from c in Context.TiposDatos where (c.TipoDato == oTipo.TipoDato && c.TipoDatoID != oTipo.TipoDatoID) select c).ToList<TiposDatos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse SetDefecto(TiposDatos oTipo)
        {
            InfoResponse oResponse = new InfoResponse();
            TiposDatos oDefault;

            try
            {
                oDefault = GetDefault((long)oTipo.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.TipoDatoID != oTipo.TipoDatoID)
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

        public List<TiposDatos> GetAllTipoDatos()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetAllTipoDatosActivos()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos where c.Activo == true orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetListasTiposDatos()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos where c.Activo == true && (c.Codigo == Comun.TIPODATO_CODIGO_LISTA || c.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE) orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetAllTiposDatosNoListas()
        {
            List<TiposDatos> lista = new List<TiposDatos>();
            lista = (from c in Context.TiposDatos where c.Activo == true && (c.Codigo != Comun.TIPODATO_CODIGO_LISTA && c.Codigo != Comun.TIPODATO_CODIGO_LISTA_MULTIPLE) orderby c.TipoDato ascending select c).ToList();

            return lista;

        }

        public List<TiposDatos> GetActivos(long clienteID) {
            List<TiposDatos> lista;
            try
            {
                lista = (from c in Context.TiposDatos where c.ClienteID == clienteID && c.Activo orderby c.TipoDato select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public TiposDatos GetTipoDatosByNombre(string sNombre)
        {
            // Local variables
            List<TiposDatos> lista = null;
            TiposDatos tipoDato = null;

            try
            {
                lista = (from c in Context.TiposDatos where c.TipoDato == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    tipoDato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoDato = null;
            }

            return tipoDato;
        }

        public List<TiposDatos> GetTipoDatosByComponenteID(long lID)
        {
            // Local variables
            List<TiposDatos> lista = null;

            try
            {
                lista = (from c in Context.TiposDatos where c.ComponenteID == lID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public TiposDatos GetDefault(long clienteID) {
            TiposDatos oTiposDatos;
            try
            {
                oTiposDatos = (from c in Context.TiposDatos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTiposDatos =null;
            }
            return oTiposDatos;
        }
    }
}