using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosEstadosTiposDocumentosController : GeneralBaseController<MantenimientoEmplazamientosEstadosTiposDocumentos, TreeCoreContext>
    {
        public MantenimientoEmplazamientosEstadosTiposDocumentosController()
            : base()
        { }
        #region NOTIFICACIONES INTERFACE

        public string GetListaByFiltro(string sFiltro, string sVisualiza)
        {
            // Local variables
            List<Vw_MantenimientoEmplazamientosEstadosTiposDocumentos> lista = null;
            string sResultado = null;

            // Invokes the method
            try
            {
                lista = GetItemsList<Vw_MantenimientoEmplazamientosEstadosTiposDocumentos>(sFiltro);

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

                        foreach (Vw_MantenimientoEmplazamientosEstadosTiposDocumentos nuevoObjeto in lista)
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
        public List<MantenimientoEmplazamientosEstadosTiposDocumentos> GetMantenimientoEmplazamientosEstadosTipoDocumentosByEstadoTipologia(long lEstadoID, long? lTipologiaID)
        {
            List<MantenimientoEmplazamientosEstadosTiposDocumentos> listaDatos = new List<MantenimientoEmplazamientosEstadosTiposDocumentos>();
            MantenimientoEmplazamientosEstadosTiposDocumentos listaEstados = null;

            //if (lTipologiaID != null)
            //{
            //    listaEstados = (from c in Context.MantenimientoEmplazamientosEstadosTiposDocumentos where c.MantenimientoEmplazamientoEstadoID == lEstadoID && c.MantenimientoTipologiaID == lTipologiaID select c).FirstOrDefault();
            //}
            //else
            //{
            //    listaEstados = (from c in Context.MantenimientoEmplazamientosEstadosTiposDocumentos where c.MantenimientoEmplazamientoEstadoID == lEstadoID && c.MantenimientoTipologiaID == null select c).FirstOrDefault();
            //}

            if (listaEstados != null)
            {
                listaDatos = (from c in Context.MantenimientoEmplazamientosEstadosTiposDocumentos where c.MantenimientoEmplazamientoEstadoID == listaEstados.MantenimientoEmplazamientoEstadoID select c).ToList();
            }

            return listaDatos;
        }

        public bool ExisteDocumento(MantenimientoEmplazamientosEstados oEstado, long lDoc)
        {
            bool bExiste = false;

            MantenimientoEmplazamientosEstadosTiposDocumentos oDocumentos = new MantenimientoEmplazamientosEstadosTiposDocumentos();

            oDocumentos = (from c in Context.MantenimientoEmplazamientosEstadosTiposDocumentos where c.MantenimientoEmplazamientoEstadoID == oEstado.MantenimientoEmplazamientoEstadoID && c.DocumentoTipoID == lDoc select c).First();

            if (oDocumentos != null)
            {
                bExiste = true;
            }

            return bExiste;
        }
    }
}