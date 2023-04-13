using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using TreeCore.Clases;

namespace CapaNegocio
{
    public sealed class UsuariosRolesController : GeneralBaseController<UsuariosRoles, TreeCoreContext>, IGestionBasica<UsuariosRoles>
    {
        public UsuariosRolesController()
            : base()
        {

        }

        public InfoResponse Add(UsuariosRoles oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(UsuariosRoles oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(UsuariosRoles oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        #region GLOBAL
        /// <summary>
        /// Obtiene la lista de Perfiles que aún no tiene asignado el usuario que se le pasa como parámetro
        /// </summary>
        /// <remarks>Mds</remarks>

        public List<Roles> perfilesNoAsignado(long usuarioID, long clienteID)
        {
            List<long> roles;
            roles = (from c in Context.UsuariosRoles where c.UsuarioID == usuarioID select c.RolID).ToList<long>();

            return (from c in Context.Roles where (!(roles.Contains(c.RolID))) && c.ClienteID == clienteID && c.Activo == true select c).OrderBy("Nombre").ToList<Roles>();


        }

        /// <summary>
        /// Obtiene los Perfiles asignados a un usuario.
        /// </summary>

        public List<Vw_UsuariosPerfiles> perfilesAsignados(long usuarioID)
        {
            List<long> perfiles;
            perfiles = (from c in Context.UsuariosPerfiles where c.UsuarioID == usuarioID select c.PerfilID).ToList<long>();
            return (from c in Context.Vw_UsuariosPerfiles where (perfiles.Contains(c.PerfilID.Value) && usuarioID == c.UsuarioID) select c).OrderBy("Perfil_esES").ToList<Vw_UsuariosPerfiles>();
        }

        /// <summary>
        /// Obtener Los Perfiles de un Usuario
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        public List<long> perfilesAsignadosIDs(long usuarioID)
        {
            List<long> perfiles;
            perfiles = (from c in Context.UsuariosPerfiles where c.UsuarioID == usuarioID select c.PerfilID).ToList<long>();
            return perfiles;
        }

        public List<long?> modulosByPerfilesAsignadosIDs(long usuarioID)
        {
            List<long?> proyectosTiposID;
            try
            {
                proyectosTiposID = (from urserRol in Context.UsuariosRoles
                                    join rol in Context.Roles on urserRol.RolID equals rol.RolID
                                    join rolPer in Context.RolesPerfiles on urserRol.RolID equals rolPer.RolID
                                    join per in Context.Perfiles on rolPer.PerfilID equals per.PerfilID
                                    where
                                        urserRol.UsuarioID == usuarioID &&
                                        rol.Activo &&
                                        per.Activo
                                    select per.TipoProyectoID).Distinct().ToList<long?>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectosTiposID = new List<long?>();
            }

            return proyectosTiposID;
        }

        public List<string> modulosByRolesAsignados(long usuarioID)
        {
            List<string> proyectosTiposID;
            try
            {
                proyectosTiposID = (from urserRol in Context.UsuariosRoles
                                    join rol in Context.Roles on urserRol.RolID equals rol.RolID
                                    join rolPer in Context.RolesPerfiles on urserRol.RolID equals rolPer.RolID
                                    join per in Context.Perfiles on rolPer.PerfilID equals per.PerfilID
                                    join proyectoTipo in Context.ProyectosTipos on per.TipoProyectoID equals proyectoTipo.ProyectoTipoID
                                    where
                                        urserRol.UsuarioID == usuarioID &&
                                        rol.Activo &&
                                        per.Activo
                                    select proyectoTipo.ProyectoTipo).Distinct().ToList<string>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectosTiposID = new List<string>();
            }

            return proyectosTiposID;
        }

        public List<long?> getModulosIdByPerfilesUsuario(long usuarioID)
        {
            List<long?> modulosID = new List<long?>();

            modulosID = (from c in Context.Vw_UsuariosPerfiles
                         where c.UsuarioID == usuarioID
                         select c.TipoProyectoID).Distinct().ToList<long?>();

            return modulosID;
        }

        public List<Usuarios> ObtenerUsuariosPorPerfil(string perfil)
        {
            List<Usuarios> datos;

            List<long> usuarios = (from c in Context.UsuariosPerfiles where c.PerfilID == Int64.Parse(perfil) select c.UsuarioID).Distinct<long>().ToList<long>();
            datos = (from a in Context.Usuarios where usuarios.Contains(a.UsuarioID) select a).ToList();

            return datos;
        }

        public List<Usuarios> ObtenerUsuariosPorPerfilID(long perfil)
        {
            List<Usuarios> datos = null;
            try
            {
                List<long> usuarios = (from c in Context.UsuariosPerfiles where c.PerfilID == perfil select c.UsuarioID).Distinct<long>().ToList<long>();
                datos = (from a in Context.Usuarios where usuarios.Contains(a.UsuarioID) select a).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = new List<Usuarios>();
            }

            return datos;
        }

        /// <summary>
        /// Devuelve true si el Usuario pertence al PerfilIndicado
        /// </summary>
        /// <param name="perfil"></param>
        /// <returns></returns>
        public bool EsdelPerfil(string perfil, long UsuarioID)
        {
            bool res = false;
            List<long> usuarios = (from c in Context.UsuariosPerfiles where c.PerfilID == Int64.Parse(perfil) select c.UsuarioID).Distinct<long>().ToList<long>();
            if ((usuarios.Count > 0) && (usuarios.Contains(UsuarioID)))
            {
                res = true;
            }
            return res;
        }

        public bool EsdelPerfilByName(string perfil, long UsuarioID)
        {
            bool res = false;
            List<long> usuarios = (from c in Context.Vw_UsuariosPerfiles where (c.Perfil_esES == perfil && c.UsuarioID == UsuarioID) select c.UsuarioID).Distinct<long>().ToList<long>();
            if ((usuarios.Count > 0) && (usuarios.Contains(UsuarioID)))
            {
                res = true;
            }
            return res;
        }

        public List<long> UsuariosModulo(long ModuloID)
        {
            List<long> perfiles;
            perfiles = (from c in Context.Perfiles where c.TipoProyectoID == ModuloID select c.PerfilID).ToList<long>();
            return (from c in Context.Vw_UsuariosPerfiles where perfiles.Contains(c.PerfilID.Value) select c.UsuarioID).Distinct().ToList();
        }

        public List<long> UsuarioperfilesAsignadosIDs(long usuarioID)
        {
            List<long> perfiles = new List<long>();
            perfiles = (from c in Context.UsuariosPerfiles where c.UsuarioID == usuarioID select c.UsuarioPerfilID).ToList<long>();
            return perfiles;
        }

        public List<long> UsuariosModuloySuperUsuarios(long ModuloID)
        {
            List<long> perfiles;
            perfiles = (from c in Context.Perfiles where (c.TipoProyectoID == ModuloID || c.TipoProyectoID == null) select c.PerfilID).ToList<long>();
            return (from c in Context.Vw_UsuariosPerfiles where perfiles.Contains(c.PerfilID.Value) select c.UsuarioID).Distinct().ToList();
        }

        /// <summary>
        /// Obtiene los identificadores de todos los usuarios que están asociados al rol que se recibe como parámetro.
        /// </summary>
        /// <param name="perfilID">Identificador del perfil del que obtener los usuarios</param>
        /// <returns>Lista de Identificadores de los usuarios asociados al perfil.</returns>
        /// <remarks>MDS</remarks>
        public List<long> ObtenerUsuariosPorPerfil(long perfilID)
        {
            List<long> datos;

            datos = (from c in Context.UsuariosPerfiles where c.PerfilID == perfilID select c.UsuarioID).Distinct<long>().ToList<long>();

            return datos;
        }

        public List<long> getRolesAsignadosByUsuario(long lUsuarioID)
        {
            List<long> listaIDs;

            try
            {
                listaIDs = (from c in Context.UsuariosRoles where c.UsuarioID == lUsuarioID select c.UsuarioRolID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }

            return listaIDs;
        }

        #endregion
    }
}
