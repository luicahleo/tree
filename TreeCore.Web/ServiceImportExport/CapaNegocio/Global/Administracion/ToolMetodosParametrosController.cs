using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ToolMetodosParametrosController : GeneralBaseController<ToolMetodosParametros, TreeCoreContext>
    {
        public ToolMetodosParametrosController()
            : base()
        { }

        public bool getOrdenRepetido(int Orden, long MetodoID)
        {
            return (((from c in Context.Vw_ToolMetodosParametros where c.MetodoID == MetodoID && c.Orden == Orden select c).ToList<Vw_ToolMetodosParametros>()).Count > 0);


        }

        public bool RegistroDuplicado(int Orden, long cliID, long MetodoID)
        {
            bool isExiste = false;
            List<ToolMetodosParametros> datos = new List<ToolMetodosParametros>();


            datos = (from c in Context.ToolMetodosParametros where (c.ClienteID == cliID && c.Orden == Orden && c.MetodoID == MetodoID) select c).ToList<ToolMetodosParametros>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicado(int Orden, long MetodoID)
        {
            bool isExiste = false;
            List<ToolMetodosParametros> datos = new List<ToolMetodosParametros>();


            datos = (from c in Context.ToolMetodosParametros where (c.Orden == Orden && c.MetodoID == MetodoID) select c).ToList<ToolMetodosParametros>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public long GetNumeroParametros(long MetodoID) {
            try
            {
                return (from c in Context.Vw_ToolMetodosParametros where c.MetodoID == MetodoID select c).ToList().Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }

    }
}