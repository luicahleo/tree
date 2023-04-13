using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreInventarioElementosPlantillasAtributosCategoriasAtributosController : GeneralBaseController<CoreInventarioElementosPlantillasAtributosCategoriasAtributos, TreeCoreContext>
    {
        public CoreInventarioElementosPlantillasAtributosCategoriasAtributosController()
            : base()
        { }

        public List<CoreInventarioElementosPlantillasAtributosCategoriasAtributos> GetPlantillasAplicadas(long lElementoID)
        {
            List<CoreInventarioElementosPlantillasAtributosCategoriasAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioElementosPlantillasAtributosCategoriasAtributos where c.InventarioElementoID == lElementoID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
        public List<CoreInventarioPlantillasAtributosCategorias> GetPlantillasAplicadasElemento(long lElementoID)
        {
            List<CoreInventarioPlantillasAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategorias join
                              rel in Context.CoreInventarioElementosPlantillasAtributosCategoriasAtributos on c.CoreInventarioPlantillaAtributoCategoriaID equals rel.CoreInventarioPlantillaAtributoCategoriaID
                              where rel.InventarioElementoID == lElementoID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<CoreInventarioPlantillasAtributosCategoriasAtributos> GetAtributosPlantillasAplicadas(long lElementoID)
        {
            List<CoreInventarioPlantillasAtributosCategoriasAtributos> listaDatos;
            try
            {
                listaDatos = (from elePla in Context.CoreInventarioElementosPlantillasAtributosCategoriasAtributos
                              join PlaAtr in Context.CoreInventarioPlantillasAtributosCategoriasAtributos on elePla.CoreInventarioPlantillaAtributoCategoriaID equals PlaAtr.CoreInventarioPlantillaAtributoCategoriaID
                              where elePla.InventarioElementoID == lElementoID
                              select PlaAtr).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<CoreInventarioPlantillasAtributosCategoriasAtributos> GetAtributosPlantillasAplicadas(List<long> listaPlantillas)
        {
            List<CoreInventarioPlantillasAtributosCategoriasAtributos> listaDatos;
            try
            {
                listaDatos = (from PlaAtr in Context.CoreInventarioPlantillasAtributosCategoriasAtributos
                              where listaPlantillas.Contains(PlaAtr.CoreInventarioPlantillaAtributoCategoriaID)
                              select PlaAtr).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioElementosPlantillasJson> AplicarPlantillas(List<object> ListaPlantillas, long CategoriaID, out bool Correc, List<long> listasPlantillas = null)
        {
            List<InventarioElementosPlantillasJson> listaPlantillas;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInvCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            cInvCatConf.SetDataContext(this.Context);
            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oInvCatConf;
            InventarioElementosPlantillasJson oDato;
            List<long> listaIDs;
            bool bCorrecto = true;
            try
            {
                listaPlantillas = new List<InventarioElementosPlantillasJson>();
                if (listasPlantillas == null)
                {
                    listaIDs = GetListaConfiguracionesIDPlantillasObligatorias(CategoriaID);
                }
                else
                {
                    listaIDs = new List<long>(listasPlantillas);
                }
                if (bCorrecto)
                {
                    foreach (var oPla in ListaPlantillas)
                    {
                        var PlantillaID = oPla.GetType().GetProperty("PlantillaID");
                        var InvCatConfID = oPla.GetType().GetProperty("InvCatConfID");
                        var NombrePlantilla = oPla.GetType().GetProperty("NombrePlantilla");
                        oInvCatConf = cInvCatConf.GetItem(long.Parse(InvCatConfID.GetValue(oPla).ToString()));
                        oDato = new InventarioElementosPlantillasJson
                        {
                            InvCatConfID = long.Parse(InvCatConfID.GetValue(oPla).ToString()),
                            PlantillaID = long.Parse(PlantillaID.GetValue(oPla).ToString()),
                            NombrePlantilla = NombrePlantilla.GetValue(oPla).ToString(),
                            NombreCategoria = oInvCatConf.InventarioAtributosCategorias.InventarioAtributoCategoria
                        };
                        listaPlantillas.Add(oDato);
                        listaIDs.Remove(long.Parse(InvCatConfID.GetValue(oPla).ToString()));
                    }
                    if (listaIDs.Count > 0)
                    {
                        bCorrecto = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bCorrecto = false;
                listaPlantillas = null;
            }
            Correc = bCorrecto;
            return listaPlantillas;
        }

        public List<long> GetListaConfiguracionesIDPlantillasObligatorias(long lCatID)
        {
            List<long> listaIDs;
            try
            {
                listaIDs = (from catVin in Context.CoreInventarioCategoriasAtributosCategorias
                            join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                            join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                            join catAtr in Context.InventarioAtributosCategorias on catConf.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                            join atr in Context.CoreAtributosConfiguraciones on atrConf.CoreAtributoConfiguracionID equals atr.CoreAtributoConfiguracionID
                            join vwAtrProp in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades on atr.CoreAtributoConfiguracionID equals vwAtrProp.CoreAtributoConfiguracionID
                            where vwAtrProp.CodigoTipoDatoPropiedad == "AllowBlank" && vwAtrProp.Valor == "False" && catAtr.EsPlantilla && catVin.InventarioCategoriaID == lCatID
                            select catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }
            return listaIDs;
        }


    }
}