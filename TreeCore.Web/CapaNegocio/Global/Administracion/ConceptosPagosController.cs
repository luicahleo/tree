using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ConceptosPagosController : GeneralBaseController<ConceptosPagos, TreeCoreContext>, IBasica<ConceptosPagos>
    {
        public ConceptosPagosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ConceptoPago, long clienteID)
        {
            bool isExiste = false;
            List<ConceptosPagos> datos = new List<ConceptosPagos>();


            datos = (from c in Context.ConceptosPagos where (c.ConceptoPago == ConceptoPago && c.ClienteID == clienteID) select c).ToList<ConceptosPagos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ConceptoPagoID)
        {
            ConceptosPagos dato = new ConceptosPagos();
            ConceptosPagosController cController = new ConceptosPagosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ConceptoPagoID == " + ConceptoPagoID.ToString());

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

        public ConceptosPagos GetDefault(long clienteid)
        {
            ConceptosPagos oConceptosPagos;
            try
            {
                oConceptosPagos = (from c in Context.ConceptosPagos where c.Defecto && c.ClienteID == clienteid select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oConceptosPagos = null;
            }
            return oConceptosPagos;
        }

        public long GetConceptoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.ConceptosPagos where c.ConceptoPago.Equals(Nombre) select c.ConceptoPagoID).First();
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