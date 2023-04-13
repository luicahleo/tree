using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosTiposEdificiosController : GeneralBaseController<EmplazamientosTiposEdificios, TreeCoreContext>, IBasica<EmplazamientosTiposEdificios>
    {
        public EmplazamientosTiposEdificiosController()
            : base()
        { }

        public bool RegistroVinculado(long EmplazamientoTipoEdificioID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool HasActiveTipoEdificio(string tipoEdificio, long clienteID) 
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTiposEdificios
                          where c.Activo && 
                                  c.TipoEdificio == tipoEdificio && 
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
        
        public EmplazamientosTiposEdificios GetActivoTipoEdificio(string tipoEdificio, long clienteID)
        {
            EmplazamientosTiposEdificios result;

            try
            {
                result = (from c in Context.EmplazamientosTiposEdificios
                          where c.Activo &&
                                  c.TipoEdificio == tipoEdificio &&
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

        public bool RegistroDuplicado(string TipoEdificio, long clienteID)
        {
            bool isExiste = false;
            List<EmplazamientosTiposEdificios> datos = new List<EmplazamientosTiposEdificios>();


            datos = (from c in Context.EmplazamientosTiposEdificios where (c.TipoEdificio == TipoEdificio && c.ClienteID == clienteID) select c).ToList<EmplazamientosTiposEdificios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoTipoEdificioID)
        {
            EmplazamientosTiposEdificios dato = new EmplazamientosTiposEdificios();
            EmplazamientosTiposEdificiosController cController = new EmplazamientosTiposEdificiosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoTipoEdificioID == " + EmplazamientoTipoEdificioID.ToString());

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
        public long GetTipoEdificioByNombreAll(string Nombre)
        {

            long tipoedificioID = 0;
            try
            {

                tipoedificioID = (from c in Context.EmplazamientosTiposEdificios where c.TipoEdificio.Equals(Nombre) select c.EmplazamientoTipoEdificioID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                tipoedificioID = -1;

            }
            return tipoedificioID;
        }

        public EmplazamientosTiposEdificios GetDefault(long lClienteID)
        {
            EmplazamientosTiposEdificios oTipo;

            try
            {
                oTipo = (from c in Context.EmplazamientosTiposEdificios where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipo = null;
            }

            return oTipo;
        }

        public List<EmplazamientosTiposEdificios> GetActivos (long lClienteID)
        {
            List<EmplazamientosTiposEdificios> listaTipos;

            try
            {
                listaTipos = (from c 
                              in Context.EmplazamientosTiposEdificios 
                              where c.Activo && 
                                    c.ClienteID == lClienteID 
                              orderby c.TipoEdificio 
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public List<string> GetEdificiosNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTiposEdificios
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.TipoEdificio
                         select c.TipoEdificio).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetTipoEdificioByNombreAndTipoEmplazamiento(long EmplazamientoTipoID, string Nombre)
        {
            long tipoedificioID = 0;
            try
            {
                tipoedificioID = (from c in Context.EmplazamientosTiposEdificios where c.TipoEdificio.Equals(Nombre) select c.EmplazamientoTipoEdificioID).First();

                //if (tipoedificioID != 0)
                //{
                //    tipoedificioID = (from c in Context.EmplazamientosTiposEmplazamientosTiposEdificios where c.EmplazamientoTipoEdificioID == tipoedificioID && c.EmplazamientoTipoID == EmplazamientoTipoID select c.EmplazamientoTipoEdificioID).First();
                //}

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoedificioID = -1;
            }

            return tipoedificioID;
        }

    }
}