using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class LocationRequest
    {
        public LocationRequest() : base() { }

        /// <summary>
        /// OBLIGATORIO-Define la operación requerida por el sistema externo. Valores permitidos Create, Modify -- OPERACION
        /// </summary>
        public string OPERATION { get; set; }

        /// <summary>
        /// OPCIONAL - Identificador del Sitio/emplazamiento. -- COD_EMPLAZAMIENTO
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string LEGACY_ID { get; set; }

        /// <summary>
        /// OPCIONAL - Nombre asignado al emplazamiento --NOMBRE_EMPLAZAMIENTO
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Dirección física del sitio. -- DIRECCION
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string MASTER_ADDRESS { get; set; }

        /// <summary>
        /// OPCIONAL - Disponibilidad del sitio. -- DISPONIBILIDAD_ACCESO_SITIO
        /// </summary>
        public string ACCESS_INSTRUCTIONS { get; set; }

        /// <summary>
        /// OPCIONAL - Allowed values:
        ///NOT READY
        ///DEVELOPMENT
        ///DECOMMISIONED
        ///OPERATING
        ///It is mandatory when tag operation value has “Create” value.
        ///Locations with DECOMMISIONED value WILL NOT BE created.
        ///Es obligatorio cuando el valor del tag “operation” es Create.
        ///Ubicaciones con estado DECOMMISIONED no serán creadas. -- ESTADO
        /// </summary>
        public string OPERATIONAL_STATUS { get; set; }

        /// <summary>
        /// OPCIONAL - Altura sobre el nivel del mar. -- ALTURA_NIVEL_MAR
        /// </summary>
        public string ELEVATION { get; set; }

        /// <summary>
        /// OPCIONAL - Año PEP del emplazamiento. -- ANNO_PEP
        /// Es obligatorio cuando el valor del tag “operation” es Create
        /// </summary>
        public string PEP_CODE_YEAR { get; set; }

        /// <summary>
        /// OPCIONAL - Código PEP sociedad del emplazamiento. -- COD_PEP_SOCIEDAD
        /// Es obligatorio cuando el valor del tag “operation” es Create
        /// </summary>
        public string PEP_CODE_SOCIETY { get; set; }

        /// <summary>
        /// OPCIONAL - Código PEP del emplazamiento. -- COD_PEP_N1
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string PEP_CODE_N1 { get; set; }


        /// <summary>
        /// OPCIONAL - Start date of activities at the site. -- FECHA_INI_OPERACION
        /// </summary>
        public string INSTALL_DATE { get; set; }

        /// <summary>
        /// OPCIONAL - Date of end of activities on the site. -- FECHA_FIN_OPERACION
        /// </summary>
        public string DIPOSAL_DATE { get; set; }


        /// <summary>
        /// OPCIONAL - Radiobase identifier code. -- IDENTIFICADOR
        /// </summary>
        public string DISTINGUISH_NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Environmental Regulatory Follow-up value -- IMPACTO_VISUAL
        /// </summary>
        public string MASTER_VISUAL_IMPACT { get; set; }

        /// <summary>
        /// OPCIONAL - Cordenada geographic latitude of the location. -- LATITUD
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string LATITUDE { get; set; }

        /// <summary>
        /// OPCIONAL - Geographic type Cordenada Length of the site -- LONGITUD
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string LONGITUDE { get; set; }

        /// <summary>
        /// OPCIONAL - Is the letter established as "P", Type of Project(CapEx). -- LETRA_PEP
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string PEP_CODE_LETTER { get; set; }

        /// <summary>
        /// OPCIONAL - Number of the civil aero site license -- LICENCIA_AEROCIVIL
        /// </summary>
        public string DETAIL_LICENCES_AEROCIVIL { get; set; }

        /// <summary>
        /// OPCIONAL - The license number of construction site -- LICENCIA_CONSTRUCCION
        /// </summary>
        public string DETAIL_LICENCES_CONSTRUCTION { get; set; }

        /// <summary>
        /// OPCIONAL - Contact number of the person or area responsible for the security of access -- NUMERO_CONTACTO_SEGURIDAD
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_1 { get; set; }

        /// <summary>
        /// OPCIONAL - Regional operacoines assigned by field to the site eg: O_CAMBU, OR_CAMBA -- REGIONAL_OPE
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_2 { get; set; }

        /// <summary>
        /// OPCIONAL - Regional rf assigned by rf for the site -- REGIONAL_RF
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_3 { get; set; }

        /// <summary>
        /// OPCIONAL - Name Regional Chief RF -- RESPONSABLE_RF
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_4 { get; set; }

        /// <summary>
        /// OPCIONAL - Name of the responsible construction assigned by operations buildings -- RESPONSABLE_CONSTRUCCIONES
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_5 { get; set; }

        /// <summary>
        /// OPCIONAL - Name of the responsible oper energy assigned by operations field -- RESPONSABLE_OPERACIONES_EX
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_6 { get; set; }

        /// <summary>
        /// OPCIONAL - Name of the responsible oper field assigned by operations field -- RESPONSABLE_OPE_CAMPO
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_7 { get; set; }

        /// <summary>
        /// OPCIONAL - Telephone Contact Number of Access of security. -- CONTACTO_SEGURIDAD
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_8 { get; set; }

        /// <summary>
        /// OPCIONAL - With the person or company that is or generates contract placement -- PROPIETARIO_EMPLAZAMIENTO
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME_9 { get; set; }

        /// <summary>
        /// OPCIONAL - Hierarchy that is assigned to the site or site depndendo of the teams in this: (Type 8 _ K-Type 3 _) -- TIPO_CONTRATO
        /// </summary>
        public string DETAIL_TO_CONTRACT_RELATION_MASTERCONTRACT_CONTRACTTYPE { get; set; }

        /// <summary>
        /// OPCIONAL - Description of the type of technology location: (Cell-REPEATER MW-SWICHT) -- TIPO_EMPLAZAMIENTO
        /// </summary>
        public string MASTER_LOCATION_DETAILED_TYPE_NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Type of description of the location of the site (Building-LOT-booth-STREETSITE) -- TIPO_INMUEBLE
        /// </summary>
        public string MASTER_SITE_TYPE { get; set; }

        /// <summary>
        /// OPCIONAL - Type of operation up and running in operating on site (Fixed/Mobile-FIXED-MOBILE-probe) -- TIPO_OPERACION
        /// </summary>
        public string MASTER_SITE_TYPE_MIGRATION_IDENTIFIER { get; set; }

        /// <summary>
        /// OPCIONAL - Description of the type of technology location: (Cell-REPEATER MW-SWICHT) -- TIPO_TX
        /// </summary>
        public string MASTER_TX_TYPE { get; set; }

        /// <summary>
        /// OPCIONAL - Identification of the type of site if it is (Urban/RURAL) -- TIPO_UBICACION
        /// Es obligatorio cuando el valor del tag “operation” es Create.
        /// </summary>
        public string MASTER_LOCATION_DETAILED_TYPE_MIGRATION_IDENTIFIER { get; set; }

        /// <summary>
        /// OPCIONAL - Identifying the area of construction at the site -- ZONA_CONSTRUCCIONES
        /// </summary>
        public string MASTER_CONSTRUCTION_AREA_TYPE { get; set; }

        /// <summary>
        /// OPCIONAL - W arm or company that provides on-site service -- ALIADO_OPERACIONES_CAMPO
        /// </summary>
        public string DETAIL_INTEREST_MASTER_CONTACT_UNIT_NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Maximum time established for the attention of events on site or location -- TIEMPO_REQUERIDO_ATENCION
        /// </summary>
        public string REQUIRED_ATTENTION_TIME { get; set; }

        /// <summary>
        /// OPCIONAL - Value assigned to a site that depends on the groupof operations field and ally that opera -- ZONA_OPERACIONES_CAMPO
        /// </summary>
        public string MASTER_OPERATION_AREAS { get; set; }

        /// <summary>
        /// OPCIONAL - Name of the neighborhood in which is located the site code, for the sites where applicable -- BARRIO
        /// </summary>
        public string MASTER_NEIHGBORHOOD { get; set; }

        /// <summary>
        /// OPCIONAL - Field used for identifying the level of fragility of the sites for environmental issues -- FRAGILIDAD_VISUAL
        /// </summary>
        public string MASTER_VISUAL_IMPACT_2 { get; set; }

        /// <summary>
        /// OPCIONAL - Field used for identifying the level of debris located on site -- DISPOSICION_ESCOMBRO
        /// </summary>
        public string DEBRIS_AMOUNT { get; set; }

        /// <summary>
        /// OPCIONAL - Relational attribute specific usage between this layer and the department
        /// Es obligatorio cuando el valor del tag “operation” es Create. -- DEPARTAMENTO_CODIGO_DANE
        /// </summary>
        public string MASTER_ADDRESS_2 { get; set; }

        /// <summary>
        /// OPCIONAL - Relational attribute specific usage between this layer and the municipality
        /// Es obligatorio cuando el valor del tag “operation” es Create. -- MUNICIPIO_CODIGO_DANE
        /// </summary>
        public string MASTER_CITY_AREA_NAME { get; set; }

        /// <summary>
        /// OPCIONAL - Relational attribute specific usage between this layer and the Locality (City center - populated)
        /// Es obligatorio cuando el valor del tag “operation” es Create. -- LOCALIDAD_CODIGO_DANE_POBLADO
        /// </summary>
        public string MASTER_ADDRESS_3 { get; set; }

        /// <summary>
        /// OPCIONAL - Specific attribute of use Relational Database for towns with administrative division of commune or town -- LOCALIDAD_CODIGO_DANE
        /// </summary>
        public string MASTER_ADDRESS_4 { get; set; }

        /// <summary>
        /// OPCIONAL - Valor asigando a un emplazamiento según el entorno en el que se encuentre ubicado -- PRIORITY
        /// </summary>
        public string ENVIRONMENT_PRIORITY { get; set; }

        /// <summary>
        /// OPCIONAL - Identificar cuantos sitios propios y/o arrendados tenemos en laed -- COSITE
        /// </summary>
        public string COSITE { get; set; }
    }
}