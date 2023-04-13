using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class SociedadesController : GeneralBaseController<Sociedades, TreeCoreContext>, IBasica<Sociedades>
    {
        public SociedadesController()
            : base()
        {

        }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Sociedad, long clienteID)
        {
            bool isExiste = false;
            List<Sociedades> datos;

            datos = (from c in Context.Sociedades where (c.Sociedad == Sociedad && c.ClienteID == clienteID) select c).ToList<Sociedades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SociedadID)
        {
            Sociedades dato;
            SociedadesController cController = new SociedadesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SociedadID == " + SociedadID.ToString());

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

        public List<Sociedades> GetActivos()
        {
            List<Sociedades> listaSociedades;
            try
            {
                listaSociedades = (from c in Context.Sociedades where c.Activa orderby c.Sociedad select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaSociedades = null;
            }

            return listaSociedades;
        }

        public Sociedades GetDefault(long ClienteID)
        {
            Sociedades sociedad;
            try
            {
                sociedad = (from c
                         in Context.Sociedades
                         where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sociedad = null;
            }
            return sociedad;
        }
        public Sociedades GetSociedadFiscalByCodigo(long clienteID, string sCodigo)
        {
            // Local variables
            List<Sociedades> lista = null;
            Sociedades dato = null;

            try
            {

                lista = (from c in Context.Sociedades where c.ClienteID == clienteID && c.CodigoSociedad == sCodigo && (c.SociedadCECO == false || c.SociedadCECO == null) select c).ToList();

                if (lista != null && lista.Count > 0)
                {

                    dato = lista.ElementAt(0);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public Sociedades GetSociedadCECOByCodigo(long clienteID, string sCodigo)
        {
            // Local variables
            List<Sociedades> lista = null;
            Sociedades dato = null;

            try
            {

                lista = (from c in Context.Sociedades where c.ClienteID == clienteID && c.CodigoSociedad == sCodigo && c.SociedadCECO == true select c).ToList();

                if (lista != null && lista.Count > 0)
                {

                    dato = lista.ElementAt(0);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }
    }
}