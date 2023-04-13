using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class AlquileresHistoricosEstadosController : GeneralBaseController<AlquileresHistoricosEstados, TreeCoreContext>, IBasica<AlquileresHistoricosEstados>
    {
        public AlquileresHistoricosEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string NombreEstado, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresHistoricosEstados> datos = new List<AlquileresHistoricosEstados>();


            datos = (from c in Context.AlquileresHistoricosEstados where (c.NombreEstado == NombreEstado && c.ClienteID == clienteID) select c).ToList<AlquileresHistoricosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerHistoricoEstadoID)
        {
            AlquileresHistoricosEstados dato = new AlquileresHistoricosEstados();
            AlquileresHistoricosEstadosController cController = new AlquileresHistoricosEstadosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerHistoricoEstadoID == " + AlquilerHistoricoEstadoID.ToString());

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

        public long GetEstadoByCodigo(string cod)
        {

            long lID = 0;
            AlquileresHistoricosEstados alrHisEst = new AlquileresHistoricosEstados();

            try
            {
                alrHisEst = GetItem("CodigoEstado ==\"" + cod + "\"");
                lID = alrHisEst.AlquilerHistoricoEstadoID;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return lID;
            }

            return lID;
        }

        public AlquileresHistoricosEstados GetDefault(long lClienteID)
        {
            AlquileresHistoricosEstados oHistorico;

            try
            {
                oHistorico = (from c in Context.AlquileresHistoricosEstados where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oHistorico = null;
            }

            return oHistorico;
        }
    }
}