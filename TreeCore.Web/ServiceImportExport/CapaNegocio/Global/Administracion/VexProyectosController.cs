using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class VexProyectosController : GeneralBaseController<VexProyectos, TreeCoreContext>
    {
        public VexProyectosController()
            : base()
        { }

        public bool RegistroDuplicado(string txtCodigo,  string vexproyecto, long clienteID)
        {
            bool isExiste = false;
            List<VexProyectos> datos = new List<VexProyectos>();


            datos = (from c in Context.VexProyectos where (c.Codigo == txtCodigo || c.VexProyecto == vexproyecto && c.ClienteID == clienteID) select c).ToList<VexProyectos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}