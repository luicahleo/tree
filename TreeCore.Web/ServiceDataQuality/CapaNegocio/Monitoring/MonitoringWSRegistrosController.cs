using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using System.Linq;
using TreeCore;

namespace CapaNegocio
{
    public class MonitoringWSRegistrosController : GeneralBaseController<MonitoringWSRegistros, TreeCoreContext>
    {
        public MonitoringWSRegistrosController()
            : base()
        {

        }

        #region AGREGAR

        public bool AgregarRegistro(string proyectoTipo, long? userID, string InParametro, string outParametro, string sServicio, string sNombreMetodo, string sComentarios, long? clienteID, bool bExito, bool bPropio, long? AlquilerID, long? EmplazamientoID)
        {
            // Local variables
            bool bResultado = false;
            MonitoringWSRegistros ws = null;
            ProyectosTiposController cTipo = new ProyectosTiposController();
            ProyectosTipos tipo = null;
            ToolServicios tool = null;
            ToolServiciosController cTool = new ToolServiciosController();

            // Performs the operation
            try
            {
                // Prepares the object
                ws = new MonitoringWSRegistros();

                // Takes the project type
                tipo = cTipo.GetProyectosTiposByNombre(proyectoTipo);
                ws.ProyectoTipoID = tipo.ProyectoTipoID;
                ws.UsuarioID = userID;
                ws.FechaCreacion = DateTime.Now;
                ws.ParametrosEntrada = InParametro;
                ws.ParametrosSalida = outParametro;
                // Takes the service information
                tool = cTool.GetServicioByName(sServicio);
                if (tool != null)
                {
                    ws.ToolServicioID = tool.ServicioID;
                }
                ws.NombreMetodo = sNombreMetodo;
                ws.Comentarios = sComentarios;
                ws.ClienteID = clienteID;
                ws.Exito = bExito;
                ws.EsServicioPropio = bPropio;
                ws.AlquilerID = AlquilerID;
                ws.EmplazamientoID = EmplazamientoID;


                if (AddItem(ws) != null)
                {
                    bResultado = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            // Returns the result
            return bResultado;

        }

        #endregion


    }
}