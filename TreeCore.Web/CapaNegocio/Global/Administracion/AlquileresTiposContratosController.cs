using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
//using System.Web.Optimization;
//using Microsoft.Ajax.Utilities;

namespace CapaNegocio
{
    public class AlquileresTiposContratosController : GeneralBaseController<AlquileresTiposContratos, TreeCoreContext>, IBasica<AlquileresTiposContratos>
    {
        public AlquileresTiposContratosController()
            : base()
        { }

        public bool RegistroVinculado(long AlquilerTipoContratoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoContrato, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresTiposContratos> datos = new List<AlquileresTiposContratos>();


            datos = (from c in Context.AlquileresTiposContratos where (c.TipoContrato == TipoContrato && c.ClienteID == clienteID) select c).ToList<AlquileresTiposContratos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerTipoContratoID)
        {
            AlquileresTiposContratos dato = new AlquileresTiposContratos();
            AlquileresTiposContratosController cController = new AlquileresTiposContratosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerTipoContratoID == " + AlquilerTipoContratoID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public AlquileresTiposContratos GetDefault(long clienteID)
        {
            AlquileresTiposContratos oAlquileresTiposContratos;
            try
            {
                oAlquileresTiposContratos = (from c in Context.AlquileresTiposContratos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oAlquileresTiposContratos = null;
            }
            return oAlquileresTiposContratos;
        }

        public long GetTipoByNombreTipoContratacion(string Nombre, long tipoContratacionID)
        {

            long tipoID = 0;
            List<AlquileresTiposContratos> list = new List<AlquileresTiposContratos>();
            List<AlquileresTiposContratacionesTiposContratos> lTipoContratacionContrato = new List<AlquileresTiposContratacionesTiposContratos>();
            try
            {
                //list = (from c in Context.AlquileresTiposContratos select c).ToList();
                list = (from c in Context.AlquileresTiposContratos where c.TipoContrato.Equals(Nombre) select c).ToList();
                if (list != null)
                {
                    tipoID = list.ElementAt(0).AlquilerTipoContratoID;
                    lTipoContratacionContrato = (from a in Context.AlquileresTiposContratacionesTiposContratos where a.AlquilerTipoContratacionID == tipoContratacionID && a.AlquilerTipoContratoID == tipoID select a).ToList();
                    if (lTipoContratacionContrato == null)
                    {
                        tipoID = -1;
                    }
                }
                else
                {
                    tipoID = -1;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;
            }

            return tipoID;
        }
    }
}