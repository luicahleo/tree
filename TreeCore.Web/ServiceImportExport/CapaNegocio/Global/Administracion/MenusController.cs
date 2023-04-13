using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MenusController : GeneralBaseController<Menus, TreeCoreContext>
    {
        public MenusController()
            : base()
        { }

        public List<Vw_Menus> GetVwByModuloID(long menuModuloID, string userEmail)
        {
            List<Vw_Menus> menus = new List<Vw_Menus>();
            List<Vw_Menus> carpetas = new List<Vw_Menus>();

            try
            {
                if (userEmail == Comun.TREE_SERVICES_USER)
                {
                    carpetas = (from menu in Context.Vw_Menus
                                where menu.MenuModuloID == menuModuloID && menu.ModuloID == null
                                select menu).ToList();

                    menus = (from menu in Context.Vw_Menus
                             join modulo in Context.Modulos on menu.ModuloID equals modulo.ModuloID
                             where menu.MenuModuloID == menuModuloID && modulo.Activo == true
                             select menu).ToList();

                    menus.AddRange(carpetas);
                }
                else
                {
                    carpetas = (from menu in Context.Vw_Menus
                                where menu.MenuModuloID == menuModuloID && menu.ModuloID == null
                                select menu).ToList();

                    menus = (from menu in Context.Vw_Menus
                             join modulo in Context.Modulos on menu.ModuloID equals modulo.ModuloID
                             where menu.MenuModuloID == menuModuloID &&
                             modulo.Activo == true && modulo.SuperUser == false
                             select menu).ToList();

                    menus.AddRange(carpetas);

                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                menus = null;
            }

            return menus;
        }

        public List<Vw_Menus> GetVwActivosByNombreModuloAndFuncionalidades(string nombreModulo, List<long> listaFuncionalidades)
        {
            List<Vw_Menus> menus;
            List<long> funcionalidadesIds;

            try
            {
                funcionalidadesIds = (from c
                                   in Context.Funcionalidades
                                      where listaFuncionalidades.Contains(c.Codigo)
                                      select c.ModuloID).ToList();

                menus = (from c
                         in Context.Vw_Menus
                         where c.NombreModulo.Equals(nombreModulo) &&
                                c.ActivoMenu &&
                                (funcionalidadesIds.Contains(c.ModuloID.Value) || c.ModuloID == null)
                         orderby c.Nombre
                         select c).ToList();
            }
            catch (Exception)
            {
                menus = new List<Vw_Menus>();
            }
            return menus;
        }

        public bool RegistroDuplicado(string Menu, long lModuloID)
        {
            bool isExiste = false;

            try
            {
                if (lModuloID != 0)
                {
                    isExiste = (from c in Context.Menus
                                where (c.Nombre == Menu &&
        c.MenuModuloID == lModuloID)
                                select c).Count() > 0;
                }
                else
                {
                    isExiste = (from c in Context.Menus
                                where (c.Nombre == Menu)
                                select c).Count() > 0;
                }

            }
            catch (Exception)
            {
                isExiste = false;
            }

            return isExiste;
        }

        public bool HasChildren(long menuID)
        {
            bool hasChildren = false;

            try
            {
                hasChildren = (from c
                               in Context.Menus
                               where c.MenuPadreID == menuID
                               select c).Count() > 0;
            }
            catch (Exception)
            {
                hasChildren = true;
            }

            return hasChildren;
        }

        public List<Vw_Menus> GetNodosHijos(long menuID)
        {
            List<Vw_Menus> menus;

            menus = (from c
                     in Context.Vw_Menus
                     where c.MenuPadreID == menuID
                     select c).ToList();

            return menus;
        }

        public int GetMaxDepth(long menuID, int limit)
        {
            return GetMaxDepthRecursivo(menuID, 0, 0, limit);
        }

        private int GetMaxDepthRecursivo(long menuID, int count, int max, int limit)
        {
            if (HasChildren(menuID))
            {
                count++;
                if (count > max) { max = count; }

                List<Vw_Menus> lMenus = GetNodosHijos(menuID);
                foreach (Vw_Menus oMenu in lMenus)
                {
                    if (max == limit) { break; }
                    return GetMaxDepthRecursivo(oMenu.MenuID, count, max, limit);
                }
            }

            return max;
        }

        public int GetNivelNodo(long menuID)
        {
            return NivelNodoRecursivo(menuID, 1);
        }

        private int NivelNodoRecursivo(long menuID, int nivel)
        {
            long? lMenuPadreID = GetItem(menuID).MenuPadreID;

            if (lMenuPadreID == null)
            {
                return nivel;
            }

            return NivelNodoRecursivo(Convert.ToInt64(lMenuPadreID), ++nivel);
        }

        public List<string> GetNombreModulosByFuncionalidades(List<long> listaFuncionalidades)
        {
            List<string> menus;
            List<long> funcionalidadesIds;
            try
            {
                funcionalidadesIds = (from c in Context.Funcionalidades where listaFuncionalidades.Contains(c.Codigo) select c.ModuloID).ToList();
                menus = (from c in Context.Vw_Menus where c.ActivoMenu && (funcionalidadesIds.Contains(c.ModuloID.Value) || c.ModuloID == null) orderby c.Nombre select c.NombreModulo).ToList();
            }
            catch (Exception)
            {
                menus = new List<string>();
            }
            return menus;
        }

        public List<MenuFavoritosUsuarios> GetMenusFavoritosByUsuarioID(long usuarioID)
        {
            return (from c in Context.MenuFavoritosUsuarios where c.UsuarioID == usuarioID select c).ToList();
        }

        public MenuFavoritosUsuarios GetMenuFavoritoByUsuarioIDAndMenuID(long usuarioID, long menuID)
        {
            return (from c in Context.MenuFavoritosUsuarios where c.UsuarioID == usuarioID && c.MenuID == menuID select c).FirstOrDefault();
        }

        public string getNombreByAliasFromUserControl(Vw_Menus oDato, TreeCore.Page.BaseUserControl paginaRef)
        {
            string sNombre = "";

            try
            {
                if (oDato.Nombre != "")
                {
                    sNombre = oDato.Nombre;
                }
                if (oDato.Alias != "" && sNombre == "")
                {
                    sNombre = paginaRef.GetGlobalResource(oDato.Alias);
                }
                if (oDato.AliasModulo != null && sNombre == "")
                {
                    sNombre = paginaRef.GetGlobalResource(oDato.AliasModulo);
                }
                if (sNombre == "")
                {
                    sNombre = oDato.DescripcionPagina;
                }
            }
            catch (Exception)
            {
                sNombre = "";
            }

            return sNombre;
        }

        public string getNombreByAliasFromBasePageExtNet(Vw_Menus oDato, TreeCore.Page.BasePageExtNet paginaRef)
        {
            string sNombre = "";

            try
            {
                if (oDato.Nombre != "")
                {
                    sNombre = oDato.Nombre;
                }
                if (oDato.Alias != "" && sNombre == "")
                {
                    sNombre = paginaRef.GetGlobalResource(oDato.Alias);
                }
                if (oDato.AliasModulo != null && sNombre == "")
                {
                    sNombre = paginaRef.GetGlobalResource(oDato.AliasModulo);
                }
                if (sNombre == "")
                {
                    sNombre = oDato.DescripcionPagina;
                }
            }
            catch (Exception)
            {
                sNombre = "";
            }

            return sNombre;
        }

    }
}