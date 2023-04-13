using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public sealed class UsuariosPerfilesController : GeneralBaseController<UsuariosPerfiles, TreeCoreContext>
    {
        public UsuariosPerfilesController()
            : base()
        {

        }
        #region GLOBAL
        /// <summary>
        /// Obtiene la lista de Perfiles que aún no tiene asignado el usuario que se le pasa como parámetro
        /// </summary>
        /// <remarks>Mds</remarks>

        public List<Perfiles> perfilesNoAsignado(long usuarioID, long clienteID)
        {
            List<long> perfiles;
            perfiles = (from c in Context.UsuariosPerfiles where c.UsuarioID == usuarioID select c.PerfilID).ToList<long>();

            //UsuariosController cUsuario = new UsuariosController();
            //Usuarios usuario = new Usuarios();
            //usuario = cUsuario.GetItem(usuarioID);
           
            return (from c in Context.Perfiles where (!(perfiles.Contains(c.PerfilID))) && c.ClienteID == clienteID && c.Activo == true select c).OrderBy("Perfil_esES").ToList<Perfiles>();
            

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
            List<long?> proyectosTiposID = new List<long?>();
            proyectosTiposID = (from c in Context.Vw_UsuariosPerfiles where c.UsuarioID == usuarioID select c.TipoProyectoID).Distinct().ToList<long?>();
            return proyectosTiposID;
        }

        public List<long?> getModulosIdByPerfilesUsuario(long usuarioID)
        {
            List<long?> modulosID = new List<long?>();

            modulosID = (from c in Context.Vw_UsuariosPerfiles
                         where c.UsuarioID == usuarioID select c.TipoProyectoID).Distinct().ToList<long?>();
            
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

        #endregion
    }
}
