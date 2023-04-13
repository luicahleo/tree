using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class SAPIFRS16TratamientosPreviosController : GeneralBaseController<IFRS16TratamientosPrevios, TreeCoreContext>
    {
        public SAPIFRS16TratamientosPreviosController()
           : base()
        {

        }
        /// Devuelve si existe un departamento con igual nombre al que se desea insertar o editar
        /// </summary>
        public bool isExisteTratamientoPrevio(string TratamientoPrevio)
        {
            bool isExiteTratamientoPre = false;

            List<IFRS16TratamientosPrevios> ListaTratamientoPrevio = new List<IFRS16TratamientosPrevios>();

            ListaTratamientoPrevio = (from c in Context.IFRS16TratamientosPrevios where c.IFRS16TratamientoPrevio == TratamientoPrevio select c).ToList<IFRS16TratamientosPrevios>();
            if (ListaTratamientoPrevio.Count > 0)
            {
                isExiteTratamientoPre = true;
            }

            return isExiteTratamientoPre;
        }

        public IFRS16TratamientosPrevios GetTratamientoPrevioByNombre(string sNombre)
        {
            List<IFRS16TratamientosPrevios> lista = null;
            IFRS16TratamientosPrevios dato = null;

            try
            {

                lista = (from c in Context.IFRS16TratamientosPrevios where c.IFRS16TratamientoPrevio == sNombre && c.Activo == true select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }
    }
}