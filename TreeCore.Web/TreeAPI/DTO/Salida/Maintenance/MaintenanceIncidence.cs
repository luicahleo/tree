using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TreeAPI.DTO.Interfaces;

namespace TreeAPI.DTO.Salida.Maintenance
{
    public class MaintenanceIncidence : TreeObject
    {

        #region CONSTRUCTOR

        public MaintenanceIncidence()
        : base()
        {

        }

        #endregion

        #region PARAMETROS
        // Local attributes
        
        public long? lIncidenceID;

        [JsonIgnore]
        public string lIncidenceIDs
        {
            get
            {
                return (lIncidenceID.HasValue) ? lIncidenceID.ToString() : string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.lIncidenceID = long.Parse(value);
                }
            }
        }


        [JsonIgnore]
        public string lIdentifierCorrectiveMaintenance { get; set; }

        [Required]
        public string sMaintenanceIncidence { get; set; }
        
        [Required]
        public string dDate
        {
            get
            {
                return this.dDateDT.ToString(Comun.FORMATO_FECHA);
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && this.DateIsValid(value))
                {
                    this.dDateDT = DateTime.Parse(value);
                }
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime dDateDT { get; set; }
        public string sMaintenanceDescriptionIncidence { get; set; }

        
        public string sSiteCode { get; set; }


        [JsonIgnore]
        public string sEmail { get; set; }

        public bool bEmergency { get; set; }

        public bool bInProcess { get; set; }

        [Required]
        public string sCategory { get; set; }

        public string sElementInventory { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public float? dLatitudeFloat { get; set; }

        [JsonIgnore]
        public string dLatitude
        {
            get
            {
                return this.dLatitudeFloat.HasValue ?
                    this.dLatitudeFloat.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.dLatitudeFloat = float.Parse(value);
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public float? dLongitudeFloat { get; set; }
        
        [JsonIgnore]
        public string dLongitude
        {
            get
            {
                return this.dLongitudeFloat.HasValue ?
                    this.dLongitudeFloat.Value.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.dLongitudeFloat = float.Parse(value);
            }
        }

        #region PARAMETROS IN_PROCESS
        public string sProject { get; set; }

        public string sAgency { get; set; }

        public string sSeverity { get; set; }

        public string sWorkflowType { get; set; }

        public string sConflictLevel { get; set; }
        #endregion

        #endregion

        #region OBJETO BASE

        #region PRIVATE

        private void SetIncidenceID(string lValor)
        {
            lIncidenceIDs = lValor;
        }

        private string GetIncidenceID()
        {
            return lIncidenceIDs;
        }
        #endregion

        #endregion

    }
}