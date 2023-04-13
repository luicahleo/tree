using System;
using System.Collections.Generic;
using System.Data;
using TreeCore.Data;
using System.Linq;

namespace CapaNegocio
{
    public class ToolIntegracionesServiciosMetodosController : GeneralBaseController<ToolIntegracionesServiciosMetodos, TreeCoreContext>
    {
        public ToolIntegracionesServiciosMetodosController ()
            : base()
        { }

        public Vw_ToolServicios GetIntegracionActivaByNombreServicio (string servicio)
        {
            List<Vw_ToolServicios> lista = new List<Vw_ToolServicios>();

            lista = (from c in Context.Vw_ToolServicios where (c.Servicio == servicio && c.Activo==true) select c).ToList();

            if (lista.Count > 0)
            {
                return lista.ElementAt(0);
            }
            else
            {
                return null;
            }
        }

        public Vw_ToolServicios GetIntegracionByNombreServicio(string servicio)
        {
            List<Vw_ToolServicios> lista = new List<Vw_ToolServicios>();

            lista = (from c in Context.Vw_ToolServicios where (c.Servicio == servicio) select c).ToList();

            if (lista.Count > 0)
            {
                return lista.ElementAt(0);
            }
            else
            {
                return null;
            }
        }
        public bool existeRegistro(long pIntegracionID, long pServicioID, long pMetodoID)
        {
            bool existe = false;
            List<ToolIntegracionesServiciosMetodos> lista = new List<ToolIntegracionesServiciosMetodos>();

            lista = (from c in Context.ToolIntegracionesServiciosMetodos where (c.IntegracionID == pIntegracionID && c.ServicioID == pServicioID && c.MetodoID == pMetodoID) select c).ToList();

            if (lista.Count > 0)
            {
                existe = true;
            }

            return existe;
        }
        public string getMetodo(long metodoID)
        {
            //List<Vw_ToolIntegracionesServiciosMetodos> lista = new List<Vw_ToolIntegracionesServiciosMetodos>();

            string lista = (from c in Context.Vw_ToolIntegracionesServiciosMetodos where (c.MetodoID == metodoID) select c.Metodo).FirstOrDefault();

            return lista;
        }

        public ToolIntegracionesServiciosMetodos getItemByIntegracionID (long? lIntegracionID)
        {
            ToolIntegracionesServiciosMetodos oDato = null;

            try
            {
                oDato = (from c in Context.ToolIntegracionesServiciosMetodos where c.IntegracionID == lIntegracionID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }
    }
}