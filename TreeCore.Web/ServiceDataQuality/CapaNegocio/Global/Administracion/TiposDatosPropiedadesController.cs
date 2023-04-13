using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class TiposDatosPropiedadesController : GeneralBaseController<TiposDatosPropiedades, TreeCoreContext>
    {
        public TiposDatosPropiedadesController()
            : base()
        { }

        public bool ComprobarDuplicadosPorTipoPropiedad(long TipoDatoID, string Propiedad)
        {
            bool bTieneDuplicidad = false;

            TiposDatosPropiedades dato = (from tdp in Context.TiposDatosPropiedades
                                          where tdp.TipoDatoID == TipoDatoID && tdp.Codigo == Propiedad
                                          select tdp).FirstOrDefault();

            if (dato != null)
                bTieneDuplicidad = true;

            return bTieneDuplicidad;
        }

    }
}