using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalDireccionesTiposController : GeneralBaseController<GlobalDireccionesTipos, TreeCoreContext>, IBasica<GlobalDireccionesTipos>
    {
        public GlobalDireccionesTiposController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalDireccionTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Nombre, long clienteID)
        {
            bool isExiste = false;
            List<GlobalDireccionesTipos> datos = new List<GlobalDireccionesTipos>();


            datos = (from c in Context.GlobalDireccionesTipos where (c.Nombre == Nombre && c.ClienteID == clienteID) select c).ToList<GlobalDireccionesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalDireccionTipoID)
        {
            GlobalDireccionesTipos dato = new GlobalDireccionesTipos();
            GlobalDireccionesTiposController cController = new GlobalDireccionesTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalDireccionTipoID == " + GlobalDireccionTipoID.ToString());

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

        public bool isExisteDireccion(string Direccion)
        {
            bool isExiste = false;

            List<GlobalDireccionesTipos> ListaDireccion;

            ListaDireccion = (from c in Context.GlobalDireccionesTipos where c.Nombre == Direccion select c).ToList<GlobalDireccionesTipos>();
            if (ListaDireccion.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public GlobalDireccionesTipos GetDefault(long ClienteID)
        {
            GlobalDireccionesTipos direccionTipo;
            try
            {
                direccionTipo = (from c 
                                 in Context.GlobalDireccionesTipos 
                                 where c.Defecto && 
                                        c.ClienteID == ClienteID 
                                 select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direccionTipo = null;
            }
            return direccionTipo;
        }
    }
}