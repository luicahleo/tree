using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeCore.Data;

namespace CapaNegocio
{
    public class Vw_EntidadesOperadoresController : GeneralBaseController<Vw_EntidadesOperadores, TreeCoreContext>
    {

        public List<Vw_EntidadesOperadores> GetActivos(long clienteID)
        {
            List<Vw_EntidadesOperadores> entidades;
            try
            {
                entidades = (from c in Context.Vw_EntidadesOperadores
                             where
                                 c.ClienteID == clienteID
                            orderby c.Nombre
                             select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                entidades = new List<Vw_EntidadesOperadores>();
            }
            return entidades;
        }
    }
}