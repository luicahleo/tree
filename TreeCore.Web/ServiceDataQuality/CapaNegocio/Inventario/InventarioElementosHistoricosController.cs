using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using Newtonsoft.Json.Linq;
using TreeCore.Data;
using Ext.Net;
using System.Globalization;

namespace CapaNegocio
{
    public class InventarioElementosHistoricosController : GeneralBaseController<InventarioElementosHistoricos, TreeCoreContext>
    {
        public InventarioElementosHistoricosController()
            : base()
        { }

        public List<long> GetLastModified(int days, long clienteID)
        {
            List<long> IDs;
            DateTime fecha = DateTime.Today;

            fecha = fecha.AddDays(-days);

            try
            {
                IDs = (from c in Context.InventarioElementosHistoricos
                       where c.FechaModificacion > fecha
                       select c.ElementoID).ToList();
            }
            catch (Exception ex)
            {
                IDs = new List<long>();
                log.Error(ex.Message);
            }

            return IDs;
        }

        public InventarioElementosHistoricos getHistoricoByID(long inventarioID)
        {
            InventarioElementosHistoricos res;

            try
            {
                res = (from c in Context.InventarioElementosHistoricos
                       where c.ElementoID == inventarioID
                       orderby c.FechaModificacion descending
                       select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                res = null;
                log.Error(ex.Message);
            }

            return res;
        }
        public List<Vw_InventarioElementosHistoricos> getVwHistoricosByID(long inventarioID)
        {
            List<Vw_InventarioElementosHistoricos> res;

            try
            {
                res = (from c in Context.Vw_InventarioElementosHistoricos
                       where c.ElementoID == inventarioID
                       orderby c.FechaModificacion descending
                       select c).ToList();
            }
            catch (Exception ex)
            {
                res = null;
                log.Error(ex.Message);
            }

            return res;
        }

        public List<Vw_InventarioElementosHistoricos> getVwHistoricosByHistoricosIDs(List<long> listaIDs)
        {
            List<Vw_InventarioElementosHistoricos> res;

            try
            {
                res = (from c in Context.Vw_InventarioElementosHistoricos
                       where listaIDs.Contains(c.InventarioElementoHistoricoID)
                       orderby c.FechaModificacion descending
                       select c).ToList();
            }
            catch (Exception ex)
            {
                res = null;
                log.Error(ex.Message);
            }

            return res;
        }

        public bool CrearHistorico(InventarioElementos oDato)
        {

            bool Correcto = true;
            CoreAtributosConfiguraciones oAtr;
            List<CoreInventarioPlantillasAtributosCategoriasAtributos> listaAtrPlantillas;
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            cAtributosConf.SetDataContext(this.Context);
            UsuariosController cUsuarios = new UsuariosController();
            cUsuarios.SetDataContext(this.Context);
            CoreInventarioPlantillasAtributosCategoriasAtributosController cAtrPlantillas = new CoreInventarioPlantillasAtributosCategoriasAtributosController();
            cAtrPlantillas.SetDataContext(this.Context);
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            cPlantillas.SetDataContext(this.Context);
            List<long> listaPlaIds;

            string sValor;

            try
            {


                InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
                cHistorico.SetDataContext(this.Context);
                InventarioElementosHistoricos oHistorico = new InventarioElementosHistoricos();

                oHistorico.NombreElemento = oDato.Nombre;
                oHistorico.NumeroElemento = oDato.NumeroInventario;

                InventarioElementosAtributosEstadosController cInvElemAtbEstados = new InventarioElementosAtributosEstadosController();
                cInvElemAtbEstados.SetDataContext(this.Context);

                if (oDato.InventarioElementoAtributoEstadoID != null)
                {
                    oHistorico.Estado = cInvElemAtbEstados.GetItem(Convert.ToInt64(oDato.InventarioElementoAtributoEstadoID)).Nombre;
                }

                oHistorico.CodigoEstado = "";
                oHistorico.FechaDesde = (DateTime)oDato.UltimaModificacionFecha;
                oHistorico.Categoria = "";
                oHistorico.ElementoID = oDato.InventarioElementoID;
                oHistorico.CategoriaID = oDato.InventarioCategoriaID;
                oHistorico.FechaModificacion = (DateTime)oDato.UltimaModificacionFecha;
                oHistorico.InventarioElementoPadreID = oDato.InventarioElementoPadreID;
                oHistorico.PlantillaID = oDato.PlantillaID;
                oHistorico.OperadorID = oDato.OperadorID;
                oHistorico.EstadoID = oDato.InventarioElementoAtributoEstadoID;
                oHistorico.UsuarioID = oDato.UltimaModificacionUsuarioID;

                OperadoresController cOperadores = new OperadoresController();
                cOperadores.SetDataContext(this.Context);
                string sOperador = cOperadores.GetItem("OperadorID=" + oHistorico.OperadorID.ToString()).Operador;

                InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();
                cEstados.SetDataContext(this.Context);
                string sEstado = cEstados.GetItem((long)oDato.InventarioElementoAtributoEstadoID).Nombre;

                InventarioElementosController cInvElementos = new InventarioElementosController();
                cInvElementos.SetDataContext(this.Context);
#if SERVICEIMPORTEXPORT
                JObject json = ComunServicios.ObjectToJSON(oDato, "InventarioElementos");
#elif TREEAPI
                JObject json = Comun.ObjectToJSON(oDato, "InventarioElementos");
#else
                JObject json = Comun.ObjectToJSON(oDato, "InventarioElementos");
#endif
                //var sPadre = "";
                //if (oDato.InventarioElementoPadreID != null && oDato.InventarioElementoPadreID != 0)
                //JObject json = new JObject();
                //{
                //    sPadre = cInvElementos.GetItem("InventarioElementoID=" + oDato.InventarioElementoPadreID).Nombre;
                //}
                //json.Add("strInventarioElementoPadre", sPadre);
                JsonObject listaValores, jsonAux;
                object oAux = null;
                JToken OJsonT;

                json.Add("strFechaModificacion", ((DateTime)oDato.UltimaModificacionFecha).ToString(Comun.FORMATO_FECHA));

                json.Add("strUsuario", cUsuarios.GetItem((long)oDato.UltimaModificacionUsuarioID).NombreCompleto);

                json.Add("strEstado", sEstado);

                json.Add("strOperador", sOperador);

                foreach (var oValAtr in cAtrJson.Deserializacion(oDato.JsonAtributosDinamicos))
                {
                    if (!json.TryGetValue("Atr" + oValAtr.AtributoID, out OJsonT))
                    {
                        json.Add("Atr" + oValAtr.AtributoID, oValAtr.TextLista);
                    }
                }

                listaPlaIds = new List<long>();

                foreach (var oPla in cPlaJson.Deserializacion(oDato.JsonPlantillas))
                {
                    if (!json.TryGetValue("Pla" + oPla.InvCatConfID, out OJsonT))
                    {
                        json.Add("Pla" + oPla.InvCatConfID, oPla.NombrePlantilla);
                        listaPlaIds.Add(oPla.PlantillaID);
                    }
                }

                var listaAtrPla = cPlantillas.GetPlantillas(listaPlaIds);

                foreach (var atr in listaAtrPla)
                {
                    foreach (var oValAtr in cPlaAtrJson.Deserializacion(atr.JsonAtributosDinamicos))
                    {
                        if (!json.TryGetValue("Atr" + oValAtr.AtributoID, out OJsonT))
                        {
                            json.Add("Atr" + oValAtr.AtributoID, oValAtr.TextLista);
                        }
                    }
                }

                oHistorico.HistoricoAtributosDinamicos = json.ToString();
                if (AddItem(oHistorico) == null)
                {
                    Correcto = false;
                }
            }
            catch (Exception ex)
            {
                Correcto = false;
                log.Error(ex.Message);
            }

            return Correcto;
        }

        public bool CrearHistoricoAPI(InventarioElementos oDato, List<object> listaAtributos)
        {
            bool Correcto = true;
            try
            {
                InventarioElementosHistoricos oHistorico = new InventarioElementosHistoricos();
                oHistorico.NombreElemento = oDato.Nombre;
                oHistorico.NumeroElemento = oDato.NumeroInventario;

                InventarioElementosAtributosEstadosController cInvElemAtbEstados = new InventarioElementosAtributosEstadosController();
                oHistorico.Estado = cInvElemAtbEstados.GetItem(Convert.ToInt64(oDato.InventarioElementoAtributoEstadoID)).Nombre;

                oHistorico.CodigoEstado = "";
                oHistorico.FechaDesde = DateTime.Now;
                oHistorico.Categoria = "";
                oHistorico.ElementoID = oDato.InventarioElementoID;
                oHistorico.CategoriaID = oDato.InventarioCategoriaID;
                oHistorico.FechaModificacion = DateTime.Now;
                oHistorico.InventarioElementoPadreID = oDato.InventarioElementoPadreID;
                oHistorico.PlantillaID = oDato.PlantillaID;
                oHistorico.OperadorID = oDato.OperadorID;
                oHistorico.EstadoID = oDato.InventarioElementoAtributoEstadoID;
                oHistorico.UsuarioID = oDato.UltimaModificacionUsuarioID;

                OperadoresController cOperadores = new OperadoresController();
                cOperadores.SetDataContext(this.Context);
                string sOperador = cOperadores.GetItem("OperadorID=" + oHistorico.OperadorID.ToString()).Operador;

                if (listaAtributos != null)
                {
                    var AtributoID = listaAtributos.First().GetType().GetProperty("AtributoID");
                    var NombreAtributo = listaAtributos.First().GetType().GetProperty("NombreAtributo");
                    var Valor = listaAtributos.First().GetType().GetProperty("Valor");

#if SERVICEIMPORTEXPORT
                    JObject json = ComunServicios.ObjectToJSON(oDato, "InventarioElementos");
#elif TREEAPI
                JObject json = Comun.ObjectToJSON(oDato, "InventarioElementos");
#else
                    JObject json = Comun.ObjectToJSON(oDato, "InventarioElementos");
#endif

                    InventarioElementosController cInvElementos = new InventarioElementosController();

                    var sPadre = oDato.Nombre;
                    if (oDato.InventarioElementoPadreID != null && oDato.InventarioElementoPadreID != 0)
                    {
                        sPadre = cInvElementos.GetItem("InventarioElementoID=" + oDato.InventarioElementoPadreID).Nombre;
                    }

                    json.Add("InventarioElementoPadre", sPadre);

                    var sPlantilla = "";
                    if (oDato.PlantillaID != null && oDato.PlantillaID != 0)
                    {
                        sPlantilla = cInvElementos.GetItem("InventarioElementoID=" + oDato.PlantillaID).Nombre;
                    }
                    json.Add("strPlantilla", sPlantilla);

                    foreach (var item in listaAtributos)
                    {

                        json.Add(NombreAtributo.GetValue(item).ToString(), Valor.GetValue(item).ToString());
                    }

                    json.Add("strEstado", oHistorico.Estado);

                    json.Add("strOperador", sOperador);

                    oHistorico.HistoricoAtributosDinamicos = json.ToString();
                }
                if (this.AddItem(oHistorico) == null)
                {
                    Correcto = false;
                }
            }
            catch (Exception ex)
            {
                Correcto = false;
                log.Error(ex.Message);
            }
            return Correcto;
        }

        public bool CrearHistoricoCambioPadre(InventarioElementos oDato)
        {
            bool Correcto = true;
            try
            {
                InventarioElementosController cInvElementos = new InventarioElementosController();
                InventarioElementosHistoricos oHistoricoAnt = (from c in Context.InventarioElementosHistoricos where c.ElementoID == oDato.InventarioElementoID select c).OrderBy(item => item.FechaModificacion).ToList().Last();
                InventarioElementosHistoricos oHistorico = new InventarioElementosHistoricos();

                oHistorico.NombreElemento = oHistoricoAnt.NombreElemento;
                oHistorico.NumeroElemento = oHistoricoAnt.NumeroElemento;
                oHistorico.Estado = oHistoricoAnt.Estado;
                oHistorico.CodigoEstado = oHistoricoAnt.CodigoEstado;
                oHistorico.FechaDesde = oHistoricoAnt.FechaDesde;
                oHistorico.Categoria = oHistoricoAnt.Categoria;
                oHistorico.ElementoID = oHistoricoAnt.ElementoID;
                oHistorico.CategoriaID = oHistoricoAnt.CategoriaID;
                oHistorico.FechaModificacion = DateTime.Now;
                oHistorico.InventarioElementoPadreID = oDato.InventarioElementoPadreID;
                oHistorico.PlantillaID = oHistoricoAnt.PlantillaID;
                oHistorico.OperadorID = oHistoricoAnt.OperadorID;
                oHistorico.EstadoID = oHistoricoAnt.EstadoID;
                oHistorico.UsuarioID = oDato.UltimaModificacionUsuarioID;

                JObject oJson = JObject.Parse(oHistoricoAnt.HistoricoAtributosDinamicos);

                var sPadre = "";
                if (oDato.InventarioElementoPadreID != null && oDato.InventarioElementoPadreID != 0)
                {
                    sPadre = cInvElementos.GetItem("InventarioElementoID=" + oDato.InventarioElementoPadreID).Nombre;
                }
                oJson["strInventarioElementoPadre"] = sPadre;

                oHistorico.HistoricoAtributosDinamicos = oJson.ToString();

                if (this.AddItem(oHistorico) == null)
                {
                    Correcto = false;
                }

            }
            catch (Exception ex)
            {
                Correcto = false;
                log.Error(ex.Message);
            }
            return Correcto;
        }



    }
}