using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class LicenciasTiposController : GeneralBaseController<LicenciasTipos, TreeCoreContext>, IBasica<LicenciasTipos>
    {
        public LicenciasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long LicenciaTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string LicenciaTipo, long clienteID)
        {
            bool isExiste = false;
            List<LicenciasTipos> datos = new List<LicenciasTipos>();


            datos = (from c in Context.LicenciasTipos where (c.LicenciaTipo == LicenciaTipo && c.ClienteID == clienteID) select c).ToList<LicenciasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long LicenciaTipoID)
        {
            LicenciasTipos dato = new LicenciasTipos();
            LicenciasTiposController cController = new LicenciasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && LicenciaTipoID == " + LicenciaTipoID.ToString());

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

        public List<LicenciasTipos> GetActivos()
        {
            List<LicenciasTipos> listaLicencias;

            try
            {
                listaLicencias = (from c in Context.LicenciasTipos where c.Activo orderby c.LicenciaTipo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaLicencias = null;
            }

            return listaLicencias;
        }

        public LicenciasTipos GetDefault(long clienteID)
        {
            LicenciasTipos oTipo;
            try
            {
                oTipo = (from c in Context.LicenciasTipos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipo = null;
            }

            return oTipo;
        }

        #region FUNCIONES GENERALES

        /// <summary>
        /// Comprueba si el tipo de licencia que se le pasa como parámetro tiene registros asociados
        /// </summary>
        /// <param name="tipoID">identificador del elemento</param>
        /// <returns>si tiene registros asignados o no (bool)</returns>
        /// <remarks>MDF</remarks>
        public bool tieneRegistrosAsociado(long tipoID)
        {
            bool tiene = false;
            LicenciasController raControl = new LicenciasController();
            List<Licencias> datos;
            datos = raControl.GetItemsList("LicenciaTipoID == " + tipoID.ToString());
            //comprueba que tiene avisos asignados
            if (datos.Count > 0)
            {
                tiene = true;
            }

            return tiene;
        }



        /// <summary>
        /// Comprueba si el nombre del tipo de licencia está ya en uso o no
        /// </summary>
        /// <param name="nombreLicencia">Nombre del tipo de licencia que se desea comprobar</param>        
        /// <returns>True en caso de existir un tipo de licencia con el nombre indicado</returns>
        public bool ExisteLicenciaTipo(string nombreLicencia)
        {
            bool res = false;


            //Se recupera la lista de roles con ese nombre cuyo Id no sea el indicado
            List<LicenciasTipos> roles = (from c in Context.LicenciasTipos where c.LicenciaTipo == nombreLicencia select c).ToList();

            if (roles != null && roles.Count > 0)
            {
                res = true;
            }

            return res;
        }


        /// <summary>
        /// Devuelve el tipo de licencia por su codigo
        /// </summary>
        /// <param name="nombreLicencia">Nombre del tipo de licencia que se desea comprobar</param>        
        /// <returns>True en caso de existir un tipo de licencia con el nombre indicado</returns>
        public LicenciasTipos GetLicenciaTipoByCodigo(string sCodigo)
        {
            LicenciasTipos res = null;


            //Se recupera la lista de roles con ese nombre cuyo Id no sea el indicado
            List<LicenciasTipos> roles = (from c in Context.LicenciasTipos where c.CodigoLicenciaTipo == sCodigo select c).ToList();

            if (roles != null && roles.Count > 0)
            {
                res = roles.ElementAt(0);
            }

            return res;
        }

        #endregion
    }
}