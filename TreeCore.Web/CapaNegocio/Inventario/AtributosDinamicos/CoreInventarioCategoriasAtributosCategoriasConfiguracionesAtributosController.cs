using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController : GeneralBaseController<CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos, TreeCoreContext>
    {
        public CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController()
            : base()
        { }

        public CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos GetVinculacion(long lCateAtrConf, long lAtrID) {
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos where c.CoreAtributoConfiguracionID == lAtrID && c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCateAtrConf select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

		public List<CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos> GetVinculaciones(long lAtrID) {
            List<CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos where c.CoreAtributoConfiguracionID == lAtrID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public bool AtributoUsado(long AtrID)
        {
            bool bUsada = false;
            string query1 = "select c.InventarioElementoID from InventarioElementos c cross apply openjson(c.JsonAtributosDinamicos) with (AtributoID int '$.\""+AtrID+"\".\"AtributoID\"') as jsonValues where jsonValues.AtributoID = " + AtrID;
            string query2 = "select c.CoreInventarioPlantillaAtributoCategoriaID from CoreInventarioPlantillasAtributosCategorias c cross apply openjson(c.JsonAtributosDinamicos) with (AtributoID int '$.\"" + AtrID + "\".\"AtributoID\"') as jsonValues where jsonValues.AtributoID = " + AtrID;
            try
            {
                if (EjecutarQuery(query1).Rows.Count > 0 || EjecutarQuery(query2).Rows.Count > 0)
                {
                    bUsada = true;
                }
            }
            catch (Exception ex)
            {
                bUsada = true;
                log.Error(ex.Message);
            }
            return bUsada;
        }

        public bool AtributoUsadoCategoria(long AtrID, long InvCatID)
        {
            bool bUsada = false;
            string query1 = "select c.InventarioElementoID from InventarioElementos c cross apply openjson(c.JsonAtributosDinamicos) with (AtributoID int '$.\"" + AtrID + "\".\"AtributoID\"') as jsonValues where jsonValues.AtributoID = " + AtrID + " AND c.InventarioCategoriaID = "+ InvCatID;
            try
            {
                if (EjecutarQuery(query1).Rows.Count > 0)
                {
                    bUsada = true;
                }
            }
            catch (Exception ex)
            {
                bUsada = true;
                log.Error(ex.Message);
            }
            return bUsada;
        }

        public Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos getConfInventarioCategoriasAtributosConfiguracion(string inventarioCategoria, string inventarioAtributoCategoria, string Nombre)
        {
            Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos cReturn;
            try
            {
                if (inventarioCategoria.Equals(""))
                {
                    cReturn = (from c in Context.Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos
                               where c.InventarioCategoria == null && c.InventarioAtributoCategoria == inventarioAtributoCategoria
                                && c.Nombre == Nombre select c).First(); ;
                }
                else
                {
                    cReturn = (from c in Context.Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos
                               where c.InventarioCategoria == inventarioCategoria && c.InventarioAtributoCategoria == inventarioAtributoCategoria
                                && c.Nombre == Nombre
                               select c).First(); ;
                }
            }
            catch (Exception ex)
            {
                cReturn = null;
                log.Error(ex.Message);
            }
            return cReturn;
        }
        
    }
}
