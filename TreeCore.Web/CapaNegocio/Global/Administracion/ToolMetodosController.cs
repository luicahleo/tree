using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ToolMetodosController : GeneralBaseController<ToolMetodos, TreeCoreContext> 
    {
        public ToolMetodosController()
            : base()
        { }

        public bool RegistroDefecto(long id)
        {
            throw new NotImplementedException();
        }

        public bool RegistroDuplicado(string metodo, long clienteID)
        {
            bool isExiste = false;
            List<ToolMetodos> datos = new List<ToolMetodos>();


            datos = (from c in Context.ToolMetodos where (c.Metodo == metodo && c.ClienteID == clienteID) select c).ToList<ToolMetodos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }



    }
}