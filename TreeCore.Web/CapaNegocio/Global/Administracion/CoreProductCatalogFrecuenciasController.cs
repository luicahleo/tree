using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreProductCatalogFrecuenciasController : GeneralBaseController<CoreFrecuencias, TreeCoreContext>, IGestionBasica<CoreFrecuencias>
    {
        public CoreProductCatalogFrecuenciasController()
            : base()
        { }

        public InfoResponse Add(CoreFrecuencias oFrecuencia)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oFrecuencia))
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
                    oResponse = AddEntity(oFrecuencia);
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

        public InfoResponse Update(CoreFrecuencias oFrecuencia)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oFrecuencia))
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
                    oResponse = UpdateEntity(oFrecuencia);
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

        public InfoResponse Delete(CoreFrecuencias oFrecuencia)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oFrecuencia);
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

        public InfoResponse ModificarActivar(CoreFrecuencias oFrecuencia)
        {
            InfoResponse oResponse;

            try
            {
                oFrecuencia.Activo = !oFrecuencia.Activo;
                oResponse = Update(oFrecuencia);
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

        public bool RegistroDuplicado(CoreFrecuencias oFrecuencia)
        {
            bool isExiste = false;
            List<CoreFrecuencias> datos;

            datos = (from c in Context.CoreFrecuencias where ((c.Nombre == oFrecuencia.Nombre || c.Codigo == oFrecuencia.Codigo) && c.CoreFrecuenciaID != oFrecuencia.CoreFrecuenciaID) select c).ToList<CoreFrecuencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<CoreFrecuencias> getTiposActivos()
        {
            List<CoreFrecuencias> listaTipos;

            try
            {
                listaTipos = (from c in Context.CoreFrecuencias where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

    }
}