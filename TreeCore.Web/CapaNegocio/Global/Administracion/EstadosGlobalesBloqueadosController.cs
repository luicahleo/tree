using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public class EstadosGlobalesBloqueadosController : GeneralBaseController<EstadosGlobalesBloqueados, TreeCoreContext>, IGestionBasica<EstadosGlobalesBloqueados>
    {
        public EstadosGlobalesBloqueadosController()
            : base()
        {

        }

        public InfoResponse Add(EstadosGlobalesBloqueados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = AddEntity(oEntidad);
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

        public InfoResponse Update(EstadosGlobalesBloqueados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(EstadosGlobalesBloqueados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = DeleteEntity(oEntidad);
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
        /// Obtiene la lista de tipos de proyectos que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <remarks>Mds</remarks>

        public List<ProyectosTipos> proyectosTiposNoAsignado(long estadoID)
        {
            List<long> tipos;
            tipos = (from c in Context.EstadosGlobalesBloqueados where c.EstadoGlobalID == estadoID select c.ProyectoTipoID).ToList<long>();

            EstadosGlobalesController cEstadoGlobal = new EstadosGlobalesController();
            Data.EstadosGlobales estado = new EstadosGlobales();
            estado = cEstadoGlobal.GetItem(estadoID);


            return (from c in Context.ProyectosTipos where (!(tipos.Contains(c.ProyectoTipoID))) select c).ToList<ProyectosTipos>();


        }



        /// <summary>
        /// Obtiene los ProyectosTipos asignados a un usuario.
        /// </summary>

        public List<Vw_EstadosGlobalesBloqueados> proyectosTiposAsignados(long estadoID)
        {
            List<long> tipos;
            tipos = (from c in Context.EstadosGlobalesBloqueados where c.EstadoGlobalID == estadoID select c.ProyectoTipoID).ToList<long>();
            return (from c in Context.Vw_EstadosGlobalesBloqueados where (tipos.Contains(c.ProyectoTipoID) && estadoID == c.EstadoGlobalID) select c).ToList<Vw_EstadosGlobalesBloqueados>();
        }

        /// <summary>
        /// Obtener Los ProyectosTipos de un EstadoGlobal
        /// </summary>
        /// <param name="estadoID"></param>
        /// <returns></returns>
        public List<long> proyectosTiposAsignadosIDs(long estadoID)
        {
            List<long> tipos = new List<long>();
            tipos = (from c in Context.EstadosGlobalesBloqueados where c.EstadoGlobalID == estadoID select c.ProyectoTipoID).ToList<long>();
            return tipos;
        }

        public List<EstadosGlobales> ObtenerEstadosGlobalesPorProyectoTipo(string sProyectoTipo)
        {
            List<EstadosGlobales> datos;

            List<long> lista = (from c in Context.EstadosGlobalesBloqueados where c.ProyectoTipoID == Int64.Parse(sProyectoTipo) select c.EstadoGlobalID).Distinct<long>().ToList<long>();
            datos = (from a in Context.EstadosGlobales where lista.Contains(a.EstadoGlobalID) select a).ToList();

            return datos;
        }


        public List<EstadosGlobales> ObtenerEstadosGlobalesPorProyectoTipoID(long proyectoTipoID)
        {
            List<EstadosGlobales> datos = null;
            try
            {
                List<long> lista = (from c in Context.EstadosGlobalesBloqueados where c.ProyectoTipoID == proyectoTipoID select c.EstadoGlobalID).Distinct<long>().ToList<long>();
                datos = (from a in Context.EstadosGlobales where lista.Contains(a.EstadoGlobalID) select a).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = new List<EstadosGlobales>();
            }

            return datos;
        }

        /// <summary>
        /// Devuelve true si el EstadoGlobal pertence al ProyectoTipoIndicado
        /// </summary>
        /// <param name="proyectoTipo"></param>
        /// <returns></returns>
        public bool EsdelProyectoTipo(string proyectoTipo, long EstadoGlobalID)
        {
            bool res = false;
            List<long> lista = (from c in Context.EstadosGlobalesBloqueados where c.ProyectoTipoID == Int64.Parse(proyectoTipo) select c.EstadoGlobalID).Distinct<long>().ToList<long>();
            if ((lista.Count > 0) && (lista.Contains(EstadoGlobalID)))
            {
                res = true;
            }
            return res;
        }
        public bool EsdelProyectoTipoByName(string proyectoTipo, long EstadoGlobalID)
        {
            bool res = false;
            List<long> lista = (from c in Context.Vw_EstadosGlobalesBloqueados where (c.ProyectoTipo == proyectoTipo && c.EstadoGlobalID == EstadoGlobalID) select c.EstadoGlobalID).Distinct<long>().ToList<long>();
            if ((lista.Count > 0) && (lista.Contains(EstadoGlobalID)))
            {
                res = true;
            }
            return res;
        }



        /// <summary>
        /// Obtiene los identificadores de todos los usuarios que están asociados al rol que se recibe como parámetro.
        /// </summary>
        /// <param name="proyectoTipoID">Identificador del proyecto tipo del que obtener los estados globales</param>
        /// <returns>Lista de Identificadores de los usuarios asociados al perfil.</returns>
        /// <remarks>MDS</remarks>
        public List<long> ObtenerEstadosGlobalesPorProyectoTipo(long proyectoTipoID)
        {
            List<long> datos;

            datos = (from c in Context.EstadosGlobalesBloqueados where c.ProyectoTipoID == proyectoTipoID select c.EstadoGlobalID).Distinct<long>().ToList<long>();

            return datos;
        }

    }
    #endregion
}