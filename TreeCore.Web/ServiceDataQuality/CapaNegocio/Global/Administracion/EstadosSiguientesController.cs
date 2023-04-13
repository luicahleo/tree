using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EstadosSiguientesController : GeneralBaseController<CoreEstadosSiguientes, TreeCoreContext>
    {
        public EstadosSiguientesController()
               : base()
        { }

        public List<CoreEstadosSiguientes> getEstadosSiguientesByEstadoID(long lEstadoID)
        {
            List<CoreEstadosSiguientes> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool RegistroDuplicado(long lEstadoID, long lEstadoPosibleID)
        {
            bool isExiste = false;
            List<CoreEstadosSiguientes> datos = new List<CoreEstadosSiguientes>();

            datos = (from c in Context.CoreEstadosSiguientes where (c.CoreEstadoID == lEstadoID && c.CoreEstadoPosibleID == lEstadoPosibleID) select c).ToList<CoreEstadosSiguientes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreEstadosSiguientes GetDefault(long lEstadoID)
        {
            CoreEstadosSiguientes oDato;

            try
            {
                oDato = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID && c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public CoreEstadosSiguientes GetEstadoSiguiente (long lEstadoID, long lEstadoPosibleID)
        {
            CoreEstadosSiguientes oDato;

            try
            {
                oDato = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID && c.CoreEstadoPosibleID == lEstadoPosibleID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public bool RegistroDefecto(long lEstadoID, long lEstadoSiguienteID)
        {
            CoreEstadosSiguientes oDato;
            bool bDefecto = false;

            oDato = (from c in Context.CoreEstadosSiguientes where c.CoreEstadoID == lEstadoID && c.CoreEstadoSiguienteID == lEstadoSiguienteID && c.Defecto select c).First();

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }
    }
}