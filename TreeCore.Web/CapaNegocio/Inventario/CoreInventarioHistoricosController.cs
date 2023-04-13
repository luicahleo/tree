using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using TreeCore.Data;
using Ext.Net;
using Tree.Linq.GenericExtensions;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using System.Globalization;
using Newtonsoft.Json;

namespace CapaNegocio
{
    public class CoreInventarioHistoricosController : GeneralBaseController<CoreInventarioHistoricos, TreeCoreContext>
    {
        public CoreInventarioHistoricosController()
            : base()
        { }

        public List<InventarioHistorico> GetHistoricoTotal(DateTime fechaIncio)
        {
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();

            List<InventarioHistorico> lista = new List<InventarioHistorico>();
            List<CoreInventarioHistoricos> coreHistoricos;

            try
            {
                List<InventarioCategorias> categorias = cInventarioCategorias.GetItems().ToList();
                coreHistoricos = (from c in Context.CoreInventarioHistoricos 
                                  join cat in Context.InventarioCategorias on c.InventarioCategoriaID equals cat.InventarioCategoriaID
                         where c.Fecha.Date >= fechaIncio.Date 
                         select c).ToList();

                coreHistoricos = coreHistoricos.OrderBy(h => h.Fecha).ToList();

                foreach (CoreInventarioHistoricos historico in coreHistoricos)
                {
                    InventarioCategorias cat = categorias.Find(c => historico.InventarioCategoriaID == c.InventarioCategoriaID);

                    lista.Add(new InventarioHistorico()
                    {
                        fecha = historico.Fecha,
                        InventarioCategoriaID = historico.InventarioCategoriaID,
                        InventarioCategoria = cat.InventarioCategoria,
                        CoreInventarioHistoricoID = historico.CoreInventarioHistoricoID,
                        informacion = historico.Informacion
                    });
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<InventarioHistorico> GetHistoricoByCategory(DateTime fechaIncio, long categoryID)
        {
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();

            List<InventarioHistorico> lista = new List<InventarioHistorico>(); 
            List<CoreInventarioHistoricos> coreHistoricos;


            try
            {
                List<InventarioCategorias> categorias = cInventarioCategorias.GetItems().ToList();

                coreHistoricos = (from c in Context.CoreInventarioHistoricos 
                         where 
                             c.Fecha.Date >= fechaIncio.Date && 
                             c.InventarioCategoriaID == categoryID 
                         select c).ToList();

                coreHistoricos = coreHistoricos.OrderBy(h => h.Fecha).ToList();


                foreach (CoreInventarioHistoricos historico in coreHistoricos)
                {
                    InventarioCategorias cat = categorias.Find(c => historico.InventarioCategoriaID == c.InventarioCategoriaID);

                    lista.Add(new InventarioHistorico()
                    {
                        fecha = historico.Fecha,
                        InventarioCategoriaID = historico.InventarioCategoriaID,
                        InventarioCategoria = cat.InventarioCategoria,
                        CoreInventarioHistoricoID = historico.CoreInventarioHistoricoID,
                        informacion = historico.Informacion
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public DateTime GetFechaUltimoHistorico()
        {
            DateTime fechaUltimoHistorico;

            try
            {
                fechaUltimoHistorico = (from c in Context.CoreInventarioHistoricos
                                        select c.Fecha.Date).OrderBy(c => c).First();
            }
            catch (Exception ex)
            {
                fechaUltimoHistorico = GetFechaCreacionPrimerElemento();
                log.Error(ex.Message);
            }
            return fechaUltimoHistorico;
        }

        public DateTime GetFechaCreacionPrimerElemento()
        {
            DateTime fechaCreacion;

            try
            {
                fechaCreacion = (from c in Context.InventarioElementos
                                        select c.FechaCreacion.Date).OrderBy(c => c).First();
            }
            catch (Exception ex)
            {
                fechaCreacion = DateTime.Now.Date.AddDays(-1);
                log.Error(ex.Message);
            }
            return fechaCreacion;
        }
        public List<InventarioHistorico> GetHistoricoByTopCategories(int top, List<long> categoriasValidas, DateTime fechaIncio)
        {
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
            List<InventarioHistorico> historicos = new List<InventarioHistorico>();
            List<CoreInventarioHistoricos> coreHistoricos;
            List<ItemInvInicio> categoriasTop;

            

            try
            {
                if (categoriasValidas.Count > 0)
                {
                    List<InventarioCategorias> cats = cInventarioCategorias.GetItemList();

                    categoriasTop = new List<ItemInvInicio>();
                    categoriasValidas.ForEach(c => {
                        categoriasTop.Add(new ItemInvInicio()
                        {
                            InventarioCategoriaID = c,
                            InventarioCategoria = cats.Find(a => a.InventarioCategoriaID==c).InventarioCategoria
                        });
                    });
                }
                else
                {
                    categoriasTop = cInventarioElementos.GetTopCategorias(top);
                }

                List<long> listIDCategories = new List<long>();
                categoriasTop.ForEach(x => listIDCategories.Add(x.InventarioCategoriaID));


                coreHistoricos = (from c in Context.CoreInventarioHistoricos
                              where
                                listIDCategories.Contains(c.InventarioCategoriaID) &&
                                c.Fecha.Date > fechaIncio.Date
                              select c).ToList();

                foreach (CoreInventarioHistoricos historico in coreHistoricos)
                {
                    string inventarioCategoria = "";
                    ItemInvInicio iii = categoriasTop.Find(c => c.InventarioCategoriaID == historico.InventarioCategoriaID);
                    if (iii != null)
                    {
                        inventarioCategoria = iii.InventarioCategoria;
                    }

                    historicos.Add(new InventarioHistorico()
                    {
                        fecha = historico.Fecha,
                        InventarioCategoriaID = historico.InventarioCategoriaID,
                        InventarioCategoria = inventarioCategoria,
                        CoreInventarioHistoricoID = historico.CoreInventarioHistoricoID,
                        informacion = historico.Informacion
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                historicos = null;
            }

            return historicos;
        }

        #region GenerarHistorico
        public void GenerarHistorico(DateTime fechaGeneracion)
        {
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
            CoreInventarioHistoricosController cCoreInventarioHistoricos = new CoreInventarioHistoricosController();

            try
            {
                List<InventarioCategorias> categorias = cInventarioCategorias.GetItems().ToList();
                List<CoreInventarioHistoricos> coreInventarioHistoricos = GetCoreInventarioHistoricosByFecha(fechaGeneracion);


                foreach (InventarioCategorias categoria in categorias)
                {
                    log.Info("Generate History of Category: " + categoria.InventarioCategoria);
                    InventarioHistoricoInformacion info = new InventarioHistoricoInformacion()
                    {
                        Creados = GetCreadosToday(fechaGeneracion, categoria.InventarioCategoriaID),
                        Modificados = GetModificadosToday(fechaGeneracion, categoria.InventarioCategoriaID),
                        TotalCategory = GetTotalCategoryToday(fechaGeneracion, categoria.InventarioCategoriaID),
                        Total = GetTotalToday(fechaGeneracion)
                    };

                    string infoJSON = JsonConvert.SerializeObject(info);

                    List<CoreInventarioHistoricos> coreInventarioHistoricosTemp = coreInventarioHistoricos.Where(c => c.InventarioCategoriaID == categoria.InventarioCategoriaID).ToList();

                    if (coreInventarioHistoricosTemp.Count > 0)
                    {
                        CoreInventarioHistoricos historico = cCoreInventarioHistoricos.GetItem(coreInventarioHistoricosTemp.First().CoreInventarioHistoricoID);

                        historico.Informacion = infoJSON;

                        cCoreInventarioHistoricos.UpdateItem(historico);
                    }
                    else
                    {
                        CoreInventarioHistoricos historico = new CoreInventarioHistoricos()
                        {
                            Fecha = fechaGeneracion,
                            InventarioCategoriaID = categoria.InventarioCategoriaID,
                            Informacion = infoJSON
                        };

                        cCoreInventarioHistoricos.AddItem(historico);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public long GetCreadosToday(DateTime today, long categoriaID)
        {
            long creados = 0;

            try
            {
                creados = (from c in Context.InventarioElementos
                           where
                               c.FechaCreacion.Date == today.Date &&
                               c.InventarioCategoriaID == categoriaID
                           select c).Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return creados;
        }

        public long GetModificadosToday(DateTime today, long categoriaID)
        {
            long modificados = 0;

            try
            {
                modificados = (from c in Context.InventarioElementos
                               where
                                   c.UltimaModificacionFecha.HasValue &&
                                   c.UltimaModificacionFecha.Value.Date == today.Date &&
                                   c.InventarioCategoriaID == categoriaID
                               select c).Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return modificados;
        }

        public long GetTotalCategoryToday(DateTime today, long categoriaID)
        {
            long total = 0;

            try
            {
                total = (from c in Context.InventarioElementos
                         where 
                            c.Activo &&
                            c.InventarioCategoriaID == categoriaID
                         select c).Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return total;
        }

        public long GetTotalToday(DateTime today)
        {
            long total = 0;

            try
            {
                total = (from c in Context.InventarioElementos
                         where c.Activo
                         select c).Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return total;
        }

        public List<CoreInventarioHistoricos> GetCoreInventarioHistoricosByFecha(DateTime today)
        {
            List<CoreInventarioHistoricos> historicos;

            try
            {
                historicos = (from c in Context.CoreInventarioHistoricos
                              where c.Fecha.Date == today.Date
                              select c).ToList();
            }
            catch (Exception ex)
            {
                historicos = null;
                log.Error(ex.Message);
            }
            return historicos;
        }
        #endregion

    }

    public class InventarioHistorico {

        public long CoreInventarioHistoricoID { get; set; }
        public DateTime fecha { get; set; }
        public long InventarioCategoriaID { get; set; }
        public string InventarioCategoria { get; set; }
        public string  informacion { get; set; }
    }
    
    public class InventarioHistoricoInformacion
    {
        public long Creados { get; set; }
        public long Modificados { get; set; }
        public long Total { get; set; }
        public long TotalCategory { get; set; }
    }
}