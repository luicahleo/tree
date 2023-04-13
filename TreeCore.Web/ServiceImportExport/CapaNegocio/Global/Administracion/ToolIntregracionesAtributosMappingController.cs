using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ToolIntregracionesAtributosMappingController : GeneralBaseController<ToolIntregracionesAtributosMapping, TreeCoreContext>
    {
        public ToolIntregracionesAtributosMappingController()
            : base()
        { }

        public bool RegistroVinculado(long ToolIntegracionAtrbitutoMappingID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string AtributoMaximo)
        {
            bool isExiste = false;
            List<ToolIntregracionesAtributosMapping> datos = new List<ToolIntregracionesAtributosMapping>();


            datos = (from c in Context.ToolIntregracionesAtributosMapping where (c.AtributoMaximo == AtributoMaximo) select c).ToList<ToolIntregracionesAtributosMapping>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool ComprobarDuplicadoAtributoTree(string AtributoTree, bool Agregar, long ToolIntegracionesAtributoID)
        {
            bool isExiste = false;
            List<ToolIntregracionesAtributosMapping> datos = new List<ToolIntregracionesAtributosMapping>();


            datos = (from c in Context.ToolIntregracionesAtributosMapping where (c.AtributoTree == AtributoTree) select c).ToList<ToolIntregracionesAtributosMapping>();

            if (Agregar)
            {
                if (datos.Count > 0)
                {
                    isExiste = true;
                }
            }
            else
            {
                if (datos.Count >= 1 && datos.First().ToolIntegracionAtrbitutoMappingID != ToolIntegracionesAtributoID)
                {
                    isExiste = true;
                }
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ToolIntegracionAtrbitutoMappingID)
        {
            ToolIntregracionesAtributosMapping dato = new ToolIntregracionesAtributosMapping();
            ToolIntregracionesAtributosMappingController cController = new ToolIntregracionesAtributosMappingController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ToolIntegracionAtrbitutoMappingID == " + ToolIntegracionAtrbitutoMappingID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }
    }
}