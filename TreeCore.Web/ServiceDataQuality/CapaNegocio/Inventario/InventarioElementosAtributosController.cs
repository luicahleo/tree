using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Tree.Linq.GenericExtensions;
using System.Text.RegularExpressions;
using Ext.Net;


namespace CapaNegocio
{
    public class InventarioElementosAtributosController : GeneralBaseController<InventarioElementosAtributos, TreeCoreContext>
    {
        public InventarioElementosAtributosController()
            : base()
        {

        }

        #region GESTION BASE

        public List<InventarioElementosAtributos> GetElementosAtributosByElemento(long lElementoID)
        {
            // Local variable
            List<InventarioElementosAtributos> lista = new List<InventarioElementosAtributos>();


            try
            {

                lista = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == lElementoID select c).ToList();


            }
            catch (Exception)
            {

            }

            return lista;
        }
        public List<Vw_InventarioElementosAtributos> GetElementosAtributosViewByElemento(long lElementoID)
        {
            // Local variable
            List<Vw_InventarioElementosAtributos> lista = new List<Vw_InventarioElementosAtributos>();


            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID select c).ToList();


            }
            catch (Exception)
            {

            }

            return lista;
        }

        public InventarioElementosAtributos GetItemByEmplazamientoAtributo(long lElementoID, long atributoID)
        {
            // Local variable
            List<InventarioElementosAtributos> lista = new List<InventarioElementosAtributos>();
            InventarioElementosAtributos dato = null;

            try
            {

                lista = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.InventarioAtributoID == atributoID select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception)
            {
            }

            return dato;
        }

        public string GetValorAtributoByElementoCodigoAtributo(long lElementoID, string codigoAtributo)
        {
            // Local variable
            List<Vw_InventarioElementosAtributos> lista = new List<Vw_InventarioElementosAtributos>();
            string Valor = "";
            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.CodigoAtributo == codigoAtributo select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    Valor = lista.ElementAt(0).Valor;
                }

            }
            catch (Exception)
            {
            }

            return Valor;
        }

        public long GetAtributoIDByElementoCodigoAtributo(long lElementoID, string codigoAtributo)
        {
            // Local variable
            List<long> lista = new List<long>();
            long lresultado = 0;


            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.CodigoAtributo == codigoAtributo select c.InventarioAtributoID).ToList();

                if (lista != null && lista.Count > 0)
                {
                    lresultado = lista.ElementAt(0);
                }

            }
            catch (Exception)
            {

            }

            return lresultado;
        }

        public long GetAtributoElementoIDByElementoCodigoAtributo(long lElementoID, string codigoAtributo)
        {
            // Local variable
            List<long> lista = new List<long>();
            long lresultado = 0;


            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.CodigoAtributo == codigoAtributo select c.InventarioElementoAtributoID).ToList();

                if (lista != null && lista.Count > 0)
                {
                    lresultado = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {

            }

            return lresultado;
        }

        public string GetAtributoValorByElementoAtributo(long lElementoID, string sNombreAtributo)
        {
            // Local variable
            List<string> lista = null;
            string sResultado = null;


            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.NombreAtributo == sNombreAtributo select c.Valor).ToList();

                if (lista != null && lista.Count > 0)
                {
                    sResultado = lista.ElementAt(0);
                }

            }
            catch (Exception)
            {

            }

            return sResultado;
        }

        public string GetAtributoValorByElementoCodigoAtributo(long lElementoID, string sCodigoAtributo)
        {
            // Local variable
            List<string> lista = null;
            string sResultado = null;


            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.CodigoAtributo == sCodigoAtributo select c.Valor).ToList();

                if (lista != null && lista.Count > 0)
                {
                    sResultado = lista.ElementAt(0);
                }

            }
            catch (Exception)
            {

            }

            return sResultado;
        }

        public string GetValorByAtributoIDListaAtributos(long AtributoID, List<InventarioElementosAtributos> listaAtributos)
        {
            string Valor = "";
            try
            {
                Valor = (from c in listaAtributos where c.InventarioAtributoID == AtributoID select c.Valor).First();
            }
            catch (Exception)
            { }

            return Valor;
        }

        public List<InventarioElementosAtributos> getAtributosDinamicos(long ElementoID)
        {
            List<InventarioElementosAtributos> l = new List<InventarioElementosAtributos>();
            l = (from c in Context.InventarioElementosAtributos
                 where c.InventarioElementoID == ElementoID
                 select c).ToList();

            return l;

        }
        public List<AtributosDinamicos> get(List<InventarioElementosAtributos> l)
        {
            List<AtributosDinamicos> a = new List<AtributosDinamicos>();
            InventarioAtributosController atc = new InventarioAtributosController();


            foreach (var it in l)
            {
                AtributosDinamicos at = new AtributosDinamicos();
                InventarioAtributos ia = new InventarioAtributos();
                ia = atc.GetItem("InventarioAtributoID ==" + it.InventarioAtributoID);
                at.Atributo = ia.NombreAtributo;
                at.Valor = it.Valor;
                a.Add(at);
            }
            return a;

        }

        #endregion

        #region EXPORT INVENTARIO SITES

        public List<Vw_InventarioElementosAtributos> GetElementosAtributosByElementoID(long lElementoID)
        {
            // Local variable
            List<Vw_InventarioElementosAtributos> lista = new List<Vw_InventarioElementosAtributos>();


            try
            {

                lista = (from c in Context.Vw_InventarioElementosAtributos where c.InventarioElementoID == lElementoID select c).ToList();


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return lista;
        }

        public List<InventarioElementosAtributos> GetTablaElementosAtributosByElementoID(long lElementoID)
        {
            // Local variable
            List<InventarioElementosAtributos> lista = new List<InventarioElementosAtributos>();


            try
            {

                lista = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == lElementoID select c).ToList();


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return lista;
        }

        public InventarioElementosAtributos GetAtributoElemento(long lElementoID, long lAtributoID)
        {
            // Local variable
            InventarioElementosAtributos oDato;
            try
            {
                oDato = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == lElementoID && c.InventarioAtributoID == lAtributoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        #endregion

        public bool EliminarAtributosElemento(long ElementoID)
        {
            bool correct = true;
            List<InventarioElementosAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == ElementoID select c).ToList();
                foreach (var item in listaDatos)
                {
                    if (!this.DeleteItem(item.InventarioElementoAtributoID))
                    {
                        correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                correct = false;
            }
            return correct;
        }

        public InventarioElementosAtributos GetAtributo(long Elemento, long lAtributoID)
        {
            // Local variable
            InventarioElementosAtributos oDato;
            try
            {
                oDato = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == Elemento && c.InventarioAtributoID == lAtributoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public InventarioAtributos GetAtributoByCodigo(long Categoria, string sCodigo)
        {
            InventarioAtributos oDato;
            try
            {
                oDato = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == Categoria && c.CodigoAtributo == sCodigo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public InventarioElementosAtributos SaveUpdateAtributo(long lCatID, long lEmplazamientoID, string sCodAtributo, string Valor, bool bActualizar, JsonObject listas, long lUsuarioID, out bool Obligatorio)
        {
            InventarioElementosAtributosController cAtributos = new InventarioElementosAtributosController();
            cAtributos.SetDataContext(this.Context);
            InventarioAtributosController cAtributosConf = new InventarioAtributosController();
            cAtributosConf.SetDataContext(this.Context);
            InventarioAtributos oAtributoConf;
            InventarioElementosAtributos oDato = null;
            InventarioAtributosTiposDatosPropiedadesController cPropiedades = new InventarioAtributosTiposDatosPropiedadesController();
            cPropiedades.SetDataContext(this.Context);
            List<Vw_InventarioAtributosTiposDatosPropiedades> listaPropiedades;
            Vw_InventarioAtributosTiposDatosPropiedades oPropiedad;
            object oAux;
            JsonObject listaValores;
            bool Valido = true;
            Obligatorio = false;
            JsonObject jsonAux;
            try
            {
                oAtributoConf = GetAtributoByCodigo(lCatID, sCodAtributo);

                #region PROPIEDADES

                listaPropiedades = cPropiedades.GetPropiedadesFromAtributo(oAtributoConf.InventarioAtributoID);

                if (listaPropiedades != null && listaPropiedades.Count > 0)
                {
                    try
                    {
                        oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.AllowBlank.ToString()).First();
                        if (oPropiedad != null)
                        {
                            Obligatorio = true;
                        }
                        else
                        {
                            Obligatorio = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex.Message);
                        Obligatorio = false;
                    }
                }
                else
                {
                    Obligatorio = false;
                }

                #endregion

                switch (oAtributoConf.TiposDatos.Codigo)
                {
                    case Comun.TIPODATO_CODIGO_TEXTO:

                        #region COMPROBAR PROPIEDADES

                        #region REGEX

                        if (listaPropiedades != null && listaPropiedades.Count > 0)
                        {
                            try
                            {
                                oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.Regex.ToString()).First();
                                if (oPropiedad != null)
                                {
                                    Regex rgx = new Regex(oPropiedad.Valor);
                                    Valido = rgx.IsMatch(Valor);
                                }
                                else
                                {
                                    Valido = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                //log.Error(ex.Message);
                                Valido = true;
                            }
                        }

                        #endregion

                        #endregion

                        if (Valido && (!Obligatorio || (Obligatorio && Valor != "")))
                        {
                            oDato = new InventarioElementosAtributos
                            {
                                InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                InventarioElementoID = lEmplazamientoID,
                                Valor = Valor
                            };
                        }
                        else
                        {
                            oDato = null;
                        }

                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO:
                        double auxNum;
                        long auxNum2;

                        #region COMPROBAR PROPIEDADES

                        if (listaPropiedades != null && listaPropiedades.Count > 0)
                        {
                            #region MINVALUE

                            try
                            {
                                oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MinValue.ToString()).First();
                                if (oPropiedad != null)
                                {
                                    Valido = double.Parse(oPropiedad.Valor) <= double.Parse(Valor.Replace(',', '.'));
                                }
                                else
                                {
                                    Valido = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                //log.Error(ex.Message);
                                Valido = true;
                            }

                            #endregion

                            #region MAXVALUE
                            if (Valido)
                            {
                                try
                                {
                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MaxValue.ToString()).First();
                                    if (oPropiedad != null)
                                    {
                                        Valido = double.Parse(oPropiedad.Valor) >= double.Parse(Valor.Replace(',', '.'));
                                    }
                                    else
                                    {
                                        Valido = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //log.Error(ex.Message);
                                    Valido = true;
                                }
                            }
                            #endregion

                            #region Format

                            oPropiedad = null;
                            try
                            {
                                oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == "AllowDecimals").First();

                                if (oPropiedad != null && Valido)
                                {
                                    if (oPropiedad.Valor == true.ToString())
                                    {
                                        try
                                        {
                                            oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == "DecimalPrecision").First();

                                            if (oPropiedad != null)
                                            {
                                                oDato = new InventarioElementosAtributos
                                                {
                                                    InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                                    InventarioElementoID = lEmplazamientoID,
                                                    Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), int.Parse(oPropiedad.Valor)).ToString()
                                                };
                                            }
                                            else
                                            {
                                                oDato = new InventarioElementosAtributos
                                                {
                                                    InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                                    InventarioElementoID = lEmplazamientoID,
                                                    Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), 2).ToString()
                                                };
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //log.Error(ex.Message);
                                            oDato = new InventarioElementosAtributos
                                            {
                                                InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                                InventarioElementoID = lEmplazamientoID,
                                                Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), 2).ToString()
                                            };
                                        }
                                    }
                                    else if(long.TryParse(Valor, out auxNum2))
                                    {
                                        oDato = new InventarioElementosAtributos
                                        {
                                            InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                            InventarioElementoID = lEmplazamientoID,
                                            Valor = Valor
                                        };
                                    }
                                    else
                                    {
                                        Valido = false;
                                    }
                                }
                                else
                                {
                                    oDato = null;
                                }
                            }
                            catch (Exception ex)
                            {
                                //log.Error(ex.Message);
                                oDato = null;
                            }

                            #endregion
                        }

                        #endregion
                        if (oDato == null)
                        {
                            if (Valido && double.TryParse(Valor.Replace(',', '.'), out auxNum))
                            {
                                oDato = new InventarioElementosAtributos
                                {
                                    InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                    InventarioElementoID = lEmplazamientoID,
                                    Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), 2).ToString()
                                };
                            }
                            else if (!Obligatorio && (Valor == null || Valor == ""))
                            {
                                oDato = new InventarioElementosAtributos
                                {
                                    InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                    InventarioElementoID = lEmplazamientoID,
                                    Valor = ""
                                };
                            }
                            else
                            {
                                oDato = null;
                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_FECHA:
                        DateTime auxDate;

                        #region COMPROBAR PROPIEDADES

                        if (listaPropiedades != null && listaPropiedades.Count > 0)
                        {
                            #region MINDATE

                            try
                            {
                                oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MinDate.ToString()).First();
                                if (oPropiedad != null)
                                {
                                    Valido = (GetFechaExcel(Valor).Date != new DateTime() && GetFechaExcel(Valor).Date.CompareTo(DateTime.Parse(Valor)) >= 0);
                                }
                                else
                                {
                                    Valido = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                //log.Error(ex.Message);
                                Valido = true;
                            }

                            #endregion

                            #region MAXDATE

                            if (Valido)
                            {
                                try
                                {
                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MaxDate.ToString()).First();
                                    if (oPropiedad != null)
                                    {
                                        Valido = (GetFechaExcel(Valor).Date != new DateTime() && GetFechaExcel(Valor).Date.CompareTo(DateTime.Parse(Valor)) <= 0);
                                    }
                                    else
                                    {
                                        Valido = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //log.Error(ex.Message);
                                    Valido = true;
                                }
                            }

                            #endregion

                        }

                        #endregion

                        if (Valido && GetFechaExcel(Valor).Date != new DateTime().Date)
                        {
                            oDato = new InventarioElementosAtributos
                            {
                                InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                InventarioElementoID = lEmplazamientoID,
                                Valor = GetFechaExcel(Valor).ToString("M/dd/yyyy") + " 12:00:00 AM"
                            };
                        }
                        else
                        {
                            oDato = null;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_LISTA:
                        listaValores = null;
                        oAux = null;
                        try
                        {
                            listas.TryGetValue(oAtributoConf.NombreAtributo, out oAux);
                            if (oAux != null)
                            {
                                listaValores = (JsonObject)oAux;
                                if (listaValores.TryGetValue(Valor, out oAux))
                                {
                                    jsonAux = (JsonObject)oAux;
                                    if (jsonAux.TryGetValue("Value", out oAux))
                                    {
                                        oDato = new InventarioElementosAtributos
                                        {
                                            InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                            InventarioElementoID = lEmplazamientoID,
                                            Valor = oAux.ToString()
                                        };
                                    }
                                    else
                                    {
                                        oDato = null;
                                    }
                                }
                                else
                                {
                                    oDato = null;
                                }
                            }
                            else
                            {
                                oDato = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            oDato = null;
                        }

                        break;
                    case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                        listaValores = null;
                        oAux = null;
                        string sValorAux = "";
                        try
                        {
                            listas.TryGetValue(oAtributoConf.NombreAtributo, out oAux);
                            if (oAux != null)
                            {
                                listaValores = (JsonObject)oAux;
                                bool correcto = true;
                                foreach (var item in Valor.Split(',').ToList())
                                {
                                    if (listaValores.TryGetValue(Valor, out oAux) && correcto)
                                    {
                                        jsonAux = (JsonObject)oAux;
                                        if (jsonAux.TryGetValue("Value", out oAux))
                                        {
                                            if (sValorAux == "")
                                            {
                                                sValorAux += oAux.ToString();
                                            }
                                            else
                                            {
                                                sValorAux += "," + oAux.ToString();
                                            }
                                        }                                            
                                    }
                                    else
                                    {
                                        correcto = false;
                                    }
                                }
                                if (correcto)
                                {
                                    oDato = new InventarioElementosAtributos
                                    {
                                        InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                        InventarioElementoID = lEmplazamientoID,
                                        Valor = sValorAux
                                    };
                                }
                                else
                                {
                                    oDato = null;
                                }
                            }
                            else
                            {
                                oDato = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            oDato = null;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_BOOLEAN:
                        bool auxBool;
                        if (bool.TryParse(Valor, out auxBool))
                        {
                            oDato = new InventarioElementosAtributos
                            {
                                InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                                InventarioElementoID = lEmplazamientoID,
                                Valor = Valor
                            };
                        }
                        else
                        {
                            oDato = null;
                        }
                        break;
                    //case "ENTERO":

                    //    break;
                    //case "FLOTANTE":

                    //    break;
                    //case "MONEADA":

                    //    break;
                    //case "GEOPOSICION":

                    //    break;
                    case Comun.TIPODATO_CODIGO_TEXTAREA:
                        oDato = new InventarioElementosAtributos
                        {
                            InventarioAtributoID = oAtributoConf.InventarioAtributoID,
                            InventarioElementoID = lEmplazamientoID,
                            Valor = Valor
                        };
                        break;
                    default:
                        oDato = null;
                        break;
                }

                if (oDato != null && oDato.Valor != null)
                {
                    oDato.FechaCreacion = DateTime.Now;
                    oDato.CreadorID = lUsuarioID;
                    oDato.Activo = true;
                    if (bActualizar)
                    {
                        InventarioElementosAtributos oAtributo = GetAtributo(lEmplazamientoID, oAtributoConf.InventarioAtributoID);
                        if (oAtributo != null)
                        {
                            oAtributo.Valor = oDato.Valor;
                            oAtributo.FechaCreacion = oDato.FechaCreacion;
                            oAtributo.CreadorID = oDato.CreadorID;
                            oAtributo.Activo = oDato.Activo;
                            if (UpdateItem(oAtributo))
                            {
                                oDato = oAtributo;
                            }
                            else
                            {
                                oDato = null;
                            }
                        }
                        else
                        {
                            oDato = AddItem(oDato);
                        }
                    }
                    else
                    {
                        oDato.FechaCreacion = DateTime.Now;
                        oDato.CreadorID = lUsuarioID;
                        oDato.Activo = true;
                        oDato = AddItem(oDato);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
                Obligatorio = false;
            }
            return oDato;
        }

        public static DateTime GetFechaExcel(string sDate)
        {
            DateTime oDate = new DateTime();
            try
            {
                if (sDate.Length == 8)
                {
                    oDate = new DateTime(int.Parse(sDate.Substring(0, 4)), int.Parse(sDate.Substring(4, 2)), int.Parse(sDate.Substring(6, 2)));
                }
                else
                {
                    oDate = new DateTime();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDate = new DateTime();
            }
            return oDate;
        }

    }
}