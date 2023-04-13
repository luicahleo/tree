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
    public class EmplazamientosTamanosController : GeneralBaseController<EmplazamientosTamanos, TreeCoreContext>, IGestionBasica<EmplazamientosTamanos>
    {
        public EmplazamientosTamanosController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosTamanos oEntidad)
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

        public InfoResponse Update(EmplazamientosTamanos oEntidad)
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

        public InfoResponse Delete(EmplazamientosTamanos oEntidad)
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

        public InfoResponse ModificarActivar(EmplazamientosTamanos oEntidad)
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

        public InfoResponse SetDefecto(EmplazamientosTamanos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EmplazamientosTamanos oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EmplazamientoTamanoID != oEntidad.EmplazamientoTamanoID)
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

        public bool RegistroDuplicado(EmplazamientosTamanos oEntidad)
        {
            bool isExiste = false;
            List<EmplazamientosTamanos> datos;

            datos = (from c in Context.EmplazamientosTamanos where (c.Tamano == oEntidad.Tamano && c.ClienteID == oEntidad.ClienteID && c.EmplazamientoTamanoID != oEntidad.EmplazamientoTamanoID) select c).ToList<EmplazamientosTamanos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public EmplazamientosTamanos GetDefault(long ClienteID)
        {
            EmplazamientosTamanos emplazamientoTamano;
            try
            {
                emplazamientoTamano = (from c
                         in Context.EmplazamientosTamanos
                                       where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                emplazamientoTamano = null;
            }
            return emplazamientoTamano;
        }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveTamano(string tamano, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTamanos 
                          where c.Activo && 
                                  c.Tamano == tamano && 
                                  c.ClienteID==clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }
        public EmplazamientosTamanos GetActivoTamano(string tamano, long clienteID)
        {
            EmplazamientosTamanos existe;

            try
            {
                existe = (from c in Context.EmplazamientosTamanos
                          where c.Activo &&
                                  c.Tamano == tamano &&
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

        public bool RegistroDefecto(long EmplazamientoTamanoID)
        {
            EmplazamientosTamanos dato = new EmplazamientosTamanos();
            EmplazamientosTamanosController cController = new EmplazamientosTamanosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoTamanoID == " + EmplazamientoTamanoID.ToString());

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

        public List<EmplazamientosTamanos> GetEmplazamientosTamanosActivos(long ClienteID)
        {
            List<EmplazamientosTamanos> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosTamanos where c.Activo && c.ClienteID == ClienteID orderby c.Tamano select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EmplazamientosTamanos>();
            }

            return lista;
        }
        
        public List<EmplazamientosTamanos> GetActivos(long clienteID)
        {
            List<EmplazamientosTamanos> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTamanos
                         where c.Activo &&
                                c.ClienteID == clienteID
                         orderby c.Tamano
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetTamanosNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTamanos
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.Tamano
                         select c.Tamano).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetTamanoByNombreAll(string Nombre)
        {

            long tamanoID = 0;
            try
            {

                tamanoID = (from c in Context.EmplazamientosTamanos where c.Tamano.Equals(Nombre) select c.EmplazamientoTamanoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tamanoID = -1;

            }
            return tamanoID;
        }

    }
}