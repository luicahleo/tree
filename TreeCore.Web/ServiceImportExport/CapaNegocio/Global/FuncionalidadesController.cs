using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class FuncionalidadesController : GeneralBaseController<Funcionalidades, TreeCoreContext>
    {
        public FuncionalidadesController()
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
        public bool RegistroDuplicado(long codigo)
        {
            bool isExiste = false;
            List<Funcionalidades> datos = new List<Funcionalidades>();


            datos = (from c in Context.Funcionalidades where (c.Codigo == codigo) select c).ToList<Funcionalidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoNombre(string nombre)
        {
            bool isExiste = false;
            List<Funcionalidades> datos = new List<Funcionalidades>();


            datos = (from c in Context.Funcionalidades where (c.Funcionalidad == nombre) select c).ToList<Funcionalidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Vw_Funcionalidades> GetFuncionalidadesByModulo(long ModuloID)
        {
            List<Vw_Funcionalidades> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_Funcionalidades where c.ModuloID == ModuloID && c.Activo && c.Alias != "" select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Vw_Funcionalidades> GetFuncionalidadesByModuloAll(long ModuloID)
        {
            List<Vw_Funcionalidades> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_Funcionalidades where c.ModuloID == ModuloID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<long> GetFuncionalidadesIDsPefilByModulo(long ModuloID, long PerfilID)
        {
            List<long> listaDatos;
            List<long> FuncionalidadesAsociadas;
            try
            {
                FuncionalidadesAsociadas = (from c in Context.PerfilesFuncionalidades where c.PerfilID == PerfilID select c.FuncionalidadID).ToList();
                listaDatos = (from c in Context.Funcionalidades where FuncionalidadesAsociadas.Contains(c.FuncionalidadID) && c.ModuloID == ModuloID select c.FuncionalidadID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Funcionalidades> GetFunciolidadesByTipo(long lProyectoTipoID, long lTipoFunID)
        {
            List<long> listaIDsModulos = new List<long>();
            List<Funcionalidades> listaDatos;
            try
            {
                if (Produccion)
                {
                    listaIDsModulos = (from c in Context.Modulos where c.ProyectoTipoID == lProyectoTipoID && c.Activo && c.SuperUser == false && c.Produccion == true select c.ModuloID).ToList();
                }
                else
                {
                    listaIDsModulos = (from c in Context.Modulos where c.ProyectoTipoID == lProyectoTipoID && c.Activo && c.SuperUser == false && c.Produccion == true && c.SuperUser == false select c.ModuloID).ToList();
                }

                listaDatos = (from c in Context.Funcionalidades where listaIDsModulos.Contains(c.ModuloID) && c.Activo && c.FuncionalidadTipoID == lTipoFunID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        #region FUNCIONALIDADES (NO) ASIGNADAS

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un rol.
        /// </summary>
        /// <param name="perfilaID">Perfil del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_PerfilesFuncionalidades> funcionalidadesAsignadas(long perfilID)
        {
            List<long> funcionalidades;
            funcionalidades = (from c in Context.PerfilesFuncionalidades where c.PerfilID == perfilID select c.FuncionalidadID).ToList<long>();
            return (from c in Context.Vw_PerfilesFuncionalidades where (funcionalidades.Contains(c.FuncionalidadID) && perfilID == c.PerfilID) select c).OrderBy("Modulo").ToList<Vw_PerfilesFuncionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el perfil que se le pasa como parámetro
        /// </summary>
        /// <param name="perfilID">Identificador del perfil del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignada(long perfilID)
        { //TRegionesPaises
            List<long> funcionalidades;
            funcionalidades = (from c in Context.PerfilesFuncionalidades where c.PerfilID == perfilID select c.FuncionalidadID).ToList<long>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region ESTADOS ADQUISICIONES
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstados(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AdquisicionesSARFEstados where c.AdquisicionSARFEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstados(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AdquisicionesSARFEstados where c.AdquisicionSARFEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosEmplazamientos(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AdquisicionesEmplazamientosEstados where c.AdquisicionEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosEmplazamientosAmpliacion(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AmpliacionesEmplazamientosEstados where c.AmpliacionEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }


        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosSARFEmplazamientos(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AdquisicionesSARFEmplazamientosEstados where c.AdquisicionSARFEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }



        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado candidato que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado candidato del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosSARFEmplazamientos(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AdquisicionesSARFEmplazamientosEstados where c.AdquisicionSARFEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosEmplazamientosAmpliacion(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AmpliacionesEmplazamientosEstados where c.AmpliacionEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosEmplazamientosAccess(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AccessEmplazamientosEstados where c.AccessEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region ESTADOS AMBIENTAL
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosAmbiental(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AmbientalEstados where c.AmbientalEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadasEstadosAmbiental(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AmbientalEstados where c.AmbientalEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS TOWER
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato para Tower SARF.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosSARFEmplazamientosTower(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosSARFEmplazamientosEstados where c.TorreroSARFEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato para Tower Emplazamiento.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosEmplazamientosTower(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosEmplazamientosEstados where c.TorreroEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato para Access Emplazamiento.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosEmplazamientosAccess(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AccessEmplazamientosEstados where c.AccessEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato para Access Emplazamiento.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasPropertyIncidenciasEstados(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.PropertyIncidenciasEstados where c.PropertyIncidenciaEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato para Tower Colocalizaciones.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosTowerColocalizaciones(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosEmplazamientosEstados where c.Colocalizacion && c.TorreroEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();

        }

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado Candidato para Tower Emplazamiento.
        /// </summary>
        /// <param name="EstadoID">Estado candidato del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosSARFTower(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosSARFEstados where c.TorreroSARFEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado candidato que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado candidato del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosTowerColocalizaciones(long estadoID)
        {
            //descomentar y hacer la consulta cuando la tabla TorrerosColocalizacionesEstados este creada 
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosEmplazamientosEstados where c.Colocalizacion && c.TorreroEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosEmplazamientos(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AdquisicionesEmplazamientosEstados where c.AdquisicionEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }


        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadesNoAsignadasPropertyIncidenciasEstados(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.PropertyIncidenciasEstados where c.PropertyIncidenciaEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadesNoAsignadasEstadosEmplazamientosTower(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosEmplazamientosEstados where c.TorreroEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadesNoAsignadasEstadosSARFTower(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.TorrerosSARFEstados where c.TorreroSARFEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }


        #endregion

        #region ESTADOS SPACE
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosEmplazamientosSpace(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SpaceEmplazamientosEstados where c.SpaceEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        public Vw_Funcionalidades FuncionalidadesAsignadasSpace(long lEstadoID)
        {
            long? lFunc;
            Vw_Funcionalidades vFunc = null;

            lFunc = (from c in Context.SpaceEmplazamientosEstados where c.SpaceEmplazamientoEstadoID == lEstadoID select c.FuncionalidadID).First();

            if (lFunc != null)
            {
                vFunc = (from c in Context.Vw_Funcionalidades where c.FuncionalidadID == lFunc select c).First();
            }
            else
            {
                vFunc = null;
            }
            return vFunc;
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosEmplazamientosSpace(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SpaceEmplazamientosEstados where c.SpaceEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS AUDIT
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosAudit(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AuditEmplazamientosEstados where c.AuditEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosAudit(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AuditEmplazamientosEstados where c.AuditEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS FIRMA DIGITAL
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosFirmaDigital(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.FirmaDigitalEstados where c.FirmaDigitalEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosFirmaDigital(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.FirmaDigitalEstados where c.FirmaDigitalEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS ASSETS PURCHASE
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosAssetsPurchase(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AssetsPurchaseEmplazamientosEstados where c.AssetPurchaseEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosAssetsPurchase(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.AssetsPurchaseEmplazamientosEstados where c.AssetPurchaseEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS DOCUMENTAL
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosDocument(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.DocumentEstados where c.DocumentEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosDocument(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.DocumentEstados where c.DocumentEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }


        #endregion

        #region ESTADOS SAVING
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosSaving(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SavingEmplazamientosEstados where c.SavingEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosSaving(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SavingEmplazamientosEstados where c.SavingEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS SHARING
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosSharing(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SharingEmplazamientosEstados where c.SharingEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosSharing(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SharingEmplazamientosEstados where c.SharingEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS LEGAL
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosLegal(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.LegalEmplazamientosEstados where c.LegalEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosLegal(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.LegalEmplazamientosEstados where c.LegalEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS ENERGY
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosEnergy(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.EnergyEmplazamientosEstados where c.EnergyEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosEnergy(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.EnergyEmplazamientosEstados where c.EnergyEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS INSTALL
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosInstallObraCivil(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.InstallObraCivilEmplazamientosEstados where c.InstallObraCivilEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosInstallObraCivil(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.InstallObraCivilEmplazamientosEstados where c.InstallObraCivilEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosInstallTecnica(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.InstallTecnicaEmplazamientosEstados where c.InstallTecnicaEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosInstallTecnica(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.InstallTecnicaEmplazamientosEstados where c.InstallTecnicaEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS UNINSTALL
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosUninstallAdmin(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.UninstallAdminEmplazamientosEstados where c.UninstallAdminEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosUninstallAdmin(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.UninstallAdminEmplazamientosEstados where c.UninstallAdminEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosUninstallElectrica(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.UninstallElectricaEmplazamientosEstados where c.UninstallElectricaEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosUninstallElectrica(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.UninstallElectricaEmplazamientosEstados where c.UninstallElectricaEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosUninstallTecnica(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.UninstallTecnicaEmplazamientosEstados where c.UninstallTecnicaEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosUninstallTecnica(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.UninstallTecnicaEmplazamientosEstados where c.UninstallTecnicaEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region MANTENIMIENTO

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosMantenimiento(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosEstados where c.MantenimientoEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosMantenimiento(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosEstados where c.MantenimientoEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region MANTENIMIENTO CORRECTIVO

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosMantenimientoCorrectivo(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosCorrectivosEstados where c.MantenimientoEmplazamientoCorrectivoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosMantenimientoCorrectivo(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosCorrectivosEstados where c.MantenimientoEmplazamientoCorrectivoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region MANTENIMIENTO TP

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosMantenimientoTP(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosTPEstados where c.MantenimientoEmplazamientoTPEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosMantenimientoTP(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosTPEstados where c.MantenimientoEmplazamientoTPEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region MANTENIMIENTO SBD

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosMantenimientoSBD(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosSBDEstados where c.MantenimientoEmplazamientoSBDEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosMantenimientoSBD(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.MantenimientoEmplazamientosSBDEstados where c.MantenimientoEmplazamientoSBDEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion

        #region ESTADOS FINANCIERO ALQUILERES
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosFinancieros(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.FinancieroAlquileresEstados where c.FinancieroAlquilerEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosFinancieros(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.FinancieroAlquileresEstados where c.FinancieroAlquilerEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS_DESPLIEGUE
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosDespliegue(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.DesplieguesEmplazamientosEstados where c.DespliegueEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadasEstadosDespliegue(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.DesplieguesEmplazamientosEstados where c.DespliegueEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        public List<Vw_Funcionalidades> funcionalidadNoAsignadasEstadosDBIDespliegue(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.DesplieguesSARFEstados where c.DespliegueSARFEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosDBIDespliegue(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.DesplieguesSARFEstados where c.DespliegueSARFEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS_SSRR
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosSSRR(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SSRREmplazamientosEstados where c.SSRREmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosSSRR(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.SSRREmplazamientosEstados where c.SSRREmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region ESTADOS_PLANNING
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosPLANNING(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.PlanningPlanificacionesEstados where c.PlanningPlanificacionEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosPLANNING(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.PlanningPlanificacionesEstados where c.PlanningPlanificacionEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region FUNCIONALIDADES USUARIOS
        /// <summary>
        /// Obtiene las funcionalidades del usuario que se loguea
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        /// <remarks>MDS</remarks>
        public List<long> getFuncionalidades(long usuarioID)
        {

            List<long> funcionalidades;
            try
            {
                funcionalidades = (from userRol in Context.UsuariosRoles
                                   join rol in Context.Roles on userRol.RolID equals rol.RolID
                                   join rolPerfil in Context.RolesPerfiles on rol.RolID equals rolPerfil.RolID
                                   join perfFunc in Context.PerfilesFuncionalidades on rolPerfil.PerfilID equals perfFunc.PerfilID
                                   join funcionalidad in Context.Funcionalidades on perfFunc.FuncionalidadID equals funcionalidad.FuncionalidadID
                                   join modulo in Context.Modulos on funcionalidad.ModuloID equals modulo.ModuloID
                                   where
                                      userRol.UsuarioID == usuarioID && rol.Activo && modulo.Activo && modulo.SuperUser == false
                                   select funcionalidad.Codigo).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                funcionalidades = new List<long>();
            }

            return funcionalidades;
        }

        public List<Vw_Funcionalidades> getListFuncionalidades(long usuarioID)
        {

            List<Vw_Funcionalidades> funcionalidades;
            try
            {
                funcionalidades = (from userRol in Context.UsuariosRoles
                                   join rol in Context.Roles on userRol.RolID equals rol.RolID
                                   join rolPerfil in Context.RolesPerfiles on rol.RolID equals rolPerfil.RolID
                                   join perfFunc in Context.PerfilesFuncionalidades on rolPerfil.PerfilID equals perfFunc.PerfilID
                                   join funcionalidad in Context.Vw_Funcionalidades on perfFunc.FuncionalidadID equals funcionalidad.FuncionalidadID
                                   join modulo in Context.Modulos on funcionalidad.ModuloID equals modulo.ModuloID
                                   where
                                      userRol.UsuarioID == usuarioID && rol.Activo && modulo.Activo && modulo.SuperUser == false
                                   select funcionalidad).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                funcionalidades = new List<Vw_Funcionalidades>();
            }

            return funcionalidades;
        }

        public List<long> getFuncionalidadesSuperUsuario(long UsuarioID)
        {
            UsuariosController cUser = new UsuariosController();
            List<long> funcionalidades = new List<long>();
            List<long> funcionalidadesTipos = new List<long>();

            try
            {
                Usuarios user = new Usuarios();
                user = cUser.GetItem(UsuarioID);

                if (user != null && user.EMail.Equals(Comun.TREE_SUPER_USER))
                {
                    funcionalidadesTipos = (from c in Context.FuncionalidadesTipos where c.Activo && (c.Total || c.Super || c.Otro || c.Exportar) select c.FuncionalidadTipoID).ToList();

                    if (funcionalidadesTipos != null && funcionalidadesTipos.Count > 0)
                    {
                        funcionalidades = (from c in Context.Funcionalidades where c.FuncionalidadTipoID != null && funcionalidadesTipos.Contains((long)c.FuncionalidadTipoID) select c.Codigo).ToList();
                    }
                }
                else
                {
                    funcionalidades = getFuncionalidades(UsuarioID);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                funcionalidades = new List<long>();
            }

            return funcionalidades;
        }
        
        public List<Vw_Funcionalidades> getListFuncionalidadesSuperUsuario(long UsuarioID)
        {
            UsuariosController cUser = new UsuariosController();
            List<Vw_Funcionalidades> funcionalidades = new List<Vw_Funcionalidades>();
            List<long> funcionalidadesTipos = new List<long>();

            try
            {
                Usuarios user = new Usuarios();
                user = cUser.GetItem(UsuarioID);

                if (user != null && user.EMail.Equals(Comun.TREE_SUPER_USER))
                {
                    funcionalidadesTipos = (from c in Context.FuncionalidadesTipos where c.Activo && (c.Total || c.Super || c.Otro || c.Exportar) select c.FuncionalidadTipoID).ToList();

                    if (funcionalidadesTipos != null && funcionalidadesTipos.Count > 0)
                    {
                        funcionalidades = (from c in Context.Vw_Funcionalidades /*where c.FuncionalidadTipoID != null && funcionalidadesTipos.Contains((long)c.FuncionalidadTipoID)*/ select c).ToList();
                    }
                }
                else
                {
                    funcionalidades = getListFuncionalidades(UsuarioID);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                funcionalidades = new List<Vw_Funcionalidades>();
            }

            return funcionalidades;
        }
        public long getidfuncionalidad(string funcionalidad)
        {

            long funcid;
            try
            {
                funcid = (from c in Context.Vw_Funcionalidades where c.Funcionalidad == funcionalidad select c.FuncionalidadID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                funcid = new long();
            }
            return funcid;
        }

        /// <summary>
        /// Comprueba si la funcionalidad que se le pasa como parámetro tiene registros asociados
        /// </summary>
        /// <param name="funcionalidadID">Identificador del elemento</param>
        /// <returns></returns>
        /// <remarks>Mds</remarks>
        public bool tieneRegistrosAsociado(long funcionalidadID)
        {
            bool tiene = false;
            PerfilesFuncionalidadesController cPerfilesFuncionalidades = new PerfilesFuncionalidadesController();
            List<PerfilesFuncionalidades> datos;
            datos = cPerfilesFuncionalidades.GetItemsList("FuncionalidadID == " + funcionalidadID.ToString());
            if (datos.Count > 0)
            {
                tiene = true;
            }
            return tiene;
        }
        public bool PerfilConFuncionalidad(long? funcionalidadID, List<Vw_UsuariosPerfiles> usuariosPerfiles, long usuarioID)
        {
            List<Vw_PerfilesFuncionalidades> perfiles;

            bool res = false;

            perfiles = (from c in Context.Vw_PerfilesFuncionalidades where c.FuncionalidadID == funcionalidadID select c).ToList<Vw_PerfilesFuncionalidades>(); ;
            if ((perfiles.Count > 0))
            {
                foreach (Vw_UsuariosPerfiles up in usuariosPerfiles)
                {
                    foreach (Vw_PerfilesFuncionalidades pf in perfiles)
                    {
                        if (pf.PerfilID == up.PerfilID)
                        {
                            res = true;
                        }
                    }
                }
            }
            return res;
        }

        public bool GetFuncionalidadByCodigo(long FuncionalidadID)
        {
            List<Funcionalidades> funcionalidades = null;
            bool res = false;

            funcionalidades = (from c in Context.Funcionalidades where c.Codigo == FuncionalidadID select c).ToList<Funcionalidades>();

            if (funcionalidades.Count > 0)
                res = true;

            return res;
        }
        #endregion

        #region ESTADOS INDOOR
        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosIndoor(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.IndoorEstados where c.IndoorEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosIndoor(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.IndoorEstados where c.IndoorEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }
        #endregion

        #region VANDALISMO

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosVandalismo(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.VandalismoEmplazamientosEstados where c.VandalismoEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosVandalismo(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.VandalismoEmplazamientosEstados where c.VandalismoEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion
        #region CITY

        /// <summary>
        /// Obtiene las funcionalidades asignadas a un Estado.
        /// </summary>
        /// <param name="EstadoID">Estado del que obtener las funcionalidades asignadas.</param>
        /// <returns>Lista de funcionalidades asignadas al rol</returns>
        public List<Vw_Funcionalidades> funcionalidadesAsignadasEstadosCity(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.CityEmplazamientosEstados where c.CityEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where (funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        /// <summary>
        /// Obtiene la lista de funcionalidades que aún no tiene asignado el estado que se le pasa como parámetro
        /// </summary>
        /// <param name="EstadoID">Identificador del estado del que se quieren obtener las funcionalidades que no tiene asignado</param>
        /// <returns>Lista de funcionalidades no asignadas al rol</returns>
        /// <remarks>MDS</remarks>
        public List<Vw_Funcionalidades> funcionalidadNoAsignadaEstadosCity(long estadoID)
        {
            List<long?> funcionalidades;
            funcionalidades = (from c in Context.CityEmplazamientosEstados where c.CityEmplazamientoEstadoID == estadoID select c.FuncionalidadID).ToList<long?>();
            return (from c in Context.Vw_Funcionalidades where !(funcionalidades.Contains(c.FuncionalidadID)) select c).OrderBy("Modulo").ToList<Vw_Funcionalidades>();
        }

        #endregion
    }
}
