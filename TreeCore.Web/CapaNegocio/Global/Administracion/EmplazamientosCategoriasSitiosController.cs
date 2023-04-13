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
    public class EmplazamientosCategoriasSitiosController : GeneralBaseController<EmplazamientosCategoriasSitios, TreeCoreContext>, IGestionBasica<EmplazamientosCategoriasSitios>
    {
        public EmplazamientosCategoriasSitiosController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosCategoriasSitios oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
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
                    oResponse = AddEntity(oEntidad);
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

        public InfoResponse Update(EmplazamientosCategoriasSitios oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
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
                    oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(EmplazamientosCategoriasSitios oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oEntidad.Defecto)
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
                    oResponse = DeleteEntity(oEntidad);
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

        public InfoResponse ModificarActivar(EmplazamientosCategoriasSitios oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oEntidad.Activo = !oEntidad.Activo;
                oResponse = Update(oEntidad);
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

        public InfoResponse SetDefecto(EmplazamientosCategoriasSitios oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EmplazamientosCategoriasSitios oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EmplazamientoCategoriaSitioID != oEntidad.EmplazamientoCategoriaSitioID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }
                        oEntidad.Defecto = true;
                        oEntidad.Activo = true;
                        oResponse = Update(oEntidad);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oEntidad
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oEntidad.Defecto = true;
                    oEntidad.Activo = true;
                    oResponse = Update(oEntidad);
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

        public bool RegistroDuplicado(EmplazamientosCategoriasSitios oEntidad)
        {
            bool isExiste = false;
            List<EmplazamientosCategoriasSitios> datos;

            datos = (from c in Context.EmplazamientosCategoriasSitios where (c.CategoriaSitio == oEntidad.CategoriaSitio && c.ClienteID == oEntidad.ClienteID && c.EmplazamientoCategoriaSitioID != oEntidad.EmplazamientoCategoriaSitioID) select c).ToList<EmplazamientosCategoriasSitios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public EmplazamientosCategoriasSitios GetDefault(long ClienteID)
        {
            EmplazamientosCategoriasSitios categoriaSitio;
            try
            {
                categoriaSitio = (from c
                         in Context.EmplazamientosCategoriasSitios
                                  where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                categoriaSitio = null;
            }
            return categoriaSitio;
        }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveCategoria(string categoria, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosCategoriasSitios
                          where c.Activo && 
                                c.CategoriaSitio == categoria && 
                                c.ClienteID == clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }

        public EmplazamientosCategoriasSitios GetActivoCategoria(string categoria, long clienteID)
        {
            EmplazamientosCategoriasSitios result;

            try
            {
                result = (from c in Context.EmplazamientosCategoriasSitios
                          where c.Activo &&
                                c.CategoriaSitio == categoria &&
                                c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }

        public List<EmplazamientosCategoriasSitios> GetCategoriasSitiosActivas(long ClienteID)
        {
            List<EmplazamientosCategoriasSitios> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosCategoriasSitios where c.Activo && c.ClienteID == ClienteID orderby c.CategoriaSitio select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EmplazamientosCategoriasSitios>();
            }

            return lista;
        }

        public List<EmplazamientosCategoriasSitios> GetActivos(long clienteID)
        {
            List<EmplazamientosCategoriasSitios> lista;

            try
            {
                lista = (from c 
                         in Context.EmplazamientosCategoriasSitios 
                         where c.Activo && 
                                c.ClienteID == clienteID
                         orderby c.CategoriaSitio
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetCategoriasNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosCategoriasSitios
                         where c.Activo == true && c.ClienteID == ClienteID
                         orderby c.CategoriaSitio
                         select c.CategoriaSitio).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetCategoriaByNombreAll(string Nombre)
        {

            long categoriaSitioID = 0;
            try
            {

                categoriaSitioID = (from c in Context.EmplazamientosCategoriasSitios where c.CategoriaSitio.Equals(Nombre) select c.EmplazamientoCategoriaSitioID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                categoriaSitioID = -1;

            }
            return categoriaSitioID;
        }

    }
}