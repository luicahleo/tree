using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeCore.Data;
using CapaNegocio;

namespace TreeCore.Clases
{
    public class InventarioFuncionesCargaStores
    {

        #region ORIENTACION ANTENA
        //Carga de las posibles orientaciones de una antena en función de las orientaciones de la torre
        public List<Object> GetOrientacionesByTorre(long ElementoID)
        {
            InventarioElementosController cElemento = new InventarioElementosController();
            InventarioElementosAtributosController cElementosAtributos = new InventarioElementosAtributosController();

            List<Object> lista = new List<object>();
            try
            {
                //El parametro que se recibe siempre es el elemento a tratar. Para cargar este combobox, buscamos un elemento padre con Codigo 'TORRE' y un atributo 'ORIENT_TORRE'

                int LadosSoporte = 360;

                Data.InventarioElementos actual = cElemento.GetItem(ElementoID);

                if (actual != null && actual.InventarioElementoPadreID != null)
                {
                    string lados = cElementosAtributos.GetValorAtributoByElementoCodigoAtributo((long)actual.InventarioElementoPadreID, "ORIENT_TORRE");
                    if (lados != "")
                    {
                        LadosSoporte = Convert.ToInt32(lados);
                    }
                }

                int valor = 0;
                int incremento = 360 / LadosSoporte;
                Object objeto = null;

                for (int i = 1; i <= LadosSoporte; i = i + 1)
                {
                    objeto = new { Valor = (valor).ToString() + '°' };
                    lista.Add(objeto);
                    valor = valor + incremento;
                }
            }
            catch (Exception ex)
            {
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
            }

            return lista;
        }

        public List<String> GetInventarioElementosByElementoID(long? lEmplazamientoID, long? lElementoID, long lAtributoID)
        {
            InventarioElementosController cElemento = new InventarioElementosController();
            List<Vw_InventarioElementosReducida> listaElementos = new List<Vw_InventarioElementosReducida>();
            List<String> lista = new List<String>();

            try
            {
                //El parametro que se recibe siempre es el elemento a tratar y el atributoID 

                if (lEmplazamientoID != null)
                {
                    string sValoresPosibles = "";
                    long lInventarioElementoID = 0;

                    if (lElementoID != null)
                    {
                        Data.InventarioElementos oActual;
                        oActual = cElemento.GetItem((long)lElementoID);

                        if (oActual != null)
                        {
                            lInventarioElementoID = oActual.InventarioElementoID;
                        }
                    }

                    InventarioAtributosController cAtributos = new InventarioAtributosController();
                    InventarioAtributos oAtributo;
                    oAtributo = cAtributos.GetItem(lAtributoID);

                    if (oAtributo != null && oAtributo.ValoresPosibles != null)
                    {
                        sValoresPosibles = oAtributo.ValoresPosibles;
                    }

                    listaElementos = cElemento.GetInventarioElementosByEmplazamientoFiltroCategorias((long)lEmplazamientoID, lInventarioElementoID, sValoresPosibles);

                    if (listaElementos != null && listaElementos.Count > 0)
                    {
                        foreach (Vw_InventarioElementosReducida elemento in listaElementos)
                        {
                            lista.Add(elemento.NumeroInventario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
            }

            return lista;
        }


        #endregion
    }
}