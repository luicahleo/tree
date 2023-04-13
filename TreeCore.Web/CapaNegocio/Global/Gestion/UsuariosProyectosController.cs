using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class UsuariosProyectosController : GeneralBaseController<UsuariosProyectos, TreeCoreContext>, IGestionBasica<UsuariosProyectos>
    {
        public UsuariosProyectosController()
            : base() { }

        public InfoResponse Add(UsuariosProyectos oUser)
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

        public InfoResponse Update(UsuariosProyectos oUser)
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

        public InfoResponse Delete(UsuariosProyectos oUser)
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

        public List<UsuariosProyectos> GetUsuarioID (long lUsuarioID)
        {
            List<UsuariosProyectos> listaUsuarios;

            try
            {
                listaUsuarios = (from c in Context.UsuariosProyectos where c.UsuarioID == lUsuarioID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }

            return listaUsuarios;
        }

        public List<Usuarios> ProyectosUsuariosNoVinculados(long proyectoID)
        {
            List<Usuarios> listaUsuarios;
            List<long> listaVinculados;
            List<long> listaEntidades;

            try
            {
                List<long> tipos;
                tipos = (from c in Context.ProyectosEmpresasProveedoras where c.ProyectoID == proyectoID select c.EmpresaProveedoraID).ToList<long>();
                listaEntidades = (from c in Context.Entidades where (tipos.Contains((long)c.EmpresaProveedoraID) && c.EmpresaProveedoraID != null) || c.EntidadCliente  select c.EntidadID).ToList<long>();

                listaVinculados = (from c in Context.UsuariosProyectos where c.ProyectoID == proyectoID select c.UsuarioID).ToList();
                listaUsuarios = (from c in Context.Usuarios where !listaVinculados.Contains(c.UsuarioID) && listaEntidades.Contains((long)c.EntidadID) select c).ToList<Usuarios>();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }

            return listaUsuarios;
        }

        public List<Vw_ProyectosUsuarios> ProyectosUsuarios()
        {
            List<Vw_ProyectosUsuarios> listaUsuarios;

            try
            {
                listaUsuarios = (from c in Context.Vw_ProyectosUsuarios where c.ProyectoID != null select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarios = null;
            }

            return listaUsuarios;
        }

        public UsuariosProyectos GetUsuarioProyectoByIDs(long proyectoID, long usuarioID)
        {
            UsuariosProyectos oDato;

            try
            {
                oDato = (from c in Context.UsuariosProyectos where c.UsuarioID == usuarioID && c.ProyectoID == proyectoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public bool controlDuplicidad(long proyectoID, long usuarioID)
        {
            List<UsuariosProyectos> oDato;
            bool control = false;
            try
            {
                oDato = (from c in Context.UsuariosProyectos where c.UsuarioID == usuarioID && c.ProyectoID == proyectoID select c).ToList();
                if (oDato.Count > 0)
                {
                    control = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return control;

        }
    }
}