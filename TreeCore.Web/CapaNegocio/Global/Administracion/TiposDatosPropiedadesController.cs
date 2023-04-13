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
    public class TiposDatosPropiedadesController : GeneralBaseController<TiposDatosPropiedades, TreeCoreContext>, IGestionBasica<TiposDatosPropiedades>
    {
        public TiposDatosPropiedadesController()
            : base()
        { }

        public InfoResponse Add(TiposDatosPropiedades oTipo)
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

        public InfoResponse Update(TiposDatosPropiedades oTipo)
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

        public InfoResponse Delete(TiposDatosPropiedades oTipo)
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

        public InfoResponse ModificarActivar(TiposDatosPropiedades oTipo)
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

        public bool RegistroDuplicado(TiposDatosPropiedades oTipo)
        {
            bool bTieneDuplicidad = false;

            TiposDatosPropiedades dato = (from tdp in Context.TiposDatosPropiedades
                                          where tdp.TipoDatoID == oTipo.TipoDatoID && tdp.Codigo == oTipo.Codigo
                                          select tdp).FirstOrDefault();

            if (dato != null)
                bTieneDuplicidad = true;

            return bTieneDuplicidad;
        }

    }
}