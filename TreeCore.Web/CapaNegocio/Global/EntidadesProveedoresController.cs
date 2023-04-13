using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EntidadesProveedoresController : GeneralBaseController<Vw_EntidadesProveedores, TreeCoreContext>
    {
        public EntidadesProveedoresController()
            : base()
        { }


        public List<Vw_EntidadesProveedores> GetAllProveedores()
        {
            // Local variables
            List<Vw_EntidadesProveedores> lista = null;
            try
            {
                lista = (from c in Context.Vw_EntidadesProveedores select c).ToList<Vw_EntidadesProveedores>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<Vw_EntidadesProveedores> GetAllEntidadesByClienteID(long CliID)
        {
            // Local variables
            List<Vw_EntidadesProveedores> lista = null;
            try
            {
                lista = (from c in Context.Vw_EntidadesProveedores where c.ClienteID == CliID select c).ToList<Vw_EntidadesProveedores>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        

    }
}