using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoTipologiasController : GeneralBaseController<MantenimientoTipologias, TreeCoreContext>
    {
        public MantenimientoTipologiasController()
            : base()
        { }
        #region NOTIFICACIONES INTERFACE

        public string GetListaByFiltro(string sFiltro, string sVisualiza)
        {
            // Local variables
            List<Vw_MantenimientoTipologias> lista = null;
            string sResultado = null;

            // Invokes the method
            try
            {
                lista = GetItemsList<Vw_MantenimientoTipologias>(sFiltro);

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

                        foreach (Vw_MantenimientoTipologias nuevoObjeto in lista)
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
            }

            // Returns the result
            return sResultado;

        }

        #endregion
        public long getTipologiaID(string sTipo)
        {
            long lDatos;

            try
            {
                lDatos = (from c in Context.MantenimientoTipologias where c.MantenimientoTipologia == sTipo select c.MantenimientoTipologiaID).First();
            }
            catch (Exception ex)
            {
                lDatos = new long();
                log.Error(ex.Message);
            }

            return lDatos;
        }

        public List<MantenimientoTipologias> GetAllTipologia(bool bActivo)
        {

            List<MantenimientoTipologias> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.MantenimientoTipologias where c.Activo == bActivo select c).ToList();
                }
                else
                {
                    lista = (from c in Context.MantenimientoTipologias select c).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return lista;
        }

        public MantenimientoTipologias GetByMantenimientoTipologia(string MantenimientoTipologia)
        {
            MantenimientoTipologias tipologia;
            try
            {
                tipologia = (from c 
                             in Context.MantenimientoTipologias 
                             where c.MantenimientoTipologia == MantenimientoTipologia && 
                                    c.Activo
                             select c).First();
            }
            catch (Exception ex)
            {
                tipologia = null;
                log.Error(ex.Message);
            }
            return tipologia;
        }
    }
}