using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalDireccionesTiposCaminosController : GeneralBaseController<GlobalDireccionesTiposCaminos, TreeCoreContext>, IBasica<GlobalDireccionesTiposCaminos>
    {
        public GlobalDireccionesTiposCaminosController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalDireccionTipoCaminoID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string GlobalDireccionTipoCamino, long clienteID)
        {
            bool isExiste = false;
            List<GlobalDireccionesTiposCaminos> datos = new List<GlobalDireccionesTiposCaminos>();


            datos = (from c in Context.GlobalDireccionesTiposCaminos where (c.Nombre == GlobalDireccionTipoCamino && c.ClienteID == clienteID) select c).ToList<GlobalDireccionesTiposCaminos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalDireccionTipoCaminoID)
        {
            GlobalDireccionesTiposCaminos dato;
            GlobalDireccionesTiposCaminosController cController = new GlobalDireccionesTiposCaminosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalDireccionTipoCaminoID == " + GlobalDireccionTipoCaminoID.ToString());

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

        public GlobalDireccionesTiposCaminos GetDefault(long ClienteID)
        {
            GlobalDireccionesTiposCaminos direccionTipoCamino;
            try
            {
                direccionTipoCamino = (from c
                                       in Context.GlobalDireccionesTiposCaminos
                                       where c.Defecto &&
                                              c.ClienteID == ClienteID
                                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direccionTipoCamino = null;
            }
            return direccionTipoCamino;
        }
    }
}