using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTablasModeloDatosController : GeneralBaseController<TablasModeloDatos, TreeCoreContext>
    {
        public GlobalTablasModeloDatosController()
            : base()
        { }

        public bool RegistroDuplicadoByNombre(string NombreTabla)
        {
            bool isExiste = false;
            List<TablasModeloDatos> datos = new List<TablasModeloDatos>();


            datos = (from c in Context.TablasModeloDatos where c.NombreTabla == NombreTabla select c).ToList<TablasModeloDatos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<TablasModeloDatos> GetAllTablaModeloDatosActivos(long lTablaModeloID)
        {
            List<TablasModeloDatos> lista = new List<TablasModeloDatos>();
            lista = (from c in Context.TablasModeloDatos where c.Activo == true && c.TablaModeloDatosID != lTablaModeloID orderby c.NombreTabla ascending select c).ToList();

            return lista;

        }


    }
}