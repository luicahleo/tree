using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class FuncionalidadesTiposController : GeneralBaseController<FuncionalidadesTipos, TreeCoreContext>
    {
        public FuncionalidadesTiposController()
            : base()
        { }


        public List<FuncionalidadesTipos> GetActivos()
        {
            // Local variables
            List<FuncionalidadesTipos> lista = null;
            try
            {
                lista = (from c in Context.FuncionalidadesTipos where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public bool RegistroDuplicadoNombre(string funcionalidadTipo)
        {
            bool isExiste = false;
            List<FuncionalidadesTipos> datos = new List<FuncionalidadesTipos>();


            datos = (from c in Context.FuncionalidadesTipos where (c.Nombre == funcionalidadTipo) select c).ToList<FuncionalidadesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoCodigo(string codigo)
        {
            bool isExiste = false;
            List<FuncionalidadesTipos> datos = new List<FuncionalidadesTipos>();


            datos = (from c in Context.FuncionalidadesTipos where (c.Codigo == codigo) select c).ToList<FuncionalidadesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}