using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ActivosEstadosController : GeneralBaseController<ActivosEstados, TreeCoreContext>, IBasica<ActivosEstados>
    {
        public ActivosEstadosController()
            : base()
        { }

        public bool RegistroVinculado(long ActivoEstadoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ActivoEstado, long clienteID)
        {
            bool isExiste = false;
            List<ActivosEstados> datos = new List<ActivosEstados>();


            datos = (from c in Context.ActivosEstados where (c.ActivoEstado == ActivoEstado && c.ClienteID == clienteID) select c).ToList<ActivosEstados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ActivoEstadoID)
        {
            ActivosEstados dato = new ActivosEstados();
            ActivosEstadosController cController = new ActivosEstadosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ActivoEstadoID == " + ActivoEstadoID.ToString());

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

        public ActivosEstados GetDefault(long clienteid) {
            ActivosEstados oActivosEstados;
            try
            {
                oActivosEstados = (from c in Context.ActivosEstados where c.Defecto && c.ClienteID == clienteid select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oActivosEstados = null;
            }
            return oActivosEstados;
        }

        #region ALMACEN

        public ActivosEstados GetEstadoAlmacen(bool bActivo)
        {
            List<ActivosEstados> lista = null;
            ActivosEstados dato = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.ActivosEstados where c.Activo && c.Almacen select c).ToList();
                }
                else
                {

                    lista = (from c in Context.ActivosEstados where c.Almacen select c).ToList();
                }

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<ActivosEstados>();
                return dato;
            }

            return dato;
        }

        #endregion

        #region TRASLADOS

        public ActivosEstados GetEstadoTraslados(bool bActivo)
        {
            List<ActivosEstados> lista = null;
            ActivosEstados dato = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.ActivosEstados where c.Activo && c.Traslado select c).ToList();
                }
                else
                {

                    lista = (from c in Context.ActivosEstados where c.Traslado select c).ToList();
                }

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<ActivosEstados>();
                return dato;
            }

            return dato;
        }

        #endregion

        #region BAJAS

        public ActivosEstados GetEstadoBajas(bool bActivo)
        {
            List<ActivosEstados> lista = null;
            ActivosEstados dato = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.ActivosEstados where c.Activo && c.Baja select c).ToList();
                }
                else
                {

                    lista = (from c in Context.ActivosEstados where c.Baja select c).ToList();
                }

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<ActivosEstados>();
                return dato;
            }

            return dato;
        }

        #endregion

        #region MANTENIMIENTO

        public ActivosEstados GetEstadoMantenimientos(bool bActivo)
        {
            List<ActivosEstados> lista = null;
            ActivosEstados dato = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.ActivosEstados where c.Activo && c.Mantenimiento select c).ToList();
                }
                else
                {

                    lista = (from c in Context.ActivosEstados where c.Mantenimiento select c).ToList();
                }

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<ActivosEstados>();
                return dato;
            }

            return dato;
        }

        #endregion

        #region REPARACIONES

        public ActivosEstados GetEstadoReparaciones(bool bActivo)
        {
            List<ActivosEstados> lista = null;
            ActivosEstados dato = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.ActivosEstados where c.Activo && c.Reparacion select c).ToList();
                }
                else
                {

                    lista = (from c in Context.ActivosEstados where c.Reparacion select c).ToList();
                }

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<ActivosEstados>();
                return dato;
            }

            return dato;
        }

        #endregion

    }
}