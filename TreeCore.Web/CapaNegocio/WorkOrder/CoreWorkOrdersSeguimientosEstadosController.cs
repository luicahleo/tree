using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreWorkOrdersSeguimientosEstadosController : GeneralBaseController<CoreWorkOrdersSeguimientosEstados, TreeCoreContext>
    {
        public CoreWorkOrdersSeguimientosEstadosController()
            : base()
        { }

        public List<CoreWorkOrdersSeguimientosEstados> GetEstadosWorkOrder(long lWorkO) {
            List<CoreWorkOrdersSeguimientosEstados> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkOrdersSeguimientosEstados where c.CoreWorkOrderID == lWorkO orderby c.Fecha descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public CoreWorkOrdersSeguimientosEstados GetEstadoActual(long lWorkO) {
            CoreWorkOrdersSeguimientosEstados oDato;
            try
            {
                oDato = GetEstadosWorkOrder(lWorkO).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public List<CoreWorkOrdersSeguimientosEstados> GetEstadosAnteriores(long lWorkO) {
            List<CoreWorkOrdersSeguimientosEstados> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkOrdersSeguimientosEstados where c.CoreWorkOrderID == lWorkO orderby c.Fecha descending select c).ToList();
                listaDatos.RemoveAt(0);
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