using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GruposAccesosWebUsuariosController : GeneralBaseController<GruposAccesosWebUsuarios, TreeCoreContext>
    {
        public GruposAccesosWebUsuariosController()
            : base()
        { }
        public List<Usuarios> UsuariosAsignados(long idGrupoAcceso, long idCliente)
        {
            List<long> usuarios;
            usuarios = (from c in Context.GruposAccesosWebUsuarios where c.GrupoAccesoWebID == idGrupoAcceso select c.UsuarioID).ToList<long>();
            return (from c in Context.Usuarios where (!(usuarios.Contains(c.UsuarioID)) && c.Activo && c.ClienteID == idCliente) select c).ToList<Usuarios>();
        }
        public List<Vw_GruposAccesoWebUsuarios> getUsuariosGrupoAccesoWeb(long idGrupoAcceso)
        {
            List<Vw_GruposAccesoWebUsuarios> lista = new List<Vw_GruposAccesoWebUsuarios>();
            lista = (from c in Context.Vw_GruposAccesoWebUsuarios where idGrupoAcceso == c.GrupoAccesoWebID select c).ToList<Vw_GruposAccesoWebUsuarios>();
            return lista;
        }
        public GruposAccesosWebUsuarios getRegistro(long idUsuario, long idGrupoAcceso)
        {
            List<GruposAccesosWebUsuarios> usuarios;
            usuarios = (from c in Context.GruposAccesosWebUsuarios where (c.GrupoAccesoWebID == idGrupoAcceso && c.UsuarioID == idUsuario) select c).ToList<GruposAccesosWebUsuarios>();
            if (usuarios.Count == 1)
            {
                return usuarios.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public List<long> getGrupoAccesoWebID(List<GruposAccesosWebUsuarios> listaDatos)
        {
            List<long> listaID;

            try
            {
                listaID = (from c in listaDatos select c.GrupoAccesoWebID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaID = null;
            }

            return listaID;
        }

    }
}