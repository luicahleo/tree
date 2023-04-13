using System;
using System.Collections.Generic;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosController : GeneralBaseController<MantenimientoEmplazamientos, TreeCoreContext>
    {
       public MantenimientoEmplazamientosController()
            : base()
        { }

        #region NOTIFICACIONES INTERFACE

        public string GetListaByFiltro(string sFiltro, string sVisualiza)
        {
            // Local variables
            List<Vw_MantenimientoEmplazamientos> lista = null;
            string sResultado = null;

            // Invokes the method
            try
            {
                lista = GetItemsList<Vw_MantenimientoEmplazamientos>(sFiltro);

                if (lista != null && lista.Count > 0)
                {

                    // Creates the header
                    object propertyValue = null;
                    List<string> cabecera = null;
                    cabecera = sVisualiza.Split(';').ToList();
                    sResultado = "<table style=\"border: 1px solid #999\">" + Comun.NuevaLinea;
                    sResultado = sResultado + "<tr>" + Comun.NuevaLinea;
                    if (cabecera != null && cabecera.Count > 0)
                    {
                        foreach (string nuevaCabecera in cabecera)
                        {
                            sResultado = sResultado + "<th style=\"font-weight: bold; background: #0061ac; color: white\">" + nuevaCabecera + "</th>" + Comun.NuevaLinea;
                        }

                        sResultado = sResultado + "</tr>" + Comun.NuevaLinea;

                        foreach (Vw_MantenimientoEmplazamientos nuevoObjeto in lista)
                        {
                            sResultado = sResultado + "<tr>" + Comun.NuevaLinea;
                            foreach (string nuevaCelda in cabecera)
                            {
                                sResultado = sResultado + "<th style=\"font-weight: bold; background: #0061ac; color: white\">";
                                // Reads the information
                                propertyValue = nuevoObjeto.GetType().GetProperty(nuevaCelda).GetValue(nuevoObjeto, null);
                                if (propertyValue != null)
                                {
                                    sResultado = sResultado + propertyValue.ToString();
                                }

                                sResultado = sResultado + "</th>" + Comun.NuevaLinea;
                            }
                            sResultado = sResultado + "</tr>" + Comun.NuevaLinea;
                        }
                    }
                    sResultado = sResultado + "</table>" + Comun.NuevaLinea;
                }
            }
            catch (Exception ex)
            {
                sResultado = "";
                log.Error(ex.Message);
            }

            // Returns the result
            return sResultado;

        }

        #endregion

        #region GENERICAS

        public List<Vw_MantenimientoEmplazamientos> GetEmplazamientoByID(long ProyectoID, long EmplazamientoID)
        {

            List<Vw_MantenimientoEmplazamientos> datos = new List<Vw_MantenimientoEmplazamientos>();
            try
            {
                datos = (from c in Context.Vw_MantenimientoEmplazamientos where c.ProyectoID == ProyectoID && c.EmplazamientoID == EmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return datos;

        }

        #endregion

    }
}