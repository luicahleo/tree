using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class ProveedoresFuncionesController : GeneralBaseController<ProveedoresFunciones, TreeCoreContext>
    {
        public ProveedoresFuncionesController()
            : base()
        {

        }

        #region GESTION BASICA

        public ProveedoresFunciones GetFuncionByCodigo(string sNombre)
        {
            List<ProveedoresFunciones> lista = null;
            ProveedoresFunciones dato = null;

            try
            {

                lista = (from c in Context.ProveedoresFunciones where c.Codigo == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return dato;
        }


        #endregion
    }
}
