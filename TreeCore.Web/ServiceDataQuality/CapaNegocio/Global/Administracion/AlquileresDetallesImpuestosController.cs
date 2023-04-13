using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public sealed class AlquileresDetallesImpuestosController : GeneralBaseController<AlquileresDetallesImpuestos, TreeCoreContext>
    {
        public AlquileresDetallesImpuestosController()
            : base()
        {

        }

        public List<long> GetAllImpuestosDetallesID(long alquilerDetalleID)
        {

            List<long> datos = null;
            datos = (from c in Context.AlquileresDetallesImpuestos where c.AlquilerDetalleID == alquilerDetalleID select c.ImpuestoID).ToList();


            return datos;
        }


        public List<AlquileresDetallesImpuestos> GetAllImpuestosDetallesListID(long alquilerDetalleID)
        {

            List<AlquileresDetallesImpuestos> datos = null;
            datos = (from c in Context.AlquileresDetallesImpuestos where c.AlquilerDetalleID == alquilerDetalleID select c).ToList();


            return datos;
        }

        public List<Vw_AlquileresDetallesImpuestos> GetAllImpuestosDetallesVista(long alquilerDetalleID)
        {

            List<Vw_AlquileresDetallesImpuestos> datos = null;
            datos = (from c in Context.Vw_AlquileresDetallesImpuestos where c.AlquilerDetalleID == alquilerDetalleID select c).ToList();


            return datos;
        }


        public bool RemoveAllByAlquilerDetalle(long alquilerDetalleID)
        {
            // Local variables
            bool bRes = false;

            // Removes all the records for a give requirement
            List<AlquileresDetallesImpuestos> datos = null;
            try
            {
                datos = (from c in Context.AlquileresDetallesImpuestos where c.AlquilerDetalleID == alquilerDetalleID select c).ToList();

                if (datos != null && datos.Count > 0)
                {
                    foreach (AlquileresDetallesImpuestos alq in datos)
                    {
                        DeleteItem(alq.AlquilerDetalleImpuestoID);
                    }
                    bRes = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bRes;
        }

        

        public bool HasImpuestoDetalleDuplicado(long alquilerDetalleID, long impuestoID)
        {
            // Local variables
            bool bRes = false;

            // Removes all the records for a give requirement
            List<AlquileresDetallesImpuestos> datos = null;
            try
            {
                datos = (from c in Context.AlquileresDetallesImpuestos where c.AlquilerDetalleID == alquilerDetalleID && c.ImpuestoID == impuestoID select c).ToList();

                if (datos != null && datos.Count > 0)
                {
                    bRes = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bRes;
        }


        public AlquileresDetallesImpuestos GetItemByDetalleIDImpuesto(long alquilerDetalleID, long impuestoID)
        {

            List<AlquileresDetallesImpuestos> lista = null;
            AlquileresDetallesImpuestos datos = null;

            try
            {
                lista = (from c in Context.AlquileresDetallesImpuestos where c.AlquilerDetalleID == alquilerDetalleID && c.ImpuestoID == impuestoID select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    datos = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return datos;
        }


    }
}


