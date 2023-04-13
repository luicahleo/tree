using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Clases
{
    public class Valores
    {
        
        public int ValorID { get; set; }
        public string Valor { get; set; }
    }

    public class ItemInvInicio
    {
        public long InventarioElementoID { get; set; }
        public string Nombre { get; set; }
        public string NumeroInventario { get; set; }
        public long InventarioCategoriaID { get; set; }
        public string InventarioCategoria { get; set; }
        //EmplazamientoID
        //Codigo
        //NombreSitio
        //Creador
        //NombreCompleto
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public long InventarioElementoAtributoEstado { get; set; }
        public string NombreAtributoEstado { get; set; }
        public string CodigoAtributoEstado { get; set; }
        public long OperadorID { get; set; }
        public string Operador { get; set; }
        //jsonPlantillas
        //Plantilla
        public long ocurrencies { get; set; }
        public DateTime UltimaModificacionFecha { get; set; }

    }
}