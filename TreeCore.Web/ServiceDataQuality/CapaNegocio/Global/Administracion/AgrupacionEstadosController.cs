using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
namespace CapaNegocio
{
    public class AgrupacionEstadosController : GeneralBaseController<EstadosAgrupaciones, TreeCoreContext>, IBasica<EstadosAgrupaciones>
    {
        public AgrupacionEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string NombreEstadosAgrupaciones, long clienteID)
        {
            bool isExiste = false;
            List<EstadosAgrupaciones> datos;

            datos = (from c in Context.EstadosAgrupaciones where (c.Nombre == NombreEstadosAgrupaciones && c.ClienteID == clienteID) select c).ToList<EstadosAgrupaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long estadoAgrupacionID)
        {

            List<EstadosAgrupaciones>  dato;

            dato = (from c in Context.EstadosAgrupaciones where c.Defecto && c.EstadoAgrupacionID == estadoAgrupacionID select c).ToList();

            if (dato.Count != 0)
            {
                return true;
            } else
            {
                return false;
            }

        }

        public EstadosAgrupaciones GetDefault(long ClienteID)
        {
            EstadosAgrupaciones dato;
            try
            {
                dato = (from c 
                         in Context.EstadosAgrupaciones
                         where c.Defecto && 
                                c.ClienteID == ClienteID 
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public EstadosAgrupaciones GetEstadosAgrupacionesByNombre(string sNombre)
        {
            EstadosAgrupaciones datos = null;
            List<EstadosAgrupaciones> lista = null;
            try
            {
                datos = new EstadosAgrupaciones();
                lista = (from c in Context.EstadosAgrupaciones where (c.Nombre == sNombre) select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    datos = lista.ElementAt(0);
                }
                else
                {
                    datos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }
            return datos;
        }

        public long getNombreAgrupacion (string sNombre)
        {
            long lValor = 0;

            try
            {
                lValor = (from c in Context.EstadosAgrupaciones where c.Nombre == sNombre select c.EstadoAgrupacionID).First();
            }
            catch(Exception)
            {
                lValor = 0;
            }

            return lValor;
        }
    }
}