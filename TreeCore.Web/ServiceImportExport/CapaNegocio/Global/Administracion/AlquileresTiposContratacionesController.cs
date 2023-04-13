using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class AlquileresTiposContratacionesController : GeneralBaseController<AlquileresTiposContrataciones, TreeCoreContext>, IBasica<AlquileresTiposContrataciones>
    {
        public AlquileresTiposContratacionesController()
            : base()
        { }

        public bool RegistroVinculado(long AlquilerTipoContratacionID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string TipoContratacion, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresTiposContrataciones> datos;


            datos = (from c in Context.AlquileresTiposContrataciones where (c.TipoContratacion == TipoContratacion && c.ClienteID == clienteID) select c).ToList<AlquileresTiposContrataciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerTipoContratacionID)
        {
            AlquileresTiposContrataciones dato;
            AlquileresTiposContratacionesController cController = new AlquileresTiposContratacionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerTipoContratacionID == " + AlquilerTipoContratacionID.ToString());

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

        public AlquileresTiposContrataciones GetDefault(long ClienteID)
        {
            AlquileresTiposContrataciones banco;
            try
            {
                banco = (from c
                         in Context.AlquileresTiposContrataciones
                         where c.Defecto &&
                            c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                banco = null;
            }
            return banco;
        }

        public List<AlquileresTiposContrataciones> GetListByClienteId(long clienteID)
        {
            List<AlquileresTiposContrataciones> listaTiposContrataciones;

            try
            {
                listaTiposContrataciones = (from c
                                            in Context.AlquileresTiposContrataciones
                                            where c.ClienteID == clienteID
                                            orderby c.TipoContratacion
                                            select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTiposContrataciones = null;
            }
            return listaTiposContrataciones;
        }

        public long GetTipoByNombreAll(string Nombre)
        {
            long tipoID = 0;
            //List<AlquileresTiposContrataciones> list = new List<AlquileresTiposContrataciones>();

            try
            {
                //list = (from c in Context.AlquileresTiposContrataciones select c).ToList();
                tipoID = (from c in Context.AlquileresTiposContrataciones where c.TipoContratacion.Equals(Nombre) select c.AlquilerTipoContratacionID).First();

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