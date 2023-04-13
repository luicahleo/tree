using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class TiposDatosOperadoresController : GeneralBaseController<TiposDatosOperadores, TreeCoreContext>
    {
        public TiposDatosOperadoresController()
            : base()
        { }

        public bool ComprobarDuplicadosPorNombre(long TipoDatoID, string Nombre)
        {
            bool bTieneDuplicidad = false;

            TiposDatosOperadores dato = (from tdp in Context.TiposDatosOperadores
                                        where tdp.TipoDatoID == TipoDatoID && tdp.Nombre == Nombre
                                         select tdp).FirstOrDefault();

            if (dato != null)
                bTieneDuplicidad = true;

            return bTieneDuplicidad;
        }

        public List<Vw_TiposDatosOperadores> getAllOperadoresActivos()
        {
            List<Vw_TiposDatosOperadores> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_TiposDatosOperadores where c.Activo == true select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_TiposDatosOperadores> getOperadorByTipoDato (long lTipoDatoID)
        {
            List<Vw_TiposDatosOperadores> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_TiposDatosOperadores where c.TipoDatoID == lTipoDatoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public long getIDByNombre (string sOperador)
        {
            long lTipoDatoID;

            try
            {
                lTipoDatoID = (from c in Context.TiposDatosOperadores where (c.Operador == sOperador || c.Nombre == sOperador) select c.TipoDatoOperadorID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lTipoDatoID = 0;
            }

            return lTipoDatoID;
        }
    }
}