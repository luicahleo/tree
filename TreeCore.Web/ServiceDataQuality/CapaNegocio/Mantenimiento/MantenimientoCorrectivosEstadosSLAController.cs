using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoCorrectivosEstadosSLAController : GeneralBaseController<MantenimientoCorrectivosEstadosSLA, TreeCoreContext>
    {
        public MantenimientoCorrectivosEstadosSLAController()
            : base()
        {

        }

        public long getadqestslaid(string EstSLA)
        {

            long datos = new long();
            try
            {

                datos = (from c in Context.MantenimientoCorrectivosEstadosSLA where c.EstadoSLA == EstSLA select c.MantenimientoCorrectivoEstadoSLAID).First();
            }
            catch (Exception ex)
            {
                datos = new long();
                log.Error(ex.Message);
            }

            return datos;
        }

        public bool TieneRegistrosAsociados(long tipoID)
        {

            bool tiene = false;
            MantenimientoCorrectivosEmplazamientosEstadosSLAController cEmplazamientos = new MantenimientoCorrectivosEmplazamientosEstadosSLAController();
            List<MantenimientoCorrectivosEmplazamientosEstadosSLA> datos = new List<MantenimientoCorrectivosEmplazamientosEstadosSLA>();
            datos = cEmplazamientos.GetItemsList("MantenimientoCorrectivosEstadoSLAID == " + tipoID.ToString());
            if (datos.Count > 0)
            {
                tiene = true;
            }

            return tiene;

        }
        public MantenimientoCorrectivosEstadosSLA GetDefault()
        {
            MantenimientoCorrectivosEstadosSLA estado;

            try
            {
                estado = (from c 
                          in Context.MantenimientoCorrectivosEstadosSLA 
                          where c.Defecto 
                          select c).FirstOrDefault();

                if (estado == null)
                {
                    estado = (from c 
                              in Context.MantenimientoCorrectivosEstadosSLA 
                              orderby c.MantenimientoCorrectivoEstadoSLAID 
                              select c).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                estado = null;
                log.Error(ex.Message);
            }
            return estado;
        }

        public long getmttoestadoslaid(string estado)
        {
            long datos = new long();
            try
            {
                datos = (from c in Context.MantenimientoCorrectivosEstadosSLA where Convert.ToString(c.MantenimientoCorrectivosEmplazamientosEstadosSLA) == estado select c.MantenimientoCorrectivoEstadoSLAID).First();
            }
            catch (Exception ex)
            {
                datos = new long();
                log.Error(ex.Message);
            }

            return datos;
        }



        public MantenimientoCorrectivosEstadosSLA EstadoImplicaParadaSLA()
        {
            List<MantenimientoCorrectivosEstadosSLA> datos = null;
            datos = GetItemsList("ImplicaParadaSLA==true");
            if (datos.Count > 0)
            {
                return datos[0];
            }
            else
            {
                datos = GetItemsList("", "MantenimientoCorrectivosEstadoSLAID");
                if (datos.Count > 0)
                {
                    return datos[0];
                }
                else
                {
                }
            }
            return null;
        }


    }
}
