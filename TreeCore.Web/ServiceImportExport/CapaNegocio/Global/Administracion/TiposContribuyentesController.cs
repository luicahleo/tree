using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class TiposContribuyentesController : GeneralBaseController<TiposContribuyentes, TreeCoreContext>, IBasica<TiposContribuyentes>
    {
        public TiposContribuyentesController()
            : base()
        { }

        public bool RegistroVinculado(long TipoContribuyenteID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoContribuyente, long clienteID)
        {
            bool isExiste = false;
            List<TiposContribuyentes> datos = new List<TiposContribuyentes>();


            datos = (from c in Context.TiposContribuyentes where (c.TipoContribuyente == TipoContribuyente && c.ClienteID == clienteID) select c).ToList<TiposContribuyentes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipoContribuyenteID)
        {
            TiposContribuyentes dato = new TiposContribuyentes();
            TiposContribuyentesController cController = new TiposContribuyentesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipoContribuyenteID == " + TipoContribuyenteID.ToString());

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

        public TiposContribuyentes GetDefault(long clienteID) {
            TiposContribuyentes oTiposContribuyentes;
            try
            {
                oTiposContribuyentes = (from c in Context.TiposContribuyentes where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTiposContribuyentes = null;
            }
            return oTiposContribuyentes;
        }

        public List<TiposContribuyentes> GetTiposContribuyentesByCliente(long clienteID)
        {
            List<TiposContribuyentes> datos = new List<TiposContribuyentes>();

            datos = (from c in Context.TiposContribuyentes where (c.ClienteID == clienteID) orderby c.TipoContribuyente select c).ToList<TiposContribuyentes>();

            return datos;
        }

        public long GetTiposContribuyentes(string tipoContribuyente)
        {
            long tContr = 0;

            try
            {

                tContr = (from c in Context.TiposContribuyentes where c.TipoContribuyente.Equals(tipoContribuyente) select c.TipoContribuyenteID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tContr = -1;

            }

            return tContr;

        }
    }
}