using System;
using System.Collections.Generic;
using System.Data;
using TreeCore.Data;
using System.Linq;

namespace CapaNegocio
{
    public class GruposAccesosWebRolesController : GeneralBaseController<GruposAccesosWebRoles, TreeCoreContext>
    {
        public GruposAccesosWebRolesController()
            : base()
        { }

        public GruposAccesosWebRoles getAccesoWebByRol(long lWebID, long lRolID)
        {
            GruposAccesosWebRoles oDato;

            try
            {
                oDato = (from c in Context.GruposAccesosWebRoles where c.RolID == lRolID && c.GrupoAccesoWebID == lWebID select c).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }
        public bool RegistroDuplicado(long RolID, long lGrupoAccesoID)
        {
            bool isExiste = false;
            List<GruposAccesosWebRoles> datos;

            datos = (from c in Context.GruposAccesosWebRoles where (c.RolID == RolID && c.GrupoAccesoWebID == lGrupoAccesoID) select c).ToList<GruposAccesosWebRoles>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<long> getRolByGrupoAcceso(long lGrupo)
        {
            List<long> listaRolID;
            string sGrupo;
            long lID;

            try
            {
                sGrupo = (from c in Context.Vw_GruposAccesosWebRoles where c.GrupoAccesoWebRolID == lGrupo select c.GrupoAcceso).First();
                lID = (from c in Context.GruposAccesosWeb where c.GrupoAcceso == sGrupo select c.GrupoAccesoWebID).First();
                listaRolID = (from c in Context.GruposAccesosWebRoles where c.GrupoAccesoWebID == lID select c.RolID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaRolID = null;
            }

            return listaRolID;
        }
        public List<GruposAccesosWebRoles> getGruposAccesoByRolID(long rolID)
        {
            List<GruposAccesosWebRoles> listaRolID;

            try
            {
                listaRolID = (from c in Context.GruposAccesosWebRoles where c.RolID == rolID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaRolID = null;
            }

            return listaRolID;
        }

        public List<GruposAccesosWebRoles> getGruposAccesoByUsuarioID (long lUsuarioID)
        {
            List<GruposAccesosWebRoles> listaGruposAccesosWebRoles;
            List<long> listaRoles;

            try
            {
                listaRoles = (from c in Context.UsuariosRoles where c.UsuarioID == lUsuarioID select c.RolID).ToList();
                listaGruposAccesosWebRoles = (from c in Context.GruposAccesosWebRoles where listaRoles.Contains(c.RolID) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaGruposAccesosWebRoles = null;
            }

            return listaGruposAccesosWebRoles;
        }

        public List<long> getGrupoAccesoWebID(List<GruposAccesosWebRoles> listaDatos)
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

        public Vw_GruposAccesosWebRoles getVistaByGrupoAcceso (string sGrupo)
        {
            Vw_GruposAccesosWebRoles oDato;

            try
            {
                oDato = (from c in Context.Vw_GruposAccesosWebRoles where c.GrupoAcceso == sGrupo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public List<long> getIDsByGrupo (long lGrupoID)
        {
            List<long> listaIDs;

            try
            {
                listaIDs = (from c in Context.GruposAccesosWebRoles where c.GrupoAccesoWebID == lGrupoID select c.GrupoAccesoWebRolID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }

            return listaIDs;
        }

        public List<long> getListaByGrupoAcceso(long lGrupo)
        {
            List<long> listaDatos;

            try
            {
                listaDatos = (from c in Context.GruposAccesosWebRoles where c.GrupoAccesoWebID == lGrupo select c.RolID).ToList();
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