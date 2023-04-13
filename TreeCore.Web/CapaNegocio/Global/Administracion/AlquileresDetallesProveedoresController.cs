using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class AlquileresDetallesProveedoresController : GeneralBaseController<AlquileresDetallesProveedores, TreeCoreContext>
    {
        public AlquileresDetallesProveedoresController()
            : base()
        {

        }

        public List<long> GetAllProveedoresID(long alquilerDetalleID)
        {

            List<long> datos = null;
            datos = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID select c.ProveedorID).ToList();


            return datos;
        }

        public List<Vw_AlquileresDetallesProveedores> GetAllByAlquilerDetalleID(long alquilerDetalleID)
        {

            List<Vw_AlquileresDetallesProveedores> datos = null;
            try
            {
                datos = (from c in Context.Vw_AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return datos;
        }

        public List<AlquileresDetallesProveedores> GetTablaAllByAlquilerDetalleID(long alquilerDetalleID)
        {

            List<AlquileresDetallesProveedores> datos = null;
            try
            {
                datos = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return datos;
        }


        public bool RemoveAllByAlquilerDetalle(long alquilerDetalleID)
        {
            // Local variables
            bool bRes = false;

            // Removes all the records for a give requirement
            List<AlquileresDetallesProveedores> datos = null;
            try
            {
                datos = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID select c).ToList();

                if (datos != null && datos.Count > 0)
                {
                    foreach (AlquileresDetallesProveedores alq in datos)
                    {
                        DeleteItem(alq.AlquilerDetalleProveedorID);
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

        

        public bool HasProveedorDuplicado(long alquilerDetalleID, long proveedorID, long conceptoPagoID, long monedaID, double cantidad)
        {
            // Local variables
            bool bRes = false;

            // Removes all the records for a give requirement
            List<AlquileresDetallesProveedores> datos = null;
            List<AlquileresDetallesProveedores> alquileresDetallesProveedores = new List<AlquileresDetallesProveedores>();
            double sum = 0;
            try
            {
                //datos = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && c.ProveedorID == proveedorID && c.ConceptoPagoID == conceptoPagoID && c.MonedaID == monedaID && c.Activo && !c.EnNegociacion select c).ToList();
                datos = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && c.ProveedorID == proveedorID && c.Activo && !c.EnNegociacion select c).ToList();
                if (datos != null && datos.Count > 0)
                {
                    bRes = true;
                }
                else
                {
                    alquileresDetallesProveedores = new List<AlquileresDetallesProveedores>();

                    alquileresDetallesProveedores = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && c.Activo && !c.EnNegociacion select c).ToList();
                    if (alquileresDetallesProveedores != null && alquileresDetallesProveedores.Count > 0)
                    {
                        sum = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && c.Activo && !c.EnNegociacion select c.CantidadPorcentaje).Sum();

                        if (sum + cantidad > 100)
                        {
                            bRes = true;
                        }

                    }
                    else
                    {
                        if (cantidad > 100)
                        {
                            bRes = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bRes;
        }

        public bool HasProveedorDuplicadoEnNegociacion(long alquilerDetalleID, long proveedorID, long conceptoPagoID, long monedaID, double cantidad)
        {
            // Local variables
            bool bRes = false;

            // Removes all the records for a give requirement
            List<AlquileresDetallesProveedores> datos = null;
            List<AlquileresDetallesProveedores> alquileresDetallesProveedores = new List<AlquileresDetallesProveedores>();
            double sum = 0;
            try
            {
                datos = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && c.ProveedorID == proveedorID && c.ConceptoPagoID == conceptoPagoID && c.MonedaID == monedaID && (c.EnNegociacion || (!c.EnNegociacion && !c.PendienteBorrar)) select c).ToList();

                if (datos != null && datos.Count > 0)
                {
                    bRes = true;
                }
                else
                {
                    alquileresDetallesProveedores = new List<AlquileresDetallesProveedores>();

                    alquileresDetallesProveedores = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && (c.EnNegociacion || (!c.EnNegociacion && !c.PendienteBorrar)) select c).ToList();
                    if (alquileresDetallesProveedores != null && alquileresDetallesProveedores.Count > 0)
                    {
                        sum = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && (c.EnNegociacion || (!c.EnNegociacion && !c.PendienteBorrar)) select c.CantidadPorcentaje).Sum();

                        if (sum + cantidad > 100)
                        {
                            bRes = true;
                        }

                    }
                    else
                    {
                        if (cantidad > 100)
                        {
                            bRes = true;
                        }

                    }




                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bRes;
        }

        public bool CompruebaSumCantidadPorcentaje(long alquilerDetalleID, double cantidad)
        {
            // Local variables
            bool bRes = false;

            // Removes all the records for a give requirement

            double sum = 0;
            try
            {

                sum = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID select c.CantidadPorcentaje).Sum();

                if (sum + cantidad > 100)
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

        public List<Vw_AlquileresDetallesProveedores> GetAlquilerDetalleProveedorByconcepto(long ConceptoID)
        {
            List<Vw_AlquileresDetallesProveedores> alquileres = new List<Vw_AlquileresDetallesProveedores>();

            alquileres = (from c in Context.Vw_AlquileresDetallesProveedores where c.AlquilerDetalleID == ConceptoID select c).ToList();
            if (alquileres.Count > 0)
                return alquileres;
            else
                return null;

        }

        public List<Vw_AlquileresDetallesProveedores> GetDetallesByAlquilerBeneficiariosV2(long alquilerID)
        {
            List<Vw_AlquileresDetallesProveedores> lista = null;

            try
            {

                lista = (from c in Context.Vw_AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerID  select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<Vw_AlquileresDetallesProveedores>();
                return lista;
            }

            return lista;
        }

        public AlquileresDetallesProveedores GetItemByDetalleIDProveedor(long alquilerDetalleID, long proveedorID)
        {

            List<AlquileresDetallesProveedores> lista = null;
            AlquileresDetallesProveedores datos = null;

            try
            {
                lista = (from c in Context.AlquileresDetallesProveedores where c.AlquilerDetalleID == alquilerDetalleID && c.ProveedorID == proveedorID select c).ToList();
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

        #region CALIDAD

        public int GetCalidad(string sFiltro)
        {
            // Local variables
            int iResultado = 0;
            List<AlquileresDetallesProveedores> lista = null;

            try
            {
                lista = GetItemsList(sFiltro);
                if (lista != null)
                {
                    iResultado = lista.Count;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return iResultado;
        }

        public int GetCalidadVista(string sFiltro)
        {
            // Local variables
            int iResultado = 0;
            List<Vw_AlquileresDetallesProveedores> lista = null;

            try
            {
                lista = GetItemsList<Vw_AlquileresDetallesProveedores>(sFiltro);
                if (lista != null)
                {
                    iResultado = lista.Count;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return iResultado;
        }

        #endregion

    }
}

