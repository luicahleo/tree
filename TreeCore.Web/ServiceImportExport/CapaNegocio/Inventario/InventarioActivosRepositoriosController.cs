using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioActivosRepositoriosController : GeneralBaseController<ActivosRepositorios, TreeCoreContext>, IBasica<ActivosRepositorios>
    {
        public InventarioActivosRepositoriosController()
            : base()
        { }

        public bool RegistroVinculado(long ActivoRepositorioID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ActivoRepositorio, long clienteID)
        {
            bool isExiste = false;
            List<ActivosRepositorios> datos = new List<ActivosRepositorios>();


            datos = (from c in Context.ActivosRepositorios where (c.ActivoRepositorio == ActivoRepositorio && c.ClienteID == clienteID) select c).ToList<ActivosRepositorios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ActivoRepositorioID)
        {
            ActivosRepositorios dato = new ActivosRepositorios();
            InventarioActivosRepositoriosController cController = new InventarioActivosRepositoriosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ActivoRepositorioID == " + ActivoRepositorioID.ToString());

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

        public ActivosRepositorios GetDefault(long lClienteID) 
        {
            List<ActivosRepositorios> listaDatos = new List<ActivosRepositorios>();

            listaDatos = (from c in Context.ActivosRepositorios where c.Defecto && c.Activo && c.ClienteID == lClienteID select c).ToList();

            if (listaDatos.Count == 1)
            {
                return listaDatos.FirstOrDefault();
            }

            return null;
        }
    }
}