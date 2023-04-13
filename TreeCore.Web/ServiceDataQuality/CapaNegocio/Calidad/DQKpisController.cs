using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DQKpisController : GeneralBaseController<DQKpis, TreeCoreContext>
    {
        public DQKpisController() : base()
        {
        }

        public List<Vw_DQKpis> getKPIsActivos(long ClienteID)
        {
            List<Vw_DQKpis> activos;
            try
            {
                activos = (from c in Context.Vw_DQKpis
                           where
                               c.Activo
                           select c).ToList();
            }
            catch (Exception ex)
            {
                activos = null;
                log.Error(ex.Message);
            }
            return activos;
        }

        public List<DQKpis> getActivosService(long? ClienteID)
        {
            List<DQKpis> activos;
            try
            {
                if (ClienteID != null)
                {
                    activos = (from c in Context.DQKpis
                               where
                                   c.Activo && c.ClienteID == ClienteID
                               select c).ToList();
                }
                else
                {
                    activos = (from c in Context.DQKpis
                               where
                                   c.Activo
                               select c).ToList();
                }
            }
            catch (Exception ex)
            {
                activos = null;
                log.Error(ex.Message);
            }
            return activos;
        }

        public List<Vw_DQKpis> getActivosServiceVw(long? ClienteID)
        {
            List<Vw_DQKpis> activos;
            try
            {
                List<long> ids = getActivosService(ClienteID).Select(c => c.DQKpiID).ToList();
                activos = (from c in Context.Vw_DQKpis
                           where ids.Contains(c.DQKpiID)
                           select c).ToList();
            }
            catch (Exception ex)
            {
                activos = null;
                log.Error(ex.Message);
            }
            return activos;
        }

        public int getKpisByCategoriaID(long lCategoriaID)
        {
            int iValor = 0;
            List<long> listaIDs;
            DQKpisMonitoring oDato;

            try
            {
                listaIDs = (from c in Context.Vw_DQKpis where c.DQCategoriaID == lCategoriaID && c.Activo select c.DQKpiID).ToList();

                foreach (long i in listaIDs)
                {
                    oDato = (from c in Context.DQKpisMonitoring where c.DQKpiID == i select c).First();

                    if (oDato != null)
                    {
                        iValor++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return iValor;
        }


        public DQKpis getKPIByID(long lID)
        {
            DQKpis oDato;

            try
            {
                oDato = (from c in Context.DQKpis where c.DQKpiID == lID select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }

            return oDato;
        }

        public DQKpis getKPIByName(string sNombre)
        {
            DQKpis oDato;

            try
            {
                oDato = (from c in Context.DQKpis where c.DQKpi == sNombre select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }

            return oDato;
        }

        public long? getTablaIDByKpi(long lKPI)
        {
            long? lTablaID;

            try
            {
                lTablaID = (from c in Context.DQKpis where c.DQKpiID == lKPI select c.DQTablaPaginaID).First();
            }
            catch (Exception ex)
            {
                lTablaID = 0;
                log.Error(ex.Message);
            }

            return lTablaID;
        }


        public ResultDQKpi EjecutarKPI(DQKpis kpi, List<long> DQGroupIDs, long UsuarioID)
        {
            //Variables
            ResultDQKpi result;
            ILog Log = LogManager.GetLogger("");
            string select = "SELECT ";
            string WHERE = " WHERE ";
            string where = "";
            string whereFiltros = "";
            string whereGrupos = "";
            string queryString = "";
            string queryFiltros = "";
            string queryGrupos = "";
            int total = -1;
            int numElementos = -1;
            int version = 1;
            List<long> listaIds = new List<long>();

            //Controllers
            DQTablasPaginasController cDQTablasPaginas = new DQTablasPaginasController();
            DQKpisGroupsController cDQKpisGroups = new DQKpisGroupsController();
            DQKpisMonitoringController cDQKpiMonitoring = new DQKpisMonitoringController();
            DQGroupsMonitoringController cDQGroupsMonitoring = new DQGroupsMonitoringController();
            DQKpisGroupsRulesController cDQKpisGroupsRules = new DQKpisGroupsRulesController();

            try
            {

                if (kpi.DQTablaPaginaID.HasValue)
                {
                    Vw_DQTablasPaginas dqTablaPincipal = cDQTablasPaginas.GetVwItem(kpi.DQTablaPaginaID.Value);

                    #region Obtención de la ultima ejecución del KPI
                    DQKpisMonitoring ultimaEjecucionKPI = cDQKpiMonitoring.GetUltimoKPI(kpi.DQKpiID);
                    if (ultimaEjecucionKPI != null)
                    {
                        ultimaEjecucionKPI.Ultima = false;
                        version = ultimaEjecucionKPI.Version + 1;
                    }
                    #endregion

                    if (dqTablaPincipal != null)
                    {
                        #region SELECT
                        select += dqTablaPincipal.Indice + " FROM ";
                        select += dqTablaPincipal.NombreTabla;
                        Log.Info("SELECT: " + select);
                        #endregion

                        #region GROUP BY
                        string groupBy = " GROUP BY " + dqTablaPincipal.Indice;
                        #endregion

                        #region Filtro
                        whereFiltros = buildFiltro(kpi, dqTablaPincipal);

                        Log.Info("WHERE_FILTROS: " + whereFiltros);
                        queryFiltros = select;
                        if (!string.IsNullOrEmpty(whereFiltros))
                        {
                            queryFiltros += WHERE + whereFiltros + groupBy;
                        }

                        #endregion

                        //SELECT + FILTRO
                        int dt = ejecutarConsulta(queryFiltros, out listaIds);

                        #region Registro monitoring KPI
                        total = dt;
                        DQKpisMonitoring dQKpiMonitoring = new DQKpisMonitoring()
                        {
                            DQKpiID = kpi.DQKpiID,
                            Activa = true,
                            Ultima = true,
                            FechaEjecucion = DateTime.Now,
                            UsuarioID = UsuarioID,
                            Total = total,
                            NumeroElementos = numElementos,
                            Version = version,
                            Pagina = null,
                            Filtro = queryFiltros
                        };

                        dQKpiMonitoring = cDQKpiMonitoring.AddItem(dQKpiMonitoring);
                        #endregion

                        if (dt != -1)
                        {
                            #region GRUPOS

                            DQGroupIDs.ForEach(DQGroup =>
                            {
                                Log.Info("TYPE_CONDITION_ID: " + DQGroup);

                                List<Vw_DQKpisGroups> gruposKPI = cDQKpisGroups.GetGruposActivosByKPI(kpi.DQKpiID, DQGroup);

                                if (gruposKPI != null && gruposKPI.Count > 0)
                                {
                                    whereGrupos = "";
                                    long? DQKpiGroupID = null;
                                    gruposKPI.ForEach(grupoKPI =>
                                    {
                                        DQKpiGroupID = grupoKPI.DQKpiGroupID;
                                        Log.Info("DQKpiGroupID: " + grupoKPI.DQKpiGroupID);
                                        #region WHERE Por grupos
                                        //int versionGrupo = 1;
                                        DQGroupsMonitoring ultimaEjecucionGrupo = cDQGroupsMonitoring.GetUltimoByGrupo(grupoKPI.DQGroupID);
                                        if (ultimaEjecucionGrupo != null)
                                        {
                                            ultimaEjecucionGrupo.Ultima = false;
                                            cDQGroupsMonitoring.UpdateItem(ultimaEjecucionGrupo);
                                            //versionGrupo = int.Parse(ultimaEjecucionGrupo.Version) + 1;
                                        }

                                        List<Vw_DQKpisGroupsRules> reglas = cDQKpisGroupsRules.GetReglasByGrupo(grupoKPI.DQKpiGroupID);

                                        string whereGroupTemp = buildWhereByGroup(reglas, dqTablaPincipal, grupoKPI);
                                        Log.Info("WHERE_GROUP: " + whereGroupTemp);

                                        #endregion

                                        #region Contatenación de grupos
                                        if (string.IsNullOrEmpty(whereGrupos))
                                        {
                                            whereGrupos += " (" + whereGroupTemp + ") ";
                                        }
                                        else
                                        {
                                            whereGrupos += " " + (kpi.IsAnd ? Comun.SQL.INTERSECT : Comun.SQL.UNION) + " (" + whereGroupTemp + ") ";
                                        }
                                        #endregion
                                    });

                                    Log.Info("WHERE_GRUPOS: " + whereGrupos);

                                    queryGrupos = select;

                                    if (!string.IsNullOrEmpty(whereGrupos))
                                    {
                                        queryGrupos += WHERE + ((string.IsNullOrEmpty(whereFiltros)) ? "" : whereFiltros + " AND ") + " " + dqTablaPincipal.Indice + " IN (" + whereGrupos + ")";
                                    }
                                    dt = ejecutarConsulta(queryGrupos, out listaIds);

                                    #region Registro Monitoring por grupo
                                    int numElementosByGrupo = dt;
                                    DQGroupsMonitoring dQGroupsMonitoring = new DQGroupsMonitoring()
                                    {
                                        Activa = true,
                                        Ultima = true,
                                        Filtro = queryGrupos,
                                        FechaEjecucion = DateTime.Now,
                                        Total = total,
                                        NumeroElementos = numElementosByGrupo,
                                        UsuarioID = UsuarioID,
                                        Version = version.ToString(),
                                        Pagina = "",
                                        DQGroupID = DQGroup,
                                        DQKpiGroupID = DQKpiGroupID
                                    };
                                    cDQGroupsMonitoring.AddItem(dQGroupsMonitoring);
                                    #endregion

                                    if (dt != -1)
                                    {

                                    }
                                    else
                                    {
                                        Log.Error("Error to execute query: " + queryGrupos);
                                    }
                                }
                            });

                            #endregion

                            #region Contrucción del WHERE(Filtros + Grupos)
                            if (!string.IsNullOrEmpty(whereGrupos))
                            {
                                string whereTemp = (string.IsNullOrEmpty(whereFiltros) ? "" : (whereFiltros + " AND "));
                                where = whereTemp + dqTablaPincipal.Indice + " IN (" + whereGrupos + ")";
                            }
                            else
                            {
                                where = whereFiltros;
                            }
                            Log.Info("WHERE: " + where);
                            #endregion

                            #region EXECUTE QUERY
                            if (string.IsNullOrEmpty(where))
                            {
                                queryString = select;
                            }
                            else
                            {
                                queryString = select + WHERE + where;
                            }

                            queryString += groupBy;


                            Log.Info("QUERY: " + queryString);

                            dt = ejecutarConsulta(queryString, out listaIds);
                            if (dt != -1)
                            {
                                numElementos = dt;
                            }
                            #endregion

                            #region Monitoring KPI final
                            dQKpiMonitoring.Total = total;
                            dQKpiMonitoring.NumeroElementos = numElementos;
                            dQKpiMonitoring.Filtro = queryString;

                            cDQKpiMonitoring.UpdateItem(dQKpiMonitoring);
                            #endregion

                            result = new ResultDQKpi(true, queryString, numElementos, total, listaIds, dQKpiMonitoring);
                        }
                        else
                        {
                            result = new ResultDQKpi(false, "Malformed query Filters", numElementos, total, listaIds, null);
                        }
                    }
                    else
                    {
                        result = new ResultDQKpi(false, "Table not found", numElementos, total, listaIds, null);
                    }
                }
                else
                {
                    result = new ResultDQKpi(false, "KPI There are no table selected", numElementos, total, listaIds, null);
                }
            }
            catch (Exception ex)
            {
                result = new ResultDQKpi(false, ex.Message, numElementos, total, listaIds, null);
                log.Error(ex.Message);
            }

            return result;
        }

        private string buildWhereByGroup(List<Vw_DQKpisGroupsRules> reglas, Vw_DQTablasPaginas dqTablaPincipal, Vw_DQKpisGroups grupoKPI)
        {
            string tempWhereGrupos = "";

            try
            {
                if (reglas != null && reglas.Count > 0)
                {
                    TiposDatosOperadoresController cTiposDatosOperadores = new TiposDatosOperadoresController();
                    ColumnasModeloDatosController columnasModeloDatos = new ColumnasModeloDatosController();

                    reglas.ForEach(regla =>
                    {
                        TiposDatosOperadores tipoDatoOperador = cTiposDatosOperadores.GetItem(regla.TipoDatoOperadorID);

                        ColumnasModeloDatos colReferencia = columnasModeloDatos.GetColumnaReferencia(regla.ColumnaModeloDatosID, dqTablaPincipal.TablaModeloDatosID.Value);


                        string foreingKey = (colReferencia != null) ? colReferencia.NombreColumna : dqTablaPincipal.Indice;/*dqTablaPincipal.Indice*//*regla.Indice*/
                        string tabla = regla.NombreTabla;
                        string operador = regla.Operador;
                        string nombreCol = regla.NombreColumna;
                        string temp = "";
                        bool requiereValor = tipoDatoOperador.RequiereValor;
                        string valor = "";

                        if (requiereValor)
                        {
                            if (regla.IsDataTable)
                            {
                                valor = getValorSelectTable(long.Parse(regla.Valor), regla.ColumnaModeloDatosID);
                            }
                            else
                            {
                                valor = getValorByTipoDato(regla.Valor, regla.Codigo, regla.Operador, dqTablaPincipal.TablaModeloDatosID.Value);
                            }
                        }

                        if (!string.IsNullOrEmpty(tempWhereGrupos))
                        {
                            temp += " " + (grupoKPI.IsAnd ? Comun.SQL.INTERSECT : Comun.SQL.UNION) + " ";
                        }

                        temp += "(SELECT " + foreingKey + " FROM " + tabla + " WHERE " + nombreCol + " " + operador + " " + valor + ")";

                        tempWhereGrupos += temp;
                    });

                }
            }
            catch (Exception ex)
            {
                tempWhereGrupos = null;
                log.Error(ex.Message);
            }
            return tempWhereGrupos;
        }

        private string buildFiltro(DQKpis kpi, Vw_DQTablasPaginas dqTablaPincipal)
        {
            string whereFiltros = " ClienteID=" + kpi.ClienteID;

            DQKpisFiltrosController cDQKpisFiltros = new DQKpisFiltrosController();

            try
            {
                List<Vw_DQKpisFiltros> filtrosKPI = cDQKpisFiltros.GetFiltrosActivosPorKPI(kpi.DQKpiID);
                if (filtrosKPI != null && filtrosKPI.Count > 0)
                {
                    filtrosKPI.ForEach(filtroKPI =>
                    {
                        TiposDatosOperadoresController cTiposDatosOperadores = new TiposDatosOperadoresController();
                        TiposDatosOperadores tipoDatoOperador = cTiposDatosOperadores.GetItem(filtroKPI.TipoDatoOperadorID);
                        bool requiereValor = tipoDatoOperador.RequiereValor;

                        whereFiltros += " " + Comun.SQL.AND + " ";

                        string columna = filtroKPI.NombreColumna;
                        string operador = filtroKPI.Operador;
                        string valor = "";

                        if (requiereValor)
                        {
                            if (filtroKPI.IsDataTable)
                            {
                                valor = getValorSelectTable(long.Parse(filtroKPI.Valor), filtroKPI.ColumnaModeloDatoID);
                            }
                            else
                            {
                                valor = getValorByTipoDato(filtroKPI.Valor, filtroKPI.Codigo, filtroKPI.Operador, dqTablaPincipal.TablaModeloDatosID.Value);
                            }
                        }

                        whereFiltros += " " + columna + " " + operador + " " + valor;

                    });
                }
            }
            catch (Exception ex)
            {
                whereFiltros = null;
                log.Error(ex.Message);
            }

            return whereFiltros;
        }
        private string getValorByTipoDato(string valor, string tipoDato, string Operador, long colum)
        {
            string valorResult;

            switch (tipoDato)
            {
                case Comun.TIPODATO_CODIGO_BOOLEAN:
                case Comun.TIPODATO_CODIGO_NUMERICO:
                case Comun.TIPODATO_CODIGO_NUMERICO_ENTERO:
                case Comun.TIPODATO_CODIGO_NUMERICO_FLOTANTE:
                case Comun.TIPODATO_CODIGO_LISTA:
                    valorResult = valor;
                    break;
                case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:

                    if (false)
                    {//(select UsuarioID from Usuarios)
                        ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
                        TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
                        TablasModeloDatos tabla = cTablasModeloDatos.GetItem(long.Parse(valor));
                        ColumnasModeloDatos columnaReferencia = cColumnasModeloDatos.GetColumnaReferencia(long.Parse(valor), colum);

                        valorResult = "(SELECT " + columnaReferencia.NombreColumna + " FROM " + tabla.NombreTabla + ")";
                    }
                    else
                    {
                        valorResult = "(" + valor + ")";
                    }
                    break;
                case Comun.TIPODATO_CODIGO_TEXTO:
                case Comun.TIPODATO_CODIGO_TEXTAREA:
                case Comun.TIPODATO_CODIGO_FECHA:
                default:
                    valorResult = "\'" + valor + "\'";
                    break;
            }

            return valorResult;
        }

        private string getValorSelectTable(long idTabla, long columnaID)
        {
            string result = "";
            TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();

            TablasModeloDatos tabla = cTablasModeloDatos.GetItem(idTabla);
            ColumnasModeloDatos col = cColumnasModeloDatos.getColumnaByTablaAndColumn(idTabla, columnaID);

            if (tabla != null)
            {
                string qTabla = tabla.NombreTabla;
                string qCol = col.NombreColumna;

                result = "(SELECT " + qCol + " FROM " + qTabla + ")";
            }

            return result;
        }

        public int ejecutarConsulta(string queryString, out List<long> listaIds)
        {
            ILog Log = LogManager.GetLogger("");
            Log.Info("EXECUTED_QUERY: " + queryString);
            #region CADENA CONEXIÓN
#if SERVICESETTINGS
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string connectionString = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string connectionString = TreeCore.Properties.Settings.Default.Conexion;
#endif
            #endregion

            int result = 0;
            listaIds = new List<long>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long tmp = reader.GetInt64(0);

                            if (!listaIds.Contains(tmp))
                            {
                                listaIds.Add(tmp);
                                result++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
                log.Error(ex.Message);
            }

            return result;
        }

        public string GetRutaPagina(long DQKpiID)
        {
            string result;

            try
            {
                result = (from DQKpis in Context.DQKpis
                          join DQTablasP in Context.Vw_DQTablasPaginas on DQKpis.DQTablaPaginaID equals DQTablasP.DQTablaPaginaID
                          join TablasMD in Context.Vw_TablasModelosDatos on DQTablasP.TablaModeloDatosID equals TablasMD.TablaModeloDatosID
                          join menu in Context.Vw_Menus on TablasMD.ModuloID equals menu.ModuloID
                          where
                            DQKpis.DQKpiID == DQKpiID
                          select menu.RutaPagina).First();
            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message);
            }
            return result;
        }

        public string GetIndiceTabla(long DQKpiID)
        {
            string indice;
            try
            {
                indice = (from kpi in Context.DQKpis
                          join tabPag in Context.DQTablasPaginas on kpi.DQTablaPaginaID equals tabPag.DQTablaPaginaID
                          join tabDat in Context.TablasModeloDatos on tabPag.TablaModeloDatosID equals tabDat.TablaModeloDatosID
                          where kpi.DQKpiID == DQKpiID
                          select tabDat.Indice).FirstOrDefault();
            }
            catch (Exception ex)
            {
                indice = null;
                log.Error(ex.Message);
            }
            return indice;
        }

    }



    public class ResultDQKpi
    {
        public string mensaje { get; set; }
        public bool success { get; set; }
        public long numeroElementos { get; set; }
        public long total { get; set; }
        public List<long> ids { get; set; }
        public DQKpisMonitoring DQKpiMonitoring { get; set; }

        public ResultDQKpi(bool success, string mensaje, long numeroElementos, long total, List<long> ids, DQKpisMonitoring dQKpiMonitoring)
        {
            this.mensaje = mensaje;
            this.success = success;
            this.numeroElementos = numeroElementos;
            this.total = total;
            this.ids = ids;
            this.DQKpiMonitoring = dQKpiMonitoring;
        }
    }
}