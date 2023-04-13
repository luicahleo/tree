using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemCommuneRequest
    {
        public AddressItemCommuneRequest() : base() { }

        /// <summary>
        /// OBLIGATORIO-Define la operación requerida por el sistema externo. Valores permitidos Create, Modify -- OPERACION
        /// </summary>
        public string OPERATION { get; set; }

        /// <summary>
        /// OPCIONAL - Código DANE de la localidad/comuna. -- COD_DANE_LOCALIDAD
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string ABBREVIATION_DISTRICT { get; set; }

        /// <summary>
        /// OPCIONAL - Código DANE de la comuna -- CODIGO_DANE_COMUNA
        /// Es obligatorio cuando el valor del tag “operation” es Create..
        /// </summary>
        public string ABBREVIATION_COMMUNE { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre de la localidad/comuna. -- NOMBRE_COMUNA
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string NAME { get; set; }
    }
}