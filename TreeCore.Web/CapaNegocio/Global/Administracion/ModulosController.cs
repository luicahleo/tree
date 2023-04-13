using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class ModulosController : GeneralBaseController<Modulos, TreeCoreContext>
    {
        public ModulosController()
            : base()
        {

        }

#if SERVICESETTINGS
    public static bool Produccion = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["Produccion"]);
#elif TREEAPI
    public static bool Produccion = TreeAPI.Properties.Settings.Default.Produccion;
#else
        public static bool Produccion = TreeCore.Properties.Settings.Default.Produccion;
#endif

        public bool RegistroDuplicado(string Modulo)
        {
            bool isExiste = false;
            List<Modulos> datos = new List<Modulos>();


            datos = (from c in Context.Modulos where (c.Modulo == Modulo) select c).ToList<Modulos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        /// <summary>
        /// Obtiene los modulos que pertenece al usuario
        /// </summary>
        /// <param name="userID">ientificador del Usuario</param>
        /// <returns></returns>
        /// <remarks>MDS</remarks>
        public List<string> getModulos(long userID)
        {
            List<Modulos> modulos;
            try
            {
                modulos = (from userRol in Context.UsuariosRoles
                           join rol in Context.Roles on userRol.RolID equals rol.RolID
                           join rolPerfil in Context.RolesPerfiles on rol.RolID equals rolPerfil.RolID
                           join perfFunc in Context.PerfilesFuncionalidades on rolPerfil.PerfilID equals perfFunc.PerfilID
                           join funcionalidad in Context.Funcionalidades on perfFunc.FuncionalidadID equals funcionalidad.FuncionalidadID
                           join modulo in Context.Modulos on funcionalidad.ModuloID equals modulo.ModuloID
                           where
                                userRol.UsuarioID == userID &&
                                rol.Activo
                           select modulo).ToList();
                modulos.AddRange((from c in Context.Modulos
                                  where c.ModuloPadreID.HasValue && modulos.Select(e => e.ModuloID).ToList().Contains((long)c.ModuloPadreID)
                                  select c).ToList());

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = new List<Modulos>();
            }
            return modulos.Select(c => c.Pagina).ToList();
        }

        public List<Modulos> getModulosObj(long userID)
        {
            List<Modulos> modulos;
            try
            {
                modulos = (from userRol in Context.UsuariosRoles
                           join rol in Context.Roles on userRol.RolID equals rol.RolID
                           join rolPerfil in Context.RolesPerfiles on rol.RolID equals rolPerfil.RolID
                           join perfFunc in Context.PerfilesFuncionalidades on rolPerfil.PerfilID equals perfFunc.PerfilID
                           join funcionalidad in Context.Funcionalidades on perfFunc.FuncionalidadID equals funcionalidad.FuncionalidadID
                           join modulo in Context.Modulos on funcionalidad.ModuloID equals modulo.ModuloID
                           where
                                userRol.UsuarioID == userID &&
                                rol.Activo
                           select modulo).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = new List<Modulos>();
            }
            return modulos;
        }

        public List<Modulos> getAllModulos(long userID)
        {
            List<Modulos> modulos;
            try
            {
                modulos = (from userRol in Context.UsuariosRoles
                           join rol in Context.Roles on userRol.RolID equals rol.RolID
                           join rolPerfil in Context.RolesPerfiles on rol.RolID equals rolPerfil.RolID
                           join perfFunc in Context.PerfilesFuncionalidades on rolPerfil.PerfilID equals perfFunc.PerfilID
                           join funcionalidad in Context.Funcionalidades on perfFunc.FuncionalidadID equals funcionalidad.FuncionalidadID
                           join modulo in Context.Modulos on funcionalidad.ModuloID equals modulo.ModuloID
                           where
                                userRol.UsuarioID == userID &&
                                rol.Activo
                           select modulo).ToList();
                modulos.AddRange(
                        (from c in Context.Modulos where c.ModuloPadreID.HasValue && (from mod in modulos select mod.ModuloID).ToList().Contains((long)c.ModuloPadreID) select c).ToList()
                    );

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = new List<Modulos>();
            }
            return modulos;
        }

        public List<string> getModulosSuperUser()
        {
            List<string> modulos;
            try
            {
                modulos = (from c in Context.Modulos where c.Activo select c.Pagina).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = new List<string>();
            }
            return modulos;
        }

        /// <summary>
        /// Comprueba si el módulo que se le pasa como parámetro tiene registros asociados
        /// </summary>
        /// <param name="moduloID">identificador del módulo</param>
        /// <returns></returns>
        /// <remarks>JJFD</remarks>
        public bool tieneRegistrosAsociado(long moduloID)
        {
            bool tiene = false;
            FuncionalidadesController fControl = new FuncionalidadesController();
            List<Funcionalidades> datos;
            datos = fControl.GetItemsList("ModuloID == " + moduloID.ToString());
            if (datos.Count > 0)
            {
                tiene = true;
            }
            return tiene;
        }

        public List<Modulos> getModulosLibresPerfil(long lPerfilID)
        {
            PerfilesController cPerfiles = new PerfilesController();
            Perfiles oPerfil;
            List<Modulos> modulos;

            try
            {
                oPerfil = cPerfiles.GetItem(lPerfilID);
                List<long> listaIDFuncionalidadesAsociados = (from c in Context.PerfilesFuncionalidades where c.PerfilID == oPerfil.PerfilID select c.FuncionalidadID).ToList();
                if (listaIDFuncionalidadesAsociados != null)
                {
                    List<long> listaIDModulosAsociados = (from c in Context.Funcionalidades where listaIDFuncionalidadesAsociados.Contains(c.FuncionalidadID) select c.ModuloID).ToList();

                    if (Produccion)
                    {
                        modulos = (from c in Context.Modulos where !(listaIDModulosAsociados.Contains(c.ModuloID)) && c.ProyectoTipoID == oPerfil.TipoProyectoID && c.Activo == true && c.SuperUser == false && c.Produccion == true select c).ToList();
                    }
                    else
                    {
                        modulos = (from c in Context.Modulos where !(listaIDModulosAsociados.Contains(c.ModuloID)) && c.ProyectoTipoID == oPerfil.TipoProyectoID && c.Activo == true && c.SuperUser == false select c).ToList();
                    }
                }
                else
                {
                    modulos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = null;
            }
            return modulos;
        }
        public List<Modulos> getModulosLibresPerfilSuper(long lPerfilID)
        {
            PerfilesController cPerfiles = new PerfilesController();
            Perfiles oPerfil;
            List<Modulos> modulos;
            try
            {
                oPerfil = cPerfiles.GetItem(lPerfilID);
                List<long> listaIDFuncionalidadesAsociados = (from c in Context.PerfilesFuncionalidades where c.PerfilID == oPerfil.PerfilID select c.FuncionalidadID).ToList();
                if (listaIDFuncionalidadesAsociados != null)
                {
                    List<long> listaIDModulosAsociados = (from c in Context.Funcionalidades where listaIDFuncionalidadesAsociados.Contains(c.FuncionalidadID) select c.ModuloID).ToList();
                    modulos = (from c in Context.Modulos where !(listaIDModulosAsociados.Contains(c.ModuloID)) && c.ProyectoTipoID == oPerfil.TipoProyectoID && c.Activo select c).ToList();
                }
                else
                {
                    modulos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = null;
            }
            return modulos;
        }
        public List<Modulos> getModulosPerfil(long lPerfilID)
        {
            PerfilesController cPerfiles = new PerfilesController();
            Perfiles oPerfil;
            List<Modulos> modulos;
            try
            {
                oPerfil = cPerfiles.GetItem(lPerfilID);
                List<long> listaIDFuncionalidadesAsociados = (from c in Context.PerfilesFuncionalidades where c.PerfilID == oPerfil.PerfilID select c.FuncionalidadID).ToList();
                if (listaIDFuncionalidadesAsociados != null)
                {
                    List<long> listaIDModulosAsociados = (from c in Context.Funcionalidades where listaIDFuncionalidadesAsociados.Contains(c.FuncionalidadID) select c.ModuloID).ToList();
                    modulos = (from c in Context.Modulos where (listaIDModulosAsociados.Contains(c.ModuloID)) && c.ProyectoTipoID == oPerfil.TipoProyectoID && c.Activo select c).ToList();
                }
                else
                {
                    modulos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                modulos = null;
            }
            return modulos;
        }

        public List<Modulos> getModulosbyProyectoTipo(long ProyectoTipo)
        {
            List<Modulos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Modulos where c.ProyectoTipoID == ProyectoTipo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public Modulos getModuloByPagina(string sPagina)
        {
            Modulos oModulo;

            try
            {
                oModulo = (from c in Context.Modulos where c.Pagina == sPagina select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oModulo = null;
            }

            return oModulo;
        }

        public List<Modulos> getModulosActivos()
        {
            List<Modulos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Modulos where c.Activo == true select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Modulos> getModulosNoSuper()
        {
            List<Modulos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Modulos where c.SuperUser == false select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

    }
}
