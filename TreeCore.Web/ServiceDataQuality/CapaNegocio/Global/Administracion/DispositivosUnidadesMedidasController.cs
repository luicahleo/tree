using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class DispositivosUnidadesMedidasController : GeneralBaseController<DispositivosUnidadesMedidas, TreeCoreContext>, IBasica<DispositivosUnidadesMedidas>
    {
        public DispositivosUnidadesMedidasController()
            : base()
        { }

        public bool RegistroVinculado(long DispositivoUnidadMedidaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string DispositivoUnidadMedida, long clienteID)
        {
            bool isExiste = false;
            List<DispositivosUnidadesMedidas> datos = new List<DispositivosUnidadesMedidas>();


            datos = (from c in Context.DispositivosUnidadesMedidas where (c.DispositivoUnidadMedida == DispositivoUnidadMedida && c.ClienteID == clienteID) select c).ToList<DispositivosUnidadesMedidas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long DispositivoUnidadMedidaID)
        {
            DispositivosUnidadesMedidas dato = new DispositivosUnidadesMedidas();
            DispositivosUnidadesMedidasController cController = new DispositivosUnidadesMedidasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && DispositivoUnidadMedidaID == " + DispositivoUnidadMedidaID.ToString());

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

        public DispositivosUnidadesMedidas GetDefault(long clienteid)
        {
            DispositivosUnidadesMedidas oDispositivosUnidadesMedidas;
            try
            {
                oDispositivosUnidadesMedidas = (from c in Context.DispositivosUnidadesMedidas where c.Defecto && c.ClienteID == clienteid select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDispositivosUnidadesMedidas = null;
            }
            return oDispositivosUnidadesMedidas;
        }
    }
}