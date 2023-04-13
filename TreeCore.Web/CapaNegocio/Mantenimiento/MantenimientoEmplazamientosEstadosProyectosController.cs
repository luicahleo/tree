using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosEstadosProyectosController : GeneralBaseController<MantenimientoEmplazamientosEstadosProyectos, TreeCoreContext>
    {
        public MantenimientoEmplazamientosEstadosProyectosController()
            : base()
        { }
        #region NOTIFICACIONES INTERFACE

        public string GetListaByFiltro(string sFiltro, string sVisualiza)
        {
            // Local variables
            List<Vw_MantenimientoEmplazamientosEstadosProyectos> lista = null;
            string sResultado = null;

            // Invokes the method
            try
            {
                lista = GetItemsList<Vw_MantenimientoEmplazamientosEstadosProyectos>(sFiltro);

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

                        foreach (Vw_MantenimientoEmplazamientosEstadosProyectos nuevoObjeto in lista)
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
    }
}