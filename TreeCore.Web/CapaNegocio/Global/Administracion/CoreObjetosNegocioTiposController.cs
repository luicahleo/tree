using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreObjetosNegocioTiposController : GeneralBaseController<CoreObjetosNegocioTipos, TreeCoreContext>
    {
        public CoreObjetosNegocioTiposController()
            : base()
        { }

        public CoreObjetosNegocioTipos getItemByCodigo (string sCodigo)
        {
            CoreObjetosNegocioTipos oNegocio;

            try
            {
                oNegocio = (from c in Context.CoreObjetosNegocioTipos where c.Codigo == sCodigo select c).First();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                oNegocio = null;
            }

            return oNegocio;
        }
        public List<CoreObjetosNegocioTipos> getItemByTablaModeloDato (long TablaModeloDatoID)
        {
            List<CoreObjetosNegocioTipos> oNegocio;

            try
            {
                oNegocio = (from c in Context.CoreObjetosNegocioTipos where c.TablaModeloDatoID == TablaModeloDatoID select c).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                oNegocio = null;
            }

            return oNegocio;
        }

        public DataTable getEstadosByID (long? lObjetoNegocioTipoID)
        {
            DataTable listaDatos = new DataTable();

            try
            {
                long? lColumnaID = (from c in Context.CoreObjetosNegocioTipos where c.CoreObjetoNegocioTipoID == lObjetoNegocioTipoID select c.EstadoColumnaModeloDatosID).First();

                if (lColumnaID != null)
                {
                    Vw_ColumnasModeloDatos oColumna = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c).First();

                    if (oColumna != null)
                    {
                        string query = "select ";

                        if (oColumna.Indice != null)
                        {
                            query += "* from " + oColumna.NombreTabla;

                            listaDatos = EjecutarQuery(query);
                        }
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

        public DataTable getEstadoByID(long? lObjetoNegocioTipoID, long? lObjetoEstadoID)
        {
            DataTable listaDatos = new DataTable();

            try
            {
                long? lColumnaID = (from c in Context.CoreObjetosNegocioTipos where c.CoreObjetoNegocioTipoID == lObjetoNegocioTipoID select c.EstadoColumnaModeloDatosID).First();

                if (lColumnaID != null)
                {
                    Vw_ColumnasModeloDatos oColumna = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c).First();

                    if (oColumna != null)
                    {
                        string query = "select ";

                        if (oColumna.Indice != null)
                        {
                            query += oColumna.NombreColumna + " from " + oColumna.NombreTabla + " where " + oColumna.Indice + " = " + lObjetoEstadoID;

                            listaDatos = EjecutarQuery(query);
                        }
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

        public DataTable getIDByEstadoNegocio(long? lObjetoNegocioTipoID, string sEstadoGlobal)
        {
            DataTable listaDatos = new DataTable();

            try
            {
                long? lColumnaID = (from c in Context.CoreObjetosNegocioTipos where c.CoreObjetoNegocioTipoID == lObjetoNegocioTipoID select c.EstadoColumnaModeloDatosID).First();

                if (lColumnaID != null)
                {
                    Vw_ColumnasModeloDatos oColumna = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c).First();

                    if (oColumna != null)
                    {
                        string query = "select ";

                        if (oColumna.Indice != null)
                        {
                            query += oColumna.Indice + " from " + oColumna.NombreTabla + " where " + oColumna.NombreColumna + " = '" + sEstadoGlobal + "'";

                            listaDatos = EjecutarQuery(query);
                        }
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

    }
}