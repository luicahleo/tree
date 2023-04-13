using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class VexSeccionesController : GeneralBaseController<VexSecciones, TreeCoreContext>
    {
        public VexSeccionesController()
            : base()
        { }
        public bool RegistroDuplicado(string CodigoSeccion, string Seccion, long moduloID)
        {
            bool isExiste = false;
            List<VexSecciones> datos;


            datos = (from c in Context.VexSecciones where (c.CodigoSeccion == CodigoSeccion || c.VexSeccion == Seccion) && c.VexProyectoID == moduloID select c).ToList<VexSecciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}