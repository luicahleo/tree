using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalZonasController : GeneralBaseController<GlobalZonas, TreeCoreContext>, IBasica<GlobalZonas>
    {
        public GlobalZonasController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalZonaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            bool isExiste = false;
            List<GlobalZonas> datos = new List<GlobalZonas>();


            datos = (from c in Context.GlobalZonas where (c.Nombre == nombre && c.ClienteID == clienteID) select c).ToList<GlobalZonas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalZonaID)
        {
            GlobalZonas dato = new GlobalZonas();
            GlobalZonasController cController = new GlobalZonasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalZonaID == " + GlobalZonaID.ToString());

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

        public GlobalZonas GetDefault(long lClienteID)
        {
            GlobalZonas oZona;

            try
            {
                oZona = (from c 
                         in Context.GlobalZonas 
                         where c.Defecto && 
                                c.ClienteID == lClienteID 
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oZona = null;
            }

            return oZona;
        }

        public List<GlobalZonas> GetAllGlobalZonasLibres(long proyectoID)
        {
            List<long> tipos;
            tipos = (from c in Context.ProyectosGlobalZonas where c.ProyectoID == proyectoID select c.GlobalZonaID).ToList<long>();
            return (from c in Context.GlobalZonas where !(tipos.Contains(c.GlobalZonaID)) select c).ToList<GlobalZonas>();
        }
    }
}