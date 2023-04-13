using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Reflection;


namespace CapaNegocio
{
    public sealed class ParametrosController : GeneralBaseController<Parametros, TreeCoreContext>
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ParametrosController()
            : base()
        {
            
        }

        public bool RegistroDuplicado(string parametro)
        {
            bool isExiste = false;
            List<Parametros> datos = new List<Parametros>();


            datos = (from c in Context.Parametros where (c.Parametro == parametro) select c).ToList<Parametros>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        #region FUNCIONES GENERALES

        /// <summary>
        /// obtiene el valor del parametro por el nombre del elemento.
        /// </summary>
        /// <param name="paramname">cadena de texto (nombre del parámetro)</param>
        /// <returns>Valor</returns>
        public string GetItemValor(string paramname)
        {
            try
            {
                Parametros param = new Parametros();

                param = GetItem("Parametro == \"" + paramname + "\"");

                if (param != null)
                {
                    return param.Valor;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return "";
            }

        }

        public List<Comun.Parametroslist> ObtenerListaParametro(string parametro)
        {
            string valor = "";
            List<Comun.Parametroslist> lista = new List<Comun.Parametroslist>();
            valor = ObtenerParametro(parametro);
            if (valor.Length > 0)
            {
                string[] Estados = valor.Split(',');
                for (int i = 0; i <= Estados.Length - 1; i++)
                {
                    Comun.Parametroslist param = new Comun.Parametroslist();
                    param.Codigo = Estados[i];
                    param.Valor = Estados[i];
                    lista.Add(param);
                }
            }
            return lista;
        }

        public List<Comun.Parametroslist> ObtenerListaPendientes(string sParametro, string[] sCodigo)
        {
            string valor = "";
            List<Comun.Parametroslist> lista = new List<Comun.Parametroslist>();
            valor = ObtenerParametro(sParametro);
            if (valor.Length > 0)
            {
                string[] Estados = valor.Split(',');
                for (int i = 0; i <= Estados.Length - 1; i++)
                {
                    Comun.Parametroslist param = new Comun.Parametroslist();

                    for (int j = 0; j <= sCodigo.Length - 1; j++)
                    {
                        if (!Estados[i].Equals(sCodigo[j]))
                        {
                            param.Codigo = Estados[i];
                            param.Valor = Estados[i];
                            lista.Add(param);
                        }
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// obtiene el parámetro por el nombre del elemento.
        /// </summary>
        /// <param name="paramname">cadena de texto (nombre del parámetro)</param>
        /// <returns>Parámetros</returns>
        public Parametros GetItemByName(string paramname)
        {
            try
            {
                Parametros param = new Parametros();

                param = GetItem("Parametro == \"" + paramname + "\"");

                if (param != null)
                {
                    return param;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        private bool ExisteParamentro(string paramname)
        {
            if (GetItemByName(paramname) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool UpdateItem(Parametros item)
        {
            Parametros param = GetItemByName(item.Parametro);
            if ((ExisteParamentro(item.Parametro)) && (param.ParametroID != item.ParametroID))
            {
                throw new Exception(Resources.Comun.strParametrosExiste);
            }
            else
            {
                return base.UpdateItem(item);
            }
        }

        public override Parametros AddItem(Parametros item)
        {
            if (ExisteParamentro(item.Parametro))
            {
                throw new Exception(Resources.Comun.strParametrosExiste);
            }
            else
            {
                return base.AddItem(item);
            }
        }

        /// <summary>
        /// Devuelve el parámetro solicitado o vacío en caso de no encontrarlo
        /// </summary>
        /// <param name="parametro">Parametro a buscar</param>
        /// <returns>Valor resultante</returns>
        /// <remarks>MRL - 01/07/2012 - Versión Inicial</remarks>
        public string ObtenerParametro(string parametro)
        {
            Parametros dato = GetItemByName(parametro);
            if (dato == null)
            {
                return "";
            }
            else
            {
                return dato.Valor;
            }
        }

        /// <summary>
        /// Guarda o actualiza el parámetro dependiendo de los datos del objeto
        /// </summary>
        /// <param name="parametro">cadena de texto (Parámetro)</param>
        /// <param name="valor">cadena de texto (Valor)</param>
        /// <returns>bool</returns>
        /// <remarks>JJFD</remarks>
        public bool GuardarParametro(string parametro, string valor)
        {
            Parametros dato;
            //obtiene el parámetro dependiendo del campo parametro que se le pasa a la función
            dato = GetItemByName(parametro);
            //si no trae ningún parámetro inseta el nuevo parámetro
            if (dato == null)
            {
                dato = new Parametros();
                dato.Parametro = parametro;
                dato.Valor = valor;
                AddItem(dato);
                if (dato == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //si trae el parámetro lo actualiza
            else
            {
                dato.Valor = valor;
                if (UpdateItem(dato))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        #endregion
    }
}
