using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreProductCatalogTiposController : GeneralBaseController<CoreProductCatalogTipos, TreeCoreContext>, IGestionBasica<CoreProductCatalogTipos>
    {
        public CoreProductCatalogTiposController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogTipos oTipo)
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

        public InfoResponse Update(CoreProductCatalogTipos oTipo)
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

        public InfoResponse Delete(CoreProductCatalogTipos oTipo)
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

        public InfoResponse ModificarActivar(CoreProductCatalogTipos oTipo)
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

        public InfoResponse SetDefecto(CoreProductCatalogTipos oTipo)
        {
            InfoResponse oResponse;
            CoreProductCatalogTipos oDefault;

            try
            {
                oDefault = GetDefault((long)oTipo.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.CoreProductCatalogTipoID != oTipo.CoreProductCatalogTipoID)
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

        public bool RegistroDuplicado(CoreProductCatalogTipos oTipo)
        {
            bool isExiste = false;
            List<CoreProductCatalogTipos> datos;

            datos = (from c in Context.CoreProductCatalogTipos where ((c.Nombre == oTipo.Nombre || c.Codigo == oTipo.Codigo) && c.ClienteID == oTipo.ClienteID && c.CoreProductCatalogTipoID != oTipo.CoreProductCatalogTipoID) select c).ToList<CoreProductCatalogTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreProductCatalogTipos GetDefault(long ClienteID)
        {
            CoreProductCatalogTipos oTipo;

            try
            {
                oTipo = (from c
                         in Context.CoreProductCatalogTipos
                            where c.Defecto &&
                                   c.ClienteID == ClienteID
                            select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipo = null;
            }

            return oTipo;
        }

        public List<CoreProductCatalogTipos> GetAllCoreProductCatalogTiposByClienteID(long CliID)
        {
            // Local variables
            List<CoreProductCatalogTipos> lista = null;
            try
            {
                lista = (from c in Context.CoreProductCatalogTipos where c.Activo && c.ClienteID == CliID select c).ToList<CoreProductCatalogTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public bool RegistroDefecto(long lID)
        {
            CoreProductCatalogTipos dato = new CoreProductCatalogTipos();
            CoreProductCatalogTiposController CCoreProductCatalogTipos = new CoreProductCatalogTiposController();
            bool defecto = false;

            dato = CCoreProductCatalogTipos.GetItem("Defecto == true && CoreProductCatalogTipoID == " + lID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }
    }
}