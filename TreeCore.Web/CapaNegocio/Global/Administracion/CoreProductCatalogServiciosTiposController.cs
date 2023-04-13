using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosTiposController : GeneralBaseController<CoreProductCatalogServiciosTipos, TreeCoreContext>, IGestionBasica<CoreProductCatalogServiciosTipos>
    {
        public CoreProductCatalogServiciosTiposController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogServiciosTipos oTipo)
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

        public InfoResponse Update(CoreProductCatalogServiciosTipos oTipo)
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

        public InfoResponse Delete(CoreProductCatalogServiciosTipos oTipo)
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

        public InfoResponse ModificarActivar(CoreProductCatalogServiciosTipos oTipo)
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

        public InfoResponse SetDefecto(CoreProductCatalogServiciosTipos oTipo)
        {
            InfoResponse oResponse = new InfoResponse();
            CoreProductCatalogServiciosTipos oDefault;

            try
            {
                oDefault = GetDefault();
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.CoreProductCatalogServicioTipoID != oTipo.CoreProductCatalogServicioTipoID)
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

        public bool RegistroDuplicado(CoreProductCatalogServiciosTipos oTipo)
        {
            bool isExiste = false;
            List<CoreProductCatalogServiciosTipos> datos;

            datos = (from c in Context.CoreProductCatalogServiciosTipos where ((c.Nombre == oTipo.Nombre || c.Codigo == oTipo.Codigo) && c.CoreProductCatalogServicioTipoID != oTipo.CoreProductCatalogServicioTipoID) select c).ToList<CoreProductCatalogServiciosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<CoreProductCatalogServiciosTipos> getTiposActivos()
        {
            List<CoreProductCatalogServiciosTipos> listaTipos;

            try
            {
                listaTipos = (from c in Context.CoreProductCatalogServiciosTipos where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public CoreProductCatalogServiciosTipos GetDefault()
        {
            CoreProductCatalogServiciosTipos oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosTipos where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

    }
}