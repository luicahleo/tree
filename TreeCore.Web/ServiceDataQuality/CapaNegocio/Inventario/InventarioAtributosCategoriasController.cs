using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class InventarioAtributosCategoriasController : GeneralBaseController<InventarioAtributosCategorias, TreeCoreContext>, IBasica<InventarioAtributosCategorias>
    {
        public InventarioAtributosCategoriasController()
            : base()
        {

        }


        public bool RegistroVinculado(long lInventarioAtributoCategoriaID)
        {
            bool bExiste = true;

            return bExiste;
        }

        public bool RegistroDuplicado(string sInventarioAtributoCategoria, long lClienteID)
        {
            bool bExiste = false;
            List<InventarioAtributosCategorias> listaDatos = new List<InventarioAtributosCategorias>();

            listaDatos = (from c in Context.InventarioAtributosCategorias where (c.InventarioAtributoCategoria == sInventarioAtributoCategoria && c.ClienteID == lClienteID) select c).ToList<InventarioAtributosCategorias>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool NombreDuplicadoAtributos(string sNombre, long lInventarioAtributoCategoriaID) {
            bool Duplicado = false;
            List<string> listaNombre;
            try
            {
                listaNombre = (from atr in Context.CoreAtributosConfiguraciones
                               join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atr.CoreAtributoConfiguracionID equals atrConf.CoreAtributoConfiguracionID
                               join conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                               where conf.InventarioAtributoCategoriaID == lInventarioAtributoCategoriaID
                               select atr.Codigo).ToList();
                listaNombre.AddRange((from atrP in Context.CoreAtributosConfiguraciones
                               join atrConfP in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atrP.CoreAtributoConfiguracionID equals atrConfP.CoreAtributoConfiguracionID
                               join confP in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrConfP.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals confP.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                               join vinP in Context.CoreInventarioCategoriasAtributosCategorias on confP.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals vinP.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                               where (from conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                                            join vin in Context.CoreInventarioCategoriasAtributosCategorias on conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                            where conf.InventarioAtributoCategoriaID == lInventarioAtributoCategoriaID
                                            select vin.InventarioCategoriaID).ToList().Contains(vinP.InventarioCategoriaID)
                               select atrP.Codigo).ToList());

                Duplicado = listaNombre.Contains(sNombre);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Duplicado = true;
            }
            return Duplicado;
        }

        public bool RegistroDefecto(long lInventarioAtributoCategoriaID)
        {
            InventarioAtributosCategorias oDato = new InventarioAtributosCategorias();
            InventarioAtributosCategoriasController cController = new InventarioAtributosCategoriasController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && InventarioAtributoCategoriaID == " + lInventarioAtributoCategoriaID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        public List<InventarioAtributosCategorias> getActivos(long clienteID)
        {
            List<InventarioAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributosCategorias where c.Activo && (c.ClienteID == clienteID || c.ClienteID == null) select c).ToList();
            }
            catch (Exception)
            {
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioAtributosCategorias> getCategorias(long clienteID, bool Activo, bool esPlantilla)
        {
            List<InventarioAtributosCategorias> listaDatos;
            try
            {
                if (Activo)
                {
                    listaDatos = (from c in Context.InventarioAtributosCategorias where c.Activo && c.ClienteID == clienteID && c.EsPlantilla == esPlantilla select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioAtributosCategorias where c.ClienteID == clienteID && c.EsPlantilla == esPlantilla select c).ToList();
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioAtributosCategorias> getCategorias(long clienteID, bool Activo)
        {
            List<InventarioAtributosCategorias> listaDatos;
            try
            {
                if (Activo)
                {
                    listaDatos = (from c in Context.InventarioAtributosCategorias where c.Activo && c.ClienteID == clienteID select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioAtributosCategorias where c.ClienteID == clienteID select c).ToList();
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioAtributosCategorias> getCategoriasFiltradas(long clienteID, bool Activo, bool Seccion, bool Subcategoria, bool SubcategoriaTemplate)
        {
            List<InventarioAtributosCategorias> listaDatos = new List<InventarioAtributosCategorias>();
            try
            {
                if (Seccion)
                {
                    if (Activo)
                    {
                        listaDatos.AddRange((from c in Context.InventarioAtributosCategorias where !c.EsSubcategoria && c.Activo && c.ClienteID == clienteID select c).ToList());
                    }
                    else
                    {
                        listaDatos.AddRange((from c in Context.InventarioAtributosCategorias where !c.EsSubcategoria && c.ClienteID == clienteID select c).ToList());
                    }
                }
                if (Subcategoria)
                {
                    if (Activo)
                    {
                        listaDatos.AddRange((from c in Context.InventarioAtributosCategorias where c.EsSubcategoria && !c.EsPlantilla && c.Activo && c.ClienteID == clienteID select c).ToList());
                    }
                    else
                    {
                        listaDatos.AddRange((from c in Context.InventarioAtributosCategorias where c.EsSubcategoria && !c.EsPlantilla && c.ClienteID == clienteID select c).ToList());
                    }
                }
                if (SubcategoriaTemplate)
                {
                    if (Activo)
                    {
                        listaDatos.AddRange((from c in Context.InventarioAtributosCategorias where c.EsSubcategoria && c.EsPlantilla && c.Activo && c.ClienteID == clienteID select c).ToList());
                    }
                    else
                    {
                        listaDatos.AddRange((from c in Context.InventarioAtributosCategorias where c.EsSubcategoria && c.EsPlantilla && c.ClienteID == clienteID select c).ToList());
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioAtributosCategorias> getUnselectInventarioAtributosCategorias(long invCatID, long clienteID)
        {
            try
            {
                List<long> lisCat = (from c in Context.InventarioAtributosCategorias join  conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on c.InventarioAtributoCategoriaID 
                                     equals conf.InventarioAtributoCategoriaID 
                                     join vin in Context.CoreInventarioCategoriasAtributosCategorias on conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID 
                                     equals vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                     where vin.InventarioCategoriaID == invCatID
                                     select c.InventarioAtributoCategoriaID).ToList();
                List<InventarioAtributosCategorias> listaDatos = (from c in Context.InventarioAtributosCategorias where !(lisCat.Contains(c.InventarioAtributoCategoriaID)) && c.ClienteID == clienteID && c.Activo select c).ToList();
                return listaDatos;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public List<InventarioAtributosCategorias> getSelectInventarioAtributosCategorias(long invCatID, long clienteID)
        {
            try
            {
                InventarioCategorias oInventarioCategorias = (from w in Context.InventarioCategorias where w.InventarioCategoriaID == invCatID select w).First();
                List<long?> lisCat = (from e in Context.InventarioAtributos where e.InventarioCategoriaID == invCatID select e.InventarioAtributoCategoriaID).ToList();
                List<InventarioAtributosCategorias> listaDatos = (from c in Context.InventarioAtributosCategorias where lisCat.Contains(c.InventarioAtributoCategoriaID) && c.ClienteID == clienteID select c).ToList();
                return listaDatos;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public List<InventarioAtributosCategorias> GetActivos()
        {
            List<InventarioAtributosCategorias> clientes;
            try
            {
                clientes = (from c in Context.InventarioAtributosCategorias where c.Activo orderby c.InventarioAtributoCategoria select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                clientes = null;
            }
            return clientes;
        }

        public void ActualizarOrdenCategorias(long catID, long catAtrID, bool Orden)
        {
            InventarioAtributosCategorias oAtr;
            List<InventarioAtributos> listaEjecutor;
            List<InventarioAtributos> listaVictima;
            try
            {
                oAtr = (from c in Context.InventarioAtributosCategorias where c.InventarioAtributoCategoriaID == catAtrID select c).First();
                listaEjecutor = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == catID && c.InventarioAtributoCategoriaID == oAtr.InventarioAtributoCategoriaID select c).ToList();
                if (Orden)
                {
                    listaVictima = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == catID && c.OrdenCategoria == (listaEjecutor.First().OrdenCategoria - 1) select c).ToList();
                    if (listaVictima.Count > 0)
                    {
                        listaVictima.ForEach(x => x.OrdenCategoria++);
                        Context.SubmitChanges();
                    }
                    if (--listaEjecutor.First().OrdenCategoria >= 0)
                    {
                        listaEjecutor.ForEach(x => x.OrdenCategoria--);
                        Context.SubmitChanges();
                    }
                }
                else
                {
                    listaVictima = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == catID && c.OrdenCategoria == (listaEjecutor.First().OrdenCategoria + 1) select c).ToList();
                    if (listaVictima.Count > 0)
                    {
                        listaEjecutor.ForEach(x => x.OrdenCategoria++);
                        listaVictima.ForEach(x => x.OrdenCategoria--);
                        Context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public void ActualizarOrdenCategorias(long catID, long catAtrID, long NewOrden)
        {
            InventarioAtributosCategorias oAtr;
            List<InventarioAtributos> listaEjecutor;
            try
            {
                oAtr = (from c in Context.InventarioAtributosCategorias where c.InventarioAtributoCategoriaID == catAtrID select c).First();
                listaEjecutor = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == catID && c.InventarioAtributoCategoriaID == oAtr.InventarioAtributoCategoriaID select c).ToList();
                if (listaEjecutor != null && listaEjecutor.Count > 0)
                {
                    listaEjecutor.ForEach(x => x.OrdenCategoria = NewOrden);
                    Context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public void ActualizarOrdenAtributo(long atrID, long NewOrden)
        {
            InventarioAtributos oAtrEjecutor;
            InventarioAtributos oAtrVictima;
            InventarioAtributosController cAtributos = new InventarioAtributosController();
            try
            {
                oAtrEjecutor = (from c in Context.InventarioAtributos where c.InventarioAtributoID == atrID select c).First();
                if (oAtrEjecutor != null)
                {
                    oAtrEjecutor.Orden = (int)NewOrden;
                    Context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public void ActualizarOrdenAtributo(long atrID, bool Orden)
        {
            InventarioAtributos oAtrEjecutor;
            InventarioAtributos oAtrVictima;
            InventarioAtributosController cAtributos = new InventarioAtributosController();
            try
            {
                oAtrEjecutor = (from c in Context.InventarioAtributos where c.InventarioAtributoID == atrID select c).First();
                if (Orden)
                {
                    oAtrVictima = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == oAtrEjecutor.InventarioCategoriaID && c.Orden == (oAtrEjecutor.Orden - 1) select c).First();
                    if (oAtrVictima != null)
                    {
                        oAtrEjecutor.Orden--;
                        oAtrVictima.Orden++;
                        Context.SubmitChanges();
                    }
                }
                else
                {
                    oAtrVictima = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == oAtrEjecutor.InventarioCategoriaID && c.Orden == (oAtrEjecutor.Orden + 1) select c).First();
                    if (oAtrVictima != null)
                    {
                        oAtrEjecutor.Orden++;
                        oAtrVictima.Orden--;
                        Context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public long GetOrdenCategoria(long catID, long catAtributoID)
        {
            long orden = 0;
            try
            {
                orden = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == catID && c.InventarioAtributoCategoriaID == catAtributoID select c.OrdenCategoria).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                orden = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == catID orderby c.OrdenCategoria descending select c.OrdenCategoria).First() + 1;
            }
            return orden;
        }

        public bool ExistsCategory(string categoriaAtributo, long clienteID)
        {
            bool existeCategoria = false;

            try
            {
                existeCategoria = (from c in Context.InventarioAtributosCategorias
                                   where c.Activo &&
                                        c.InventarioAtributoCategoria == categoriaAtributo &&
                                        c.ClienteID == clienteID
                                   select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existeCategoria = false;
            }

            return existeCategoria;
        }
    }
}