using CapaNegocio;
using System;
using System.Collections.Generic;
using TreeCore;
using TreeCore.Data;
namespace Sites.Clases
{
    public class InventarioFuncionesCargaStores
    {
        #region Orientaciones Antena

        //Carga de las posibles orientaciones de una antena en función de las orientaciones de la torre
        
        public List<String> GetInventarioElementosByElementoID(long? EmplazamientoID, long? ElementoID, long AtributoID)
        {
            InventarioElementosController cElemento = new InventarioElementosController();
            List<Vw_InventarioElementosReducida> lElementos = new List<Vw_InventarioElementosReducida>();
            List<String> lista = new List<String>();

            try
            {
                //El parametro que se recibe siempre es el elemento a tratar y el atributoID 

                if (EmplazamientoID != null)
                {
                    string ValoresPosibles = "";
                    long InventarioElementoID = 0;

                    if (ElementoID != null)
                    {
                        InventarioElementos actual = new InventarioElementos();
                        actual = cElemento.GetItem((long)ElementoID);

                        if (actual != null)
                        {
                            InventarioElementoID = actual.InventarioElementoID;
                        }

                    }

                    InventarioAtributosController cAtributos = new InventarioAtributosController();
                    InventarioAtributos atributo = new InventarioAtributos();
                    atributo = cAtributos.GetItem(AtributoID);

                    if (atributo != null && atributo.ValoresPosibles != null)
                    {
                        ValoresPosibles = atributo.ValoresPosibles;
                    }

                    lElementos = cElemento.GetInventarioElementosByEmplazamientoFiltroCategorias((long)EmplazamientoID, InventarioElementoID, ValoresPosibles);

                    if (lElementos != null && lElementos.Count > 0)
                    {
                        foreach (Vw_InventarioElementosReducida elemento in lElementos)
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