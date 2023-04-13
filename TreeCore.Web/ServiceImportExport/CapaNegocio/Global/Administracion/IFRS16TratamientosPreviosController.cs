using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class IFRS16TratamientosPreviosController : GeneralBaseController<IFRS16TratamientosPrevios, TreeCoreContext>, IBasica<IFRS16TratamientosPrevios>
    {
        public IFRS16TratamientosPreviosController()
            : base()
        { }

        public bool RegistroVinculado(long IFRS16TratamientoPrevioID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string IFRS16TratamientoPrevio, long clienteID)
        {
            bool isExiste = false;
            List<IFRS16TratamientosPrevios> datos = new List<IFRS16TratamientosPrevios>();


            datos = (from c in Context.IFRS16TratamientosPrevios where (c.IFRS16TratamientoPrevio == IFRS16TratamientoPrevio && c.ClienteID == clienteID) select c).ToList<IFRS16TratamientosPrevios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long IFRS16TratamientoPrevioID)
        {
            IFRS16TratamientosPrevios dato = new IFRS16TratamientosPrevios();
            IFRS16TratamientosPreviosController cController = new IFRS16TratamientosPreviosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && IFRS16TratamientoPrevioID == "  + IFRS16TratamientoPrevioID.ToString());

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

        public IFRS16TratamientosPrevios GetDefault(long clienteID) {
            IFRS16TratamientosPrevios oIFRS16TratamientosPrevios;
            try
            {
                oIFRS16TratamientosPrevios = (from c in Context.IFRS16TratamientosPrevios where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oIFRS16TratamientosPrevios = null;
            }
            return oIFRS16TratamientosPrevios;
        }
    }
}