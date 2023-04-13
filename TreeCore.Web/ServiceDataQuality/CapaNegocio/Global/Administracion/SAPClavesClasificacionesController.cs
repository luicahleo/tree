using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class SAPClavesClasificacionesController : GeneralBaseController<SAPClavesClasificaciones, TreeCoreContext>, IBasica<SAPClavesClasificaciones>
    {
        public SAPClavesClasificacionesController()
            : base()
        { }

        public bool RegistroVinculado(long SAPClaveClasificacionID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SAPClaveClasificacion, long clienteID)
        {
            bool isExiste = false;
            List<SAPClavesClasificaciones> datos;

            try
            {
                datos = (from c
                         in Context.SAPClavesClasificaciones
                         where (c.SAPClaveClasificacion == SAPClaveClasificacion &&
                                c.ClienteID == clienteID)
                         select c).ToList<SAPClavesClasificaciones>();

                if (datos.Count > 0)
                {
                    isExiste = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                isExiste = false;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPClaveClasificacionID)
        {
            SAPClavesClasificaciones dato;
            SAPClavesClasificacionesController cController = new SAPClavesClasificacionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPClaveClasificacionID == " + SAPClaveClasificacionID.ToString());

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

        public SAPClavesClasificaciones GetDefault(long clienteID)
        {
            SAPClavesClasificaciones claveClasificacion;

            try
            {
                claveClasificacion = (from c 
                                      in Context.SAPClavesClasificaciones 
                                      where c.Defecto &&
                                            c.ClienteID == clienteID
                                      select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                claveClasificacion = null;
            }

            return claveClasificacion;
        }
        public List<SAPClavesClasificaciones> GetSAPClavesClasificacionesByCliente(long clienteID)
        {
            List<SAPClavesClasificaciones> datos = new List<SAPClavesClasificaciones>();

            datos = (from c in Context.SAPClavesClasificaciones where (c.ClienteID == clienteID) orderby c.Descripcion select c).ToList<SAPClavesClasificaciones>();

            return datos;
        }

        public SAPClavesClasificaciones GetClaveClasificacionByNombre(string sNombre)
        {
            List<SAPClavesClasificaciones> lista = null;
            SAPClavesClasificaciones dato = null;

            try
            {

                lista = (from c in Context.SAPClavesClasificaciones where c.SAPClaveClasificacion == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }
    }
}