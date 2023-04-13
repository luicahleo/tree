using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreWorkOrderSeguimientosController : GeneralBaseController<CoreWorkOrderSeguimientos, TreeCoreContext>
    {
        public CoreWorkOrderSeguimientosController()
            : base()
        { }

        //public CoreWorkOrderSeguimientos GetSeguimientoActual(long lWorkOrder) {
        //    CoreWorkOrderSeguimientos oDato;
        //    try
        //    {
        //        oDato = (from c in Context.CoreWorkOrderSeguimientos
        //                 join workOrder in Context.CoreWorkOrders on c.CoreWorkOrderSeguimientoID equals workOrder.CoreWorkOrderSeguimientoID
        //                 where workOrder.CoreWorkOrderID == lWorkOrder select c).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        oDato = null;
        //    }
        //    return oDato;
        //}

        //public List<CoreWorkOrderSeguimientos> GetSeguimientosAnteriores(long lWorkOrder)
        //{
        //    List<CoreWorkOrderSeguimientos> listaDatos;
        //    CoreWorkOrderSeguimientos oDato;
        //    try
        //    {
        //        oDato = (from c in Context.CoreWorkOrderSeguimientos
        //                 join workOrder in Context.CoreWorkOrders on c.CoreWorkOrderSeguimientoID equals workOrder.CoreWorkOrderSeguimientoID
        //                 where workOrder.CoreWorkOrderID == lWorkOrder
        //                 select c).First();
        //        listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderID == lWorkOrder && !c.CoreWorkOrderSeguimientoPadreID.HasValue && c.CoreWorkOrderSeguimientoID != oDato.CoreWorkOrderSeguimientoID orderby c.Fecha select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }
        //    return listaDatos;
        //}

        public List<CoreWorkOrderSeguimientos> GetSubSeguimientos(long lSeguimientoID) {
            List<CoreWorkOrderSeguimientos> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderSeguimientoPadreID == lSeguimientoID orderby c.Fecha ascending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<CoreWorkOrderSeguimientos> GetSeguimientosFromEstado(long lEstadoID) {
            List<CoreWorkOrderSeguimientos> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderSeguimientoEstadoID == lEstadoID && !c.CoreWorkOrderSeguimientoPadreID.HasValue select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<CoreWorkOrderSeguimientos> GetSeguimientosFromEstadoFiltrado(long lEstadoID, string sFiltro) {
            List<CoreWorkOrderSeguimientos> listaDatos;
            try
            {
                
                switch (sFiltro)
                {
                    case "Todo":
                        listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderSeguimientoEstadoID == lEstadoID && !c.CoreWorkOrderSeguimientoPadreID.HasValue select c).ToList();
                        break;
                    case "Documentos":
                        listaDatos = (from c in Context.CoreWorkOrderSeguimientos join doc in Context.CoreWorkOrderSeguimientosDocumentos on c.CoreWorkOrderSeguimientoID equals doc.CoreWorkOrderSeguimientoID where c.CoreWorkOrderSeguimientoEstadoID == lEstadoID && !c.CoreWorkOrderSeguimientoPadreID.HasValue select c).Distinct().ToList();
                        break;
                    case "Cambios":
                        listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderSeguimientoEstadoID == lEstadoID && !c.CoreWorkOrderSeguimientoPadreID.HasValue && c.EsCambio select c).ToList();
                        break;
                    case "Comentarios":
                        listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderSeguimientoEstadoID == lEstadoID && !c.CoreWorkOrderSeguimientoPadreID.HasValue && !c.EsCambio select c).ToList();
                        break;
                    default: 
                        listaDatos = (from c in Context.CoreWorkOrderSeguimientos where c.CoreWorkOrderSeguimientoEstadoID == lEstadoID && !c.CoreWorkOrderSeguimientoPadreID.HasValue select c).ToList();
                        break;
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