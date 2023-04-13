using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreProductCatalogUnidadesController : GeneralBaseController<CoreUnidades, TreeCoreContext>, IGestionBasica<CoreUnidades>
    {
        public CoreProductCatalogUnidadesController()
            : base()
        { }

        public InfoResponse Add(CoreUnidades oUnidad)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oUnidad))
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
                    oResponse = AddEntity(oUnidad);
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

        public InfoResponse Update(CoreUnidades oUnidad)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oUnidad))
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
                    oResponse = UpdateEntity(oUnidad);
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

        public InfoResponse Delete(CoreUnidades oUnidad)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oUnidad);
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

        public InfoResponse ModificarActivar(CoreUnidades oUnidad)
        {
            InfoResponse oResponse;

            try
            {
                oUnidad.Activo = !oUnidad.Activo;
                oResponse = Update(oUnidad);
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

        public bool RegistroDuplicado(CoreUnidades oUnidad)
        {
            bool isExiste = false;
            List<CoreUnidades> datos;

            datos = (from c in Context.CoreUnidades where ((c.Nombre == oUnidad.Nombre || c.Codigo == oUnidad.Codigo) && c.CoreUnidadID != oUnidad.CoreUnidadID) select c).ToList<CoreUnidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<CoreUnidades> getTiposActivos()
        {
            List<CoreUnidades> listaTipos;

            try
            {
                listaTipos = (from c in Context.CoreUnidades where c.Activo select c).ToList();
            }
            catch (Exception)
            {
                listaTipos = null;
            }

            return listaTipos;
        }

    }

}