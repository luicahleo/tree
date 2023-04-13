using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosTiposEstructurasController : GeneralBaseController<EmplazamientosTiposEstructuras, TreeCoreContext>, IBasica<EmplazamientosTiposEstructuras>
    {
        public EmplazamientosTiposEstructurasController()
            : base()
        { }

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

        public bool RegistroDuplicado(string TipoEstructura, long clienteID)
        {
            bool isExiste = false;
            List<EmplazamientosTiposEstructuras> datos = new List<EmplazamientosTiposEstructuras>();


            datos = (from c in Context.EmplazamientosTiposEstructuras where (c.TipoEstructura == TipoEstructura && c.ClienteID == clienteID) select c).ToList<EmplazamientosTiposEstructuras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoTipoEstructuraID)
        {
            EmplazamientosTiposEstructuras dato = new EmplazamientosTiposEstructuras();
            EmplazamientosTiposEstructurasController cController = new EmplazamientosTiposEstructurasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoTipoEstructuraID == " + EmplazamientoTipoEstructuraID.ToString());

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

        public EmplazamientosTiposEstructuras GetDefault(long clienteID) {
            EmplazamientosTiposEstructuras oEmplazamientosTiposEstructuras;
            try
            {
                oEmplazamientosTiposEstructuras = (from c in Context.EmplazamientosTiposEstructuras where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEmplazamientosTiposEstructuras = null;
            }
            return oEmplazamientosTiposEstructuras;
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