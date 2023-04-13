using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class AlquileresEstadosController : GeneralBaseController<AlquileresEstados, TreeCoreContext>, IBasica<AlquileresEstados>
    {
        public AlquileresEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long AlquilerEstadoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string AlquilerEstado, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresEstados> datos = new List<AlquileresEstados>();


            datos = (from c in Context.AlquileresEstados where (c.Estado == AlquilerEstado && c.ClienteID == clienteID) select c).ToList<AlquileresEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerEstadoID)
        {
            AlquileresEstados oDato;
            AlquileresEstadosController cController = new AlquileresEstadosController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && AlquilerEstadoID == " + AlquilerEstadoID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        public List<AlquileresEstados> GetEstados()
        {
            AlquileresEstadosController cEstados = new AlquileresEstadosController();
            List<AlquileresEstados> listaEstados;

            listaEstados = cEstados.GetItemsList<AlquileresEstados>("", "Estado");

            return listaEstados;
        }

        public List<AlquileresEstados> GetEstadoID(long lEstadoID)
        {
            List<AlquileresEstados> listaEstados;

            listaEstados = (from c in Context.AlquileresEstados where c.AlquilerEstadoID != lEstadoID select c).ToList();

            return listaEstados;
        }

        public AlquileresEstados GetEstadoExpirado()
        {
            AlquileresEstados oEstado;

            try
            {
                oEstado = (from c in Context.AlquileresEstados where c.Expirado == true select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }
            

            return oEstado;
        }

        public AlquileresEstados GetEstadoVigente()
        {
            AlquileresEstados oEstado;

            try
            {
                oEstado = (from c in Context.AlquileresEstados where c.Vigente == true select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public AlquileresEstados GetEstadoNoVigente()
        {
            AlquileresEstados oEstado;

            try
            {
                oEstado = (from c in Context.AlquileresEstados where c.NoVigente == true select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public AlquileresEstados GetDefault(long lClienteID)
        {
            AlquileresEstados oEstado;

            try
            {
                oEstado = (from c in Context.AlquileresEstados where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public long GetEstadoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.AlquileresEstados where c.Estado.Equals(Nombre) select c.AlquilerEstadoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }
    }
}