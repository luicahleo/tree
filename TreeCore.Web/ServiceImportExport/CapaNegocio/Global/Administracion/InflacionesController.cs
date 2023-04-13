using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class InflacionesController : GeneralBaseController<Inflaciones, TreeCoreContext>
    {
        public InflacionesController()
            : base()
        {

        }

        public bool tieneRegistrosAsociados (long InflacionID)
        {
            bool tiene = false;
            List<InflacionesDetalles> detalles = new List<InflacionesDetalles>();
            InflacionesDetallesController cDetalles = new InflacionesDetallesController();
            detalles = cDetalles.GetItemsList("InflacionID == " + InflacionID.ToString());

            return tiene;
        }

        public bool hasDuplicadoInflacionEnPais(string Inflacion, long PaisID, long? ClienteID)
        {
            bool existe = false;
            List<Inflaciones> inflaciones = new List<Inflaciones>();


            if (ClienteID != null)
            {
                inflaciones = (from c in Context.Inflaciones where c.PaisID == PaisID && c.Inflacion == Inflacion && c.ClienteID == ClienteID select c).ToList();
            }
            else
            {
                inflaciones = (from c in Context.Inflaciones where c.PaisID == PaisID && c.Inflacion == Inflacion select c).ToList();
            }
            if (inflaciones.Count > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool hasDuplicadoInflacionEnPaisCliente(string Inflacion, long PaisID, long? clienteID)
        {
            bool existe = false;
            List<Inflaciones> inflaciones = new List<Inflaciones>();

            if (clienteID != null)
            {
                inflaciones = (from c in Context.Inflaciones where c.PaisID == PaisID && c.Inflacion == Inflacion && c.ClienteID == clienteID select c).ToList();
            }
            else
            {
                inflaciones = (from c in Context.Inflaciones where c.PaisID == PaisID && c.Inflacion == Inflacion select c).ToList();
            }

            if (inflaciones.Count > 0)
            {
                existe = true;
            }

            return existe;
        }

        public Inflaciones ObtenerInflacionCrear(string inflacion, long PaisID)
        {
            Inflaciones dato = new Inflaciones();

            dato = GetItem("Inflacion == \"" + inflacion + "\" && PaisID == " + PaisID.ToString());

            if (dato == null)
            {
                InflacionesDetallesController cID = new InflacionesDetallesController();
                InflacionesDetalles infDet = new InflacionesDetalles();

                dato = new Inflaciones();
                dato.Inflacion = inflacion;
                dato.PaisID = PaisID;
                dato = AddItem(dato);


                if (dato != null)
                {

                    infDet.Anualidad = DateTime.Now.Year;
                    infDet.Valor = 0;
                    infDet.InflacionID = dato.InflacionID;

                    infDet = cID.AddItem(infDet);
                }

            }

            return dato;
        }
    }
}
