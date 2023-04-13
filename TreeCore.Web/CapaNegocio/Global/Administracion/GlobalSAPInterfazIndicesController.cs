using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class SAPInterfazIndicesController : GeneralBaseController<SAPInterfazIndices, TreeCoreContext>, IBasica<SAPInterfazIndices>
    {
        public SAPInterfazIndicesController()
            : base()
        { }

        public bool RegistroVinculado(long SAPInterfazIndiceID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SAPInterfazIndice, long clienteID)
        {
            bool isExiste = false;
            List<SAPInterfazIndices> datos = new List<SAPInterfazIndices>();


            datos = (from c in Context.SAPInterfazIndices where (c.NombreIndice == SAPInterfazIndice) select c).ToList<SAPInterfazIndices>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPInterfazIndiceID)
        {
            SAPInterfazIndices dato = new SAPInterfazIndices();
            SAPInterfazIndicesController cController = new SAPInterfazIndicesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPInterfazIndiceID == " + SAPInterfazIndiceID.ToString());

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