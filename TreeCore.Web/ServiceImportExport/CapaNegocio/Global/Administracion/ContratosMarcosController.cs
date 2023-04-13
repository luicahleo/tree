using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
namespace CapaNegocio
{
    public sealed class ContratosMarcosController : GeneralBaseController<ContratosMarcos, TreeCoreContext>
    {
        public ContratosMarcosController()
            : base()
        {

        }


        public bool tieneRegistrosAsociado(long contratoMarcoID)
        {
            bool tiene = false;
            ContratosMarcosController fControl = new ContratosMarcosController();
            List<ContratosMarcos> datos = new List<ContratosMarcos>();

            datos = fControl.GetItemsList("ContratoMarcoID == " + contratoMarcoID.ToString());
            if (datos.Count > 0)
            {
                tiene = true;
            }
            return tiene;
        }

        public ContratosMarcos ContratoMarcoByNumeroContrato(string numContrato)
        {
            ContratosMarcos contratomarco = new ContratosMarcos();

            try
            {
                contratomarco = (from c in Context.ContratosMarcos where c.NumContrato == numContrato  select c).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                contratomarco = null;
            }
            return contratomarco;

        }

        public bool isExisteNumContrato(long PaisID, string NumContrato)
        {
            bool isExiste = false;

            List<ContratosMarcos> Alquileres = new List<ContratosMarcos>();

            Alquileres = (from c in Context.ContratosMarcos where c.PaisID == PaisID && c.NumContrato == NumContrato select c).ToList();
            if (Alquileres.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool isExisteDistintoEmp(string NumContrato)
        {
            bool isExiste = false;

            List<ContratosMarcos> lContratosMarcos = new List<ContratosMarcos>();

            lContratosMarcos = (from c in Context.ContratosMarcos where c.NumContrato == NumContrato select c).ToList();
            if (lContratosMarcos.Count > 0)
            {
                isExiste = true;
            }



            return isExiste;
        }
        public ContratosMarcos GetContratoMarcoByNumContrato(string sNombre)
        {
            // Local variables
            List<ContratosMarcos> lista = new List<ContratosMarcos>();
            ContratosMarcos dato = null;

            // Gets the information
            try
            {
                lista = (from c in Context.ContratosMarcos where c.NumContrato == sNombre select c).ToList();
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
