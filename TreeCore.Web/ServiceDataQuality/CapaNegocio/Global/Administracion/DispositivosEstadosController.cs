using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class DispositivosEstadosController : GeneralBaseController<DispositivosEstados, TreeCoreContext>, IBasica<DispositivosEstados>
    {
        public DispositivosEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string DispositivoEstado, long clienteID)
        {
            bool isExiste = false;
            List<DispositivosEstados> datos = new List<DispositivosEstados>();


            datos = (from c in Context.DispositivosEstados where (c.DispositivoEstado == DispositivoEstado && c.ClienteID == clienteID) select c).ToList<DispositivosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long DispositivoEstadoID)
        {
            DispositivosEstados dato = new DispositivosEstados();
            DispositivosEstadosController cController = new DispositivosEstadosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DispositivoEstadoID == " + DispositivoEstadoID.ToString());

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

        public DispositivosEstados GetDefault(long clienteID) {
            DispositivosEstados oDispositivosEstados;
            try
            {
                oDispositivosEstados = (from c in Context.DispositivosEstados where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDispositivosEstados = null;
            }
            return oDispositivosEstados;
        }
    }
}