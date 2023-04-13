using System;
using System.Collections.Generic;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoCorrectivosEmplazamientosEstadosSLAController : GeneralBaseController<MantenimientoCorrectivosEmplazamientosEstadosSLA, TreeCoreContext>
    {
        public MantenimientoCorrectivosEmplazamientosEstadosSLAController()
            : base()
        {

        }

       


        /// <summary>
        /// Se agrega un estado a un emplazamiento
        /// Se actualiza el registro del emplazamiento para asignar el nuevo estado SLA asociado al mismo
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public override MantenimientoCorrectivosEmplazamientosEstadosSLA AddItem(MantenimientoCorrectivosEmplazamientosEstadosSLA item)
        {
            MantenimientoCorrectivosEmplazamientosEstadosSLA dato = base.AddItem(item);


            if (!ActualizarEstadoSLAEnEmplazamiento(dato.MantenimientoCorrectivoEmplazamientoID, dato.MantenimientoCorrectivoEmplazamientoEstadoSLAID))
            {
            }

            return dato;
        }

        /// <summary>
        /// Actualiza el estado SLA en un emplazamiento
        /// </summary>
        /// <param name="EmplazamientoID"></param>
        /// <param name="EmplazamientoSeguimientoID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ActualizarEstadoSLAEnEmplazamiento(long MantenimientoCorrectivosEmplazamientoID, long? MantenimientoCorrectivosEmplazamientoEstadoSLAID)
        {
            try
            {
                MantenimientoEmplazamientoCorrectivo emp;
                MantenimientoEmplazamientosCorrectivosController cMantenimientoCorrectivosEmplazamientos = new MantenimientoEmplazamientosCorrectivosController();

                emp = cMantenimientoCorrectivosEmplazamientos.GetItem(MantenimientoCorrectivosEmplazamientoID);
                if ((emp != null))
                {
                    emp.MantenimientoCorrectivoEmplazamientoEstadoSLAID = MantenimientoCorrectivosEmplazamientoEstadoSLAID.Value;
                    if (!cMantenimientoCorrectivosEmplazamientos.UpdateItem(emp))
                    {
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;


        }


        /// <summary>
        /// Actualización de un seguimiento del emplazamiento
        /// La lógica a aplicar es que, en caso de que se desactive o se reactive un seguimiento hay que actualizar el seguimiento actual en el emplazamiento
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool UpdateItem(MantenimientoCorrectivosEmplazamientosEstadosSLA item)
        {
            bool res = base.UpdateItem(item);

            try
            {
                // obtenemos el ultimo seguimento activo
                long? Ultimo = UltimoSeguimientoDeEmplazamiento(item.MantenimientoCorrectivoEmplazamientoID, item.MantenimientoCorrectivoEmplazamientoEstadoSLAID);
                // actualizamos el seguimiento
                if (!ActualizarEstadoSLAEnEmplazamiento(item.MantenimientoCorrectivoEmplazamientoID, Ultimo))
                {
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return res;

        }


        /// <summary>
        /// Devuelve el identificador del ultimo estado SLA disponible (activo) para un emplazamiento
        /// </summary>
        /// <param name="EmplazamientoID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public long? UltimoSeguimientoDeEmplazamiento(long MantenimientoCorrectivosEmplazamientoID, long EmplazamientoEstadoSLAID)
        {

            long? res = null;
            List<MantenimientoCorrectivosEmplazamientosEstadosSLA> datos = new List<MantenimientoCorrectivosEmplazamientosEstadosSLA>();
            datos = GetItemsList("MantenimientoCorrectivosEmplazamientoID == " + MantenimientoCorrectivosEmplazamientoID.ToString() + " && Activo", "Fecha DESC");

            if (datos.Count > 0)
            {
                res = datos[0].MantenimientoCorrectivoEmplazamientoEstadoSLAID;
            }
            return res;
        }
        public long? UltimoSeguimientoDeEmplazamiento(long MantenimientoCorrectivosEmplazamientoID)
        {

            long? res = null;
            List<MantenimientoCorrectivosEmplazamientosEstadosSLA> datos = new List<MantenimientoCorrectivosEmplazamientosEstadosSLA>();
            datos = GetItemsList("MantenimientoCorrectivosEmplazamientoID == " + MantenimientoCorrectivosEmplazamientoID.ToString() + " && Activo", "Fecha DESC");

            if (datos.Count > 0)
            {
                res = datos[0].MantenimientoCorrectivoEmplazamientoEstadoSLAID;
            }
            return res;
        }




    }
}
