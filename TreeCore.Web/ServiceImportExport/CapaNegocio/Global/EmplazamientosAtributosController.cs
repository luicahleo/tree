using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Globalization;

namespace CapaNegocio
{
    public class EmplazamientosAtributosController : GeneralBaseController<EmplazamientosAtributos, TreeCoreContext>
    {
        public EmplazamientosAtributosController()
            : base()
        { }

        public List<EmplazamientosAtributos> GetAtributosEmplazamientos(long lEmplazamientoID)
        {
            List<EmplazamientosAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributos
                              where c.EmplazamientoID == lEmplazamientoID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<EmplazamientosAtributos>();
            }
            return listaDatos;
        }

        public EmplazamientosAtributos GetAtributo(long Emplazamiento, long lAtributoID)
        {
            // Local variable
            EmplazamientosAtributos oDato;
            try
            {
                oDato = (from c in Context.EmplazamientosAtributos where c.EmplazamientoID == Emplazamiento && c.EmplazamientoAtributoConfiguracionID == lAtributoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public EmplazamientosAtributosJson comprobarValidedAttr(out string descError, EmplazamientosAtributosConfiguracion oAtributoConf, List<Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedades,
            string Valor, bool Obligatorio, JsonObject listas)
        {
            Vw_EmplazamientosAtributosTiposDatosPropiedades oPropiedad;
            bool Valido = true;
            EmplazamientosAtributosJson oDato = null;
            object oAux, Text;
            JsonObject listaValores;
            string ValorOri = Valor;
            JsonObject jsonAux;
            descError = "";
            try
            {
                switch (oAtributoConf.TiposDatos.Codigo)
                {
                    case Comun.TIPODATO_CODIGO_TEXTO:

                        if (!Obligatorio && (Valor == null || Valor == ""))
                        {
                            oDato = new EmplazamientosAtributosJson
                            {
                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.CodigoAtributo,
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
                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.Regex.ToString()).FirstOrDefault();
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
                                oDato = new EmplazamientosAtributosJson
                                {
                                    AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.CodigoAtributo,
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
                            oDato = new EmplazamientosAtributosJson
                            {
                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.CodigoAtributo,
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

                                oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MinValue.ToString()).FirstOrDefault();

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
                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MaxValue.ToString()).FirstOrDefault();
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
                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == "AllowDecimals" && prop.Valor == false.ToString()).FirstOrDefault();

                                    if (oPropiedad != null)
                                    {
                                        if (long.TryParse(Valor, out auxNum2))
                                        {
                                            oDato = new EmplazamientosAtributosJson
                                            {
                                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                                NombreAtributo = oAtributoConf.CodigoAtributo,
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
                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == "DecimalPrecision").FirstOrDefault();

                                    if (oPropiedad != null)
                                    {
                                        oDato = new EmplazamientosAtributosJson
                                        {
                                            AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                            NombreAtributo = oAtributoConf.CodigoAtributo,
                                            TipoDato = oAtributoConf.TiposDatos.Codigo,
                                            TextLista = Valor,
                                            Valor = Math.Round(Convert.ToDouble(Valor.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture), int.Parse(oPropiedad.Valor)).ToString()
                                        };
                                    }
                                    else
                                    {
                                        oDato = new EmplazamientosAtributosJson
                                        {
                                            AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                            NombreAtributo = oAtributoConf.CodigoAtributo,
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
                                oDato = new EmplazamientosAtributosJson
                                {
                                    AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.CodigoAtributo,
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
                        else if (!Obligatorio && (Valor == ""))
                        {
                            oDato = new EmplazamientosAtributosJson
                            {
                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.CodigoAtributo,
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

                                oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MinDate.ToString()).FirstOrDefault();

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

                                    oPropiedad = listaPropiedades.FindAll(prop => prop.Codigo == Comun.TiposPropiedades.MaxDate.ToString()).FirstOrDefault();
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
                                    oDato = new EmplazamientosAtributosJson
                                    {
                                        AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                        NombreAtributo = oAtributoConf.CodigoAtributo,
                                        TipoDato = oAtributoConf.TiposDatos.Codigo,
                                        TextLista = DateTime.Parse(Valor, CultureInfo.InvariantCulture).ToString(Comun.FORMATO_FECHA),
                                        Valor = DateTime.Parse(Valor, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)
                                    };
                                }

                                #endregion
                            }
                            else
                            {
                                oDato = new EmplazamientosAtributosJson
                                {
                                    AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                    NombreAtributo = oAtributoConf.CodigoAtributo,
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
                            oDato = new EmplazamientosAtributosJson
                            {
                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.CodigoAtributo,
                                TipoDato = oAtributoConf.TiposDatos.Codigo,
                                TextLista = ValorOri,
                                Valor = Valor
                            };
                        }
                        else
                        {
                            listas.TryGetValue(oAtributoConf.CodigoAtributo, out oAux);
                            if (oAux != null)
                            {
                                listaValores = (JsonObject)oAux;
                                if (listaValores.TryGetValue(Valor, out oAux))
                                {
                                    jsonAux = (JsonObject)oAux;
                                    if (jsonAux.TryGetValue("Value", out oAux) && jsonAux.TryGetValue("Text", out Text))
                                    {
                                        oDato = new EmplazamientosAtributosJson
                                        {
                                            AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                            NombreAtributo = oAtributoConf.CodigoAtributo,
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
                            oDato = new EmplazamientosAtributosJson
                            {
                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.CodigoAtributo,
                                TipoDato = oAtributoConf.TiposDatos.Codigo,
                                TextLista = Valor,
                                Valor = Valor
                            };
                        }
                        else
                        {
                            listas.TryGetValue(oAtributoConf.CodigoAtributo, out oAux);
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
                                    oDato = new EmplazamientosAtributosJson
                                    {
                                        AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                        NombreAtributo = oAtributoConf.CodigoAtributo,
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
                            oDato = new EmplazamientosAtributosJson
                            {
                                AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                                NombreAtributo = oAtributoConf.CodigoAtributo,
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
                        oDato = new EmplazamientosAtributosJson
                        {
                            AtributoID = oAtributoConf.EmplazamientoAtributoConfiguracionID,
                            NombreAtributo = oAtributoConf.CodigoAtributo,
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
            catch (Exception ex)
            {
                log.Error(ex.Message);
                descError = "An exception has occurred.Please check the log file.";
                oDato = null;
                Obligatorio = false;
            }
            return oDato;
        }

        public EmplazamientosAtributosJson SaveUpdateAtributo(out string descError, long lClienteID, string sCodAtributo, string Valor, JsonObject listas, out bool Obligatorio, List<EmplazamientosAtributosConfiguracion> listaAtributosCargadas = null, JsonObject jsonRestricciones = null)
        {
            EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
            cAtributosConf.SetDataContext(this.Context);
            EmplazamientosAtributosConfiguracion oAtributoConf;
            EmplazamientosAtributosJson oDato = null;
            EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
            cPropiedades.SetDataContext(this.Context);
            List<Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedades;
            Vw_EmplazamientosAtributosTiposDatosPropiedades oPropiedad;

            Obligatorio = false; descError = "";
            try
            {
                if (listaAtributosCargadas == null)
                {
                    oAtributoConf = cAtributosConf.GetAtributoByCodigo(lClienteID, sCodAtributo);
                }
                else
                {
                    try
                    {
                        oAtributoConf = (from c in listaAtributosCargadas where c.NombreAtributo == sCodAtributo select c).First();
                    }
                    catch (Exception)
                    {
                        oAtributoConf = cAtributosConf.GetAtributoByCodigo(lClienteID, sCodAtributo);
                    }
                }

                #region PROPIEDADES

                object oAux;
                if (jsonRestricciones == null)
                {
                    listaPropiedades = cPropiedades.GetPropiedadesFromAtributo(oAtributoConf.EmplazamientoAtributoConfiguracionID);
                }
                else
                {
                    if (jsonRestricciones.TryGetValue(oAtributoConf.EmplazamientoAtributoConfiguracionID.ToString(), out oAux))
                    {
                        listaPropiedades = (List<Vw_EmplazamientosAtributosTiposDatosPropiedades>)oAux;
                    }
                    else
                    {
                        listaPropiedades = cPropiedades.GetPropiedadesFromAtributo(oAtributoConf.EmplazamientoAtributoConfiguracionID);
                    }
                }

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
                    catch (Exception)
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
                    oDato = comprobarValidedAttr(out descError, oAtributoConf, listaPropiedades, Valor, Obligatorio, listas);
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

        public static DateTime GetFechaAPI(string sDate)
        {
            DateTime oDate = new DateTime();
            try
            {
                if (sDate.Length == 10)
                {
                    int dia = int.Parse(sDate.Substring(0, 2));
                    int mes = int.Parse(sDate.Substring(3, 2));
                    int anyo = int.Parse(sDate.Substring(6, 4));

                    oDate = new DateTime(anyo, mes, dia);
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

        public Vw_EmplazamientosAtributos GetItemVista(long emplazamientoAtributoID)
        {
            Vw_EmplazamientosAtributos item;
            try
            {
                item = (from c in Context.Vw_EmplazamientosAtributos
                        where c.EmplazamientoAtributoID == emplazamientoAtributoID
                        select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                item = null;
            }
            return item;
        }

        public bool HasByNombre(string sCategoryAttribute, long ClienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosAtributosCategorias
                          where c.Nombre == sCategoryAttribute && c.ClienteID == ClienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }

        public bool hasCategoriaAtributo(long EmplazamientoID, string sCategoriaAtributo)
        {
            bool hayAtributosConCategoria = false;
            try
            {
                hayAtributosConCategoria = (from c in Context.Vw_EmplazamientosAtributos
                                            join cat in Context.EmplazamientosAtributosCategorias on c.EmplazamientoAtributoCategoriaID equals cat.EmplazamientoAtributoCategoriaID
                                            where c.EmplazamientoID == EmplazamientoID && cat.Nombre == sCategoriaAtributo
                                            select cat).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                hayAtributosConCategoria = false;
            }
            return hayAtributosConCategoria;
        }

    }

}