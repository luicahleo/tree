using CapaNegocio;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Data;

namespace CapaNegocio
{

    public class FiltroInventarioElementosController
    {
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<JsonItemsFiltroInventario> Deserializacion(string datos)
        {
            List<JsonItemsFiltroInventario> listaAtributos;
            try
            {
                listaAtributos = JsonConvert.DeserializeObject<List<JsonItemsFiltroInventario>>(datos);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaAtributos = null;
            }
            return listaAtributos;
        }

        public string Serializacion(List<JsonItemsFiltroInventario> listaDatos)
        {
            string oDato;
            try
            {
                oDato = JsonConvert.SerializeObject(listaDatos);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public string Serializacion(JsonItemsFiltroInventario listaDatos)
        {
            string oDato;
            try
            {
                oDato = JsonConvert.SerializeObject(listaDatos);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public List<FiltroInventarioElementos> DeserializacionFiltros(string datos)
        {
            List<FiltroInventarioElementos> listaAtributos;
            try
            {
                listaAtributos = JsonConvert.DeserializeObject<List<FiltroInventarioElementos>>(datos);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaAtributos = null;
            }
            return listaAtributos;
        }

        public string SerializacionFiltros(List<FiltroInventarioElementos> listaDatos)
        {
            string oDato;
            try
            {
                oDato = JsonConvert.SerializeObject(listaDatos);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
    }

    public class FiltroInventarioElementosComparer : IEqualityComparer<JsonItemsFiltroInventario>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(JsonItemsFiltroInventario x, JsonItemsFiltroInventario y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.listaFiltros == y.listaFiltros;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(JsonItemsFiltroInventario product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashAtributoID = product.listaFiltros.GetHashCode();

            //Calculate the hash code for the product.
            return hashAtributoID /*^ hashValorCode*/;
        }
    }

}

namespace TreeCore.Data
{
    public class JsonItemsFiltroInventario { 
        public long InventarioCategoriaID { get; set; }
        public string NombreFiltro { get; set; }
        public List<FiltroInventarioElementos> listaFiltros { get; set; }
    }

    public class FiltroInventarioElementos
    {
        public string Name { get; set; }
        public string Campo { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string TypeData { get; set; }
        public string Operador { get; set; }
        public string TipoCampo { get; set; }
    }

    public class CampoFiltroInventario
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Campo { get; set; }
        public string TipoCampo { get; set; }
        public string TypeData { get; set; }
        public string QueryValores { get; set; }
        public string NameCategory { get; set; }
    }

    public static class CamposFiltroInventario
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string TipoCampoElemento = "Elemento";
        public const string TipoCampoAtributo = "Atributo";
        public const string TipoCampoPlantilla = "Plantilla";
        public const string TipoCampoPlantillaAtributo = "AtributoPlantilla";
        public static List<CampoFiltroInventario> GetCamposFiltrosInventario(long lClienteID, long InventarioCategoriaID, long lUsuarioID)
        {
            string oValores;

            CoreInventarioCategoriasAtributosCategoriasController cCatVin = new CoreInventarioCategoriasAtributosCategoriasController();
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();

            CampoFiltroInventario campoFiltroInventario;
            List<CampoFiltroInventario> listaCampos = new List<CampoFiltroInventario>();
            try
            {
                campoFiltroInventario = new CampoFiltroInventario
                {
                    Name = "strCodigo",
                    Campo = "NumeroInventario",
                    TipoCampo = CamposFiltroInventario.TipoCampoElemento,
                    TypeData = Comun.TIPODATO_CODIGO_TEXTO
                };

                campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                listaCampos.Add(campoFiltroInventario);

                campoFiltroInventario = new CampoFiltroInventario
                {
                    Name = "strNombre",
                    Campo = "Nombre",
                    TipoCampo = CamposFiltroInventario.TipoCampoElemento,
                    TypeData = Comun.TIPODATO_CODIGO_TEXTO
                };

                campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                listaCampos.Add(campoFiltroInventario);

                campoFiltroInventario = new CampoFiltroInventario
                {
                    Name = "strEmplazamiento",
                    Campo = "Codigo",
                    TipoCampo = CamposFiltroInventario.TipoCampoElemento,
                    TypeData = Comun.TIPODATO_CODIGO_TEXTO
                };

                campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                listaCampos.Add(campoFiltroInventario);

                campoFiltroInventario = new CampoFiltroInventario
                {
                    Name = "strEstado",
                    Campo = "InventarioElementoAtributoEstadoID",
                    TipoCampo = CamposFiltroInventario.TipoCampoElemento,
                    TypeData = Comun.TIPODATO_CODIGO_LISTA,
                    QueryValores = " select InventarioElementoAtributoEstadoID, Nombre from InventarioElementosAtributosEstados where ClienteID = " + lClienteID
                };

                campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                listaCampos.Add(campoFiltroInventario);

                campoFiltroInventario = new CampoFiltroInventario
                {
                    Name = "strOperador",
                    Campo = "OperadorID",
                    TipoCampo = CamposFiltroInventario.TipoCampoElemento,
                    TypeData = Comun.TIPODATO_CODIGO_LISTA,
                    QueryValores = " select OperadorID, Nombre from Entidades where OperadorID is not null and ClienteID = " + lClienteID
                };

                campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                listaCampos.Add(campoFiltroInventario);

                foreach (var oPla in cCatConf.GetPlantillasCategoriaID(InventarioCategoriaID))
                {
                    campoFiltroInventario = new CampoFiltroInventario
                    {
                        Name = oPla.InventarioAtributosCategorias.InventarioAtributoCategoria,
                        Campo = oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID.ToString(),
                        TipoCampo = CamposFiltroInventario.TipoCampoPlantilla,
                        TypeData = Comun.TIPODATO_CODIGO_LISTA,
                        QueryValores = " select CoreInventarioPlantillaAtributoCategoriaID, Nombre from CoreInventarioPlantillasAtributosCategorias where CoreInventarioCategoriaAtributoCategoriaConfiguracionID = " + oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                    };
                    campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                    listaCampos.Add(campoFiltroInventario);
                }

                foreach (var oAtrConf in cCatVin.GetAtributosVisiblesSinPlantillasByInventarioCategoriaID(InventarioCategoriaID, lUsuarioID))
                {
                    if (oAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA || oAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        oValores = cAtributosConf.GetQueryItemsFiltros(oAtrConf.CoreAtributoConfiguracionID);
                        campoFiltroInventario = new CampoFiltroInventario
                        {
                            Name = oAtrConf.Codigo,
                            Campo = oAtrConf.CoreAtributoConfiguracionID.ToString(),
                            TipoCampo = CamposFiltroInventario.TipoCampoAtributo,
                            TypeData = oAtrConf.TiposDatos.Codigo,
                            QueryValores = oValores
                        };
                    }
                    else
                    {
                        campoFiltroInventario = new CampoFiltroInventario
                        {
                            Name = oAtrConf.Codigo,
                            Campo = oAtrConf.CoreAtributoConfiguracionID.ToString(),
                            TipoCampo = CamposFiltroInventario.TipoCampoAtributo,
                            TypeData = oAtrConf.TiposDatos.Codigo
                        };
                    }
                    campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                    listaCampos.Add(campoFiltroInventario);
                }

                foreach (var oAtrConf in cCatVin.GetAtributosVisiblesPlantillasByInventarioCategoriaID(InventarioCategoriaID, lUsuarioID))
                {
                    if (oAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA || oAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        oValores = cAtributosConf.GetQueryItemsFiltros(oAtrConf.CoreAtributoConfiguracionID);
                        campoFiltroInventario = new CampoFiltroInventario
                        {
                            Name = oAtrConf.Codigo,
                            Campo = oAtrConf.CoreAtributoConfiguracionID.ToString(),
                            TipoCampo = CamposFiltroInventario.TipoCampoAtributo,
                            TypeData = oAtrConf.TiposDatos.Codigo,
                            QueryValores = oValores
                        };
                    }
                    else
                    {
                        campoFiltroInventario = new CampoFiltroInventario
                        {
                            Name = oAtrConf.Codigo,
                            Campo = oAtrConf.CoreAtributoConfiguracionID.ToString(),
                            TipoCampo = CamposFiltroInventario.TipoCampoAtributo,
                            TypeData = oAtrConf.TiposDatos.Codigo
                        };
                    }
                    campoFiltroInventario.ID = campoFiltroInventario.TipoCampo + campoFiltroInventario.Campo;
                    listaCampos.Add(campoFiltroInventario);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaCampos = null;
            }
            return listaCampos;
        }

        public static List<CampoFiltroInventario> GetCamposFiltrosExportacionDatosPlantillas(long lClienteID, long plantillaID, bool conAtributosDinamicos, List<long> idsCategories, bool isCategoryInventory)
        {
            string oValores;

            #region Controllers
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            ColumnasModeloDatosController columnasModeloDatos = new ColumnasModeloDatosController();
            CoreInventarioCategoriasAtributosCategoriasController cCoreInventarioCategoriasAtributosCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
            #endregion

            CampoFiltroInventario campoFiltroInventario;
            List<AtributosConfigWidthCategoria> atributosConfiguraciones;
            List<CampoFiltroInventario> listaCampos = new List<CampoFiltroInventario>();
            try
            {
                #region Campos de la tabla
                CoreExportacionDatosPlantillas plantilla = cCoreExportacionDatosPlantillas.GetItem(plantillaID);
                List<Vw_ColumnasModeloDatos> columnas = columnasModeloDatos.GetColumnasTablasVista(plantilla.TablaModeloDatosID);

                foreach (Vw_ColumnasModeloDatos col in columnas)
                {
                    campoFiltroInventario = new CampoFiltroInventario
                    {
                        Name = col.ClaveRecursoColumna,
                        Campo = col.NombreColumna,
                        TipoCampo = CamposFiltroInventario.TipoCampoElemento,
                        TypeData = col.Codigo
                    };

                    
                    if (col.Codigo == Comun.TIPODATO_CODIGO_LISTA || col.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        ColumnasModeloDatos colFK = columnasModeloDatos.getColumnaByTablaAndColumn(col.TablaModeloDatosID, col.ColumnaModeloDatosID);
                        string nombreTabla = columnasModeloDatos.getDataSourceTablaColumna(colFK.ColumnaModeloDatosID);
                        string nombreColumna = columnasModeloDatos.getNombreColumnaByTabla(colFK.ColumnaModeloDatosID);
                        string indice = columnasModeloDatos.getIndiceColumna(colFK.ColumnaModeloDatosID);

                        campoFiltroInventario.QueryValores = " SELECT " + nombreColumna + ", " + indice + " FROM " + nombreTabla;
                    }

                    listaCampos.Add(campoFiltroInventario);
                }
                #endregion

                #region Campos dinámicos
                if (conAtributosDinamicos)
                {
                    if (!isCategoryInventory)
                    {
                        atributosConfiguraciones = new List<AtributosConfigWidthCategoria>();
                        cAtributosConf.GetActivos(lClienteID).ForEach(c => atributosConfiguraciones.Add(new AtributosConfigWidthCategoria(c))); ;
                    }
                    else
                    {
                        atributosConfiguraciones = cCoreInventarioCategoriasAtributosCategorias.GetAtributosByInventarioCategoriasIDs(idsCategories);
                    }
                    

                    foreach (AtributosConfigWidthCategoria oAtrConfWithCat in atributosConfiguraciones)
                    {
                        CoreAtributosConfiguraciones oAtrConf = oAtrConfWithCat.coreAtributoConfiguracion;

                        if (oAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA || oAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                        {
                            oValores = cAtributosConf.GetQueryItemsFiltros(oAtrConf.CoreAtributoConfiguracionID);
                            campoFiltroInventario = new CampoFiltroInventario
                            {
                                Name = oAtrConf.Nombre,
                                Campo = oAtrConf.CoreAtributoConfiguracionID.ToString(),
                                TipoCampo = CamposFiltroInventario.TipoCampoAtributo,
                                TypeData = oAtrConf.TiposDatos.Codigo,
                                QueryValores = oValores,
                                NameCategory = oAtrConfWithCat.categoria
                            };
                        }
                        else
                        {
                            campoFiltroInventario = new CampoFiltroInventario
                            {
                                Name = oAtrConf.Codigo,
                                Campo = oAtrConf.CoreAtributoConfiguracionID.ToString(),
                                TipoCampo = CamposFiltroInventario.TipoCampoAtributo,
                                TypeData = oAtrConf.TiposDatos.Codigo,
                                NameCategory = oAtrConfWithCat.categoria
                            };
                        }
                        listaCampos.Add(campoFiltroInventario);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaCampos = null;
            }
            return listaCampos;
        }
    }
}