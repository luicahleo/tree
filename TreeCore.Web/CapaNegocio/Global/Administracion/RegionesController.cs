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
    public class RegionesController : GeneralBaseController<Regiones, TreeCoreContext>, IGestionBasica<Regiones>
    {
        public RegionesController()
            : base()
        { }

        public bool RegistroVinculado(long RegionID)
        {
            bool existe = true;


            return existe;
        }

        public InfoResponse Add(Regiones oEntidad)
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

        public InfoResponse Update(Regiones oEntidad)
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

        public InfoResponse Delete(Regiones oEntidad)
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

        public bool RegistroDuplicado(Regiones oEntidad)
        {
            bool isExiste = false;
            List<Regiones> datos;

            datos = (from c in Context.Regiones where (c.Region == oEntidad.Region && c.ClienteID == oEntidad.ClienteID && c.RegionID != oEntidad.RegionID) select c).ToList<Regiones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse ModificarActivar(Regiones oEntidad)
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

        public InfoResponse SetDefecto(Regiones oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            Regiones oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.RegionID != oEntidad.RegionID)
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

        public Regiones GetDefault(long ClienteID)
        {
            Regiones region;
            try
            {
                region = (from c
                         in Context.Regiones
                         where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                region = null;
            }
            return region;
        }

        public Regiones GetRegionCompletaByNombre(string Nombre)
        {
            // Local variables
            List<Regiones> lista = null;
            Regiones dato = null;
            // takes the information
            lista = (from c in Context.Regiones where c.Region.Equals(Nombre) select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }
            else
            {
                dato = new Regiones();
            }

            return dato;
        }


        public Regiones GetRegionActivaByNombre(string Nombre)
        {
            Regiones dato;

            try
            {
                dato = (from c in Context.Regiones 
                        where c.Region.Equals(Nombre) 
                        select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }

            return dato;
        }

        public List<Regiones> GetActivos() {
            List<Regiones> listadatos;
            try
            {
                listadatos = (from c 
                              in Context.Regiones 
                              where c.Activo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        public Regiones GetRegionByID(long lID)
        {
            Regiones oDato = null;

            oDato = (from c in Context.Regiones where c.RegionID == lID select c).First();

            return oDato;
        }

        public List<Regiones> GetActivosByClienteID(long lClienteID)
        {
            List<Regiones> listadatos;
            try
            {
                listadatos = (from c in Context.Regiones where c.Activo && c.ClienteID == lClienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

    }
}