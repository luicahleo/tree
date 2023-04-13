using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemRegionRequest
    {
        public AddressItemRegionRequest(): base() { }

        /// <summary>
        /// OBLIGATORIO-Define la operación requerida por el sistema externo. Valores permitidos Create, Modify -- OPERACION
        /// </summary>
        public string OPERATION { get; set; }

        /// <summary>
        /// OBLIGATORIO - Identificación asociada al departamento -- CODIGO DANE DEPARTAMENTO
        /// </summary>
        public string ABBREVIATION { get; set; }

        /// <summary>
        /// OPCIONAL - Es obligatorio cuando el valor del tag “operation” es Create. -- Es el pais. -- PAIS
        /// </summary>
        public string COUNTRY { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre del departamento de acuerdo a la división político-administrativa. Es obligatorio cuando el valor del tag “operation” es Create -- NOMBRE DEPARTAMENTO
        /// </summary>
        public string REGION_NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Clasificación asignada por departamento de acuerdo a su peso comercial para la operadora. -- REGIONAL_COMERCIAL
        /// Allowed values: / Valores permitidos:
        ///BOGOTA
        ///CARIBE
        ///NOROCCIDENTE
        ///NORORIENTE
        ///SUROCCIDENTE
        ///SURORIENTE
        /// </summary>
        public string MASTER_COMMERCIAL_CLASS { get; set; }

        /// <summary>
        /// OPCIONAL - Región a la que pertenece el departamento de acuerdo a la clasificación reportada al Gobierno. -- ZONA_MINISTERIO
        /// Allowed values: / Valores permitidos:
        ///ZONA 1
        ///ZONA 2
        /// </summary>
        public string MASTER_MINISTRY_AREA { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre y Apellidos del responsable del departamento en el área de transmisión. --REGIONAL_TX
        /// </summary>
        public string MASTER_REGIONAL_TX { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre del responsable asignado por el área de ingeniería. -- RESPONSABLE_TX
        /// </summary>
        public string MASTER_RESPONSIBLE_TX { get; set; }

    }
}