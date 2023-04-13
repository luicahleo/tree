using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class PerfilesController : GeneralBaseController<Perfiles, TreeCoreContext>
    {
        public PerfilesController()
            : base()
        { }

        public List<Vw_PerfilesUsuario> GetAllPerfilesByUsuario(long lUsuario)
        {
            List<Vw_PerfilesUsuario> lPerfiles = new List<Vw_PerfilesUsuario>();

            lPerfiles = (from c in Context.Vw_PerfilesUsuario where c.UsuarioID == lUsuario orderby c.UsuarioPerfilID select c).ToList();


            return lPerfiles;
        }
        public List<Perfiles> GetAllPerfilesByTipoProyectoID(long tipoID)
        {
            List<Perfiles> perfiles = new List<Perfiles>();
            perfiles = (from c in Context.Perfiles where c.TipoProyectoID == tipoID orderby c.Perfil_esES select c).ToList();
            return perfiles;
        }
        public List<Perfiles> GetActivos()
        {
            List<Perfiles> perfiles = new List<Perfiles>();
            perfiles = (from c in Context.Perfiles orderby c.Perfil_esES select c).ToList();
            return perfiles;
        }
        public List<Perfiles> GetActivos(long? lClienteID)
        {
            List<Perfiles> perfiles = new List<Perfiles>();
            perfiles = (from c in Context.Perfiles where c.ClienteID == lClienteID orderby c.Perfil_esES select c).ToList();
            return perfiles;
        }
        public List<Perfiles> GetActivosByProyectoTipo(long? lClienteID, long lProyectoTipoID)
        {
            List<Perfiles> perfiles = new List<Perfiles>();
            perfiles = (from c in Context.Perfiles where c.ClienteID == lClienteID && c.TipoProyectoID == lProyectoTipoID orderby c.Perfil_esES select c).ToList();
            return perfiles;
        }

        public List<Perfiles> GetPerfilesLibresInventarioAtributosRestringidos(long atributoID)
        {
            List<Perfiles> perfiles;
            try
            {
                List<long> asignados = (from c in Context.InventarioAtributosPerfilesRestringidos where c.InventariosAtributoID == atributoID && c.PerfilID != null select c.PerfilID.Value).ToList();
                perfiles = (from c in Context.Perfiles where !(asignados.Contains(c.PerfilID)) orderby c.Perfil_esES select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }
            return perfiles;
        }

        public List<Perfiles> GetPerfilesLibresEmplazamientosAtributosRestringidos(long atributoID)
        {
            List<Perfiles> perfiles;
            try
            {
                List<long> asignados = (from c in Context.EmplazamientosAtributosPerfilesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.PerfilID != null select c.PerfilID.Value).ToList();
                perfiles = (from c in Context.Perfiles where !(asignados.Contains(c.PerfilID)) orderby c.Perfil_esES select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }
            return perfiles;
        }

        public List<Perfiles> GetPerfilesByRol(long rolID)
        {
            List<Perfiles> perfiles;
            try
            {
                List<long> asignados = (from c in Context.RolesPerfiles where c.RolID == rolID select c.PerfilID).ToList();
                perfiles = (from c in Context.Perfiles where asignados.Contains(c.PerfilID) orderby c.Perfil_esES select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }
            return perfiles;
        }

        public List<Vw_Perfiles> GetPerfilesByTipoProyectoID(long tipoProyectoID)
        {
            return (from c in Context.Vw_Perfiles where c.TipoProyectoID == tipoProyectoID select c).ToList();
        }
        public List<long?> GetProyectosTiposByPerfilesUsuario(long lUsuarioID)
        {
            List<long?> lTipoID = new List<long?>();
            lTipoID = (from c in Context.Vw_PerfilesUsuario where c.UsuarioID == lUsuarioID orderby c.TipoProyectoID select c.TipoProyectoID).Distinct().ToList();
            return lTipoID;
        }


        public long GetPerfilIDByPerfilES(string perfil_esES)
        {
            return (from c in Context.Perfiles where c.Perfil_esES == perfil_esES select c.PerfilID).FirstOrDefault();
        }

        public bool RegistroDuplicado(string Perfil, long clienteID)
        {
            bool isExiste = false;
            List<Perfiles> datos;

            datos = (from c in Context.Perfiles where (c.Perfil_esES == Perfil && c.ClienteID == clienteID) select c).ToList<Perfiles>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}