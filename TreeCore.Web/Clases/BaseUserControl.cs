using System;
using System.Web;
using CapaNegocio;
using TreeCore.Data;
using Ext.Net;

namespace TreeCore.Page
{
    public class BaseUserControl : System.Web.UI.UserControl
    {
        public UsuariosSesiones UsuariosSesiones;
        public string SesionGUID;

        public BaseUserControl()
        {
            HttpCookie SesionGUIDCookie = Cookies.GetCookie("SesionGUID");
            if ((SesionGUIDCookie != null) && !string.IsNullOrEmpty(SesionGUIDCookie.Value))
            {
                SesionGUID = SesionGUIDCookie.Value;
                UsuariosSesionesController SesionController = new UsuariosSesionesController();
                UsuariosSesiones = SesionController.GetSesion(SesionGUID);
            }
        }

        public string GetGlobalResource(string key)
        {
            object valor;
            try
            {
                valor = this.GetGlobalResourceObject(Comun.NOMBRE_FICHERO_RECURSOS, key);
                if (valor != null)
                {
                    return valor.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}