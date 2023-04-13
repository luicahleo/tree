using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MonitoringCargasMasivasController : GeneralBaseController<MonitoringCargasMasivas, TreeCoreContext>
    {
        public MonitoringCargasMasivasController()
            : base()
        { }
        public bool RegistroDefecto(long MonitoringCargaMasivaID)
        {
            MonitoringCargasMasivas oDato;
            MonitoringCargasMasivasController cController = new MonitoringCargasMasivasController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && MonitoringCargaMasivaID == " + MonitoringCargaMasivaID.ToString());

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
        public List<Vw_MonitoringCargasMasivas> GetAllPorFechas(DateTime FechaInicio, DateTime FechaFin)
        {
            List<Vw_MonitoringCargasMasivas> lista = new List<Vw_MonitoringCargasMasivas>();

            lista = (from c in Context.Vw_MonitoringCargasMasivas where c.FechaCarga >= FechaInicio && c.FechaCarga <= FechaFin select c).ToList<Vw_MonitoringCargasMasivas>();

            return lista;
        }

        public bool GenerarEstadisticaCarga(long UsuarioID, int Numcorrectos, int Numerrores, string Tipocarga, string archivodir)
        {
            bool res = false;

            try
            {
                MonitoringCargasMasivas CargaEst = new MonitoringCargasMasivas();


                CargaEst.UsuarioID = UsuarioID;
                CargaEst.FechaCarga = DateTime.Now;
                CargaEst.NumCorrectos = Numcorrectos;
                CargaEst.NumErroneos = Numerrores;
                CargaEst.TipoCarga = Tipocarga;
                CargaEst.Documento = archivodir.ToString();

                AddItem(CargaEst);
            }
            catch (Exception ex)
            {
                res = false;
                log.Error(ex.Message);
            }
            return res;
        }
    }
}