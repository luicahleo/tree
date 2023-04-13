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
    public class TiposDatosOperadoresController : GeneralBaseController<TiposDatosOperadores, TreeCoreContext>, IGestionBasica<TiposDatosOperadores>
    {
        public TiposDatosOperadoresController()
            : base()
        { }

        public InfoResponse Add(TiposDatosOperadores oTipo)
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

        public InfoResponse Update(TiposDatosOperadores oTipo)
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

        public InfoResponse Delete(TiposDatosOperadores oTipo)
        {
            InfoResponse oResponse;
            try
            {
                oResponse = DeleteEntity(oTipo);
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

        public InfoResponse ModificarActivar(TiposDatosOperadores oTipo)
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

        public bool RegistroDuplicado(TiposDatosOperadores oTipo)
        {
            bool isExiste = false;
            List<TiposDatosOperadores> datos;

            datos = (from c in Context.TiposDatosOperadores where (c.Nombre == oTipo.Nombre && c.TipoDatoID != oTipo.TipoDatoID) select c).ToList<TiposDatosOperadores>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Vw_TiposDatosOperadores> getAllOperadoresActivos()
        {
            List<Vw_TiposDatosOperadores> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_TiposDatosOperadores where c.Activo == true select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_TiposDatosOperadores> getOperadorByTipoDato(long lTipoDatoID)
        {
            List<Vw_TiposDatosOperadores> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_TiposDatosOperadores where c.TipoDatoID == lTipoDatoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public long getIDByNombre(string sOperador)
        {
            long lTipoDatoID;

            try
            {
                lTipoDatoID = (from c in Context.TiposDatosOperadores where (c.Operador == sOperador || c.Nombre == sOperador) select c.TipoDatoOperadorID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lTipoDatoID = 0;
            }

            return lTipoDatoID;
        }
    }
}