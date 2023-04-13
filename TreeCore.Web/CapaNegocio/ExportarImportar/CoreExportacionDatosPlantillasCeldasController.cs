using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using TreeCore;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreExportacionDatosPlantillasCeldasController : GeneralBaseController<CoreExportacionDatosPlantillasCeldas, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasCeldasController()
            : base()
        { }

        public List<CoreExportacionDatosPlantillasCeldas> GetByPlantillaID(long plantillaID)
        {
            List<CoreExportacionDatosPlantillasCeldas> lista;

            try
            {
                lista = (from celda in Context.CoreExportacionDatosPlantillasCeldas
                         join fila in Context.CoreExportacionDatosPlantillasFilas on celda.CoreExportacionDatosPlantillaFilaID equals fila.CoreExportacionDatosPlantillaFilaID
                         where fila.CoreExportacionDatosPlantillaID == plantillaID
                         select celda).ToList();
            }
            catch(Exception ex)
            {
                lista = new List<CoreExportacionDatosPlantillasCeldas>();
                log.Error(ex);
            }

            return lista;
        }

        public bool DeleteByColumna(long columnaID)
        {
            CoreExportacionDatosPlantillasReglasCeldasController cCoreExportacionDatosPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
            cCoreExportacionDatosPlantillasReglasCeldas.SetDataContext(Context);

            List<CoreExportacionDatosPlantillasCeldas> celdas;
            List<CoreExportacionDatosPlantillasReglasCeldas> reglas;

            bool result = true;

            try
            {
                celdas = (from celda in Context.CoreExportacionDatosPlantillasCeldas
                          where celda.CoreExportacionDatosPlantillaColumnaID == columnaID
                          select celda).ToList();
                reglas = (from c in Context.CoreExportacionDatosPlantillasReglasCeldas
                          join a in Context.CoreExportacionDatosPlantillasCeldas on c.CoreExportacionDatosPlantillasCeldaID equals a.CoreExportacionDatosPlantillasCeldasID
                          select c).ToList();

                reglas.ForEach(regla =>{
                    cCoreExportacionDatosPlantillasReglasCeldas.DeleteItem(regla.CoreExportacionDatosPlantillasReglaCeldaID);
                });
                celdas.ForEach(celda => {
                    DeleteItem(celda.CoreExportacionDatosPlantillasCeldasID);
                });
            }
            catch(Exception ex)
            {
                result = false;
                log.Error(ex);
            }

            return result;
        }

        public bool DeleteByFila(long FilaID)
        {
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(Context);

            List<CoreExportacionDatosPlantillasCeldas> celdas;
            bool result = true;

            try
            {
                celdas = (from c in Context.CoreExportacionDatosPlantillasCeldas
                          where c.CoreExportacionDatosPlantillaFilaID == FilaID
                          select c).OrderByDescending(c => c.CoreExportacionDatosPlantillasCeldasID).ToList();

                if (celdas != null)
                {
                    celdas = celdas.FindAll(x => x.CeldaPadreID == null);
                    foreach (CoreExportacionDatosPlantillasCeldas celda in celdas)
                    {
                        cCoreExportacionDatosPlantillasCeldas.DeleteCeldaCascada(celda.CoreExportacionDatosPlantillasCeldasID, FilaID, celda.CoreExportacionDatosPlantillaColumnaID);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.Error(ex);
            }

            return result;
        }

        public bool ResetCelda(long celdaID)
        {
            bool result = false;

            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(Context);

            try
            {
                CoreExportacionDatosPlantillasCeldas celda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdaID);
                
                cCoreExportacionDatosPlantillasCeldas.DeleteCeldaCascada(celda.CoreExportacionDatosPlantillasCeldasID, celda.CoreExportacionDatosPlantillaFilaID, celda.CoreExportacionDatosPlantillaColumnaID);

                cCoreExportacionDatosPlantillasCeldas.AddItem(new CoreExportacionDatosPlantillasCeldas() { 
                    CoreExportacionDatosPlantillaFilaID = celda.CoreExportacionDatosPlantillaFilaID,
                    CoreExportacionDatosPlantillaColumnaID = celda.CoreExportacionDatosPlantillaColumnaID
                });
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
                log.Error(ex);
            }

            return result;
        }

        public bool ResetByFila(long FilaID)
        {
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(Context);

            List<CoreExportacionDatosPlantillasCeldas> celdas;
            bool result = true;

            try
            {
                celdas = (from c in Context.CoreExportacionDatosPlantillasCeldas
                          where c.CoreExportacionDatosPlantillaFilaID == FilaID
                          select c).OrderByDescending(c => c.CoreExportacionDatosPlantillasCeldasID).ToList();

                if (celdas != null)
                {
                    celdas = celdas.FindAll(x => x.CeldaPadreID == null);
                    foreach (CoreExportacionDatosPlantillasCeldas celda in celdas)
                    {
                        cCoreExportacionDatosPlantillasCeldas.DeleteCeldaCascada(celda.CoreExportacionDatosPlantillasCeldasID, FilaID, celda.CoreExportacionDatosPlantillaColumnaID);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.Error(ex);
            }

            return result;
        }

        public bool DeleteCeldaCascada(long celdaID, long filaID, long columnaID)
        {
            bool result = true;

            try
            {
                //Obtengo todas las celdas de la columna y fila concreta.
                List<CoreExportacionDatosPlantillasCeldas> listaElementosFilaColumna;
                listaElementosFilaColumna = (from celda in Context.CoreExportacionDatosPlantillasCeldas
                                             where 
                                                celda.CoreExportacionDatosPlantillaFilaID==filaID && 
                                                celda.CoreExportacionDatosPlantillaColumnaID==columnaID
                                             select celda).ToList();

                //Recorro la lista para eliminar los ultimos elementos hasta llegar a la celdaID correspondiente.
                DeleteItems(listaElementosFilaColumna, celdaID);

            }
            catch(Exception ex)
            {
                result = false;
                log.Error(ex);
            }

            return result;
        }

        private void DeleteItems(List<CoreExportacionDatosPlantillasCeldas> lista, long celdaID)
        {
            try
            {
                CoreExportacionDatosPlantillasReglasCeldasController cCoreExportacionDatosPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
                cCoreExportacionDatosPlantillasReglasCeldas.SetDataContext(this.Context);

                List<long> IDs = new List<long>();
                Dictionary<long, bool> result = new Dictionary<long, bool>();

                long celdaIDPadre = celdaID;
                bool padreNull = false;
                while (!padreNull)
                {
                    List<CoreExportacionDatosPlantillasCeldas> hijas = lista.FindAll(x => x.CeldaPadreID != null && x.CeldaPadreID == celdaIDPadre);
                    IDs.Add(celdaIDPadre);

                    if (hijas.Count > 1)
                    {
                        DeleteItems(hijas, celdaIDPadre);
                    }
                    else if (hijas.Count > 0)
                    {
                        celdaIDPadre = hijas[0].CoreExportacionDatosPlantillasCeldasID;
                    }
                    else
                    {
                        padreNull = true;
                    }
                }
                IDs.Reverse();

                foreach(long id in IDs)
                {
                    #region Eliminar reglas de celda
                    List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglas = cCoreExportacionDatosPlantillasReglasCeldas.GetByCeldaID(id);

                    if (reglas != null)
                    {
                        reglas.ForEach(regla => {
                            cCoreExportacionDatosPlantillasReglasCeldas.DeleteItem(regla.CoreExportacionDatosPlantillasReglaCeldaID);
                        });
                    }

                    #endregion

                    result.Add(id, DeleteItem(id));
                }

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public string getTipoDato(CoreExportacionDatosPlantillasCeldas celda)
        {
            string sTipoDato = Comun.TIPODATO_CODIGO_TEXTO;

            try
            {
                if (celda.CoreAtributosConfiguracionID.HasValue)
                {
                    sTipoDato = (from tipdat in Context.TiposDatos
                                 join c in Context.CoreAtributosConfiguraciones on tipdat.TipoDatoID equals c.TipoDatoID
                                 where c.CoreAtributoConfiguracionID == celda.CoreAtributosConfiguracionID.Value
                                 select tipdat.Codigo).First();
                }
                else if (celda.ColumnasModeloDatoID.HasValue)
                {
                    sTipoDato = (from tipdat in Context.TiposDatos
                                 join c in Context.ColumnasModeloDatos on tipdat.TipoDatoID equals c.TipoDatoID
                                 where c.ColumnaModeloDatosID == celda.ColumnasModeloDatoID.Value
                                 select tipdat.Codigo).First();
                }
                else if (celda.CampoVinculado != null)
                {
                    Export.CampoVinculado campoVinculado = (Export.CampoVinculado)JSON.Deserialize(celda.CampoVinculado, typeof(Export.CampoVinculado));

                    sTipoDato = campoVinculado.TipoDato;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return sTipoDato;
        }


        public Vw_CoreExportacionDatosPlantillasCeldas GetItemVw(long celdaID)
        {
            Vw_CoreExportacionDatosPlantillasCeldas celda;

            try
            {
                celda = (from c in Context.Vw_CoreExportacionDatosPlantillasCeldas 
                         where c.CoreExportacionDatosPlantillasCeldasID == celdaID 
                         select c).First();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                celda = null;
            }
            return celda;
        }

    }
}