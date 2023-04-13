using System;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class LogController : GeneralBaseController<Log, TreeCoreContext>
    {
        public LogController()
            : base()
        {

        }

        /// <summary>
        /// Se encarga de almacenar en el log de base de datos un mensaje con la fecha actual.
        /// </summary>
        /// <param name="ip">Dirección IP a almacenar en el log.</param>
        /// <param name="usuarioID">Usuario para el que se almacena el mensaje.</param>
        /// <param name="mensaje">Mensaje a almacenar</param>
        /// <returns>True en caso de que se haga correctamente y false en caso contrario.</returns>
        /// <remarks>Author: PCB</remarks>
        public bool EscribeLog(string ip, long usuarioID, string mensaje)
        {
            Log item = new Log();
            item.IP = ip;
            item.UsuarioID = usuarioID;
            item.Mensaje = mensaje;
            item.FechaHora = DateTime.Now;
            if (this.AddItem(item) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
