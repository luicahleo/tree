using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using Ext.Net;

namespace CapaNegocio
{
    public class GlobalCondicionesReglasController : GeneralBaseController<GlobalCondicionesReglas, TreeCoreContext>
    {
        public GlobalCondicionesReglasController()
          : base()
        {

        }

        public List<Vw_GlobalCondicionesReglas> GetVwByCampoDestino(string campoDestino)
        {
            List<Vw_GlobalCondicionesReglas> menus;
            try
            {
                menus = (from c
                         in Context.Vw_GlobalCondicionesReglas
                         where c.CampoDestino == campoDestino
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                menus = null;
            }
            return menus;
        }

        public bool RegistroDuplicadoNombre(string Nombre)
        {
            bool isExiste = false;
            List<GlobalCondicionesReglas> datos = new List<GlobalCondicionesReglas>();
            datos = (from c in Context.GlobalCondicionesReglas where (c.NombreRegla == Nombre) select c).ToList<GlobalCondicionesReglas>();
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }


        public List<string> getColumnName(string NombreTabla)
        {
            List<string> columnnames;

            try
            {
                columnnames = (from t in Context.GetType().GetProperties().ToList().FindAll(tabla => tabla.Name == NombreTabla).GetType().GetProperties() select t.Name).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                columnnames = null;
            }

            return columnnames;
        }


        public bool RegistroDuplicadoProyecto(long VarProyectoTipoID, String VarCampoDestino)
        {
            bool isExiste = false;
            List<GlobalCondicionesReglas> datos = new List<GlobalCondicionesReglas>();
            datos = (from c in Context.GlobalCondicionesReglas where c.CampoDestino == VarCampoDestino && c.ProyectoTipoID == VarProyectoTipoID select c).ToList<GlobalCondicionesReglas>();
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public GlobalCondicionesReglas GetDefault(string lCampoDestino)
        {
            GlobalCondicionesReglas oEstado;

            try
            {
                oEstado = (from c in Context.GlobalCondicionesReglas where c.Defecto && c.CampoDestino == lCampoDestino select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public bool ExsisteOrden()
        {
            bool Exsiste = false;
            List<GlobalCondicionesReglas> lCondiciones = new List<GlobalCondicionesReglas>();
            lCondiciones = (from c in Context.GlobalCondicionesReglas select c).ToList();
            if (lCondiciones.Count > 0)
            {
                Exsiste = true;
            }

            return Exsiste;
        }

        public GlobalCondicionesReglas GetReglaByCampoDestino(string campoDestino, long ProyectoTipoID)
        {
            GlobalCondicionesReglas reglaAplicar = null;
            List<GlobalCondicionesReglas> listaReglas = new List<GlobalCondicionesReglas>();
            try
            {
                listaReglas = (from c in Context.GlobalCondicionesReglas
                               where c.Activo == true && c.CampoDestino == campoDestino && c.ProyectoTipoID == ProyectoTipoID
                               select c).ToList();

                if (listaReglas == null || listaReglas.Count.Equals(0))
                {
                    listaReglas = (from c in Context.GlobalCondicionesReglas
                                   where c.Activo == true && c.CampoDestino == campoDestino && c.Defecto == true
                                   select c).ToList();

                    if (listaReglas != null && listaReglas.Count > 0)
                    {
                        reglaAplicar = listaReglas[0];
                    }
                }
                else
                {
                    reglaAplicar = listaReglas[0];
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                reglaAplicar = null;
            }

            return reglaAplicar;
        }

        public JsonObject getConfiguracionRegla(long lReglaID, JsonObject listaIDs = null)
        {
            object oAux;
            JsonObject json = new JsonObject();
            JsonObject aux;
            try
            {
                List<GlobalCondicionesReglasConfiguraciones> listaCondiciones = (from c in Context.GlobalCondicionesReglasConfiguraciones where c.GlobalCondicionReglaID == lReglaID orderby c.Orden ascending select c).ToList();
                foreach (var oCondicion in listaCondiciones)
                {
                    aux = new JsonObject();
                    aux.Add("TipoCondicion", oCondicion.TipoCondicion);
                    if (oCondicion.TipoCondicion == "Formulario")
                    {
                        aux.Add("Tabla", oCondicion.ColumnasModeloDatos.TablasModeloDatos.NombreTabla.Replace("dbo.", ""));
                        aux.Add("Campo", oCondicion.ColumnasModeloDatos.NombreColumna);
                        aux.Add("Longitud", oCondicion.LongitudCadena);
                        aux.Add("Valor", string.Concat(Enumerable.Repeat("X", oCondicion.LongitudCadena)));
                    }
                    else if (oCondicion.TipoCondicion == "Tabla")
                    {
                        if (listaIDs != null && listaIDs.ContainsKey(oCondicion.ColumnasModeloDatos.TablasModeloDatos.NombreTabla.Replace("dbo.", "")))
                        {
                            if (listaIDs.TryGetValue(oCondicion.ColumnasModeloDatos.TablasModeloDatos.NombreTabla.Replace("dbo.", ""), out oAux))
                            {
                                string query = "select ";
                                query += oCondicion.ColumnasModeloDatos.NombreColumna;
                                query += " from " + oCondicion.ColumnasModeloDatos.TablasModeloDatos.NombreTabla;
                                query += " where " + oCondicion.ColumnasModeloDatos.TablasModeloDatos.Indice + " = " + oAux.ToString();

                                DataTable result = this.EjecutarQuery(query);

                                if (result != null)
                                {
                                    if (oCondicion.LongitudCadena.Equals(0))
                                    {
                                        aux.Add("Valor", result.Rows[0].ItemArray[0].ToString());
                                    }
                                    else
                                    {
                                        aux.Add("Valor", (result.Rows[0].ItemArray[0].ToString().Length > oCondicion.LongitudCadena) ? result.Rows[0].ItemArray[0].ToString().Substring(0, oCondicion.LongitudCadena) : result.Rows[0].ItemArray[0].ToString());
                                    }
                                }
                            }
                            else
                            {
                                aux.Add("Valor", string.Concat(Enumerable.Repeat("X", oCondicion.LongitudCadena)));
                            }
                        }
                        else
                        {
                            aux.Add("Valor", string.Concat(Enumerable.Repeat("X", oCondicion.LongitudCadena)));
                        }
                    }
                    else if (oCondicion.Valor.Count() > oCondicion.LongitudCadena)
                    {
                        json = null;
                        return json;
                    }
                    else
                    {
                        aux.Add("Valor", oCondicion.Valor);
                    }
                    json.Add("Regla" + oCondicion.GlobalCondicionReglaConfiguracionID.ToString(), aux);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                json = null;
            }
            return json;
        }

    }
}