using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DispositivosFabricantesController : GeneralBaseController<DispositivosFabricantes, TreeCoreContext>, IBasica<DispositivosFabricantes>
    {
        public DispositivosFabricantesController()
            : base()
        { }

        public bool RegistroVinculado(long DispositivoFabricanteID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string DispositivoFabricante, long clienteID)
        {
            bool isExiste = false;
            List<DispositivosFabricantes> datos = new List<DispositivosFabricantes>();


            datos = (from c in Context.DispositivosFabricantes where (c.DispositivoFabricante == DispositivoFabricante && c.ClienteID == clienteID) select c).ToList<DispositivosFabricantes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long DispositivoFabricanteID)
        {
            DispositivosFabricantes dato;
            DispositivosFabricantesController cController = new DispositivosFabricantesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DispositivoFabricanteID == " + DispositivoFabricanteID.ToString());

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

        public DispositivosFabricantes GetDefault(long lClienteID)
        {
            DispositivosFabricantes oDispositivo;

            try
            {
                oDispositivo = (from c in Context.DispositivosFabricantes where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDispositivo = null;
            }

            return oDispositivo;
        }
    }
}