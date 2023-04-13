using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalLocalidadesController : GeneralBaseController<GlobalLocalidades, TreeCoreContext>, IBasica<GlobalLocalidades>
    {
        public GlobalLocalidadesController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalLocalidadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalLocalidad, long clienteID)
        {
            bool isExiste = false;
            List<GlobalLocalidades> datos = new List<GlobalLocalidades>();


            datos = (from c in Context.GlobalLocalidades where (c.Localidad == GlobalLocalidad/* && c.ClienteID == clienteID*/) select c).ToList<GlobalLocalidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalLocalidadID)
        {
            GlobalLocalidades dato = new GlobalLocalidades();
            GlobalLocalidadesController cController = new GlobalLocalidadesController();

            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalLocalidadID == " + GlobalLocalidadID.ToString());

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

        public bool getLocalidadRepetido(long partidoID, string localidad)
        {
            // Local variables
            List<GlobalLocalidades> lista = null;
            bool dato = false;
            try
            {
                lista = (from c in Context.GlobalLocalidades where c.GlobalPartidoID == partidoID && c.Localidad == localidad select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }

        public bool getLocalidades_CodigoRepetido(long partidoID, string Codigo)
        {
            // Local variables
            List<GlobalLocalidades> lista = null;
            bool dato = false;
            try
            {
                lista = (from c in Context.GlobalLocalidades where c.GlobalPartidoID == partidoID && c.Codigo == Codigo select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }

        public List<GlobalLocalidades> getAllLocalidadesByPartidoID(long partidoID)
        {
            List<GlobalLocalidades> lista = null;
            try
            {
                lista = (from c in Context.GlobalLocalidades where c.GlobalPartidoID == partidoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetLocalidadesByNombrePartidoID(string Nombre, long partidoID)
        {
            // Local variables
            List<GlobalLocalidades> lista = null;
            long dato = -1;
            // takes the information
            try
            {
                lista = (from c in Context.GlobalLocalidades where c.Localidad.Equals(Nombre) && c.GlobalPartidoID == partidoID select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0).GlobalLocalidadID;
                }
                else
                {
                    dato = -1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return dato;
        }

    }
}