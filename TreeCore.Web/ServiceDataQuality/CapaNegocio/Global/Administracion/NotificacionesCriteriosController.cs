using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class NotificacionesCriteriosController : GeneralBaseController<NotificacionesCriterios, TreeCoreContext>, IBasica<NotificacionesCriterios>
    {
        public NotificacionesCriteriosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string NotificacionCriterio, long grupoID)
        {
            bool isExiste = false;
            List<NotificacionesCriterios> datos = new List<NotificacionesCriterios>();


            datos = (from c in Context.NotificacionesCriterios where (c.NotificacionCriterio == NotificacionCriterio && c.NotificacionGrupoCriterioID == grupoID) select c).ToList<NotificacionesCriterios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long NotificacionCriterioID)
        {
            NotificacionesCriterios dato = new NotificacionesCriterios();
            NotificacionesCriteriosController cController = new NotificacionesCriteriosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && NotificacionCriterioID == " + NotificacionCriterioID.ToString());

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
        public List<NotificacionesCriterios> GetAllActiveCriteria(long grupo)
        {
            // Local variables           
            List<NotificacionesCriterios> lista = null;

            // Checks if there is any linked record
            lista = (from c in Context.NotificacionesCriterios where c.NotificacionGrupoCriterioID == grupo && c.Activo select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                // Returns the result
                return lista;
            }
            else
            {
                lista = new List<NotificacionesCriterios>();
                // Returns the result
                return lista;
            }



        }
    }
}