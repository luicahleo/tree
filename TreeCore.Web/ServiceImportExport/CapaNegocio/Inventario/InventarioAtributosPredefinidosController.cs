using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
//using Microsoft.Ajax.Utilities;

namespace CapaNegocio
{
    public class InventarioAtributosPredefinidosController : GeneralBaseController<InventarioAtributosPredefinidos, TreeCoreContext>
    {
        public InventarioAtributosPredefinidosController()
            : base()
        { }

        public bool RegistroVinculado(long lInventarioAtributoPredefinidoID)
        {
            bool bExiste = true;

            return bExiste;
        }

        public bool RegistroDuplicado(string sInventarioAtributoPredefinido, string sCodigoAtributo, long lClienteID)
        {
            bool bExiste = false;
            List<Vw_InventarioAtributosPredefinidos> listaDatos = new List<Vw_InventarioAtributosPredefinidos>();


            listaDatos = (from c in Context.Vw_InventarioAtributosPredefinidos where ((c.NombreAtributo == sInventarioAtributoPredefinido || c.CodigoAtributo == sCodigoAtributo) && c.ClienteID == lClienteID) select c).ToList<Vw_InventarioAtributosPredefinidos>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDefecto(long lInventarioAtributoPredefinidoID)
        {
            InventarioAtributosPredefinidos oDato = new InventarioAtributosPredefinidos();
            InventarioAtributosPredefinidosController cController = new InventarioAtributosPredefinidosController();
            bool bDefecto = false;

            oDato = cController.GetItem("CrearPorDefecto == true && InventarioAtributoPredefinidoID == " + lInventarioAtributoPredefinidoID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        public bool RegistroDuplicado_NombreCodigo (string sNombre, string sCodigo)
        {
            bool bExiste = false;
            List<InventarioAtributosPredefinidos> listaDatos = new List<InventarioAtributosPredefinidos>();

            listaDatos = (from c in Context.InventarioAtributosPredefinidos where c.NombreAtributo == sNombre && c.CodigoAtributo == sCodigo select c).ToList();
            
            if (listaDatos != null)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public int GetMaximoOrden()
        {
            int iDato = -1;

            try
            {
                iDato = (from c in Context.InventarioAtributosPredefinidos select c.Orden).Max();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return iDato;
        }

        public InventarioAtributosPredefinidos GetDefault(long lClienteID)
        {
            InventarioAtributosPredefinidos banco;
            try
            {
                banco = (from c in Context.InventarioAtributosPredefinidos where c.CrearPorDefecto && (c.ClienteID == lClienteID || c.ClienteID == null) select c).First();
            }
            catch (Exception ex)
            {
                banco = null;
                log.Error(ex.Message);
            }
            return banco;
        }
    }
}