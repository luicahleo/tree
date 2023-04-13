using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ImpuestosDetallesController : GeneralBaseController<ImpuestosDetalles, TreeCoreContext>
    {
        public ImpuestosDetallesController()
            : base()
        { }

        public bool RegistroDuplicadoDetalle(long Valor, long ImpuestoID)
        {
            bool isExiste = false;
            List<ImpuestosDetalles> datos;


            datos = (from c in Context.ImpuestosDetalles where (c.Valor == Valor && c.ImpuestoID == ImpuestoID) select c).ToList<ImpuestosDetalles>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public ImpuestosDetalles GetDefault(long impuestoID)
        {
            ImpuestosDetalles impuestoDetalle;
            try
            {
                impuestoDetalle = (from c
                           in Context.ImpuestosDetalles
                           where c.Defecto &&
                                 c.ImpuestoID == impuestoID
                           select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                impuestoDetalle = null;
            }
            return impuestoDetalle;
        }

      
    }
}