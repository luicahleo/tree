using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class TablasModeloDatosController : GeneralBaseController<TablasModeloDatos, TreeCoreContext>
    {
        public TablasModeloDatosController()
            : base()
        { }

        public List<Vw_TablasModelosDatosAsignacion> GetTablasPagina(string pagina) {
            List<Vw_TablasModelosDatosAsignacion> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_TablasModelosDatosAsignacion where ((bool)c.Activo) && c.NombrePagina == pagina select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<TablasModeloDatos> GetTablas() {
            List<TablasModeloDatos> listaDatos;
            try
            {
                listaDatos = (from c in Context.TablasModeloDatos where ((bool)c.Activo) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<TablasModeloDatos> GetTablasModulos()
        {
            List<TablasModeloDatos> listaDatos;
            try
            {
                listaDatos = (from c in Context.TablasModeloDatos where ((bool)c.Activo) && c.ModuloID != null select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public string getTablaByColumna (long lColumnaID)
        {
            string sNombreTabla;

            try
            {
                sNombreTabla = (from c in Context.Vw_ColumnasModeloDatos where c.ColumnaModeloDatosID == lColumnaID select c.ClaveRecursoTabla).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombreTabla = "";
            }

            return sNombreTabla;
        }

        public string getNombreByID (long lTablaID)
        {
            string sNombre;

            try
            {
                long? lID = (from c in Context.DQTablasPaginas where c.DQTablaPaginaID == lTablaID select c.TablaModeloDatosID).First();
                sNombre = (from c in Context.TablasModeloDatos where c.TablaModeloDatosID == lID select c.NombreTabla).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombre = "";
            }

            return sNombre;
        }

        public long getIDByNombre (string lTablaNombre)
        {
            long ID;

            try
            {
                ID = (from c in Context.TablasModeloDatos where c.NombreTabla == lTablaNombre select c.TablaModeloDatosID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ID = 0;
            }

            return ID;
        }

        public string getNombreByClave(string sClave)
        {
            string sNombre;

            try
            {
                sNombre = (from c in Context.TablasModeloDatos where c.ClaveRecurso == sClave select c.NombreTabla).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombre = "";
            }

            return sNombre;
        }

        public List<TablasModeloDatos> getTablasByColumnaID (long? lForeignKeyID, long lTablaID)
        {
            List<TablasModeloDatos> listaDatos = null;

            try
            {
                listaDatos = (from col in Context.ColumnasModeloDatos
                              join tab in Context.TablasModeloDatos on col.TablaModeloDatosID equals tab.TablaModeloDatosID
                              where col.ColumnaModeloDatosID == lForeignKeyID || col.ForeignKeyID == lForeignKeyID && col.TablaModeloDatosID != lTablaID
                              select tab).Distinct<TablasModeloDatos>().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public string getClaveByID (long lTablaID)
        {
            string sClave;

            try
            {
                sClave = (from c in Context.TablasModeloDatos where c.TablaModeloDatosID == lTablaID select c.ClaveRecurso).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sClave = null;
            }

            return sClave;
        }

        public List<TablasModeloDatos> GetTablasAsociadas(long tablaModeloDatoID)
        {
            List<TablasModeloDatos> tablas;

            try
            {
                tablas = (from c in Context.ColumnasModeloDatos
                              join col in Context.ColumnasModeloDatos on c.ForeignKeyID equals col.ColumnaModeloDatosID
                              join tb in Context.TablasModeloDatos on col.TablaModeloDatosID equals tb.TablaModeloDatosID
                          where c.TablaModeloDatosID == tablaModeloDatoID
                          select tb).GroupBy(c => c.TablaModeloDatosID).Select(c => c.First()).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                tablas = new List<TablasModeloDatos>();
            }
            return tablas;
        }

    }
}