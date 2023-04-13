using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EntidadesOperadoresController : GeneralBaseController<Entidades, TreeCoreContext>
    {
        public EntidadesOperadoresController()
            : base()
        { }


        public List<Vw_EntidadesOperadores> GetAllOperadores()
        {
            // Local variables
            List<Vw_EntidadesOperadores> lista = null;
            try
            {
                lista = (from c in Context.Vw_EntidadesOperadores select c).ToList<Vw_EntidadesOperadores>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<Vw_EntidadesOperadores> GetAllEntidadesByClienteID(long CliID)
        {
            // Local variables
            List<Vw_EntidadesOperadores> lista = null;
            try
            {
                //lista = (from c in Context.Vw_EntidadesOperadores where c.ClienteID == CliID select c).ToList<Vw_EntidadesOperadores>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<Vw_EntidadesOperadores> GetActivos(long clienteID)
        {
            List<Vw_EntidadesOperadores> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_EntidadesOperadores where  c.ClienteID == clienteID orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }



    }
}