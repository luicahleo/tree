using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ServiciosController : GeneralBaseController<Servicios, TreeCoreContext>, IBasica<Servicios>
    {
        public ServiciosController()
            : base()
        { }

        public bool RegistroVinculado(long ServicioID)
        {
            bool existe = true;


            return existe;
        }


        public bool RegistroDuplicado(string Servicio, long clienteID)
        {
            bool isExiste = false;
            List<Servicios> datos = new List<Servicios>();


            datos = (from c in Context.Servicios where (c.NombreServicio == Servicio ) select c).ToList<Servicios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Servicios> GetServiciosActivos()
        {
            List<Servicios> lista = null;

            try
            {
                lista = (from c in Context.Servicios where c.Activo orderby c.NombreServicio select c).ToList();
            }
            catch (Exception)
            {
                lista = new List<Servicios>();
            }

            return lista;
        }

        public bool RegistroDefecto(long id)
        {
            throw new NotImplementedException();
        }
    }
}