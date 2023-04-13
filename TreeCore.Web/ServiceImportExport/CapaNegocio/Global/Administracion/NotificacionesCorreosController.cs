using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class NotificacionesCorreosController : GeneralBaseController<NotificacionesCorreos, TreeCoreContext>
    {
        public NotificacionesCorreosController()
            : base()
        { }

        public string GetCadenaDireccionesCorreosByNotificacion(long NotificacionID)
        {
            // Local variables
            List<NotificacionesCorreos> lista = null;
            string listaDirecciones = null;
            UsuariosPerfilesController cPerfil = null;
            List<Usuarios> usuarios = null;

            try
            {
                lista = (from c in Context.NotificacionesCorreos where c.NotificacionID == NotificacionID && c.Activo == true select c).ToList();

                // Searches for the profile information
                if (lista != null && lista.Count > 0)
                {
                    listaDirecciones = "";

                    foreach (NotificacionesCorreos correo in lista)
                    {
                        if (correo.Correo != null && !correo.Correo.Equals(""))
                        {
                            listaDirecciones = listaDirecciones + correo.Correo + ";";
                        }
                        else
                        {
                            cPerfil = new UsuariosPerfilesController();
                            usuarios = cPerfil.ObtenerUsuariosPorPerfilID((long)correo.PerfilID);
                            foreach (Usuarios user in usuarios)
                            {
                                if (user.EMail != null && !user.EMail.Equals(""))
                                {

                                    listaDirecciones = listaDirecciones + user.EMail + ";";

                                }
                            }
                        }
                    }

                    listaDirecciones = listaDirecciones.Substring(0, listaDirecciones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDirecciones = null;
            }

            // Returns the result
            return listaDirecciones;
        }
        public bool RegistroDuplicado(string correo)
        {
            bool isExiste = false;
            List<NotificacionesCorreos> datos = new List<NotificacionesCorreos>();


            datos = (from c in Context.NotificacionesCorreos where (c.Correo == correo) select c).ToList<NotificacionesCorreos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}