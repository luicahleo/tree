using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemDistrictRequest
    {
        public AddressItemDistrictRequest() : base() { }

        /// <summary>
        /// OBLIGATORIO-Define la operación requerida por el sistema externo. Valores permitidos Create, Modify -- OPERACION
        /// </summary>
        public string OPERATION { get; set; }

        /// <summary>
        /// OPCIONAL - Código DANE del Municipio al que pertenece la localidad/comuna enviados, es mandatorio si el sistema externo requiere una localidad. -- COD_DANE_CIUDAD_PADRE
        /// Es obligatorio cuando el valor del tag “operation” es Create..
        /// </summary>
        public string ABBREVIATION_CITY { get; set; }

        /// <summary>
        /// OPCIONAL - Código DANE de la localidad/comuna. -- COD_DANE_LOCALIDAD
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string ABBREVIATION_DISTRICT { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre de la localidad/comuna. -- NOMBRE_LOCALIDAD
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Categoria de la localidad/comuna de acuerdo al DANE. -- CATEGORIA_DIVIPOLA
        /// Allowed values / Valores permitidos:
        ///CABECERA MUNICIPAL | CM
        ///CENTRO POBLADO NO CATEGORIZADO | CP
        ///CENTRO POBLADO TIPO CASERIO | CAS
        ///CENTRO POBLADO TIPO CORREGIMIENTO | C
        ///CENTRO POBLADO TIPO INSPECCION POLICIA | IP
        ///CENTRO POBLADO TIPO INSPECCION POLICIA DPTAL | IPD
        ///CENTRO POBLADO TIPO INSPECCION POLICIA MPAL | IPM
        /// </summary>
        public string MASTER_DIVPOL_CATEGORY { get; set; }

        /// <summary>
        /// OPCIONAL - Punto central en latitud de la localidad/comuna. -- LATITUD
        /// </summary>
        public string LATITUDE { get; set; }

        /// <summary>
        /// OPCIONAL - Punto central en longitud de la localidad/comuna. --LONGITUD
        /// </summary>
        public string LONGITUDE { get; set; }
    }
}