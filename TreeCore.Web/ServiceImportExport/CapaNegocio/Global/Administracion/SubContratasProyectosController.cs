using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class SubContratasProyectosController : GeneralBaseController<Vw_SubcontratasProyectos, TreeCoreContext>
    {
        public SubContratasProyectosController()
            : base()
        { }

        public List<Vw_SubcontratasProyectos> GetSubcontratasProyectosbyIDEmpresaPrincipal(long? id)
        {
            List<Vw_SubcontratasProyectos> lista = new List<Vw_SubcontratasProyectos>();
            lista = (from c in Context.Vw_SubcontratasProyectos where c.ContrataPrincipalID == id select c).ToList();
            return lista;
        }

        public List<Vw_SubcontratasProyectos> GetSubcontratasProyectosbyIDEmpresaSubcontrata(long? id)
        {
            List<Vw_SubcontratasProyectos> lista = new List<Vw_SubcontratasProyectos>();
            lista = (from c in Context.Vw_SubcontratasProyectos where c.SubcontrataID == id select c).ToList();
            return lista;
        }
    }
}