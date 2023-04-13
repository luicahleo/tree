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
    public class EmplazamientosTiposEstructurasController : GeneralBaseController<EmplazamientosTiposEstructuras, TreeCoreContext>, IGestionBasica<EmplazamientosTiposEstructuras>
    {
        public EmplazamientosTiposEstructurasController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosTiposEstructuras oEntidad)
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

        public InfoResponse Update(EmplazamientosTiposEstructuras oEntidad)
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

        public InfoResponse Delete(EmplazamientosTiposEstructuras oEntidad)
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

        public bool RegistroDuplicado(EmplazamientosTiposEstructuras oEntidad)
        {
            bool isExiste = false;
            List<EmplazamientosTiposEstructuras> datos;

            datos = (from c in Context.EmplazamientosTiposEstructuras where (c.TipoEstructura == oEntidad.TipoEstructura && c.ClienteID == oEntidad.ClienteID && c.EmplazamientoTipoEstructuraID != oEntidad.EmplazamientoTipoEstructuraID) select c).ToList<EmplazamientosTiposEstructuras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse ModificarActivar(EmplazamientosTiposEstructuras oEntidad)
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

        public InfoResponse SetDefecto(EmplazamientosTiposEstructuras oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EmplazamientosTiposEstructuras oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EmplazamientoTipoEstructuraID != oEntidad.EmplazamientoTipoEstructuraID)
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

        public EmplazamientosTiposEstructuras GetDefault(long ClienteID)
        {
            EmplazamientosTiposEstructuras emplazamientoTipoEstructura;
            try
            {
                emplazamientoTipoEstructura = (from c
                         in Context.EmplazamientosTiposEstructuras
                                               where c.Defecto &&
                                c.ClienteID == ClienteID
                                               select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                emplazamientoTipoEstructura = null;
            }
            return emplazamientoTipoEstructura;
        }

        public bool RegistroVinculado(long EmplazamientoTipoEstructuraID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveTipoEstructura(string tipoEstructura, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTiposEstructuras
                          where c.Activo &&
                                  c.TipoEstructura == tipoEstructura &&
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

        public EmplazamientosTiposEstructuras GetActivoTipoEstructura(string tipoEstructura, long clienteID)
        {
            EmplazamientosTiposEstructuras existe;

            try
            {
                existe = (from c in Context.EmplazamientosTiposEstructuras
                          where c.Activo &&
                                  c.TipoEstructura == tipoEstructura &&
                                  c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = null;
            }
            return existe;
        }

        public List<EmplazamientosTiposEstructuras> GetActivos(long clienteID)
        {
            List<EmplazamientosTiposEstructuras> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTiposEstructuras
                         where c.Activo &&
                                c.ClienteID == clienteID
                         orderby c.TipoEstructura
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetEstructurasNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTiposEstructuras
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.TipoEstructura
                         select c.TipoEstructura).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetTipoEstructuraByNombreAndTipoEdificio(long TipoEdificioID, string Nombre)
        {
            long tipoestructuraID = 0;
            try
            {
                tipoestructuraID = (from c in Context.EmplazamientosTiposEstructuras where c.TipoEstructura.Equals(Nombre) select c.EmplazamientoTipoEstructuraID).First();

                //if (tipoestructuraID != 0)
                //{
                //    tipoestructuraID = (from c in Context.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras where c.EmplazamientoTipoEdificioID == TipoEdificioID && c.EmplazamientoTipoEstructuraID == tipoestructuraID select c.EmplazamientoTipoEstructuraID).First();
                //}

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoestructuraID = -1;
            }

            return tipoestructuraID;
        }



    }
}