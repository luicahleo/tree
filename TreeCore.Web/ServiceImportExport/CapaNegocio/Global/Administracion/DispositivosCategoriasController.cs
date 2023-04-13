using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DispositivosCategoriasController : GeneralBaseController<DispositivosCategorias, TreeCoreContext>, IBasica<DispositivosCategorias>
    {
        public DispositivosCategoriasController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string DispositivoCategoria, long clienteID)
        {
            bool isExiste = false;
            List<DispositivosCategorias> datos = new List<DispositivosCategorias>();


            datos = (from c in Context.DispositivosCategorias where (c.DispositivoCategoria == DispositivoCategoria && c.ClienteID == clienteID) select c).ToList<DispositivosCategorias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long DispositivoCategoriaID)
        {
            DispositivosCategorias dato = new DispositivosCategorias();
            DispositivosCategoriasController cController = new DispositivosCategoriasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DispositivoCategoriaID == " + DispositivoCategoriaID.ToString());

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

        public DispositivosCategorias GetDefault(long clienteID)
        {
            DispositivosCategorias dispositivoCategoria;
            try
            {
                dispositivoCategoria = (from c in Context.DispositivosCategorias where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dispositivoCategoria = null;
            }
            return dispositivoCategoria;
        }
    }
}