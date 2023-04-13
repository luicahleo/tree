using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class NotificacionesGruposCriteriosController : GeneralBaseController<NotificacionesGruposCriterios, TreeCoreContext>, IBasica<NotificacionesGruposCriterios>
    {
        public NotificacionesGruposCriteriosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string NotificacionGrupoCriterio, long clienteID)
        {
            bool isExiste = false;
            List<NotificacionesGruposCriterios> datos = new List<NotificacionesGruposCriterios>();


            datos = (from c in Context.NotificacionesGruposCriterios where (c.NotificacionGrupoCriterio == NotificacionGrupoCriterio && c.ClienteID == clienteID) select c).ToList<NotificacionesGruposCriterios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long NotificacionGrupoCriterioID)
        {
            NotificacionesGruposCriterios dato = new NotificacionesGruposCriterios();
            NotificacionesGruposCriteriosController cController = new NotificacionesGruposCriteriosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && NotificacionGrupoCriterioID == " + NotificacionGrupoCriterioID.ToString());

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
    }
}