using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class IntegracionesController : GeneralBaseController<ToolIntegraciones, TreeCoreContext>
    {
        public IntegracionesController()
            : base()
        { }       

        public List<ToolIntegraciones> GetActivos()
        {
            List<ToolIntegraciones> listaToolIntegraciones;

            try
            {
                listaToolIntegraciones = (from c in Context.ToolIntegraciones 
                                          where c.Activo 
                                          orderby c.Integracion 
                                          select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaToolIntegraciones = null;
            }

            return listaToolIntegraciones;
        }
        public bool RegistroDuplicado(string Codigo, long clienteID)
        {
            bool isExiste = false;
            List<ToolIntegraciones> datos = new List<ToolIntegraciones>();


            datos = (from c in Context.ToolIntegraciones where (c.Codigo == Codigo && c.ClienteID == clienteID) select c).ToList<ToolIntegraciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}