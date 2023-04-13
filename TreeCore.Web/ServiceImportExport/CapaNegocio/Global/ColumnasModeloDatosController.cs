using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ColumnasModeloDatosController : GeneralBaseController<ColumnasModeloDatos, TreeCoreContext>
    {
        public ColumnasModeloDatosController()
            : base()
        { }

        public List<ColumnasModeloDatos> GetColumnasTablas(long lTablaID)
        {
            List<ColumnasModeloDatos> listaDatos;
            try
            {
                listaDatos = (from c in Context.ColumnasModeloDatos where c.Activo && c.TablaModeloDatosID == lTablaID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<ColumnasModeloDatos> GetColumnasTablasNoFk(long lTablaID)
        {
            List<ColumnasModeloDatos> listaDatos;
            try
            {
                listaDatos = (from c in Context.ColumnasModeloDatos 
                              where 
                              c.Activo && 
                              c.TablaModeloDatosID == lTablaID &&
                              !c.ForeignKeyID.HasValue
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Vw_ColumnasModeloDatos> GetColumnasTablasVista(long lTablaID)
        {
            List<Vw_ColumnasModeloDatos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_ColumnasModeloDatos where c.ActivoColumna && c.TablaModeloDatosID == lTablaID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Vw_ColumnasModeloDatos> GetColumnasVistaTablas(string sTabla)
        {
            List<Vw_ColumnasModeloDatos> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ColumnasModeloDatos where c.ActivoColumna && c.NombreTabla == sTabla select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public long getColumnaNombreTabla(string sNombre)
        {
            long lColumnaID;

            try
            {
                lColumnaID = (from c in Context.ColumnasModeloDatos where c.NombreColumna == sNombre select c.ColumnaModeloDatosID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lColumnaID = 0;
            }

            return lColumnaID;
        }

        public long getTablaByColumna(long lColumnaID)
        {
            long lTablaID;

            try
            {
                lTablaID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.TablaModeloDatosID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lTablaID = 0;
            }

            return lTablaID;
        }

        public TablasModeloDatos GetTablaByColumna(long lColumnaID)
        {
            TablasModeloDatos tablaModeloDatos;
            long? lDataSourceTablaID;

            try
            {
                lDataSourceTablaID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.ForeignKeyID).First();

                if (lDataSourceTablaID.HasValue)
                {
                    lColumnaID = lDataSourceTablaID.Value;
                }


                tablaModeloDatos = (from c in Context.ColumnasModeloDatos
                                    join t in Context.TablasModeloDatos on c.TablaModeloDatosID equals t.TablaModeloDatosID
                                    where c.ColumnaModeloDatosID == lColumnaID
                                    select t).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tablaModeloDatos = null;
            }

            return tablaModeloDatos;
        }

        public string getDataSourceTablaColumna(long lColumnaID)
        {
            string sDataSourceTabla;
            long? lDataSourceTablaID;

            try
            {
                lDataSourceTablaID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.ForeignKeyID).First();

                if (lDataSourceTablaID == null)
                {
                    sDataSourceTabla = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.NombreTabla).First();
                }
                else
                {
                    sDataSourceTabla = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lDataSourceTablaID select c.NombreTabla).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDataSourceTablaID = 0;
                sDataSourceTabla = "";
            }

            return sDataSourceTabla;
        }

        public string getControllerColumna(long lColumnaID)
        {
            string sController;
            long? lDataSourceTablaID;

            try
            {
                lDataSourceTablaID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.ForeignKeyID).First();

                if (lDataSourceTablaID == null)
                {
                    sController = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.Controlador).First();
                }
                else
                {
                    sController = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lDataSourceTablaID select c.Controlador).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDataSourceTablaID = 0;
                sController = "";
            }

            return sController;
        }

        public string getColumnaByTabla(long lTablaID)
        {
            string sTraduccion;

            try
            {
                sTraduccion = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lTablaID select c.ClaveRecurso).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sTraduccion = "";
            }

            return sTraduccion;
        }

        public string getNombreColumnaByTabla(long lTablaID)
        {
            string sColumna;

            try
            {
                sColumna = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lTablaID select c.ForeignKeyNombre).First();

                if (sColumna == null)
                {
                    sColumna = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lTablaID select c.NombreColumna).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sColumna = "";
            }

            return sColumna;
        }

        public string getIndiceColumna(long lColumnaID)
        {
            long? lDataSourceTablaID;
            string sIndice;

            try
            {
                lDataSourceTablaID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.ForeignKeyID).First();

                if (lDataSourceTablaID == null)
                {
                    sIndice = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.Indice).First();
                }
                else
                {
                    sIndice = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lDataSourceTablaID select c.Indice).First();
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sIndice = "";
            }

            return sIndice;
        }

        public long? getForeignKeyID(long lColumnaID)
        {
            long? lID;

            try
            {
                lID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.ForeignKeyID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lID = 0;
            }

            return lID;
        }

        public long? getTipoDatoByColumna(long lColumnaID)
        {
            long? lTipoDatoID;

            try
            {
                lTipoDatoID = (from c in Context.ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.TipoDatoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lTipoDatoID = 0;
            }

            return lTipoDatoID;
        }

        public string getTipoDatoNombreByColumna(long lColumnaID)
        {
            string sTipoDato;

            try
            {
                sTipoDato = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.Codigo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sTipoDato = null;
            }

            return sTipoDato;
        }

        public string getDisplay(long ColumnaModeloDatosID)
        {
            string sValor;

            try
            {
                sValor = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == ColumnaModeloDatosID select c.ForeignKeyNombre).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sValor = null;
            }

            return sValor;
        }

        public string getDisplay(Vw_DQKpisGroupsRules oDato)
        {
            string sValor;

            try
            {
                sValor = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == oDato.ColumnaModeloDatosID select c.ForeignKeyNombre).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sValor = null;
            }

            return sValor;
        }

        public string getClaveRecursoTabla(long lTablaModeloDatosID)
        {
            string sClave;

            try
            {
                sClave = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lTablaModeloDatosID select c.ClaveRecursoTabla).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sClave = null;
            }

            return sClave;
        }

        public string getDisplayFiltro(Vw_DQKpisFiltros oDato)
        {
            string sValor;

            try
            {
                sValor = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == oDato.ColumnaModeloDatoID select c.ForeignKeyNombre).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sValor = null;
            }

            return sValor;
        }

        public ColumnasModeloDatos GetColumnaReferencia(long ColumnaModeloDatosID, long TablaModeloDatosID)
        {
            ColumnasModeloDatos result;
            
            try
            {
                result = (from tabReg in Context.TablasModeloDatos
                          join colGrReg in Context.ColumnasModeloDatos on tabReg.TablaModeloDatosID equals colGrReg.TablaModeloDatosID
                          join colResult in Context.ColumnasModeloDatos on tabReg.TablaModeloDatosID equals colResult.TablaModeloDatosID
                          join colFK in Context.ColumnasModeloDatos on colResult.ForeignKeyID equals colFK.ColumnaModeloDatosID
                          join tabFK in Context.TablasModeloDatos on colFK.TablaModeloDatosID equals tabFK.TablaModeloDatosID
                          where colGrReg.ColumnaModeloDatosID == ColumnaModeloDatosID && tabFK.TablaModeloDatosID == TablaModeloDatosID
                          select colResult).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }

        public ColumnasModeloDatos getColumnaByTablaAndColumn(long tablaID, long columnID) 
        {
            ColumnasModeloDatos colResult;
            ColumnasModeloDatos col;

            try
            {
                col = (from c in Context.ColumnasModeloDatos 
                       where c.ColumnaModeloDatosID == columnID 
                       select c).FirstOrDefault();

                if (col != null)
                {
                    colResult = (from c in Context.ColumnasModeloDatos
                                 where
                                      c.TablaModeloDatosID == tablaID &&
                                      c.ForeignKeyID == ((col.ForeignKeyID == null)? col.ColumnaModeloDatosID: col.ForeignKeyID)
                                 select c).FirstOrDefault();
                }
                else
                {
                    colResult = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                colResult = null;
            }

            return colResult;
        }

        public List<ColumnasModeloDatos> GetColumnasTipoLista(long tablaModeloDatosID)
        {
            List<ColumnasModeloDatos> lista;

            try
            {
                lista = (from c in Context.ColumnasModeloDatos
                         where c.TablaModeloDatosID == tablaModeloDatosID &&
                            c.ForeignKeyID.HasValue
                         select c).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                lista = new List<ColumnasModeloDatos>();
            }

            return lista;
        }

        public List<ColumnasModeloDatos> GetColumnasNoFkByTabla(long tablaID)
        {
            List<ColumnasModeloDatos> columnas;

            try
            {
                columnas = (from c in Context.ColumnasModeloDatos
                            where 
                                c.TablaModeloDatosID == tablaID && 
                                !c.ForeignKeyID.HasValue
                            select c).GroupBy(c => c.ColumnaModeloDatosID).Select(c => c.First()).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                columnas = new List<ColumnasModeloDatos>();
            }

            return columnas;
        }

        public List<TablasModeloDatos> GetTablasByIds(List<long> Ruta)
        {
            List<TablasModeloDatos> tablas;

            try
            {
                tablas = (from tabla in Context.TablasModeloDatos 
                                                  where Ruta.Contains(tabla.TablaModeloDatosID) select tabla).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                tablas = null;
            }

            return tablas;
        }

        public void GetNombreColumnasRelacionadas(out string nombreColumnaFk, out string nombreColumnaPk, long idTablaFk, long idTablaPk)
        {
            nombreColumnaFk = "";
            nombreColumnaPk = "";
            try
            {
                var obj = (from c1 in Context.ColumnasModeloDatos 
                                join c2 in Context.ColumnasModeloDatos on c1.ForeignKeyID equals c2.ColumnaModeloDatosID
                                where c1.TablaModeloDatosID==idTablaFk && c2.TablaModeloDatosID==idTablaPk
                                select new { 
                                    NameColum1 = c1.NombreColumna, 
                                    NameColum2 = c2.NombreColumna }
                                ).First();

                if (obj != null)
                {
                    nombreColumnaFk = obj.NameColum1;
                    nombreColumnaPk = obj.NameColum2;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }

    }
}