using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GruposAutorizacionesController : GeneralBaseController<GruposAutorizaciones, TreeCoreContext>, IBasica<GruposAutorizaciones>
    {
        public GruposAutorizacionesController()
            : base()
        { }

        public bool RegistroVinculado(long GrupoAutorizacionID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GrupoAutorizacion, long clienteID)
        {
            bool isExiste = false;
            List<GruposAutorizaciones> datos = new List<GruposAutorizaciones>();


            datos = (from c in Context.GruposAutorizaciones where (c.GrupoAutorizacion == GrupoAutorizacion && c.ClienteID == clienteID) select c).ToList<GruposAutorizaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GrupoAutorizacionID)
        {
            GruposAutorizaciones dato = new GruposAutorizaciones();
            GruposAutorizacionesController cController = new GruposAutorizacionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GrupoAutorizacionID == " + GrupoAutorizacionID.ToString());

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

        public GruposAutorizaciones GetDefault(long clienteid)
        {
            GruposAutorizaciones oGruposAutorizaciones;
            try
            {
                oGruposAutorizaciones = (from c in Context.GruposAutorizaciones where c.Defecto && c.ClienteID == clienteid select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGruposAutorizaciones = null;
            }
            return oGruposAutorizaciones;
        }

        public GruposAutorizaciones GetGrupoAutorizacionByNombre(string sNombre)
        {
            // Local variables
            List<GruposAutorizaciones> lista = new List<GruposAutorizaciones>();
            GruposAutorizaciones dato = null;

            // Gets the information
            try
            {
                lista = (from c in Context.GruposAutorizaciones where c.GrupoAutorizacion == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return dato;
        }
    }
}