using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemCityRequest
    {
        public AddressItemCityRequest() : base() { }

        /// <summary>
        /// OBLIGATORIO-Define la operación requerida por el sistema externo. Valores permitidos Create, Modify -- OPERACÓN
        /// </summary>
        public string OPERATION { get; set; }

        /// <summary>
        /// OBLIGATORIO - Identificación asociada al municipio -- CODIGO_DANE_CIUDAD
        /// </summary>
        public string ABBREVIATION { get; set; }

        /// <summary>
        /// OPCIONAL - Código DANE del departamento al que pertenece la ciudad/municipio enviados Es obligatorio cuando el valor del tag “operation” es Create -- CODIGO_DANE_DEPARTAMENTO
        /// </summary>
        public string ABBREVIATION_REGION { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre de la Ciudad Municipio. Es obligatorio cuando el valor del tag “operation” es Create -- NOMBRE_MUNICIPIO
        /// </summary>
        public string CITY_NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Area a la que pertenece el municipio para reportes regulatorios. -- ZONA_CRC
        /// Allowed values / Valores permitidos:
        ///1 | ZONA 1
        ///2 | ZONA 2
        /// </summary>
        public string MASTER_CRC_AREA { get; set; }

        /// <summary>
        /// OPCIONAL - Flag que define si la ciudad/municipio enviado corresponde a la capital del departamento. -- CIUDAD_CAPITAL
        /// </summary>
        public string MASTER_CAPITAL_CODE { get; set; }

        /// <summary>
        /// OPCIONAL - Flag  que define si la ciudad/municipio enviado es considerada una ciudad principal del departamento. -- MCI_CIUDAD_PRINCIPAL
        /// </summary>
        public string MASTER_MCI_MAIN { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre alias o código que se utiliza para agrupar ciudades/municipios muy cercanos a uno principal. -- MCI_CIUDAD_GRUPO
        /// Allowed values / Valores permitidos:
        /// M05001
        /// M11001
        /// M66001
        /// M08001
        /// M68001
        /// M76001
        /// M68276
        /// </summary>
        public string MASTER_MCI_GROUP { get; set; }

        /// <summary>
        /// OPCIONAL - Población estimada por cada año utilizada para reportes regulatorios. -- PROYECCION_POBLACION
        /// </summary>
        public string POPULATION_PROJECTION { get; set; }

        /// <summary>
        /// OPCIONAL - Tipificación de la ciudad/municipio. -- GRUPO_MUNICIPIO
        /// Allowed values / Valores permitidos:
        ///1 | CIUDAD CAPITAL
        ///2 | CIUDAD CAPITAL NIVEL 1
        ///3 | CIUDAD CAPITAL NIVEL 2
        ///4 | CIUDAD CAPITAL NIVEL 3
        ///5 | CIUDAD CAPITAL NIVEL 4
        ///6 | CIUDAD CAPITAL NIVEL 5
        ///8 | CENTRO TURISTICO
        ///10 | CENTRO MINERO - PETROLERO
        /// </summary>
        public string MASTER_CITY_GROUP { get; set; }

        /// <summary>
        /// OPCIONAL - Categorización asignada a la ciudad/municipio para propósitos regulatorios. -- CATEGORIZACION
        /// Allowed values / Valores permitidos:
        ///1
        ///2
        ///3
        ///4
        ///5
        ///6
        ///7
        ///ESP
        /// </summary>
        public string MASTER_CATEGORY_617 { get; set; }

        /// <summary>
        /// OPCIONAL - Ambito al que pertenece el municipio para propósitos regulatorios. -- AMBITO
        /// Allowed values / Valores permitidos:
        ///AMAZONAS
        ///AMAZONAS LETICIA
        ///ANTIOQUIA
        ///ANTIOQUIA MEDELLIN
        ///ARAUCA
        ///ARAUCA ARAUCA
        ///ATLANTICO
        ///ATLANTICO BARRANQUILLA
        ///BOGOTA BOGOTA D.C.
        ///BOLIVAR
        ///BOLIVAR CARTAGENA
        ///BOYACA
        ///BOYACA TUNJA
        ///CALDAS
        ///CALDAS MANIZALES
        ///CAQUETA
        ///CAQUETA FLORENCIA
        ///CASANARE
        ///CASANARE YOPAL
        ///CAUCA
        ///CAUCA POPAYAN
        ///CESAR
        ///CESAR VALLEDUPAR
        ///CHOCO
        ///CHOCO QUIBDO
        ///CORDOBA
        ///CORDOBA MONTERIA
        ///CUNDINAMARCA
        ///GUAINIA
        ///GUAINIA INIRIDA
        ///GUAVIARE
        ///GUAVIARE SAN JOSE DEL GUAVIARE
        /// HUILA
        ///HUILA NEIVA
        ///LA GUAJIRA
        ///LA GUAJIRA RIOHACHA
        ///MAGDALENA
        ///MAGDALENA SANTA MARTA
        ///META
        ///META VILLAVICENCIO
        ///NARIÑO
        /// NARIÑO PASTO
        ///NORTE SANTANDER
        ///NORTE SANTANDER CUCUTA
        ///PUTUMAYO
        ///PUTUMAYO MOCOA
        ///QUINDIO
        ///QUINDIO ARMENIA
        ///RISARALDA
        ///RISARALDA PEREIRA
        ///SAN ANDRES
        ///SAN ANDRES SAN ANDRES
        ///SANTANDER
        ///SANTANDER BUCARAMANGA
        ///SUCRE
        ///SUCRE SINCELEJO
        ///TOLIMA
        ///TOLIMA IBAGUE
        ///VALLE
        ///VALLE CALI
        ///VAUPES
        /// VAUPES MITU
        ///VICHADA
        ///VICHADA PUERTO CARREÑO
        /// </summary>
        public string MASTER_SCOPE { get; set; }

        /// <summary>
        /// OPCIONAL - Porcentaje de cobertura de tecnologia LTE en la ciudad/municipio. -- PORCENTAJE_COBERTURA_LTE
        /// </summary>
        public string LTE_COVERAGE { get; set; }

        /// <summary>
        /// OPCIONAL - Identificar el valor del impuesto aplicable (temas tributarios). -- MERCADO
        /// </summary>
        public string MARKET { get; set; }
    }
}