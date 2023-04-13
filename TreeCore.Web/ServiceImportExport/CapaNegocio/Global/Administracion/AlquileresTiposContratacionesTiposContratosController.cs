using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;


namespace CapaNegocio
{
    public class AlquileresTiposContratacionesTiposContratosController : GeneralBaseController<AlquileresTiposContratacionesTiposContratos, TreeCoreContext>
    {
        // GET: AlquileresTiposContratacionesTiposContratos
        public AlquileresTiposContratacionesTiposContratosController()
            : base()
        {
        }

        public List<Vw_AlquileresTiposContratacionesTiposContratos> tiposContratosAsignados(long tipoContratacionID)
        {
            //List<long> tiposContratos;
            //tiposContratos = (from c in Context.AlquileresTiposContratacionesTiposContratos where c.AlquilerTipoContratacionID == tipoContratacionID && c.Activo== true select c.AlquilerTipoContratoID).ToList<long>();
            return (from c in Context.Vw_AlquileresTiposContratacionesTiposContratos where c.AlquilerTipoContratacionID == tipoContratacionID && c.Activo select c).ToList<Vw_AlquileresTiposContratacionesTiposContratos>();
        }


        /// <summary>
        /// Obtiene la lista de Contratos que aún no tiene asignado el tipo de contratacion que se le pasa como parámetro
        /// </summary>
        /// <remarks>Mds</remarks>

        public List<AlquileresTiposContratos> tiposContratosNoAsignado(long tipoContratacionID)
        {
            List<long> tiposContratos;
            tiposContratos = (from c in Context.AlquileresTiposContratacionesTiposContratos where c.AlquilerTipoContratacionID == tipoContratacionID && c.Activo select c.AlquilerTipoContratoID).ToList<long>();

            return (from c in Context.AlquileresTiposContratos where (!(tiposContratos.Contains(c.AlquilerTipoContratoID)) && c.Activo) select c).ToList<AlquileresTiposContratos>();
        }

        public List<Vw_AlquileresTiposContratacionesTiposContratos> GetViewListByAlquilerTipoContratacionID(long alquilerTipoContratacionID)
        {
            List<Vw_AlquileresTiposContratacionesTiposContratos> lista;
            try
            {
                lista = (from c
                         in Context.Vw_AlquileresTiposContratacionesTiposContratos
                         where c.AlquilerTipoContratacionID == alquilerTipoContratacionID
                         orderby c.TipoContrato
                         select c).ToList();
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