using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class AlquileresTiposContratosSAPController : GeneralBaseController<AlquileresTiposContratosSAP, TreeCoreContext>, IBasica<AlquileresTiposContratosSAP>
    {
        public AlquileresTiposContratosSAPController()
            : base()
        {

        }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string TipoContratoSAP, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresTiposContratosSAP> datos = new List<AlquileresTiposContratosSAP>();


            datos = (from c in Context.AlquileresTiposContratosSAP where (c.TipoContratoSAP == TipoContratoSAP && c.ClienteID == clienteID) select c).ToList<AlquileresTiposContratosSAP>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerTipoContratoSAPID)
        {
            AlquileresTiposContratosSAP dato = new AlquileresTiposContratosSAP();
            AlquileresTiposContratosSAPController cController = new AlquileresTiposContratosSAPController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerTipoContratoSAPID == " + AlquilerTipoContratoSAPID.ToString());

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

        public List<AlquileresTiposContratosSAP> GetActivos(long clienteID) {
            List<AlquileresTiposContratosSAP> listadatos;
            try
            {
                listadatos = (from c in Context.AlquileresTiposContratosSAP where c.Activo && c.ClienteID == clienteID orderby c.TipoContratoSAP select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
                throw;
            }
            return listadatos;
        }

        public AlquileresTiposContratosSAP GetDefault(long clienteID) {
            AlquileresTiposContratosSAP oAlquileresTiposContratosSAP;
            try
            {
                oAlquileresTiposContratosSAP = (from c in Context.AlquileresTiposContratosSAP where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oAlquileresTiposContratosSAP = null;
            }
            return oAlquileresTiposContratosSAP;
        }

        public AlquileresTiposContratosSAP GetTiposContratosSAPbyCodigo(string codigo)
        {
            List<AlquileresTiposContratosSAP> lTipos = new List<AlquileresTiposContratosSAP>();
            AlquileresTiposContratosSAP oContratoSAP = new AlquileresTiposContratosSAP();

            try
            {
                lTipos = (from c in Context.AlquileresTiposContratosSAP where c.Activo == true && c.Codigo == codigo select c).ToList();
                if (lTipos.Count > 0)
                {
                    oContratoSAP = lTipos.ElementAt(0);
                }
                else
                {
                    oContratoSAP = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContratoSAP = null;
            }

            return oContratoSAP;
        }
    }
}