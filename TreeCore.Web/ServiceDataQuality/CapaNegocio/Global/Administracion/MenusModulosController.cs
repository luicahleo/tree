using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MenusModulosController : GeneralBaseController<MenusModulos, TreeCoreContext>, IBasica<MenusModulos>
    {
        public MenusModulosController()
            : base()
        { }

        public bool RegistroDuplicado(string MenusModulos, long clienteID)
        {
            bool isExiste = false;
            List<MenusModulos> datos;

            datos = (from c
                     in Context.MenusModulos
                     where c.Modulo == MenusModulos
                     select c).ToList<MenusModulos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<MenusModulos> GetAll()
        {
            List<MenusModulos> lista;
            try
            {
                lista = (from c in Context.MenusModulos orderby c.Modulo ascending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public bool RegistroVinculado(long id)
        {
            throw new NotImplementedException();
        }

        public bool RegistroDefecto(long id)
        {
            throw new NotImplementedException();
        }

        public List<MenusModulos> GetActivos()
        {
            List<MenusModulos> lista;
            try
            {
                lista = (from c
                         in Context.MenusModulos
                         where c.Activo
                         orderby c.Modulo
                         ascending
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public MenusModulos GetMenuModuloByStr(string strRecurso)
        {
            MenusModulos res = new MenusModulos();

            res = (from c in Context.MenusModulos
                   where c.Activo == true && c.Modulo == strRecurso
                   select c).FirstOrDefault();

            return res;
        }

        public MenusModulos GetMenuModuloByProyectoTipoId(long? id)
        {

            MenusModulos res = null;

            long? menuModuloID = (from c in Context.ProyectosTipos
                                  where c.Activo == true && c.ProyectoTipoID == id
                                  select c.MenuModuloID).FirstOrDefault();

            if (menuModuloID != null)
            {
                res = (from c in Context.MenusModulos
                       where c.Activo == true && c.MenuModuloID == menuModuloID
                       select c).FirstOrDefault();
            }

            return res;
        }
    }
}