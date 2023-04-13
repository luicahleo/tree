using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using Ext.Net;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CapaNegocio
{
    public class CoreInventarioPlantillasAtributosCategoriasAtributosController : GeneralBaseController<CoreInventarioPlantillasAtributosCategoriasAtributos, TreeCoreContext>
    {
        public CoreInventarioPlantillasAtributosCategoriasAtributosController()
            : base()
        { }

        public CoreInventarioPlantillasAtributosCategoriasAtributos GetAtributoPlantilla(long lPlantillaID, long lAtributoID)
        {
            // Local variable
            CoreInventarioPlantillasAtributosCategoriasAtributos oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioPlantillasAtributosCategoriasAtributos where c.CoreInventarioPlantillaAtributoCategoriaID == lPlantillaID && c.CoreAtributoConfiguracionID == lAtributoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public List<CoreInventarioPlantillasAtributosCategoriasAtributos> GetAtributosValoresFromPlantilla(long lPlantillaID) {
            List<CoreInventarioPlantillasAtributosCategoriasAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategoriasAtributos where c.CoreInventarioPlantillaAtributoCategoriaID == lPlantillaID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public InventarioPlantillasAtributosJson SaveUpdateAtributo(
            long lAtributoConfID,
            string Valor,
            JsonObject listas,
            out bool Obligatorio,
            out string descError,
            JsonObject listasRest = null,
            JsonObject listaObj = null)
        {
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            cAtributosConf.SetDataContext(this.Context);
            CoreAtributosConfiguraciones oAtributoConf;
            InventarioPlantillasAtributosJson oDato = null;
            CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
            cPropiedades.SetDataContext(this.Context);
            List<Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaPropiedades;
            Vw_CoreAtributosConfiguracionTiposDatosPropiedades oPropiedad;
            object oAux, Text;
            JsonObject listaValores;
            string ValorOri = Valor;
            bool Valido = true;
            Obligatorio = false;
            JsonObject jsonAux;
            descError = "";
            try
            {
                if (listaObj == null)
                {
                    oAtributoConf = cAtributosConf.GetItem(lAtributoConfID);
                }
                else
                {
                    if (listaObj.TryGetValue(lAtributoConfID.ToString(), out oAux))
                    {
                        oAtributoConf = (CoreAtributosConfiguraciones)oAux;
                    }
                    else
                    {
                        oAtributoConf = cAtributosConf.GetItem(lAtributoConfID);
                    }
                }

                #region PROPIEDADES

                if (listasRest == null)
                {
                    listaPropiedades = cPropiedades.GetVwPropiedadesFromAtributo(oAtributoConf.CoreAtributoConfiguracionID);
                }
                else
                {
                    if (listasRest.TryGetValue(lAtributoConfID.ToString(), out oAux))
                    {
                        listaPropiedades = (List<Vw_CoreAtributosConfiguracionTiposDatosPropiedades>)oAux;
                    }
                    else
                    {
                        listaPropiedades = cPropiedades.GetVwPropiedadesFromAtributo(oAtributoConf.CoreAtributoConfiguracionID);
                    }
                }


                if (listaPropiedades != null && listaPropiedades.Count > 0)
                {
                    oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == Comun.TiposPropiedades.AllowBlank.ToString() && prop.Valor == "False").FirstOrDefault();
                    if (oPropiedad != null)
                    {
                        Obligatorio = true;
                    }
                    else
                    {
                        Obligatorio = false;
                    }
                }
                else
                {
                    Obligatorio = false;
                }

                #endregion

                if (Valor.Length > 200)
                {
                    oDato = null;
                    descError = "Value exceeds maximum length.";
                }
                else if (Obligatorio && (Valor == null || Valor == ""))
                {
                    oDato = null;
                    descError = "Value does not match AllowBlank rule restrictions.";
                }
                else
                {

                    switch (oAtributoConf.TiposDatos.Codigo)
                    {
                        case Comun.TIPODATO_CODIGO_TEXTO:

                            if (!Obligatorio && (Valor == null || Valor == ""))
                            {
                                oDato = new InventarioPlantillasAtributosJson
                                {
                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.Codigo,
                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                    TextLista = Valor,
                                    Valor = ""
                                };
                            }
                            else
                            {
                                #region COMPROBAR PROPIEDADES

                                #region REGEX

                                if (listaPropiedades != null && listaPropiedades.Count > 0)
                                {
                                    try
                                    {
                                        oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == Comun.TiposPropiedades.Regex.ToString()).FirstOrDefault();
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
                                    catch (Exception)
                                    {
                                        Valido = true;
                                    }
                                }

                                #endregion

                                #endregion

                                if (!Valido)
                                {
                                    oDato = null;
                                    descError = "Value does not match RegExp rule restrictions.";
                                }
                                else
                                {
                                    oDato = new InventarioPlantillasAtributosJson
                                    {
                                        AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                        NombreAtributo = oAtributoConf.Codigo,
                                        TipoDato = oAtributoConf.TiposDatos.Codigo,
                                        TextLista = Valor,
                                        Valor = Valor
                                    };
                                }
                            }

                            break;
                        case Comun.TIPODATO_CODIGO_NUMERICO:
                            double auxNum;
                            long auxNum2;

                            if (!Obligatorio && (Valor == null || Valor == ""))
                            {
                                oDato = new InventarioPlantillasAtributosJson
                                {
                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.Codigo,
                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                    TextLista = Valor,
                                    Valor = ""
                                };
                            }
                            else if (double.TryParse(Valor.Replace(',', '.'), out auxNum))
                            {
                                if (listaPropiedades != null && listaPropiedades.Count > 0)
                                {
                                    #region COMPROBAR PROPIEDADES

                                    #region MINVALUE

                                    oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == Comun.TiposPropiedades.MinValue.ToString()).FirstOrDefault();

                                    if (oPropiedad != null)
                                    {
                                        Valido = double.Parse(oPropiedad.Valor.Replace(',', '.')) <= double.Parse(Valor);
                                    }
                                    else
                                    {
                                        Valido = true;
                                    }

                                    #endregion

                                    if (!Valido)
                                    {
                                        oDato = null;
                                        descError = "Value does not match MinValue rule restrictions";
                                        return oDato;
                                    }
                                    else
                                    {
                                        #region MAXVALUE
                                        oPropiedad = null;
                                        oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == Comun.TiposPropiedades.MaxValue.ToString()).FirstOrDefault();
                                        if (oPropiedad != null)
                                        {
                                            Valido = double.Parse(oPropiedad.Valor.Replace(',', '.')) >= double.Parse(Valor);
                                        }
                                        else
                                        {
                                            Valido = true;
                                        }

                                        #endregion
                                    }

                                    if (!Valido)
                                    {
                                        oDato = null;
                                        descError = "Value does not match MaxValue rule restrictions.";
                                        return oDato;
                                    }
                                    else
                                    {
                                        #region AllowDecimal false

                                        oPropiedad = null;
                                        oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "AllowDecimals" && prop.Valor == false.ToString()).FirstOrDefault();

                                        if (oPropiedad != null)
                                        {
                                            if (long.TryParse(Valor, out auxNum2))
                                            {
                                                oDato = new InventarioPlantillasAtributosJson
                                                {
                                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                                    NombreAtributo = oAtributoConf.Codigo,
                                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                                    TextLista = Valor,
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
                                            Valido = true;
                                        }

                                        #endregion
                                    }

                                    if (!Valido)
                                    {
                                        oDato = null;
                                        descError = "Value does not match AllowDecimals rule restrictions.";
                                        return oDato;
                                    }
                                    else if (Valido && oDato == null)
                                    {
                                        #region Decimal Precision

                                        oPropiedad = null;
                                        oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "DecimalPrecision").FirstOrDefault();

                                        if (oPropiedad != null)
                                        {
                                            oDato = new InventarioPlantillasAtributosJson
                                            {
                                                AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                                NombreAtributo = oAtributoConf.Codigo,
                                                TipoDato = oAtributoConf.TiposDatos.Codigo,
                                                TextLista = Valor,
                                                Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), int.Parse(oPropiedad.Valor)).ToString()
                                            };
                                        }
                                        else
                                        {
                                            oDato = new InventarioPlantillasAtributosJson
                                            {
                                                AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                                NombreAtributo = oAtributoConf.Codigo,
                                                TipoDato = oAtributoConf.TiposDatos.Codigo,
                                                TextLista = Valor,
                                                Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), 2).ToString()
                                            };
                                        }

                                        #endregion
                                    }

                                    #endregion
                                }
                                else
                                {
                                    oDato = new InventarioPlantillasAtributosJson
                                    {
                                        AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                        NombreAtributo = oAtributoConf.Codigo,
                                        TipoDato = oAtributoConf.TiposDatos.Codigo,
                                        TextLista = Valor,
                                        Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), 2).ToString()
                                    };
                                }
                            }
                            else
                            {
                                descError = "The attribute value must be numeric.";
                                oDato = null;
                            }

                            break;
                        case Comun.TIPODATO_CODIGO_FECHA:
                            if (Valor == "Error")
                            {
                                oDato = null;
                                descError = "Value does not match Date format restrictions.";
                            }
                            else if (!Obligatorio && Valor == "")
                            {
                                oDato = new InventarioPlantillasAtributosJson
                                {
                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.Codigo,
                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                    TextLista = Valor,
                                    Valor = ""
                                };
                            }
                            else if (DateTime.Parse(Valor, CultureInfo.InvariantCulture).Date != new DateTime().Date)
                            {

                                if (listaPropiedades != null && listaPropiedades.Count > 0)
                                {
                                    #region COMPROBAR PROPIEDADES

                                    #region MINDATE

                                    oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == Comun.TiposPropiedades.MinDate.ToString()).FirstOrDefault();

                                    if (oPropiedad != null)
                                    {
                                        string aux;
                                        string valorFinal = "";
                                        string Value = oPropiedad.Valor.Split(' ')[0];
                                        foreach (var item in Value.Split('/'))
                                        {
                                            if (valorFinal != "")
                                            {
                                                valorFinal += "/";
                                            }
                                            if (item.Length < 2)
                                            {
                                                aux = "0";
                                            }
                                            else
                                            {
                                                aux = "";
                                            }
                                            valorFinal += aux + item;
                                        }

                                        Valido = (DateTime.ParseExact(Valor.Substring(0, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture).Date != new DateTime() && DateTime.ParseExact(valorFinal, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date.CompareTo(DateTime.ParseExact(Valor.Substring(0, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture)) <= 0);
                                    }
                                    else
                                    {
                                        Valido = true;
                                    }

                                    #endregion

                                    if (!Valido)
                                    {
                                        oDato = null;
                                        descError = "Value does not match MinDate rule restrictions.";
                                        return oDato;
                                    }
                                    else
                                    {
                                        #region MAXDATE

                                        oPropiedad = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == Comun.TiposPropiedades.MaxDate.ToString()).FirstOrDefault();
                                        if (oPropiedad != null)
                                        {
                                            string aux;
                                            string valorFinal = "";
                                            string Value = oPropiedad.Valor.Split(' ')[0];
                                            foreach (var item in Value.Split('/'))
                                            {
                                                if (valorFinal != "")
                                                {
                                                    valorFinal += "/";
                                                }
                                                if (item.Length < 2)
                                                {
                                                    aux = "0";
                                                }
                                                else
                                                {
                                                    aux = "";
                                                }
                                                valorFinal += aux + item;
                                            }

                                            Valido = (DateTime.ParseExact(Valor.Substring(0, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture).Date != new DateTime() && DateTime.ParseExact(Valor.Substring(0, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture).Date.CompareTo(DateTime.ParseExact(valorFinal, "MM/dd/yyyy", CultureInfo.InvariantCulture)) <= 0);
                                        }
                                        else
                                        {
                                            Valido = true;
                                        }

                                        #endregion
                                    }

                                    if (!Valido)
                                    {
                                        oDato = null;
                                        descError = "Value does not match MaxDate rule restrictions.";
                                        return oDato;
                                    }
                                    else
                                    {
                                        oDato = new InventarioPlantillasAtributosJson
                                        {
                                            AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                            NombreAtributo = oAtributoConf.Codigo,
                                            TipoDato = oAtributoConf.TiposDatos.Codigo,
                                            TextLista = DateTime.Parse(Valor, CultureInfo.InvariantCulture).ToString(Comun.FORMATO_FECHA),
                                            Valor = DateTime.Parse(Valor, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)
                                        };
                                    }

                                    #endregion
                                }
                                else
                                {
                                    oDato = new InventarioPlantillasAtributosJson
                                    {
                                        AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                        NombreAtributo = oAtributoConf.Codigo,
                                        TipoDato = oAtributoConf.TiposDatos.Codigo,
                                        TextLista = DateTime.Parse(Valor, CultureInfo.InvariantCulture).ToString(Comun.FORMATO_FECHA),
                                        Valor = DateTime.Parse(Valor, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)
                                    };
                                }

                            }
                            else
                            {
                                oDato = null;
                                descError = "Value does not match Date format restrictions.";
                            }

                            break;
                        case Comun.TIPODATO_CODIGO_LISTA:
                            listaValores = null;
                            oAux = null;
                            Text = null;

                            if (!Obligatorio && Valor == "")
                            {
                                oDato = new InventarioPlantillasAtributosJson
                                {
                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.Codigo,
                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                    TextLista = ValorOri,
                                    Valor = Valor
                                };
                            }
                            else
                            {
                                listas.TryGetValue(oAtributoConf.Codigo, out oAux);
                                if (oAux != null)
                                {
                                    listaValores = (JsonObject)oAux;
                                    if (listaValores.TryGetValue(Valor, out oAux))
                                    {
                                        jsonAux = (JsonObject)oAux;
                                        if (jsonAux.TryGetValue("Value", out oAux) && jsonAux.TryGetValue("Text", out Text))
                                        {
                                            oDato = new InventarioPlantillasAtributosJson
                                            {
                                                AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                                NombreAtributo = oAtributoConf.Codigo,
                                                TipoDato = oAtributoConf.TiposDatos.Codigo,
                                                TextLista = Text.ToString(),
                                                Valor = oAux.ToString()
                                            };
                                        }
                                        else
                                        {
                                            oDato = null;
                                            descError = "Error loading list values.";
                                        }
                                    }
                                    else
                                    {
                                        oDato = null;
                                        descError = "Value does not match List Items restrictions.";
                                    }
                                }
                                else
                                {
                                    oDato = null;
                                    descError = "Error loading list values.";
                                }
                            }

                            break;
                        case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                            listaValores = null;
                            oAux = null;
                            string sValorAux = "";
                            Text = null;
                            string sTextAux = "";
                            if (!Obligatorio && Valor == "")
                            {
                                oDato = new InventarioPlantillasAtributosJson
                                {
                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.Codigo,
                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                    TextLista = ValorOri,
                                    Valor = sValorAux
                                };
                            }
                            else
                            {
                                listas.TryGetValue(oAtributoConf.Codigo, out oAux);
                                if (oAux != null)
                                {
                                    listaValores = (JsonObject)oAux;
                                    bool correcto = true;
                                    foreach (var item in Valor.Split(',').ToList())
                                    {
                                        if (listaValores.TryGetValue(item, out oAux) && correcto)
                                        {
                                            jsonAux = (JsonObject)oAux;
                                            if (jsonAux.TryGetValue("Value", out oAux) && jsonAux.TryGetValue("Text", out Text))
                                            {
                                                if (sValorAux == "")
                                                {
                                                    sValorAux += oAux.ToString();
                                                    sTextAux += Text.ToString();
                                                }
                                                else
                                                {
                                                    sValorAux += "," + oAux.ToString();
                                                    sTextAux += "," + Text.ToString();
                                                }
                                            }
                                            else
                                            {
                                                correcto = false;
                                                descError = "Error loading list values.";
                                            }
                                        }
                                        else
                                        {
                                            correcto = false;
                                            descError = "Value does not match List Items restrictions.";
                                        }
                                    }
                                    if (correcto)
                                    {
                                        oDato = new InventarioPlantillasAtributosJson
                                        {
                                            AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                            NombreAtributo = oAtributoConf.Codigo,
                                            TipoDato = oAtributoConf.TiposDatos.Codigo,
                                            TextLista = sTextAux,
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
                                    descError = "Error loading list values.";
                                }

                            }

                            break;
                        case Comun.TIPODATO_CODIGO_BOOLEAN:
                            bool auxBool;

                            if (Valor == "")
                            {
                                oDato = null;
                                descError = "Value of a boolean cannot be empty.";
                            }
                            else if (bool.TryParse(Valor, out auxBool))
                            {
                                oDato = new InventarioPlantillasAtributosJson
                                {
                                    AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.Codigo,
                                    TipoDato = oAtributoConf.TiposDatos.Codigo,
                                    TextLista = Valor,
                                    Valor = Valor
                                };
                            }
                            else
                            {
                                oDato = null;
                                descError = "Value does not match Boolean format restrictions.";
                            }
                            break;

                        case Comun.TIPODATO_CODIGO_TEXTAREA:
                            oDato = new InventarioPlantillasAtributosJson
                            {
                                AtributoID = oAtributoConf.CoreAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.Codigo,
                                TipoDato = oAtributoConf.TiposDatos.Codigo,
                                TextLista = Valor,

                                Valor = Valor
                            };
                            break;
                        default:
                            oDato = null;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                descError = "An exception has occurred.Please check the log file.";
                oDato = null;
                Obligatorio = false;
            }
            return oDato;
        }

        public CoreInventarioPlantillasAtributosCategoriasAtributos GetAtributo(long lPlantillaID, long lAtributoID)
        {
            // Local variable
            CoreInventarioPlantillasAtributosCategoriasAtributos oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioPlantillasAtributosCategoriasAtributos where c.CoreInventarioPlantillaAtributoCategoriaID == lPlantillaID && c.CoreAtributoConfiguracionID == lAtributoID select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }

            return oDato;
        }

    }
}