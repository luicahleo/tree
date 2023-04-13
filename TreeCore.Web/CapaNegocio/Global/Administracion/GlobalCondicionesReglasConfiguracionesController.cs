using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using System.Reflection;
using Ext.Net;

namespace CapaNegocio
{
    public class GlobalCondicionesReglasConfiguracionesController : GeneralBaseController<GlobalCondicionesReglasConfiguraciones, TreeCoreContext>
    {
        public GlobalCondicionesReglasConfiguracionesController()
          : base()
        {

        }

        public bool ExsisteOrden(double Orden)
        {
            bool Exsiste = false;
            List<GlobalCondicionesReglasConfiguraciones> lCondiciones = new List<GlobalCondicionesReglasConfiguraciones>();
            lCondiciones = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.Orden == Orden select c).ToList();
            if (lCondiciones.Count > 0)
            {
                Exsiste = true;
            }

            return Exsiste;
        }

        public List<GlobalCondicionesReglasConfiguraciones> GlobalCondicionesReglasConfiguracionesBySeleccionadoID(long seleccionadoID)
        {
            List<GlobalCondicionesReglasConfiguraciones> lCondiciones;

            try
            {
                lCondiciones = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == seleccionadoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lCondiciones = null;
            }
            return lCondiciones;
        }

        public List<Vw_GlobalCondicionesReglasConfiguraciones> VistaGlobalCondicionesReglasConfiguracionesBySeleccionadoID(long seleccionadoID)
        {
            List<Vw_GlobalCondicionesReglasConfiguraciones> lCondiciones;

            try
            {
                lCondiciones = (from c in Context.Vw_GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == seleccionadoID orderby c.Orden select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lCondiciones = null;
            }
            return lCondiciones;
        }

        public string GetSiguienteByListaCondicionesReglasConfiguraciones(List<GlobalCondicionesReglasConfiguraciones> listaReglasConfiguraciones, string UltimoCodigoGenerado, bool ReglasConfiguracionesModificadas, long lClienteID)
        {
            string CodigoFinalSimulado = "";

            try
            {
                #region LOGICA GENERACION CÓDIGO

                foreach (GlobalCondicionesReglasConfiguraciones configuracion in listaReglasConfiguraciones.OrderBy(item => item.Orden).ToList())
                {
                    string Codigo = "";
                    switch (configuracion.TipoCondicion)
                    {
                        case "Auto_Numerico":
                            {
                                if (configuracion.Valor.Any(x => char.IsNumber(x)) && configuracion.Valor.Count() == configuracion.LongitudCadena)
                                {
                                    int longitud = configuracion.LongitudCadena;
                                    int valor = Convert.ToInt32(configuracion.Valor);
                                    int valorSiguiente = valor + 1;
                                    string longValor = "D" + longitud;
                                    Codigo = valor.ToString(longValor);
                                }
                                else
                                {
                                    CodigoFinalSimulado = "";
                                    break;
                                }
                            }
                            break;
                        case "Separador":
                            {
                                if (configuracion.Valor.Count() == configuracion.LongitudCadena)
                                {
                                    Codigo = configuracion.Valor;

                                }
                                else
                                {
                                    CodigoFinalSimulado = "";
                                    break;
                                }
                            }
                            break;
                        case "Auto_Caracter":
                            {
                                if (configuracion.Valor.Any(x => !char.IsNumber(x) && configuracion.Valor.Count() == configuracion.LongitudCadena))
                                {
                                    Codigo = configuracion.Valor;

                                }
                                else
                                {
                                    CodigoFinalSimulado = "";
                                    break;
                                }
                            }
                            break;
                        case "Constante":
                            {
                                if (configuracion.Valor.Count() == configuracion.LongitudCadena)
                                {
                                    Codigo = configuracion.Valor;
                                }
                                else
                                {
                                    CodigoFinalSimulado = "";
                                    break;
                                }
                            }
                            break;
                        case "Tabla":
                            {

                                GlobalCondicionesColumnasTablasController cGlobalCondicionesColumnasTablas = new GlobalCondicionesColumnasTablasController();
                                GlobalCondicionesColumnasTablas DatoColumnas;

                                GlobalCondicionesTablasController cGlobalCondicionesTablas = new GlobalCondicionesTablasController();
                                GlobalCondicionesTablas DatoTablas;


                                /* DatoColumnas = cGlobalCondicionesColumnasTablas.GetItem(configuracion.GlobalCondicionColumnaTablaID.Value);

                                 DatoTablas = cGlobalCondicionesTablas.GetItem(long.Parse(DatoColumnas.GlobalCondicionTablaID.ToString()));

                                 if (DatoTablas != null)
                                 {
                                     String NombreTabla = DatoTablas.NombreTabla.ToString();
                                     String NombreTablaController = NombreTabla + "Controller";

                                     try
                                     {
                                         String sMetodo = "GetDefault";

                                         NombreTabla = "TreeCore.Data." + NombreTabla;
                                         Type clase = Type.GetType(NombreTabla);
                                         Type controller = Type.GetType("CapaNegocio." + NombreTablaController);

                                         ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                                         object objetoConstructor = constuctorController.Invoke(new object[] { });
                                         ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
                                         object objetoClase = constuctorClase.Invoke(new object[] { });

                                         MethodInfo m = controller.GetMethod(sMetodo);
                                         Object invocacion = m.Invoke(objetoConstructor, new object[] { lClienteID });


                                         PropertyInfo[] properties = clase.GetProperties();
                                         foreach (PropertyInfo property in properties)
                                         {
                                             if (property.Name.Equals(DatoColumnas.NombreColumna))
                                             {
                                                 string CodigoSimular = Convert.ToString(property.GetValue(invocacion));

                                                 if (CodigoSimular != "")
                                                 {
                                                     Codigo = CodigoSimular.Substring(0, Convert.ToInt32(configuracion.LongitudCadena.ToString()));
                                                 }
                                             }

                                         }

                                     }
                                     catch (Exception e)
                                     {
                                         CodigoFinalSimulado = "";
                                     }
                                 }*/
                            }
                            break;
                    }
                    CodigoFinalSimulado += Codigo;
                }

                #endregion

                if (!ReglasConfiguracionesModificadas && UltimoCodigoGenerado != null && UltimoCodigoGenerado != "")
                {
                    #region ACTUALIZAR CÓDIGO ACTUAL

                    string[] CodigoGenerado = UltimoCodigoGenerado.Split('!');
                    int j = 0;
                    int longitudes = 0;

                    foreach (GlobalCondicionesReglasConfiguraciones configuracion in listaReglasConfiguraciones)
                    {
                        if (configuracion.TipoCondicion == "Auto_Numerico" || configuracion.TipoCondicion == "Auto_Caracter")
                        {
                            int longitudAuto = configuracion.LongitudCadena;
                            string a = CodigoGenerado[j];
                            j++;
                            string b = CodigoFinalSimulado.Substring(longitudes, longitudAuto);
                            CodigoFinalSimulado = CodigoFinalSimulado.Replace(b, a);
                            longitudes += longitudAuto;
                        }
                        else
                        {
                            int longitudCadena = configuracion.LongitudCadena;
                            longitudes += configuracion.LongitudCadena;
                        }
                    }

                    #endregion

                    #region INCREMENTAR CÓDIGO

                    //string CodigoIncremtado = "";
                    //int longitud = 0;

                    //foreach (GlobalCondicionesReglasConfiguraciones configuracion in listaReglasConfiguraciones)
                    //{
                    //    if (configuracion.TipoCondicion == "Auto_Numerico")
                    //    {
                    //        if (configuracion.Valor.Any(x => char.IsNumber(x)))
                    //        {
                    //            int longitudAutoNumerico = configuracion.LongitudCadena;
                    //            int valor = Convert.ToInt32(CodigoFinalSimulado.Substring(longitud, longitudAutoNumerico));
                    //            int valorSiguiente = valor + 1;
                    //            string longValor = "D" + longitudAutoNumerico;
                    //            longitud += longitudAutoNumerico;

                    //            CodigoIncremtado += valorSiguiente.ToString(longValor);

                    //        }
                    //        else
                    //        {
                    //            CodigoFinalSimulado = "";
                    //            break;
                    //        }
                    //    }
                    //    else if (configuracion.TipoCondicion == "Auto_Caracter")
                    //    {
                    //        if (configuracion.Valor.Any(x => !char.IsNumber(x)))
                    //        {
                    //            CodigoIncremtado += Next(CodigoFinalSimulado.Substring(longitud, configuracion.LongitudCadena)).ToUpper();
                    //            longitud += configuracion.LongitudCadena;

                    //            #region INCREMENTAR CARACTERES

                    //            string Next(string str)
                    //            {
                    //                string result = String.Empty;
                    //                int index = str.Length - 1;
                    //                bool carry;
                    //                do
                    //                {
                    //                    result = Increment(str[index--], out carry) + result;
                    //                }
                    //                while (carry && index >= 0);
                    //                if (index >= 0) result = str.Substring(0, index + 1) + result;
                    //                if (carry) result = "a" + result;
                    //                return result;
                    //            }

                    //            char Increment(char value, out bool carry)
                    //            {
                    //                carry = false;
                    //                if (value >= 'a' && value < 'z' || value >= 'A' && value < 'Z')
                    //                {
                    //                    return (char)((int)value + 1);
                    //                }
                    //                if (value == 'z') return 'A';
                    //                if (value == 'Z')
                    //                {
                    //                    carry = true;
                    //                    return 'a';
                    //                }
                    //                throw new Exception(String.Format("Invalid character value: {0}", value));
                    //            }

                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            CodigoFinalSimulado = "";
                    //            break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        CodigoIncremtado += CodigoFinalSimulado.Substring(longitud, configuracion.LongitudCadena);
                    //        longitud += configuracion.LongitudCadena;
                    //    }

                    //}

                    //CodigoFinalSimulado = CodigoIncremtado;

                    #endregion

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                CodigoFinalSimulado = "";
            }

            return CodigoFinalSimulado;
        }

        public bool ActualizarUltimoCodigoByReglaID(long CondicionReglaID, string UltimoValor)
        {
            bool correcto = true;

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            cCondicionesReglasController.SetDataContext(this.Context);
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            cCondicionesConfiguraciones.SetDataContext(this.Context);

            try
            {
                string Codigo = null;
                int longitudes = 0;

                GlobalCondicionesReglas regla;
                regla = cCondicionesReglasController.GetItem(CondicionReglaID);

                if (regla != null)
                {
                    List<Vw_GlobalCondicionesReglasConfiguraciones> configuraciones = new List<Vw_GlobalCondicionesReglasConfiguraciones>();
                    configuraciones = cCondicionesConfiguraciones.VistaGlobalCondicionesReglasConfiguracionesBySeleccionadoID(CondicionReglaID);
                    foreach (Vw_GlobalCondicionesReglasConfiguraciones configuracion in configuraciones)
                    {
                        switch (configuracion.TipoCondicion)
                        {
                            case "Auto_Numerico":
                                if (configuracion.Valor.Any(x => char.IsNumber(x)) && configuracion.Valor.Count() == configuracion.LongitudCadena)
                                {
                                    Codigo += configuracion.Valor;
                                    int longitud = configuracion.LongitudCadena;
                                    int valor = Convert.ToInt32(configuracion.Valor);
                                    int valorSiguiente = valor + 1;
                                    string longValor = "D" + longitud;
                                    GlobalCondicionesReglasConfiguraciones oNumDato = cCondicionesConfiguraciones.GetItem(configuracion.GlobalCondicionReglaConfiguracionID);
                                    oNumDato.Valor = valorSiguiente.ToString(longValor);
                                    cCondicionesConfiguraciones.UpdateItem(oNumDato);
                                }
                                break;
                            case "Auto_Caracter":
                                Codigo = configuracion.Valor;
                                GlobalCondicionesReglasConfiguraciones oCarDato = cCondicionesConfiguraciones.GetItem(configuracion.GlobalCondicionReglaConfiguracionID);
                                oCarDato.Valor = Next(configuracion.Valor).ToUpper();
                                cCondicionesConfiguraciones.UpdateItem(oCarDato);

                                #region INCREMENTAR CARACTERES

                                string Next(string str)
                                {
                                    string result = String.Empty;
                                    int index = str.Length - 1;
                                    bool carry;
                                    do
                                    {
                                        result = Increment(str[index--], out carry) + result;
                                    }
                                    while (carry && index >= 0);
                                    if (index >= 0) result = str.Substring(0, index + 1) + result;
                                    if (carry) result = "a" + result;
                                    return result;
                                }

                                char Increment(char value, out bool carry)
                                {
                                    carry = false;
                                    if (value >= 'a' && value < 'z' || value >= 'A' && value < 'Z')
                                    {
                                        return (char)((int)value + 1);
                                    }
                                    if (value == 'z') return 'A';
                                    if (value == 'Z')
                                    {
                                        carry = true;
                                        return 'a';
                                    }
                                    throw new Exception(String.Format("Invalid character value: {0}", value));
                                }

                                #endregion

                                break;
                            case "Tabla":
                                Codigo += string.Concat(Enumerable.Repeat("X", configuracion.LongitudCadena));
                                break;
                            case "Separador":
                            case "Constante":
                            default:
                                Codigo += configuracion.Valor;
                                break;
                        }
                    }

                    regla.UltimoGenerado = UltimoValor;
                    regla.Modificada = false;
                    cCondicionesReglasController.UpdateItem(regla);
                }
                else
                {
                    correcto = false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                correcto = false;
            }

            return correcto;
        }

        public bool RegistroDuplicadoCodigo(string VarCodigo, long varReglaID)
        {
            bool isExiste = false;
            List<GlobalCondicionesReglasConfiguraciones> datos = new List<GlobalCondicionesReglasConfiguraciones>();
            datos = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == varReglaID && c.Codigo == VarCodigo select c).ToList<GlobalCondicionesReglasConfiguraciones>();
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoNombre(string VarNombre, long varReglaID)
        {
            bool isExiste = false;
            List<GlobalCondicionesReglasConfiguraciones> datos = new List<GlobalCondicionesReglasConfiguraciones>();
            datos = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == varReglaID && c.NombreCampo == VarNombre select c).ToList<GlobalCondicionesReglasConfiguraciones>();
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoOrden(long VarOrden, long varReglaID)
        {
            bool isExiste = false;
            List<GlobalCondicionesReglasConfiguraciones> datos = new List<GlobalCondicionesReglasConfiguraciones>();
            datos = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == varReglaID && c.Orden == VarOrden select c).ToList<GlobalCondicionesReglasConfiguraciones>();
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public GlobalCondicionesReglasConfiguraciones GetUltimoOrden(long seleccionadoID)
        {
            GlobalCondicionesReglasConfiguraciones oGlobalCondicionesReglasConfiguraciones;
            try
            {
                oGlobalCondicionesReglasConfiguraciones = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == seleccionadoID orderby c.Orden descending select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalCondicionesReglasConfiguraciones = null;
            }
            return oGlobalCondicionesReglasConfiguraciones;
        }
    }
}