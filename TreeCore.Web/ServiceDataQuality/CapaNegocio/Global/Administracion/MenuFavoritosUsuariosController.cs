using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MenuFavoritosUsuariosController : GeneralBaseController<MenuFavoritosUsuarios, TreeCoreContext>
    {
        public MenuFavoritosUsuariosController()
            : base()
        { }

        public List<MenuFavoritosUsuarios> GetFavoritosByUsuarioID(long lUsuarioID)
        {
            List<MenuFavoritosUsuarios> listaFavoritos;

            try
            {
                listaFavoritos = (from c in Context.MenuFavoritosUsuarios where c.UsuarioID == lUsuarioID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFavoritos = null;
            }

            return listaFavoritos;
        }

        public List<Vw_MenuFavoritosUsuarios> GetAllOrderByMenuModulo(long lClienteID)
        {
            List<Vw_MenuFavoritosUsuarios> listaFavoritos;

            try
            {
                listaFavoritos = (from c in Context.Vw_MenuFavoritosUsuarios where c.UsuarioID == lClienteID orderby c.MenuModulo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFavoritos = null;
            }

            return listaFavoritos;
        }
    }
}