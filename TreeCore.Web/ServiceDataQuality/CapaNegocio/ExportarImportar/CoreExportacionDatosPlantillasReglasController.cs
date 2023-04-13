using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using TreeCore.Data;
using Ext.Net;
using TreeCore;

namespace CapaNegocio
{
    public class CoreExportacionDatosPlantillasReglasController : GeneralBaseController<CoreExportacionDatosPlantillasReglas, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasReglasController()
            : base()
        { }

        public List<object> GetListReglas(long celdaID)
        {
            List<object> lista;

            try
            {
                CoreExportacionDatosPlantillasCeldas celda = (from cel in Context.CoreExportacionDatosPlantillasCeldas 
                                                              where cel.CoreExportacionDatosPlantillasCeldasID == celdaID 
                                                              select cel).First();

                if (celda.ColumnasModeloDatoID != null)
                {
                    //Obtención de reglas para columnas de BBDD
                    lista = (from regla in Context.CoreExportacionDatosPlantillasReglas
                             join tyOper in Context.TiposDatosOperadores on regla.TipoDatoOperadorID equals tyOper.TipoDatoOperadorID
                             join colModDat in Context.ColumnasModeloDatos on tyOper.TipoDatoID equals colModDat.TipoDatoID
                             join cel in Context.CoreExportacionDatosPlantillasCeldas on colModDat.ColumnaModeloDatosID equals cel.ColumnasModeloDatoID
                             join typDato in Context.TiposDatos on tyOper.TipoDatoID equals typDato.TipoDatoID
                             where cel.CoreExportacionDatosPlantillasCeldasID == celdaID
                             select new
                             {
                                 CoreExportacionDatosPlantillasReglaID = regla.CoreExportacionDatosPlantillasReglaID,
                                 TipoDatoOperadorID = tyOper.TipoDatoOperadorID,
                                 Nombre = tyOper.Nombre,
                                 ClaveRecurso = tyOper.ClaveRecurso,
                                 Operador = tyOper.Operador,
                                 RequiereValor = tyOper.RequiereValor,
                                 TipoDato = typDato.Codigo
                             }).ToList<object>();
                }
                else if (celda.CoreAtributosConfiguracionID != null)
                {
                    //Obtención de reglas para attributos dinámicos
                    lista = (from regla in Context.CoreExportacionDatosPlantillasReglas
                             join tyOper in Context.TiposDatosOperadores on regla.TipoDatoOperadorID equals tyOper.TipoDatoOperadorID
                             join attrConfig in Context.CoreAtributosConfiguraciones on tyOper.TipoDatoID equals attrConfig.TipoDatoID
                             join cel in Context.CoreExportacionDatosPlantillasCeldas on attrConfig.CoreAtributoConfiguracionID equals cel.CoreAtributosConfiguracionID
                             join typDato in Context.TiposDatos on tyOper.TipoDatoID equals typDato.TipoDatoID
                             where cel.CoreExportacionDatosPlantillasCeldasID == celdaID
                             select new
                             {
                                 CoreExportacionDatosPlantillasReglaID = regla.CoreExportacionDatosPlantillasReglaID,
                                 TipoDatoOperadorID = tyOper.TipoDatoOperadorID,
                                 Nombre = tyOper.Nombre,
                                 ClaveRecurso = tyOper.ClaveRecurso,
                                 Operador = tyOper.Operador,
                                 RequiereValor = tyOper.RequiereValor,
                                 TipoDato = typDato.Codigo
                             }).ToList<object>();
                }
                else if (!string.IsNullOrEmpty(celda.CampoVinculado))
                {
                    Export.CampoVinculado campoVinculado = (Export.CampoVinculado)JSON.Deserialize(celda.CampoVinculado, typeof(Export.CampoVinculado));

                    lista = (from regla in Context.CoreExportacionDatosPlantillasReglas
                     join tyOper in Context.TiposDatosOperadores on regla.TipoDatoOperadorID equals tyOper.TipoDatoOperadorID
                     join typDato in Context.TiposDatos on tyOper.TipoDatoID equals typDato.TipoDatoID
                     where typDato.Codigo == campoVinculado.TipoDato
                     select new
                     {
                         CoreExportacionDatosPlantillasReglaID = regla.CoreExportacionDatosPlantillasReglaID,
                         TipoDatoOperadorID = tyOper.TipoDatoOperadorID,
                         Nombre = tyOper.Nombre,
                         ClaveRecurso = tyOper.ClaveRecurso,
                         Operador = tyOper.Operador,
                         RequiereValor = tyOper.RequiereValor,
                         TipoDato = typDato.Codigo
                     }).ToList<object>();
                }
                else
                {
                    lista = new List<object>();
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

    }
}