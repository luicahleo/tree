using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public sealed class UsuariosAccesosController : GeneralBaseController<UsuariosAccesos, TreeCoreContext>
    {
        public UsuariosAccesosController()
            : base()
        {

        }

        /// <summary>
        /// Establece la sesión del usuario con la ip que se conecto
        /// </summary>
        /// <param name="ip">dirección ip del usuario al conectarse</param>
        /// <param name="usuarioID">identificador del usuario</param>
        /// <param name="sesionID">sesión iniciada del usuario</param>
        /// <remarks>JJFD</remarks>
        public void addSesion(string ip, long usuarioID, string sesionID)
        {
            UsuariosAccesos item;
            item = base.GetItem("UsuarioID = " + usuarioID);
            //si existe el usuario en el control de acceso de usuarios se actualizará
            if (item != null)
            {
                item.IP = ip;
                item.SesionID = sesionID;
                this.UpdateItem(item);
            }
            //en caso contrario se insertará el usuario, la ip y la sesión en el control de acceso de usuarios
            else
            {
                item = new UsuariosAccesos();
                item.IP = ip;
                item.UsuarioID = usuarioID;
                item.SesionID = sesionID;
                this.AddItem(item);
            }
        }

        /// <summary>
        /// Limpia la sesión del usuario y la dirección ip
        /// </summary>
        /// <param name="usuarioID">identificador del usuario</param>
        /// <remarks>JJFD</remarks>
        public void limpiarSesion(long usuarioID)
        {
            UsuariosAccesos item = base.GetItem("UsuarioID = " + usuarioID);
            if (item != null)
            {
                item.IP = "";
                item.SesionID = "";
                this.UpdateItem(item);
            }
        }

    }
}
