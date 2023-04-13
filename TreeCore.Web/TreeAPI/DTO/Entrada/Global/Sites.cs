using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeAPI.DTO.Interfaces;
using TreeCore.Data;


namespace TreeAPI.DTO.Entrada.Global
{
    public class Sites : TreeObject
    {
        private long emplazamientoID;
        private string codigo;
        private string nombreSitio;
        private string regionPais;
        private Emplazamientos emplazamiento;

        #region CONSTRUCTOR 

        public Sites()
        : base()
        {
        }

        public Sites(long EmplazamientoID, string Codigo, string NombreSitio,
            string Region, string RegionPais, string CodigoPostal, string Municipio, string Provincia, string Neighborhood, string Direccion,
            float Longitud, float Latitud, string Pais, string Operador, 
            string Moneda, string EmplazamientoTipo, string CategoriaEmplazamiento,
            string TipoEdificacion, string EmplazamientoTamano, string EmplazamientoTipoEstructura,
            string EstadoGlobal, DateTime? FechaActivacion, DateTime? FechaDesactivacion)
        : base()
        {
            this.SiteID = EmplazamientoID;
            this.sCode = Codigo;
            this.sName = NombreSitio;
            this.sRegion = Region;
            this.sCountryRegion = RegionPais;
            this.sPostalCode = CodigoPostal;
            this.sMunicipality = Municipio;
            this.sProvince = Provincia;
            this.sNeighborhood = Neighborhood;
            this.sAddress = Direccion;
            this.dLongitude = Longitud;
            this.dLatitude = Latitud;
            this.sCountry = Pais;
            this.sCustomer = Operador;
            this.sCurrency = Moneda;
            this.sSitesType = EmplazamientoTipo;
            this.sCategory = CategoriaEmplazamiento;
            this.sBuildingType = TipoEdificacion;
            this.sSize = EmplazamientoTamano;
            this.sStructureType = EmplazamientoTipoEstructura;
            this.sGlobalState = EstadoGlobal;
            this.ActivationDateDate = FechaActivacion;
            this.DeactivationDateDate = FechaDesactivacion;
        }

        #endregion

        #region PARAMETROS

        #region SITE
        [JsonIgnore]
        private long? SiteID { get; set; }

        [Required]
        public string sCode { get; set; }

        [Required]
        public string sName { get; set; }

        [Required]
        public string sCustomer { get; set; }

        [Required]
        public string sGlobalState { get; set; }

        [Required]
        public string sCurrency { get; set; }

        [Required]
        public string sCategory { get; set; }

        [Required]
        public string sSitesType { get; set; }

        [Required]
        public string sStructureType { get; set; }

        [Required]
        public string sBuildingType { get; set; }

        [Required]
        public string sSize { get; set; }

        [JsonIgnore]
        public DateTime? ActivationDateDate { get; set; }

        [JsonIgnore]
        public string sActivationDate { get; set; }
        [Required]
        public string dActivationDate
        {
            get
            {
                return this.ActivationDateDate.HasValue ? this.ActivationDateDate.Value.ToString(Comun.FORMATO_FECHA) : null;
            }
            set
            {
                this.sActivationDate = value;
                if (!string.IsNullOrEmpty(value) && DateIsValid(value))
                {
                    this.ActivationDateDate = DateTime.Parse(value);
                }
            }
        }

        [JsonIgnore]
        public DateTime? DeactivationDateDate { get; set; }

        public string dDeactivationDate 
        { 
            get 
            {
                return this.DeactivationDateDate.HasValue ? this.DeactivationDateDate.Value.ToString(Comun.FORMATO_FECHA) : null;
            }
            set 
            { 
                if (!string.IsNullOrEmpty(value) && DateIsValid(value))
                {
                    this.DeactivationDateDate = DateTime.Parse(value);
                }
                else
                {
                    this.DeactivationDateDate = DateTime.MinValue;
                }
            } 
        }

        public List<SiteAttributes> Attributes { get; set; }
        #endregion

        #region LOCALIZATION

        [Required]
        public string sRegion { get; set; }
        [Required]
        public string sCountry { get; set; }

        [Required]
        public string sCountryRegion { get; set; }

        [Required]
        public string sProvince { get; set; }

        [Required]
        public string sMunicipality { get; set; }

        [Required]
        public string sAddress { get; set; }

        public string sNeighborhood { get; set; }

        public string sPostalCode { get; set; }

        [Required]
        public double? dLatitude { get; set; }

        [Required]
        public double? dLongitude { get; set; }
        #endregion

        #region COMENTADOS
        /*
        [XmlElementAttribute(IsNullable = true)]
        public string CustomerOwnerStructure { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string OwnerFloor { get; set; }

        public float? Altitude { get; set; }

        [XmlIgnore]
        public int? NumDependentSites { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ClaveCatastral { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ComentariosGenerales { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ComentarioEdificio { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ComentarioEquipos { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string SituacionIngenieria { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string UsoSitio { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string PotencialSitio { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string EmplazamientoRiesgo { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoTorrero { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoTelco { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoNC { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoSAP { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoArchibus { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoOYM { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoDirRed { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string CodigoElectrico { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string IdentificadorUnico { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string EmplazamientoOrigen { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string EmplazamientoPadre { get; set; }

        
        public bool? Inbound { get; set; }

        [XmlIgnore]
        public bool? Outbound { get; set; }

        [XmlIgnore]
        public bool? Compartido { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Adquirido
        {
            get
            {
                return this.AdquiridoBool.HasValue ?
                    this.AdquiridoBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.AdquiridoBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? AdquiridoBool { get; set; }


        [XmlElementAttribute(IsNullable = true)]
        public string Legalizado
        {
            get
            {
                return this.LegalizadoBool.HasValue ?
                    this.LegalizadoBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.LegalizadoBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? LegalizadoBool { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ImposibleLicenciar
        {
            get
            {
                return this.ImposibleLicenciarBool.HasValue ?
                    this.ImposibleLicenciarBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.ImposibleLicenciarBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? ImposibleLicenciarBool { get; set; }



        [XmlElementAttribute(IsNullable = true)]
        public string SuministroPropio
        {
            get
            {
                return this.SuministroPropioBool.HasValue ?
                    this.SuministroPropioBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.SuministroPropioBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? SuministroPropioBool { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ImpactoVisual
        {
            get
            {
                return this.ImpactoVisualBool.HasValue ?
                    this.ImpactoVisualBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.ImpactoVisualBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? ImpactoVisualBool { get; set; }


        [XmlElementAttribute(IsNullable = true)]
        public string FragilidadVisual
        {
            get
            {
                return this.FragilidadVisualBool.HasValue ?
                    this.FragilidadVisualBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.FragilidadVisualBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? FragilidadVisualBool { get; set; }



        [XmlElementAttribute(IsNullable = true)]
        public string Completo
        {
            get
            {
                return this.CompletoBool.HasValue ?
                    this.CompletoBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.CompletoBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? CompletoBool { get; set; }



        [XmlElementAttribute(IsNullable = true)]
        public string Equipo2G
        {
            get
            {
                return this.Equipo2GBool.HasValue ?
                    this.Equipo2GBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.Equipo2GBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? Equipo2GBool { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Equipo3G
        {
            get
            {
                return this.Equipo3GBool.HasValue ?
                    this.Equipo3GBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.Equipo3GBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? Equipo3GBool { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Equipo4G
        {
            get
            {
                return this.Equipo4GBool.HasValue ?
                    this.Equipo4GBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.Equipo4GBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? Equipo4GBool { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Equipo5G
        {
            get
            {
                return this.Equipo5GBool.HasValue ?
                    this.Equipo5GBool.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.Equipo5GBool = bool.Parse(value);
            }
        }

        [XmlIgnore]
        public bool? Equipo5GBool { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public bool NodoTX { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public bool Peligroso { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string EmpresaCompradora { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string VentaSitioATercero { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Criticidad { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string SuperficieSitio
        {
            get
            {
                return this.SuperficieSitioFloat.HasValue ?
                    this.SuperficieSitioFloat.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.SuperficieSitioFloat = float.Parse(value);
            }
        }

        [XmlIgnore]
        public float? SuperficieSitioFloat { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string SuperficieVertical
        {
            get
            {
                return this.SuperficieVerticalFloat.HasValue ?
                    this.SuperficieVerticalFloat.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.SuperficieVerticalFloat = float.Parse(value);
            }
        }

        [XmlIgnore]
        public float? SuperficieVerticalFloat { get; set; }
        */
        #endregion

        #endregion

    }
}